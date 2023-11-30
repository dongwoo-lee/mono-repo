import Vue from "vue";
import VueRouter from "vue-router";
import Function from './utils/CommonFunctions';
import store from './store/index';
import { USER_ID, AUTHORITY, AUTHORITY_ADMIN, ROUTE_NAMES, SYSTEM_MANAGEMENT_ACCESS_PAGE_CODE } from '@/constants/config';
import "moment/locale/ko";
const moment = require("moment");
Vue.use(VueRouter);
import axios from "axios";
const qs = require("qs");

const routes = [
  {
    path: "/",
    redirect: "/app/my/private"
  },
  {
    path: "/user",
    component: () => import("./views/user"),
    redirect: "/user/login",
    children: [
      {
        path: "login",
        component: () => import("./views/user/login"),
        meta: { requiresAuth: true }
      }
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
        // My 선곡 순위 
        name: "music-ranking",
        path: "my/music-ranking",
        component: () => import("./views/app/my/musicRanking")
      },
      {
        // My 공간
        name: ROUTE_NAMES.PRIVATE,
        path: "my/private",
        component: () => import("./views/app/my/private")
      },
      {
        // 휴지통
        name: ROUTE_NAMES.WASTE_BASKET,
        path: "my/waste-basket",
        component: () => import("./views/app/my/wasteBasket")
      },
      /**
       * 제작
       */
      {
        // 제작 - 프로그램
        name: "program",
        path: "products/program",
        component: () => import("./views/app/products/Program")
      },
      {
        // 제작 - 부조  SPOT
        name: "scr-spot",
        path: "products/scr-spot",
        component: () => import("./views/app/products/ScrSpot")
      },
      {
        // 제작 - 공유소재
        name: ROUTE_NAMES.SHARED,
        path: "products/public",
        component: () => import("./views/app/products/Public")
      },
      {
        // 제작 - 취재물
        name: "coverage",
        path: "products/coverage",
        component: () => import("./views/app/products/Coverage")
      },
      {
        // 제작 - 프로소재
        name: "pro-mt",
        path: "products/pro-mt",
        component: () => import("./views/app/products/ProMaterials")
      },
      /**
       * 음원
       */
      {
        // 음원 - 음반기록실
        name: "song",
        path: "music/song",
        component: () => import("./views/app/music/Song")
      },
      {
        // 음원 - 효과음
        name: "effect",
        path: "music/effect",
        component: () => import("./views/app/music/Effect")
      },
      {
        // 음원 - SONG
        name: "songcache",
        path: "music/songcache",
        component: () => import("./views/app/music/SongCache")
      },
      {
        // 음원 - SONG
        name: "statistics",
        path: "music/statistics",
        component: () => import("./views/app/music/Statistics")
      },
      /**
       * 광고
       */
      {
        // 광고 - 주조SB
        name: "mcr-sb",
        path: "advertising/mcr-sb",
        component: () => import("./views/app/advertising/McrSb")
      },
      {
        // 광고 - 부조SB
        name: "scr-sb",
        path: "advertising/scr-sb",
        component: () => import("./views/app/advertising/ScrSb")
      },
      {
        // 광고 - 프로그램CM
        name: "pgm-cm",
        path: "advertising/pgm-cm",
        component: () => import("./views/app/advertising/ProgramCm")
      },
      {
        // 광고 - CM
        name: "cm",
        path: "advertising/cm",
        component: () => import("./views/app/advertising/Cm")
      },
      /**
       * 편성 MD
       */
      {
        // 편성 MD - 주조 SPOT
        name: "mcr-spot",
        path: "combinationmd/mcr-spot",
        component: () => import("./views/app/combinationMd/McrSpot")
      },
      {
        // 편성 MD - Filler(PR)
        name: "filler-pr",
        path: "combinationmd/filler",
        component: () => import("./views/app/combinationMd/Filler")
      },
      {
        // 편성 MD - Filler(소재)
        name: "filler-mt",
        path: "combinationmd/filler-mt",
        component: () => import("./views/app/combinationMd/FillerMaterial")
      },
      {
        // 편성 MD - Filler(시간)
        name: "filler-time",
        path: "combinationmd/filler-time",
        component: () => import("./views/app/combinationMd/FillerTime")
      },
      {
        // 편성 MD - Filler(기타)
        name: "filler-etcr",
        path: "combinationmd/filler-etc",
        component: () => import("./views/app/combinationMd/FillerEtc")
      },
      /**
       * DL3.0
       */
      {
        // DL3.0
        name: "dl30",
        path: "monitoring/dl30",
        component: () => import("./views/app/dl30/Index")
      },
      {
        // 송출리스트
        name: "broadcastList",
        path: "monitoring/broadcastList",
        component: () => import("./views/app/broadcastList/Index")
      },
      {
        // 스튜디오
        name: "studio",
        path: "monitoring/studio",
        component: () => import("./views/app/studio/Index"),
        props: true
      },
      {
        // 프로그램
        name: "programInfo",
        path: "monitoring/programInfo",
        component: () => import("./views/app/programInfo/Index"),
        props: true
      },
      /**
      * CueSheet
      */
      {
        // 일일 큐시트 리스트
        name: 'cuesheet-day-list',
        path: "cuesheet/day/list",
        component: () => import("./views/app/cuesheet/cuesheetDayList"),
      },
      {
        // 일일 큐시트
        name: 'cuesheet-day-detail',
        path: "cuesheet/day/detail",
        component: () => import("./views/app/cuesheet/cuesheetDayDetail"),
      },
      {
        // 기본 큐시트 리스트
        name: 'cuesheet-week-list',
        path: "cuesheet/week/list",
        component: () => import("./views/app/cuesheet/cuesheetWeekList"),
      },
      {
        // 기본 큐시트
        name: 'cuesheet-week-detail',
        path: "cuesheet/week/detail",
        component: () => import("./views/app/cuesheet/cuesheetWeekDetail"),
      },
      {
        // 템플릿 리스트
        name: 'cuesheet-template-list',
        path: "cuesheet/template/list",
        component: () => import("./views/app/cuesheet/cuesheetTemplateList"),
      },
      {
        // 템플릿
        name: 'cuesheet-template-detail',
        path: "cuesheet/template/detail",
        component: () => import("./views/app/cuesheet/cuesheetTemplateDetail"),
      },
      {
        // 큐시트 조회 리스트
        name: 'cuesheet-previous-list',
        path: "cuesheet/previous/list",
        component: () => import("./views/app/cuesheet/cuesheetList"),
        props: true
      },
      {
        // 큐시트 조회
        name: 'cuesheet-previous-detail',
        path: "cuesheet/previous/detail",
        component: () => import("./views/app/cuesheet/cuesheetDetail"),
      },
      {
        // 즐겨찾기
        name: 'cuesheet-favorite',
        path: "cuesheet/favorite",
        component: () => import("./views/app/cuesheet/cuesheetFavorite"),
      },
      {
        name: "config",
        path: "config/miros", // MIROS 설정
        component: () => import("./views/app/config/miros/Index")
      },
      {
        name: "config",
        path: "config/system", // 시스템 설정 
        component: () => import("./views/app/config/system/Index")
      },
      {
        name: "config",
        path: "config/remove", // 소재 삭제 관리
        component: () => import("./views/app/config/remove/Index")
      },
      {
        name: "config",
        path: "config/monitoring", // 관리자 모니터링
        component: () => import("./views/app/config/monitoring/Index")
      },
      // {
      //   name: "config",
      //   path: "config/monitoring-setting", // 관리자 모니터링 설정
      //   component: () => import("./views/app/config/monitoring-setting/Index")
      // },
      {
        name: "log",
        path: "log", // 사용자 로그보기
        component: () => import("./views/app/workHistory/Index")
      }
    ]
  },
  {
    path: "/error",
    component: () => import("./views/Error")
  },
  {
    path: "*",
    component: () => import("./views/Error")
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
  const tokenString = sessionStorage.getItem("access_token");
  if (!tokenString || !from) {
    if (to.path !== "/user/Login") {
      next({ path: "/user/Login", replace: true });
      return;
    } else {
      next();
      return;
    }
  }

  // 권한체크
  const roles = JSON.parse(sessionStorage.getItem("role"));
  let matchRole = null;
  if (roles) {
    matchRole = roles.filter(role => {
      return role.to === to.path && role.visible === "Y";
    });
  }

  // 이동할 페이지에 권한이 있을 경우 || 시스템 관리자 접근 페이지
  if ((matchRole && matchRole.length > 0)
    || (SYSTEM_MANAGEMENT_ACCESS_PAGE_CODE.includes(to.name) && sessionStorage.getItem(AUTHORITY) === AUTHORITY_ADMIN)) {
    store.dispatch('user/reissue', from);
    next();
    return;
  } else {
    // 이동할 페이지에 권한이 없을 경우
    next();
    //alert("접근 권한이 없습니다.");
    if (from.name) {
      next(from);
      return;
    }

    // 전페이지 정보가 없을 경우,
    next({ path: Function.getFirstAccessiblePage })
  }
});

router.onError(error => {
  var str = error.message.substring(0, 13);
  if (str == "Loading chunk") {
    //NOTE:토스트 팝업
    // Function.notify("primary", {
    //   message:
    //     "안정적인 서비스 사용을 위해 최신 버전으로 새로고침이 필요합니다."
    // });
    window.location.reload();
  }
});
export default router;
