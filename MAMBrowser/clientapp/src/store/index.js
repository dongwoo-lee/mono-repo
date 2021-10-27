import Vue from "vue";
import Vuex from "vuex";

import app from "../main";
import menu from "./modules/menu";
import user from "./modules/user";
import file from "./modules/file";
import FileStore from "./modules/FileStore";
Vue.use(Vuex);

const store = new Vuex.Store({
  state: {},
  mutations: {
    changeLang(state, payload) {
      app.$i18n.locale = payload;
      localStorage.setItem("currentLanguage", payload);
    }
  },
  actions: {
    setLang({ commit }, payload) {
      commit("changeLang", payload);
    }
  },
  modules: {
    menu,
    user,
    file,
    FileStore
  }
});

export default store;
