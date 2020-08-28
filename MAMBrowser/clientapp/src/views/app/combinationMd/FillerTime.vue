<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="필러(시간)" />
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
        <!-- 시작일 -->
        <b-form-group label="시작일" class="has-float-label">
          <common-date-picker v-model="$v.searchItems.start_dt.$model" />
          <b-form-invalid-feedback
            :state="$v.searchItems.start_dt.check_date"
          >날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
        </b-form-group>
        <!-- 종료일 -->
        <b-form-group label="종료일" class="has-float-label">
          <common-date-picker v-model="$v.searchItems.end_dt.$model" />
          <b-form-invalid-feedback
            :state="$v.searchItems.end_dt.check_date"
          >날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
        </b-form-group>
        <!-- 분류 -->
        <b-form-group label="분류" class="has-float-label">
          <b-form-select
            class="width-220"
            v-model="searchItems.cate"
            :options="categoryOptions"
            :disabled="categoryOptions.length === 0"
            value-field="id"
            text-field="name"
          >
            <template v-slot:first>
              <b-form-select-option v-if="categoryOptions.length > 0" value="">선택해주세요.</b-form-select-option>
              <b-form-select-option v-else value="">값이 존재하지 않습니다.</b-form-select-option>
            </template>
          </b-form-select>
        </b-form-group>
        <!-- 상태 -->
        <b-form-group label="상태" class="has-float-label">
          <b-form-select
            class="width-140"
            v-model="searchItems.status"
            :options="reqStatusOptions"
            value-field="id"
            text-field="name"
          >
            <template v-slot:first>
              <b-form-select-option value="">선택해주세요.</b-form-select-option>
            </template>
          </b-form-select>
        </b-form-group>
        <!-- 제작자 -->
        <b-form-group label="제작자" class="has-float-label">
          <common-dropdown-menu-input :suggestions="editorOptions" @selected="onEditorSelected" />
        </b-form-group>
        <!-- 검색 버튼 -->
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
          @scrollPerPage="onScrollPerPage"
        >
        </common-data-table-scroll-paging>
      </template>
    </common-form>
  </div>
</template>

<script>
import MixinFillerPage from '../../../mixin/MixinFillerPage';

export default {
  mixins: [ MixinFillerPage ],
  data() {
    return {
      searchItems: {
          media: 'A',                // 매체
          start_dt: '20200101',      // 시작일
          end_dt: '',                // 종료일
          cate: '',                  // 분류
          status: '',                // 상태
          editor: '',                // 사용자
          editorName: '',            // 사용자 이름
          rowPerPage: 15,
          selectPage: 1,
          sortKey: '',
          sortValue: '',
      },
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
          width: '6%',
        },
        {
          name: "startDT",
          title: "방송게시일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
          callback: (v) => {
              return this.$fn.formatDate(v, 'yyyy-mm-dd')
          }
        },
        {
          name: "endDT",
          title: "방송종료일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
          callback: (v) => {
              return this.$fn.formatDate(v, 'yyyy-mm-dd')
          }
        },
        {
          name: "status",
          title: "상태",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '6%',
        },
        {
          name: "duration",
          title: "길이",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
        },
        {
          name: "track",
          title: "트랙",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '4%',
        },
        {
          name: "editorName",
          title: "편집자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%'
        },
        {
          name: "editDtm",
          title: "편집일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '12%'
        },
        {
          name: "reqCompleteDtm",
          title: "방송의뢰일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '14%',
        },
        {
          name: "masteringDtm",
          title: "미스터링일자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '12%'
        },
      ],
    }
  },
  created() {
    // 매체목록 조회
    this.getMediaOptions();
    // 필러(시간) 분류 목록 조회
    this.getTimetoneOptions();
    // 방송의뢰 상태 목록 조회
    this.getReqStatusOptions();
    // 제작자(사용자) 목록 조회
    this.getEditorOptions();
    this.getData();
  },
  methods: {
    getData() {
      if (this.$v.$invalid) {
          this.$fn.notify('inputError', {});
          return;
      }

      const media = this.searchItems.media;

      this.$http.get(`/api/Products/filler/time/${media}`, { params: this.searchItems })
          .then(res => {
          this.setResponseData(res, 'normal');
      });
    },
  }
}
</script>
