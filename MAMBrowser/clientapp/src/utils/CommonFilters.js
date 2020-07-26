import CommonFunctions from './CommonFunctions';
export default {
    yyyyMMdd: (value) => {
        if (value) {
            return CommonFunctions.formatDate(value, 'yyyy-MM-dd');
        }
        return value;
    },
    
}