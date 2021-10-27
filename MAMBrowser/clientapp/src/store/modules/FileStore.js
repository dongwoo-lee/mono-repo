export default {
  namespaced: true,
  state: {
    MetaModal: false,
    // MetaData: {
    //   title: "",
    //   memo: "",
    //   type: "",
    //   mediaCD: "",
    //   categoryCD: ""
    // },
    localFiles: [],
    uploaderCustomData: {}
  },
  getters: {},
  mutations: {
    MetaModalOn(state) {
      state.MetaModal = true;
    },
    MetaModalOff(state) {
      state.MetaModal = false;
    }
  },
  actions: {}
};
