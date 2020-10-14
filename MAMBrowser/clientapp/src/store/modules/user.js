import $http from '@/http.js'
import $fn from '../../utils/CommonFunctions';
import url from '@/constants/url';

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
    menuList: [],
    behaviorList: [],
    processing: false,
  },
  getters: {
    menuList: state => { 
      state.menuList.forEach(menus => {
        url.forEach(item => {
          // 자식요소
          if (menus.children && menus.children.length > 0) {
            menus.children.forEach(menu => {
              if (menu && menu.id === item.id) {
                menu.to = item.to;
                menu.icon = item.icon;
              }
            });
          } 

          // 부모요소
          if (menus.id === item.id) {
            if (item.to) {
              menus.to = item.to; 
            }
            menus.icon = item.icon;
          }
        });
      })
      console.info('url', state.menuList);
      return state.menuList;
    },
    behaviorList: state => state.behaviorList,
    currentUser: state => state.currentUser,
    processing: state => state.processing,
  },
  mutations: {
    SET_AUTH(state, {token, userId, userExtId }) {
      state.isAuth = true;
      state.processing = false;
      sessionStorage.setItem('access_token', token);
      sessionStorage.setItem('user_id', userId);
      sessionStorage.setItem('user_ext_id', userExtId);
      // $http.defaults.headers.common['X-Csrf-Token'] = token;
    },
    SET_USER(state, data) {
      state.menuList = data.menuList;
      state.behaviorList = data.behaviorList;
      delete data.menuList;
      delete data.behaviorList;
      state.currentUser = data;
    },
    SET_LOGOUT(state) {
      state.isAuth = false;
      state.currentUser = {};
      state.processing = false;
      sessionStorage.removeItem('access_token')
      sessionStorage.removeItem('user_id');
      sessionStorage.removeItem('user_ext_id')
      // $http.defaults.headers.common['X-Csrf-Token'] = undefined;
    },
    SET_PROCESSING(state, payload) {
      state.processing = payload
    },
    SET_MENU(state, payload) {
      state.menuList = payload;
    }
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
        const { resultCode, resultObject, token } = response.data;
        if (resultObject && resultCode === 0) {
          commit('SET_AUTH', { token: token, userId: resultObject.id, userExtId: resultObject.userExtID });
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
        const { status, data } = response;
        if (status === 200 && data.resultObject && data.resultCode === 0) {
          commit('SET_USER', data.resultObject);
        } else {
          $fn.notify('error', { message: '사용자 정보 조회 실패: ' + data.errorMsg })
        }
      })
      .catch(error => {
        console.error(error);
      });
    },
    getMenu({commit}, userId) {
      $http.get(`/api/users/${userId}/menu`).then(response => {
        const { status, data } = response;
        console.info('getMenu', response);
        if (status === 200 && data.resultObject && data.resultCode === 0) {
          commit('SET_MENU', data.resultObject.data);
        }
      })
    }
  }
}
