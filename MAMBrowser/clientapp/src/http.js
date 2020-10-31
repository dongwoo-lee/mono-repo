import Vue from 'vue';
import axios from 'axios'
import store from './store'
import router from './router'

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

let isRefreshing = false;

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
        window.$notify(
            "error",
            `세션이 만료되었습니다. 로그인 페이지로 이동합니다.[ERROR:${status}]`,
            '', {
                duration: 8000,
                permanent: false
            }
        )
            
        isRefreshing = true;
        store.dispatch('user/signOut');
        router.push({path: '/user/login' });
    }

    return Promise.reject(err);
})

export default axios
