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
    console.info('interceptors.request.error', error);
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

    console.info('interceptors.response.error', err.response);

    const{
        response: { config, status, data, statusText }
    } = err;

    if (status === 401) {
        // sessionStorage.removeItem('access_token');
        LoginPopupRefElement.loginPopup.show();
        window.$notify(
            "error",
            `세션이 만료되었습니다. 재로그인이 필요합니다.[ERROR:${status}]`,
            '', {
                duration: 8000,
                permanent: false
            }
        )
        // store.dispatch('user/signOut');
        // router.push({path: '/user/login' });
    }

    return Promise.reject(err);
})

export default $http
