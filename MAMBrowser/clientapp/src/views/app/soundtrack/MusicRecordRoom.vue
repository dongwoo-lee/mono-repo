<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb heading="음반 기록실"/>
      <div class="separator mb-5"></div>
    </b-colxx>
  </b-row>
  <b-row>
    <b-colxx xxs="12">
        <b-card class="mb-4">
          <b-form @submit.stop>
            <b-row>
              <!-- 대분류 -->
              <b-colxx sm="3">
                <b-form-group label="대분류">
                  <b-form-checkbox-group v-model="searchItems.topCategory">
                    <b-form-checkbox value="own">ALL</b-form-checkbox>
                    <b-form-checkbox value="two">국내</b-form-checkbox>
                    <b-form-checkbox value="three">국외</b-form-checkbox>
                    <b-form-checkbox value="four">클래식</b-form-checkbox>
                  </b-form-checkbox-group>
                </b-form-group>
              </b-colxx>
              <!-- 소분류 -->
              <b-colxx sm="2">
                <b-form-group label="소분류" class="has-float-label">
                  <!-- <b-form-select size="sm" v-model="searchItems.bottomCategory" :options="mediaOptions"/> -->
                </b-form-group>
              </b-colxx>
              <!-- 검색옵션 -->
              <b-colxx sm="3">
                <b-form-group label="검색옵션">
                  <b-form-checkbox-group v-model="searchItems.searchKeyword">
                    <b-form-checkbox value="own">히트곡</b-form-checkbox>
                    <b-form-checkbox value="two">금지곡</b-form-checkbox>
                    <b-form-checkbox value="three">주의</b-form-checkbox>
                    <b-form-checkbox value="four">청소년유해</b-form-checkbox>
                  </b-form-checkbox-group>
                </b-form-group>
              </b-colxx>
              <!-- 검색어 -->
              <b-colxx sm="2">
                <b-form-group label="검색어" class="has-float-label">
                  <common-input-text v-model="searchItems.keyword"/>
                </b-form-group>
              </b-colxx>
              <b-button class="mb-1" variant="primary default" size="sm" @click="onSearch">검색</b-button>
            </b-row>
        </b-form>
      </b-card>

      <!-- 테이블 -->
      <b-card class="mb-4">
        <c-data-table-scroll-paging
          ref="scrollPaging"
          :table-height="'500px'"
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          :num-rows-to-bottom="numRowsToBottom"
          :contextmenu="contextMenu"
          :css="{'table-scroll-body': true}"
          @scrollPerPage="onScrollPerPage"
          @contextMenuAction="onContextMenuAction"
          @sortableclick="onSortable"
        />
      </b-card>
    </b-colxx>
  </b-row>
  </div>
</template>

<script>
import MixinBasicPage from '../../../mixin/MixinBasicPage';
import vSelect from "vue-select";
import "vue-select/dist/vue-select.css";

export default {
  mixins: [ MixinBasicPage ],
  components: { vSelect },
  data() {
    return {
      topCategoryOptions: [
        { label: 'ALL', code: 'ALL' },
        { label: '국내', code: 'DOMESTIC' },
        { label: '국외', code: 'FOREIGN' },
        { label: '클래식', code: 'CLASSIC' },
      ],
      searchItems: {
        topCategory: [],
        bottomCategory: '',
        keyword: '',
        rowPerPage: 16,
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