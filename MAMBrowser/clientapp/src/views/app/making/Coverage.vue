<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb heading="취재물"/>
      <div class="separator mb-5"></div>
    </b-colxx>
  </b-row>
  <b-row>
    <b-colxx xxs="12">
        <b-card class="mb-4">
          <b-form @submit.stop>
            <b-row>
              <!-- 분류 -->
              <b-colxx sm="2">
                <b-form-group label="분류" class="has-float-label">
                  <c-dropdown-menu-input :suggestions="rePortOptions" @selected="onReportSelected" />
                </b-form-group>
              </b-colxx>
              <!-- 방송일(시작일-종료일) -->
              <b-colxx sm="2">
                <b-form-group label="시작일" class="has-float-label c-zindex">
                    <c-input-date-picker v-model="$v.searchItems.start_dt.$model"/>
                    <b-form-invalid-feedback :state="$v.searchItems.start_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                </b-form-group>
              </b-colxx>
              <b-colxx sm="2">
                <b-form-group label="종료일" class="has-float-label c-zindex">
                    <c-input-date-picker v-model="$v.searchItems.end_dt.$model"/>
                    <b-form-invalid-feedback :state="$v.searchItems.end_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                </b-form-group>
              </b-colxx>
              <!-- 사용처 -->
              <b-colxx sm="2">
                <b-form-group label="사용처" class="has-float-label">
                  <c-dropdown-menu-input :suggestions="editorOptions" @selected="onEditorSelected" />
                </b-form-group>
              </b-colxx>
              <!-- 취재인 -->
              <b-colxx sm="2">
                <b-form-group label="취재인" class="has-float-label">
                  <c-dropdown-menu-input :suggestions="rePortOptions" @selected="onReportSelected" />
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
      rePortOptions: [],       // 취재물 분류 목록
      editorOptions: [],       // 사용자 목록 조회
      searchItems: {
        cate: '',              // 분류
        start_dt: '20200101',  // 방송일(시작일)
        end_dt: '20200730',    // 방송일(종료일)
        pgm: '',               // 사용처1
        pgmName: '',           // 사용처2
        reporterName: '',      // 취재인 이름
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
          name: "reporter",
          title: "취재인",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "pgmName",
          title: "사용처명",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "brdDT",
          title: "방송일",
          titleClass: "",
          dataClass: "list-item-heading",
          callback: (v) => {
            return this.$fn.formatDate(v, 'yyyy-MM-dd');
          }
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
          name: "filePath",
          title: "파일경로",
          titleClass: "",
          dataClass: "list-item-heading",
        },
      ]
    }
  },
  created() {
    this.getReportOptions();
    this.getEditorOptions();
  },
  methods: {
    getData() {
      this.$http.get(`/api/Products/report`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
      });
    },
    // 취재물 분류 선택
    onReportSelected(data) {
      const { id, name } = data;
      this.searchItems.pgm = id;
      this.searchItems.pgmName = name;
    }
  }
}
</script>