import { helpers } from 'vuelidate/lib/validators';

const required = (value) => {
    if (value === undefined) return true;
    return !helpers.req(value);
}

const date = (value) => {
    if (!value || value === undefined) return true;
    // 2599년까지 가능
    const fullDateRegex = /^(19|20|21|22|23|24|25)\d{2}(-)?(0[1-9]|1[012])(-)?(0[1-9]|[12][0-9]|3[0-1])$/;
    return fullDateRegex.test(value);
}

const commonValidate = {
    required,
    date
}

export default commonValidate;
