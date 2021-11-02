import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
import { DxDataGrid, DxColumn } from "devextreme-vue/data-grid";
import commonFunction from "../../utils/CommonFunctions";
import DxFileUploader from "devextreme-vue/file-uploader";
import DxValidator from "devextreme-vue/validator";
import DxTextBox from "devextreme-vue/text-box";
import VueStepProgressIndicator from "vue-step-progress-indicator";
import Vuetable from "vuetable-2/src/components/Vuetable";
import axios from "axios";

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
      watch: "",
      processing: false,
      fileUploading: false,
      isActive: false,
      date: "",
      formatted: "",
      dateSelected: "",
      ProgramSelected: [],
      typeOptions: [
        { value: "null", text: "소재 유형" },
        { value: "private", text: "My디스크" },
        { value: "program", text: "프로그램" },
        { value: "mcrspot", text: "주조SPOT" },
        { value: "scrspot", text: "부조SPOT" },
        { value: "static", text: "고정소재" },
        { value: "var", text: "변동소재" },
        { value: "report", text: "취재물" },
        { value: "filler", text: "필러" }
      ],
      mediaOptions: [],
      vueTableWidth: "220px",
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
          width: "8%"
        },
        {
          name: "__slot:fileSize",
          title: "파일 크기",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "4%"
        },
        {
          name: "__slot:title",
          title: "제목",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%"
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
    ...mapGetters("menu", ["getMenuType"])
  },
  watch: {
    MetaData: {
      deep: true,
      handler(v, ov) {
        if (v.typeSelected == "program") {
          this.isActive = true;
        } else {
          this.isActive = false;
        }

        if (
          v.typeSelected == "program" ||
          v.typeSelected == "mcrspot" ||
          v.typeSelected == "static" ||
          v.typeSelected == "var"
        ) {
          if (this.watch != v.typeSelected) {
            this.mediaOptions = [];
            axios.get("/api/categories/media").then(res => {
              res.data.resultObject.data.forEach(e => {
                this.mediaOptions.push({ value: e.id, text: e.name });
              });
              this.watch = v.typeSelected;
            });
          }
        }
      }
    }
  },

  methods: {
    ...mapActions("file", ["verifyMeta", "uploadRefresh"]),
    ...mapMutations("FileIndexStore", [
      "setUploaderCustomData",
      "setProgramData",
      "resetTitle",
      "resetMemo",
      "resetType"
    ]),
    fileStateFalse() {
      this.processing = false;
      this.fileUploading = false;
    },
    onSelectionChanged(v) {
      this.ProgramSelected = v.selectedRowsData[0];
    },
    getPro() {
      const replaceVal = this.date.replace(/-/g, "");
      const yyyy = replaceVal.substring(0, 4);
      const mm = replaceVal.substring(4, 6);
      const dd = replaceVal.substring(6, 8);
      var date = yyyy + "" + mm + "" + dd;
      axios
        .get(
          `/api/categories/pgm-sch?media=${this.MetaData.mediaSelected}&date=${date}`
        )
        .then(res => {
          var value = res.data.resultObject.data;
          value.forEach(e => {
            e.durationSec = this.getDurationSec(e.durationSec);

            e.onairTime = this.getOnAirTime(e.onairTime);
          });
          this.setProgramData(res.data.resultObject.data);
        });
    },
    getOnAirTime(date) {
      var d = date.substring(0, 10);
      var t = date.substring(11, 19);
      return d + " " + t;
    },
    getDurationSec(sec) {
      var sec_num = parseInt(sec, 10); // don't forget the second param
      var hours = Math.floor(sec_num / 3600);
      var minutes = Math.floor((sec_num - hours * 3600) / 60);
      var seconds = sec_num - hours * 3600 - minutes * 60;

      if (hours < 10) {
        hours = "0" + hours;
      }
      if (minutes < 10) {
        minutes = "0" + minutes;
      }
      if (seconds < 10) {
        seconds = "0" + seconds;
      }
      return hours + ":" + minutes + ":" + seconds;
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
      this.date = "";
    },
    //#endregion

    reset() {
      this.resetTitle();
      this.resetMemo();
      this.resetType();
      this.dateReset();
      this.fileStateFalse();
      this.fileSelect = false;
      if (this.processing) {
        this.$fn.notify("error", { message: "파일 업로드 취소" });
      }
    }
  }
};
