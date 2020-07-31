<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb heading="휴지통"/>
      <div class="separator mb-5"></div>
    </b-colxx>
  </b-row>
  <b-row>
    <b-colxx xxs="12">
        <b-card class="mb-4">
          <b-form>
            <b-row>
              <!-- 파일명 -->
              <b-colxx sm="2">
                <b-form-group label="파일명" class="has-float-label">
                  <c-input-text v-model="searchItems.filename"/>
                </b-form-group>
              </b-colxx>
              <!-- 제목 -->
              <b-colxx sm="2">
                <b-form-group label="제목" class="has-float-label">
                  <c-input-text v-model="searchItems.title"/>
                </b-form-group>
              </b-colxx>
              <!-- 기간: 시작일 -->
              <b-colxx sm="2">
                <b-form-group label="시작일" class="has-float-label c-zindex">
                    <c-input-date-picker v-model="$v.searchItems.start_dt.$model"/>
                    <b-form-invalid-feedback :state="$v.searchItems.start_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                </b-form-group>
              </b-colxx>
              <!-- 기간: 종료일 -->
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
                  <b-button class="mb-1" variant="primary default" size="sm">선택항목 삭제</b-button>
              </b-input-group>
              <b-input-group class="mr-2">
                  <b-button class="mb-1" variant="secondary default" size="sm">선택항목 복원</b-button>
              </b-input-group>
              <b-input-group class="mr-2">
                  <b-button class="mb-1" variant="danger default" size="sm">휴지통 비우기</b-button>
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
        cate: '',              // 분류
        filename: '',          // 파일명
        title: '',             // 제목
        memo: '',              // 메모
        start_dt: '',          // 시작일
        end_dt: '',            // 종료일
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
          dataClass: "center aligned text-center",
          width: '4%',
        },
        {
          name: "fileName",
          title: "파일명",
          titleClass: 'center aligned',
          dataClass: "center aligned",
        },
        {
          name: "title",
          title: "제목",
          titleClass: 'center aligned',
          dataClass: "center aligned",
        },
        {
          name: "memo",
          title: "메모",
          titleClass: 'center aligned',
          dataClass: "center aligned",
        },
        {
          name: "audioFormat",
          title: "파일형식",
          titleClass: 'center aligned',
          dataClass: "center aligned",
        },
        {
          name: "",
          title: "상세정보",
          titleClass: 'center aligned',
          dataClass: "center aligned",
        },
        {
          name: "editDtm",
          title: "삭제일시",
          titleClass: 'center aligned',
          dataClass: "center aligned",
        },
      ],
      contextMenu: [
        { name: 'restore', text: '복원' },
        { name: 'delete', text: '영구 삭제' },
      ]
    }
  },
  methods: {
    getData() {
      const editor = sessionStorage.getItem('user_name');

      this.$http.get(`api/products/workspace/private/recyclebin/${editor}`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
      });
    },
  }
}
</script>