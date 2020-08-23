import Vue from 'vue'
import CommonConfirm from './popup/CommonConfirm';
import CommonForm from './Form/BaseForm/CommonForm';
import CommonDatePicker from './Form/BaseForm/CommonDatePicker';
import CommonInputText from './Form/BaseForm/CommonInputText';

Vue.component('CommonConfirm', CommonConfirm);
Vue.component('CommonForm', CommonForm);
Vue.component('CommonDatePicker', CommonDatePicker);
Vue.component('CommonInputText', CommonInputText);

export default {};