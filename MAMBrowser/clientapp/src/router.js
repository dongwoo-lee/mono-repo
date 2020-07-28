import Vue from "vue";
import VueRouter from "vue-router";

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    component: () => import("./views/home"),
    redirect: "/app/private",
  },
  {
    path: "/user",
    component: () => import("./views/user"),
    redirect: "/user/login",
    children: [
      {
        path: "login",
        component: () => import("./views/user/Login"),
        meta: { requiresAuth: true }
      },
    ]
  },
  {
    path: "/app",
    component: () => import("./views/app"),
    redirect: "/app/private",
    children: [
      /**
       * My 공간
       */
      {
        // My 공간
        path: "private",
        component: () => import("./views/app/private/index"),
      },
      {
        // 휴지통
        path: "waste-basket",
        component: () => import("./views/app/wasteBasket/Index"),
      },
      {
        // 개발 컴포넌트
        path: "dev",
        component: () => import("./views/app/dev/Index"),
      },
      {
        // 개발 API 연결 컴포넌트
        path: "apidev",
        component: () => import("./views/app/dev-api/Index"),
      },
      /**
       * 제작
       */
      {
        // 제작 - 프로그램
        path: "program",
        component: () => import("./views/app/making/Program"),
      },
      {
        // 제작 - 부조  SPOT
        path: "relief-spot",
        component: () => import("./views/app/making/ReliefSpot"),
      },
      {
        // 제작 - 공유소재
        path: "shared-material",
        component: () => import("./views/app/making/SharedMaterial"),
      },
      {
        // 제작 - 취재물
        path: "coverage",
        component: () => import("./views/app/making/Coverage"),
      },
      {
        // 제작 - (구)프로소재
        path: "pro-materials",
        component: () => import("./views/app/making/ProMaterials"),
      },
      /**
       * 음원
       */
      {
        // 음원 - 음반기록실
        path: "music-record-room",
        component: () => import("./views/app/soundtrack/MusicRecordRoom"),
      },
      {
        // 음원 - 효과음
        path: "sound-effect",
        component: () => import("./views/app/soundtrack/SoundEffect"),
      },
      /**
       * 광고
       */
      {
        // 광고 - 주조SB
        path: "casting-sb",
        component: () => import("./views/app/advertising/CastingSb"),
      },
      {
        // 광고 - 부조SB
        path: "relief-sb",
        component: () => import("./views/app/advertising/ReliefSb"),
      },
      {
        // 광고 - CM
        path: "cm",
        component: () => import("./views/app/advertising/Cm"),
      },
      /**
       * 편성 MD
       */
      {
        // 편성 MD - 주조 SPOT
        path: "casting-spot",
        component: () => import("./views/app/combinationMd/CastingSpot"),
      },
      {
        // 편성 MD - Filler(PR)
        path: "filler-pr",
        component: () => import("./views/app/combinationMd/FillerPr"),
      },
      {
        // 편성 MD - Filler(소재)
        path: "filler-material",
        component: () => import("./views/app/combinationMd/FillerMaterial"),
      },
      {
        // 편성 MD - Filler(시간)
        path: "filler-time",
        component: () => import("./views/app/combinationMd/FillerTime"),
      },
      {
        // 편성 MD - Filler(기타)
        path: "filler-other",
        component: () => import("./views/app/combinationMd/FillerOther"),
      },
      /**
       * DL3.0
       */
      {
        // DL3.0
        path: "dl3",
        component: () => import("./views/app/dl3/Index"),
      },
    ]
  },
  {
    path: "/error",
    component: () => import("./views/Error")
  },
  {
    path: "*",
    component: () => import("./views/Error")
  },
];

const router = new VueRouter({
  linkActiveClass: "active",
  routes,
  mode: "history"
});

// 라우터 내비게이션 가드
router.beforeEach((to, from, next) => {
  // 토큰 유무 체크
  const tokenString = sessionStorage.getItem('access_token');
  if (!tokenString) {
    if (to.path !== '/user/login') {
      next({ path: '/user/login', replace: true });
    }
  }

  next();
});


export default router;