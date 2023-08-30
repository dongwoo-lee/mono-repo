<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="큐시트" />
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
        <!-- 방송일 -->
        <b-form-group label="방송일" class="has-float-label">
          <common-date-picker
            v-model="$v.searchItems.brd_dt.$model"
            required
            @changeDatePicker="changeDate"
            :maxDate="maxDate"
            :minDate="minDate"
          />
        </b-form-group>
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            style="width: 100px"
            v-model="searchItems.media"
            :options="mediaOptions"
            @change="onMediaChange($event)"
          />
        </b-form-group>
        <!-- 프로그램명 -->
        <b-form-group label="프로그램명" class="has-float-label">
          <b-form-select
            style="width: 400px"
            v-model="searchItems.pgmcode"
            :options="programOptions"
            @change="onPgmChange($event)"
          />
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
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <common-actions :rowData="props.props.rowData"> </common-actions>
          </template>
        </common-data-table-scroll-paging>
      </template>
    </common-form>
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import "moment/locale/ko";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import { USER_NAME, CUESHEET_CODE } from "@/constants/config";
const moment = require("moment");

export default {
  mixins: [MixinBasicPage],
  data() {
    const now = new Date();
    const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    const nextMonth = new Date(today);
    nextMonth.setMonth(nextMonth.getMonth() + 1);

    return {
      maxDate: nextMonth,
      minDate: new Date(2022, 11 - 1, 1),
      date: new Date(),
      pramObj: { person: null, brd_dt: null, media: null, pgmcode: "NEW" },
      pgmList: [],
      programOptions: [],
      mediaOptions: [],
      productIds: [],
      selectMediaAllProductIds: [],
      searchItems: {
        brd_dt: null, // 방송일
        media: "", // 매체
        pgmcode: "",
        productid: "", // 프로그램명
        rowPerPage: 30,
        selectPage: 1,
      },
      fields: [
        {
          name: "media",
          title: "매체",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
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
          name: "r_ONAIRTIME",
          title: "방송예정일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "15%",
          callback: (value) => {
            return value === null
              ? ""
              : moment(value, "YYYY-MM-DD'T'HH:mm:ss").format(
                  "YYYY-MM-DD (ddd)"
                );
          },
        },
        {
          name: "r_ONAIRTIME",
          title: "방송시간",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%",
          callback: (value) => {
            return value === null
              ? ""
              : moment(value, "YYYY-MM-DD'T'HH:mm:ss").format("HH:mm");
          },
        },
        {
          name: "eventname",
          title: "프로그램명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
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
  computed: {
    ...mapGetters("user", ["behaviorList"]),
  },
  async mounted() {
    this.isTableLoading = true;
    const toDay = await this.GetDateString(this.date);
    const userName = sessionStorage.getItem(USER_NAME);
    await this.renewal();
    const isCueAdmin = this.behaviorList.some(
      (data) => data.id === CUESHEET_CODE && data.visible === "Y"
    );
    if (!isCueAdmin) this.pramObj.person = userName;
    this.pramObj.brd_dt = toDay;
    this.searchItems.brd_dt = toDay;
    this.pgmList = await this.GetSchPgmList(this.pramObj);

    [this.mediaOptions, this.programOptions, this.productIds] =
      await Promise.all([
        this.SetMediaOption(this.pgmList),
        this.SetProgramCodeOption(this.pgmList),
        this.SetProductIds(this.pgmList),
      ]);
    this.getData();
  },
  methods: {
    ...mapActions("cueList", ["GetDateString"]),
    ...mapActions("cueList", ["getcuesheetListArr"]),
    ...mapActions("cueList", ["GetSchPgmList"]),
    ...mapActions("cueList", ["SetMediaOption"]),
    ...mapActions("cueList", ["SetProgramCodeOption"]),
    ...mapActions("cueList", ["SetProductIds"]),
    ...mapActions("user", ["renewal"]),
    async getData() {
      this.searchItems.rowPerPage = Number(this.searchItems.rowPerPage);
      this.isTableLoading = this.isScrollLodaing ? false : true;
      if (this.searchItems.productid === "")
        this.searchItems.productid = this.productIds;
      const params = {
        brd_dt: this.searchItems.brd_dt,
        products: this.searchItems.productid,
        row_per_page: this.searchItems.rowPerPage,
        select_page: this.searchItems.selectPage,
        media: this.searchItems.media,
      };
      const arrListResult = await this.getcuesheetListArr(params);
      if (arrListResult) {
        this.setResponseData(arrListResult);
      }
      this.addScrollClass();
      this.isTableLoading = false;
      this.isScrollLodaing = false;
    },
    async onMediaChange(e) {
      if (e === "") {
        this.programOptions = await this.SetProgramCodeOption(this.pgmList);
        this.searchItems.pgmcode = "";
        this.searchItems.productid = this.productIds;
      } else {
        const selectMediaObj = this.pgmList.filter((pgm) => pgm.media === e);
        this.selectMediaAllProductIds = await this.SetProductIds(
          selectMediaObj
        );
        this.programOptions = await this.SetProgramCodeOption(selectMediaObj);
        this.searchItems.pgmcode = "";
        this.searchItems.productid = this.selectMediaAllProductIds;
      }
      this.onSearch();
    },
    async onPgmChange(e) {
      if (e) {
        const selectPgmCodeObj = this.pgmList.filter(
          (pgm) => pgm.pgmcode === e
        );
        this.searchItems.productid = await this.SetProductIds(selectPgmCodeObj);
      } else {
        if (this.searchItems.media) {
          this.searchItems.productid = this.selectMediaAllProductIds;
        } else {
          this.searchItems.productid = this.productIds;
        }
      }
    },
    async changeDate(date) {
      this.pramObj.brd_dt = date;
      this.programOptions = [];
      this.productIds = [];
      this.searchItems.media = "";
      this.searchItems.pgmcode = "";
      this.searchItems.productid = "";
      this.pgmList = await this.GetSchPgmList(this.pramObj);
      [this.mediaOptions, this.programOptions, this.productIds] =
        await Promise.all([
          this.SetMediaOption(this.pgmList),
          this.SetProgramCodeOption(this.pgmList),
          this.SetProductIds(this.pgmList),
        ]);

      this.searchItems.productid = this.productIds;
      this.onSearch();
    },
  },
};
</script>
