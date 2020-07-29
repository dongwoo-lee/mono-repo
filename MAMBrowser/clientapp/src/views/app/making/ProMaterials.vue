<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb heading="(구)프로소재"/>
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
                  <b-form-select v-model="searchItems.media" :options="mediaOptions"></b-form-select>
                </b-form-group>
              </b-colxx>
              <!-- 구분 -->
              <b-colxx sm="2">
                <b-form-group label="구분">
                  <b-form-checkbox-group v-model.trim="searchItems.type" >
                    <b-form-checkbox value="Y">방송중</b-form-checkbox>
                    <b-form-checkbox value="N">폐지</b-form-checkbox>
                  </b-form-checkbox-group>
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
      searchItems: {
        media: '',             // 매체
        cate: '',              // 분류
        type: 'Y',              // 타입
        editor: '',            // 제작자
        name: '',              // 소재명
        rowPerPage: 16,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      fields: [
        {
          name: 'rowNO',
          title: 'No',
          titleClass: 'center aligned',
          dataClass: 'right aligned',
          width: '4%',
        },
        {
          name: "name",
          title: "소재명",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "categoryName",
          title: "분류",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "duration",
          title: "길이",
          titleClass: "",
          dataClass: "list-item-heading",
          callback: (v) => {
            return this.$fn.splitFirst(v);
          }
        },
        {
          name: "track",
          title: "트랙",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "editorName",
          title: "제작자",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "editDtm",
          title: "편집일시",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "masteringDtm",
          title: "마스터링일시",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "proType",
          title: "타입",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "filePath",
          title: "파일경로",
          titleClass: "",
          dataClass: "list-item-heading",
        },
      ]
    }
  },
  created() {
    this.getEditorOptions();
    this.getProOptions();
  },
  methods: {
    getData() {
      this.$http.get(`/api/Products/old_pro`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
      });
    }
  }
}
</script>