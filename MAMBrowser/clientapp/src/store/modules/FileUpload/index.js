let db;
import axios from "axios";
export default {
  namespaced: true,
  state: {
    FileModal: false,
    fileUploading: false,
    button: "",
    isActive: true,
    type: "",
    typeOptions: [],
    MetaModalTitle: "",
    duration: "",
    audioFormat: "",
    localFiles: [],
    uploaderCustomData: {},
    //#region 소재 유형
    myDiskMetaData: {
      title: "",
      memo: "",
    },
    pgmMetaData: {
      title: "",
      memo: "",
      media: "",
      date: "",
      tempDate: "",
    },
    pgmMediaOptions: [],
    pgmDataOptions: [
      {
        eventName: "",
        eventType: "",
        productId: "",
        onairTime: "",
        durationSec: "",
        audioClipID: "",
      },
    ],
    userPgmList: [],
    pgmSelected: {
      eventName: "",
      eventType: "",
      productId: "",
      onairTime: "",
      durationSec: "",
      audioClipID: "",
    },
    proMetaData: {
      title: "",
      memo: "",
      category: "",
      type: "",
      typeName: "",
    },
    proCategoryOptions: [],
    mcrMetaData: {
      title: "",
      memo: "",
      media: "",
      date: "",
      tempDate: "",
      advertiser: "",
    },
    mcrMediaOptions: [],
    mcrDataOptions: [
      {
        name: "",
        id: "",
        duration: "",
        audioClipID: "",
      },
    ],
    mcrSelected: {
      id: "",
      name: "",
      duration: "",
      audioClipID: "",
    },
    scrMetaData: {
      title: "",
      memo: "",
      category: "",
      advertiser: "",
    },
    scrCategoryOptions: [],
    scrRange: [],
    staticMetaData: {
      title: "",
      memo: "",
      media: "",
      sDate: "",
      sTempDate: "",
      eDate: "",
      eTempDate: "",
      advertiser: "",
    },
    staticMediaOptions: [],
    staticDataOptions: [],
    staticSelected: {},
    varMetaData: {
      title: "",
      memo: "",
      media: "",
      sDate: "",
      sTempDate: "",
      eDate: "",
      eTempDate: "",
      advertiser: "",
    },
    varMediaOptions: [],
    varDataOptions: [],
    varSelected: {},
    reportMetaData: {
      title: "",
      memo: "",
      media: "",
      category: "",
      date: "",
      tempDate: "",
      reporter: "",
    },
    reportMediaOptions: [],
    reportCategoryOptions: [],
    reportDataOptions: [],
    reportSelected: {
      eventName: "",
      productId: "",
      onairTime: "",
    },
    fillerMetaData: {
      title: "",
      memo: "",
      category: "",
      date: "",
      tempDate: "",
    },
    fillerCategoryOptions: [],
    //#endregion
    masteringListData: [],
    masteringLogData: [],
  },
  getters: {
    //#region
    myDiskTitleState(state) {
      return state.myDiskMetaData.title.length >= 1 ? true : false;
    },
    myDiskMemoState(state) {
      return state.myDiskMetaData.memo.length >= 1 ? true : false;
    },
    pgmSelectedState(state) {
      return state.pgmSelected.productId != "" ? true : false;
    },
    pgmMemoState(state) {
      return state.pgmMetaData.memo.length >= 1 ? true : false;
    },
    proTitleState(state) {
      return state.proMetaData.title.length >= 1 ? true : false;
    },
    proMemoState(state) {
      return state.proMetaData.memo.length >= 1 ? true : false;
    },
    proCategoryState(state) {
      return state.proMetaData.category != "" ? true : false;
    },
    mcrTitleState(state) {
      return state.mcrMetaData.title.length >= 1 ? true : false;
    },
    mcrMemoState(state) {
      return state.mcrMetaData.memo.length >= 1 ? true : false;
    },
    mcrAdvertiserState(state) {
      return state.mcrMetaData.advertiser.length >= 1 ? true : false;
    },
    mcrSelectedState(state) {
      return state.mcrSelected.id != "" ? true : false;
    },
    scrTitleState(state) {
      return state.scrMetaData.title.length >= 1 ? true : false;
    },
    scrMemoState(state) {
      return state.scrMetaData.memo.length >= 1 ? true : false;
    },
    scrAdvertiserState(state) {
      return state.scrMetaData.advertiser.length >= 1 ? true : false;
    },
    scrRangeState(state) {
      return state.scrRange.length >= 1 ? true : false;
    },
    staticMemoState(state) {
      return state.staticMetaData.memo.length >= 1 ? true : false;
    },
    staticAdvertiserState(state) {
      return state.staticMetaData.advertiser.length >= 1 ? true : false;
    },
    staticSelectedState(state) {
      return state.staticSelected.id != "" ? true : false;
    },
    staticSEDateState(state) {
      return state.staticMetaData.sDate.length == 10 &&
        state.staticMetaData.eDate.length == 10
        ? true
        : false;
    },
    varMemoState(state) {
      return state.varMetaData.memo.length >= 1 ? true : false;
    },
    varAdvertiserState(state) {
      return state.varMetaData.advertiser.length >= 1 ? true : false;
    },
    varSelectedState(state) {
      return state.varSelected.id != "" ? true : false;
    },
    varSEDateState(state) {
      return state.varMetaData.sDate.length == 10 &&
        state.varMetaData.eDate.length == 10
        ? true
        : false;
    },
    reportTitleState(state) {
      return state.reportMetaData.title.length >= 1 ? true : false;
    },
    reportMemoState(state) {
      return state.reportMetaData.memo.length >= 1 ? true : false;
    },
    reportReporterState(state) {
      return state.reportMetaData.reporter.length >= 1 ? true : false;
    },
    reportSelectedState(state) {
      return state.reportSelected.id != "" ? true : false;
    },
    fillerTitleState(state) {
      return state.fillerMetaData.title.length >= 1 ? true : false;
    },
    fillerMemoState(state) {
      return state.fillerMetaData.memo.length >= 1 ? true : false;
    },
    fillerDateState(state) {
      return state.fillerMetaData.date.length >= 1 ? true : false;
    },
    //#endregion

    typeState(state) {
      return state.type != "null" ? true : false;
    },
    audioClipIdState(state) {
      if (state.type == "program") {
        return state.pgmSelected.audioClipID != null ? true : false;
      } else if (state.type == "mcr-spot") {
        return state.mcrSelected.audioClipID != null ? true : false;
      }
    },
    durationState(state) {
      var dh = state.duration.slice(0, 2);
      var dm = state.duration.slice(3, 5);
      var ds = state.duration.slice(6, 8);
      var calcD = dh * 60 * 60 + dm * 60 + ds * 1;

      if (state.type == "program" && state.pgmSelected.durationSec != "") {
        var ph = state.pgmSelected.durationSec.slice(0, 2);
        var pm = state.pgmSelected.durationSec.slice(3, 5);
        var ps = state.pgmSelected.durationSec.slice(6, 8);
        var calcP = ph * 60 * 60 + pm * 60 + ps * 1;
        var abs = Math.abs(calcD - calcP);
        if (5 < abs) {
          return false;
        }
      } else if (state.type == "mcr-spot" && state.mcrSelected.id != "") {
        if (state.mcrSelected.duration == null) {
          return true;
        }
        var eh = state.mcrSelected.duration.slice(0, 2);
        var em = state.mcrSelected.duration.slice(3, 5);
        var es = state.mcrSelected.duration.slice(6, 8);
        var calcE = eh * 60 * 60 + em * 60 + es * 1;
        var abs = Math.abs(calcD - calcE);
        if (5 < abs) {
          return false;
        }
      } else if (state.type == "static-spot" && state.staticSelected.id != "") {
        if (state.staticSelected.duration == null) {
          return true;
        }
        var eh = state.staticSelected.duration.slice(0, 2);
        var em = state.staticSelected.duration.slice(3, 5);
        var es = state.staticSelected.duration.slice(6, 8);
        var calcE = eh * 60 * 60 + em * 60 + es * 1;
        var abs = Math.abs(calcD - calcE);
        if (5 < abs) {
          return false;
        }
      } else if (state.type == "var-spot" && state.varSelected.id != "") {
        if (state.varSelected.duration == null) {
          return true;
        }
        var eh = state.varSelected.duration.slice(0, 2);
        var em = state.varSelected.duration.slice(3, 5);
        var es = state.varSelected.duration.slice(6, 8);
        var calcE = eh * 60 * 60 + em * 60 + es * 1;
        var abs = Math.abs(calcD - calcE);
        if (5 < abs) {
          return false;
        }
      }
      return true;
    },
    metaValid(state, getters) {
      if (state.type == "my-disk") {
        if (
          getters.typeState &&
          getters.myDiskTitleState &&
          getters.myDiskMemoState
        )
          return true;
      } else if (state.type == "program") {
        if (getters.pgmSelectedState) {
          return true;
        }
      } else if (state.type == "pro") {
        if (getters.proTitleState && getters.proCategoryState) {
          return true;
        }
      } else if (state.type == "mcr-spot") {
        if (getters.mcrSelectedState) {
          return true;
        }
      } else if (state.type == "scr-spot") {
        if (getters.scrTitleState && getters.scrRangeState) {
          return true;
        }
      } else if (state.type == "static-spot") {
        if (getters.staticSelectedState && getters.staticSEDateState) {
          return true;
        }
      } else if (state.type == "var-spot") {
        if (getters.varSelectedState && getters.varSEDateState) {
          return true;
        }
      } else if (state.type == "report") {
        if (getters.reportReporterState && getters.reportSelectedState) {
          return true;
        }
      } else if (state.type == "filler") {
        if (getters.fillerTitleState && getters.fillerDateState) {
          return true;
        }
      }
      return false;
    },
    getBadge(state) {
      return state.masteringListData.length;
    },
  },
  mutations: {
    //#region MyDisk
    SET_MYDISK_TITLE(state, payload) {
      state.myDiskMetaData.title = payload;
    },
    RESET_MYDISK_METADATA(state) {
      state.myDiskMetaData = {
        title: "",
        memo: "",
      };
    },
    //#endregion

    //#region PGM
    SET_PGM_DATE(state, payload) {
      state.pgmMetaData.date = payload;
    },
    SET_PGM_TEMP_DATE(state, payload) {
      state.pgmMetaData.tempDate = payload;
    },
    SET_PGM_TITLE(state, payload) {
      state.pgmMetaData.title = payload;
    },
    SET_PGM_MEDIA_SELECTED(state, payload) {
      state.pgmMetaData.media = payload;
    },
    SET_PGM_MEDIA_OPTIONS(state, payload) {
      state.pgmMediaOptions.push(payload);
    },
    SET_PGM_DATA_OPTIONS(state, payload) {
      state.pgmDataOptions = payload;
    },
    SET_USER_PGM_LIST(state, payload) {
      state.userPgmList = payload;
    },
    SET_PGM_SELECTED(state, payload) {
      state.pgmSelected = payload;
    },
    RESET_PGM_DATE(state) {
      state.pgmMetaData.date = "";
    },
    RESET_PGM_TEMP_DATE(state) {
      state.pgmMetaData.tempDate = "";
    },
    RESET_PGM_MEDIA_OPTIONS(state) {
      state.pgmMediaOptions = [];
    },
    RESET_PGM_DATA_OPTIONS(state) {
      state.pgmDataOptions = [
        {
          eventName: "",
          eventType: "",
          productId: "",
          onairTime: "",
          durationSec: "",
          audioClipID: "",
        },
      ];
    },
    RESET_PGM_SELECTED(state) {
      state.pgmSelected = {
        eventName: "",
        eventType: "",
        productId: "",
        onairTime: "",
        durationSec: "",
        audioClipID: "",
      };
    },
    RESET_PGM(state) {
      state.pgmMetaData = {
        title: "",
        memo: "",
        media: "",
        date: "",
      };

      state.pgmMediaOptions = [];

      state.pgmDataOptions = [
        {
          eventName: "",
          eventType: "",
          productId: "",
          onairTime: "",
          durationSec: "",
          audioClipID: "",
        },
      ];

      state.pgmSelected = {
        eventName: "",
        eventType: "",
        productId: "",
        onairTime: "",
        durationSec: "",
        audioClipID: "",
      };
    },
    //#endregion

    //#region pro
    SET_PRO_TITLE(state, payload) {
      state.proMetaData.title = payload;
    },
    SET_PRO_CATEGORY(state, payload) {
      state.proMetaData.category = payload;
    },
    SET_PRO_TYPE(state, payload) {
      state.proMetaData.type = payload;
    },
    SET_PRO_TYPE_NAME(state, payload) {
      state.proMetaData.typeName = payload;
    },
    SET_PRO_CATEGORY_OPTIONS(state, payload) {
      state.proCategoryOptions.push(payload);
    },
    RESET_PRO_CATEGORY_OPTIONS(state) {
      state.proCategoryOptions = [];
    },
    RESET_PRO(state) {
      state.proMetaData = {
        title: "",
        memo: "",
        category: "",
        type: "",
        typeName: "",
      };
      state.proCategoryOptions = [];
    },
    //#endregion

    //#region mcr
    SET_MCR_TITLE(state, payload) {
      state.mcrMetaData.title = payload;
    },
    SET_MCR_MEDIA(state, payload) {
      state.mcrMetaData.media = payload;
    },
    SET_MCR_DATE(state, payload) {
      state.mcrMetaData.date = payload;
    },
    SET_MCR_TEMP_DATE(state, payload) {
      state.mcrMetaData.tempDate = payload;
    },
    SET_MCR_MEDIA_OPTIONS(state, payload) {
      state.mcrMediaOptions.push(payload);
    },
    SET_MCR_DATA_OPTIONS(state, payload) {
      state.mcrDataOptions = payload;
    },
    SET_MCR_SELECTED(state, payload) {
      state.mcrSelected = payload;
    },
    RESET_MCR_MEDIA_OPTIONS(state) {
      state.mcrMediaOptions = [];
    },
    RESET_MCR_DATA_OPTIONS(state) {
      state.mcrDataOptions = [
        {
          name: "",
          id: "",
          duration: "",
          audioClipID: "",
        },
      ];
    },
    RESET_MCR_SELECTED(state) {
      state.mcrSelected = {
        name: "",
        id: "",
        duration: "",
        audioClipID: "",
      };
    },
    RESET_MCR(state) {
      state.mcrMetaData = {
        title: "",
        memo: "",
        media: "",
        date: "",
        tempDate: "",
        advertiser: "",
      };
      state.mcrMediaOptions = [];
    },
    //#endregion

    //#region scr
    SET_SCR_TITLE(state, payload) {
      state.scrMetaData.title = payload;
    },
    SET_SCR_CATEGORY(state, payload) {
      state.scrMetaData.category = payload;
    },
    SET_SCR_CATEGORY_OPTIONS(state, payload) {
      state.scrCategoryOptions.push(payload);
    },
    SET_SCR_RANGE(state, payload) {
      state.scrRange.push(payload);
    },
    RESET_SCR_CATEGORY_OPTIONS(state) {
      state.scrCategoryOptions = [];
    },
    RESET_SCR_RANGE(state) {
      state.scrRange = [];
    },
    RESET_SCR(state) {
      state.scrMetaData = {
        title: "",
        memo: "",
        category: "",
        advertiser: "",
      };
      state.scrCategoryOptions = [];
      state.scrRange = [];
    },
    //#endregion

    //#region static
    SET_STATIC_TITLE(state, payload) {
      state.staticMetaData.title = payload;
    },
    SET_STATIC_MEDIA(state, payload) {
      state.staticMetaData.media = payload;
    },
    SET_STATIC_S_DATE(state, payload) {
      state.staticMetaData.sDate = payload;
    },
    SET_STATIC_S_TEMP_DATE(state, payload) {
      state.staticMetaData.sTempDate = payload;
    },
    SET_STATIC_E_DATE(state, payload) {
      state.staticMetaData.eDate = payload;
    },
    SET_STATIC_E_TEMP_DATE(state, payload) {
      state.staticMetaData.eTempDate = payload;
    },
    SET_STATIC_MEDIA_OPTIONS(state, payload) {
      state.staticMediaOptions.push(payload);
    },
    SET_STATIC_DATA_OPTIONS(state, payload) {
      state.staticDataOptions = payload;
    },
    SET_STATIC_SELECTED(state, payload) {
      state.staticSelected = payload;
    },
    RESET_STATIC_MEDIA_OPTIONS(state) {
      state.staticMediaOptions = [];
    },
    RESET_STATIC_DATA_OPTIONS(state) {
      state.staticDataOptions = [];
    },
    RESET_STATIC_SELECTED(state) {
      state.staticSelected = {
        name: "",
        id: "",
        duration: "",
      };
    },
    RESET_STATIC(state) {
      state.staticMetaData = {
        title: "",
        memo: "",
        media: "",
        sDate: "",
        sTempDate: "",
        eDate: "",
        eTempDate: "",
        advertiser: "",
      };
      state.staticMediaOptions = [];
      state.staticDataOptions = [];
      state.staticSelected = {
        name: "",
        id: "",
        duration: "",
      };
    },
    //#endregion

    //#region var
    SET_VAR_TITLE(state, payload) {
      state.varMetaData.title = payload;
    },
    SET_VAR_MEDIA(state, payload) {
      state.varMetaData.media = payload;
    },
    SET_VAR_S_DATE(state, payload) {
      state.varMetaData.sDate = payload;
    },
    SET_VAR_S_TEMP_DATE(state, payload) {
      state.varMetaData.sTempDate = payload;
    },
    SET_VAR_E_DATE(state, payload) {
      state.varMetaData.eDate = payload;
    },
    SET_VAR_E_TEMP_DATE(state, payload) {
      state.varMetaData.eTempDate = payload;
    },
    SET_VAR_MEDIA_OPTIONS(state, payload) {
      state.varMediaOptions.push(payload);
    },
    SET_VAR_DATA_OPTIONS(state, payload) {
      state.varDataOptions = payload;
    },
    SET_VAR_SELECTED(state, payload) {
      state.varSelected = payload;
    },
    RESET_VAR_MEDIA_OPTIONS(state) {
      state.varMediaOptions = [];
    },
    RESET_VAR_DATA_OPTIONS(state) {
      state.varDataOptions = [];
    },
    RESET_VAR_SELECTED(state) {
      state.varSelected = {
        name: "",
        id: "",
        duration: "",
      };
    },
    RESET_VAR(state) {
      state.varMetaData = {
        title: "",
        memo: "",
        media: "",
        sDate: "",
        sTempDate: "",
        eDate: "",
        eTempDate: "",
        advertiser: "",
      };
      state.varMediaOptions = [];
      state.varDataOptions = [];
      state.varSelected = {
        name: "",
        id: "",
        duration: "",
      };
    },
    //#endregion

    //#region report
    SET_REPORT_TITLE(state, payload) {
      state.reportMetaData.title = payload;
    },
    SET_REPORT_MEDIA(state, payload) {
      state.reportMetaData.media = payload;
    },
    SET_REPORT_CATEGORY(state, payload) {
      state.reportMetaData.category = payload;
    },
    SET_REPORT_DATE(state, payload) {
      state.reportMetaData.date = payload;
    },
    SET_REPORT_TEMP_DATE(state, payload) {
      state.reportMetaData.tempDate = payload;
    },
    SET_REPORT_MEDIA_OPTIONS(state, payload) {
      state.reportMediaOptions.push(payload);
    },
    SET_REPORT_CATEGORY_OPTIONS(state, payload) {
      state.reportCategoryOptions.push(payload);
    },
    SET_REPORT_DATA_OPTIONS(state, payload) {
      state.reportDataOptions = payload;
    },
    SET_REPORT_SELECTED(state, payload) {
      state.reportSelected = payload;
    },
    RESET_REPORT_MEDIA_OPTIONS(state) {
      state.reportMediaOptions = [];
    },
    RESET_REPORT_CATEGORY_OPTIONS(state) {
      state.reportCategoryOptions = [];
    },
    RESET_REPORT_DATA_OPTIONS(state) {
      state.reportDataOptions = [];
    },
    RESET_REPORT_SELECTED(state) {
      state.reportSelected = {
        eventName: "",
        productId: "",
        onairTime: "",
      };
    },
    RESET_REPORT(state) {
      state.reportMetaData = {
        title: "",
        memo: "",
        category: "",
        date: "",
        reporter: "",
      };
      state.reportCategoryOptions = [];
      state.reportMediaOptions = [];
      state.reportDataOptions = [];
      state.reportSelected = { eventName: "", productId: "", onairTime: "" };
    },
    //#endregion

    //#region filler
    SET_FILLER_TITLE(state, payload) {
      state.fillerMetaData.title = payload;
    },
    SET_FILLER_DATE(state, payload) {
      state.fillerMetaData.date = payload;
    },
    SET_FILLER_TEMP_DATE(state, payload) {
      state.fillerMetaData.tempDate = payload;
    },
    SET_FILLER_CATEGORY(state, payload) {
      state.fillerMetaData.category = payload;
    },
    SET_FILLER_CATEGORY_OPTIONS(state, payload) {
      state.fillerCategoryOptions.push(payload);
    },
    RESET_FILLER_DATE(state) {
      state.fillerMetaData.date = "";
    },
    RESET_FILLER_TEMP_DATE(state) {
      state.fillerMetaData.tempDate = "";
    },
    RESET_FILLER_CATEGORY_OPTIONS(state) {
      state.fillerCategoryOptions = [];
    },
    RESET_FILLER(state) {
      state.fillerMetaData = {
        title: "",
        memo: "",
        category: "",
        date: "",
        tempDate: "",
      };
      state.fillerCategoryOptions = [];
    },
    //#endregion

    addLocalFiles(state, payload) {
      state.localFiles.push(payload);
    },
    setFileModal(state, payload) {
      state.FileModal = payload;
    },
    setButton(state, payload) {
      state.button = payload;
    },
    setIsActive(state, payload) {
      state.isActive = payload;
    },
    setTypeSelected(state, payload) {
      state.type = payload;
    },
    setFileUploading(state, payload) {
      state.fileUploading = payload;
    },
    setMetaModalTitle(state, payload) {
      state.MetaModalTitle = payload;
    },
    setUploaderCustomData(state, payload) {
      state.uploaderCustomData = payload;
    },
    setMasteringListData(state, payload) {
      state.masteringListData = payload;
    },
    setMasteringLogData(state, payload) {
      state.masteringLogData = payload;
    },
    setDuration(state, payload) {
      state.duration = payload;
    },
    setAudioFormat(state, payload) {
      state.audioFormat = payload;
    },
    resetLocalFiles(state) {
      state.localFiles = [];
    },
    resetType(state) {
      state.type = "null";
    },
    resetTypeOptions(state) {
      state.typeOptions = [];
    },
    resetUploaderCustomData(state) {
      state.uploaderCustomData = {};
    },
    startDBConnection(state, payload) {
      clearInterval(db);
      db = setInterval(() => {
        axios
          .get(`/api/Mastering/mastering-status?user_id=${payload}`)
          .then((res) => {
            var masteringListData = [];
            res.data.resultObject.data.forEach((e) => {
              if (e.category == "MY") {
                e.category = "MY 디스크";
              } else if (e.category == "AC") {
                e.category = "프로소재";
              } else if (e.category == "PM") {
                e.category = "프로그램";
              } else if (e.category == "MS") {
                e.category = "주조SPOT";
              } else if (e.category == "ST") {
                e.category = "부조SPOT";
              } else if (e.category == "FC") {
                e.category = "FILLER";
              } else if (e.category == "RC") {
                e.category = "취재물";
              } else if (e.category == "TT") {
                e.category = "고정소재";
              } else if (e.category == "TS") {
                e.category = "변동소재";
              }
              var data = {
                title: e.title,
                type: e.category,
                user_id: e.regUserName,
                date: e.regDtm,
                step: e.workStatus,
              };
              masteringListData.push(data);
            });
            state.masteringListData = masteringListData;
          });
      }, 500);
    },
    stopDBConnection() {
      clearInterval(db);
    },
  },
};
