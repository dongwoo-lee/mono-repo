export default {
  namespaced: true,
  state: {
    fileModal: false
  },
  getters: {
    getFileModal: state => state.fileModal
  },
  mutations: {
    openFileModal(state) {
      state.fileModal = true;
    }
  },
  actions: {}
};
