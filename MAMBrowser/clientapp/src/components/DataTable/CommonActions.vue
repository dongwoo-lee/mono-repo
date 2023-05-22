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
    onPreview() {
      this.$emit("preview", this.rowData);
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
  padding: 1px 10px;
  margin: 0px 5px;
}
</style>
