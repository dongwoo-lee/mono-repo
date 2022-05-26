let db;
import axios from "axios";
export default {
  namespaced: true,
  state: {
    FileModal: false,
    fileUploading: false,
    button: "",
    isActive: true,
    typeOptions: [],
    MetaModalTitle: "",
    localFiles: [],
    uploaderCustomData: {},

    fileMediaOptions: [],
    coverageTypeOptions: [],
    fillerTypeOptions: [],
    date: "",
    tempDate: "",
    fileSDate: "",
    tempFileSDate: "",
    fileEDate: "",
    tempFileEDate: "",

    myDiskMetaData: {
      title: "",
      memo: "",
      editor: sessionStorage.getItem("user_id"),
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

    MetaData: {
      title: "",
      memo: "",
      reporter: "",
      proType: "",
      proTypeName: "",
      advertiser: "",
      typeSelected: "null",
      mediaSelected: "A",
      coverageTypeSelected: "RC07",
      fillerTypeSelected: "",
      duration: "",
      audioFormat: "",
    },
    userProgramList: [],
    masteringListData: [],
    masteringLogData: [],
    ProgramData: [
      {
        eventName: "",
        eventType: "",
        productId: "",
        onairTime: "",
        durationSec: "",
      },
    ],
    ProgramSelected: {
      eventName: "",
      eventType: "",
      productId: "",
      onairTime: "",
      durationSec: "",
      audioClipID: "",
    },
    EventData: [
      {
        name: "",
        id: "",
        duration: "",
      },
    ],
    EventSelected: {
      id: "",
      name: "",
      duration: "",
      audioClipID: "",
    },
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
    //#endregion

    typeState(state) {
      return state.MetaData.typeSelected != "null" ? true : false;
    },
    dateState(state) {
      return state.date.length == 10 ? true : false;
    },
    SEDateState(state) {
      return state.fileSDate.length == 10 && state.fileEDate.length == 10
        ? true
        : false;
    },
    proMediaState(state) {
      return state.MetaData.mediaSelected != null &&
        state.MetaData.mediaSelected != ""
        ? true
        : false;
    },
    titleState(state) {
      return state.MetaData.title.length >= 1 ? true : false;
    },
    memoState(state) {
      return state.MetaData.memo.length >= 1 ? true : false;
    },
    reporterState(state) {
      return state.MetaData.reporter.length >= 1 ? true : false;
    },
    advertiserState(state) {
      return state.MetaData.advertiser.length >= 1 ? true : false;
    },
    programState(state) {
      return state.ProgramSelected.productId != "" ? true : false;
    },
    eventState(state) {
      return state.EventSelected.id != "" ? true : false;
    },

    audioClipIdState(state) {
      if (state.MetaData.typeSelected == "program") {
        return state.pgmSelected.audioClipID != null ? true : false;
      } else if (state.MetaData.typeSelected == "mcr-spot") {
        return state.mcrSelected.audioClipID != null ? true : false;
      }
    },
    durationState(state) {
      var dh = state.MetaData.duration.slice(0, 2);
      var dm = state.MetaData.duration.slice(3, 5);
      var ds = state.MetaData.duration.slice(6, 8);
      var calcD = dh * 60 * 60 + dm * 60 + ds * 1;

      if (
        state.MetaData.typeSelected == "program" &&
        state.pgmSelected.durationSec != ""
      ) {
        var ph = state.pgmSelected.durationSec.slice(0, 2);
        var pm = state.pgmSelected.durationSec.slice(3, 5);
        var ps = state.pgmSelected.durationSec.slice(6, 8);
        var calcP = ph * 60 * 60 + pm * 60 + ps * 1;
        var abs = Math.abs(calcD - calcP);
        if (5 < abs) {
          return false;
        }
      } else if (
        state.MetaData.typeSelected == "mcr-spot" &&
        state.mcrSelected.id != ""
      ) {
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
      } else if (
        state.MetaData.typeSelected == "static-spot" &&
        state.staticSelected.id != ""
      ) {
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
      } else if (
        state.MetaData.typeSelected == "var-spot" &&
        state.EventSelected.id != ""
      ) {
        if (state.EventSelected.duration == null) {
          return true;
        }
        var eh = state.EventSelected.duration.slice(0, 2);
        var em = state.EventSelected.duration.slice(3, 5);
        var es = state.EventSelected.duration.slice(6, 8);
        var calcE = eh * 60 * 60 + em * 60 + es * 1;
        var abs = Math.abs(calcD - calcE);
        if (5 < abs) {
          return false;
        }
      }
      return true;
    },
    metaValid(state, getters) {
      if (state.MetaData.typeSelected == "my-disk") {
        if (
          getters.typeState &&
          getters.myDiskTitleState &&
          getters.myDiskMemoState
        )
          return true;
      } else if (state.MetaData.typeSelected == "program") {
        if (getters.pgmSelectedState) {
          return true;
        }
      } else if (state.MetaData.typeSelected == "pro") {
        if (getters.proTitleState && getters.proCategoryState) {
          return true;
        }
      } else if (state.MetaData.typeSelected == "mcr-spot") {
        if (getters.mcrSelectedState) {
          return true;
        }
      } else if (state.MetaData.typeSelected == "scr-spot") {
        if (getters.scrTitleState && getters.scrRangeState) {
          return true;
        }
      } else if (state.MetaData.typeSelected == "static-spot") {
        if (getters.staticSelectedState && getters.staticSEDateState) {
          return true;
        }
      } else if (state.MetaData.typeSelected == "var-spot") {
        if (getters.eventState && getters.SEDateState) {
          return true;
        }
      } else if (state.MetaData.typeSelected == "report") {
        if (getters.reporterState && getters.eventState) {
          return true;
        }
      } else if (state.MetaData.typeSelected == "filler") {
        if (getters.titleState && getters.dateState) {
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
        editor: sessionStorage.getItem("user_id"),
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

    addLocalFiles(state, payload) {
      state.localFiles.push(payload);
    },
    setFileModal(state, payload) {
      state.FileModal = payload;
    },
    setButton(state, payload) {
      state.button = payload;
    },
    setTitle(state, payload) {
      state.MetaData.title = payload;
    },
    setDate(state, payload) {
      state.date = payload;
    },
    setTempDate(state, payload) {
      state.tempDate = payload;
    },
    setFileSDate(state, payload) {
      state.fileSDate = payload;
    },
    setFileEDate(state, payload) {
      state.fileEDate = payload;
    },
    setTempFileSDate(state, payload) {
      state.tempFileSDate = payload;
    },
    setTempFileEDate(state, payload) {
      state.tempFileEDate = payload;
    },
    setIsActive(state, payload) {
      state.isActive = payload;
    },
    setTypeSelected(state, payload) {
      state.MetaData.typeSelected = payload;
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
    setFileMediaOptions(state, payload) {
      state.fileMediaOptions.push(payload);
    },
    setCoverageTypeOptions(state, payload) {
      state.coverageTypeOptions.push(payload);
    },
    setFillerTypeOptions(state, payload) {
      state.fillerTypeOptions.push(payload);
    },
    setMediaSelected(state, payload) {
      state.MetaData.mediaSelected = payload;
    },
    setCoverageTypeSelected(state, payload) {
      state.MetaData.coverageTypeSelected = payload;
    },
    setFillerTypeSelected(state, payload) {
      state.MetaData.fillerTypeSelected = payload;
    },
    setProType(state, payload) {
      state.MetaData.proType = payload;
    },
    setProTypeName(state, payload) {
      state.MetaData.proTypeName = payload;
    },
    setDuration(state, payload) {
      state.MetaData.duration = payload;
    },
    setAudioFormat(state, payload) {
      state.MetaData.audioFormat = payload;
    },
    setProgramData(state, payload) {
      state.ProgramData = payload;
    },
    setProgramSelected(state, payload) {
      state.ProgramSelected = payload;
    },
    setUserProgramList(state, payload) {
      state.userProgramList = payload;
    },
    setEventSelected(state, payload) {
      state.EventSelected = payload;
    },
    setEventData(state, payload) {
      state.EventData = payload;
    },
    resetDate(state) {
      state.date = "";
    },
    resetTempDate(state) {
      state.tempDate = "";
    },
    resetFileSDate(state) {
      state.fileSDate = "";
    },
    resetFileEDate(state) {
      state.fileEDate = "";
    },
    resetTempFileSDate(state) {
      state.tempFileSDate = "";
    },
    resetTempFileEDate(state) {
      state.tempFileEDate = "";
    },
    resetLocalFiles(state) {
      state.localFiles = [];
    },
    resetTitle(state) {
      state.MetaData.title = "";
    },
    resetMemo(state) {
      state.MetaData.memo = "";
    },
    resetMediaSelected(state) {
      state.MetaData.mediaSelected = "";
    },
    resetCoverageTypeSelected(state) {
      state.MetaData.coverageTypeSelected = "";
    },
    resetProType(state) {
      state.MetaData.proType = "";
    },
    resetProTypeName(state) {
      state.MetaData.proTypeName = "";
    },
    resetReporter(state) {
      state.MetaData.reporter = "";
    },
    resetAdvertiser(state) {
      state.MetaData.advertiser = "";
    },
    resetType(state) {
      state.MetaData.typeSelected = "null";
    },
    resetTypeOptions(state) {
      state.typeOptions = [];
    },
    resetFileMediaOptions(state) {
      state.fileMediaOptions = [];
    },
    resetCoverageTypeOptions(state) {
      state.coverageTypeOptions = [];
    },
    resetFillerTypeOptions(state) {
      state.fillerTypeOptions = [];
    },
    resetProgramData(state) {
      state.ProgramData = [
        {
          eventName: "",
          eventType: "",
          productId: "",
          onairTime: "",
          durationSec: "",
        },
      ];
    },
    resetProgramSelected(state) {
      state.ProgramSelected = {
        eventName: "",
        eventType: "",
        productId: "",
        onairTime: "",
        durationSec: "",
      };
    },
    resetEventSelected(state) {
      state.EventSelected = {
        id: "",
        name: "",
        duration: "",
      };
    },
    resetEventData(state) {
      state.EventData = [
        {
          name: "",
          id: "",
          duration: "",
        },
      ];
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
