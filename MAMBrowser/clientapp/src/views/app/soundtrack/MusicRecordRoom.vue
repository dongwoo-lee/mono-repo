<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="음반 기록실" />
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
        <!-- 대분류 -->
        <b-form-group label="대분류">
          <b-form-checkbox-group v-model="searchItems.topCategory">
            <b-form-checkbox value="own">ALL</b-form-checkbox>
            <b-form-checkbox value="two">국내</b-form-checkbox>
            <b-form-checkbox value="three">국외</b-form-checkbox>
            <b-form-checkbox value="four">클래식</b-form-checkbox>
          </b-form-checkbox-group>
        </b-form-group>
        <!-- 소분류 -->
        <b-form-group label="소분류">
          <b-form-select
            class="width-100"
            v-model="searchItems.cate"
            :options="rePortOptions"
            value-field="id"
            text-field="name" 
          />
        </b-form-group>
        <!-- 검색옵션 -->
        <b-form-group label="검색옵션">
          <b-form-checkbox-group v-model="searchItems.searchKeyword">
            <b-form-checkbox value="own">히트곡</b-form-checkbox>
            <b-form-checkbox value="two">금지곡</b-form-checkbox>
            <b-form-checkbox value="three">주의</b-form-checkbox>
            <b-form-checkbox value="four">청소년유해</b-form-checkbox>
          </b-form-checkbox-group>
        </b-form-group>
        <!-- 검색어 -->
         <b-form-group label="검색어">
            <common-input-text v-model="searchItems.keyword"/>
          </b-form-group>
        <!-- 검색 버튼 -->
        <fieldset class="form-group">
          <div class="bv-no-focus-ring pt-26">
              <button type="button" class="btn btn-outline-primary default" @click="onSearch">검색</button>
          </div>
        </fieldset>
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
          @selectedIds="onSelectedIds"
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
        topCategory: [],
        bottomCategory: '',
        keyword: '',
        rowPerPage: 15,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      topCategoryOptions: [
        { label: 'ALL', code: 'ALL' },
        { label: '국내', code: 'DOMESTIC' },
        { label: '국외', code: 'FOREIGN' },
        { label: '클래식', code: 'CLASSIC' },
      ],
      fields: [
        {
          name: 'rowNO',
          title: 'No',
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '5%',
        },
        {
          name: "name",
          title: "곡명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%"
        },
        {
          name: "artistName",
          title: "아티스트",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "duration",
          title: "재생시간",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "albumName",
          title: "음반",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "trackNO",
          title: "트랙번호",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "releaseDate",
          title: "발매년도",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "composer",
          title: "작곡가",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "writer",
          title: "작사가",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "sequenceNO",
          title: "배열번호",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "lyrics",
          title: "마스터링",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
        },
        {
          name: "filePath",
          title: "파일경로",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned text-center word-break",
          width: "10%"
        },
      ]
    }
  },
  methods: {
    getData() {
      this.$http.get(`/api/Products/music`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
      });
    },
  }
}
</script>