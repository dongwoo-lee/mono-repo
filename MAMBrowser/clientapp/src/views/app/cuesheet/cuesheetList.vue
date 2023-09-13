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
      <template slot="form-search-area">
        <span class="row" v-if="status == 'not_accepted'">
          <!-- 시작일 ~ 종료일 -->
          <common-start-end-date-picker
            startDateLabel="방송 시작일"
            endDateLabel="방송 종료일"
            :startDate.sync="searchItems.start_dt"
            :endDate.sync="searchItems.end_dt"
            :maxPeriodMonth="6"
            :required="false"
            :isCurrentDate="false"
            @SEDateEvent="onSearch"
            @SDateError="SDateErrorLog"
          />
          <!-- 매체 -->
          <b-form-group label="매체" class="has-float-label">
            <b-form-select
              style="width: 100px"
              v-model="searchItems.media"
              :options="mediaOptions"
            />
          </b-form-group>
          <!-- 프로그램명 -->
          <b-form-group label="프로그램명" class="has-float-label">
            <common-input-text v-model="searchItems.title" />
          </b-form-group>
        </span>
        <b-form-checkbox
          class="mr-3 ml-3 custom-checkbox-group-non-align"
          v-model="status"
          name="checkbox-1"
          value="accepted"
          unchecked-value="not_accepted"
        >
          태그로 검색
        </b-form-checkbox>
        <span class="row" v-if="status == 'accepted'">
          <b-form-group label="태그" class="has-float-label mr-3">
            <common-input-text v-model="searchItems.tag" />
          </b-form-group>
        </span>
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
      status: "not_accepted",
      mediaOptions: [],
      searchItems: {
        start_dt: startDay, // 시작일
        end_dt: endDay, // 종료일
        media: "", // 매체
        title: "",
        tag: "", //태그
        rowPerPage: 100,
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
  },
  async mounted() {
    const allMedia = [{ media: "A" }, { media: "F" }, { media: "D" }];
    this.mediaOptions = await this.SetMediaOption(allMedia);
    if (this.tagItem) {
      this.status = "accepted";
      this.searchItems.tag = this.tagItem;
    }
    this.getData();
  },
  methods: {
    ...mapActions("cueList", ["getarchiveCuesheetListArr"]),
    ...mapActions("cueList", ["SetMediaOption"]),
    ...mapActions("cueList", ["GetDateString"]),

    async getData() {
      this.searchItems.rowPerPage = Number(this.searchItems.rowPerPage);
      this.isTableLoading = this.isScrollLodaing ? false : true;
      const params = {};
      if (this.status === "accepted") {
        if (this.searchItems.tag === "") {
          this.$fn.notify("error", {
            message: "태그를 입력하세요.",
          });
          this.isTableLoading = false;
          this.isScrollLodaing = false;
          return;
        }
        params.start_dt = "19990101";
        params.end_dt = endDay;
        params.tag = this.searchItems.tag;
        params.row_per_page = this.searchItems.rowPerPage;
        params.select_page = this.searchItems.selectPage;
      } else {
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
          this.isTableLoading = false;
          this.isScrollLodaing = false;
          return;
        }
        params.start_dt = this.searchItems.start_dt;
        params.end_dt = this.searchItems.end_dt;
        params.title = this.searchItems.title;
        if (this.searchItems.media) params.media = this.searchItems.media;
        params.row_per_page = this.searchItems.rowPerPage;
        params.select_page = this.searchItems.selectPage;
      }
      const arrListResult = await this.getarchiveCuesheetListArr(params);
      if (arrListResult) {
        this.setResponseData(arrListResult);
      }
      this.addScrollClass();
      this.isTableLoading = false;
      this.isScrollLodaing = false;
    },
    OnClickCommonTagItem(tag) {
      this.status = "accepted";
      this.searchItems.tag = tag;
      this.getData();
    },
    SDateErrorLog() {
      this.$fn.notify("error", {
        message: "시작 날짜가 종료 날짜보다 큽니다.",
      });
      this.hasErrorClass = true;
    },
  },
};
</script>