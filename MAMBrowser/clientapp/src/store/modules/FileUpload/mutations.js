let db;
import axios from "axios";
export default {
  addLocalFiles(state, payload) {
    state.localFiles.push(payload);
  },
  setFileModal(state, payload) {
    state.FileModal = payload;
  },
  setFileSelected(state, payload) {
    state.fileSelected = payload;
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
  setProcessing(state, payload) {
    state.processing = payload;
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
  setMediaName(state, payload) {
    state.MetaData.mediaName = payload;
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
  setProgramState(state, payload) {
    state.programState = payload;
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
  resetMediaName(state) {
    state.MetaData.mediaName = "";
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
              user_id: e.regUserId,
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
};
