import { helpers } from 'vuelidate/lib/validators';
import { INPUT_MAX_LENGTH } from '@/constants/config';
import $fn from '@/utils/CommonFunctions';

const required = (value) => {
    if (value === undefined) return true;
    return !helpers.req(value);
}

const date = (value) => {
    if (!value || value === undefined) return true;
    return $fn.validDate(value);
}

const maxLength = (value) => {
    if (!value || value === undefined) return false;
    return value.length >= INPUT_MAX_LENGTH;
}

const commonValidate = {
    required,
    date,
    maxLength
}

export default commonValidate;
