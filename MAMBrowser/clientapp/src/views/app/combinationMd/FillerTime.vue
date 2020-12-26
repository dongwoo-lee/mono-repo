<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="FILLER(시간)" />
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
        <b-form-group label="방송기간(시작일)" 
          class="has-float-label"
          :class="{ 'hasError': hasErrorClass }">
          <common-date-picker v-model="searchItems.start_dt" :dayAgo="30" required/>
        </b-form-group>
        <!-- 종료일 -->
        <b-form-group label="방송기간(종료일)"
          class="has-float-label"
          :class="{ 'hasError': hasErrorClass }">
          <common-date-picker v-model="$v.searchItems.end_dt.$model" required/>
        </b-form-group>
        <!-- 분류 -->
        <b-form-group label="분류" class="has-float-label">
          <b-form-select
            class="width-220"
            v-model="searchItems.cate"
            :options="timetoneOptions"
            :disabled="timetoneOptions.length === 0"
            value-field="id"
            text-field="name"
          >
            <template v-slot:first>
              <b-form-select-option v-if="timetoneOptions.length > 0" value="">선택해주세요.</b-form-select-option>
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
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          is-actions-slot
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
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
import MixinFillerPage from '../../../mixin/MixinFillerPage';

export default {
  mixins: [ MixinFillerPage ],
  data() {
    return {
      searchItems: {
          media: 'A',                // 매체
          start_dt: '',              // 시작일
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
      isTableLoading: false,
      fields: [
        {
          name: 'rowNO',
          title: '순서',
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '4%',
        },
        {
          name: "name",
          title: "소재명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: '6%',
          sortField: 'name',
        },
        {
          name: "startDT",
          title: "방송게시일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: '8%',
          sortField: 'startDT',
          callback: (v) => {
              return this.$fn.dateStringTohaipun(v)
          }
        },
        {
          name: "endDT",
          title: "방송종료일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: '8%',
          sortField: 'endDT',
          callback: (v) => {
              return this.$fn.dateStringTohaipun(v)
          }
        },
        {
          name: "status",
          title: "상태",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '6%',
          sortField: 'status',
        },
        {
          name: "duration",
          title: "길이(초)",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
          sortField: 'duration',
        },
        {
          name: "editorName",
          title: "편집자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '7%',
          sortField: 'editorName',
        },
        {
          name: "editDtm",
          title: "최종편집일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '12%',
          sortField: 'editDtm',
        },
        {
          name: "fileName",
          title: "파일명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '10%',
          sortField: 'fileName',
        },
        
        {
          name: "masteringDtm",
          title: "마스터링일자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '12%',
          sortField: 'masteringDtm',
        },
        {
          name: '__slot:actions',
          title: '추가작업',
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%"
        }
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
    // 제작자(Md) 목록 조회
    this.getEditorForMd();
    this.getData();
  },
  methods: {
    getData() {
      if (this.$fn.checkGreaterStartDate(this.searchItems.start_dt, this.searchItems.end_dt)) {
        this.$fn.notify('error', { message: '시작 날짜가 종료 날짜보다 큽니다.' });
        this.hasErrorClass = true;
      }

      this.isTableLoading = this.isScrollLodaing ? false: true;
      const media = this.searchItems.media;

      this.$http.get(`/api/products/filler/time/${media}`, { params: this.searchItems })
          .then(res => {
          this.setResponseData(res, 'normal');
          this.addScrollClass();
          this.isTableLoading = false;
          this.isScrollLodaing = false;
      });
    },
  }
}
</script>
