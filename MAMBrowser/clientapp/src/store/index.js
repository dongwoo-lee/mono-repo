import Vue from "vue";
import Vuex from "vuex";

import app from "../main";
import menu from "./modules/menu";
import user from "./modules/user";
import file from "./modules/file";
import ScrSpotDuration from "./modules/ScrSpotDuration";
import cueList from "./modules/cueList";
import monitoring from "./modules/monitoring";
import createPersistedState from "vuex-persistedstate";

import FileIndexStore from "./modules/FileUpload/index";
Vue.use(Vuex);

const store = new Vuex.Store({
  state: {},
  plugins: [
    createPersistedState({
      paths: ["cuesheet"],
    }),
  ],
  mutations: {
    changeLang(state, payload) {
      app.$i18n.locale = payload;
      localStorage.setItem("currentLanguage", payload);
    },
  },
  actions: {
    setLang({ commit }, payload) {
      commit("changeLang", payload);
    },
  },
  modules: {
    menu,
    user,
    file,
    cueList,
    FileIndexStore,
    ScrSpotDuration,
    monitoring,
  },
});

export default store;
