import Vue from "vue";
import VueRouter from "vue-router";
import AuthRequired from "./utils/AuthRequired";

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
        path: "dev",
        component: () => import(/* webpackChunkName: "private" */ "./views/app/Dev"),
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
router.beforeEach((to, from, next) => {
  // 토큰 유무 체크
  const tokenString = localStorage.getItem('access_token');
  if (!tokenString) {
    if (to.path !== '/user/login') {
      next({ path: '/user/login', replace: true });
    }
  }

  next();
});


export default router;
