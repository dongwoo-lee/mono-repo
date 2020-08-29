/**
 * 날짜 포메터
 * @param {*} d 
 * @param {*} p 
 */
const formatDate = (d, p = 'yyyyMMdd') => {
    if (!d) { return d; }

    let parseDate = d;
    if (d.length === 8) {
        const sY = d.substring(0,4);
        const sM = d.substring(4, 6);
        const sD = d.substring(6, 8);
        parseDate = `${sY}-${sM}-${sD}`;
    }

    const dateTime = new Date(parseDate);
    if (dateTime.toString() === 'Invalid Date') { return d; }
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
 * 시작 날짜가 종료 날짜보다 큰지 체크
 * emptyAlow : 빈값 허용(검색폼에서는 빈값 허용.)
 * @param {*} sDate
 * @param {*} eDate
 */
const checkGreaterStartDate = function(sDate, eDate, emptyAllow = true) {
    if (!sDate || !eDate) return !emptyAllow;
    const parseStartDate = Date.parse(this.formatDate(sDate, 'yyyy-MM-dd'));
    const parseEndDate = Date.parse(this.formatDate(eDate, 'yyyy-MM-dd'));
    return parseStartDate > parseEndDate;
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
const notify = (type, options = { title, message, duration, permanent }) => {
    const optionInOptions = {
        duration: options.duration ? options.duration : 5000,
        permanent: options.permanent ? options.permanent : false,
    }
    if (type === 'server-error') {
        return window.$notify('error', 'Server Error', '서버 에러입니다.', optionInOptions);
    }
    if (type === 'undefined') {
        return window.$notify('error', 'Data undefined', '데이터가 없습니다.', optionInOptions);
    }
    if (type === 'inputError') {
        return window.$notify('error', '입력 폼 에러', '입력 폼 양식을 확인해주세요.', optionInOptions);
    }

    window.$notify(type, options.title, options.message, optionInOptions);
}

const changeSortValue = (items, key) => {
    if (!items[key]) return 'ASC'
    const outValue = items[key];
    if (outValue === 'ASC') {
        return 'DESC';
    }

    return 'ASC';
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
            const filenameRegex = /filename[^;=\n]=((['"]).*?\2|[^;\n]*)/;
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

const formatBytes = (bytes, decimals = 2) => {
    if (bytes === 0) return '0 Bytes';

    const k = 1024;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

    const i = Math.floor(Math.log(bytes) / Math.log(k));

    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
}

const commonFunctions = {
    formatDate,
    checkGreaterStartDate,
    splitFirst,
    notify,
    fileDownload,
    changeSortValue,
    formatBytes
}

export default commonFunctions;
