<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="주조SPOT" />
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
            @change="onChangeMedia()"
          />
        </b-form-group>
        <!-- 시작일 -->
        <b-form-group label="방송기간(시작일)"
          class="has-float-label"
          :class="{ 'hasError': hasErrorClass }">
          <common-date-picker v-model="searchItems.start_dt" :dayAgo="7" required/>
        </b-form-group>
        <!-- 종료일 -->
        <b-form-group label="방송기간(종료일)"
          class="has-float-label"
          :class="{ 'hasError': hasErrorClass }">
          <common-date-picker v-model="searchItems.end_dt" required/>
        </b-form-group>
        <!-- 사용처 -->
        <b-form-group label="사용처" class="has-float-label">
          <common-dropdown-menu-input 
            classString="width-220"
            :isLoadingClass="isLoadingClass"
            :suggestions="spotOptions"
            @selected="onSpotSelected"
          />
        </b-form-group>
        <!-- 상태 -->
        <!-- <b-form-group label="상태" class="has-float-label">
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
        </b-form-group> -->
        <!-- 제작자 -->
        <b-form-group label="제작자" class="has-float-label">
          <common-dropdown-menu-input classString="width-180" :suggestions="editorOptions" @selected="onEditorSelected" />
        </b-form-group>
        <!-- 검색버튼 -->
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
          is-actions-slot
          :per-page="responseData.rowPerPage"
          :num-rows-to-bottom="numRowsToBottom"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @sortableclick="onSortable"
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
        cate: '',                  // 분류
        start_dt: '',              // 시작일
        end_dt: '',                // 종료일
        status: '',                // 상태
        editor: '',                // 사용자
        editorName: '',            // 사용자 이름
        spotId: '',                // spotId
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
          name: "brdDT",
          title: "방송일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
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
        },
        {
          name: "duration",
          title: "길이",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
        },
        {
          name: "editorName",
          title: "제작자",
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
          name: '__slot:actions',
          title: 'Actions',
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
    // 사용자 목록 조회
    this.getEditorOptions();
    // 주조 spot 분류 목록 조회
    this.getSpotOptions(this.searchItems.media);
    // 방송의뢰 상태 목록 조회
    // this.getReqStatusOptions();
  },
  methods: {
    getData() {
      if (this.$fn.checkGreaterStartDate(this.searchItems.start_dt, this.searchItems.end_dt)) {
        this.$fn.notify('error', { message: '시작 날짜가 종료 날짜보다 큽니다.' });
        this.hasErrorClass = true;
      }

      this.isTableLoading = this.isScrollLodaing ? false: true;
      const media = this.searchItems.media;

      this.$http.get(`/api/products/spot/mcr/${media}`, { params: this.searchItems })
        .then(res => {
           this.setResponseData(res, 'normal');
           this.addScrollClass();
           this.isTableLoading = false;
           this.isScrollLodaing = false;
      });
    },
    onChangeMedia() {
        this.getSpotOptions(this.searchItems.media);
    },
  },
}
</script>