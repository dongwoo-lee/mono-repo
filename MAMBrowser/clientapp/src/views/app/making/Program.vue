<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="프로그램" />
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
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            class="width-120"
            v-model="searchItems.media"
            :options="mediaOptions"
            value-field="id"
            text-field="name" 
          />
        </b-form-group>
        <!-- 방송일 -->
       <b-form-group label="방송일" class="has-float-label">
          <common-date-picker v-model="$v.searchItems.brd_dt.$model" />
          <b-form-invalid-feedback :state="$v.searchItems.brd_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
        </b-form-group>
        <!-- 검색버튼 -->
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
        <common-data-table
          :fields="fields"
          :rows="responseData.data"
          :isTableLoading="isTableLoading"
          @contextMenuAction="onContextMenuAction"
        />
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
        media: 'A',
        brd_dt: '',
        rowPerPage: 15,
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
          name: "mediaName",
          title: "매체",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '5%',
        },
        {
          name: "name",
          title: "프로그램",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "brdDT",
          title: "방송일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
          callback: (v) => {
            return this.$fn.dateStringTohaipun(v)
          }
        },
        {
          name: "brdTime",
          title: "방송시간",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '7%',
        },
        {
          name: "status",
          title: "상태",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
        },
        {
          name: "duration",
          title: "길이",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
        },
        {
          name: "track",
          title: "트랙",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
        },
        {
          name: "editorName",
          title: "제작자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%'
        },
        {
          name: "editDtm",
          title: "편집일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '12%'
        },
        {
          name: "reqCompleteDtm",
          title: "방송의뢰일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '15%',
        },
      ],
      contextMenu: [
        { name: 'download', text: '다운로드' },
        { name: 'storage', text: '내 저장공간으로 복사' },
      ]
    }
  },
  created() {
    // 매체목록 조회
    this.getMediaOptions();
  },
  methods: {
    getData() {
      if (this.$v.$invalid) {
        this.$fn.notify('inputError', {});
        return;
      }

      this.isTableLoading = this.isScrollLodaing ? false: true;
      const media = this.searchItems.media;
      const brd_dt = this.searchItems.brd_dt;

      this.$http.get(`/api/Products/pgm/${media}/${brd_dt}`)
        .then(res => {
           this.setResponseData(res, 'normal');
           this.isTableLoading = false;
           this.isScrollLodaing = false;
      });
    }
  },
}
</script>