export default {
  addLocalFiles(state, payload) {
    state.localFiles.push(payload);
  },
  setMetaModalTitle(state, payload) {
    state.MetaModalTitle = payload;
  },
  setConnectionId(state, payload) {
    state.connectionId = payload;
  },
  setUploaderCustomData(state, payload) {
    state.uploaderCustomData = payload;
  },
  setVueTableData(state, payload) {
    state.vueTableData.push(payload);
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
  resetLocalFiles(state) {
    state.localFiles = [];
  },
  resetTitle(state) {
    state.MetaData.title = "";
  },
  resetMemo(state) {
    state.MetaData.memo = "";
  },
  resetType(state) {
    state.MetaData.typeSelected = "null";
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
  forEachVueTableData(state, payload) {
    state.vueTableData.forEach(element => {
      if (payload.title == element.title) {
        element.step = payload.step;
      }
    });
  }
};
