<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb heading="효과음"/>
      <div class="separator mb-5"></div>
    </b-colxx>
  </b-row>
  <b-row>
    <b-colxx xxs="12">
      <b-card class="mb-4">
        <b-form @submit.stop>
          <b-row>
            <!-- 키워드 검색 -->
            <b-colxx sm="2">
              <b-form-group label="검색어" class="has-float-label">
                <c-input-text v-model="searchItems.name"/>
              </b-form-group>
            </b-colxx>
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
        searchWord: '',
        rowPerPage: 16,
        selectPage: 1,
        sortKey: '',
        sortValue: 'DESC',
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
          name: "name",
          title: "제목",
          titleClass: 'center aligned',
          dataClass: "center aligned",
        },
        {
          name: "description",
          title: "설명",
          titleClass: 'center aligned',
          dataClass: "center aligned",
        },
        {
          name: "name",
          title: "길이",
          titleClass: 'center aligned',
          dataClass: "center aligned",
        },
        {
          name: "duration",
          title: "음반",
          titleClass: 'center aligned',
          dataClass: "center aligned",
        },
        {
          name: "audioFormat",
          title: "오디오 포맷",
          titleClass: 'center aligned',
          dataClass: "center aligned",
        },
        {
          name: "filePath",
          title: "파일 경로",
          titleClass: 'center aligned',
          dataClass: "center aligned",
        },
      ]
    }
  },
  methods: {
    getData() {
      this.$http.get(`/api/Products/effect`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
      });
    }
  }
}
</script>