import { MINIMUM_DATE, MAXIMUM_DATE_NUM, MAXIMUM_SEARCH_DATE } from '@/constants/config';

/**
 * 날짜 포메터
 * @param {*} d 
 * @param {*} p 
 */
const formatDate = (d, p = 'yyyyMMdd', haipunToString = true) => {
    if (!d) { return d; }

    let date = null;
    if (d instanceof Date) {
        date = d;        
    } else {
        let convertDate = d;
        if (d.length === 8) {
            const sY = d.substring(0, 4);
            const sM = d.substring(4, 6);
            const sD = d.substring(6, 8);
            convertDate = `${sY}-${sM}-${sD}`;
        }
        date = new Date(convertDate);
        if (date.toString() === 'Invalid Date') { return d; }
    }
    
    const sepDate = {
        y: date.getFullYear(),
        M: ('0' + (date.getMonth() + 1)).slice(-2),
        d: ('0' + date.getDate()).slice(-2),
        h: ('0' + date.getHours()).slice(-2),
        m: ('0' + date.getMinutes()).slice(-2),
        s: ('0' + date.getSeconds()).slice(-2),
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
 * 스트링 날짜를 하이픈(대쉬) 넣기 
 * @param {*} d 
 */
const dateStringTohaipun = (d) => {
    if (!d) { return d; }

    let parseDate = d;
    if (d.length === 8) {
        const sY = d.substring(0, 4);
        const sM = d.substring(4, 6);
        const sD = d.substring(6, 8);
        
        return parseDate = `${sY}-${sM}-${sD}`;
    }
    return parseDate;
}

/**
 * 시작 날짜가 종료 날짜보다 큰지 체크
 * emptyAlow : 빈값 허용(검색폼에서는 빈값 허용.)
 * @param {*} sDate
 * @param {*} eDate
 */
const checkGreaterStartDate = function(sDate, eDate, emptyAllow = true) {
    if (!sDate || !eDate) return !emptyAllow;
    const parseStartDate = Date.parse(formatDate(sDate, 'yyyy-MM-dd'));
    const parseEndDate = Date.parse(formatDate(eDate, 'yyyy-MM-dd'));
    return parseStartDate > parseEndDate;
};

/**
 * 날짜가 유효한지 체크
 * @param {*} value 
 */
const validDate = (value) => {
    if (!value || value === undefined) return true;
    // 1900~2199년까지 가능
    const fullDateRegex = /^(19|20|21)\d{2}(-)?(0[1-9]|1[012])(-)?(0[1-9]|[12][0-9]|3[0-1])$/;
    return fullDateRegex.test(value);
}

const getMaxDate = () => {
    const now = new Date();
    const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    const maxDate = new Date(today);
    maxDate.setDate(maxDate.getDate() + MAXIMUM_DATE_NUM);
    return maxDate;
}

const checkMaximumSearchDate = (date, maximumType) => {
    const currentDate = Date.parse(formatDate(date, 'yyyy-MM-dd'));
    const maxSearchDate = new Date(currentDate);
    if (maximumType === 's') {
        maxSearchDate.setDate(maxSearchDate.getDate() + MAXIMUM_SEARCH_DATE);
        return currentDate >= maxSearchDate;
    } 
    
    if (maximumType === 'e') {
        maxSearchDate.setDate(maxSearchDate.getDate() - MAXIMUM_SEARCH_DATE);
        return currentDate <= maxSearchDate;
    }
    
    return true;
}

/**
 * 시작일과 종료일 사이 확인 및 최대 검색일 수 확인
 * @param {*} date 
 */
const checkBetweenDate = (date) => {
    if (!date) return false;
    const currentDate = Date.parse(formatDate(date, 'yyyy-MM-dd'));
    const parseStartDate = Date.parse(formatDate(MINIMUM_DATE, 'yyyy-MM-dd'));
    const parseEndDate = Date.parse(formatDate(getMaxDate(), 'yyyy-MM-dd'));
    return currentDate >= parseStartDate  && currentDate <= parseEndDate;
}

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
        duration: options.duration ? options.duration : 8000,
        permanent: options.permanent ? options.permanent : false,
    }
    if (type === 'server-error') {
        return window.$notify('error', 'Server Error', '서버 에러입니다.', optionInOptions);
    }
    if (type === 'undefined') {
        return window.$notify('error', 'Data undefined', '데이터가 없습니다.', optionInOptions);
    }
    if (type === 'inputError') {
        return window.$notify('error', '입력 폼 에러', '필수 입력 항목을 확인해주세요.', optionInOptions);
    }
    if (type === 'dateError') {
        return window.$notify('error', '날짜 입력 에러', '날짜 항목을 확인해주세요.', optionInOptions);
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

const formatMBBytes = (bytes, decimals = 1) => {    
    if (bytes === 0) return '0 MB';
    // 1 000 = 1kb
    // 1 000 000 = 1mb
    // 1 000 000 000 = 1gb
    const baseBytes = 1000000; // 기본 MB단위입니다.
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

    let i = 0;
    let k = Math.floor(bytes / baseBytes);

    while(k >= 1000) {
        i = i + 1;
        k = Math.floor(k / 1000);
    }

    return parseFloat((bytes / (baseBytes * Math.pow(1000, i))).toFixed(dm)) + ' ' + sizes[i];
}

const formatBytes = (size, decimals = 2) => {
    if (size === 0) return '0 B';
    let bytes = size;
    const isMinusSign = size < 0;
    if (isMinusSign) {
        bytes = size * -1;
    }

    const k = 1000;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['B', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

    const i = Math.floor(Math.log(bytes) / Math.log(k));
    const formatData = parseFloat((bytes / Math.pow(k, i)).toFixed(dm));
    if (isMinusSign) {
        return formatData * -1 + sizes[i];
    }
    return formatData + sizes[i];
}

const getFirstAccessiblePage = () => {
    const roles = JSON.parse(sessionStorage.getItem('role'));
    const firstAccessiblePageIndex = roles.findIndex(role => role.visible === 'Y' && role.to);
    if (firstAccessiblePageIndex > -1) {
        return roles[firstAccessiblePageIndex].to;
    }
    return '/user/login';
}

const isBrowserCheck = () => { 
	const agt = navigator.userAgent.toLowerCase(); 
	if (agt.indexOf("chrome") != -1) return 'Chrome'; 
	if (agt.indexOf("opera") != -1) return 'Opera'; 
	if (agt.indexOf("staroffice") != -1) return 'Star Office'; 
	if (agt.indexOf("webtv") != -1) return 'WebTV'; 
	if (agt.indexOf("beonex") != -1) return 'Beonex'; 
	if (agt.indexOf("chimera") != -1) return 'Chimera'; 
	if (agt.indexOf("netpositive") != -1) return 'NetPositive'; 
	if (agt.indexOf("phoenix") != -1) return 'Phoenix'; 
	if (agt.indexOf("firefox") != -1) return 'Firefox'; 
	if (agt.indexOf("safari") != -1) return 'Safari'; 
	if (agt.indexOf("skipstone") != -1) return 'SkipStone'; 
	if (agt.indexOf("netscape") != -1) return 'Netscape'; 
	if (agt.indexOf("mozilla/5.0") != -1) return 'Mozilla'; 
	if (agt.indexOf("msie") != -1) { 
    	let rv = -1; 
		if (navigator.appName == 'Microsoft Internet Explorer') { 
			let ua = navigator.userAgent; var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})"); 
		if (re.exec(ua) != null) 
			rv = parseFloat(RegExp.$1); 
		} 
		return 'Internet Explorer '+rv; 
	} 
}

const commonFunctions = {
    formatDate,
    dateStringTohaipun,
    checkGreaterStartDate,
    validDate,
    splitFirst,
    notify,
    fileDownload,
    changeSortValue,
    formatMBBytes,
    formatBytes,
    getFirstAccessiblePage,
    getMaxDate,
    checkBetweenDate,
    checkMaximumSearchDate,
}

export default commonFunctions;
