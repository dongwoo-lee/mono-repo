import Vue from 'vue'
import App from './App'

import { BootstrapVue, BootstrapVueIcons } from 'bootstrap-vue'
import router from './router'
import store from './store'
import messages from './locales/index'
import VueI18n from 'vue-i18n'
import { defaultLocale, localeOptions } from './constants/config'
import Notifications from './components/Common/Notification'
import Breadcrumb from './components/Common/Breadcrumb'
import Colxx from './components/Common/Colxx'
import vuePerfectScrollbar from 'vue-perfect-scrollbar'
import contentmenu from 'v-contextmenu'
import VueUploadComponent from 'vue-upload-component';
import Vuelidate from 'vuelidate';

import http from './http.js'
import commonFunctions from './utils/CommonFunctions';
import commonFilters from './utils/CommonFilters';
import './components/index';

Vue.use(BootstrapVue);
Vue.use(BootstrapVueIcons);
Vue.use(VueI18n);
// const messages = { ko: ko, en: en };
const locale = (localStorage.getItem('currentLanguage') && localeOptions.filter(x => x.id === localStorage.getItem('currentLanguage')).length > 0) ? localStorage.getItem('currentLanguage') : defaultLocale;
const i18n = new VueI18n({
  locale: locale,
  fallbackLocale: 'en',
  messages
});
Vue.use(Notifications);
Vue.use(contentmenu);
Vue.use(Vuelidate);
Vue.use({ install(Vue) { Vue.prototype.$http = http } })

Vue.component('piaf-breadcrumb', Breadcrumb);
Vue.component('b-colxx', Colxx);
Vue.component('vue-perfect-scrollbar', vuePerfectScrollbar);
Vue.component('file-upload', VueUploadComponent)

Vue.prototype.$fn = commonFunctions;

Object.keys(commonFilters).forEach((key) => {
  Vue.filter(key, commonFilters[key]);
});

Vue.config.productionTip = false

export default new Vue({
  i18n,
  router,
  store,
  render: h => h(App)
}).$mount('#app')
