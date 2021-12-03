let db;
import axios from "axios";
export default {
  addLocalFiles(state, payload) {
    state.localFiles.push(payload);
  },
  setFileModal(state, payload) {
    state.FileModal = payload;
  },
  setDate(state, payload) {
    state.date = payload;
  },
  setFileSDate(state, payload) {
    state.fileSDate = payload;
  },
  setFileEDate(state, payload) {
    state.fileEDate = payload;
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
  setEventSelected(state, payload) {
    state.EventSelected = payload;
  },
  setEventData(state, payload) {
    state.EventData = payload;
  },
  setEditor(state, payload) {
    state.MetaData.editor = payload;
  },
  setProgramState(state, payload) {
    state.programState = payload;
  },
  resetDate(state) {
    state.date = "";
  },
  resetFileSDate(state) {
    state.fileSDate = "";
  },
  resetFileEDate(state) {
    state.fileEDate = "";
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
  resetEditor(state) {
    state.MetaData.editor = "";
  },
  resetReporter(state) {
    state.MetaData.reporter = "";
  },
  resetUsage(state) {
    state.MetaData.usage = "";
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
    };
  },
  resetEventData(state) {
    state.EventData = [
      {
        name: "",
        id: "",
      },
    ];
  },
  startDBConnection(state) {
    db = setInterval(() => {
      axios.get("/api/Mastering/mastering-status").then((res) => {
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
        console.log(masteringListData);
        state.masteringListData = masteringListData;
      });
    }, 1000);
  },
  stopDBConnection() {
    clearInterval(db);
  },
};
