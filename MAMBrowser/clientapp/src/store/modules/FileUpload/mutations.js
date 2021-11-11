export default {
  addLocalFiles(state, payload) {
    state.localFiles.push(payload);
  },
  setDate(state, payload) {
    state.date = payload;
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
  setVueTableData(state, payload) {
    state.vueTableData.push(payload);
  },
  setFileMediaOptions(state, payload) {
    state.fileMediaOptions.push(payload);
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
  resetType(state) {
    state.MetaData.typeSelected = "null";
  },
  resetFileMediaOptions(state) {
    state.fileMediaOptions = [];
  },
  resetProgramData(state) {
    for (var i = 1; i < state.ProgramData.length; ) {
      state.ProgramData.pop();
    }
    state.ProgramData.eventName = "";
    state.ProgramData.eventType = "";
    state.ProgramData.productId = "";
    state.ProgramData.onairTime = "";
    state.ProgramData.durationSec = "";
  },
  resetProgramSelected(state) {
    state.ProgramSelected = {
      eventName: "",
      eventType: "",
      productId: "",
      onairTime: "",
      durationSec: ""
    };
  },
  resetEventSelected(state) {
    state.EventSelected = {
      id: "",
      name: ""
    };
  },
  resetEventData(state) {
    state.EventData = [
      {
        name: "",
        id: ""
      }
    ];
  },
  forEachVueTableData(state, payload) {
    state.vueTableData.forEach(element => {
      if (payload.title == element.title) {
        element.step = payload.step;
      }
    });
  }
};
