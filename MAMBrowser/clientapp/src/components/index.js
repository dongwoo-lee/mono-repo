import Vue from 'vue'
import CommonConfirm from './popup/CommonConfirm';
import CommonForm from './Form/CommonForm';
import CommonDatePicker from './Form/CommonDatePicker';
import CommonInputText from './Form/CommonInputText';
import CommonDropdownMenuInput from './Form/CommonDropdownMenuInput';
import CommonDataTable from './DataTable/CommonDataTable';
import CommonDataTableScrollPaging from './DataTable/CommonDataTableScrollPaging';
import CommonActions from './DataTable/CommonActions';
import CommonPreviewPopup from './popup/CommonPreviewPopup';

Vue.component('CommonConfirm', CommonConfirm);
Vue.component('CommonForm', CommonForm);
Vue.component('CommonDatePicker', CommonDatePicker);
Vue.component('CommonInputText', CommonInputText);
Vue.component('CommonDropdownMenuInput', CommonDropdownMenuInput);
Vue.component('CommonDataTable', CommonDataTable);
Vue.component('CommonDataTableScrollPaging', CommonDataTableScrollPaging);
Vue.component('CommonActions', CommonActions);
Vue.component('CommonPreviewPopup', CommonPreviewPopup);

export default {};