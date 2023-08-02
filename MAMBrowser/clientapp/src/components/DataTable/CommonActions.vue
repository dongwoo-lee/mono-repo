<template>
  <div class="common-actions">
    <b-button
      v-if="display(PREVIEW_CODE)"
      class="icon-buton"
      :disabled="!existFile()"
      :style="getDownloadStyle()"
      v-b-tooltip.hover.top="{
        title:
          isFilePathPreviewTitle && IS_ADMIN ? rowData.filePath : '미리듣기',
      }"
      @click.stop="onPreview()"
    >
      <b-icon icon="caret-right-square" class="icon"></b-icon>
    </b-button>
    <b-button
      v-if="display(DOWNLOAD_CODE)"
      :id="`download-${rowData.rowNO}`"
      :disabled="!existFile()"
      :style="getDownloadStyle()"
      class="icon-buton"
      v-b-tooltip.hover.top="{
        title: IS_ADMIN ? rowData.filePath : '다운로드',
      }"
      @click.stop="onDownload()"
    >
      <b-icon icon="download" class="icon"></b-icon>
    </b-button>
    <b-button
      v-if="displayEtc('modify')"
      class="icon-buton"
      :disabled="!isPossibleUpdate"
      :style="getUpdateStyle()"
      :title="getTitle('modify')"
      @click.stop="onMetaModify()"
    >
      <b-icon icon="pencil-square" class="icon" variant="info"></b-icon>
    </b-button>
    <b-button
      v-if="displayEtc('delete')"
      class="icon-buton"
      :title="getTitle('delete')"
      :disabled="!isPossibleDelete"
      @click.stop="onDelete()"
    >
      <b-icon icon="trash" class="icon" variant="danger"></b-icon>
    </b-button>
    <b-button
      v-if="displayMyDiskCopy()"
      class="icon-buton"
      :disabled="!existFile()"
      :title="getTitle('mydisk-copy')"
      @click.stop="onMyDiskCopy()"
    >
      <i class="iconsminds-shop i-custom-actions-shop" />
    </b-button>
    <b-button
      v-if="rowData.r_ONAIRTIME && rowData.cueid == -1 && !oldVal"
      id="cueBtn"
      variant="outline-primary"
      @click="getCueData('day')"
      >큐시트 작성</b-button
    >
    <b-button
      v-if="rowData.r_ONAIRTIME && rowData.cueid > 0"
      id="cueBtn_update"
      variant="success"
      @click="getCueData('day')"
      >큐시트 수정</b-button
    >
    <b-button
      v-if="rowData.cuetype == 'B'"
      id="cueBtn"
      variant="outline-primary"
      @click="getCueData('week')"
      >기본큐시트 작성</b-button
    >
    <b-button
      v-if="rowData.cuetype == 'T'"
      id="cueBtn"
      variant="outline-primary"
      @click="getCueData('template')"
      >템플릿 작성</b-button
    >
    <b-button
      v-if="rowData.cuetype == 'A'"
      id="cueBtn"
      variant="outline-primary"
      @click="getCueData('previous')"
      >큐시트 조회</b-button
    >
    <b-button
      v-if="configActions.includes('modify')"
      class="config_btn"
      variant="outline-primary"
      @click="modifyConfigRowData()"
      >편집</b-button
    >
    <b-button
      v-if="configActions.includes('download')"
      :disabled="rowData.masterfile ? false : true"
      class="config_btn"
      variant="success"
      @click="downloadConfigRowData()"
    >
      다운로드</b-button
    >
    <b-button
      v-if="configActions.includes('delete')"
      class="config_btn"
      variant="danger"
      @click="deleteConfigRowData()"
    >
      삭제</b-button
    >
    <span id="broadcast_btn">
      <b-dropdown
        v-if="isBroadcastConfigAction"
        :disabled="!isDropdownStatus(rowData)"
      >
        <template #button-content> 미리듣기 </template>
        <b-dropdown-item
          v-if="rowData.pgmfilepath"
          @click.stop="onPreview('pgm')"
          >PGM</b-dropdown-item
        >
        <b-dropdown-item v-if="rowData.dlfilepatH_1" @click="onPreview('dl_1')"
          >DL (주)</b-dropdown-item
        >
        <b-dropdown-item v-if="rowData.dlfilepatH_2" @click="onPreview('dl_2')"
          >DL (예비)</b-dropdown-item
        >
      </b-dropdown>
      <b-dropdown
        v-if="isBroadcastConfigAction"
        :disabled="!isDropdownStatus(rowData)"
      >
        <template #button-content>
          다운로드
          <!-- <b-icon icon="play"></b-icon> -->
        </template>
        <b-dropdown-item
          v-if="rowData.pgmfilepath"
          @click="
            downloadFileAction(rowData.pgmfilepath, 'PGM_' + rowData.eventname)
          "
          >PGM</b-dropdown-item
        >
        <b-dropdown-item
          v-if="rowData.dlfilepatH_1"
          @click="
            downloadFileAction(
              rowData.dlfilepatH_1,
              'DL_주' + rowData.eventname
            )
          "
          >DL (주)</b-dropdown-item
        >
        <b-dropdown-item
          v-if="rowData.dlfilepatH_2"
          @click="
            downloadFileAction(
              rowData.dlfilepatH_2,
              'DL_예비' + rowData.eventname
            )
          "
          >DL (예비)</b-dropdown-item
        >
      </b-dropdown>
      <b-button
        v-if="isBroadcastConfigAction || isProgramInfoConfigAction"
        :disabled="!rowData.cueid"
        variant="outline-primary"
        size="sm"
        title="큐시트"
        @click="getCueListAndData()"
      >
        <b-icon icon="receipt-cutoff" class="icon"></b-icon
      ></b-button>
      <b-button
        v-if="isBroadcastConfigAction"
        :disabled="!rowData.sourceid"
        variant="outline-primary"
        size="sm"
        title="선곡리스트"
        @click="getMusicSelectionList()"
      >
        <b-icon icon="music-note-list" class="icon"></b-icon
      ></b-button>
    </span>
  </div>
