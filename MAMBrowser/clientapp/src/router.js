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
const date = new Date();

function get_date_str(date) {
  var sYear = date.getFullYear();
  var sMonth = date.getMonth() + 1;
  var sDate = date.getDate();

  sMonth = sMonth > 9 ? sMonth : "0" + sMonth;
  sDate = sDate > 9 ? sDate : "0" + sDate;

  return sYear + "" + sMonth + "" + sDate;
}

var toDay = get_date_str(date);

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
        // 제작 - (구)프로소재
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
        path: "dl30",
        component: () => import("./views/app/dl30/Index")
      },
      /**
      * CueSheet
      */
      {
        // (구) DAP 리스트
        name: 'cuesheet-old-list',
        path: "cuesheet/old/list",
        component: () => import("./views/app/cuesheet/cuesheetOldList"),
      },
      {
        // (구) DAP 상세페이지
        name: 'cuesheet-old-detail',
        path: "cuesheet/old/detail",
        component: () => import("./views/app/cuesheet/cuesheetOldDetail"),
        beforeEnter: (async (to, from, next) => {
          const userId = sessionStorage.getItem(USER_ID);
          var cueDataObj = { ...store.getters['cueList/cueInfo'] }
          if (Object.keys(cueDataObj).length === 0) {
            cueDataObj = JSON.parse(sessionStorage.getItem("USER_INFO"));
          }
          if (cueDataObj.cueid != -1) {
            //큐시트 수정
            var params = { productid: cueDataObj.productid, pgmcode: cueDataObj.pgmcode };
            if (Object.keys(cueDataObj).includes("detail")) {
              params.cueid = cueDataObj.detail[0].cueid
              params.brd_dt = cueDataObj.brddate
            } else {
              params.cueid = cueDataObj.cueid
              params.brd_dt = cueDataObj.day
            }
            await axios.get(`/api/daycuesheet/GetdayCue`, {
              params: params,
              paramsSerializer: (params) => {
                return qs.stringify(params);
              },
            })
              .then(async (res) => {
                var newInfo = { ...res.data.cueSheetDTO }
                newInfo.day = cueDataObj.day
                newInfo.liveflag = cueDataObj.liveflag
                newInfo.onairday = cueDataObj.onairday
                newInfo.seqnum = cueDataObj.seqnum
                newInfo.startdate = cueDataObj.startdate
                cueDataObj = newInfo
                await store.dispatch('cueList/setCueConData', res.data);
                store.dispatch('cueList/getProUserList', cueDataObj.productid);
              });
          } else {
            //큐시트 작성
            cueDataObj.detail = [{
              cueid: cueDataObj.cueid
            }]
            cueDataObj.brddate = cueDataObj.day;
            cueDataObj.brdtime = cueDataObj.r_ONAIRTIME;
            cueDataObj.cuetype = "D"
            cueDataObj.title = cueDataObj.eventname

            //작성된 기본큐시트가 있는지 확인
            var defCueId = [];
            await store.dispatch('cueList/getcuesheetListArrDef', {
              productids: cueDataObj.productid,
              row_per_page: 30,
              select_page: 1
            })

            var defCueList = store.getters['cueList/defCuesheetListArr'];
            defCueList.data.forEach((ele) => {
              var result = ele.detail.filter((v) => {
                return v.week == moment(cueDataObj.brdtime, "YYYY-MM-DD'T'HH:mm:ss").lang("en").format("ddd").toUpperCase()
              })
              if (result.length > 0) {
                defCueId = result
              }
            });
            if (defCueId.length > 0) {
              //기본큐시트 작성 내용 가져오기
              var params = { productid: cueDataObj.productid, week: defCueId[0].week, pgmcode: cueDataObj.pgmcode };
              if (Object.keys(cueDataObj).includes("detail")) {
                params.brd_dt = cueDataObj.brddate
              } else {
                params.brd_dt = cueDataObj.day
              }
              // var params = {
              //   productid: cueDataObj.productid,
              //   //cueid: defCueId[0].cueid,
              //   week: defCueId[0].week,
              // };
              await axios.get(`/api/defcuesheet/GetdefCue`, {
                params: params,
                paramsSerializer: (params) => {
                  return qs.stringify(params);
                },
              })
                .then((res) => {
                  var defcueData = res.data.cueSheetDTO
                  cueDataObj.r_ONAIRTIME = defcueData.detail[0].onairtime;
                  cueDataObj.directorname = defcueData.directorname;
                  cueDataObj.djname = defcueData.djname;
                  cueDataObj.footertitle = defcueData.footertitle;
                  cueDataObj.headertitle = defcueData.headertitle;
                  cueDataObj.membername = defcueData.membername;
                  cueDataObj.memo = defcueData.memo;
                  //나중에 여기에 태그도 추가되어야함
                  store.dispatch('cueList/setCueConData', res.data);
                });

            } else {
              store.dispatch('cueList/setclearCon');
              store.dispatch('cueList/setSponsorList', { pgmcode: cueDataObj.pgmcode, brd_dt: cueDataObj.brddate });
            }
            await store.dispatch('cueList/getProUserList', cueDataObj.productid);
          }
          store.dispatch('cueList/setclearFav');
          if (
            !cueDataObj.directorname ||
            cueDataObj.directorname == ""
          ) {
            cueDataObj.directorname = store.getters['cueList/proUserList'].length < 20 ? store.getters['cueList/proUserList'] : store.getters['cueList/proUserList'].substr(0, 20);
          }
          if (
            !cueDataObj.headertitle ||
            cueDataObj.headertitle == ""
          ) {
            cueDataObj.headertitle = cueDataObj.title;
          }
          if (
            !cueDataObj.footertitle ||
            cueDataObj.footertitle == ""
          ) {
            cueDataObj.footertitle = "참여방법 : #8001번 단문 50원, 장문&포토문자 100원 / 미니 무료 / (03925)서울시 마포구 성암로 267"
          }
          cueDataObj.personid = userId;
          store.commit('cueList/SET_CUEINFO', cueDataObj)
          sessionStorage.setItem("USER_INFO", JSON.stringify(cueDataObj));
          next();
        })
      },
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
        beforeEnter: (async (to, from, next) => {
          const userId = sessionStorage.getItem(USER_ID);
          var cueDataObj = { ...store.getters['cueList/cueInfo'] }
          if (Object.keys(cueDataObj).length === 0) {
            cueDataObj = JSON.parse(sessionStorage.getItem("USER_INFO"));
          }
          var params = {};
          if (Object.keys(cueDataObj).includes("detail")) {
            params.cueid = cueDataObj.detail[0].cueid
          } else {
            params.cueid = cueDataObj.cueid
          }
          await axios.get(`/api/tempcuesheet/GettempCue`, {
            params: params,
            paramsSerializer: (params) => {
              return qs.stringify(params);
            },
          })
            .then((res) => {
              console.log(res)
              cueDataObj = res.data.cueSheetDTO
              store.dispatch('cueList/setCueConData', res.data);
            });
          store.dispatch('cueList/setclearFav');

          if (
            !cueDataObj.headertitle ||
            cueDataObj.headertitle == "" ||
            cueDataObj.headertitle == null || cueDataObj.headertitle == undefined
          ) {
            cueDataObj.headertitle = cueDataObj.title;

          }
          if (
            !cueDataObj.footertitle ||
            cueDataObj.footertitle == "" ||
            cueDataObj.footertitle == null
          ) {
            cueDataObj.footertitle = "참여방법 : #8001번 단문 50원, 장문&포토문자 100원 / 미니 무료 / (03925)서울시 마포구 성암로 267"
          }
          cueDataObj.personid = userId;
          store.commit('cueList/SET_CUEINFO', cueDataObj)
          sessionStorage.setItem("USER_INFO", JSON.stringify(cueDataObj));
          next();
        }
        )
      },
      {
        // 큐시트 조회 리스트
        name: 'cuesheet-previous-list',
        path: "cuesheet/previous/list",
        component: () => import("./views/app/cuesheet/cuesheetList"),
      },
      {
        // 큐시트 조회
        name: 'cuesheet-previous-detail',
        path: "cuesheet/previous/detail",
        component: () => import("./views/app/cuesheet/cuesheetDetail"),
        beforeEnter: (async (to, from, next) => {
          const userId = sessionStorage.getItem(USER_ID);
          var cueDataObj = { ...store.getters['cueList/cueInfo'] }
          if (Object.keys(cueDataObj).length === 0) {
            cueDataObj = JSON.parse(sessionStorage.getItem("USER_INFO"));
          }
          var params = {};
          if (Object.keys(cueDataObj).includes("detail")) {
            params.cueid = cueDataObj.detail[0].cueid
          } else {
            params.cueid = cueDataObj.cueid
          }
          await axios.get(`/api/ArchiveCueSheet/GetArchiveCue`, {
            params: params,
            paramsSerializer: (params) => {
              return qs.stringify(params);
            },
          })
            .then((res) => {
              console.log(res)
              cueDataObj = res.data.cueSheetDTO
              store.dispatch('cueList/setCueConData', res.data);
            });
          store.dispatch('cueList/setclearFav');
          cueDataObj.personid = userId;
          store.commit('cueList/SET_CUEINFO', cueDataObj)
          sessionStorage.setItem("USER_INFO", JSON.stringify(cueDataObj));
          next();
        }
        )
      },
      {
        // 즐겨찾기
        name: 'cuesheet-favorite',
        path: "cuesheet/favorite",
        component: () => import("./views/app/cuesheet/cuesheetFavorite"),
        beforeEnter: ((to, from, next) => {
          store.dispatch('cueList/setclearFav');
          next();
        }
        )
      },
      {
        name: "config",
        path: "config", // 설정
        component: () => import("./views/app/config/Index")
      },
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
