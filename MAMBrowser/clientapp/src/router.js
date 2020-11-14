import Vue from "vue";
import VueRouter from "vue-router";
import Function from './utils/CommonFunctions';
import {AUTHORITY_ADMIN, ROUTE_NAMES} from '@/constants/config';

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    component: () => import("./views/home"),
    redirect: "/app/my/private",
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
        path: "my/waste-basket",
        component: () => import("./views/app/my/wasteBasket"),
      },
      /**
       * 제작
       */
      {
        // 제작 - 프로그램
        name: 'program',
        path: "making/program",
        component: () => import("./views/app/making/Program"),
      },
      {
        // 제작 - 부조  SPOT
        name: 'relief-spot',
        path: "making/relief-spot",
        component: () => import("./views/app/making/ReliefSpot"),
      },
      {
        // 제작 - 공유소재
        name: ROUTE_NAMES.SHARED,
        path: "making/shared-material",
        component: () => import("./views/app/making/SharedMaterial"),
      },
      {
        // 제작 - 취재물
        name: 'coverage',
        path: "making/coverage",
        component: () => import("./views/app/making/Coverage"),
      },
      {
        // 제작 - (구)프로소재
        name: 'pro-materials',
        path: "making/pro-materials",
        component: () => import("./views/app/making/ProMaterials"),
      },
      /**
       * 음원
       */
      {
        // 음원 - 음반기록실
        name: 'music-record-room',
        path: "soundtrack/music-record-room",
        component: () => import("./views/app/soundtrack/MusicRecordRoom"),
      },
      {
        // 음원 - 효과음
        name: 'sound-effect',
        path: "soundtrack/sound-effect",
        component: () => import("./views/app/soundtrack/SoundEffect"),
      },
      /**
       * 광고
       */
      {
        // 광고 - 주조SB
        name: 'casting-sb',
        path: "advertising/casting-sb",
        component: () => import("./views/app/advertising/CastingSb"),
      },
      {
        // 광고 - 부조SB
        name: 'relief-sb',
        path: "advertising/relief-sb",
        component: () => import("./views/app/advertising/ReliefSb"),
      },
      {
        // 광고 - 프로그램CM
        name: 'program-cm',
        path: "advertising/program-cm",
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
        name: 'casting-spot',
        path: "combinationmd/casting-spot",
        component: () => import("./views/app/combinationMd/CastingSpot"),
      },
      {
        // 편성 MD - Filler(PR)
        name: 'filler-pr',
        path: "combinationmd/filler-pr",
        component: () => import("./views/app/combinationMd/FillerPr"),
      },
      {
        // 편성 MD - Filler(소재)
        name: 'filler-material',
        path: "combinationmd/filler-material",
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
        name: 'filler-other',
        path: "combinationmd/filler-other",
        component: () => import("./views/app/combinationMd/FillerOther"),
      },
      /**
       * DL3.0
       */
      {
        // DL3.0
        name: 'dl3',
        path: "dl3",
        component: () => import("./views/app/dl3/Index"),
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
    if (to.path !== '/user/login') {
      next({ path: '/user/login', replace: true });
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
  
  // 시스템 관리자 접근 페이지
  const systemPageNames = ['config', 'log']; // 시스템 접근 페이지 route 이름
  if (systemPageNames.includes(to.name) && sessionStorage.getItem('authority') === AUTHORITY_ADMIN) {
    next();
    return;
  }

  // 이동할 페이지에 권한이 있을 경우
  if (matchRole && matchRole.length > 0) {
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