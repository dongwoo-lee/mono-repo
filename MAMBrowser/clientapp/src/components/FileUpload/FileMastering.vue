<template>
  <div id="file-container">
    <div
      v-show="processing"
      style="
        position: fixed;
        z-index: 9800;
        top: -80px;
        left: -80px;
        width: 2000px;
        height: 120%;
        background-color: rgba(0, 0, 0, 0.4);
        display: table;
      "
    >
      <div
        style="
          margin-top: 500px;
          margin-left: 140px;
          text-align: center;
          color: white;
          font-size: 48px;
        "
      >
        <b-spinner large type="grow"></b-spinner>
        <span class="label">파일 확인 중</span>
      </div>
    </div>
    <div
      id="dropzone-external"
      class="dropzone"
      v-show="dropzone"
      style="
        position: fixed;
        z-index: 9800;
        top: -80px;
        left: -80px;
        width: 2000px;
        height: 120%;
        background-color: rgba(0, 0, 0, 0.5);
        display: table;
      "
      :class="[
        isDropZoneActive
          ? 'dx-theme-accent-as-border-color dropzone-active'
          : 'dx-theme-border-color',
      ]"
    >
      <p
        style="
          margin-top: 500px;
          margin-left: 200px;
          text-align: center;
          color: white;
          font-size: 48px;
        "
      >
        음원 파일 업로드
      </p>
      <div id="dropzone-text" class="flex-box" style="margin-top: 80px">
        <span
          style="
            position: absolute;
            z-index: 1;
            color: white;
            margin-top: 700px;
            font-size: 48px;
          "
        >
        </span>
      </div>
    </div>
    <transition name="slide-fade">
      <CommonFileModal
        v-show="FileModal"
        @close="closeFileModal"
        style="font-family: 'Times New Roman', Times, serif; font-weight: bold"
      >
        <h2 slot="header">마스터링</h2>
        <h4 slot="body">
          <DxFileUploader
            :chunk-size="200000"
            dialog-trigger="#addFile"
            :upload-custom-data="uploaderCustomData"
            :ref="dxfu"
            name="file"
            drop-zone=".dropzone"
            @drop-zone-enter="onDropZoneEnter"
            @drop-zone-leave="onDropZoneLeave"
            :upload-url="getUrl"
            upload-mode="useButtons"
            :multiple="false"
            :visible="false"
            @valueChanged="valueChanged"
            @upload-started="() => (chunks = [])"
            @upload-error="uploadError"
            @uploaded="uploadSuccess"
            @progress="onUploadProgress($event)"
          />
          <div>
            <b-button
              style="
                position: absolute;
                top: 85px;
                right: 20px;
                border-color: silver;
                color: black;
                background-color: white;
                z-index: 9999;
              "
              class="btn btn-outline-primary btn-sm default cutom-label mr-2"
              id="addFile"
            >
              파일 업로드
            </b-button>
            <b-tabs content-class="mt-3" v-model="tabIndex">
              <b-tab title="작업목록">
                <b-card style="color: black">
                  <list v-if="this.tabIndex == 0 && this.FileModal"></list>
                </b-card>
              </b-tab>
              <b-tab title="로그">
                <b-card style="color: black">
                  <div
                    style="
                      width: 1300px;
                      height: 70px;
                      border: 1px solid #d7d7d7;
                      padding-top: 20px;
                    "
                  >
                    <b-form-group
                      label="시작일"
                      class="has-float-label"
                      style="
                        width: 200px;
                        height: 30px;
                        float: left;
                        margin-left: 20px;
                        margin-right: 20px;
                        font-size: 13px;
                      "
                    >
                      <b-input-group
                        class="mb-3"
                        style="width: 200px; float: left"
                      >
                        <input
                          style="height: 34px; font-size: 13px"
                          id="sdateinput"
                          type="text"
                          class="form-control input-picker date-input"
                          :value="logSDate"
                          @input="onsInput"
                        />
                        <b-input-group-append>
                          <b-form-datepicker
                            style="height: 34px"
                            v-model="logSDate"
                            button-variant="outline-dark"
                            button-only
                            right
                            aria-controls="example-input"
                            @context="onContext"
                          ></b-form-datepicker>
                        </b-input-group-append>
                      </b-input-group>
                    </b-form-group>
                    <b-form-group
                      label="종료일"
                      class="has-float-label"
                      style="width: 200px; font-size: 13px; float: left"
                    >
                      <b-input-group
                        class="mb-3"
                        style="width: 200px; float: left"
                      >
                        <input
                          style="height: 34px; font-size: 13px"
                          id="edateinput"
                          type="text"
                          class="form-control input-picker date-input"
                          :value="logEDate"
                          @input="oneInput"
                        />
                        <b-input-group-append>
                          <b-form-datepicker
                            style="height: 34px"
                            v-model="logEDate"
                            button-only
                            button-variant="outline-dark"
                            right
                            aria-controls="example-input"
                            @context="onContext"
                          ></b-form-datepicker>
                        </b-input-group-append>
                      </b-input-group>
                    </b-form-group>
                    <b-form-group
                      v-if="this.role == 'ADMIN'"
                      label="제작자"
                      class="has-float-label"
                      style="
                        width: 200px;
                        float: left;
                        margin-left: 20px;
                        font-size: 13px;
                      "
                    >
                      <common-vue-select
                        class="h145"
                        style="font-size: 14px; width: 200px"
                        :suggestions="editorOptions"
                        @inputEvent="onEditorSelected"
                      ></common-vue-select>
                    </b-form-group>
                    <b-button
                      style="
                        margin-top: -43px;
                        margin-left: 20px;
                        border-color: silver;
                        color: black;
                        background-color: white;
                        height: 34px;
                      "
                      class="btn btn-outline-primary btn-sm default cutom-label mr-2"
                      @click="logSearch"
                    >
                      검색
                    </b-button>
                  </div>

                  <div
                    style="
                      width: 1300px;
                      margin-top: 20px;
                      margin-left: auto;
                      margin-right: auto;
                      font-size: 14px;
                    "
                  >
                    <DxDataGrid
                      v-show="this.role == 'ADMIN'"
                      style="
                        height: 380px;
                        border: 1px solid silver;
                        font-family: 'MBC 새로움 M';
                      "
                      :data-source="masteringLogData"
                      :show-borders="false"
                      :hover-state-enabled="true"
                      key-expr="title"
                      :allow-column-resizing="true"
                      :column-auto-width="true"
                      no-data-text="No Data"
                      @row-expanding="logSearch"
                    >
                      <DxPager :visible="false" />
                      <DxScrolling mode="standard" />
                      <DxColumn
                        :width="500"
                        data-field="title"
                        caption="제목"
                      />
                      <DxColumn :width="90" data-field="type" caption="타입" />
                      <DxColumn
                        :width="110"
                        data-field="user"
                        caption="등록자"
                      />
                      <DxColumn :width="300" data-field="date" caption="날짜" />
                      <DxColumn
                        :width="50"
                        data-field="silence"
                        alignment="left"
                        caption="무음"
                      />
                      <DxColumn
                        :width="80"
                        data-field="worker"
                        caption="서버"
                      />
                      <DxColumn
                        :width="70"
                        data-field="status"
                        caption="상태"
                      />
                    </DxDataGrid>

                    <DxDataGrid
                      v-show="this.role != 'ADMIN'"
                      style="
                        height: 360px;
                        border: 1px solid silver;
                        font-family: 'MBC 새로움 M';
                      "
                      :data-source="masteringLogData"
                      :show-borders="false"
                      :hover-state-enabled="true"
                      key-expr="title"
                      :allow-column-resizing="true"
                      :column-auto-width="true"
                      no-data-text="No Data"
                    >
                      <DxPager :visible="false" />
                      <DxScrolling mode="standard" />
                      <DxColumn
                        :width="500"
                        data-field="title"
                        caption="제목"
                      />
                      <DxColumn :width="120" data-field="type" caption="타입" />
                      <DxColumn :width="350" data-field="date" caption="날짜" />
                      <DxColumn
                        :width="70"
                        data-field="silence"
                        caption="무음"
                      />
                      <DxColumn
                        :width="90"
                        data-field="worker"
                        caption="서버"
                      />
                      <DxColumn
                        :width="80"
                        data-field="status"
                        caption="상태"
                      />
                    </DxDataGrid>
                  </div>
                </b-card>
              </b-tab>
            </b-tabs>
          </div>
        </h4>
      </CommonFileModal>
    </transition>
    <MetaModal
      @upload="upload"
      @reset="reset"
      @cancel="fileUploadCancel"
      @close="MetaModalClose"
      :MetaModal="MetaModal"
      :fileState="fileState"
      :percent="percent"
    ></MetaModal>
  </div>
