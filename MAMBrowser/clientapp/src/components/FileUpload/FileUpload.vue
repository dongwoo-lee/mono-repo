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
      style="position:absolute; top:-80px; right:580px; z-index:1030; border-color:#008ECC; color:#008ECC; background-color:white;"
      >파일 업로드
    </b-button>
    <transition name="slide-fade">
      <CommonFileModal
        v-show="FileModal"
        @close="closeFileModal"
        style="font-family: 'Times New Roman', Times, serif; font-weight:bold;"
      >
        <h3 slot="header">
          음원 파일 업로드
          <b-button
            class="btn btn-outline-primary btn-sm default cutom-label mr-2"
            id="addFile"
            >추가</b-button
          >
          <DxFileUploader
            :chunk-size="200000"
            dialog-trigger="#addFile"
            :upload-custom-data="uploaderCustomData"
            :ref="dxfu"
            name="file"
            drop-zone=".dropzone"
            @drop-zone-enter="onDropZoneEnter"
            @drop-zone-leave="onDropZoneLeave"
            upload-url="/api/fileupload/"
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
        </h3>
        <h4 slot="body">
          <div>
            <b-card no-body>
              <b-tabs pills justified vertical>
                <b-tab title="알림" active
                  ><div>
                    <vuetable
                      :table-height="vueTableWidth"
                      ref="vuetable-scrollable"
                      :api-mode="false"
                      :fields="notiFields"
                      :data="vueTableData"
                      no-data-template=""
                    >
                      <template slot="name" scope="props">
                        <div style="font-size:18px;">
                          {{ props.rowData.fileName }}
                        </div>
                      </template>
                      <template slot="mastering" scope="props">
                        <div style="width:200px; height:20px;">
                          <vue-step-progress-indicator
                            :steps="[
                              '대기 중',
                              '스토리지 복사',
                              '파일 샘플링',
                              '파일 마스터링',
                              '완료'
                            ]"
                            :active-step="props.rowData.step"
                            :is-reactive="false"
                            :styles="styleData"
                            :colors="colorData"
                            style="margin-left:10px; width:670px;"
                          />
                        </div>
                      </template>
                    </vuetable>
                  </div>
                </b-tab>
                <b-tab title="로그">
                  <div style=" margin-left:20px; width:995px; height:150px;">
                    <vuetable
                      :table-height="vueTableWidth"
                      ref="vuetable-scrollable"
                      :api-mode="false"
                      :fields="logFields"
                      :data="vueTableData"
                      no-data-template=""
                    >
                      <template slot="rowNO" scope="props">
                        <div>{{ props.rowIndex + 1 }}</div>
                        <!-- <button @click="getRowData(props)">확인</button> 
                        -+ -->
                      </template>
                      <template slot="fileName" scope="props">
                        <div>
                          {{ props.rowData.fileName }}
                        </div>
                      </template>
                      <template slot="fileSize" scope="props">
                        <div>
                          {{ (props.rowData.fileSize / 1048576).toFixed(2) }} MB
                        </div>
                      </template>
                      <template slot="title" scope="props">
                        <div style="font-size:18px;">
                          {{ props.rowData.title }}
                        </div>
                      </template>
                      <template slot="memo" scope="props">
                        <div style="font-size:18px;">
                          {{ props.rowData.memo }}
                        </div>
                      </template>
                    </vuetable>
                  </div>
                </b-tab>
                <b-tab title="파일 업로드"> </b-tab>
              </b-tabs>
            </b-card>
          </div>
        </h4>
      </CommonFileModal>
    </transition>

    <MetaModal
      @upload="upload"
      :fileState="fileState"
      :percent="percent"
    ></MetaModal>
  </div>
</template>

