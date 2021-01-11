import Vue from "vue";
import VueRouter from "vue-router";
import Function from './utils/CommonFunctions';
import store from './store/index';
import {AUTHORITY, AUTHORITY_ADMIN, ROUTE_NAMES, SYSTEM_MANAGEMENT_ACCESS_PAGE_CODE} from '@/constants/config';

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    redirect: "/app/my/private",
  },
  {
    path: "/user",
    component: () => import("./views/user"),
    redirect: "/user/Login",
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
    redirect: "/app/my/private",
    children: [
      /**
       * My 공간
       */
      {
        // My 공간
        name: ROUTE_NAMES.PRIVATE,
        path: "my/private",
        component: () => import("./views/app/my/private"),
      },
      {
        // 휴지통
        name: ROUTE_NAMES.WASTE_BASKET,
        path: "my/waste-basket",
        component: () => import("./views/app/my/wasteBasket"),
      },
      /**
       * 제작
       */
      {
        // 제작 - 프로그램
        name: 'program',
        path: "products/program",
        component: () => import("./views/app/products/Program"),
      },
      {
        // 제작 - 부조  SPOT
        name: 'scr-spot',
        path: "products/scr-spot",
        component: () => import("./views/app/products/ScrSpot"),
      },
      {
        // 제작 - 공유소재
        name: ROUTE_NAMES.SHARED,
        path: "products/public",
        component: () => import("./views/app/products/Public"),
      },
      {
        // 제작 - 취재물
        name: 'coverage',
        path: "products/coverage",
        component: () => import("./views/app/products/Coverage"),
      },
      {
        // 제작 - (구)프로소재
        name: 'pro-mt',
        path: "products/pro-mt",
        component: () => import("./views/app/products/ProMaterials"),
      },
      /**
       * 음원
       */
      {
        // 음원 - 음반기록실
        name: 'song',
        path: "music/song",
        component: () => import("./views/app/music/Song"),
      },
      {
        // 음원 - 효과음
        name: 'effect',
        path: "music/effect",
        component: () => import("./views/app/music/Effect"),
      },
      /**
       * 광고
       */
      {
        // 광고 - 주조SB
        name: 'mcr-sb',
        path: "advertising/mcr-sb",
        component: () => import("./views/app/advertising/McrSb"),
      },
      {
        // 광고 - 부조SB
        name: 'scr-sb',
        path: "advertising/scr-sb",
        component: () => import("./views/app/advertising/ScrSb"),
      },
      {
        // 광고 - 프로그램CM
        name: 'pgm-cm',
        path: "advertising/pgm-cm",
        component: () => import("./views/app/advertising/ProgramCm"),
      },
      {
        // 광고 - CM
        name: 'cm',
        path: "advertising/cm",
        component: () => import("./views/app/advertising/Cm"),
      },
      /**
       * 편성 MD
       */
      {
        // 편성 MD - 주조 SPOT
        name: 'mcr-spot',
        path: "combinationmd/mcr-spot",
        component: () => import("./views/app/combinationMd/McrSpot"),
      },
      {
        // 편성 MD - Filler(PR)
        name: 'filler-pr',
        path: "combinationmd/filler",
        component: () => import("./views/app/combinationMd/Filler"),
      },
      {
        // 편성 MD - Filler(소재)
        name: 'filler-mt',
        path: "combinationmd/filler-mt",
        component: () => import("./views/app/combinationMd/FillerMaterial"),
      },
      {
        // 편성 MD - Filler(시간)
        name: 'filler-time',
        path: "combinationmd/filler-time",
        component: () => import("./views/app/combinationMd/FillerTime"),
      },
      {
        // 편성 MD - Filler(기타)
        name: 'filler-etcr',
        path: "combinationmd/filler-etc",
        component: () => import("./views/app/combinationMd/FillerEtc"),
      },
      /**
       * DL3.0
       */
      {
        // DL3.0
        name: 'dl30',
        path: "dl30",
        component: () => import("./views/app/dl30/Index"),
      },
      {
        name: 'config',
        path: "config", // 설정
        component: () => import("./views/app/config/Index"),
      },
      {
        name: 'log',
        path: "log", // 사용자 로그보기
        component: () => import("./views/app/workHistory/Index"),
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
  if (!tokenString || !from) {
    if (to.path !== '/user/Login') {
      next({ path: '/user/Login', replace: true });
      return;
    } else {
      next();
      return;
    }
  }  
  
  // 권한체크
  const roles = JSON.parse(sessionStorage.getItem('role'));
  let matchRole = null;
  if (roles) {
    matchRole = roles.filter(role => {
      return role.to === to.path && role.visible === 'Y';
    })
  }

  // 이동할 페이지에 권한이 있을 경우 || 시스템 관리자 접근 페이지
  if ((matchRole && matchRole.length > 0) 
    || (SYSTEM_MANAGEMENT_ACCESS_PAGE_CODE.includes(to.name) && sessionStorage.getItem(AUTHORITY) === AUTHORITY_ADMIN)) 
    {
      store.dispatch('user/reissue', from);
      next();
      return;
  } else {
    // 이동할 페이지에 권한이 없을 경우
    alert("접근 권한이 없습니다.");
    if (from.name) {
      next(from);
      return;
    }
  
    // 전페이지 정보가 없을 경우,
    next({path: Function.getFirstAccessiblePage })
  }
});

export default router;