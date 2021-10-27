export default {
  namespaced: true,
  state: {
    MetaModal: false,
    MetaModalTitle: "",
    // MetaData: {
    //   title: "",
    //   memo: "",
    //   type: "",
    //   mediaCD: "",
    //   categoryCD: ""
    // },
    localFiles: [],
    uploaderCustomData: {},
    connectionId: ""
  },
  getters: {},
  mutations: {
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
    }
  },
  actions: {}
};
