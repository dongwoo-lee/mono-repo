import Vue from "vue";
import VueCookies from 'vue-cookies';
import VueRouter from "vue-router";
import AuthRequired from "./utils/AuthRequired";
import store from './store'

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    component: () => import(/* webpackChunkName: "home" */ "./views/home"),
    redirect: "/app/private",
    // beforeEnter: function(to, from, next) {
    //   if (localStorage.getItem('access_token') !== null) {
    //     next({ path: '/app/private' });
    //   } else {
    //     next({ path: '/user' });
    //   }
    // }
  },
  {
    path: "/app",
    component: () => import(/* webpackChunkName: "app" */ "./views/app"),
    redirect: "/app/private",
    // beforeEnter: AuthRequired,
    children: [
      {
        path: "private",
        component: () => import(/* webpackChunkName: "private" */ "./views/app/Private"),
      },
      {
        path: "waste-basket",
        component: () => import(/* webpackChunkName: "second-menu" */ "./views/app/Waste-basket"),
      },


      {
        path: "single",
        component: () =>
          import(/* webpackChunkName: "single" */ "./views/app/single")
      }
    ]
  },
  {
    path: "/error",
    component: () => import(/* webpackChunkName: "error" */ "./views/Error")
  },
  {
    path: "/user",
    component: () => import(/* webpackChunkName: "user" */ "./views/user"),
    redirect: "/user/login",
    children: [
      {
        path: "login",
        component: () => import(/* webpackChunkName: "user" */ "./views/user/Login"),
        meta: { requiresAuth: true }
        // beforeEnter: (to, from, next) => {
        //   if (localStorage.getItem('access_token') != null) {
        //     next({ path: "/" });
        //   } else {
        //     next();
        //   }
        // }
      },
      // {
      //   path: "register",
      //   component: () =>
      //     import(/* webpackChunkName: "user" */ "./views/user/Register")
      // },
      // {
      //   path: "forgot-password",
      //   component: () =>
      //     import(/* webpackChunkName: "user" */ "./views/user/ForgotPassword")
      // },
      // {
      //   path: "reset-password",
      //   component: () =>
      //     import(/* webpackChunkName: "user" */ "./views/user/ResetPassword")
      // },
    ]
  },
  {
    path: "*",
    component: () => import(/* webpackChunkName: "error" */ "./views/Error")
  }
];

const router = new VueRouter({
  linkActiveClass: "active",
  routes,
  mode: "history"
});

// 라우터 내비게이션 가드
router.beforeEach(async(to, from, next) => {
  // localStorage에 access_token값이 없을 경우 + refreshToken 값이 있을 경우 -> 토큰 재발급 함수 실행
  if (localStorage.getItem('access_token') && VueCookies.get('refreshToken') !== null) {
     await store.dispatch('user/refreshToken');
  }

  // localStorage에 access_token값이 있거나 ~ requiresAuth 가 참일 경우 -> 다음 페이지로 이동
  if (to.matched.some(record => record.meta.requiresAuth) || localStorage.getItem('access_token')){
    return next();
  }

  // 아무것도 아닐 경우 (access_token값도 없고 refreshToken도 없고) -> 로그인 페이지로 이동
  next('/user/login');
});


export default router;
