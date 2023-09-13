<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="SONG" tooltip="Song" />
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
        <!-- 검색어 -->
        <b-form-group label="음반명" class="has-float-label">
          <common-input-text
            v-model="searchItems.albumName"
            @inputEnterEvent="onSearch"
          />
        </b-form-group>
        <b-form-group label="아티스트" class="has-float-label">
          <common-input-text
            v-model="searchItems.artistName"
            @inputEnterEvent="onSearch"
          />
        </b-form-group>
        <b-form-group label="곡명" class="has-float-label">
          <common-input-text
            v-model="searchItems.title"
            @inputEnterEvent="onSearch"
          />
        </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >검색</b-button
          >
        </b-form-group>
      </template>
      <!-- 테이블 페이지 -->
      <template slot="form-table-page-area">
        {{ getPageInfo() }}
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <common-data-table-scroll-paging
          ref="scrollPaging"
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          :totalCount="responseData.totalRowCount"
          is-actions-slot
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @selectedIds="onSelectedIds"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :behaviorData="behaviorList"
              @preview="onPreview"
              @download="onDownloadProduct"
              @mydiskCopy="onCopyToMySpacePopup"
            >
            </common-actions>
          </template>
        </common-data-table-scroll-paging>

        <CopyToMySpacePopup
          ref="refCopyToMySpacePopup"
          :show="copyToMySpacePopup"
          :MySpaceScreenName="MySpaceScreenName"
          @ok="onMyDiskCopyFromProduct"
          @close="copyToMySpacePopup = false"
        >
        </CopyToMySpacePopup>
      </template>
    </common-form>
    <SongPlayerPopup
      :showPlayerPopup="showPlayerPopup"
      :music="soundItem"
      :streamingUrl="streamingUrl"
      :waveformUrl="waveformUrl"
      :tempDownloadUrl="tempDownloadUrl"
      requestType="token"
      @closePlayer="onClosePlayer"
    >
    </SongPlayerPopup>

    <sound-copyright-popup
      :show="soundCopyrightPopup"
      @close="soundCopyrightPopup = false"
      @agree="onAgree"
    />
  </div>
</template>

<script>
import MixinMusicPage from "../../../mixin/MixinMusicPage";
import SoundCopyrightPopup from "@/components/Popup/SoundCopyrightPopup";
import CopyToMySpacePopup from "../../../components/Popup/CopyToMySpacePopup";
import { mapActions } from "vuex";

export default {
  mixins: [MixinMusicPage],
  components: { SoundCopyrightPopup, CopyToMySpacePopup },
  data() {
    return {
      streamingUrl: "/api/products/streaming",
      waveformUrl: "/api/products/waveform",
      tempDownloadUrl: "/api/products/temp-download",
      MySpaceScreenName: "[SONG]",
      allSelected: false,
      indeterminate: false,

      searchItems: {
        albumName: "",
        artistName: "",
        title: "",
        rowPerPage: 100,
        selectPage: 1,
        sortKey: "",
        sortValue: "",
      },
      soundCopyrightPopup: false,
      copyrightItem: null,
      isTableLoading: false,
      fields: [
        {
          name: "rowNO",
          title: "순서",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "5%",
        },
        {
          name: "name",
          title: "곡명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "15%",
        },
        {
          name: "artistName",
          title: "아티스트",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "12%",
        },
        {
          name: "albumName",
          title: "음반명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "12%",
        },
        {
          name: "duration",
          title: "길이",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
        },
        {
          name: "musicID",
          title: "Music ID",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
        },
        {
          name: "encodeDate",
          title: "인코딩일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
        },
        {
          name: "__slot:actions",
          title: "추가작업",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%",
        },
      ],
    };
  },

  methods: {
    ...mapActions("file", ["downloadProduct"]),
    getData() {
      this.isTableLoading = this.isScrollLodaing ? false : true;
      this.$http
        .get(`/api/Products/song`, { params: this.searchItems })
        .then((res) => {
          this.setResponseData(res);
          this.addScrollClass();
          this.isTableLoading = false;
          this.isScrollLodaing = false;
        });
    },
    toggleAll(checked) {
      this.selectedSearchType1 = checked ? [1, 2, 4] : [];
    },
    onDownloadProduct(item) {
      this.soundCopyrightPopup = true;
      this.copyrightItem = item;
    },
    onAgree() {
      this.downloadProduct({
        item: this.copyrightItem,
        downloadName: this.downloadName(this.copyrightItem),
      });
      this.soundCopyrightPopup = false;
    },
    downloadName(rowData) {
      var tmpName = `${rowData.name}-${rowData.artistName}`;
      return tmpName;
    },
  },
};
</script>
