import $http from '@/http.js'
import $fn from '../../utils/CommonFunctions';
import url from '@/constants/url';

// URL주소와 ICON 요소 추가 & Role 데이터 생성
const getAddUrlAndIconMenuList = (menuList, roleList) => {
  menuList.forEach(menus => {
    url.forEach(item => {
      // 자식요소
      if (menus.children && menus.children.length > 0) {
        menus.children.forEach(menu => {
          if (menu && menu.id === item.id) {
            menu.to = item.to;
            menu.icon = item.icon;
            roleList.push(getRole(menu));
          }
        });
      }

      // 부모요소
      if (menus.id === item.id) {
        if (item.to) {
          menus.to = item.to;
          roleList.push(getRole(menus));
        }
        menus.icon = item.icon;
      }
    });
  });

  return menuList;
};

const getRole = (menu) => {
  return {
    parentID: menu.parentID
    , id: menu.id
    , visible: menu.visible
    , enable: menu.enable
    , to: menu.to
  }
}

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
    roleList: [],
    processing: false,
    isDisplayMyDiskMenu: false,
  },
  getters: {
    menuList: state => state.menuList,
    roleList: state => state.roleList,
    behaviorList: state => state.behaviorList,
    currentUser: state => state.currentUser,
    processing: state => state.processing,
    isDisplayMyDiskMenu: state => {
      const findIndex = state.roleList.findIndex(role => role.id === 'S01G01C007' && role.visible === 'Y');
      return findIndex > -1;
    }
  },
  mutations: {
    // 최초 로그인 시점 실행
    SET_AUTH(state, {token, userId, userExtId, role }) {
      state.isAuth = true;
      state.processing = false;
      state.menuList = getAddUrlAndIconMenuList(role, state.roleList);
      sessionStorage.setItem('access_token', token);
      sessionStorage.setItem('user_id', userId);
      sessionStorage.setItem('user_ext_id', userExtId);
      sessionStorage.setItem('role', JSON.stringify(state.roleList));
      $http.defaults.headers.common['X-Csrf-Token'] = token;
    },
    // 새로고침 시점 실행
    SET_USER(state, data) {
      state.menuList = getAddUrlAndIconMenuList(data.menuList, state.roleList);
      state.behaviorList = data.behaviorList;
      delete data.menuList;
      delete data.behaviorList;
      state.currentUser = data;
      sessionStorage.setItem('role', JSON.stringify(state.roleList));
    },
    SET_LOGOUT(state) {
      state.isAuth = false;
      state.currentUser = {};
      state.processing = false;
      sessionStorage.removeItem('access_token');
      sessionStorage.removeItem('user_id');
      sessionStorage.removeItem('user_ext_id');
      sessionStorage.removeItem('role');
      $http.defaults.headers.common['X-Csrf-Token'] = undefined;
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
        console.info('response.data', response.data);
        if (resultObject && resultCode === 0) {
          commit('SET_AUTH', { 
            token: token, 
            userId: resultObject.id, 
            userExtId: resultObject.userExtID,
            role: resultObject.menuList,
          });
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
