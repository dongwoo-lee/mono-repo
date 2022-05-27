import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
import { DxDataGrid, DxColumn, DxSelection } from "devextreme-vue/data-grid";

export default {
  components: {
    DxDataGrid,
    DxColumn,
    DxSelection,
  },
  data() {
    return {
      role: "",
      formatted: "",
      dateSelected: "",
      logTableHeight: "360px",
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
      type: (state) => state.type,
      MetaModalTitle: (state) => state.MetaModalTitle,
      localFiles: (state) => state.localFiles,
      masteringListData: (state) => state.masteringListData,
      masteringLogData: (state) => state.masteringLogData,
      isActive: (state) => state.isActive,
      fileUploading: (state) => state.fileUploading,
    }),
    ...mapGetters("FileIndexStore", [
      "typeState",
      "durationState",
      "audioClipIdState",
      "metaValid",
    ]),
    ...mapGetters("user", ["getMenuGrpName"]),
    getVariant() {
      return this.isActive ? "outline-dark" : "outline-dark";
    },
    ...mapGetters("menu", ["getMenuType"]),
  },
  created() {
    this.role = sessionStorage.getItem("authority");
  },
  watch: {
    type: {
      handler(v) {
        if (v == "null" || v == "my-disk") {
          this.setIsActive(true);
        } else {
          this.setIsActive(false);
        }
      },
    },
  },
  methods: {
    ...mapActions("file", ["verifyMeta", "uploadRefresh"]),
    ...mapMutations("FileIndexStore", ["setFileUploading", "setIsActive"]),
    fileStateFalse() {
      this.setFileUploading(false);
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
    onContext(ctx) {
      this.formatted = ctx.selectedFormatted;
      this.dateSelected = ctx.selectedYMD;
    },
  },
};
