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
        <!-- 시작일 ~ 종료일 -->
        <common-start-end-date-picker 
          :startDate.sync="searchItems.start_dt"
          :startDayAgo="7"
          :maxPeriodMonth="3"
          :endDate.sync="searchItems.end_dt"
          :required="true"
        />
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
        <!-- 사용처 -->
        <b-form-group label="사용처명" class="has-float-label w-10">
          <common-input-text v-model="searchItems.pgmName"/>
        </b-form-group>
        <!-- 취재인 -->
        <b-form-group label="취재인" class="has-float-label w-10">
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
        <!-- 마스터링 완료한 소재만 보기 -->
         <b-form-checkbox class="custom-checkbox-group-non-align"
            v-model="searchItems.isMastering"
            value="Y"
            unchecked-value="N"
            aria-describedby="selectedSearchType1"
            aria-controls="selectedSearchType1">
            마스터링 완료한 소재만 보기
          </b-form-checkbox>
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
          tableHeight='520px'
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          :totalCount="responseData.totalRowCount"
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
              @mydiskCopy="onCopyToMySpacePopup"
            >
            </common-actions>
          </template>
        </common-data-table-scroll-paging>

         <CopyToMySpacePopup
          ref="refCopyToMySpacePopup"
          :show="copyToMySpacePopup"
          @ok="onMyDiskCopyFromProduct"
          @close="copyToMySpacePopup = false">
        </CopyToMySpacePopup>
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
import CopyToMySpacePopup from "../../../components/Popup/CopyToMySpacePopup";
export default {
  components:{CopyToMySpacePopup},
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
        isMastering: 'Y',
        rowPerPage: 30,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      isTableLoading: false,
      fields: [
        {
          name: 'rowNO',
          title: '순서',
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '4%',
        },
        {
          name: "name",
          title: "소재명",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center bold",
          sortField: 'name',
          
        },
        {
          name: "reporter",
          title: "취재인",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '6%',
          sortField: 'reporter',
        },
        {
          name: "pgmName",
          title: "사용처명",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center bold",
          sortField: 'pgmName',
        },
        {
          name: "brdDT",
          title: "방송일",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center bold",
          width: '10%',
          sortField: 'brdDT',
          callback: (v) => {
            return this.$fn.dateStringTohaipun(v);
          }
        },
        {
          name: "duration",
          title: "길이(초)",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '6%',
          sortField: 'duration',
          callback: (v) => {
            return this.$fn.splitFirst(v);
          }
        },
        {
          name: "editorName",
          title: "제작자",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '8%',
          sortField: 'editorName',
        },
        {
          name: "editDtm",
          title: "최종편집일시",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '11%',
          sortField: 'editDtm',
        },
        {
          name: "masteringDtm",
          title: "마스터링일시",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center",
          width: '11%',
          sortField: 'masteringDtm',
        },
        {
          name: '__slot:actions',
          title: '추가작업',
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "7%"
        }
      ]
    }
  },
  created() {
    // 취재물 분류 목록 조회
    this.getReportOptions();
    // 사용자 목록 조회
    this.getEditorForReporter();
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