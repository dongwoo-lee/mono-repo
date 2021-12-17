let db;
import axios from "axios";
export default {
  addLocalFiles(state, payload) {
    state.localFiles.push(payload);
  },
  setFileModal(state, payload) {
    state.FileModal = payload;
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
  setMediaSelected(state, payload) {
    state.MetaData.mediaSelected = payload;
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
  resetFileMediaOptions(state) {
    state.fileMediaOptions = [];
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
            if (e.category == 0) {
              e.category = "My 디스크";
            } else if (e.category == 1) {
              e.category = "프로소재";
            } else if (e.category == 2) {
              e.category = "프로그램";
            } else if (e.category == 3) {
              e.category = "주조SPOT";
            } else if (e.category == 4) {
              e.category = "부조SPOT";
            } else if (e.category == 5) {
              e.category = "FILLER";
            } else if (e.category == 6) {
              e.category = "취재물";
            } else if (e.category == 7) {
              e.category = "고정소재";
            } else if (e.category == 8) {
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
    }, 1000);
  },
  stopDBConnection() {
    clearInterval(db);
  },
};
