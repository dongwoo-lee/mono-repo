import Vue from 'vue'

import Breadcrumb from './Common/Breadcrumb'
import Colxx from './Common/Colxx'
import vuePerfectScrollbar from 'vue-perfect-scrollbar'
import VueUploadComponent from 'vue-upload-component';
import PlayerPopup from './Popup/PlayerPopup.vue';
import MusicPlayerPopup from './Popup/MusicPlayerPopup.vue';
import CommonConfirm from './Popup/CommonConfirm';
import CommonForm from './Form/CommonForm';
import CommonDatePicker from './Form/CommonDatePicker';
import CommonStartEndDatePicker from './Form/CommonStartEndDatePicker';
import CommonInputText from './Form/CommonInputText';
import CommonInputTextMaxLength from './Form/CommonInputTextMaxLength';
import CommonTextAreaMaxLength from './Form/CommonTextAreaMaxLength';
import CommonDropdownMenuInput from './Form/CommonDropdownMenuInput';
import CommonDataTable from './DataTable/CommonDataTable';
import CommonDataTableScrollPaging from './DataTable/CommonDataTableScrollPaging';
import CommonActions from './DataTable/CommonActions';
import Player from './Common/Player';
import Clock from './Common/Clock';
import Timer from './Common/Timer';

Vue.component('piaf-breadcrumb', Breadcrumb);
Vue.component('b-colxx', Colxx);
Vue.component('vue-perfect-scrollbar', vuePerfectScrollbar);
Vue.component('file-upload', VueUploadComponent)
Vue.component('PlayerPopup', PlayerPopup)
Vue.component('MusicPlayerPopup', MusicPlayerPopup)
Vue.component('CommonConfirm', CommonConfirm);
Vue.component('CommonForm', CommonForm);
Vue.component('CommonDatePicker', CommonDatePicker);
Vue.component('CommonStartEndDatePicker', CommonStartEndDatePicker);
Vue.component('CommonInputText', CommonInputText);
Vue.component('CommonInputTextMaxLength', CommonInputTextMaxLength);
Vue.component('CommonDropdownMenuInput', CommonDropdownMenuInput);
Vue.component('CommonTextAreaMaxLength', CommonTextAreaMaxLength);
Vue.component('CommonDataTable', CommonDataTable);
Vue.component('CommonDataTableScrollPaging', CommonDataTableScrollPaging);
Vue.component('CommonActions', CommonActions);
Vue.component('Player', Player);
Vue.component('Timer', Timer);
Vue.component('Clock', Clock);

export default {};