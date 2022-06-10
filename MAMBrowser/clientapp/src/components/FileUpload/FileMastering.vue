<template>
  <div id="file-container">
    <div
      v-show="processing"
      style="
        position: fixed;
        z-index: 9800;
        top: 0px;
        left: 0px;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.4);
        display: table;
      "
    >
      <div
        style="
          margin-top: 23%;
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
      @dragleave="dragLeave"
      id="dropzone-external"
      class="dropzone"
      v-show="dropzone"
      style="
        position: fixed;
        z-index: 9800;
        top: 0px;
        left: 0px;
        width: 100%;
        height: 100%;
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
          margin: auto;
          margin-top: 450px;
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
        <h2 slot="header">방송의뢰</h2>
        <h4 slot="body">
          <!--
           -->
          <DxFileUploader
            :chunk-size="chunkSize"
            :upload-custom-data="uploaderCustomData"
            :ref="dxfu"
            name="file"
            :accept="accept"
            :allowed-file-extensions="['.mp3', '.wav']"
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
              @click="select"
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
                            :value="logSDate"
                            @input="eventSInput"
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
                            :value="logEDate"
                            @input="eventEInput"
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
                      ref="logTable"
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
                      :allow-column-resizing="false"
                      :column-auto-width="true"
                      no-data-text="No Data"
                    >
                      <DxScrolling mode="virtual" />
                      <DxColumn
                        :width="500"
                        data-field="title"
                        caption="제목"
                      />
                      <DxColumn
                        :width="120"
                        data-field="type"
                        caption="소재유형"
                      />
                      <DxColumn
                        :width="120"
                        data-field="user"
                        caption="등록자"
                      />
                      <DxColumn :width="200" data-field="date" caption="날짜" />
                      <DxColumn
                        :width="50"
                        data-field="silence"
                        alignment="left"
                        caption="무음"
                      />
                      <DxColumn
                        :width="100"
                        data-field="worker"
                        caption="서버"
                      />
                      <DxColumn
                        :width="80"
                        data-field="status"
                        caption="상태"
                      />
                      <DxColumn :width="120" data-field="note" caption="비고" />
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
                      :allow-column-resizing="false"
                      :column-auto-width="true"
                      no-data-text="No Data"
                    >
                      <DxScrolling mode="virtual" />
                      <DxColumn
                        :width="530"
                        data-field="title"
                        caption="제목"
                      />
                      <DxColumn :width="120" data-field="type" caption="타입" />
                      <DxColumn :width="300" data-field="date" caption="날짜" />
                      <DxColumn
                        :width="50"
                        data-field="silence"
                        caption="무음"
                      />
                      <DxColumn
                        :width="120"
                        data-field="worker"
                        caption="서버"
                      />
                      <DxColumn
                        :width="80"
                        data-field="status"
                        caption="상태"
                      />
                      <DxColumn :width="80" data-field="note" caption="비고" />
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
import {
  DxDataGrid,
  DxColumn,
  DxSelection,
  DxLoadPanel,
} from "devextreme-vue/data-grid";
import DxFileUploader from "devextreme-vue/file-uploader";
import DxValidator from "devextreme-vue/validator";
import DxTextBox from "devextreme-vue/text-box";
import Vuetable from "vuetable-2/src/components/Vuetable";
import { DxScrolling } from "devextreme-vue/data-grid";
var DB;
import { mapState, mapGetters, mapMutations } from "vuex";
export default {
  mixins: [CommonFileFunction],
  props: {
    DragFileModalState: {
      type: Boolean,
      default: false,
    },
  },
  components: {
    DxDataGrid,
    DxColumn,
    DxSelection,
    DxFileUploader,
    DxTextBox,
    DxValidator,
    Vuetable,
    DxScrolling,
    DxLoadPanel,
    CommonFileModal,
    CommonVueSelect,
    MetaModal,
    list,
  },
  data() {
    return {
      chunkSize: 1000000,
      accept: "audio/mpeg,audio/wav",
      tabIndex: 0,
      dxfu,
      MetaModal: false,
      processing: false,
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
  async created() {
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

    var res = await axios.get("/api/Categories/users/pd");

    this.editorOptions = res.data.resultObject.data;
    this.editorOptions.unshift({ id: "", name: "전체 선택" });

    var sdt = this.logSDate.replace(/-/g, "");
    var edt = this.logEDate.replace(/-/g, "");

    var res = await axios.get(
      `/api/Mastering/mastering-logs?startDt=${sdt}&endDt=${edt}&user_id=${sessionStorage.getItem(
        "user_id"
      )}`
    );

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
  },
  computed: {
    fileupload: function () {
      return this.$refs[dxfu].instance;
    },
    ...mapGetters("user", ["diskAvailable"]),
    ...mapState("FileIndexStore", {
      MetaModalTitle: (state) => state.MetaModalTitle,
      uploaderCustomData: (state) => state.uploaderCustomData,
      localFiles: (state) => state.localFiles,
      masteringListData: (state) => state.masteringListData,
      type: (state) => state.type,
      FileModal: (state) => state.FileModal,
    }),
    getUrl() {
      if (this.type == null) {
        return `/api/Mastering/`;
      } else {
        return `/api/Mastering/${this.type}`;
      }
    },
    logTable() {
      return this.$refs["logTable"].instance;
    },
  },
  methods: {
    ...mapMutations("FileIndexStore", [
      "SET_MYDISK_TITLE",
      "addLocalFiles",
      "resetLocalFiles",
      "setFileUploading",
      "setTypeSelected",
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
    async masteringStatus() {
      var res = await axios.get(
        `/api/Mastering/mastering-status?user_id=${sessionStorage.getItem(
          "user_id"
        )}`
      );
      var masteringListData = [];
      res.data.resultObject.data.forEach((e) => {
        var data = {
          title: e.title,
          type: this.getCategory(e.category),
          user_id: e.regUserName,
          date: e.regDtm,
          step: e.workStatus,
        };
        masteringListData.push(data);
      });
      this.setMasteringListData(masteringListData);
    },
    async logSearch() {
      this.logTable.beginCustomLoading();
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
      var res = await axios
        .get(
          `/api/Mastering/mastering-logs?startDt=${sdt}&endDt=${edt}&user_id=${user_id}`
        )
        .catch((error) => {
          this.logTable.endCustomLoading();
          this.$fn.notify("error", { title: error });
        });
      console.info("res", res);

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
          note: e.note,
        };
        masteringLogData.push(data);
      });

      this.setMasteringLogData(masteringLogData);
      this.logTable.endCustomLoading();
    },
    getStatus(v) {
      if (v == 5) {
        return "성공";
      } else if (v == 6) {
        return "실패";
      }
    },
    eventSInput(value) {
      this.logSDate = value;
      this.tempSDate = value;

      const replaceAllFileSDate = this.logSDate.replace(/-/g, "");
      const replaceAllFileEDate = this.logEDate.replace(/-/g, "");
      if (
        replaceAllFileEDate < replaceAllFileSDate &&
        replaceAllFileEDate != ""
      ) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        return;
      } else {
        this.logSearch();
      }
    },
    eventEInput(value) {
      this.logEDate = value;
      this.tempEDate = value;

      const replaceAllFileSDate = this.logSDate.replace(/-/g, "");
      const replaceAllFileEDate = this.logEDate.replace(/-/g, "");
      if (replaceAllFileEDate < replaceAllFileSDate) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        return;
      } else {
        this.logSearch();
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

            const replaceAllFileSDate = this.logSDate.replace(/-/g, "");
            const replaceAllFileEDate = this.logEDate.replace(/-/g, "");
            if (
              replaceAllFileEDate < replaceAllFileSDate &&
              replaceAllFileEDate != ""
            ) {
              this.$fn.notify("error", {
                message: "시작 날짜가 종료 날짜보다 큽니다.",
              });
              return;
            } else {
              this.logSearch();
            }
            return;
          }
          this.logSDate = convertDate;
          this.tempSDate = convertDate;
          const replaceAllFileSDate = this.logSDate.replace(/-/g, "");
          const replaceAllFileEDate = this.logEDate.replace(/-/g, "");
          if (
            replaceAllFileEDate < replaceAllFileSDate &&
            replaceAllFileEDate != ""
          ) {
            this.$fn.notify("error", {
              message: "시작 날짜가 종료 날짜보다 큽니다.",
            });
            return;
          } else {
            this.logSearch();
          }
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

            const replaceAllFileSDate = this.logSDate.replace(/-/g, "");
            const replaceAllFileEDate = this.logEDate.replace(/-/g, "");
            if (replaceAllFileEDate < replaceAllFileSDate) {
              this.$fn.notify("error", {
                message: "시작 날짜가 종료 날짜보다 큽니다.",
              });
              return;
            } else {
              this.logSearch();
            }
            return;
          }
          this.logEDate = convertDate;
          this.tempEDate = convertDate;

          const replaceAllFileSDate = this.logSDate.replace(/-/g, "");
          const replaceAllFileEDate = this.logEDate.replace(/-/g, "");
          if (replaceAllFileEDate < replaceAllFileSDate) {
            this.$fn.notify("error", {
              message: "시작 날짜가 종료 날짜보다 큽니다.",
            });
            return;
          } else {
            this.logSearch();
          }
        }
      }
    },
    //#region 파일 조작
    upload() {
      this.fileState = "업로드 시작";
      this.fileupload.upload(0);
    },
    async valueChanged(event) {
      this.processing = true;
      this.resetLocalFiles();
      this.addLocalFiles(event.value[0]);
      if (event.value.length != 0) {
        if (2147483648 <= event.value[0].size) {
          this.resetLocalFiles();
          this.processing = false;
          this.setFileUploading(false);
          this.$fn.notify("error", {
            title: "최대 업로드 크기는 2GB 입니다.",
          });
          return;
        }
        this.setMetaModalTitle(event.value[0].name);
        this.SET_MYDISK_TITLE(this.sliceExt(200));
        if (event.value[0].size / 50 <= 1000000) {
          this.chunkSize = 1000000;
        } else {
          this.chunkSize = Math.ceil(event.value[0].size / 50);
        }
        if (
          event.value[0].type == "audio/mpeg" ||
          event.value[0].type == "audio/wav"
        ) {
          var formData = new FormData();

          if (event.value[0].type == "audio/mpeg") {
            var blob = event.value[0].slice(0, 1000000);
          } else if (event.value[0].type == "audio/wav") {
            var blob = event.value[0].slice(0, 10000);
          }

          formData.append("file", blob);
          formData.append("fileName", event.value[0].name);
          formData.append("fileSize", event.value[0].size);

          try {
            var res = await axios.post("/api/Mastering/Validation", formData);
            if (res.data.errorMsg != null && res.data.resultCode != 0) {
              this.$fn.notify("error", {
                title: "잘못된 파일 형식입니다.",
              });
              return;
            }
            this.setDuration(res.data.resultObject.duration);
            this.setAudioFormat(res.data.resultObject.audioFormatInfo);
            this.openFileModal();
            this.dropzone = false;
            this.processing = false;
            this.setFileUploading(false);
            this.MetaModal = true;
          } catch (error) {
            this.$fn.notify("error", {
              title: `파일 유효성 검사 실패(${error.message})`,
            });
            this.fileupload.abortUpload(0);
            this.resetLocalFiles();
            this.typeReset();
            this.percent = 0;
            this.MetaModal = false;
          }
        } else {
          this.$fn.notify("error", {
            title: "오디오 파일만 업로드 가능합니다.",
          });
          this.fileupload.removeFile(0);
          this.processing = false;
          this.setFileUploading(false);
        }
      } else if (event.value.length == 0) {
        this.processing = false;
        this.setFileUploading(false);
      }
    },
    sliceExt(maxLength) {
      var result = this.MetaModalTitle.replace(/(.wav|.mp3)$/, "");
      return result.substring(0, maxLength);
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
      this.percent = 0;
      this.MetaModal = false;
    },
    fileUploadCancel() {
      this.$fn.notify("error", { title: "파일 업로드 취소" });
      this.fileupload.abortUpload(0);
      this.resetLocalFiles();
      this.processing = false;
      this.setFileUploading(false);
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
      this.processing = false;
      this.setFileUploading(false);
      this.fileRemove();
    },
    uploadError(e) {
      this.processing = false;
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
    select() {
      this.fileupload._isCustomClickEvent = true;
      this.fileupload._$fileInput[0].click();
      this.setTypeSelected("my-disk");
    },
    openFileModal() {
      if (!this.FileModal) {
        this.tabIndex = 0;
      }
      this.setFileModal(true);
    },
    closeFileModal() {
      this.tabIndex = 1;
      this.processing = false;
      this.setFileUploading(false);
      this.setFileModal(false);
      this.isDropZoneActive = false;
      this.dropzone = false;
      this.$emit("dropZoneLeave");
    },
    onDropZoneEnter(e) {
      if (e.dropZoneElement.id === "dropzone-external") {
        this.isDropZoneActive = true;
        this.setTypeSelected("my-disk");
      }
    },
    onDropZoneLeave(e) {
      if (e.dropZoneElement.id === "dropzone-external") {
        this.isDropZoneActive = false;
        this.dropzone = false;
        this.$emit("dropZoneLeave");
      }
    },
    dragLeave() {
      this.$emit("dragLeave");
    },
    //#endregion
    getCategory(v) {
      switch (v) {
        case "MY":
          return "MY 디스크";
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
