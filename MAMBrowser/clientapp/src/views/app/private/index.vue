<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb :heading="$t('menu.private')"/>
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <b-row>
      <b-colxx xxs="12">
        <!-- 검색 -->
        <b-card class="mb-4">
          <b-form @submit.stop>
            <b-row>
              <!-- 제목 -->
              <b-colxx sm="2">
                <b-form-group label="제목" class="has-float-label c-zindex">
                  <c-input-text v-model="searchItems.title" />
                </b-form-group>
              </b-colxx>
              <!-- 메모 -->
              <b-colxx sm="2">
                <b-form-group label="메모" class="has-float-label c-zindex">
                  <c-input-text v-model="searchItems.memo" />
                </b-form-group>
              </b-colxx>
              <!-- 등록일: 시작일 -->
              <b-colxx sm="2">
                <b-form-group label="시작일" class="has-float-label c-zindex">
                    <c-input-date-picker v-model="$v.searchItems.start_dt.$model"/>
                    <b-form-invalid-feedback :state="$v.searchItems.start_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                </b-form-group>
              </b-colxx>
              <!-- 등록일: 종료일 -->
              <b-colxx sm="2">
                <b-form-group label="종료일" class="has-float-label c-zindex">
                    <c-input-date-picker v-model="$v.searchItems.end_dt.$model"/>
                    <b-form-invalid-feedback :state="$v.searchItems.end_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                </b-form-group>
              </b-colxx>
              <b-button class="mb-1" variant="primary default" size="sm" @click="getData">검색</b-button>
            </b-row>
          </b-form>
        </b-card>

        <!-- 테이블 -->
        <b-card class="mb-4">
            <b-form class="mb-3" inline>
                <b-input-group class="mr-2">
                    <b-button class="mb-1" variant="primary default" size="sm" @click="showModalFileUpload = true">파일 업로드</b-button>
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
                :is-actions-slot="true"
                :num-rows-to-bottom="5"
                :contextmenu="contextMenu"
                @scrollPerPage="onScrollPerPage"
                @contextMenuAction="onContextMenuAction"
                @sortableclick="onSortable"
            >
                <template slot="actions" scope="props">
                    <b-button class="mb-1" variant="primary default" @click="handlerPreview(props, props.rowIndex)">미리듣기</b-button>
                </template>
            </c-data-table-scroll-paging>
          </b-card>
      </b-colxx>
    </b-row>
    <!-- 파일업로드 팝업 -->
    <multi-file-upload-popup :show="showModalFileUpload" @close="showModalFileUpload=false"></multi-file-upload-popup>
  </div>
</template>

<script>
import MixinBasicPage from '../../../mixin/MixinBasicPage';

export default {
  mixins: [ MixinBasicPage ],
  data() {
    return {
      searchItems: {
        cate: '',              // 분류(cate)
        title: '',             // 제목
        memo: '',              // 메모(memo)
        start_dt: '20200101',  // 등록일 시작일
        end_dt: '20200730',    // 등록일 종료일
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
          name: "title",
          title: "파일명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%"
        },
        {
          name: "sales",
          title: "메모",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "stock",
          title: "파일형식",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "20%"
        },
        {
          name: "category",
          title: "상세정보",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "25%"
        },
        {
          name: "writeDate",
          title: "등록일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
      ],
    }
  },
  methods: {
    getData() {
      const editor = sessionStorage.getItem('user_name');

      this.$http.get(`/api/products/workspace/private/files/${editor}`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
      });
    },
  }
}
</script>