<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="부조 SPOT" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      @changeRowPerpage="onChangeRowPerpage"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            class="width-120"
            v-model="searchItems.media"
            :options="mediaOptions"
            value-field="id"
            text-field="name" 
          />
        </b-form-group>
        <!-- 등록일: 시작일 -->
        <b-form-group label="시작일"
          class="has-float-label"
          :class="{ 'hasError': (hasErrorClass || $v.searchItems.start_dt.required) }">
          <common-date-picker v-model="$v.searchItems.start_dt.$model" :dayAgo="7" />
          <b-form-invalid-feedback
            :state="!$v.searchItems.start_dt.required"
          >날짜는 필수 입력입니다.</b-form-invalid-feedback>
        </b-form-group>
      <!-- 등록일: 종료일 -->
        <b-form-group label="종료일" 
          class="has-float-label"
          :class="{ 'hasError': (hasErrorClass || $v.searchItems.end_dt.required) }">
          <common-date-picker v-model="$v.searchItems.end_dt.$model" isCurrentDate/>
            <b-form-invalid-feedback
            :state="!$v.searchItems.end_dt.required"
          >날짜는 필수 입력입니다.</b-form-invalid-feedback>
        </b-form-group>
      <!-- 제작자 -->
        <b-form-group label="제작자" class="has-float-label">
          <common-dropdown-menu-input :suggestions="editorOptions" @selected="onEditorSelected" />
        </b-form-group>
      <!-- 소재명 -->
        <b-form-group label="소재명" class="has-float-label">
          <common-input-text v-model="searchItems.name"/>
        </b-form-group>
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch">검색</b-button>
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
          :table-height="'500px'"
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          :is-actions-slot="true"
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
        </common-data-table-scroll-paging>
      </template>
    </common-form>
  </div>
</template>

<script>
import MixinBasicPage from '../../../mixin/MixinBasicPage';

export default {
  mixins: [ MixinBasicPage ],
  data() {
    return {
      searchItems: {
        start_dt: '',       // 시작일
        end_dt: '',                 // 종료일
        editor: '',                 // 제작자ID
        editorName: '',             // 제작자이름
        name: '',                   // 소재명
        media: 'A',
        rowPerPage: 15,
        selectPage: 1,
        sortKey: '',
        sortValue: 'DESC',
      },
      isTableLoading: false,
      fields: [
        {
          name: 'rowNO',
          title: 'No',
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '4%',
        },
        {
          name: "mediaName",
          title: "매체",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '5%',
        },
        {
          name: "name",
          title: "소재명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "categoryName",
          title: "분류",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "duration",
          title: "길이",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '6%',
          callback: (v) => {
            return this.$fn.splitFirst(v);
          }
        },
        {
          name: "track",
          title: "트랙",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '4%',
        },
        {
          name: "brdDT",
          title: "방송일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
          callback: (v) => {
            return this.$fn.dateStringTohaipun(v);
          }
        },
        {
          name: "editorName",
          title: "제작자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '5%',
        },
        {
          name: "pgmName",
          title: "사용처명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "masteringDtm",
          title: "마스터링 일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '9%',
        },
      ]
    }
  },
  created() {
    // 매체 목록 조회
    this.getMediaOptions();
    // 사용자 목록 조회
    this.getEditorOptions();
  },
  methods: {
    getData() {
      const { start_dt, end_dt} = this.$v.searchItems;
      if (!start_dt.$invalid || !end_dt.$invalid) {
        this.$fn.notify('inputError', {});
        return;
      }

      if (this.$fn.checkGreaterStartDate(this.searchItems.start_dt, this.searchItems.end_dt)) {
        this.$fn.notify('error', { message: '시작 날짜가 종료 날짜보다 큽니다.' });
        this.hasErrorClass = true;
        return;
      }

      this.isTableLoading = this.isScrollLodaing ? false: true;
      this.$http.get(`/api/Products/spot/scr/${this.searchItems.media}`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
            this.addScrollClass();
            this.isTableLoading = false;
            this.isScrollLodaing = false;
      });
    },
  }
}
</script>