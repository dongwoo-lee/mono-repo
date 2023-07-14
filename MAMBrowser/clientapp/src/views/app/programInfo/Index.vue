<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="프로그램 정보" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      :isDisplayBtnArea="true"
      :isDisplayPageSize="false"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select style="width: 100px" v-model="searchItems.media" />
        </b-form-group>
        <!-- 방송일 -->
        <b-form-group label="방송일" class="has-float-label">
          <common-date-picker required />
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
        <!-- {{ getPageInfo() }} -->
        <div style="height: 250px"></div>
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <common-data-table-scroll-paging
          ref="scrollPaging"
          tableHeight="290px"
          :fields="fields"
          :rows="testdata"
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
const moment = require("moment");

export default {
  mixins: [MixinBasicPage],
  data() {
    const now = new Date();
    const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    const nextMonth = new Date(today);
    nextMonth.setMonth(nextMonth.getMonth() + 1);

    return {
      date: new Date(),
      searchItems: {
        brdDate: null, // 방송일
        media: "A", // 매체
        rowPerPage: 30,
        selectPage: 1,
      },
      fields: [
        {
          name: "num",
          title: "순서",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
        },
        {
          name: "productid",
          title: "프로그램 ID",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "20%",
        },
        {
          name: "eventname",
          title: "프로그램 명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "25%",
        },
        {
          name: "onairtime",
          title: "방송시작시간",
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
          name: "td",
          title: "기술감독",
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
      testdata: [
        {
          num: 1,
          productid: "PMDAT1NF",
          eventname: "2시의 데이트 1부",
          td: "김무녕",
          onairtime: "2022-10-11 05:00:23",
        },
        {
          num: 2,
          productid: "PMDAT2NF",
          eventname: "2시의 데이트 2부",
          td: "김무녕",
          onairtime: "2022-10-11 05:22:23",
        },
        {
          num: 3,
          productid: "PMDAT3NF",
          eventname: "2시의 데이트 3부",
          td: "박대안",
          onairtime: "2022-10-11 05:42:23",
        },
        {
          num: 4,
          productid: "PMDAT4NF",
          eventname: "2시의 데이트 4부",
          td: "박대안",
          onairtime: "2022-10-11 06:02:23",
        },
      ],
    };
  },
  mounted() {
    this.getData();
  },
  methods: {
    async getData() {
      console.log("getData");
    },
  },
};
</script>
