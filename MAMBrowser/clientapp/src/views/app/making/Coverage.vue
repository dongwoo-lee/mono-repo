<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="취재물" />
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
        <!-- 분류 -->
        <b-form-group label="분류" class="has-float-label">
          <b-form-select
            class="width-100"
            v-model="searchItems.cate"
            :options="rePortOptions"
            value-field="id"
            text-field="name" 
          />
        </b-form-group>
        <!-- 방송 시작일 -->
        <b-form-group label="시작일" 
          class="has-float-label"
          :class="{ 'hasError': hasErrorClass }">
          <common-date-picker v-model="searchItems.start_dt" required/>
        </b-form-group>
        <!-- 방송 종료일 -->
        <b-form-group label="종료일"
          class="has-float-label"
          :class="{ 'hasError': hasErrorClass }">
          <common-date-picker v-model="searchItems.end_dt" required/>
        </b-form-group>
        <!-- 사용처 -->
        <b-form-group label="사용처명" class="has-float-label">
          <common-input-text v-model="searchItems.pgmName"/>
        </b-form-group>
        <!-- 취재인 -->
        <b-form-group label="취재인" class="has-float-label">
          <common-input-text v-model="searchItems.reporterName"/>
        </b-form-group>
        <!-- 제작자 -->
        <b-form-group label="제작자" class="has-float-label">
          <common-dropdown-menu-input classString="width-120" :suggestions="editorOptions" @selected="onEditorSelected" />
        </b-form-group>
        <!-- 소재명 -->
        <b-form-group label="소재명" class="has-float-label">
          <common-input-text v-model="searchItems.name"/>
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
          is-actions-slot
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @selectedIds="onSelectedIds"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :behaviorData="behaviorList"
              @preview="onPreview"
              @download="onDownloadProduct"
            >
            </common-actions>
          </template>
        </common-data-table-scroll-paging>
      </template>
    </common-form>

     <PlayerPopup 
    :showPlayerPopup="showPlayerPopup"
    :title="soundItem.name"
    :fileKey="soundItem.fileToken"
    :streamingUrl="streamingUrl"
    :waveformUrl="waveformUrl"
    :tempDownloadUrl="tempDownloadUrl"
    requestType="token"
    @closePlayer="onClosePlayer">
    </PlayerPopup>
  </div>
</template>

<script>
import MixinBasicPage from '../../../mixin/MixinBasicPage';

export default {
  mixins: [ MixinBasicPage ],
  data() {
    return {
      searchItems: {
        cate: 'RC07',          // 분류
        start_dt: '',          // 방송 시작일
        end_dt: '',            // 방송 종료일
        // brd_dt: '20200801',    // 방송일
        pgm: '',               // 사용처1
        pgmName: '',           // 사용처2
        reporterName: '',      // 취재인 이름
        editor: '',            // 제작자
        name: '',              // 소재명
        rowPerPage: 15,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      isTableLoading: false,
      fields: [
        {
          name: 'rowNO',
          title: 'No',
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '4%',
        },
        {
          name: "name",
          title: "소재명",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          
        },
        {
          name: "categoryName",
          title: "분류",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '8%'
        },
        {
          name: "reporter",
          title: "취재인",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '6%'
        },
        {
          name: "pgmName",
          title: "사용처명",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '8%',
        },
        {
          name: "brdDT",
          title: "방송일",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '8%',
          callback: (v) => {
            return this.$fn.dateStringTohaipun(v);
          }
        },
        {
          name: "duration",
          title: "길이",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '6%',
          callback: (v) => {
            return this.$fn.splitFirst(v);
          }
        },
        {
          name: "editorName",
          title: "제작자",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '5%',
        },
        {
          name: "editDtm",
          title: "편집일시",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '9%',
        },
        {
          name: "masteringDtm",
          title: "마스터링일시",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '9%',
        },
        {
          name: '__slot:actions',
          title: 'Actions',
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%"
        }
      ]
    }
  },
  created() {
    // 취재물 분류 목록 조회
    this.getReportOptions();
    // 사용자 목록 조회
    this.getEditorOptions();
  },
  methods: {
    getData() {
      if (this.$fn.checkGreaterStartDate(this.searchItems.start_dt, this.searchItems.end_dt)) {
        this.$fn.notify('warning', { message: '시작 날짜가 종료 날짜보다 큽니다.' });
        this.hasErrorClass = true;
      }

      this.isTableLoading = this.isScrollLodaing ? false: true;

      this.$http.get(`/api/products/report`, { params: this.searchItems })
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