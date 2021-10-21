<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="아카이브(DL)" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      :isDisplayPageSize="false"
      @changeRowPerpage="onChangeRowPerpage"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 송출일시 -->
        <b-form-group
          label="송출일시"
          class="has-float-label"
          :class="{ hasError: $v.searchItems.regDtm.required }"
        >
          <common-date-picker
            v-model="$v.searchItems.regDtm.$model"
            @input="searchCheck"
            required
          />

          <b-form-invalid-feedback :state="!$v.searchItems.regDtm.required"
            >날짜는 필수 입력입니다.</b-form-invalid-feedback
          >
        </b-form-group>
        <!-- 단말 -->
        <b-form-group label="단말" class="has-float-label">
          <b-form-select
            class="width-120"
            v-model="searchItems.dlDeviceSeq"
            :options="dlDeviceOptions"
            value-field="id"
            text-field="name"
            @change="searchCheck"
          />
        </b-form-group>
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            class="width-120"
            v-model="searchItems.media"
            :options="mediaOptions"
            value-field="id"
            text-field="name"
            @change="searchCheck"
          />
        </b-form-group>
        <!-- 녹음소재명 -->
        <b-form-group label="녹음소재명" class="has-float-label c-zindex">
          <common-input-text
            @inputEnterEvent="searchCheck"
            v-model="searchItems.pgmName"
          />
        </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="searchCheck"
            >검색</b-button
          >
        </b-form-group>
      </template>
      <!-- 테이블 페이지 -->
      <template slot="form-table-page-area">
        {{ getTotalRowCount() }}
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <b-row>
          <b-colxx xs="12" md="12" class="no-r-p">
            <b-table
              style="height:580px"
              class="custom-table non-h"
              ref="custom-table"
              thead-class="custom-table-color"
              sort-by="title"
              sort-desc.sync="false"
              selectable
              sticky-header
              responsive
              show-empty
              empty-text="데이터가 없습니다."
              :busy="isTableLoading"
              select-mode="single"
              selectedVariant="primary"
              :fields="fields"
              :items="responseData.data"
            >
              <template v-slot:cell(index)="data">
                {{ data.index + 1 }}
              </template>

              <template v-slot:table-busy>
                <div class="text-center text-primary my-2">
                  <b-spinner class="align-middle"></b-spinner>
                  <strong>Loading...</strong>
                </div>
              </template>
              <template v-slot:cell(actions)="data">
                <common-actions
                  :rowData="data.item"
                  :behaviorData="behaviorList"
                  @preview="onPreview"
                  @download="onDownloadDl30"
                  @mydiskCopy="onCopyToMySpacePopup"
                />
              </template>
            </b-table>
          </b-colxx>
        </b-row>
      </template>
    </common-form>

    <CopyToMySpacePopup
      ref="refCopyToMySpacePopup"
      :show="copyToMySpacePopup"
      @ok="onMyDiskCopyFromDl30"
      @close="copyToMySpacePopup = false"
    >
    </CopyToMySpacePopup>
    <PlayerPopup
      :showPlayerPopup="showPlayerPopup"
      :title="soundItem.recName"
      :fileKey="soundItem.seq"
      :streamingUrl="streamingUrl"
      :waveformUrl="waveformUrl"
      :tempDownloadUrl="tempDownloadUrl"
      requestType="key"
      direct="Y"
      @closePlayer="onClosePlayer"
    >
    </PlayerPopup>
  </div>
</template>

<script>
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CopyToMySpacePopup from "../../../components/Popup/CopyToMySpacePopup";
export default {
  components: { CopyToMySpacePopup },
  mixins: [MixinBasicPage],
  data() {
    return {
      streamingUrl: "/api/Products/dl30-streaming",
      waveformUrl: "/api/Products/dl30-waveform",
      tempDownloadUrl: "/api/Products/dl30-temp-download",
      searchItems: {
        media: "A", // 매체
        regDtm: new Date().toISOString().substring(0, 10), // 편성일자
        dlDeviceSeq: 0,
        pgmName: "", // 녹음명
        rowPerPage: 30,
        selectPage: 1,
        sortKey: "",
        sortValue: ""
      },
      isTableLoading: false,
      fields: [
        {
          key: "index",
          label: "순서",
          tdClass: "list-item-heading",
          thClass: "text-center",
          tdClass: "text-center",
          thStyle: { width: "4%" }
        },
        {
          key: "brdDate",
          label: "송출일시",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center bold",
          thStyle: { width: "15%" }
        },
        {
          key: "recName",
          label: "녹음소재명",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center bold"
        },
        {
          key: "sourceID",
          label: "Source ID",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
          thStyle: { width: "7%" }
        },
        {
          key: "duration",
          label: "녹음분량",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
          thStyle: { width: "10%" }
        },
        {
          key: "fileSize",
          label: "파일사이즈",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
          thStyle: { width: "10%" },
          formatter: v => {
            return this.$fn.formatBytes(v);
          }
        },
        {
          key: "regDtm",
          label: "등록일시",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
          thStyle: { width: "15%" }
        },
        {
          key: "actions",
          label: "추가작업",
          thClass: "text-center",
          tdClass: "text-center",
          thStyle: { width: "6%" }
        }
      ],
      dlDeviceOptions: []
    };
  },
  created() {
    this.getMediaOptions();
    this.getDlDeviceOptions();
  },
  methods: {
    searchCheck() {
      if (this.searchItems.dlDeviceSeq != 0) {
        this.onSearch();
      }
    },
    getData() {
      if (!this.$v.searchItems.regDtm.$invalid) {
        this.$fn.notify("inputError", {});
        return;
      }

      this.isTableLoading = true;
      const media = this.searchItems.media;
      const regDtm = this.searchItems.regDtm;

      this.$http
        .get(`/api/products/dl30/${media}/${regDtm}`, {
          params: this.searchItems
        })
        .then(res => {
          this.setResponseData(res, "normal");
          this.isTableLoading = false;
        });
    },
    // 매체목록 조회
    async getDlDeviceOptions() {
      await this.requestCall(
        "/api/Categories/dldevice-list",
        "dlDeviceOptions"
      );
      if (this.dlDeviceOptions.length > 0) {
        this.searchItems.dlDeviceSeq = this.dlDeviceOptions[0].id;
        this.getData();
      }
    }
  }
};
</script>
<style scope>
.b-table-sticky-header > .table.b-table > thead > tr > th {
  font-weight: 400 !important;
}
</style>
