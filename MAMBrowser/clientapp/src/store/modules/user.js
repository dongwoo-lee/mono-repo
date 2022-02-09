import $http from "@/http.js";
import $fn from "../../utils/CommonFunctions";
import url from "@/constants/url";
import jwt_decode from "jwt-decode";
import {
  USER_ID,
  USER_NAME,
  ROLE,
  ACCESS_TOKEN,
  AUTHORITY,
  SYSTEM_MANAGEMENT_CODE,
  AUTHORITY_ADMIN,
  AUTHORITY_MANAGER,
  SYSTEM_TOP_ADMIN_CODE,
  ACCESS_GROP_ID,
} from "@/constants/config";

// URL주소와 ICON 요소 추가 & Role 데이터 생성
const getAddUrlAndIconMenuList = (menuList, roleList) => {
  menuList.forEach((menus) => {
    url.forEach((item) => {
      // 자식요소
      if (menus.children && menus.children.length > 0) {
        menus.children.forEach((menu) => {
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
  const findIndex = behaviorList.findIndex(
    (behavior) => behavior.id === SYSTEM_MANAGEMENT_CODE
  );
  if (findIndex > -1 && behaviorList[findIndex].visible === "Y") {
    return AUTHORITY_ADMIN;
  }
  return AUTHORITY_MANAGER;
};

const getRole = (menu) => {
  return {
    parentID: menu.parentID,
    id: menu.id,
    visible: menu.visible,
    enable: menu.enable,
    to: menu.to,
  };
};

export default {
  namespaced: true,
  state: {
    currentUser: {
      name: "",
      menuGrpName: "",
      diskMax: 0,
      diskAvailable: 0,
      diskUsed: 0,
    },
    menuList: [],
    behaviorList: [],
    roleList: [],
    processing: false,
    callLoginAuthTryCnt: 0,
    tokenExpires: 0,
    timerProccessing: false,
    authority: AUTHORITY_MANAGER,
    timer: 0
  },
  getters: {
    getMenuGrpName: (state) => state.currentUser.menuGrpName,
    menuList: (state) => state.menuList,
    roleList: (state) => state.roleList,
    behaviorList: (state) => state.behaviorList,
    currentUser: (state) => state.currentUser,
    processing: (state) => state.processing,
    diskAvailable: (state) => state.currentUser.diskAvailable,
    isAuth: () =>
      sessionStorage.getItem(ACCESS_TOKEN) != null &&
      sessionStorage.getItem(USER_ID) != null,
    userId: () => sessionStorage.getItem(USER_ID),
    tokenExpires: (state) => state.tokenExpires,
    timerProccessing: (state) => state.timerProccessing,
    conDBName: (state) => state.currentUser.conDBName,
    conNetworkName: (state) => state.currentUser.conNetworkName,
    isSystemTopAdmin: (state) =>
      state.currentUser.authorCD === SYSTEM_TOP_ADMIN_CODE,
    timer: (state) => state.timer
  },
  mutations: {
    SET_TOKEN(state, token) {
      // token 시간 만료(초) 설정
      const { exp } = jwt_decode(token);
      const now = Date.now() / 1000;
      const timeDiff = exp - now;
      state.tokenExpires = timeDiff;
      state.timerProccessing = true;
      sessionStorage.setItem(ACCESS_TOKEN, token);
    },
    SET_AUTH(state, resultObject) {
      const { menuList, behaviorList, id, name, menuGrpID } = resultObject;
      state.isAuth = true;
      state.processing = false;
      state.roleList = [];
      state.behaviorList = behaviorList;
      state.menuList = getAddUrlAndIconMenuList(menuList, state.roleList);
      delete resultObject.menuList;
      delete resultObject.behaviorList;
      state.currentUser = resultObject;
      state.authority = getAuthority(state.behaviorList);

      sessionStorage.setItem(USER_ID, id);
      sessionStorage.setItem(USER_NAME, name);
      sessionStorage.setItem(ROLE, JSON.stringify(state.roleList));
      sessionStorage.setItem(AUTHORITY, state.authority);
      //큐시트 작성 및 조회 권한 세션 등록
      sessionStorage.setItem(ACCESS_GROP_ID, menuGrpID);
    },
    SET_SUMMARY_USER(state, resultObject) {
      delete resultObject.menuList;
      delete resultObject.behaviorList;
      state.currentUser = resultObject;
    },
    SET_LOGOUT(state) {
      state.isAuth = false;
      state.currentUser = {};
      state.processing = false;
      state.timerProccessing = false;
      sessionStorage.removeItem(ACCESS_TOKEN);
      sessionStorage.removeItem(USER_ID);
      sessionStorage.removeItem(ROLE);
      sessionStorage.removeItem(AUTHORITY);
      sessionStorage.removeItem(USER_NAME);
      sessionStorage.removeItem(ACCESS_GROP_ID);
    },
    SET_REMOVE_TOKEN(state) {
      state.isAuth = false;
      sessionStorage.removeItem(ACCESS_TOKEN);
    },
    SET_PROCESSING(state, payload) {
      state.processing = payload;
    },
    SET_MENU(state, payload) {
      state.menuList = payload;
    },
    SET_CALL_LOGIN_AUTH_TRY_CNT(state) {
      state.callLoginAuthTryCnt += 1;
    },
    SET_INIT_CALL_LOGIN_AUTH_TRY_CNT(state) {
      state.callLoginAuthTryCnt = 0;
    },
    SET_INIT_TOKEN_TIMER(state, payload) {
      state.timerProccessing = payload;
      state.tokenExpires = 0;
    },
    SET_TIMER(state, payload) {
      state.timer = payload
    }
  },
  actions: {
    async login({ commit }, payload) {
      commit("SET_PROCESSING", true);
      commit("SET_CALL_LOGIN_AUTH_TRY_CNT");

      const params = {
        UserID: payload.userId,
        Pass: payload.pass,
      };

      try {
        const response = await $http.post("/api/Authenticate", params);
        const { resultCode, resultObject, token } = response.data;
        if (resultObject && resultCode === 0) {
          commit("SET_TOKEN", token);
          commit("SET_AUTH", resultObject);
        }
        commit("SET_PROCESSING", false);
        return response;
      } catch (error) {
        return error;
      }
    },
    async renewal({ state, commit }) {
      if (state.callLoginAuthTryCnt > 0) {
        return;
      }
      commit("SET_INIT_TOKEN_TIMER", false);
      const params = {
        UserID: sessionStorage.getItem(USER_ID),
        Pass: "undefined",
      };

      try {
        const response = await $http.post("/api/Renewal", params);
        const { resultCode, resultObject, token } = response.data;

        if (resultObject && resultCode === 0) {
          commit("SET_TOKEN", token);
          commit("SET_AUTH", resultObject);
        } else {
          $fn.notify("error", {
            message: "사용자 정보 조회 실패: " + data.errorMsg,
          });
        }
        return response;
      } catch (error) {
        console.error(error);
        return error;
      }
    },
    async reissue({ commit }, from) {
      // 페이지 이동 조건
      if (!from.name || from.path == "/") {
        return;
      }
      commit("SET_INIT_TOKEN_TIMER", false);

      const params = {
        UserID: sessionStorage.getItem(USER_ID),
        Pass: "undefined",
      };

      try {
        const response = await $http.post("/api/Reissue", params);
        const { resultCode, token } = response.data;

        if (resultCode === 0) {
          commit("SET_TOKEN", token);
        } else {
          $fn.notify("error", {
            message: "사용자 인증에 실패: " + data.errorMsg,
          });
        }

        return response.data.resultCode;
      } catch (error) {
        console.error(error);
        return error;
      }
    },
    getSummaryUser({ getters, commit }) {
      $http.get(`/api/users/summary/${getters.userId}`).then((response) => {
        const { resultCode, resultObject } = response.data;
        if (resultObject && resultCode === 0) {
          commit("SET_SUMMARY_USER", resultObject);
        }
      });
    },
    getMenu({ commit }, userId) {
      $http.get(`/api/users/${userId}/menu`).then((response) => {
        const { status, data } = response;
        if (status === 200 && data.resultObject && data.resultCode === 0) {
          commit("SET_MENU", data.resultObject.data);
        }
      });
    },
  },
};
