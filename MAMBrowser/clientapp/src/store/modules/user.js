import $http from '@/http.js'
import $fn from '../../utils/CommonFunctions';
import url from '@/constants/url';
import {SYSTEM_MANAGEMENT_CODE, AUTHORITY_ADMIN, AUTHORITY_MANAGER} from '@/constants/config';

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

// 시스템 권한 유무 가져오기
const getAuthority = (behaviorList) => {
  const findIndex = behaviorList.findIndex(behavior => behavior.id === SYSTEM_MANAGEMENT_CODE);
  if (findIndex > -1 && behaviorList[findIndex].visible === 'Y') {
    return AUTHORITY_ADMIN;
  }
  return AUTHORITY_MANAGER;
}

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
    callLoginAuthTryCnt: 0,
  },
  getters: {
    menuList: state => state.menuList,
    roleList: state => state.roleList,
    behaviorList: state => state.behaviorList,
    currentUser: state => state.currentUser,
    processing: state => state.processing,
    // isDisplayMyDiskMenu: state => {
    //   console.info('state.roleList', state.roleList);
    //   const findIndex = state.roleList.findIndex(role => role.id === 'S01G01C007' && role.visible === 'Y');
    //   return findIndex > -1;
    // },
    isAuth: () => sessionStorage.getItem('access_token') != null && sessionStorage.getItem('user_id') != null,
    getUserId: () => sessionStorage.getItem('user_id'),
  },
  mutations: {
    SET_AUTH(state, {token, resultObject }) {
      const { menuList, behaviorList, id } = resultObject;  //확인필요
      state.isAuth = true;
      state.processing = false;
      state.roleList = [];
      state.behaviorList = behaviorList;
      state.menuList = getAddUrlAndIconMenuList(menuList, state.roleList);
      delete resultObject.menuList;
      delete resultObject.behaviorList;
      state.currentUser = resultObject;
      
      sessionStorage.setItem('access_token', token);
      sessionStorage.setItem('user_id', id);
      sessionStorage.setItem('role', JSON.stringify(state.roleList));
      sessionStorage.setItem('authority', getAuthority(state.behaviorList));
    },
    SET_LOGOUT(state) {
      state.isAuth = false;
      state.currentUser = {};
      state.processing = false;
      sessionStorage.removeItem('access_token');
      sessionStorage.removeItem('user_id');
      sessionStorage.removeItem('role');
      sessionStorage.removeItem('authority');
    },
    SET_PROCESSING(state, payload) {
      state.processing = payload
    },
    SET_MENU(state, payload) {
      state.menuList = payload;
    },
    SET_CALL_LOGIN_AUTH_TRY_CNT(state) {
      state.callLoginAuthTryCnt += 1;
    }
  },
  actions: {
    async login({ commit }, payload) {
      commit('SET_PROCESSING', true);
      commit('SET_CALL_LOGIN_AUTH_TRY_CNT');

      const params = {
        UserID: payload.userId,
        Pass: payload.pass
      };

      try {
        const response = await $http.post('/api/Authenticate', params);
        const { resultCode, resultObject, token } = response.data;
        if (resultObject && resultCode === 0) {
          commit('SET_AUTH', {
            token: token, 
            resultObject: resultObject
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
    async getUser({ state, commit }) {
      if (state.callLoginAuthTryCnt > 0) { return; }

      const params = {
        UserID: sessionStorage.getItem('user_id'),
        Pass: 'undefined',
      };

      try {
        const response = await $http.post('/api/Renewal', params);
        const { resultCode, resultObject, token } = response.data;

        if (resultObject && resultCode === 0) {
          commit('SET_AUTH', {
            token: token, 
            resultObject: resultObject
          });
        } else {
          $fn.notify('error', { message: '사용자 정보 조회 실패: ' + data.errorMsg })
        }
      } catch(error) {
        console.error(error);
      }
    },
    getMenu({commit}, userId) {
      $http.get(`/api/users/${userId}/menu`).then(response => {
        const { status, data } = response;
          if (status === 200 && data.resultObject && data.resultCode === 0) {
          commit('SET_MENU', data.resultObject.data);
        }
      })
    }
  }
}
