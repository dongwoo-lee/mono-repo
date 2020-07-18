/**
 * 날짜 포메터
 * @param {*} d 
 * @param {*} p 
 */
const formatDate = (d, p = 'yyyyMMdd') => {
    const dateTime = new Date(d);
    const sepDate = {
        y: dateTime.getFullYear(),
        M: ('0' + (dateTime.getMonth() + 1)).slice(-2),
        d: ('0' + dateTime.getDate()).slice(-2),
        h: ('0' + dateTime.getHours()).slice(-2),
        m: ('0' + dateTime.getMinutes()).slice(-2),
        s: ('0' + dateTime.getSeconds()).slice(-2),
    };

    const format = p.replace(/(y+|M+|d+|h+|m+|s+)/g, $v => {
        switch($v) {
            case 'yyyy': return sepDate.y;
            case 'MM': return sepDate.M;
            case 'dd': return sepDate.d;
            case 'hh': return sepDate.h;
            case 'mm': return sepDate.m;
            case 'ss': return sepDate.s;
            default: return $v;
        }
    })

    return format;
};

const commonFunctions = {
    formatDate,
}

export default commonFunctions;