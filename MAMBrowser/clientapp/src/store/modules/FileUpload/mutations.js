export default {
  MetaModalOn(state) {
    state.MetaModal = true;
  },
  MetaModalOff(state) {
    state.MetaModal = false;
    state.localFiles = [];
  },
  addLocalFiles(state, payload) {
    state.localFiles.push(payload);
  },
  resetLocalFiles(state) {
    state.localFiles = [];
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
  forEachVueTableData(state, payload) {
    state.vueTableData.forEach(element => {
      if (payload.fileName == element.fileName) {
        element.step = payload.step;
      }
    });
  }
};
