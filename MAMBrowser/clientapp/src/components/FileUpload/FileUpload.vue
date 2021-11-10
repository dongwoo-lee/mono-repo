<template>
  <div id="file-container">
    <div
      id="dropzone-external"
      class="dropzone"
      v-show="dropzone"
      style="position: fixed; z-index: 9800; top: -80px; left: -80px; width: 2000px; height: 120%; background-color: rgba(0, 0, 0, 0.5); display: table;"
      :class="[
        isDropZoneActive
          ? 'dx-theme-accent-as-border-color dropzone-active'
          : 'dx-theme-border-color'
      ]"
    >
      <p
        style="margin-top:500px; margin-left:200px; text-align:center;color:white; font-size:48px;"
      >
        음원 파일 업로드
      </p>
      <div id="dropzone-text" class="flex-box" style="margin-top:80px;">
        <span
          style="position:absolute; z-index:1; color:white;  margin-top:700px;font-size:48px;"
        >
        </span>
      </div>
    </div>
    <b-button
      class="btn btn-outline-primary btn-sm default cutom-label mr-2"
      id="fileuploadbutton"
      @click="openFileModal"
      style="position:absolute; top:-80px; right:570px; z-index:1030; border-color:#008ECC; color:#008ECC; background-color:white;"
    >
      <b-icon
        icon="file-earmark-music"
        style="margin-right:15px;"
        aria-hidden="true"
      >
      </b-icon>
      마스터링
    </b-button>
    <b-badge
      v-show="getBadge != 0"
      style="position:absolute; top:-90px; right:570px; z-index:1030; 
      bordercolor:red; color:red; background-color:white;  border-radius:80%"
      variant="outline-danger"
      >{{ getBadge }}</b-badge
    >
    <transition name="slide-fade">
      <CommonFileModal
        v-show="FileModal"
        @close="closeFileModal"
        style="font-family: 'Times New Roman', Times, serif; font-weight:bold;"
      >
        <h2 slot="header">
          마스터링
        </h2>
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
            @upload-aborted="uploadAborted"
            @upload-error="uploadError"
            @uploaded="uploadSuccess"
            @progress="onUploadProgress($event)"
          />
          <div>
            <!-- <b-card no-body>
              <b-tabs pills>
                <b-tab title="알림" active> -->
            <!-- <span>
              <p
                style="color: #008ecc; font-size:22px; margin-top:20px; margin-left: 20px;"
              >
                알림
              </p> -->

            <b-button
              style="position:absolute; top:115px; right:45px;  border-color:#008ECC; color:#008ECC; background-color:white; z-index:9999;"
              class="btn btn-outline-primary btn-sm default cutom-label mr-2"
              id="addFile"
            >
              파일 업로드
            </b-button>
            <!-- </span> -->
            <b-card style="color: #008ECC;" title="마스터링 작업목록">
              <!-- <hr style="width:99%; height:1px; background-color:#008ecc;" /> -->
              <div
                style="width:1300px; margin-left:auto; margin-right:auto; font-size:14px;"
              >
                <vuetable
                  v-show="this.role == 'ADMIN'"
                  :table-height="vueTableWidth"
                  ref="vuetable-scrollable"
                  :api-mode="false"
                  :fields="adminFields"
                  :data="vueTableData"
                  no-data-template="데이터가 없습니다."
                >
                  <template slot="title" scope="props">
                    <div style="font-size:14px;">
                      {{ props.rowData.title }}
                    </div>
                    <!-- <b-button @click="getProps(props)">확인</b-button> -->
                  </template>
                  <template slot="type" scope="props">
                    <div>
                      {{ props.rowData.type }}
                    </div>
                  </template>
                  <template slot="user_id" scope="props">
                    <div>
                      {{ props.rowData.user_id }}
                    </div>
                  </template>
                  <template slot="date" scope="props">
                    <div>
                      {{ props.rowData.date }}
                    </div>
                  </template>
                  <template slot="step" scope="props">
                    <div style="width:220px; height:20px;">
                      <vue-step-progress-indicator
                        :steps="[
                          '대기 중',
                          '디코딩',
                          '리샘플링',
                          '노말라이즈',
                          '스토리지 저장',
                          '완료'
                        ]"
                        :active-step="props.rowData.step"
                        :is-reactive="false"
                        :styles="styleData"
                        :colors="successColorData"
                        style="margin-left:30px; width:680px;"
                      />
                    </div>
                  </template>
                </vuetable>
                <vuetable
                  v-show="this.role != 'ADMIN'"
                  :table-height="vueTableWidth"
                  ref="vuetable-scrollable"
                  :api-mode="false"
                  :fields="userFields"
                  :data="vueTableData"
                  no-data-template="데이터가 없습니다."
                >
                  <template slot="title" scope="props">
                    <div style="font-size:14px;">
                      {{ props.rowData.title }}
                    </div>
                  </template>
                  <template slot="type" scope="props">
                    <div>
                      {{ props.rowData.type }}
                    </div>
                  </template>
                  <template slot="date" scope="props">
                    <div>
                      {{ props.rowData.date }}
                    </div>
                  </template>
                  <template slot="step" scope="props">
                    <div style="width:220px; height:20px;">
                      <vue-step-progress-indicator
                        :steps="[
                          '대기 중',
                          '디코딩',
                          '리샘플링',
                          '노말라이즈',
                          '스토리지 저장',
                          '완료'
                        ]"
                        :active-step="props.rowData.step"
                        :is-reactive="false"
                        :styles="styleData"
                        :colors="successColorData"
                        style="margin-left:30px; width:680px;"
                      />
                    </div>
                  </template>
                </vuetable>
              </div>
            </b-card>

            <b-card style="color: #008ECC;" title="마스터링 로그">
              <div
                style="position:absolute; top:22px; left: 160px; font-size:14px;"
              >
                <b-form-group
                  label="시작일"
                  class="has-float-label"
                  style="width:200px; float:left; margin-left:20px; margin-right:20px;"
                >
                  <b-input-group class="mb-3" style="width:200px; float:left;">
                    <input
                      style="height:33px; font-size:13px;"
                      id="sdateinput"
                      type="text"
                      class="form-control input-picker date-input"
                      :value="sdate"
                      @input="onsInput"
                    />
                    <b-input-group-append>
                      <b-form-datepicker
                        style="height:33px;"
                        v-model="sdate"
                        button-only
                        button-variant="outline-primary"
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
                  style="width:200px;"
                >
                  <b-input-group class="mb-3" style="width:200px; float:left;">
                    <input
                      style="height:33px; font-size:13px;"
                      id="edateinput"
                      type="text"
                      class="form-control input-picker date-input"
                      :value="edate"
                      @input="oneInput"
                    />
                    <b-input-group-append>
                      <b-form-datepicker
                        style="height:33px;"
                        v-model="edate"
                        button-only
                        button-variant="outline-primary"
                        right
                        aria-controls="example-input"
                        @context="onContext"
                      ></b-form-datepicker>
                    </b-input-group-append>
                  </b-input-group>
                </b-form-group>
              </div>

              <div
                style="width:1300px; margin-left:auto; margin-right:auto; font-size:14px;"
              >
                <vuetable
                  v-show="this.role == 'ADMIN'"
                  :table-height="vueTableWidth"
                  ref="vuetable-scrollable"
                  :api-mode="false"
                  :fields="adminFields"
                  :data="vueTableData"
                  no-data-template="데이터가 없습니다."
                >
                  <template slot="rowNO" scope="props">
                    <div>{{ props.rowIndex + 1 }}</div>
                  </template>

                  <template slot="title" scope="props">
                    <div style="font-size:14px;">
                      {{ props.rowData.title }}
                    </div>
                  </template>
                  <template slot="type" scope="props">
                    <div>
                      {{ props.rowData.type }}
                    </div>
                  </template>
                  <template slot="user_id" scope="props">
                    <div>
                      {{ props.rowData.user_id }}
                    </div>
                  </template>
                  <template slot="date" scope="props">
                    <div>
                      {{ props.rowData.date }}
                    </div>
                  </template>
                </vuetable>

                <vuetable
                  v-show="this.role != 'ADMIN'"
                  :table-height="vueTableWidth"
                  ref="vuetable-scrollable"
                  :api-mode="false"
                  :fields="userFields"
                  :data="vueTableData"
                  no-data-template="데이터가 없습니다."
                >
                  <template slot="title" scope="props">
                    <div style="font-size:14px;">
                      {{ props.rowData.title }}
                    </div>
                  </template>
                  <template slot="type" scope="props">
                    <div>
                      {{ props.rowData.type }}
                    </div>
                  </template>
                  <template slot="user_id" scope="props">
                    <div>
                      {{ props.rowData.user_id }}
                    </div>
                  </template>
                  <template slot="date" scope="props">
                    <div>
                      {{ props.rowData.date }}
                    </div>
                  </template>
                </vuetable>
              </div>
            </b-card>
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
import MetaModal from "./MetaModal";
import axios from "axios";
const dxfu = "my-fileupload";
import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
export default {
  mixins: [CommonFileFunction],
  props: {
    DragFileModalState: {
      type: Boolean,
      default: false
    }
  },
  components: {
    CommonFileModal,
    MetaModal
  },
  data() {
    return {
      role: "",
      dxfu,
      FileModal: false,
      MetaModal: false,
      dropzone: false,
      isDropZoneActive: false,
      chunks: [],
      fileSelect: false,
      fileState: "",
      percent: 0,
      sdate: "",
      edate: ""
    };
  },
  watch: {
    DragFileModalState(v) {
      this.dropzone = v;
    }
  },
  computed: {
    fileupload: function() {
      return this.$refs[dxfu].instance;
    },
    ...mapGetters("user", ["diskAvailable"]),
    ...mapGetters("FileIndexStore", ["getBadge"]),
    ...mapState("FileIndexStore", {
      uploaderCustomData: state => state.uploaderCustomData,
      localFiles: state => state.localFiles,
      vueTableData: state => state.vueTableData,
      MetaData: state => state.MetaData
    }),
    getUrl() {
      if (this.MetaData.typeSelected == null) {
        return `/api/Mastering/`;
      } else {
        return `/api/Mastering/${this.MetaData.typeSelected}`;
      }
    }
  },
  methods: {
    getProps(props) {
      console.log(props);
    },
    onsInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");
      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          this.sdate = convertDate;
        }
      } else if (targetValue == "-") {
        const replaceAllTargetValue = targetValue.replace(/-/g, "");
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          this.sdate = convertDate;
        }
      } else {
        this.sdate = "";
        this.$fn.notify("error", { message: "숫자만 입력 가능 합니다." });
      }
    },
    oneInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");
      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          this.edate = convertDate;
        }
      } else if (targetValue == "-") {
        const replaceAllTargetValue = targetValue.replace(/-/g, "");
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          this.edate = convertDate;
        }
      } else {
        this.edate = "";
        this.$fn.notify("error", { message: "숫자만 입력 가능 합니다." });
      }
    },
    ...mapMutations("FileIndexStore", [
      "addLocalFiles",
      "resetLocalFiles",
      "setMetaModalTitle",
      "setConnectionId",
      "setVueTableData",
      "setDuration",
      "setAudioFormat",
      "forEachVueTableData"
    ]),
    //#region 파일 조작
    upload() {
      this.fileState = "업로드 시작";
      this.fileupload.upload(0);
    },
    valueChanged(event) {
      this.resetLocalFiles();
      this.addLocalFiles(event.value[0]);
      if (event.value.length != 0) {
        this.setMetaModalTitle(event.value[0].name);
      }
      if (event.value.length != 0) {
        if (this.notDiskAvailable(event.value[0].size)) {
          if (
            event.value[0].type == "audio/mpeg" ||
            event.value[0].type == "audio/wav" ||
            event.value[0].type == "image/jpeg"
          ) {
            var blob = event.value[0].slice(0, 1000);
            var formData = new FormData();
            formData.append("file", blob);
            formData.append("fileExt", event.value[0].name);
            axios.post("/api/Mastering/Validation", formData).then(res => {
              if (
                res.data.resultObject.duration == null ||
                res.data.resultObject.audioFormatInfo == null
              ) {
                this.$fn.notify("error", { title: "오디오 파일 확인" });
                return;
              }
              this.setDuration(res.data.resultObject.duration);
              this.setAudioFormat(res.data.resultObject.audioFormatInfo);
              this.openFileModal();
              this.MetaModal = true;
              this.fileSelect = true;
              this.fileUploading = true;
            });
          } else {
            //TODO: 얼럿 창 예쁜 모달로 변경
            alert("업로드 할 수 없는 파일 형식입니다.");
            this.fileupload.removeFile(0);
            this.fileselect = false;
          }
        } else {
          this.$fn.notify("error", { title: "디스크 공간이 부족합니다." });
        }
      } else if (event.value.length == 0) {
        this.fileselect = false;
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
      this.resetLocalFiles();
      this.reset();
      this.percent = 0;
      this.MetaModal = false;
    },
    fileUploadCancel() {
      this.$fn.notify("error", { title: "파일 업로드 취소" });
      this.fileupload.abortUpload(0);
      this.resetLocalFiles();
      this.reset();
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
      this.fileUploading = false;
      this.fileRemove();
      this.uploadRefresh();
    },
    uploadError(e) {
      this.percent = 0;
      this.$fn.notify("error", { message: e.error.response });
    },
    uploadAborted() {
      this.fileselect = false;
      // this.percent = 0;
      // this.$fn.notify("error", { message: "파일 업로드 취소" });
    },
    onUploadProgress(e) {
      this.percent = Math.ceil((e.bytesLoaded / e.bytesTotal) * 100);
    },
    //#endregion
    //#region 모달 조작
    openFileModal() {
      this.FileModal = true;
    },
    closeFileModal() {
      this.FileModal = false;
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
    }
    //#endregion
  }
};
</script>
<style>
@import "./FileUploadCSS.css";
</style>
<style scoped>
/* //TODO: vueTable height 조절 */
.myTableHeader {
  margin-left: 100px !important;
  height: 400px !important;
}
.progress__wrapper {
  width: 600px !important;
  /* margin-left: 20px !important; */
  margin: 0px !important;
}
.card {
  width: 1350px;
  height: 330px;
  margin-left: 20px;
  margin-top: 20px;
}
.date-input:focus {
  border: 1px solid #4475c4 !important;
}
.date-input {
  border: 1px solid #008ecc !important;
}
.media-select {
  border: 1px solid #008ecc !important;
}
.media-select:focus {
  border: 1px solid #4475c4 !important;
}
</style>
