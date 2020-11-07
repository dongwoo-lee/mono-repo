import axios from 'axios'
import store from './store'
import router from './router'
import LoginPopupRefElement from './lib/loginPopup/LoginPopupRefElement';

const $http = axios.create({
    baseURL: process.env.baseURL,
    // 인증헤더 추가해야 함
    withCredentials: false,
    headers: {
        'Access-Control-Allow-Origin': '*',
        'Content-Type': 'application/json',
    },
    timeout: 10000,
});

axios.interceptors.response.use(res =>{
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

    const{
        response: { config, status, data, statusText }
    } = err;

    if (status === 401) {
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

export default axios
