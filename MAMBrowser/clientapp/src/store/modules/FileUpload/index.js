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
    fileMediaOptions: [],
    coverageTypeOptions: [],
    fillerTypeOptions: [],
    MetaModalTitle: "",
    localFiles: [],
    uploaderCustomData: {},
    date: "",
    tempDate: "",
    fileSDate: "",
    tempFileSDate: "",
    fileEDate: "",
    tempFileEDate: "",
    scrRange: [],
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
    FileUploadProgress: {},
  },
  getters: {
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
    scrRangeState(state) {
      return state.scrRange.length >= 1 ? true : false;
    },
    audioClipIdState(state) {
      if (state.MetaData.typeSelected == "program") {
        return state.pgmSelected.audioClipID != null ? true : false;
      } else if (state.MetaData.typeSelected == "mcr-spot") {
        return state.ProgramSelected.audioClipID != null ? true : false;
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
      } else if (
        state.MetaData.typeSelected == "static-spot" &&
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
        if (getters.titleState && getters.proMediaState) {
          return true;
        }
      } else if (state.MetaData.typeSelected == "mcr-spot") {
        if (getters.eventState) {
          return true;
        }
      } else if (state.MetaData.typeSelected == "scr-spot") {
        if (getters.titleState && getters.scrRangeState) {
          return true;
        }
      } else if (state.MetaData.typeSelected == "static-spot") {
        if (getters.eventState && getters.SEDateState) {
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
    RESET_PGM_METADATA(state) {
      state.pgmMetaData = {
        title: "",
        memo: "",
        media: "",
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
    setScrRange(state, payload) {
      state.scrRange.push(payload);
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
    resetScrRange(state) {
      state.scrRange = [];
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
