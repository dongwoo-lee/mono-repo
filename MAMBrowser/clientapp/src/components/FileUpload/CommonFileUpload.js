import { mapGetters, mapActions } from "vuex";
import { DxDataGrid, DxColumn } from "devextreme-vue/data-grid";
import commonFunction from "../../utils/CommonFunctions";
import FileModal from "../Modal/FileModal.vue";
import MetaModal from "../Modal/MetaModal.vue";
import DxFileUploader from "devextreme-vue/file-uploader";
import DxValidator from "devextreme-vue/validator";
import DxTextBox from "devextreme-vue/text-box";
import VueStepProgressIndicator from "vue-step-progress-indicator";
import Vuetable from "vuetable-2/src/components/Vuetable";
import * as signalR from "@microsoft/signalr";
const dxfu = "my-fileupload";
export default {
  props: {
    modalState: {
      type: Boolean,
      default: false
    }
  },
  components: {
    DxDataGrid,
    DxColumn,
    FileModal,
    MetaModal,
    commonFunction,
    DxFileUploader,
    DxTextBox,
    DxValidator,
    Vuetable,
    VueStepProgressIndicator
  },
  data() {
    return {
      dxfu,
      processing: false,
      dropzone: false,
      isDropZoneActive: false,
      chunks: [],
      fileModal: false,
      metaModal: false,
      localFiles: {},
      title: "",
      memo: "",
      type: "",
      mediaCD: "",
      categoryCD: "",
      uploaderCustomData: {},
      step: 1,
      typeSelected: "f",
      mediaSelected: "a",
      isActive: false,
      date: "",
      formatted: "",
      dateSelected: "",
      fileSelect: false,
      typeOptions: [
        { value: "f", text: "소재 유형" },
        { value: "a", text: "My디스크" }
      ],
      logTable: "560px",
      notiTable: "560px",
      vtData: [
        {
          fileName: "숀 (SHAUN) - Way Back Home",
          fileSize: 8858948,
          title: "건강한 아침 오프닝",
          memo: "12시까지 확인",
          step: 1
        },
        {
          fileName: "모모랜드 (MOMOLAND) - 뿜뿜",
          fileSize: 8858948,
          title: "생생 정보통 오프닝",
          memo: "오늘 칼퇴",
          step: 2
        },
        {
          fileName: "CHRISTOPHER - Monogamy (Lyrics)",
          fileSize: 8858948,
          title: "ddd",
          memo: "sss",
          step: 1
        }
      ],
      mediaOptions: [
        { value: "a", text: "AM" },
        { value: "f", text: "FM" }
      ],
      logFields: [
        {
          name: "__slot:rowNO",
          title: "순서",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "7%"
        },
        {
          name: "__slot:fileName",
          title: "파일명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "40%"
        },
        {
          name: "__slot:fileSize",
          title: "파일 크기",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%"
        },
        {
          name: "__slot:title",
          title: "제목",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center"
        },
        {
          name: "__slot:memo",
          title: "메모",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center"
        }
      ],
      notiFields: [
        {
          name: "__slot:name",
          title: "파일명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "__slot:mastering",
          title: "마스터링",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "20%"
        }
      ],
      styleData: {
        progress__block: {
          display: "flex",
          alignItems: "center"
        },
        progress__bridge: {
          backgroundColor: "grey",
          height: "2px",
          flexGrow: "1",
          width: "20px"
        },
        progress__bubble: {
          margin: "0",
          padding: "0",
          lineHeight: "10px",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          height: "18px",
          width: "18px",
          borderRadius: "10%",
          backgroundColor: "transparent",
          border: "2px grey solid",
          textAlign: "center"
        },
        progress__label: {
          margin: "0 0.8rem",
          font: "14px;"
        }
      },
      colorData: {
        progress__bubble: {
          active: {
            color: "#fff",
            backgroundColor: "#27ae60",
            borderColor: "#27ae60"
          },
          inactive: {
            color: "#fff",
            backgroundColor: "#EF5350",
            borderColor: "#EF5350"
          },
          completed: {
            color: "#fff",
            borderColor: "#27ae60",
            backgroundColor: "#27ae60"
          }
        },
        progress__label: {
          active: {
            color: "#27ae60"
          },
          inactive: {
            color: "#EF5350"
          },
          completed: {
            color: "#27ae60"
          }
        }
      }
    };
  },
  computed: {
    ...mapGetters("menu", ["getMenuType"]),
    fileupload: function() {
      return this.$refs[dxfu].instance;
    },
    typeState() {
      return this.typeSelected == "f" ? true : false;
    },
    titleState() {
      return this.title.length >= 1 ? true : false;
    },
    memoState() {
      return this.memo.length >= 1 ? true : false;
    },
    metavalid() {
      if (
        this.title.length >= 1 &&
        this.memo.length >= 1 &&
        this.typeSelected != "f"
      )
        return true;
      else return false;
    }
  },
  watch: {
    typeSelected: function(v) {
      if (v == "f") {
        this.isActive = false;
      } else if (v == "a") {
        this.isActive = true;
      }
    },
    modalState(v) {
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
    this.connection.start();
    // 화살표 함수
    this.connection.on("send", (res, message) => {
      console.log(res);
      console.log(message);
      if (res == 1) {
        this.vtData.push(message);
      } else if (res == 2) {
        this.vtData.forEach(element => {
          if (message.fileName == element.fileName) {
            element.step = message.step;
          }
        });
      } else if (res == 3) {
        this.vtData.forEach(element => {
          if (message.fileName == element.fileName) {
            element.step = message.step;
          }
        });
      } else if (res == 4) {
        this.vtData.forEach(element => {
          if (message.fileName == element.fileName) {
            element.step = message.step;
          }
        });
      } else if (res == 5) {
        this.vtData.forEach(element => {
          if (message.fileName == element.fileName) {
            element.step = message.step;
          }
        });
        console.log("파일 업로드 성공");
      }
    });
    // window.addEventListener("beforeunload", this.unLoadEvent);
    setTimeout(() => {
      document.body.classList.add("default-transition");
    }, 100);
  },
  beforeUnmount() {
    this.connection.stop();
    // window.removeEventListener("beforeunload", this.unLoadEvent);
  },
  // beforeRouteLeave(to, from, next) {
  //   if (!this.fileuploading) {
  //     if (
  //       confirm(
  //         "이 사이트에서 나가시겠습니까?\n변경사항이 저장되지 않을 수 있습니다."
  //       )
  //     ) {
  //       next();
  //     }
  //   }
  // },
  methods: {
    onSelectionChanged() {
      console.log("selection changed");
    },
    metaModalOff() {
      this.metaModal = false;
      this.reset();
    },
    metaModalOn() {
      this.metaModal = true;
    },
    ...mapActions("file", ["verifyMeta", "uploadRefresh"]),
    getRowData(props) {
      console.log(props);
    },
    getPro() {
      console.log(this.date + "_" + this.mediaSelected);
    },
    //#region 파일 업로드 모달 캘린더
    onInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");
      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          this.date = convertDate;
        }
      } else if (targetValue == "-") {
        const replaceAllTargetValue = targetValue.replace(/-/g, "");
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          this.date = convertDate;
        }
      } else {
        this.datereset();
        this.$fn.notify("error", { message: "숫자만 입력 가능 합니다." });
      }
    },
    convertDateSTH(value) {
      const replaceVal = value.replace(/-/g, "");
      const yyyy = replaceVal.substring(0, 4);
      const mm = replaceVal.substring(4, 6);
      const dd = replaceVal.substring(6, 8);
      if (12 < mm) {
        this.$fn.notify("error", { message: "날짜 형식 오류" });
        this.date = "";
      } else if (31 < dd) {
        this.$fn.notify("error", { message: "날짜 형식 오류" });
        this.date = "";
      } else {
        return `${yyyy}-${mm}-${dd}`;
      }
    },
    onContext(ctx) {
      // The date formatted in the locale, or the `label-no-date-selected` string
      this.formatted = ctx.selectedFormatted;
      // The following will be an empty string until a valid date is entered
      this.dateSelected = ctx.selectedYMD;
    },
    datereset() {
      var input = document.getElementById("dateinput");
      input.value = null;
    },
    //#endregion
    uploadError() {
      this.$fn.notify("error", { message: "파일 업로드 실패" });
    },
    uploadSuccess() {
      this.$fn.notify("primary", { message: "파일 업로드 성공" });
      this.reset();
      this.metaModalOff();
      this.uploadRefresh();
    },
    memoreset() {
      this.memo = "";
    },
    titlereset() {
      this.title = "";
    },
    reset() {
      this.memo = "";
      this.title = "";
      this.typeSelected = "f";
      this.datereset();
      this.fileupload.removeFile(0);
      this.fileuploading = false;
      this.fileselect = false;
      this.localFiles = [];
      if (this.processing) {
        this.$fn.notify("error", { message: "파일 업로드 취소" });
      }
    },
    uploadAborted() {
      this.fileuploading = false;
      this.fileselect = false;
      this.$fn.notify("error", { message: "파일 업로드 취소" });
    },
    fileRemove() {
      this.fileupload.removeFile(0);
      this.localFiles = [];
    },
    async uploadfile() {
      if (this.metavalid && this.fileselect) {
        if (this.typeSelected == "a") {
          this.type = "private";
        }

        //NOTE: 커스텀 데이터 파라미터

        var data = {
          user_id: sessionStorage.getItem("user_id"),
          title: this.title,
          memo: this.memo,
          fileSize: this.localFiles[0].size,
          connectionId: this.connection.connectionId
        };

        this.uploaderCustomData = data;
        this.processing = true;
        this.verifyMeta({
          type: this.type,
          title: this.title,
          files: this.localFiles,
          categoryCD: this.categoryCD
        }).then(res => {
          this.processing = false;
          if (res) {
            // 파일 업로드
            this.fileupload.upload(0);
          }
        });
      } else if (!this.metavalid) {
        alert("메타데이터");
      } else if (!this.fileselect) {
        alert("파일 확인");
      }
    },
    valueChanged(event) {
      this.localFiles = [];
      this.localFiles.push(event.value[0]);
      if (event.value.length != 0) {
        if (
          event.value[0].type == "audio/mpeg" ||
          event.value[0].type == "image/jpeg"
        ) {
          this.fileModal = true;
          this.metaModalOn();
          this.fileselect = true;
          this.fileuploading = true;
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
    onUploadProgress(e) {
      this.chunks.push({
        segmentSize: e.segmentSize,
        bytesLoaded: e.bytesLoaded,
        bytesTotal: e.bytesTotal
      });
    },
    makeToast(variant = null) {
      this.$bvToast.toast("FileName Background Task Start", {
        title: "File Upload Complete",
        variant: variant,
        solid: true,
        noAutoHide: false,
        autoHideDelay: 5000
      });
    },
    openModal() {
      this.fileModal = true;
    },
    closeModal() {
      this.fileModal = false;
    },
    confirm() {
      if (this.localFiles.length != null) {
        if (this.localFiles.length == 1) {
          if (confirm("현재 진행 중인 작업이 있습니다. 창을 닫으시겠습니까?")) {
            this.fileModal = false;
            this.localFiles = [];
            this.reset();
          } else {
            return;
          }
        } else {
          this.fileModal = false;
        }
      } else {
        this.fileModal = false;
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
  }
};
