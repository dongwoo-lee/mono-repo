import { currentUser, isAuthActive } from '../../constants/config'
import $http from '@/http.js'

export default {
  namespaced: true,
  state: {
    currentUser: currentUser,
    loginError: null,
    processing: false,
    forgotMailSuccess:null,
    resetPasswordSuccess:null
  },
  getters: {
    currentUser: state => state.currentUser,
    processing: state => state.processing,
    loginError: state => state.loginError,
    forgotMailSuccess: state => state.forgotMailSuccess,
    resetPasswordSuccess:state => state.resetPasswordSuccess,
  },
  mutations: {
    setUser(state, payload) {
      state.isAuth = true;
      state.currentUser = payload;
      state.processing = false;
      state.loginError = null;
    },
    setLogout(state) {
      state.isAuth = false;
      state.currentUser = null;
      state.processing = false;
      state.loginError = null;
    },
    setProcessing(state, payload) {
      state.processing = payload
      state.loginError = null
    },
    setError(state, payload) {
      state.loginError = payload
      state.currentUser = null
      state.processing = false
    },
    setForgotMailSuccess(state) {
      state.loginError = null
      state.currentUser = null
      state.processing = false
      state.forgotMailSuccess=true
    },
    setResetPasswordSuccess(state) {
      state.loginError = null
      state.currentUser = null
      state.processing = false
      state.resetPasswordSuccess=true
    },
    clearError(state) {
      state.loginError = null
    }
  },
  actions: {
    async login({ commit }, payload) {
      commit('clearError');
      commit('setProcessing', true);

      const requestBody = {
        username: payload.email,
        password: payload.password
      };

      try {
        // 서버 연결해서 작업할 경우
        // const response = await $http.post('/api/Account/authenticate', JSON.stringify(requestBody),
        // {
        //   headers: { 'Content-Type': 'application/json' }
        // });
        // commit('setUser', { uid: response.data.username, ...currentUser });
        // commit('setProcessing', false);
        // sessionStorage.setItem('access_token', response.data.jwtToken);
        // sessionStorage.setItem('user_name', response.data.username);
        // return response;

        commit('setUser', { uid: 1, ...currentUser });
        sessionStorage.setItem('access_token', 'testtoken');
        sessionStorage.setItem('user_name', 'test');
        return { status: 200 };
      } catch (error) {
        // sever error
        return error;
      }
    },
    refreshToken({ commit }) {
      console.log('refreshToken');
      $http.post('/api/Account/refresh-token');
    },
    forgotPassword({ commit }, payload) {
      commit('clearError')
      commit('setProcessing', true)
    },
    resetPassword({ commit }, payload) {
      commit('clearError')
      commit('setProcessing', true)
    },

    signOut({ commit }) {
      commit('setLogout');
      sessionStorage.removeItem('access_token')
      sessionStorage.removeItem('user_name')
      return true;
    }
  }
}