</template>
<script>
import {
  PREVIEW_CODE,
  DOWNLOAD_CODE,
  AUTHORITY,
  AUTHORITY_ADMIN,
  ROLE,
  MY_DISK_PAGE_ID,
  ROUTE_NAMES,
} from "@/constants/config";
import { mapActions, mapGetters, mapMutations } from "vuex";
import { USER_ID } from "@/constants/config";
import "moment/locale/ko";
const moment = require("moment");
export default {
  props: {
    rowData: {
      type: Object,
      default: () => {},
    },
    etcData: {
      type: Array,
      default: () => [], // ['delete', 'modify']
    },
    etcTitles: {
      type: Object,
      default: () => {}, // {'delete': '삭제', 'modify': '정보편집'}
    },
    behaviorData: {
      type: Array,
      default: () => [],
    },
    isPossibleDelete: {
      type: Boolean,
      default: true,
    },
    isPossibleUpdate: {
      type: Boolean,
      default: true,
    },
    isFilePathPreviewTitle: {
      type: Boolean,
      default: false,
    },
    downloadName: {
      type: String,
      default: "",
    },
    oldVal: {
      type: Boolean,
      default: false,
    },
    configActions: {
      type: Array,
      default: () => [],
    },
    isBroadcastConfigAction: {
      type: Boolean,
      default: false,
    },
    isProgramInfoConfigAction: {
      type: Boolean,
      defalut: false,
    },
  },
  data() {
    return {
      PREVIEW_CODE,
      DOWNLOAD_CODE,
      MY_DISK_PAGE_ID,
      ROUTE_NAMES,
      currentPageName: "",
      IS_ADMIN: sessionStorage.getItem(AUTHORITY) === AUTHORITY_ADMIN,
    };
  },
  computed: {
    ...mapGetters("user", ["roleList", "isSystemTopAdmin"]),
  },
  mounted() {},
  watch: {
    $route: {
      handler(to, from) {
        this.currentPageName = this.$route.name;
      },
      immediate: true,
    },
  },
  methods: {
    ...mapMutations("cueList", ["SET_CUEINFO"]),
    logout() {
      this.SET_LOGOUT();
      this.$router.push("/user/Login");
    },
    display(value) {
      return this.behaviorData.some(
        (data) => data.id === value && data.visible === "Y"
      );
    },
    displayMyDiskCopy() {
      if (this.configActions.length != 0) {
        return false;
      }
      const exceptPageNames = [
        this.ROUTE_NAMES.PRIVATE,
        this.ROUTE_NAMES.WASTE_BASKET,
      ];
      if (
        exceptPageNames.includes(this.currentPageName) ||
        this.rowData.cueid != undefined ||
        this.rowData.detail ||
        this.rowData.addDate ||
        this.rowData.broadcastStatus
      ) {
        return false;
      }
      return this.roleList.some(
        (data) => data.id === this.MY_DISK_PAGE_ID && data.visible === "Y"
      );
    },
    displayEtc(value) {
      return this.etcData.some((data) => data === value);
    },
    onPreview(key) {
      const resultData = { ...this.rowData };
      switch (key) {
        case "dl_1":
          resultData.fileToken = resultData.dlfiletokeN_1;
          this.$emit("preview", resultData);
          break;
        case "dl_2":
          resultData.fileToken = resultData.dlfiletokeN_2;
          this.$emit("preview", resultData);
          break;
        case "pgm":
          resultData.fileToken = resultData.pgmfiletoken;
          this.$emit("preview", resultData);
          break;

        default:
          this.$emit("preview", this.rowData);
          break;
      }
    },
    onDownload() {
      this.$emit("download", this.rowData, this.downloadName);
    },
    onDelete() {
      this.$emit("MasteringDelete", this.rowData);
      if (!this.isPossibleDelete || !this.isSystemTopAdmin) {
        return;
      }

      this.$emit("delete", this.rowData);
    },
    onMetaModify() {
      this.$emit("modify", this.rowData);
    },
    onMyDiskCopy() {
      if (!this.existFile()) {
        return;
      }
      this.$emit("mydiskCopy", this.rowData);
    },
    getTitle(type) {
      if (this.etcTitles && this.etcTitles[type]) {
        return this.etcTitles[type];
      }
      if (type === "delete") {
        return "휴지통";
      }
      if (type === "modify") {
        return "정보편집";
      }
      if (type === "mydisk-copy") {
        return "My 공간으로 복사";
      }
      return "";
    },
    getDeleteStyle() {
      return {
        opacity: this.isPossibleDelete ? 1 : 0.2,
      };
    },
    getUpdateStyle() {
      return {
        opacity: this.isPossibleUpdate ? 1 : 0.2,
      };
    },
    existFile() {
      return this.rowData.existFile;
    },
    getDownloadStyle() {
      return {
        opacity: this.rowData.existFile ? 1 : 0.2,
      };
    },
    //큐시트 USER_INFO set
    getCueData(V) {
      sessionStorage.setItem("USER_INFO", JSON.stringify(this.rowData));
      this.$router.push({ path: "/app/cuesheet/" + V + "/detail" });
    },
    modifyConfigRowData() {
      this.$emit("modifyConfigRowData", this.rowData);
    },
    deleteConfigRowData() {
      this.$emit("deleteConfigRowData", this.rowData);
    },
    downloadConfigRowData() {
      this.$emit("downloadConfigRowData", this.rowData);
    },
    downloadFileAction(filePath, fileName) {
      this.$http
        .get(
          `/api/managementdeleteproducts/RecycleFileDownload?guid=${filePath}&userid=${sessionStorage.getItem(
            USER_ID
          )}&downloadName=${fileName}`
        )
        .then((res) => {
          if (res.status === 200) {
            const link = document.createElement("a");
            link.href = `/api/managementdeleteproducts/RecycleFileDownload?guid=${filePath}&userid=${sessionStorage.getItem(
              USER_ID
            )}&downloadName=${fileName}`;
            document.body.appendChild(link);
            link.click();
          }
        });
    },
    getCueListAndData() {
      this.$emit("goCueSheetDate", this.rowData);
    },
    isDropdownStatus(rowData) {
      return (
        rowData.dlfilepatH_1 || rowData.dlfilepatH_2 || rowData.pgmfilepath
      );
    },
    getMusicSelectionList() {
      this.$emit("clickMusicSelectionListBtn");
    },
  },
};
</script>
<style>
#cueBtn_update {
  border-color: #28a745;
  color: #28a745;
}
#cueBtn {
  /* color: rgb(0, 110, 229);
  border-color: #007bff; */
}
#cueBtn:active,
#cueBtn_update:active {
  color: white;
  opacity: 0.8;
}
.config_btn {
  padding: 1px 10px !important;
  margin: 0px 5px;
}
#broadcast_btn button:disabled:hover {
  pointer-events: none;
}
#broadcast_btn button:disabled {
  color: #d7d7d7 !important;
  border-color: #d7d7d7 !important;
}
</style>
