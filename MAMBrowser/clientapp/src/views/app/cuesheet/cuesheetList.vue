<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="이전 큐시트 조회" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      :isDisplayBtnArea="true"
      @changeRowPerpage="onChangeRowPerpage"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 시작일 ~ 종료일 -->
        <common-start-end-date-picker
          startDateLabel="방송 시작일"
          endDateLabel="방송 종료일"
          :startDate.sync="searchItems.start_dt"
          :endDate.sync="searchItems.end_dt"
          :maxPeriodMonth="6"
          :required="false"
          :isCurrentDate="false"
        />
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            style="width: 100px"
            v-model="searchItems.media"
            :options="mediasOption"
            @change="eventClick($event)"
          />
        </b-form-group>
        <!-- 프로그램명 -->
        <b-form-group label="프로그램명" class="has-float-label">
          <b-form-select
            style="width: 400px"
            v-model="searchItems.productid"
            :options="programList"
          />
        </b-form-group>
        <!-- 태그 -->
        <b-form-group label="태그" class="has-float-label">
          <common-input-text v-model="searchItems.tag" />
        </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >검색</b-button
          >
        </b-form-group>
      </template>
      <!-- 테이블 페이지 -->
      <template slot="form-table-page-area">
        {{ getPageInfo() }}
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <common-data-table-scroll-paging
          ref="scrollPaging"
          tableHeight="525px"
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          :totalCount="responseData.totalRowCount"
          is-actions-slot
          :isTagSlot="true"
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <common-actions :rowData="props.props.rowData"> </common-actions>
          </template>
          <template slot="tag" scope="props">
            <common-tag
              :rowData="props.props.rowData"
              @tagItemFromCommonTag="OnClickCommonTagItem"
            >
            </common-tag>
          </template>
        </common-data-table-scroll-paging>
      </template>
    </common-form>
  </div>
</template>

<script>
import CommonTag from "../../../components/DataTable/CommonTag.vue";
import { mapActions, mapGetters } from "vuex";
import "moment/locale/ko";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
const moment = require("moment");
const date = new Date();

function get_date_str(date) {
  var sYear = date.getFullYear();
  var sMonth = date.getMonth() + 1;
  var sDate = date.getDate();

  sMonth = sMonth > 9 ? sMonth : "0" + sMonth;
  sDate = sDate > 9 ? sDate : "0" + sDate;

  return sYear + "" + sMonth + "" + sDate;
}

var endDay = get_date_str(date);

date.setDate(date.getDate() - 7);
var startDay = get_date_str(date);

export default {
  props: {
    tagItem: {
      type: String,
      default: "",
    },
  },
  mixins: [MixinBasicPage],
  data() {
    return {
      date,
      programList: [{ value: "", text: "매체를 선택하세요" }],
      searchItems: {
        start_dt: startDay, // 시작일
        end_dt: endDay, // 종료일
        media: "", // 매체
        productid: "", // 프로그램명
        tag: "", //태그
        rowPerPage: 30,
        selectPage: 1,
      },
      fields: [
        {
          name: "media",
          title: "매체",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "7%",
          callback: (value) => {
            switch (value) {
              case "A":
                return "AM";
              case "F":
                return "FM";
              case "D":
                return "DMB";
              case "C":
                return "공통";
              case "Z":
                return "기타";
              default:
                break;
            }
          },
        },
        {
          name: "brdtime",
          title: "방송완료일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "15%",
          callback: (value) => {
            return value === null
              ? ""
              : moment(value, "YYYYMMDD").format("YYYY-MM-DD (ddd)");
          },
        },
        {
          name: "brdtime",
          title: "방송시간",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "7%",
          callback: (value) => {
            return value === null
              ? ""
              : moment(value, "YYYY-MM-DD'T'HH:mm:ss").format("HH:mm");
          },
        },
        {
          name: "title",
          title: "프로그램명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
        },
        {
          name: "__slot:tag",
          title: "태그",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          // width: "18%",
        },
        {
          name: "__slot:actions",
          title: "작업",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
        },
      ],
    };
  },
  components: { CommonTag },
  computed: {
    ...mapGetters("cueList", ["archiveCuesheetListArr"]),
    ...mapGetters("cueList", ["userProOption"]),
    ...mapGetters("cueList", ["mediasOption"]),
    ...mapGetters("cueList", ["userProList"]),
  },
  mounted() {
    if (this.tagItem) {
      this.searchItems.tag = this.tagItem;
    }
    this.getData();
    this.getProductName();
  },
  methods: {
    ...mapActions("cueList", ["getarchiveCuesheetListArr"]),
    ...mapActions("cueList", ["getMediasOption"]),
    ...mapActions("cueList", ["getuserProOption"]),

    async getData() {
      this.searchItems.rowPerPage = Number(this.searchItems.rowPerPage);
      this.isTableLoading = this.isScrollLodaing ? false : true;
      if (
        this.$fn.checkGreaterStartDate(
          this.searchItems.start_dt,
          this.searchItems.end_dt
        )
      ) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        this.hasErrorClass = true;
        return;
      }
      if (this.searchItems.productid == "") {
        var pram = { person: null, gropId: null };
        await this.getMediasOption(pram);
        this.searchItems.productid = this.userProList;
      }
      if (this.searchItems.productid == undefined) {
        this.searchItems.productid = this.userProList;
      }
      var params = {
        start_dt: this.searchItems.start_dt,
        end_dt: this.searchItems.end_dt,
        products: this.searchItems.productid,
        tag: this.searchItems.tag,
        row_per_page: this.searchItems.rowPerPage,
        select_page: this.searchItems.selectPage,
      };
      var arrListResult = await this.getarchiveCuesheetListArr(params);
      this.setResponseData(arrListResult);
      this.addScrollClass();
      this.isTableLoading = false;
      this.isScrollLodaing = false;
    },
    //매체 선택시 프로그램 목록 가져오기
    eventClick(e) {
      this.getProductName(e);
    },
    async getProductName(media) {
      var pram = { person: null, gropId: null, media: media };
      var proOption = await this.getuserProOption(pram);
      this.programList = this.userProOption;
      this.searchItems.productid = this.userProList;
    },
    OnClickCommonTagItem(tag) {
      this.searchItems.tag = tag;
      this.getData();
    },
  },
};
</script>