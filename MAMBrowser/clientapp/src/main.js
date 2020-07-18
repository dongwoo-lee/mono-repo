import Vue from 'vue'
import App from './App'

// BootstrapVue add
import BootstrapVue from 'bootstrap-vue'
// Router & Store add
import router from './router'
import store from './store'
// Multi Language Add
import messages from './locales/index'
import VueI18n from 'vue-i18n'
import { defaultLocale, localeOptions } from './constants/config'
// Notification Component Add
import Notifications from './components/Common/Notification'
// Breadcrumb Component Add
import Breadcrumb from './components/Common/Breadcrumb'
// RefreshButton Component Add
import RefreshButton from './components/Common/RefreshButton'
// Colxx Component Add
import Colxx from './components/Common/Colxx'
// Perfect Scrollbar Add
import vuePerfectScrollbar from 'vue-perfect-scrollbar'
import contentmenu from 'v-contextmenu'
import VueLineClamp from 'vue-line-clamp'
import VueScrollTo from 'vue-scrollto'
// vue-upload-component
import VueUploadComponent from 'vue-upload-component';

import http from './http.js'
import commonFunctions from './utils/CommonFunctions';

Vue.use(BootstrapVue);
Vue.use(VueI18n);
// const messages = { ko: ko, en: en };
const locale = (localStorage.getItem('currentLanguage') && localeOptions.filter(x => x.id === localStorage.getItem('currentLanguage')).length > 0) ? localStorage.getItem('currentLanguage') : defaultLocale;
const i18n = new VueI18n({
  locale: locale,
  fallbackLocale: 'en',
  messages
});
Vue.use(Notifications);
Vue.use(require('vue-shortkey'));
Vue.use(contentmenu);
Vue.use(VueScrollTo);
Vue.use(VueLineClamp, {
  importCss: true
});

Vue.use({ install(Vue) { Vue.prototype.$http = http } })

Vue.component('piaf-breadcrumb', Breadcrumb);
Vue.component('b-refresh-button', RefreshButton);
Vue.component('b-colxx', Colxx);
Vue.component('vue-perfect-scrollbar', vuePerfectScrollbar);
Vue.component('file-upload', VueUploadComponent)

Vue.prototype.$fn = commonFunctions;

Vue.config.productionTip = false

export default new Vue({
  i18n,
  router,
  store,
  render: h => h(App)
}).$mount('#app')
