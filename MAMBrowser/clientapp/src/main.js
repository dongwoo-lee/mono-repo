import "core-js/modules/es.array.iterator";
import Vue from "vue";
import App from "./App";

import { BootstrapVue, BootstrapVueIcons } from "bootstrap-vue";
import router from "./router";
import store from "./store";
import messages from "./locales/index";
import VueI18n from "vue-i18n";
import Notifications from "./components/Common/Notification";
import contentmenu from "v-contextmenu";
import Vuelidate from "vuelidate";
import http from "./http.js";
import commonFunctions from "./utils/CommonFunctions";
import commonFilters from "./utils/CommonFilters";
import vSelect from "vue-select";
import "vue-select/dist/vue-select.css";
import "devextreme/dist/css/dx.light.css";
import "./components/index";

Vue.component("v-select", vSelect);
Vue.use(BootstrapVue);
Vue.use(BootstrapVueIcons);
Vue.use(VueI18n);
Vue.use(Notifications);
Vue.use(contentmenu);
Vue.use(Vuelidate);
Vue.use({
  install(Vue) {
    Vue.prototype.$http = http;
  }
});

Vue.prototype.$fn = commonFunctions;
Object.keys(commonFilters).forEach(key => {
  Vue.filter(key, commonFilters[key]);
});
const i18n = new VueI18n({ locale: "ko", fallbackLocale: "en", messages });

Vue.config.productionTip = false;
Vue.config.errorHandler = function(err, vm, info) {
  console.log(`Error: ${err.toString()}\nInfo: ${info}`);
};

export default new Vue({
  i18n,
  router,
  store,
  render: h => h(App)
}).$mount("#app");
