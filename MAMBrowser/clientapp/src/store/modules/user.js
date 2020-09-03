import $http from '@/http.js'

export default {
  namespaced: true,
  state: {
    currentUser: '',
    processing: false,
  },
  getters: {
    currentUser: state => state.currentUser,
    processing: state => state.processing,
  },
  mutations: {
    setUser(state, {token, userId, userExtId}) {
      state.isAuth = true;
      state.currentUser = userId;
      state.processing = false;
      sessionStorage.setItem('access_token', token);
      sessionStorage.setItem('user_ext_id', userExtId);
      $http.defaults.headers.common['X-Csrf-Token'] = token;
    },
    setLogout(state) {
      state.isAuth = false;
      state.currentUser = null;
      state.processing = false;
      sessionStorage.removeItem('access_token')
      sessionStorage.removeItem('user_ext_id')
      $http.defaults.headers.common['X-Csrf-Token'] = undefined;
    },
    setProcessing(state, payload) {
      state.processing = payload
    },
  },
  actions: {
    async login({ commit }, payload) {
      commit('setProcessing', true);

      const params = {
        UserID: payload.userId,
        Pass: payload.pass
      };

      try {
        const response = await $http.post('/api/Authenticate', params);
        if (response.data.resultCode === 0) {
          commit('setUser', { token: response.data.token, userId: payload.userId, userExtId: '159' });
        }
        commit('setProcessing', false);
        return response;
      } catch (error) {
        return error;
      }
    },
    signOut({ commit }) {
      commit('setLogout');
      return true;
    }
  }
}
