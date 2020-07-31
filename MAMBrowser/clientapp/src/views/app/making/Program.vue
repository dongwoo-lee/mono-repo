<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb heading="프로그램"/>
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
            <!-- 방송일 -->
            <b-colxx sm="2">
              <b-form-group label="방송일" class="has-float-label">
                <c-input-date-picker v-model="$v.searchItems.brd_dt.$model" />
                <b-form-invalid-feedback :state="$v.searchItems.brd_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
              </b-form-group>
            </b-colxx>
            <b-button class="mb-1" variant="primary default" size="sm" @click="onSearch">검색</b-button>
          </b-row>
        </b-form>
      </b-card>
      <!-- 테이블 -->
      <b-card class="mb-4">
        <c-data-table
          :fields="fields"
          :rows="responseData.data"
          :contextmenu="contextMenu"
          @contextMenuAction="onContextMenuAction"
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
        media: 'A',
        brd_dt: '20200101',
      },      
      fields: [
        {
            name: '__sequence',
            title: 'No',
            titleClass: 'center aligned',
            dataClass: "center aligned text-center",
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
            return this.$fn.formatDate(v, 'yyyy-mm-dd')
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
          width: '6%',
        },
        {
          name: "duration",
          title: "길이",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '7%',
        },
        {
          name: "track",
          title: "트랙",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '4%',
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
          width: '8%'
        },
        {
          name: "reqCompleteDtm",
          title: "방송의뢰일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '9%',
        },
        {
          name: "filePath",
          title: "파일경로",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center word-break",
          width: '10%',
          callback: (v) => {
            return v.substring(1, v.length).replace(/\\/g, '/');
            // return v.replace(/\\\\/g, '/');
          }
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

      const media = this.searchItems.media;
      const brd_dt = this.searchItems.brd_dt;

      this.$http.get(`/api/Products/pgm/${media}/${brd_dt}`)
        .then(res => {
           this.setResponseData(res, 'normal');
      });
    }
  },
}
</script>