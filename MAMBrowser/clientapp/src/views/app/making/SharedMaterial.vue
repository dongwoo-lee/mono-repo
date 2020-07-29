<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb heading="공유소재"/>
      <div class="separator mb-5"></div>
    </b-colxx>
  </b-row>
  <b-row>
    <b-colxx xxs="12">
      <b-card class="mb-4">
        <b-form @submit.stop>
            <b-row>
              <!-- 매체 -->
              <b-colxx sm="2">
                <b-form-group label="매체" class="has-float-label">
                  <b-form-select size="sm" v-model="searchItems.media" :options="mediaOptions"/>
                </b-form-group>
              </b-colxx>
              <!-- 분류 -->
              <b-colxx sm="2">
                <b-form-group label="분류" class="has-float-label">
                  <c-dropdown-menu-input :suggestions="proOptions" @selected="onProSelected" />
                </b-form-group>
              </b-colxx>
              <!-- 제작자 -->
              <b-colxx sm="2">
                <b-form-group label="제작자" class="has-float-label">
                  <c-dropdown-menu-input :suggestions="editorOptions" @selected="onEditorSelected" />
                </b-form-group>
              </b-colxx>
              <!-- 소재명 -->
              <b-colxx sm="2">
                <b-form-group label="소재명" class="has-float-label">
                  <c-input-text v-model="searchItems.name"/>
                </b-form-group>
              </b-colxx>
              <b-button class="mb-1" variant="primary default" size="sm" @click="onSearch">검색</b-button>
            </b-row>
        </b-form>
      </b-card>

      <!-- 테이블 -->
      <b-card class="mb-4">
        <b-form class="mb-3" inline>
          <b-input-group class="mr-2">
              <b-button class="mb-1" variant="primary default" size="sm">파일 업로드</b-button>
          </b-input-group>
          <b-input-group class="mr-2">
              <b-button class="mb-1" variant="secondary default" size="sm">다운로드</b-button>
          </b-input-group>
          <b-input-group class="mr-2">
              <b-button class="mb-1" variant="danger default" size="sm">삭제</b-button>
          </b-input-group>
          <b-input-group class="mr-2">
              <b-button class="mb-1" variant="info default" size="sm">정보편집</b-button>
          </b-input-group>
        </b-form>
        <c-data-table-scroll-paging
          ref="scrollPaging"
          :table-height="'500px'"
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          :num-rows-to-bottom="numRowsToBottom"
          :contextmenu="contextMenu"
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

export default {
  mixins: [ MixinBasicPage ],
  data() {
    return {
      proOptions: [],       // 분류 목록
      searchItems: {
        media: '',          // 매체
        cate: '',           // 분류
        type: '',           // 구분
        editor: '',         // 제작자
        name: '',           // 소재명
        rowPerPage: 16,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      fields: [
        {
          name: "__checkbox",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "5%"
        },
        {
          name: 'rowNO',
          title: 'No',
          titleClass: 'center aligned',
          dataClass: 'right aligned',
          width: '5%',
        },
        {
          name: "name",
          title: "소재명",
          titleClass: "",
          dataClass: "list-item-heading",
          width: "15%"
        },
        {
          name: "categoryName",
          title: "분류",
          titleClass: "",
          dataClass: "list-item-heading",
          width: "15%"
        },
        {
          name: "duration",
          title: "길이",
          titleClass: "",
          dataClass: "list-item-heading",
          width: "10%",
          callback: (v) => {
            return this.$fn.splitFirst(v);
          }
        },
        {
          name: "track",
          title: "트랙",
          titleClass: "",
          dataClass: "list-item-heading",
          width: "10%"
        },
        {
          name: "editorName",
          title: "제작자",
          titleClass: "",
          dataClass: "list-item-heading",
          width: "10%"
        },
        {
          name: "editDtm",
          title: "편집일시",
          titleClass: "",
          dataClass: "list-item-heading",
          width: "15%"
        },
        {
          name: "masteringDtm",
          title: "마스터일시",
          titleClass: "",
          dataClass: "list-item-heading",
          width: "15%"
        },
        {
          name: "proType",
          title: "타입",
          titleClass: "",
          dataClass: "list-item-heading",
          width: "10%"
        },
        {
          name: "filePath",
          title: "파일경로",
          titleClass: "",
          dataClass: "list-item-heading",
          width: "15%"
        },
      ]
    }
  },
  created() {
    this.getProOptions();
    this.getEditorOptions();
  },
  methods: {
    getData() {
      this.$http.get(`/api/Products/old_pro`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
      });
    },
    onProSelected(data) {
      console.info('onProSelected', data);
    }
  }
}
</script>