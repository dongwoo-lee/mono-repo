<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="효과음" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      :isDisplayBtnArea="true"
      :isDisplayPageSize="false"
      @changeRowPerpage="onChangeRowPerpage"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <b-form-group label="검색어" class="has-float-label">
          <common-input-text v-model="searchItems.searchText"/>
        </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch">검색</b-button>
        </b-form-group>
      </template>
      <!-- 테이블 페이지 -->
      <template slot="form-table-page-area">
        {{ getTotalRowCount() }}
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <common-data-table
          :fields="fields"
          :rows="responseData.data"
          :isTableLoading="isTableLoading"
          is-actions-slot
        >
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :downloadName="downloadName(props.props.rowData)"
              :behaviorData="behaviorList"
              @preview="onPreview"
              @download="onDownloadMusic"
              @mydiskCopy="onCopyToMySpacePopup"
            >
            </common-actions>
          </template>
        </common-data-table>

         <CopyToMySpacePopup
          ref="refCopyToMySpacePopup"
          :show="copyToMySpacePopup"
          @ok="onMyDiskCopyFromMusic"
          @close="copyToMySpacePopup = false">
        </CopyToMySpacePopup>
      </template>
    </common-form>

     <PlayerPopup 
    :showPlayerPopup="showPlayerPopup"
    :title="soundItem.name"
    :fileKey="soundItem.fileToken"
    :streamingUrl="streamingUrl"
    :waveformUrl="waveformUrl"
    :tempDownloadUrl="tempDownloadUrl"
    requestType="token"
    @closePlayer="onClosePlayer">
    </PlayerPopup>

  </div>
</template>

<script>
import MixinMusicPage from '../../../mixin/MixinMusicPage';
import CopyToMySpacePopup from "../../../components/Popup/CopyToMySpacePopup";
export default {
  components:{CopyToMySpacePopup},
  mixins: [ MixinMusicPage ],
  data() {
    return {
      streamingUrl : '/api/musicsystem/streaming',
      waveformUrl : '/api/musicsystem/waveform',
     tempDownloadUrl : '/api/musicsystem/temp-download',

      searchItems: {
        searchText: '',
        rowPerPage: 30,
        selectPage: 1,
        sortKey: '',
        sortValue: 'DESC',
      },
      isTableLoading: false,
      fields: [
        {
          name: 'rowNO',
          title: '순서',
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '4%',
        },
        {
          name: "name",
          title: "효과음명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
        },
        {
          name: "description",
          title: "설명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "duration",
          title: "길이(초)",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
        },
        {
          name: "audioFormat",
          title: "오디오 포맷",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: '__slot:actions',
          title: '추가작업',
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%"
        }
      ]
    }
  },
  methods: {
    getData() {
      this.isTableLoading = this.isScrollLodaing ? false: true;
      this.$http.get(`/api/musicsystem/effect`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res, 'normal');
            this.isTableLoading = false;
      });
    },
    downloadName(rowData) {
      var tmpName = `${rowData.name}`;
      return tmpName;
    },
  }
}
</script>