import Vue from 'vue'
import CommonConfirm from './popup/CommonConfirm';
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
import Clock from './CommonCustom/clock';
import Timer from './CommonCustom/timer';

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