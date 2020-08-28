import Vue from 'vue'
import CommonConfirm from './popup/CommonConfirm';
import CommonForm from './Form/BaseForm/CommonForm';
import CommonDatePicker from './Form/BaseForm/CommonDatePicker';
import CommonInputText from './Form/BaseForm/CommonInputText';
import CommonDropdownMenuInput from './Form/BaseForm/CommonDropdownMenuInput';
import CommonDataTable from './DataTable/CommonDataTable';
import CommonDataTableScrollPaging from './DataTable/CommonDataTableScrollPaging';

Vue.component('CommonConfirm', CommonConfirm);
Vue.component('CommonForm', CommonForm);
Vue.component('CommonDatePicker', CommonDatePicker);
Vue.component('CommonInputText', CommonInputText);
Vue.component('CommonDropdownMenuInput', CommonDropdownMenuInput);
Vue.component('CommonDataTable', CommonDataTable);
Vue.component('CommonDataTableScrollPaging', CommonDataTableScrollPaging);

export default {};