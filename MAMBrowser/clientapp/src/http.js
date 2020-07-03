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
    return res;
}, async err => {
    const{
        config,
        response: { status, data }
    } = err;

    if (status === 401 && data.message === 'Invalid token') {
        isRefreshing = true;
        store.commit('user/signOut');
        router.push({path: '/user/login' });
    }

    return Promise.reject(err);
})

export default axios