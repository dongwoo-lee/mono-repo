import { mapGetters, mapActions } from "vuex";
import { DxDataGrid, DxColumn } from "devextreme-vue/data-grid";
import commonFunction from "../../utils/CommonFunctions";

import DxFileUploader from "devextreme-vue/file-uploader";
import DxValidator from "devextreme-vue/validator";
import DxTextBox from "devextreme-vue/text-box";
import VueStepProgressIndicator from "vue-step-progress-indicator";
import Vuetable from "vuetable-2/src/components/Vuetable";

export default {
  components: {
    DxDataGrid,
    DxColumn,
    commonFunction,
    DxFileUploader,
    DxTextBox,
    DxValidator,
    Vuetable,
    VueStepProgressIndicator
  },
  data() {
    return {
      processing: false,
      fileUploading: false,
      typeSelected: "f",
      mediaSelected: "a",
      isActive: false,
      date: "",
      formatted: "",
      dateSelected: "",
      title: "",
      memo: "",
      type: "",
      mediaCD: "",
      categoryCD: "",
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
    }
  },

  methods: {
    ...mapActions("file", ["verifyMeta", "uploadRefresh"]),

    onSelectionChanged() {
      console.log("selection changed");
    },
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
    }
  }
};
