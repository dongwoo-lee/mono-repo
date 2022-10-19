import $http from "../../http";
const qs = require("qs");
import "moment/locale/ko";
const moment = require("moment");
const _ = require('lodash');

export default {
    namespaced: true,
    state: {
        cuesheetListArr: [], //일일큐시트 리스트 목록
        defCuesheetListArr: [], //기본큐시트 리스트 목록
        tempCuesheetListArr: [], //템플릿 리스트 목록
        archiveCuesheetListArr: [], //이전 큐시트 리스트 목록
        proUserList: "", //프로그램 - 전체 유저
        cueInfo: {}, //작성 정보 (CUE)
        searchListData: [], //소재
        printArr: [], //출력용
        abCartArr: [], //ab 카트
        cChannelData: [], //c 카트
        cueFavorites: [], //즐겨찾기
        attachments: [], //첨부파일
        tags: [],
        printTem: [{
            code: "",
            contents: "---------- 1부  ----------",
            usedtime: 0,
            etc: "",
            rownum: 1,
            starttime: 0,
        },
        {
            code: "CSGP01",
            contents: "<오프닝>",
            usedtime: 0,
            etc: "",
            rownum: 2,
            starttime: 0,
        },
        {
            code: "CSGP07",
            contents: "음원  Play",
            usedtime: 0,
            etc: "",
            rownum: 3,
            starttime: 0,
        },
        {
            code: "CSGP02",
            contents: "1부 CM",
            usedtime: 0,
            etc: "",
            rownum: 4,
            starttime: 0,
        },
        {
            code: "CSGP09",
            contents: "코너",
            usedtime: 0,
            etc: "",
            rownum: 5,
            starttime: 0,
        },
        {
            code: "CSGP07",
            contents: "음원  Play",
            usedtime: 0,
            etc: "",
            rownum: 6,
            starttime: 0,
        },
        {
            code: "CSGP03",
            contents: "SB + 상품소개",
            usedtime: 0,
            etc: "",
            rownum: 7,
            starttime: 0,
        },
        {
            code: "CSGP09",
            contents: "코너",
            usedtime: 0,
            etc: "",
            rownum: 8,
            starttime: 0,
        },
        {
            code: "CSGP07",
            contents: "음원  Play",
            usedtime: 0,
            etc: "",
            rownum: 9,
            starttime: 0,
        },
        {
            code: "CSGP02",
            contents: "2부 CM",
            usedtime: 0,
            etc: "",
            rownum: 10,
            starttime: 0,
        },
        {
            code: "",
            contents: "예비",
            usedtime: 0,
            etc: "",
            rownum: 11,
            starttime: 0,
        },
        {
            code: "",
            contents: "---------- 3부  ----------",
            usedtime: 0,
            etc: "",
            rownum: 12,
            starttime: 0,
        },
        {
            code: "CSGP06",
            contents: "로고송",
            usedtime: 0,
            etc: "",
            rownum: 13,
            starttime: 0,
        },
        {
            code: "CSGP05",
            contents: "코너",
            usedtime: 0,
            etc: "",
            rownum: 14,
            starttime: 0,
        },
        {
            code: "CSGP08",
            contents: "",
            usedtime: 0,
            etc: "",
            rownum: 15,
            starttime: 0,
        },
        {
            code: "CSGP02",
            contents: "3부 CM",
            usedtime: 0,
            etc: "",
            rownum: 16,
            starttime: 0,
        },
        {
            code: "CSGP09",
            contents: "코너",
            usedtime: 0,
            etc: "",
            rownum: 17,
            starttime: 0,
        },
        {
            code: "CSGP07",
            contents: "음원  Play",
            usedtime: 0,
            etc: "",
            rownum: 18,
            starttime: 0,
        },
        {
            code: "CSGP07",
            contents: "음원  Play",
            usedtime: 0,
            etc: "",
            rownum: 19,
            starttime: 0,
        },
        {
            code: "CSGP03",
            contents: "SB + 캠페인",
            usedtime: 0,
            etc: "",
            rownum: 20,
            starttime: 0,
        },
        {
            code: "",
            contents: "---------- 4부  ----------",
            usedtime: 0,
            etc: "",
            rownum: 21,
            starttime: 0,
        },
        {
            code: "CSGP09",
            contents: "코너",
            usedtime: 0,
            etc: "",
            rownum: 22,
            starttime: 0,
        },
        {
            code: "CSGP07",
            contents: "음원  Play",
            usedtime: 0,
            etc: "",
            rownum: 23,
            starttime: 0,
        },
        {
            code: "CSGP02",
            contents: "4부 CM",
            usedtime: 0,
            etc: "",
            rownum: 24,
            starttime: 0,
        },
        ]
    },
    getters: {
        cuesheetListArr: state => state.cuesheetListArr,
        defCuesheetListArr: state => state.defCuesheetListArr,
        tempCuesheetListArr: state => state.tempCuesheetListArr,
        archiveCuesheetListArr: state => state.archiveCuesheetListArr,
        proUserList: state => state.proUserList,
        cueInfo: state => state.cueInfo,
        searchListData: state => state.searchListData,
        printArr: state => state.printArr,
        abCartArr: state => state.abCartArr,
        cChannelData: state => state.cChannelData,
        cueFavorites: state => state.cueFavorites,
        attachments: state => state.attachments,
        printTem: state => state.printTem,
        tags: state => state.tags,
    },
    mutations: {
        SET_CUESHEETLISTARR(state, payload) {
            state.cuesheetListArr = payload;
        },
        SET_DEFCUESHEETLISTARR(state, payload) {
            state.defCuesheetListArr = payload;
        },
        SET_TEMPCUESHEETLISTARR(state, payload) {
            state.tempCuesheetListArr = payload;
        },
        SET_ARCHIVECUESHEETLISTARR(state, payload) {
            state.archiveCuesheetListArr = payload
        },
        SET_PROUSERLIST(state, payload) {
            state.proUserList = payload;
        },
        SET_CUEINFO(state, payload) {
            state.cueInfo = payload;
        },
        SET_SEARCHLISTDATA(state, payload) {
            state.searchListData = payload;
        },
        SET_PRINTARR(state, payload) {
            state.printArr = payload;
        },
        SET_ABCARTARR(state, payload) {
            state.abCartArr = payload;
        },
        SET_CCHANNELDATA(state, payload) {
            state.cChannelData = payload;
        },
        SET_ATTACHMENTS(state, payload) {
            state.attachments = payload;
        },
        SET_TAGS(state, payload) {
            state.tags = payload.map(item => ({ ['text']: item }))
        },
        SET_CUEFAVORITES(state, payload) {
            state.cueFavorites = payload;
        },
    },
    actions: {
        GetPgmListByBrdDate({ }, payload) {
            return $http.get(`/api/CueUserInfo/GetPgmListByBrdDate`, { params: payload }).then((res) => res.data.resultObject)
        },
        GetSchPgmList({ }, payload) {
            return $http.get(`/api/CueUserInfo/GetSCHPgmList`, { params: payload }).then((res) => res.data.resultObject)
        },
        SetMediaOption({ }, payload) {
            const pgmList = payload
            const optionList = [{ value: "", text: "전체" }];
            const medias = [{ value: "A", text: "AM" }, { value: "F", text: "FM" }, { value: "D", text: "DMB" }]
            medias.forEach((media) => {
                if (pgmList?.some((item) => item.media === media.value)) optionList.push(media)
            })
            return optionList;
        },
        SetProgramCodeOption({ }, payload) {
            const pgmList = payload
            const optionList = [];
            pgmList?.forEach((pgm) => {
                optionList.push({ value: pgm.pgmcode, text: pgm.pgmname })
            })
            return optionList
        },
        SetProgramProductIdOption({ }, payload) {
            const pgmList = payload
            const optionList = [];
            pgmList?.forEach((pgm) => {
                pgm.pgmItem.forEach((item) => {
                    optionList.push({ value: item.productid, text: item.eventname })
                })
            })
            return optionList
        },
        SetProductIds({ }, payload) {
            const pgmList = payload
            const productIds = []
            pgmList?.forEach((pgm) => {
                pgm.pgmItem.forEach((item) => productIds.push(item.productid))
            })
            return productIds
        },
        GetDateString({ }, payload) {
            const date = payload
            let sYear = date.getFullYear();
            let sMonth = date.getMonth() + 1;
            let sDate = date.getDate();
            sMonth = sMonth > 9 ? sMonth : "0" + sMonth;
            sDate = sDate > 9 ? sDate : "0" + sDate;
            return sYear + "" + sMonth + "" + sDate;
        },
        //프로그램 - 전체 유저 가져오기
        async getProUserList({ commit }, payload) {
            await $http.get(`/api/CueUserInfo/GetDirectorList?productid=` + payload)
                .then((res) => {
                    if (res.data.resultObject) {
                        var setData = new Set(res.data.resultObject.split(","));
                        var result = ""
                        setData.forEach((ele) => {
                            result = result.concat(ele + ",")
                        })
                        commit('SET_PROUSERLIST', result.slice(0, -1))
                    }
                })
        },
        // 일일 큐시트 목록 전체 가져오기
        getcuesheetListArr({ commit }, payload) {
            if (typeof payload.products == 'string') {
                payload.products = [payload.products]
            }
            return $http
                .post(`/api/daycuesheet/Getdaycuelist`, payload)
                .then(res => {
                    if (res.data.resultObject) {
                        res.data.resultObject.data.sort((a, b) => {
                            return new Date(a.r_ONAIRTIME) - new Date(b.r_ONAIRTIME)
                        })
                        commit('SET_CUESHEETLISTARR', res.data.resultObject);
                        return res;
                    }
                });
        },
        // 기본 큐시트 목록 전체 가져오기
        getcuesheetListArrDef({ commit, dispatch }, payload) {
            if (typeof payload.productids == 'string') {
                payload.productids = [payload.productids]
            }
            return $http.post(`/api/DefCueSheet/GetDefList`, payload)
                .then(async (res) => {
                    var productWeekList = await dispatch('disableList', res.data.resultObject.data);
                    var seqnum = 0;
                    res.data.resultObject.data.forEach((ele) => {
                        var activeWeekList = [];
                        var cueids = [];
                        var weeks = [];
                        ele.productWeekList = productWeekList.filter((week) => {
                            return week.productid == ele.productid;
                        });
                        ele.detail.forEach((activeWeek) => {
                            activeWeekList.push(activeWeek.week);
                            cueids.push(activeWeek.cueid);
                            weeks.push(activeWeek.week)
                        });
                        ele.activeWeekList = activeWeekList;
                        ele.weeks = weeks;
                        ele.cueid = cueids;
                        ele.seq = seqnum;
                        seqnum = seqnum + 1;
                    });
                    commit('SET_DEFCUESHEETLISTARR', res.data.resultObject);
                    return res;
                })
                .catch((err => {
                    console.log("getcuesheetListArrDef" + err);
                }));
        },
        // 템플릿 목록 전체 가져오기
        getcuesheetListArrTemp({ commit }, payload) {
            return $http.get(`/api/TempCueSheet/GetTempList?personid=${payload.personid}&title=${payload.titie}&row_per_page=${payload.row_per_page}&select_page=${payload.select_page}`)
                .then((res) => {
                    var seqnum = 0;
                    res.data.resultObject.data.forEach((ele) => {
                        ele.seq = seqnum;
                        seqnum = seqnum + 1;
                    });
                    commit('SET_TEMPCUESHEETLISTARR', res.data.resultObject);
                    return res;
                })
                .catch((err => {
                    console.log("getcuesheetListArrTemp" + err);
                }));
        },
        //이전 큐시트 목록 가져오기
        getarchiveCuesheetListArr({ commit }, payload) {
            return $http.post(`/api/ArchiveCueSheet/GetArchiveCueList`, payload)
                .then((res) => {
                    commit('SET_ARCHIVECUESHEETLISTARR', res.data.resultObject);
                    return res;
                })
        },
        async getarchiveCuesheetCon({ }, payload) {
            return await $http.get(`/api/ArchiveCueSheet/GetArchiveCue`, {
                params: payload,
                paramsSerializer: (params) => {
                    return qs.stringify(params);
                },
            })
                .then((res) => {
                    return res.data.resultObject;
                });
        },
        //프로그램별 요일 확인
        disableList({ commit }, payload) {
            var productWeekList = [];
            payload.forEach((ele) => {
                var checker = true;
                var key = ele.productid;
                var value = [];
                ele.detail.forEach((week) => {
                    value.push(week.week);
                });
                if (productWeekList.length == 0) {
                    var result = {
                        productid: key,
                        weekList: value,
                    };
                    productWeekList.push(result);
                } else {
                    for (let i = 0; i < productWeekList.length; i++) {
                        if (productWeekList[i].productid == key) {
                            productWeekList[i].weekList =
                                productWeekList[i].weekList.concat(value);
                            checker = false;
                            return;
                        }
                    }
                    if (checker) {
                        var result = {
                            productid: key,
                            weekList: value,
                        };
                        productWeekList.push(result);
                    }
                }
            });
            return productWeekList;
        },
        async addByTemplate({ }, payload) {
            var temLength = 0
            await $http.get(`/api/TempCueSheet/GetTempList?personid=${payload.CueSheetDTO.personid}&row_per_page=100`)
                .then((res) => {
                    temLength = res.data.resultObject.data.length
                })
            if (temLength > 49) {
                window.$notify(
                    "error",
                    `템플릿을 더이상 추가할 수 없습니다.`,
                    '', {
                    duration: 10000,
                    permanent: false
                }
                )
            } else {
                await $http
                    .post(`/api/TempCueSheet/SaveByTemp`, payload)
                    .then((res) => {
                        window.$notify(
                            "info",
                            `템플릿 추가완료.`,
                            '', {
                            duration: 10000,
                            permanent: false
                        }
                        )
                    })
            }
        },
        //상세내용 -즐겨찾기
        async getCueDayFav({ state, commit, dispatch }, payload) {
            await $http.get(
                `/api/Favorite/GetFavorites?personid=${payload.personid}&pgmcode=${payload.pgmcode}&brd_dt=${payload.brd_dt}`)
                .then((res) => {
                    commit('SET_CUEFAVORITES', res.data.resultObject);
                })
                .catch((err => {
                    console.log("getCueDayFav" + err);
                }));
        },
        //일일큐시트 저장
        async saveDayCue({ commit, state, dispatch }, payload) {
            var pram = await dispatch('setCueConFav_save', true)
            pram.CueSheetDTO = state.cueInfo;
            await $http
                .post(`/api/DayCueSheet/SaveDayCue`, pram)
                .then(async (res) => {
                    dispatch('saveFavorites', { personid: state.cueInfo.personid, favConParam: pram.favConParam })
                    var newInfo = { ...state.cueInfo }
                    let rowData = JSON.parse(sessionStorage.getItem("USER_INFO"));
                    var params = {
                        productid: rowData.productid,
                        pgmcode: rowData.pgmcode,
                        brd_dt: rowData.day,
                    };
                    await $http.get(`/api/daycuesheet/GetDayCue`, {
                        params: params,
                        paramsSerializer: (params) => {
                            return qs.stringify(params);
                        },
                    }).then((cueRes) => {
                        if (cueRes.data.resultObject.attachments.length > 0) {
                            commit('SET_ATTACHMENTS', cueRes.data.resultObject.attachments)
                        }
                        newInfo.detail[0].cueid = res.data.resultObject
                        newInfo.edittime = cueRes.data.resultObject.cueSheetDTO.edittime
                        delete newInfo.cueid
                        commit('SET_CUEINFO', newInfo)
                    })
                    window.$notify(
                        "info",
                        `일일 큐시트 저장완료.`,
                        '', {
                        duration: 10000,
                        permanent: false
                    }
                    )
                })
                .catch((err => {
                    console.log("saveDayCue" + err.message);
                    window.$notify(
                        "error",
                        `일일 큐시트 저장실패.`,
                        '', {
                        duration: 10000,
                        permanent: false
                    }
                    )
                }));

            if (payload == true) {
                await $http
                    .post(`/api/DayCueSheet/SaveOldCue`, pram)
                    .then(async (res) => {
                        if (res.data == 1) {
                            window.$notify(
                                "info",
                                `(구) DAP에 큐시트 저장완료.`,
                                '', {
                                duration: 10000,
                                permanent: false
                            }
                            )
                        }
                        if (res.data == 0) {
                            window.$notify(
                                "error",
                                `해당 날짜의 큐시트는 작성이 불가합니다. (기존 큐시트 삭제 불가)`,
                                '', {
                                duration: 10000,
                                permanent: false
                            }
                            )
                        }
                        if (res.data == -1) {
                            window.$notify(
                                "error",
                                `My디스크, DL3 소재는 저장할 수 없습니다. 소재 삭제 후 다시 시도해주세요.`,
                                '', {
                                duration: 10000,
                                permanent: false
                            }
                            )
                        }
                    })
                    .catch((err) => {
                        console.log(err)
                        window.$notify(
                            "error",
                            `(구) DAP에 큐시트 저장실패.`,
                            '', {
                            duration: 10000,
                            permanent: false
                        }
                        )
                    })
            }
        },
        //구 DAP 저장
        async saveOldCue({ state, dispatch }) {
            var pram = await dispatch('setCueConFav_save', true)
            pram.CueSheetDTO = state.cueInfo;
            await $http
                .post(`/api/DayCueSheet/SaveOldCue`, pram)
                .then(async (res) => {
                    if (res.data == 1) {
                        window.$notify(
                            "info",
                            `(구) DAP에 큐시트 저장완료.`,
                            '', {
                            duration: 10000,
                            permanent: false
                        }
                        )
                    }
                    if (res.data == 0) {
                        window.$notify(
                            "error",
                            `해당 날짜의 큐시트는 작성이 불가합니다. (기존 큐시트 삭제 불가)`,
                            '', {
                            duration: 10000,
                            permanent: false
                        }
                        )
                    }
                    if (res.data == -1) {
                        window.$notify(
                            "error",
                            `My디스크, DL3 소재는 저장할 수 없습니다. 소재 삭제 후 다시 시도해주세요.`,
                            '', {
                            duration: 10000,
                            permanent: false
                        }
                        )
                    }
                })
                .catch((err) => {
                    console.log(err)
                    window.$notify(
                        "error",
                        `(구) DAP에 큐시트 저장실패.`,
                        '', {
                        duration: 10000,
                        permanent: false
                    }
                    )
                })
        },
        //기본큐시트 저장
        async saveDefCue({ commit, state, dispatch }) {
            var pram = await dispatch('setCueConFav_save', true)
            var cueInfoData = { ...state.cueInfo }
            if (Object.keys(cueInfoData).includes("newdetail")) {
                cueInfoData.detail.forEach((ele) => {
                    cueInfoData.newdetail.forEach((newItem) => {
                        if (ele.week == newItem.week) {
                            newItem.cueid = ele.cueid
                        }
                    })
                })
                cueInfoData.detail = cueInfoData.newdetail;
                pram.delParams = cueInfoData.delId
            }

            var daycue = [];
            cueInfoData.detail.forEach((ele) => {
                var cueItem = { ...cueInfoData }
                cueItem.detail = [ele];
                if (!Object.keys(cueItem.detail[0]).includes("cueid")) {
                    cueItem.detail[0].cueid = -1
                }
                daycue.push(cueItem);
            })
            pram.DefCueSheetDTO = daycue;

            await $http
                .post(`/api/defCueSheet/SavedefCue`, pram)
                .then(async (res) => {
                    dispatch('saveFavorites', { personid: state.cueInfo.personid, favConParam: pram.favConParam })
                    var params = {
                        productid: cueInfoData.productid,
                        week: cueInfoData.activeWeekList,
                    };
                    await $http.get(`/api/defcuesheet/GetdefCue`, {
                        params: params,
                        paramsSerializer: (params) => {
                            return qs.stringify(params);
                        },
                    })
                        .then((cueRes) => {
                            // 새로 cueid채워주려면 우선 볼러올 cueid를 알아야함 불가능
                            // cue가 아닌 요일 정보로 가져오던가 하나의 cueid만 넘겨도 포함된 모든 cueid를 주도록 해야할듯?
                            cueInfoData.detail = cueRes.data.resultObject.cueSheetDTO.detail
                            cueInfoData.edittime = cueRes.data.resultObject.cueSheetDTO.edittime
                            commit('SET_CUEINFO', cueInfoData)
                            sessionStorage.setItem("USER_INFO", JSON.stringify(cueInfoData));
                        });
                    window.$notify(
                        "info",
                        `기본 큐시트 저장완료.`,
                        '', {
                        duration: 10000,
                        permanent: false
                    }
                    )
                })
                .catch((err => {
                    console.log("saveDefCue" + err);
                    window.$notify(
                        "error",
                        `기본 큐시트 저장실패.`,
                        '', {
                        duration: 10000,
                        permanent: false
                    }
                    )
                }));
        },
        //템플릿 저장
        async saveTempCue({ commit, state, dispatch }) {
            var pram = await dispatch('setCueConFav_save', true)
            pram.CueSheetDTO = state.cueInfo;
            await $http
                .post(`/api/TempCueSheet/SaveTempCue`, pram)
                .then(async (res) => {
                    dispatch('saveFavorites', { personid: state.cueInfo.personid, favConParam: pram.favConParam })
                    var newInfo = { ...state.cueInfo }
                    var params = {
                        cueid: res.data.resultObject
                    };
                    await $http.get(`/api/tempcuesheet/GettempCue`, {
                        params: params,
                        paramsSerializer: (params) => {
                            return qs.stringify(params);
                        },
                    })
                        .then((cueRes) => {
                            newInfo.edittime = cueRes.data.resultObject.cueSheetDTO.edittime
                            commit('SET_CUEINFO', newInfo)
                            sessionStorage.setItem("USER_INFO", JSON.stringify(newInfo));
                        });
                    window.$notify(
                        "info",
                        `템플릿 저장완료.`,
                        '', {
                        duration: 10000,
                        permanent: false
                    }
                    )
                })
                .catch((err => {
                    console.log("saveTempCue" + err);
                    window.$notify(
                        "error",
                        `템플릿 저장실패.`,
                        '', {
                        duration: 10000,
                        permanent: false
                    }
                    )

                }));
        },
        saveFavorites({ }, payload) {
            $http.post(`/api/Favorite/SetFavorites?personid=${payload.personid}`, payload.favConParam);
        },
        //AB, C 필터
        cartCodeFilter({ }, payload) {
            switch (payload.row.cartcode) {
                //MY 디스크
                case "S01G01C007":
                    payload.row.maintitle = payload.search_row.title;
                    payload.search_row.memo != null ? payload.row.subtitle = payload.search_row.memo : payload.row.subtitle = '';
                    break;
                //DL30
                case "S01G01C006":
                    payload.row.maintitle = payload.search_row.recName;
                    payload.row.subtitle = payload.search_row.sourceID;
                    break;
                //음반 기록실
                case "S01G01C014":
                    // Song으로 변환
                    payload.row.cartcode = 'S01G01C032'
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.artistName;
                    payload.row.carttype = "SS";
                    break;
                //Song
                case "S01G01C032":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.artistName;
                    payload.row.carttype = "SS";

                    break;
                //효과음
                case "S01G01C015":
                    // 프로소재로 변환
                    payload.row.cartcode = 'S01G01C013'
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.categoryName;
                    payload.row.carttype = "AC";

                    break;
                //프로소재
                case "S01G01C013":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.categoryName;
                    payload.row.carttype = "AC";
                    break;
                //부조SB
                case "S01G01C017":
                    payload.row.maintitle = payload.search_row.name;
                    if (payload.search_row.brdDT) {
                        payload.row.subtitle = payload.search_row.pgmName;
                        payload.row.onairdate = payload.search_row.brdDT;
                        payload.row.pgmcode = payload.search_row.pgmCODE;
                        payload.row.carttype = "AS";
                    } else {
                        payload.row.subtitle = payload.search_row.categoryName;
                        payload.row.carttype = payload.search_row.categoryID;
                    }
                    break;
                //부조 SPOT
                case "S01G01C010":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.pgmName;
                    payload.row.carttype = "ST";
                    break;
                //프로그램CM
                case "S01G01C018":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.carttype = "CM";
                    if (payload.search_row.brdDT) {
                        payload.row.subtitle = payload.search_row.status;
                        payload.row.onairdate = payload.search_row.brdDT;
                        payload.row.pgmcode = payload.search_row.pgmCODE;
                    } else {
                        payload.row.subtitle = payload.search_row.advertiser;
                    }
                    break;
                //CM
                case "S01G01C019":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.carttype = "CM";
                    if (payload.search_row.brdDT) {
                        payload.row.subtitle = payload.search_row.status;
                        payload.row.onairdate = payload.search_row.brdDT;
                        payload.row.pgmcode = payload.search_row.pgmCODE;
                    } else {
                        payload.row.subtitle = payload.search_row.advertiser;
                    }
                    break;
                //취재물
                case "S01G01C012":
                    payload.row.carttype = "RC";
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.pgmName;
                    break;
                // Filler(PR)
                case "S01G01C021":
                    payload.row.carttype = "FC";
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.categoryName;
                    payload.row.cartid = payload.search_row.id;
                    break;
                // Filler(소재)
                case "S01G01C022":
                    payload.row.carttype = "FC";
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.categoryName;
                    payload.row.cartid = payload.search_row.id;
                    break;
                // Filler(시간)
                case "S01G01C023":
                    payload.row.carttype = "FC";
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.status;
                    payload.row.cartid = payload.search_row.id;
                    break;
                // Filler(기타)
                case "S01G01C024":
                    payload.row.carttype = "FC";
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.categoryName;
                    payload.row.cartid = payload.search_row.id;
                    break;
                // 프로그램
                case "S01G01C009":
                    payload.row.carttype = "PM";
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.status;
                    break;
                // 주조SB
                case "S01G01C016":
                    payload.row.maintitle = payload.search_row.name;
                    if (payload.search_row.brdDT) {
                        payload.row.subtitle = payload.search_row.id;
                        payload.row.onairdate = payload.search_row.brdDT;
                        payload.row.pgmcode = payload.search_row.pgmCODE;
                        payload.row.carttype = "AS";
                    } else {
                        payload.row.subtitle = payload.search_row.categoryName;
                        payload.row.carttype = payload.search_row.categoryID;
                    }

                    break;
                // 주조SPOT
                case "S01G01C020":
                    payload.row.carttype = "MS";
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.brdDT;
                    break;

                default:
                    break;
            }
            return payload.row
        },
        setCueConData({ state, commit }, payload) {
            commit('SET_ABCARTARR', payload.normalCon);
            commit('SET_CCHANNELDATA', payload.instanceCon)
            payload.attachments ? commit('SET_ATTACHMENTS', payload.attachments) : commit('SET_ATTACHMENTS', [])
            commit('SET_TAGS', payload.tags)
            if (payload.printDTO.length > 0) {
                var cueDataObj = { ...state.cueInfo }
                if (Object.keys(cueDataObj).length === 0) {
                    cueDataObj = JSON.parse(sessionStorage.getItem("USER_INFO"));
                }
                payload.printDTO.sort(function (a, b) {
                    if (a.rownum > b.rownum) {
                        return 1;
                    }
                    if (a.rownum < b.rownum) {
                        return -1;
                    }
                    return 0;
                });
            }
            commit('SET_PRINTARR', payload.printDTO);
        },
        async setSponsorList({ commit }, payload) {
            await $http.get(`/api/DayCueSheet/GetAddSponsor`, {
                params: payload,
                paramsSerializer: (params) => {
                    return qs.stringify(params);
                },
            })
                .then((res) => {
                    if (res.data.resultObject) {
                        commit('SET_ABCARTARR', res.data.resultObject);
                    }
                })
        },
        setCueConFav_save({ state }, fav) {
            var printData = state.printArr
            var abData = state.abCartArr
            var cData = state.cChannelData
            var files = state.attachments
            const tags = [];
            state.tags.forEach(item => tags.push(item.text))
            var favData = state.cueFavorites

            //출력용
            var printResult = [];
            printData.forEach((ele, index) => {
                printResult[index] = Object.assign({}, ele);
                printResult[index].rownum = index + 1;
                printResult[index].usedtime = ele.usedtime;
                if (ele.code == "") {
                    printResult[index].code = "CSGP10";
                }
            });
            //AB채널
            var abDataResult = [];
            abData.forEach((ele, index) => {
                abDataResult[index] = Object.assign({}, ele);
                abDataResult[index].channeltype = "N";
                abDataResult[index].rownum = index + 1;
            });

            var pram = {
                PrintDTO: printResult,
                NormalCon: abDataResult,
                InstanceCon: cData,
                Attachments: files,
                Tags: tags
            };
            if (fav) {
                pram.favConParam = favData
            }
            return pram
        },
        //con 모두 지우기
        setclearCon({ state, commit, dispatch }) {
            const printTemplate = _.cloneDeep(state.printTem);
            var insData = {}
            var cueDataObj = { ...state.cueInfo }
            if (Object.keys(cueDataObj).length === 0) {
                cueDataObj = JSON.parse(sessionStorage.getItem("USER_INFO"));
            }
            dispatch(`getPgmCodeKeywordsList`, cueDataObj.pgmcode);
            commit('SET_PRINTARR', printTemplate)
            commit('SET_ABCARTARR', [])
            commit('SET_ATTACHMENTS', [])
            for (var c = 0; 4 > c; c++) {
                var arr = [];
                for (var i = 0; 16 > i; i++) {
                    arr.push({ rownum: i + 1 })
                }
                insData["channel_" + (c + 1)] = arr
            }
            commit('SET_CCHANNELDATA', insData)
        },
        async getPgmCodeKeywordsList({ commit }, payload) {
            await $http.get(`/api/CueUserInfo/GetKeyword?pgmcode=` + payload)
                .then((res) => {
                    const obj = res.data.resultObject;
                    if (obj) {
                        let keywords = obj.replace(/[^\w\s|ㄱ-ㅎ|가-힣|,]/gi, "").replace(/ /g, "")
                        commit(`SET_TAGS`, keywords.split(","))
                    }
                })
        },
        //즐겨찾기 모두 지우기
        setclearFav({ commit }) {
            var favArr = [];
            for (var i = 0; 16 > i; i++) {
                favArr.push({ rownum: i + 1 })
            }
            commit('SET_CUEFAVORITES', favArr)
        },
        setStartTime({ state }) {
            if (state.printArr.length > 0) {
                if (state.cueInfo.r_ONAIRTIME == undefined) {
                    state.printArr[0].starttime = moment(
                        state.cueInfo.brdtime,
                        "YYYY-MM-DDHH:mm:ss"
                    ).valueOf();
                } else {
                    state.printArr[0].starttime = moment(
                        state.cueInfo.r_ONAIRTIME,
                        "YYYY-MM-DDHH:mm:ss"
                    ).valueOf();
                }
                state.printArr.forEach((ele, index) => {
                    if (index != 0) {
                        ele.starttime =
                            state.printArr[index - 1].usedtime +
                            state.printArr[index - 1].starttime;
                    }
                });
            }
        },
        maxLengthChecker({ }, payload) {
            if (payload.arrLength >= payload.maxLength) {
                window.$notify("error", `최대 개수를 초과하였습니다.`, "", {
                    duration: 10000,
                    permanent: false,
                });
                return false;
            } else {
                return true;
            }
        },
        async setContents({ dispatch }, payload) {
            var row;
            var type = payload.type
            var search_row = payload.search_row
            var formRowData = payload.formRowData
            var cartcode = payload.cartcode

            var sortable_index = payload.index
            var sortable_toIndex = payload.toIndex

            switch (type) {
                case "ab":
                    switch (search_row.contentType) {
                        case "P":
                            row = { ...formRowData };
                            row.memo = search_row.contents;
                            return row;

                        case "C":
                            row = { ...search_row };
                            row.transtype = "S";
                            delete row.editTarget;
                            delete row.contentType;
                            return row;

                        case "S":
                            row = { ...formRowData };
                            if (cartcode == "S01G01C014") {
                                search_row = await $http
                                    .post(`/api/SearchMenu/GetSongItem`, search_row)
                                    .then((res) => {
                                        return res.data;
                                    });
                            }
                            if (cartcode == "S01G01C015") {
                                search_row = await $http
                                    .post(`/api/SearchMenu/GetEffectItem`, search_row)
                                    .then((res) => {
                                        return res.data;
                                    });
                            }
                            row.filetoken = search_row.fileToken;
                            row.filepath = search_row.filePath;
                            if (!search_row.intDuration) {
                                row.endposition = 0;
                                row.duration = 0;
                            } else {
                                row.endposition = search_row.intDuration;
                                row.duration = search_row.intDuration;
                            }
                            row.cartid = search_row.id;
                            row.cartcode = cartcode;
                            dispatch(`cartCodeFilter`, ({
                                row: row,
                                search_row: search_row,
                            }));
                            return row;

                        default:
                            break;
                    }
                    break;

                case "print":
                    switch (search_row.contentType) {
                        case "AB":
                            row = { ...formRowData };
                            if (search_row.cartcode == "") {
                                //빈칸
                                row.contents = search_row.memo;
                            } else {
                                //아이템
                                row.contents = search_row.maintitle;
                                row.usedtime = search_row.endposition - search_row.startposition;
                            }
                            return row;

                        case "S":
                            row = { ...formRowData };
                            row.usedtime = search_row.intDuration;
                            switch (cartcode) {
                                case "S01G01C007":
                                    row.contents = search_row.title;
                                    break;
                                case "S01G01C006":
                                    row.contents = search_row.recName;
                                    break;
                                default:
                                    row.contents = search_row.name;
                                    break;
                            }
                            return row;
                        default:
                            break;
                    }
                    break;
                case "c":
                    switch (search_row.contentType) {
                        case "AB":
                            row = { ...search_row };
                            row.rownum = sortable_toIndex + sortable_index;
                            row.edittarget = true;
                            return row;

                        case "S":
                            row = { ...formRowData };
                            row.rownum = sortable_toIndex + sortable_index;
                            if (cartcode == "S01G01C014") {
                                search_row = await $http
                                    .post(`/api/SearchMenu/GetSongItem`, search_row)
                                    .then((res) => {
                                        return res.data;
                                    });
                            }
                            if (cartcode == "S01G01C015") {
                                search_row = await $http
                                    .post(`/api/SearchMenu/GetEffectItem`, search_row)
                                    .then((res) => {
                                        return res.data;
                                    });
                            }
                            row.filetoken = search_row.fileToken;
                            row.filepath = search_row.filePath;
                            if (!search_row.intDuration) {
                                row.endposition = 0;
                                row.duration = 0;
                            } else {
                                row.endposition = search_row.intDuration;
                                row.duration = search_row.intDuration;
                            }
                            row.cartid = search_row.id;
                            row.cartcode = cartcode;
                            dispatch(`cartCodeFilter`, ({
                                row: row,
                                search_row: search_row,
                            }));
                            return row;

                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        },
        getDateStr({ }, payload) {
            const date = payload;
            let sYear = date.getFullYear();
            let sMonth = date.getMonth() + 1;
            let sDate = date.getDate();
            let hours = date.getHours();
            let minutes = date.getMinutes();
            let seconds = date.getSeconds();
            let milliseconds = date.getMilliseconds();

            sMonth = sMonth > 9 ? sMonth : "0" + sMonth;
            sDate = sDate > 9 ? sDate : "0" + sDate;
            hours = hours > 9 ? hours : "0" + hours;
            minutes = minutes > 9 ? minutes : "0" + minutes;
            seconds = seconds > 9 ? seconds : "0" + seconds;

            const result =
                sYear +
                "" +
                sMonth +
                "" +
                sDate +
                "" +
                hours +
                "" +
                minutes +
                "" +
                seconds +
                "" +
                milliseconds;
            return result;
        }
    }
}