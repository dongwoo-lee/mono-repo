<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="(구)프로소재" />
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
        <!-- 매체 -->
        <!-- <b-form-group label="매체" class="has-float-label">
          <b-form-select
          class="width-100"
          v-model="searchItems.media"
          :options="mediaPrimaryOptions"
          value-field="id"
          text-field="name" 
        /> 
        </b-form-group>-->
        <!-- 구분 -->
        <b-form-group label="구분" class="has-float-label">
          <b-form-select
            class="width-140"
            v-model="searchItems.type"
            :options="[{ value: '', text: '선택해주세요.' }, { value: 'Y', text: '방송중' }, { value: 'N', text: '폐지' }]"
          />
        </b-form-group>
        <!-- 분류 -->
        <b-form-group label="분류" class="has-float-label">
            <common-dropdown-menu-input 
            classString="width-220" 
            :suggestions="proOptions" 
            @selected="onProSelected"
          />
          <!-- <b-form-select
            class="width-120"
            v-model="searchItems.cate"
            :options="proOptions"
            value-field="id"
            text-field="name"
          /> -->
        </b-form-group>
        <!-- 제작자 -->
        <b-form-group label="제작자" class="has-float-label">
          <common-dropdown-menu-input :suggestions="editorOptions" @selected="onEditorSelected" />
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
        media: 'A',             // 매체
        cate: '',              // 분류
        type: '',              // 타입
        editor: '',            // 제작자
        name: '',              // 소재명
        rowPerPage: 15,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      proOptions: [],
      fields: [
        {
          name: 'rowNO',
          title: 'No',
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '4%',
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
          name: "editorName",
          title: "제작자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
        },
        {
          name: "editDtm",
          title: "편집일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '12%',
        },
        {
          name: "masteringDtm",
          title: "마스터링 일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '12%',
        },
        {
          name: "proType",
          title: "타입",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
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
    // (구)프로소재, 공유소재 매체 목록 조회
    this.getMediaPrimaryOptions();
    // 사용자 목록 조회
    this.getEditorOptions();
    // (구)프로 목록 조회
    this.getProOptions();
  },
  methods: {
    getData() {
      this.isTableLoading = this.isScrollLodaing ? false: true;
      this.$http.get(`/api/products/old_pro`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
            this.addScrollClass();
            this.isTableLoading = false;
            this.isScrollLodaing = false;
      });
    },
    onProSelected(data) {
      this.searchItems.cate = data.id;
    }
  }
}
</script>