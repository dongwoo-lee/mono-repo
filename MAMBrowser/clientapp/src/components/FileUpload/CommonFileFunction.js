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
      isActive: false,
      date: "",
      formatted: "",
      dateSelected: "",
      MetaData: {
        title: "",
        memo: "",
        mediaCD: "",
        categoryCD: "",
        typeSelected: "null",
        mediaSelected: "a"
      },
      typeOptions: [
        { value: "null", text: "소재 유형" },
        { value: "private", text: "My디스크" }
      ],
      mediaOptions: [
        { value: "a", text: "AM" },
        { value: "f", text: "FM" }
      ],
      vueTableWidth: "560px",
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
      return this.MetaData.typeSelected == "null" ? true : false;
    },
    titleState() {
      return this.MetaData.title.length >= 1 ? true : false;
    },
    memoState() {
      return this.MetaData.memo.length >= 1 ? true : false;
    },
    metaValid() {
      if (
        this.MetaData.title.length >= 1 &&
        this.MetaData.memo.length >= 1 &&
        this.MetaData.typeSelected != "null"
      )
        return true;
      else return false;
    }
  },
  watch: {
    MetaData: {
      deep: true,
      handler(v) {
        if (v.typeSelected == "null") {
          this.isActive = false;
        } else if (v.typeSelected == "private") {
          this.isActive = true;
        }
      }
    }
  },

  methods: {
    ...mapActions("file", ["verifyMeta", "uploadRefresh"]),
    fileStateFalse() {
      this.processing = false;
      this.fileUploading = false;
    },
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
        this.dateReset();
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
    dateReset() {
      var input = document.getElementById("dateinput");
      input.value = null;
    },
    //#endregion

    memoReset() {
      this.MetaData.memo = "";
    },
    titleReset() {
      this.MetaData.title = "";
    },
    reset() {
      this.titleReset();
      this.memoReset();
      this.MetaData.typeSelected = "null";
      this.dateReset();
      this.fileStateFalse();
      this.fileSelect = false;
      if (this.processing) {
        this.$fn.notify("error", { message: "파일 업로드 취소" });
      }
    }
  }
};
