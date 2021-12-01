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
            // switch (payload.type) {
            //     case "channel_1":
            //         state.cChannelData.channel_1 = payload.value;
            //         break;

            //     case "channel_2":
            //         state.cChannelData.channel_2 = payload.value;
            //         break;

            //     case "channel_3":
            //         state.cChannelData.channel_3 = payload.value;
            //         break;

            //     case "channel_4":
            //         state.cChannelData.channel_4 = payload.value;
            //         break;

            //     default:
            //         state.cChannelData = [];
            //         break;
            // }
        },
        SET_CUEFAVORITES(state, payload) {
            state.cueFavorites = payload;
        },
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
                    console.log(res);
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
                    console.log(res)
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
                    console.log(res);
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
                    console.log(res)
                    alert("템플릿 추가완료")
                })
        },
        //상세내용 -즐겨찾기
        async getCueDayFav({ state, commit, dispatch }, payload) {
            await axios.get(
                `/api/Favorite/GetFavorites?personid=${payload}`)
                .then((res) => {
                    // var favResult = [];
                    // var rowNum_fav = 0;
                    // for (var i = 0; 16 > i; i++) {
                    //     var row = {};
                    //     for (var index = 0; res.data.length > index; index++) {
                    //         if (res.data[index].seqnum == i + 1) {
                    //             row = res.data[index];
                    //             row.rowNum = rowNum_fav;
                    //             row.transtype = "N";
                    //             row.filePath = res.data[index].cons[0].p_MASTERFILE
                    //             row.editTarget = true;
                    //             rowNum_fav = rowNum_fav + 1;
                    //             row.duration = moment(row.endposition)
                    //                 .add(-9, "hours")
                    //                 .format("HH:mm:ss.SS");
                    //             dispatch('productFilter', row);

                    //         }
                    //     }
                    //     favResult.push(row);
                    // }
                    // commit('SET_CUEFAVORITES', favResult);
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
            console.log("pram")
            console.log(pram)
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
                    alert("저장완료");
                })
                .catch((err => {
                    console.log("saveDayCue" + err.message);
                    alert("오류발생");
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
                    alert("저장완료");
                })
                .catch((err => {
                    console.log("saveDefCue" + err);
                    alert("오류발생");
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
                    console.log(state.cueInfo);
                    alert("저장완료");

                })
                .catch((err => {
                    console.log("saveTempCue" + err);
                    alert("오류발생");

                }));
        },
        productFilter({ }, arr) {
            switch (arr.cartcode) {
                case "S01G01C011":
                    arr.productType = "PUBLIC_FILE";
                    break;
                case "S01G01C013":
                    arr.productType = "OLD_PRO";
                    break;
                case "S01G01C017":
                    arr.productType = "SCR_SB";
                    break;
                case "S01G01C010":
                    arr.productType = "SCR_SPOT";
                    break;
                case "S01G01C018":
                    arr.productType = "PGM_CM";
                    break;
                case "S01G01C019":
                    arr.productType = "CM";
                    break;
                case "S01G01C012":
                    arr.productType = "REPOTE";
                    break;
                case "S01G01C021":
                    arr.productType = "FILLER_PR";
                    break;
                case "S01G01C022":
                    arr.productType = "FILLER_MT";
                    break;
                case "S01G01C023":
                    arr.productType = "FILLER_TIME";
                    break;
                case "S01G01C024":
                    arr.productType = "FILLER_ETC";
                    break;
                case "S01G01C009":
                    arr.productType = "PGM";
                    break;
                case "S01G01C016":
                    arr.productType = "MCR_SB";
                    break;
                case "S01G01C020":
                    arr.productType = "MCR_SPOT";
                    break;

                default:
                    break;
            }
            return arr;
        },
        //AB, C 필터
        cartCodeFilter({ }, payload) {
            switch (payload.row.cartcode) {
                case "S01G01C011":
                    payload.row.maintitle = payload.search_row.title;
                    payload.row.subtitle = payload.search_row.categoryName;
                    break;
                case "S01G01C013":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.categoryName;
                    break;
                case "S01G01C017":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.pgmName;
                    payload.row.onairdate = payload.search_row.brdDT;
                    payload.row.cartid = payload.search_row.id;
                    break;
                case "S01G01C010":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.pgmName;
                    break;
                case "S01G01C018":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.status;
                    payload.row.onairdate = payload.search_row.brdDT;
                    payload.row.cartid = payload.search_row.id;
                    break;
                case "S01G01C019":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.status;
                    payload.row.onairdate = payload.search_row.brdDT;
                    payload.row.cartid = payload.search_row.id;
                    break;
                case "S01G01C012":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.pgmName;
                    break;
                case "S01G01C021":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.categoryName;
                    payload.row.cartid = payload.search_row.id;
                    break;
                case "S01G01C022":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.categoryName;
                    payload.row.cartid = payload.search_row.id;
                    break;
                case "S01G01C023":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.status;
                    payload.row.cartid = payload.search_row.id;
                    break;
                case "S01G01C024":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.categoryName;
                    payload.row.cartid = payload.search_row.id;
                    break;
                case "S01G01C009":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.status;
                    break;
                case "S01G01C016":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.id;
                    payload.row.onairdate = payload.search_row.brdDT;
                    payload.row.cartid = payload.search_row.id;
                    break;
                case "S01G01C020":
                    payload.row.maintitle = payload.search_row.name;
                    payload.row.subtitle = payload.search_row.brdDT;
                    payload.row.cartid = payload.search_row.id;
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
                payload.printDTO.forEach((ele) => {
                    if (ele.rownum == 1) {
                        if (cueDataObj.r_ONAIRTIME == undefined) {
                            ele.starttime = moment(cueDataObj.brdtime, "YYYY-MM-DDHH:mm:ss").valueOf();
                        } else {
                            ele.starttime = moment(cueDataObj.r_ONAIRTIME, "YYYY-MM-DDHH:mm:ss").valueOf();
                        }
                        // var time1 = moment("00:00:00", "HH:mm:ss");
                        // var time2 = moment(cueDataObj.brdtime, "YYYY-MM-DDTHH:mm:ss");
                        // ele.starttime = moment
                        //     .duration(time2.diff(time1))
                        //     .asMilliseconds();
                    }
                })
            }
            commit('SET_PRINTARR', payload.printDTO);
        },
        //con + 출력용 가공 (가져오기)
        // setCueConData({ commit, dispatch }, payload) {
        //     //출력용
        //     var printData = [];
        //     payload.data.prints.forEach((ele, index) => {
        //         printData[index] = Object.assign({}, ele);
        //         printData[index].rowNum = index;
        //         printData[index].code = ele.code.trim();
        //         printData[index].usedtime = ele.usedtime;
        //         ele.contents == null ? printData[index].contents = "" : printData[index].contents = ele.contents;
        //         ele.etc == null ? printData[index].etc = "" : printData[index].etc = ele.etc;
        //         ele.starttime == null ? printData[index].starttime = "" : printData[index].starttime = ele.starttime;

        //         // printData[index].contents = ele.contents;
        //         // printData[index].etc = ele.etc;
        //         // printData[index].starttime = ele.starttime;
        //         delete printData[index].seqnum;
        //     });

        //     const cueSheetCons = payload.data.cueSheetCons;
        //     var rowNum_ab = 0;
        //     var rowNum_c = 0;
        //     var filePath = []; //그룹 소재의 경우 여러개 , 나중에 이부분 수정 필요함

        //     //AB채널
        //     var abData = cueSheetCons.filter((ele) => {
        //         if (ele.channeltype == "N") {
        //             ele.rowNum = rowNum_ab;
        //             ele.filePath = ele.cons[0].p_MASTERFILE
        //             rowNum_ab = rowNum_ab + 1;
        //             ele.duration = ele.cons[0].p_DURATION
        //             // ele.duration = moment(ele.endposition)
        //             //     .add(-9, "hours")
        //             //     .format("HH:mm:ss.SS");
        //             dispatch('productFilter', ele);
        //             //this.productFilter(ele);
        //             return ele;
        //         }
        //     });
        //     //C채널 -그룹
        //     var cDataGroup = cueSheetCons.filter((ele) => {
        //         if (ele.channeltype == "I") {
        //             ele.rowNum = rowNum_c;
        //             ele.filePath = ele.cons[0].p_MASTERFILE
        //             ele.editTarget = true;
        //             rowNum_c = rowNum_c + 1;
        //             ele.duration = ele.cons[0].p_DURATION
        //             // ele.duration = moment(ele.endposition)
        //             //     .add(-9, "hours")
        //             //     .format("HH:mm:ss.SS");
        //             dispatch('productFilter', ele);
        //             //   this.productFilter(ele);
        //             return ele;
        //         }
        //     });

        //     //C채널 - 카트별
        //     var cDataResult = [];
        //     var row = {};
        //     for (var channelNum = 0; 4 > channelNum; channelNum++) {
        //         cDataResult = [];
        //         for (var i = 0; 16 > i; i++) {
        //             for (var index = 0; cDataGroup.length > index; index++) {
        //                 if (
        //                     cDataGroup[index].seqnum ==
        //                     i + 16 * channelNum + 1
        //                 ) {
        //                     row = cDataGroup[index];
        //                     break;
        //                 } else {
        //                     row = {};
        //                 }
        //             }
        //             cDataResult.push(row);
        //         }
        //         commit('SET_CCHANNELDATA', {
        //             type: "channel_" + (channelNum + 1),
        //             value: cDataResult,
        //         })
        //     }
        //     commit('SET_PRINTARR', printData);
        //     commit('SET_ABCARTARR', abData);
        // },
        //con + 출력용 가공 (저장), 이거 나중에 즐겨찾기를 따로 빼기 즐찾 가공이랑 즐찾 저장이랑 2개로 나누기
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
            printTemplate.forEach((ele) => {
                if (ele.rownum == 1) {
                    console.log(cueDataObj.r_ONAIRTIME)
                    ele.starttime = moment(cueDataObj.r_ONAIRTIME, "YYYY-MM-DDHH:mm:ss").valueOf();
                    // var time1 = moment("00:00:00", "HH:mm:ss");
                    // var time2 = moment(cueDataObj.brdtime, "YYYY-MM-DDTHH:mm:ss");
                    // ele.starttime = moment
                    //     .duration(time2.diff(time1))
                    //     .asMilliseconds();
                }
            })
            console.log("state.printTem");
            console.log(state.printTem);
            console.log("printTemplate");
            console.log(printTemplate);
            commit('SET_PRINTARR', printTemplate)
            commit('SET_ABCARTARR', [])
            // commit('SET_CUEINFO', payload)
            for (var c = 0; 4 > c; c++) {
                var arr = [];
                for (var i = 0; 16 > i; i++) {
                    arr.push({ rownum: i + 1 })
                }
                insData["channel_" + (c + 1)] = arr
                // commit('SET_CCHANNELDATA', {
                //     type: "channel_" + (c + 1),
                //     value: arr,
                // })
            }
            commit('SET_CCHANNELDATA', insData)

        },
        //즐겨찾기 모두 지우기
        setclearFav({ commit }) {
            var favArr = [];
            for (var i = 0; 16 > i; i++) {
                favArr.push({})
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
    }
}