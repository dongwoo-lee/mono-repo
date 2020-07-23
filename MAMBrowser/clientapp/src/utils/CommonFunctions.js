/**
 * 날짜 포메터
 * @param {*} d 
 * @param {*} p 
 */
const formatDate = (d, p = 'yyyyMMdd') => {
    let parseDate = d;
    if (d.length === 8) {
        const sY = d.substring(0,4);
        const sM = d.substring(4, 6);
        const sD = d.substring(6, 8);
        parseDate = `${sY}-${sM}-${sD}`;
    }

    const dateTime = new Date(parseDate);
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

/**
 * 값 분리하여 첫번째 값 가져오기
 * @param {*} d 
 * @param {*} g 
 */
const splitFirst = (d, g = '.') => {
    const split = d.split(g);
    if (split.length > 0) {
        return split[0];
    }

    return d;
}

/**
 * 알림: Toast
 * @param {*} type 
 * @param {*} options 
 */
const notify = (type, options = { title, message, options: { duration: 4000, permanent: false } }) => {
    if (type === 'server-error') {
        return window.$notify('error', 'Server Error', '서버 에러입니다.', options.options);
    }
    if (type === 'undefined') {
        return window.$notify('error', 'Data undefined', '데이터가 없습니다.', options.options);
    }

    window.$notify(type, options.title, options.message, options.options);
}

/**
 * 파일 다운로드
 */
const fileDownload = (res, fileNm = '') => {
    let fileName = fileNm;
    if (res.data === undefined) {
        this.notify('undefined');
        return;
    }

    const blob = new Blob([res.data], {type: res.headers['content-type'] });
    if (!fileName) {
        const disposition = res.headers['content-disposition'];
        if (disposition && disposition.indexOf('attachment') != -1) {
            const filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
            const matches = filenameRegex.exec(disposition);
            if (matches != null && matches[1]) {
                fileName = decodeURI(matches[1].replace(/['"]/g, ''));
            }
        }
    }
    // IE11
    if (window.navigator.msSaveOrOpenBlob) {
        window.navigator.msSaveOrOpenBlob(blob, fileName);
    } else {
        // not IE
        const link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.target = '_self';
        link.download = fileName;
        link.click();
    }
}

const commonFunctions = {
    formatDate,
    splitFirst,
    notify,
    fileDownload,
}

export default commonFunctions;
