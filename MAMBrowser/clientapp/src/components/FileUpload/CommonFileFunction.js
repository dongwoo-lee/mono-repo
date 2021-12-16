import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
import { DxDataGrid, DxColumn, DxSelection } from "devextreme-vue/data-grid";
import commonFunction from "../../utils/CommonFunctions";
import DxFileUploader from "devextreme-vue/file-uploader";
import DxValidator from "devextreme-vue/validator";
import DxTextBox from "devextreme-vue/text-box";
import Vuetable from "vuetable-2/src/components/Vuetable";
import axios from "axios";

export default {
  components: {
    DxDataGrid,
    DxColumn,
    DxSelection,
    commonFunction,
    DxFileUploader,
    DxTextBox,
    DxValidator,
    Vuetable,
  },
  data() {
    return {
      watch: "null",
      role: "",
      formatted: "",
      dateSelected: "",
      userID: sessionStorage.getItem("user_name"),
      logTableHeight: "480px",

      adminLogFields: [
        {
          name: "__slot:title",
          title: "제목",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "20%",
        },
        {
          name: "__slot:type",
          title: "소재유형",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%",
        },
        {
          name: "__slot:user",
          title: "등록자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "18%",
        },
        {
          name: "__slot:date",
          title: "등록일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "20%",
        },
        {
          name: "__slot:silence",
          title: "무음",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%",
        },
        {
          name: "__slot:worker",
          title: "서버",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "7%",
        },
        {
          name: "__slot:status",
          title: "상태",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "20%",
        },
      ],
      userLogFields: [
        {
          name: "__slot:title",
          title: "제목",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "20%",
        },
        {
          name: "__slot:type",
          title: "소재유형",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%",
        },
        {
          name: "__slot:date",
          title: "등록일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "20%",
        },
        {
          name: "__slot:silence",
          title: "무음",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%",
        },
        {
          name: "__slot:worker",
          title: "서버",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "7%",
        },
        {
          name: "__slot:status",
          title: "상태",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "20%",
        },
      ],
    };
  },
  computed: {
    ...mapState("FileIndexStore", {
      MetaModalTitle: (state) => state.MetaModalTitle,
      localFiles: (state) => state.localFiles,
      date: (state) => state.date,
      tempDate: (state) => state.tempDate,
      fileSDate: (state) => state.fileSDate,
      tempFileSDate: (state) => state.tempFileSDate,
      fileEDate: (state) => state.fileEDate,
      tempFileEDate: (state) => state.tempFileEDate,
      MetaData: (state) => state.MetaData,
      fileMediaOptions: (state) => state.fileMediaOptions,
      masteringListData: (state) => state.masteringListData,
      masteringLogData: (state) => state.masteringLogData,
      ProgramData: (state) => state.ProgramData,
      ProgramSelected: (state) => state.ProgramSelected,
      userProgramList: (state) => state.userProgramList,
      EventData: (state) => state.EventData,
      EventSelected: (state) => state.EventSelected,
      isActive: (state) => state.isActive,
      processing: (state) => state.processing,
      fileUploading: (state) => state.fileUploading,
    }),
    ...mapGetters("FileIndexStore", [
      "typeState",
      "titleState",
      "memoState",
      "editorState",
      "reporterState",
      "programState",
      "eventState",
      "SEDateState",
      "advertiserState",
      "durationState",
      "metaValid",
    ]),
    ...mapGetters("user", ["getMenuGrpName"]),
    getVariant() {
      return this.isActive ? "outline-dark" : "outline-dark";
    },
    ...mapGetters("menu", ["getMenuType"]),
    programState() {
      return this.ProgramSelected.productId != "" ? true : false;
    },
  },
  created() {
    this.role = sessionStorage.getItem("authority");
  },
  watch: {
    programState(v) {
      this.setProgramState(v);
    },
    MetaData: {
      deep: true,
      handler(v) {
        if (
          v.typeSelected == "my-disk" ||
          v.typeSelected == "null" ||
          v.typeSelected == "scr-spot" ||
          v.typeSelected == "pro"
        ) {
          this.setIsActive(true);
        } else {
          this.setIsActive(false);
        }
      },
    },
  },

  methods: {
    ...mapActions("file", ["verifyMeta", "uploadRefresh"]),
    ...mapMutations("FileIndexStore", [
      "setUploaderCustomData",
      "setDate",
      "setTempDate",
      "setFileSDate",
      "setTempFileSDate",
      "setFileEDate",
      "setTempFileEDate",
      "setProgramData",
      "setEventData",
      "setProgramState",
      "setIsActive",
      "setProcessing",
      "setFileUploading",
      "setFileMediaOptions",
      "setMediaSelected",
      "setProType",
      "setProTypeName",
      "setProgramSelected",
      "setUserProgramList",
      "setEventSelected",
      "resetDate",
      "resetTempDate",
      "resetFileSDate",
      "resetTempFileSDate",
      "resetFileEDate",
      "resetTempFileEDate",
      "resetTitle",
      "resetMemo",
      "resetReporter",
      "resetType",
      "resetProType",
      "resetProTypeName",
      "resetAdvertiser",
      "resetFileMediaOptions",
      "resetMediaSelected",
      "resetProgramData",
      "resetProgramSelected",
      "resetEventData",
      "resetEventSelected",
    ]),
    fileStateFalse() {
      this.setProcessing(false);
      this.setFileUploading(false);
    },
    onRowClick(v) {
      if (this.MetaData.typeSelected == "program") {
        if (!this.userProgramList.includes(v.data.productId)) {
          this.proDataGrid.deselectRows(v.data.productId);
          this.resetProgramSelected();
          return;
        }
        this.setProgramSelected(v.data);
      } else if (this.MetaData.typeSelected == "mcr-spot") {
        this.setEventSelected(v.data);
      } else if (this.MetaData.typeSelected == "static-spot") {
        this.setEventSelected(v.data);
      } else if (this.MetaData.typeSelected == "var-spot") {
        this.setEventSelected(v.data);
      } else if (this.MetaData.typeSelected == "report") {
        this.setEventSelected(v.data);
      }
    },
    getPro() {
      if (this.MetaData.typeSelected == "program") {
        const replaceVal = this.date.replace(/-/g, "");
        const yyyy = replaceVal.substring(0, 4);
        const mm = replaceVal.substring(4, 6);
        const dd = replaceVal.substring(6, 8);
        var date = yyyy + "" + mm + "" + dd;

        this.resetProgramData();
        axios
          .get(
            `/api/categories/pgm-sch?media=${
              this.MetaData.mediaSelected
            }&date=${20191206}`
          )
          .then((res) => {
            var value = res.data.resultObject.data;
            value.forEach((e) => {
              e.durationSec = this.getDurationSec(e.durationSec);
              e.onairTime = this.getOnAirTime(e.onairTime);
            });
            this.setProgramData(res.data.resultObject.data);
          });
      } else if (this.MetaData.typeSelected == "mcr-spot") {
        const replaceVal = this.date.replace(/-/g, "");
        const yyyy = replaceVal.substring(0, 4);
        const mm = replaceVal.substring(4, 6);
        const dd = replaceVal.substring(6, 8);
        var date = yyyy + "" + mm + "" + dd;
        this.resetEventData();
        axios
          .get(
            `/api/categories/spot-sch?media=${this.MetaData.mediaSelected}&date=${date}&spotType=MS`
          )
          .then((res) => {
            var value = res.data.resultObject.data;
            value.forEach((e) => {
              e.duration = this.getDurationSec(e.duration);
            });
            this.setEventData(res.data.resultObject.data);
          });
      } else if (this.MetaData.typeSelected == "static-spot") {
        const replaceVal = this.fileSDate.replace(/-/g, "");
        const yyyy = replaceVal.substring(0, 4);
        const mm = replaceVal.substring(4, 6);
        const dd = replaceVal.substring(6, 8);
        var date = yyyy + "" + mm + "" + dd;
        this.resetEventData();
        axios
          .get(
            `/api/categories/spot-sch?media=${this.MetaData.mediaSelected}&date=${date}&spotType=TT`
          )
          .then((res) => {
            var value = res.data.resultObject.data;
            value.forEach((e) => {
              e.duration = this.getDurationSec(e.duration);
              e.startDate = this.getStartDate(e.startDate);
            });
            this.setEventData(res.data.resultObject.data);
          });
      } else if (this.MetaData.typeSelected == "var-spot") {
        const replaceVal = this.fileSDate.replace(/-/g, "");
        const yyyy = replaceVal.substring(0, 4);
        const mm = replaceVal.substring(4, 6);
        const dd = replaceVal.substring(6, 8);
        var date = yyyy + "" + mm + "" + dd;
        this.resetEventData();
        axios
          .get(
            `/api/categories/spot-sch?media=${this.MetaData.mediaSelected}&date=${date}&spotType=TS`
          )
          .then((res) => {
            var value = res.data.resultObject.data;
            value.forEach((e) => {
              e.duration = this.getDurationSec(e.duration);
              e.startDate = this.getStartDate(e.startDate);
            });
            this.setEventData(res.data.resultObject.data);
          });
      } else if (this.MetaData.typeSelected == "report") {
        const replaceVal = this.date.replace(/-/g, "");
        const yyyy = replaceVal.substring(0, 4);
        const mm = replaceVal.substring(4, 6);
        const dd = replaceVal.substring(6, 8);
        var date = yyyy + "" + mm + "" + dd;
        this.resetEventData();
        axios.get(`/api/categories/pgmcodes?brd_dt=${date}`).then((res) => {
          this.setEventData(res.data.resultObject.data);
        });
      }
    },
    validDateType(value) {
      const dateRegex = /^(\d{0,4})[-]?\d{0,2}[-]?\d{0,2}$/;
      return !dateRegex.test(value);
    },
    getStartDate(date) {
      var y = date.substring(0, 4);
      var d = date.substring(4, 6);
      var m = date.substring(6, 8);
      return y + "-" + d + "- " + m;
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
    eventInput(event) {
      this.setDate(event);
      this.setTempDate(event);
    },
    onInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        event.target.value = this.tempDate;
        this.$fn.notify("error", { message: "날짜 형식 오류입니다." });
        return;
      }

      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          this.setDate(convertDate);
          this.setTempDate(convertDate);
        }
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
    //#endregion
    reset() {
      this.resetTitle();
      this.resetMemo();
      this.resetDate();
      this.fileStateFalse();
      this.resetProgramData();
      this.resetProgramSelected();
      this.resetEventData();
      this.resetEventSelected();
      this.resetProType();
      this.resetProTypeName();
      this.resetAdvertiser();
      this.resetReporter();
      this.watch = "";
      if (this.processing) {
        this.$fn.notify("error", { message: "파일 업로드 취소" });
      }
    },
    typeReset() {
      this.resetTitle();
      this.resetMemo();
      this.resetType();
      this.resetDate();
      this.fileStateFalse();
      this.resetProgramData();
      this.resetProgramSelected();
      this.resetEventData();
      this.resetEventSelected();
      this.resetProType();
      this.resetProTypeName();
      this.resetAdvertiser();
      this.watch = "";
      if (this.processing) {
        this.$fn.notify("error", { message: "파일 업로드 취소" });
      }
    },
  },
};
