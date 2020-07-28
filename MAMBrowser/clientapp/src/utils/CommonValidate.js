import { helpers } from 'vuelidate/lib/validators'
const required = (value) => !helpers.req(value);
const date = (value) => {
    if (!value) return false;
    const fullDateRegex = /^(19|20|21|22)\d{2}(-)?(0[1-9]|1[012])(-)?(0[1-9]|[12][0-9]|3[0-1])$/;
    return !fullDateRegex.test(value);
}

const commonValidate = {
    required,
    date
}


export default commonValidate;