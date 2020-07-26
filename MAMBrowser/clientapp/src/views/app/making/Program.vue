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
            <!-- 채널 -->
            <b-colxx sm="2">
              <b-form-group label="채널">
                <b-form-select v-model="searchItems.media" :options="mediaOptions"></b-form-select>
              </b-form-group>
            </b-colxx>
            <!-- 방송일 -->
            <b-colxx sm="2">
              <b-form-group label="방송일" class="inline-block">
                <c-input-date-picker v-model="searchItems.brd_dt" />
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
            dataClass: 'right aligned'
        },
        {
          name: "mediaName",
          title: "매체",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "name",
          title: "프로그램",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "brdDT",
          title: "방송일",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "brdTime",
          title: "방송시간",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "status",
          title: "상태",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "duration",
          title: "길이",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "track",
          title: "트랙",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "editorID",
          title: "제작자",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "editorName",
          title: "편집일시",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "editDtm",
          title: "방송의뢰일시",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "reqCompleteDtm",
          title: "마스터링",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "filePath",
          title: "파일경로",
          titleClass: "",
          dataClass: "list-item-heading",
        },
      ],
      contextMenu: [
        { name: 'download', text: '다운로드' },
        { name: 'storage', text: '내 저장공간으로 복사' },
      ]
    }
  },
  methods: {
    getData() {
      const media = this.searchItems.media;
      const brd_dt = this.searchItems.brd_dt;

      this.$http.get(`/api/Products/pgm/${media}/${brd_dt}`)
        .then(res => {
           this.setResponseData(res, 'normal');
      });
    }
  }
}
</script>