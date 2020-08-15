<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb heading="부조 SPOT"/>
      <div class="separator mb-5"></div>
    </b-colxx>
  </b-row>
  <b-row>
    <b-colxx xxs="12">
      <!-- 검색 -->
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
            <!-- 시작일-종료일 -->
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
        start_dt: '20200101',       // 시작일
        end_dt: '',                 // 종료일
        editor: '',                 // 제작자ID
        editorName: '',             // 제작자이름
        name: '',                   // 소재명
        media: 'A',
        rowPerPage: 16,
        selectPage: 1,
        sortKey: '',
        sortValue: 'DESC',
      },
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
          name: "track",
          title: "트랙",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '4%',
        },
        {
          name: "brdDT",
          title: "방송일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
          callback: (v) => {
            return this.$fn.formatDate(v, 'yyyy-MM-dd');
          }
        },
        {
          name: "pgmName",
          title: "사용처명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "masteringDtm",
          title: "마스터링 일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '9%',
        },
        {
          name: "filePath",
          title: "파일경로",
          titleClass: 'center aligned text-center',
          dataClass: "center aligned word-break",
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
  },
  methods: {
    getData() {
      if (this.$v.$invalid) {
        this.$fn.notify('inputError', {});
        return;
      }

      this.$http.get(`/api/Products/spot/scr`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
      });
    },
  }
}
</script>