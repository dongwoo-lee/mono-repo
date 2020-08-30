<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="효과음" />
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
        <b-form-group label="검색어" class="has-float-label">
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
          :is-actions-slot="true"
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
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
        searchWord: '',
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
          name: "name",
          title: "제목",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "description",
          title: "설명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "name",
          title: "길이",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "duration",
          title: "음반",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "audioFormat",
          title: "오디오 포맷",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "filePath",
          title: "파일 경로",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
      ]
    }
  },
  methods: {
    getData() {
      this.isTableLoading = this.isScrollLodaing ? false: true;
      this.$http.get(`/api/Products/effect`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
            this.addScrollClass();
            this.isTableLoading = false;
            this.isScrollLodaing = false;
      });
    }
  }
}
</script>