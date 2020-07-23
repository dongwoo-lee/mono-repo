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
    const { status, data } = res;
    if (status === 200 && data.resultObject === null) {
        window.$notify(
            "error",
            "응답값:" + data.resultObject,
            "값이 넘어오지 않았습니다.", {
                duration: 4000,
                permanent: false
            }
        )
    }
    return res;
}, async err => {
    const{
        response: { status, data }
    } = err;

    if (status === 401 && data.message === 'Invalid token') {
        isRefreshing = true;
        store.commit('user/signOut');
        router.push({path: '/user/login' });
    }

    if (status !== 200) {
        window.$notify(
            "error",
            "응답값:" + status,
            "Error", {
                duration: 4000,
                permanent: false
            }
        )
    }

    return Promise.reject(err);
})

export default axios