<script>
import CommonFileModal from "../Modal/CommonFileModal.vue";
import CommonFileFunction from "./CommonFileFunction";
import MetaModal from "./MetaModal";
import * as signalR from "@microsoft/signalr";
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
      dxfu,
      FileModal: false,
      dropzone: false,
      isDropZoneActive: false,
      chunks: [],
      fileSelect: false,
      fileState: "",
      percent: 0
    };
  },
  watch: {
    DragFileModalState(v) {
      this.dropzone = v;
    }
  },
  created() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("/FileHubs", {
        skipNegotiation: false
        // transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect([3000, 5000, 10000, null])
      .configureLogging(signalR.LogLevel.Information)
      .build();
  },
  mounted() {
    this.connection.start().then(
      setTimeout(() => {
        this.setConnectionId(this.connection.connectionId);
      }, 500)
    );
    this.connection.on("send", (res, message) => {
      if (res == 1) {
        this.setVueTableData(message);
        this.MetaModalOff();
        this.fileState = "업로드 성공";
      } else if (res == 2) {
        this.forEachVueTableData(message);
      } else if (res == 3) {
        this.forEachVueTableData(message);
      } else if (res == 4) {
        this.forEachVueTableData(message);
      } else if (res == 5) {
        this.forEachVueTableData(message);
      }
    });

    // window.addEventListener("beforeunload", this.unLoadEvent);
    setTimeout(() => {
      document.body.classList.add("default-transition");
    }, 100);
  },
  beforeUnmount() {
    this.connection.stop();
  },
  // window.removeEventListener("beforeunload", this.unLoadEvent);
  // beforeRouteLeave(to, from, next) {
  //   if (!this.fileSelect) {
  //     if (
  //       confirm(
  //         "이 사이트에서 나가시겠습니까?\n변경사항이 저장되지 않을 수 있습니다."
  //       )
  //     ) {
  //       next();
  //     }
  //   }
  // },
  computed: {
    fileupload: function() {
      return this.$refs[dxfu].instance;
    },
    ...mapState("FileIndexStore", {
      uploaderCustomData: state => state.uploaderCustomData,
      localFiles: state => state.localFiles,
      vueTableData: state => state.vueTableData
    })
  },
  methods: {
    ...mapMutations("FileIndexStore", [
      "MetaModalOn",
      "MetaModalOff",
      "addLocalFiles",
      "resetLocalFiles",
      "setMetaModalTitle",
      "setConnectionId",
      "setVueTableData",
      "forEachVueTableData"
    ]),
    //#region 파일 조작
    upload() {
      this.fileState = "업로드 시작";
      this.fileupload.upload(0);
    },
    valueChanged(event) {
      //#region header stream
      // var blob = event.value[0].slice(0, 44);
      // // const reader = new FileReader(blob);
      // // reader.onload = function(res) {
      // // };
      // // var buffer = reader.readAsArrayBuffer(blob);
      // let form = new FormData();
      // form.append("file", blob);

      // axios.post("/api/fileupload/check", form);
      //#endregion
      this.resetLocalFiles();
      this.addLocalFiles(event.value[0]);
      if (event.value.length != 0) {
        this.setMetaModalTitle(event.value[0].name);
      }
      if (event.value.length != 0) {
        if (
          event.value[0].type == "audio/mpeg" ||
          event.value[0].type == "audio/wav" ||
          event.value[0].type == "image/jpeg"
        ) {
          this.FileModal = true;
          this.MetaModalOn();
          this.fileSelect = true;
          this.fileUploading = true;
        } else {
          //TODO: 얼럿 창 예쁜 모달로 변경
          alert("업로드 할 수 없는 파일 형식입니다.");
          this.fileupload.removeFile(0);
          this.fileselect = false;
        }
      } else if (event.value.length == 0) {
        this.fileselect = false;
      }
    },
    fileRemove() {
      this.fileupload.removeFile(0);
      this.resetLocalFiles();
    },
    uploadSuccess() {
      this.$fn.notify("primary", { message: "파일 업로드 성공" });
      this.fileState = "리셋";
      this.fileRemove();
      this.uploadRefresh();
    },
    uploadError() {
      this.$fn.notify("error", { message: "파일 업로드 실패" });
    },
    uploadAborted() {
      this.fileselect = false;
      this.$fn.notify("error", { message: "파일 업로드 취소" });
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
      if (this.localFiles.length != null) {
        if (this.localFiles.length == 1) {
          if (confirm("현재 진행 중인 작업이 있습니다. 창을 닫으시겠습니까?")) {
            this.FileModal = false;
            // this.localFiles = [];
            // this.reset();
          } else {
            return;
          }
        } else {
          this.FileModal = false;
        }
      } else {
        this.FileModal = false;
      }
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
.progress__wrapper {
  margin: 0px !important;
}
.card {
  height: 675px !important;
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
