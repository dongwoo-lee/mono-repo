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
  forEachVueTableData(state, payload) {
    state.vueTableData.forEach(element => {
      if (payload.title == element.title) {
        element.step = payload.step;
      }
    });
  }
};
