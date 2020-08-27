import Vue from 'vue'
import CommonConfirm from './popup/CommonConfirm';
import CommonForm from './Form/BaseForm/CommonForm';
import CommonDatePicker from './Form/BaseForm/CommonDatePicker';
import CommonInputText from './Form/BaseForm/CommonInputText';
import CommonDropdownMenuInput from './Form/BaseForm/CommonDropdownMenuInput';

Vue.component('CommonConfirm', CommonConfirm);
Vue.component('CommonForm', CommonForm);
Vue.component('CommonDatePicker', CommonDatePicker);
Vue.component('CommonInputText', CommonInputText);
Vue.component('CommonDropdownMenuInput', CommonDropdownMenuInput);

export default {};