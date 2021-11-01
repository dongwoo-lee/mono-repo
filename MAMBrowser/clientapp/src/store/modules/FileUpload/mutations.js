export default {
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
      if (payload.title == element.title) {
        element.step = payload.step;
      }
    });
  }
};
