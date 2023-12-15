

export default {
  namespaced: true,
  state: {
   connection: null,
  },
  getters: {
    GetMonitoringServerStatus(state) {
      return state.connection?.state == "Connected" ? true : false;
    },
  },
  mutations: {
    SET_CONNECTION(state, param) {
      state.connection = param;
    }
  },
  actions: {
    
  }
};
