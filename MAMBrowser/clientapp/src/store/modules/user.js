import $http from '@/http.js'
import $fn from '../../utils/CommonFunctions';

export default {
  namespaced: true,
  state: {
    currentUser: {
      name: '',
      menuGrpName: '',
      diskMax: 0,
      diskAvailable: 0,
      diskUsed: 0,
    },
    processing: false,
    userId: '',
  },
  getters: {
    currentUser: state => state.currentUser,
    processing: state => state.processing,
  },
  mutations: {
    SET_AUTH(state, {token, userId }) {
      state.isAuth = true;
      state.processing = false;
      sessionStorage.setItem('user_id', userId);
      sessionStorage.setItem('access_token', token);
      // $http.defaults.headers.common['X-Csrf-Token'] = token;
    },
    SET_USER(state, data) {
      state.currentUser = data;
      sessionStorage.setItem('user_ext_id', data.userExtID);
    },
    SET_LOGOUT(state) {
      state.isAuth = false;
      state.currentUser = {};
      state.processing = false;
      sessionStorage.removeItem('access_token')
      sessionStorage.removeItem('user_ext_id')
      // $http.defaults.headers.common['X-Csrf-Token'] = undefined;
    },
    SET_PROCESSING(state, payload) {
      state.processing = payload
    },
  },
  actions: {
    async login({ commit }, payload) {
      commit('SET_PROCESSING', true);

      const params = {
        UserID: payload.userId,
        Pass: payload.pass
      };

      try {
        const response = await $http.post('/api/Authenticate', params);
        if (response.data.resultCode === 0) {
          commit('SET_AUTH', { token: response.data.token, userId: payload.userId });
        }
        commit('SET_PROCESSING', false);
        return response;
      } catch (error) {
        return error;
      }
    },
    signOut({ commit }) {
      commit('SET_LOGOUT');
      return true;
    },
    getUser({ commit, state }) {
      const userId = sessionStorage.getItem('user_id');
      $http.get(`/api/users/${userId}`).then(response => {
        console.info('response', response);
        const { status, data } = response;
        if (status === 200 && data.resultCode === 0) {
          commit('SET_USER', data.resultObject);
        } else {
          $fn.notify('error', { message: '사용자 정보 조회 실패: ' + data.errorMsg })
        }
      })
      .catch(error => {
        console.error(error);
      });
    }
  }
}
