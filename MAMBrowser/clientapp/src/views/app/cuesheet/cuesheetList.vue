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
          startDateLabel="방송일(~부터)"
          endDateLabel="방송일(~까지)"
          :startDate.sync="searchItems.start_dt"
          :endDate.sync="searchItems.end_dt"
          :required="false"
          :isCurrentDate="false"
        />
        <!-- 채널 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            class="width-140"
            v-model="searchItems.channel"
            :options="[
              { value: '', text: '전체' },
              { value: 'Y', text: 'FM4U' },
              { value: 'N', text: '표준FM' },
            ]"
          />
        </b-form-group>
        <!-- 프로그램명 -->
        <b-form-group label="프로그램명" class="has-float-label">
          <b-form-select
            class="width-230"
            v-model="searchItems.programName"
            :options="[
              { value: '', text: '전체' },
              { value: 'Y', text: '김이나의 별이 빛나는 밤에' },
              { value: 'N', text: '두시의 데이트 뮤지, 안영미입니다' },
            ]"
          />
        </b-form-group>
        <!-- 태그 -->
        <b-form-group label="태그" class="has-float-label">
          <common-input-text class-string="memo" v-model="searchItems.memo" />
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
          :rows="cuesheet"
          :per-page="responseData.rowPerPage"
          :totalCount="responseData.totalRowCount"
          is-actions-slot
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @sortableclick="onSortable"
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
import { mapGetters } from "vuex";
import MixinBasicPage from "../../../mixin/MixinBasicPage";

const date = new Date();
date.setDate(date.getDate() - 14);

export default {
  mixins: [MixinBasicPage],
  data() {
    return {
      date,
      searchItems: {
        broadcastDate: "", // 방송예정일
        broadcastTime: "", // 방송시간
        channel: "", // 채널
        responsibility: "", // 담당자
        programName: "", // 프로그램명
        broadcastStatus: "", //방송여부
      },
      proOptions: [],
      fields: [
        {
          name: "broadcastDate",
          title: "방송일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "20%",
        },
        {
          name: "broadcastTime",
          title: "방송시간",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%",
        },
        {
          name: "programName",
          title: "프로그램명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "categoryName",
        },
        {
          name: "channel",
          title: "채널",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%",
          sortField: "duration",
        },
        {
          name: "__slot:actions",
          title: "",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
        },
      ],
    };
  },

  computed: {
    ...mapGetters("cuesheet", ["cuesheet"]),
  },

  created() {
    // (구)프로소재, 공유소재 매체 목록 조회
    this.getMediaPrimaryOptions();
    // 사용자 목록 조회
    this.getEditorOptions();
    // (구)프로 목록 조회
    this.getProOptions();
  },
  methods: {
    getData() {
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
      this.isTableLoading = this.isScrollLodaing ? false : true;
      this.$http
        .get(`/api/products/old_pro`, { params: this.searchItems })
        .then((res) => {
          this.setResponseData(res);
          this.addScrollClass();
          this.isTableLoading = false;
          this.isScrollLodaing = false;
        });
    },
  },
};
</script>