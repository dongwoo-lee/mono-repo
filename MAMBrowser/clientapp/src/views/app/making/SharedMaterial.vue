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
                  <b-form-select 
                  v-model="searchItems.media"
                  :options="mediaOptions"
                  value-field="id"
                  text-field="name" 
                />
                </b-form-group>
              </b-colxx>
              <!-- 분류 -->
              <b-colxx sm="2">
                <b-form-group label="분류" class="has-float-label">
                  <c-dropdown-menu-input :suggestions="publicOptions" @selected="onPublicSelected" />
                </b-form-group>
              </b-colxx>
               <!-- 시작일 -->
              <b-colxx sm="2">
                <b-form-group label="시작일" class="has-float-label c-zindex">
                    <c-input-date-picker v-model="$v.searchItems.start_dt.$model"/>
                    <b-form-invalid-feedback :state="$v.searchItems.start_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                </b-form-group>
              </b-colxx>
              <!-- 종료일 -->
              <b-colxx sm="2">
                <b-form-group label="종료일" class="has-float-label c-zindex">
                    <c-input-date-picker v-model="$v.searchItems.end_dt.$model"/>
                    <b-form-invalid-feedback :state="$v.searchItems.end_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                </b-form-group>
              </b-colxx>
              <!-- 제작자 -->
              <b-colxx sm="2">
                <b-form-group label="제작자" class="has-float-label">
                  <c-dropdown-menu-input :suggestions="editorOptions" @selected="onEditorSelected" />
                </b-form-group>
              </b-colxx>
              <!-- 제목 -->
              <b-colxx sm="2">
                <b-form-group label="제목" class="has-float-label">
                  <c-input-text v-model="searchItems.title"/>
                </b-form-group>
              </b-colxx>
              <!-- 메모 -->
              <b-colxx sm="2">
                <b-form-group label="제목" class="has-float-label">
                  <c-input-text v-model="searchItems.memo"/>
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
              <b-button class="mb-1" variant="primary default" size="sm" @click="onShowModalFileUpload">파일 업로드</b-button>
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
      searchItems: {
        media: 'A',                // 매체
        cate: '',                  // 분류
        cateName: '',              // 분류명
        start_dt: '20200101',      // 시작일
        end_dt: '20200706',        // 종료일
        type: '',                  // 구분
        editor: '',                // 제작자
        title: '',                 // 제목
        memo: '',                  // 메모
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
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '5%',
        },
        {
          name: "name",
          title: "소재명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%"
        },
        {
          name: "categoryName",
          title: "분류",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%"
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
          name: "track",
          title: "트랙",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '5%',
        },
        {
          name: "editorName",
          title: "제작자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "editDtm",
          title: "편집일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%"
        },
        {
          name: "masteringDtm",
          title: "마스터일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%"
        },
        {
          name: "proType",
          title: "타입",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "filePath",
          title: "파일경로",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center word-break",
          width: "10%"
        },
      ]
    }
  },
  created() {
    // 매체 목록 조회
    this.getMediaOptions();
    // 사용자 목록 조회
    this.getEditorOptions();
    // 공유 소재 분류 목록 조회
    this.getpublicOptions();
  },
  methods: {
    getData() {
      this.$http.get(`/api/Products/old_pro`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
      });
    },
     onShowModalFileUpload() {
      this.open_popup();
    }
  }
}
</script>