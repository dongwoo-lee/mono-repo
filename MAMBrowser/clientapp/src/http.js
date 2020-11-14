import axios from 'axios'
import LoginPopupRefElement from './lib/loginPopup/LoginPopupRefElement';

const $http = axios.create({
    baseURL: process.env.baseURL,
    // 인증헤더 추가해야 함
    withCredentials: false,
    timeout: 10000,
});

$http.interceptors.request.use(
   async config => {
    config.headers = { 
        'Content-Type': 'application/json',
        'X-Csrf-Token': sessionStorage.getItem('access_token'),
    }
    return config;
  },
  error => {
    console.debug('interceptors.request.error', error);
    Promise.reject(error);
});

$http.interceptors.response.use(res =>{
    const { config, status, data } = res;
    if (status === 200 && data.resultObject === null && data.errorMsg) {
        if (config && config['Content-Type'] === 'multipart/form-data') { return res; }
        window.$notify(
            "error",
            '',
            data.errorMsg, {
                duration: 8000,
                permanent: false
            }
        )
    }
    return res;
}, async err => {
    if (!err.response) {
        return Promise.reject(err);
    }

    console.debug('interceptors.response.error', err.response);

    const{
        response: { config, status, data, statusText }
    } = err;

    if (status === 401) {
        LoginPopupRefElement.loginPopup.show();
        errorNotify(status, '세션이 만료되었습니다. 재로그인이 필요합니다.');
    }

    if (status === 500) {
        errorNotify(status, data);
        return err.response;
    }

    return err.response;
})

const errorNotify=(status, msg)=> {
    window.$notify(
        "error",
        `${msg}[ERROR:${status}]`,
        '', {
            duration: 8000,
            permanent: false
        }
    )
}

export default $http
