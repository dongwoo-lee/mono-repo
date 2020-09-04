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

    if (status === 401 && data.message === 'Invalid token') {
        isRefreshing = true;
        store.commit('user/signOut');
        router.push({path: '/user/login' });
    }

    if (status !== 200) {
        // window.$notify(
        //     "error",
        //     "응답값:" + status + '(' + statusText + ')',
        //     "url: " + config.url, {
        //         duration: 8000,
        //         permanent: false
        //     }
        // )
    }

    return Promise.reject(err);
})

export default axios
