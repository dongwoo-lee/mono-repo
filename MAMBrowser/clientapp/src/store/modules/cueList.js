import axios from "axios";
const qs = require("qs");
import "moment/locale/ko";
const moment = require("moment");

export default {
    namespaced: true,
    state: {
        cuesheetListArr: [], //일일큐시트 리스트 목록
        defCuesheetListArr: [], //기본큐시트 리스트 목록
        tempCuesheetListArr: [], //템플릿 리스트 목록
        archiveCuesheetListArr: [], //이전 큐시트 리스트 목록


        userProOption: [], //리스트 옵션 - 프로그램명
        mediasOption: [], //리스트 옵션 - 매체
        userProList: [], //유저 - 전체 프로그램
        proUserList: "", //프로그램 - 전체 유저

        cueInfo: {}, //작성 정보 (CUE)
        searchListData: [], //소재

        printArr: [], //출력용
        abCartArr: [], //ab 카트
        cChannelData: [], //c 카트
        cueFavorites: [], //즐겨찾기
        cueSheetAutoSave: true,
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
        userProOption: state => state.userProOption,
        mediasOption: state => state.mediasOption,
        userProList: state => state.userProList,
        proUserList: state => state.proUserList,
        cueInfo: state => state.cueInfo,
        searchListData: state => state.searchListData,
        printArr: state => state.printArr,
        abCartArr: state => state.abCartArr,
        cChannelData: state => state.cChannelData,
        cueFavorites: state => state.cueFavorites,
        printTem: state => state.printTem,
        cueSheetAutoSave: state => state.cueSheetAutoSave,
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
        SET_USERPROOPTION(state, payload) {
            state.userProOption = payload;
        },
        SET_MEDIASOPTION(state, payload) {
            state.mediasOption = payload;
        },
        SET_USERPROLIST(state, payload) {
            state.userProList = payload;
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
        SET_CUEFAVORITES(state, payload) {
            state.cueFavorites = payload;
        },
        SET_CUESHEETAUTOSAVE(state, payload) {
            state.cueSheetAutoSave = payload;
        }
    },
    actions: {
        //리스트 옵션 - 프로그램명 가져오기
        async getuserProOption({ commit }, payload) {
            return await axios.get(`/api/CueUserInfo/GetProgramList?personid=${payload.personid}&media=${payload.media}`)
                .then((res) => {
                    var dataList = res.data
                    var products = [];
                    if (dataList) {
                        dataList.forEach((ele) => {
                            ele.details.filter((pro) => {
                                products.push({
                                    value: pro.productid,
                                    text: pro.eventname,
                                });
                            });
                        });
                    }
                    commit('SET_USERPROOPTION', products);
                })
                .catch((err => {
                    console.log("getuserProOption" + err);
                }));
        },
        //유저 - 전체 프로그램 + 매체 가져오기
        async getMediasOption({ commit }, payload) {
            return await axios.get(`/api/CueUserInfo/GetProgramList?personid=` + payload)
                .then((res) => {
                    var dataList = res.data
                    var medias = [];
                    var products = [];
                    if (dataList) {
                        dataList.forEach((ele) => {
                            switch (ele.media) {
                                case "A":
                                    medias.push({
                                        value: ele.media,
                                        text: "AM"
                                    })
                                    break;
                                case "F":
                                    medias.push({
                                        value: ele.media,
                                        text: "FM"
                                    })
                                    break;
                                case "D":
                                    medias.push({
                                        value: ele.media,
                                        text: "DMB"
                                    })
                                    break;
                                case "C":
                                    medias.push({
                                        value: ele.media,
                                        text: "공통"
                                    })
                                    break;
                                case "Z":
                                    medias.push({
                                        value: ele.media,
                                        text: "기타"
                                    })
                                    break;
                                default:
                                    break;
                            }
                            ele.details.filter((pro) => {
                                products.push(pro.productid);
                            });
                        });
                    }
                    commit('SET_MEDIASOPTION', medias)
                    commit('SET_USERPROLIST', products)

                })
                .catch((err => {
                    console.log("getMediasOption" + err);
                }));
        },
        //프로그램 - 전체 유저 가져오기
        async getProUserList({ commit }, payload) {
            await axios.get(`/api/CueUserInfo/GetDirectorList?productid=` + payload)
                .then((res) => {
                    commit('SET_PROUSERLIST', res.data)
                })
                .catch((err => {
                    console.log("getProUserList" + err);
                }));
        },
        // 일일 큐시트 목록 전체 가져오기
        getcuesheetListArr({ commit }, payload) {
            return axios.get(`/api/daycuesheet/Getdaycuelist`, {
                params: payload,
                paramsSerializer: (params) => {
                    return qs.stringify(params);
                },
            })
                .then((res) => {
                    res.data.resultObject.data.sort((a, b) => {
                        return new Date(a.r_ONAIRTIME) - new Date(b.r_ONAIRTIME)
                    })
                    commit('SET_CUESHEETLISTARR', res.data.resultObject);
                    return res;
                })
                .catch((err => {
                    console.log("getcuesheetListArr" + err);
                }));
        },
        // 기본 큐시트 목록 전체 가져오기
        getcuesheetListArrDef({ commit, dispatch }, payload) {
            return axios.get(`/api/DefCueSheet/GetDefList`, {
                params: payload,
                paramsSerializer: (params) => {
                    return qs.stringify(params);
                },
            })
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
            return axios.get(`/api/TempCueSheet/GetTempList?personid=${payload.personid}&title=${payload.temptitle}`)
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
            return axios.get(`/api/ArchiveCueSheet/GetArchiveCueList`, {
                params: payload,
                paramsSerializer: (params) => {
                    return qs.stringify(params);
                },
            })
                .then((res) => {
                    commit('SET_ARCHIVECUESHEETLISTARR', res.data.resultObject);
                    return res;
                })
                .catch((err => {
                    console.log("getarchiveCuesheetListArr" + err);
                }));

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
        //템플릿 추가 + 템플릿으로 저장 (나중에 템플릿 저장이랑 합치기)
        async addTemplate({ }, payload) {
            await axios
                .post(`/api/TempCueSheet/SaveTempCue`, payload)
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
        },
        //상세내용 -즐겨찾기
        async getCueDayFav({ state, commit, dispatch }, payload) {
            await axios.get(
                `/api/Favorite/GetFavorites?personid=${payload.personid}&pgmcode=${payload.pgmcode}&brd_dt=${payload.brd_dt}`)
                .then((res) => {
                    commit('SET_CUEFAVORITES', res.data);
                })
                .catch((err => {
                    console.log("getCueDayFav" + err);
                }));
        },
        //일일큐시트 저장
        async saveDayCue({ commit, state, dispatch }) {
            var pram = await dispatch('setCueConFav_save', true)
            pram.CueSheetDTO = state.cueInfo;
            await axios
                .post(`/api/DayCueSheet/SaveDayCue`, pram)
                .then(async (res) => {
                    await axios.post(`/api/Favorite/SetFavorites?personid=${state.cueInfo.personid}`, pram.favConParam);
                    var newInfo = { ...state.cueInfo }
                    var params = { productid: newInfo.productid, cueid: res.data }
                    await axios.get(`/api/daycuesheet/GetdayCue`, {
                        params: params,
                        paramsSerializer: (params) => {
                            return qs.stringify(params);
                        },
                    }).then((cueRes) => {
                        newInfo.detail[0].cueid = res.data
                        newInfo.edittime = cueRes.data.cueSheetDTO.edittime
                        delete newInfo.cueid
                        commit('SET_CUEINFO', newInfo)
                        sessionStorage.setItem("USER_INFO", JSON.stringify(newInfo));
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

            await axios
                .post(`/api/defCueSheet/SavedefCue`, pram)
                .then(async (res) => {
                    await axios.post(`/api/Favorite/SetFavorites?personid=${state.cueInfo.personid}`, pram.favConParam);
                    var params = {
                        productid: cueInfoData.productid,
                        week: cueInfoData.activeWeekList,
                    };
                    await axios.get(`/api/defcuesheet/GetdefCue`, {
                        params: params,
                        paramsSerializer: (params) => {
                            return qs.stringify(params);
                        },
                    })
                        .then((cueRes) => {
                            // 새로 cueid채워주려면 우선 볼러올 cueid를 알아야함 불가능
                            // cue가 아닌 요일 정보로 가져오던가 하나의 cueid만 넘겨도 포함된 모든 cueid를 주도록 해야할듯?
                            cueInfoData.detail = cueRes.data.cueSheetDTO.detail
                            cueInfoData.edittime = cueRes.data.cueSheetDTO.edittime
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
            await axios
                .post(`/api/TempCueSheet/SaveTempCue`, pram)
                .then(async (res) => {
                    await axios.post(`/api/Favorite/SetFavorites?personid=${state.cueInfo.personid}`, pram.favConParam);
                    var newInfo = { ...state.cueInfo }
                    var params = {
                        cueid: res.data
                    };
                    await axios.get(`/api/tempcuesheet/GettempCue`, {
                        params: params,
                        paramsSerializer: (params) => {
                            return qs.stringify(params);
                        },
                    })
                        .then((cueRes) => {
                            newInfo.edittime = cueRes.data.cueSheetDTO.edittime
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
        //AB, C 필터
        cartCodeFilter({ }, payload) {
            switch (payload.row.cartcode) {
                //MY 디스크
                case "S01G01C007":
                    payload.row.maintitle = payload.search_row.title;
                    payload.row.subtitle = payload.search_row.memo;
                    break;
                //DL30
                case "S01G01C006":
                    payload.row.maintitle = payload.search_row.recName;
                    payload.row.subtitle = payload.search_row.sourceID;
                    break;
                //음반 기록실
                case "S01G01C014":
                    payload.row.maintitle = payload.search_row.songName;
                    payload.row.subtitle = payload.search_row.artistName;
                    break;
                //효과음
                case "S01G01C015":
                    payload.row.maintitle = payload.search_row.songName;
                    payload.row.subtitle = payload.search_row.artistName;
                    break;
                //(구)프로소재
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
        //DTO 하는중
        setCueConData({ state, commit }, payload) {
            // commit('SET_CUEINFO', payload.cue)
            commit('SET_ABCARTARR', payload.normalCon);
            commit('SET_CCHANNELDATA', payload.instanceCon)
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
        setSponsorList({ commit }, payload) {
            axios.get(`/api/DayCueSheet/GetAddSponsor`, {
                params: payload,
                paramsSerializer: (params) => {
                    return qs.stringify(params);
                },
            })
                .then((res) => {
                    commit('SET_ABCARTARR', res.data);
                })
                .catch((err => {
                    console.log("setSponsorList" + err);
                }));
        },
        setCueConFav_save({ state }, fav) {
            var printData = state.printArr
            var abData = state.abCartArr
            var cData = state.cChannelData
            var favData = state.cueFavorites
            //출력용
            var printResult = [];
            printData.forEach((ele, index) => {
                printResult[index] = Object.assign({}, ele);
                printResult[index].rownum = index + 1;
                printResult[index].usedtime = ele.usedtime;
                //delete printResult[index].rowNum;
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
                //CueSheetDTO: conParams,
                PrintDTO: printResult,
                NormalCon: abDataResult,
                InstanceCon: cData
            };
            if (fav) {
                pram.favConParam = favData
            }
            return pram
        },
        //con 모두 지우기
        setclearCon({ state, commit }) {
            const printTemplate = [...state.printTem]
            var insData = {}
            var cueDataObj = { ...state.cueInfo }
            if (Object.keys(cueDataObj).length === 0) {
                cueDataObj = JSON.parse(sessionStorage.getItem("USER_INFO"));
            }
            commit('SET_PRINTARR', printTemplate)
            commit('SET_ABCARTARR', [])
            // commit('SET_CUEINFO', payload)
            for (var c = 0; 4 > c; c++) {
                var arr = [];
                for (var i = 0; 16 > i; i++) {
                    arr.push({ rownum: i + 1 })
                }
                insData["channel_" + (c + 1)] = arr
            }
            commit('SET_CCHANNELDATA', insData)

        },
        //즐겨찾기 모두 지우기
        setclearFav({ commit }) {
            var favArr = [];
            for (var i = 0; 16 > i; i++) {
                favArr.push({ rownum: i + 1 })
            }
            commit('SET_CUEFAVORITES', favArr)
        },
        //시간 string > milliseconds
        millisecondsFuc({ }, payload) {
            var itemTime = moment(payload, "HH:mm:ss.SSS");
            var defTime = moment("00:00:00.0", "HH:mm:ss.SSS");
            var result = moment.duration(itemTime.diff(defTime)).asMilliseconds();
            return result
        },
        getautosave({ commit }, payload) {
            return axios.get(`/api/users/summary/${payload}`)
                .then((res) => {
                    var autosave = null
                    if (res.data.resultObject.cueSheetAutoSave == "Y") {
                        autosave = true
                    } else {
                        autosave = false
                    }
                    commit('SET_CUESHEETAUTOSAVE', autosave);
                })
        },
        setautosave({ }, payload) {
            return axios.patch(`/api/user`, payload)
                .then((res) => {
                    console.log(res)
                })
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
                    if (index != 0)
                        ele.starttime =
                            state.printArr[index - 1].usedtime +
                            state.printArr[index - 1].starttime;
                });
            }
        },
    }
}