</template>

<script>
import CommonFileModal from "../Modal/CommonFileModal.vue";
import CommonFileFunction from "./CommonFileFunction";
import CommonVueSelect from "../Form/CommonVueSelect.vue";
import MetaModal from "./MetaModal";
import list from "./list.vue";
import axios from "axios";
const dxfu = "my-fileupload";
import { DxScrolling, DxLoadPanel, DxPager } from "devextreme-vue/data-grid";
var DB;
import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
export default {
  mixins: [CommonFileFunction],
  props: {
    DragFileModalState: {
      type: Boolean,
      default: false,
    },
  },
  components: {
    DxScrolling,
    DxLoadPanel,
    DxPager,
    CommonFileModal,
    CommonVueSelect,
    MetaModal,
    list,
  },
  data() {
    return {
      tabIndex: 0,
      dxfu,
      MetaModal: false,
      dropzone: false,
      isDropZoneActive: false,
      chunks: [],
      fileState: "",
      percent: 0,
      tempSDate: "",
      tempEDate: "",
      logSDate: "",
      logEDate: "",
      editorOptions: [],
      editorIdList: [],
      logEditor: "",
    };
  },
  watch: {
    DragFileModalState(v) {
      this.dropzone = v;
    },
    FileModal(v) {
      if (v) {
        this.tabIndex = 0;
      } else {
        this.tabIndex = 1;
      }
    },
    tabIndex(v) {
      if (v == 1 && this.FileModal) {
        this.logSearch();
      }
    },
  },
  created() {
    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.logEDate = today;
    this.tempEDate = today;

    var newDate = new Date();
    var dayOfMonth = newDate.getDate();
    newDate.setDate(dayOfMonth - 7);
    newDate = this.$fn.formatDate(newDate, "yyyy-MM-dd");

    this.logSDate = newDate;
    this.tempSDate = newDate;

    this.masteringStatus();

    axios.get("/api/Categories/users/pd").then((res) => {
      this.editorOptions = res.data.resultObject.data;
      this.editorOptions.unshift({ id: "", name: "전체 선택" });
    });

    var sdt = this.logSDate.replace(/-/g, "");
    var edt = this.logEDate.replace(/-/g, "");

    axios
      .get(
        `/api/Mastering/mastering-logs?startDt=${sdt}&endDt=${edt}&user_id=${sessionStorage.getItem(
          "user_id"
        )}`
      )
      .then((res) => {
        var masteringLogData = [];
        res.data.resultObject.data.forEach((e) => {
          var data = {
            title: e.title,
            type: this.getCategory(e.category),
            user: e.regUserName,
            date: e.regDtm,
            status: this.getStatus(e.workStatus),
            silence: e.silenceCount,
            worker: e.workerName,
          };
          masteringLogData.push(data);
        });
        this.setMasteringLogData(masteringLogData);
      });
  },
  computed: {
    fileupload: function () {
      return this.$refs[dxfu].instance;
    },
    ...mapGetters("user", ["diskAvailable"]),
    ...mapState("FileIndexStore", {
      uploaderCustomData: (state) => state.uploaderCustomData,
      localFiles: (state) => state.localFiles,
      masteringListData: (state) => state.masteringListData,
      MetaData: (state) => state.MetaData,
      FileModal: (state) => state.FileModal,
    }),
    getUrl() {
      if (this.MetaData.typeSelected == null) {
        return `/api/Mastering/`;
      } else {
        return `/api/Mastering/${this.MetaData.typeSelected}`;
      }
    },
  },
  methods: {
    ...mapMutations("FileIndexStore", [
      "addLocalFiles",
      "resetLocalFiles",
      "setFileUploading",
      "setMetaModalTitle",
      "setMasteringListData",
      "setMasteringLogData",
      "setDuration",
      "setAudioFormat",
      "setFileModal",
      "stopDBConnection",
    ]),
    onEditorSelected(data) {
      this.logEditor = data.id;
    },
    masteringStatus() {
      axios
        .get(
          `/api/Mastering/mastering-status?user_id=${sessionStorage.getItem(
            "user_id"
          )}`
        )
        .then((res) => {
          var masteringListData = [];
          res.data.resultObject.data.forEach((e) => {
            var data = {
              title: e.title,
              type: this.getCategory(e.category),
              user_id: e.regUserId,
              date: e.regDtm,
              step: e.workStatus,
            };
            masteringListData.push(data);
          });
          this.setMasteringListData(masteringListData);
        });
    },
    logSearch() {
      var user_id;

      this.editorOptions.forEach((e) => {
        this.editorIdList.push(e.id);
      });

      if (sessionStorage.getItem("authority") == "ADMIN") {
        if (this.logEditor == "") {
          user_id = this.logEditor;
        } else if (this.editorIdList.includes(this.logEditor)) {
          user_id = this.logEditor;
        } else {
          user_id = sessionStorage.getItem("user_id");
        }
      } else {
        user_id = sessionStorage.getItem("user_id");
      }
      var sdt = this.logSDate.replace(/-/g, "");
      var edt = this.logEDate.replace(/-/g, "");
      if (edt < sdt) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        return;
      }
      axios
        .get(
          `/api/Mastering/mastering-logs?startDt=${sdt}&endDt=${edt}&user_id=${user_id}`
        )
        .then((res) => {
          var masteringLogData = [];
          res.data.resultObject.data.forEach((e) => {
            var data = {
              title: e.title,
              type: this.getCategory(e.category),
              user: e.regUserName,
              date: e.regDtm,
              status: this.getStatus(e.workStatus),
              silence: e.silenceCount,
              worker: e.workerName,
            };
            masteringLogData.push(data);
          });
          this.setMasteringLogData(masteringLogData);
        });
    },
    getStatus(v) {
      if (v == 5) {
        return "성공";
      } else if (v == 6) {
        return "실패";
      }
    },
    get7daysago() {
      var newDate = new Date();
      var dayOfMonth = newDate.getDate();
      newDate.setDate(dayOfMonth - 7);
      newDate = this.$fn.formatDate(newDate, "yyyy-MM-dd");
      return newDate;
    },
    onsInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.tempSDate == null) {
          event.target.value = this.get7daysago();
          return;
        }
        event.target.value = this.tempSDate;
        return;
      }

      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);

          if (
            convertDate == "" ||
            convertDate == null ||
            convertDate == "undefined"
          ) {
            event.target.value = this.get7daysago();
            this.logSDate = this.get7daysago();
            this.tempSDate = this.get7daysago();
            return;
          }
          this.logSDate = convertDate;
          this.tempSDate = convertDate;
        }
      }
    },
    oneInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.tempEDate == null) {
          event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
          return;
        }
        event.target.value = this.tempEDate;
        return;
      }

      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          if (
            convertDate == "" ||
            convertDate == null ||
            convertDate == "undefined"
          ) {
            event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
            this.logEDate = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
            this.tempEDate = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
            return;
          }
          this.logEDate = convertDate;
          this.tempEDate = convertDate;
        }
      }
    },
    //#region 파일 조작
    upload() {
      this.fileState = "업로드 시작";
      this.fileupload.upload(0);
    },
    valueChanged(event) {
      this.setProcessing(true);
      this.resetLocalFiles();
      this.addLocalFiles(event.value[0]);
      if (event.value.length != 0) {
        this.setMetaModalTitle(event.value[0].name);
      }
      if (event.value.length != 0) {
        if (this.notDiskAvailable(event.value[0].size)) {
          if (
            event.value[0].type == "audio/mp3" ||
            event.value[0].type == "audio/wav"
          ) {
            var formData = new FormData();
            if (event.value[0].type == "audio/mp3") {
              var blob = event.value[0].slice(0, 10000);
              formData.append("file", blob);
              formData.append("fileExt", event.value[0].name);
            } else if (event.value[0].type == "audio/wav") {
              var blob = event.value[0].slice(0, 10000);
              formData.append("file", blob);
              formData.append("fileExt", event.value[0].name);
            }

            axios.post("/api/Mastering/Validation", formData).then((res) => {
              if (
                res.data.resultObject.duration == null ||
                res.data.resultObject.audioFormatInfo == null
              ) {
                this.$fn.notify("error", {
                  title: "잘못된 파일 형식입니다.",
                });
                return;
              }
              this.setDuration(res.data.resultObject.duration);
              this.setAudioFormat(res.data.resultObject.audioFormatInfo);
              this.openFileModal();
              this.dropzone = false;
              this.MetaModal = true;
              this.setProcessing(false);
              // this.setFileUploading(true);
            });
          } else {
            this.$fn.notify("error", {
              title: "오디오 파일만 업로드 가능합니다.",
            });
            this.fileupload.removeFile(0);
            this.setProcessing(false);
          }
        } else {
          this.$fn.notify("error", { title: "디스크 공간이 부족합니다." });
          this.fileupload.removeFile(0);

          this.setProcessing(false);
        }
      } else if (event.value.length == 0) {
        this.setProcessing(false);
      }
    },
    notDiskAvailable(files) {
      const result = this.diskAvailable - files;
      if (result < 0) {
        return false;
      } else {
        return true;
      }
    },
    MetaModalClose() {
      this.fileRemove();
      this.typeReset();
      this.percent = 0;
      this.MetaModal = false;
    },
    fileUploadCancel() {
      this.$fn.notify("error", { title: "파일 업로드 취소" });
      this.fileupload.abortUpload(0);
      this.resetLocalFiles();
      this.typeReset();
      this.percent = 0;
      this.MetaModal = false;
    },
    fileRemove() {
      this.fileupload.removeFile(0);
      this.resetLocalFiles();
    },
    uploadSuccess() {
      this.$fn.notify("primary", { message: "파일 업로드 성공" });
      this.fileState = "업로드 성공";
      this.percent = 0;
      this.setFileUploading(false);
      this.fileRemove();
    },
    uploadError(e) {
      console.log(e);
      this.setProcessing(false);
      this.setFileUploading(false);
      this.MetaModalClose();
      this.$fn.notify("error", {
        title: e.file.name,
        message: e.error.response,
        permanent: true,
      });
    },
    onUploadProgress(e) {
      this.percent = Math.ceil((e.bytesLoaded / e.bytesTotal) * 100);
    },
    //#endregion
    //#region 모달 조작
    openFileModal() {
      if (!this.FileModal) {
        this.tabIndex = 0;
      }
      this.setFileModal(true);
    },
    closeFileModal() {
      this.tabIndex = 1;
      this.setFileModal(false);
    },
    onDropZoneEnter(e) {
      if (e.dropZoneElement.id === "dropzone-external") {
        this.isDropZoneActive = true;
      }
    },
    onDropZoneLeave(e) {
      if (e.dropZoneElement.id === "dropzone-external") {
        this.isDropZoneActive = false;
        this.dropzone = false;
        this.$emit("dropZoneLeave");
      }
    },
    //#endregion
    getCategory(v) {
      switch (v) {
        case "MY":
          return "My 디스크";
        case "AC":
          return "프로소재";
        case "PM":
          return "프로그램";
        case "MS":
          return "주조SPOT";
        case "ST":
          return "부조SPOT";
        case "FC":
          return "FILLER";
        case "RC":
          return "취재물";
        case "TT":
          return "고정소재";
        case "TS":
          return "변동소재";
        default:
          return "";
      }
    },
  },
};
</script>
<style>
@import "./FileUploadCSS.css";
</style>
<style scoped>
.v-select .vs__dropdown-toggle {
  border: 1px solid white !important;
}
.myTableHeader {
  margin-left: 100px !important;
  height: 400px !important;
}
.card {
  width: 1350px;
  height: 530px;
  margin-left: 20px;
}
.date-input:focus {
  border: 1px solid #4475c4 !important;
}
.date-input {
  border: 1px solid silver !important;
}
.media-select {
  border: 1px solid silver !important;
}
.media-select:focus {
  border: 1px solid #4475c4 !important;
}
</style>
