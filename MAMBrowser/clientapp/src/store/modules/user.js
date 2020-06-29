// import firebase from 'firebase/app'
// import 'firebase/auth'
import { currentUser, isAuthActive } from '../../constants/config'
import $http from 'axios'

export default {
  state: {
    currentUser: isAuthActive ? currentUser : (localStorage.getItem('user') != null ? JSON.parse(localStorage.getItem('user')) : null),
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
      // state.user_name = payload.user_name
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
      // commit('setProcessing', true);

      const requestBody = {
        username: payload.email,
        password: payload.password
      };

      try {
        const response = await $http.post('/api/Account/authenticate', JSON.stringify(requestBody),
        {
          headers: { 'Content-Type': 'application/json' }
        });
        commit('setUser', { uid: response.data.username, ...currentUser })
        localStorage.setItem('access_token', response.data.jwtToken)
        localStorage.setItem('user_name', response.data.username)
        return response;
      } catch (error) {
        // sever error
        return error;
      }
    },
    refreshToken({ commit }, payload) {
      $http.post('/api/Account/refresh-token').then(response => {
        console.info('/api/Account/refresh-token', response);
      })
      .error(error => {
        console.info('/api/Account/refresh-token-error', error);
      })
    },
    forgotPassword({ commit }, payload) {
      commit('clearError')
      commit('setProcessing', true)
      // firebase
      //   .auth()
      //   .sendPasswordResetEmail(payload.email)
      //   .then(
      //     user => {
      //       commit('clearError')
      //       commit('setForgotMailSuccess')
      //     },
      //     err => {
      //       commit('setError', err.message)
      //       setTimeout(() => {
      //         commit('clearError')
      //       }, 3000)
      //     }
      //   )
    },
    resetPassword({ commit }, payload) {
      commit('clearError')
      commit('setProcessing', true)
      // firebase
      //   .auth()
      //   .confirmPasswordReset(payload.resetPasswordCode,payload.newPassword)
      //   .then(
      //     user => {
      //       commit('clearError')
      //       commit('setResetPasswordSuccess')
      //     },
      //     err => {
      //       commit('setError', err.message)
      //       setTimeout(() => {
      //         commit('clearError')
      //       }, 3000)
      //     }
      //   )
    },



    /*
       return await auth.(resetPasswordCode, newPassword)
        .then(user => user)
        .catch(error => error);
    */
    signOut({ commit }) {
      commit('setLogout');
      localStorage.removeItem('access_token')
      localStorage.removeItem('user_name')
      return true;
      // firebase
      //   .auth()
      //   .signOut()
      //   .then(() => {
      //     localStorage.removeItem('user')
      //     commit('setLogout')
      //   }, _error => { })
    }
  }
}
