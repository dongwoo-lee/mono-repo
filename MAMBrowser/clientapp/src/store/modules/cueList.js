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
    },
    getters: {
        cuesheetListArr: state => state.cuesheetListArr,
        defCuesheetListArr: state => state.defCuesheetListArr,
        tempCuesheetListArr: state => state.tempCuesheetListArr,
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
            switch (payload.type) {
                case "channel_1":
                    state.cChannelData.channel_1 = payload.value;
                    break;

                case "channel_2":
                    state.cChannelData.channel_2 = payload.value;
                    break;

                case "channel_3":
                    state.cChannelData.channel_3 = payload.value;
                    break;

                case "channel_4":
                    state.cChannelData.channel_4 = payload.value;
                    break;

                default:
                    state.cChannelData = [];
                    break;
            }
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
        async getcuesheetListArr({ commit }, payload) {
            await axios.get(`/api/daycuesheet/Getdaycuelist`, {
                params: payload,
                paramsSerializer: (params) => {
                    return qs.stringify(params);
                },
            })
                .then((res) => {
                    commit('SET_CUESHEETLISTARR', res);
                })
                .catch((err => {
                    console.log("getcuesheetListArr" + err);
                }));
        },
        // 기본 큐시트 목록 전체 가져오기
        getcuesheetListArrDef({ state, commit, dispatch }, payload) {
            axios.get(`/api/DefCueSheet/GetDefList`, {
                params: payload,
                paramsSerializer: (params) => {
                    return qs.stringify(params);
                },
            })
                .then(async (res) => {
                    var productWeekList = await dispatch('disableList', res.data);
                    var seqnum = 0;
                    res.data.forEach((ele) => {
                        var activeWeekList = [];
                        var cueids = [];
                        ele.productWeekList = productWeekList.filter((week) => {
                            return week.productid == ele.productid;
                        });
                        ele.detail.forEach((activeWeek) => {
                            activeWeekList.push(activeWeek.week);
                            cueids.push(activeWeek.cueid);
                        });
                        ele.activeWeekList = activeWeekList;
                        ele.cueid = cueids;
                        ele.seq = seqnum;
                        seqnum = seqnum + 1;
                    });
                    commit('SET_DEFCUESHEETLISTARR', res);
                })
                .catch((err => {
                    console.log("getcuesheetListArrDef" + err);
                }));
        },
        // 템플릿 목록 전체 가져오기
        getcuesheetListArrTemp({ commit }, payload) {
            axios.get(`/api/TempCueSheet/GetTempList?personid=${payload.personid}&title=${payload.temptitle}`)
                .then((res) => {
                    var seqnum = 0;
                    res.data.forEach((ele) => {
                        ele.seq = seqnum;
                        seqnum = seqnum + 1;
                    });
                    commit('SET_TEMPCUESHEETLISTARR', res);
                })
                .catch((err => {
                    console.log("getcuesheetListArrTemp" + err);
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
        },
        //상세내용 -즐겨찾기
        async getCueDayFav({ state, commit, dispatch }, payload) {
            await axios.get(
                `/api/Favorite/GetFavorites?personid=${payload}`)
                .then((res) => {
                    var favResult = [];
                    for (var i = 0; 16 > i; i++) {
                        var row = {};
                        var rowNum_fav = 0;
                        for (var index = 0; res.data.length > index; index++) {
                            if (res.data[index].seqnum == i + 1) {
                                row = res.data[index];
                                row.rowNum = rowNum_fav;
                                row.transtype = "N";
                                row.filePath = res.data[index].cons[0].p_MASTERFILE
                                row.editTarget = true;
                                rowNum_fav = rowNum_fav + 1;
                                row.duration = moment(row.endposition)
                                    .add(-9, "hours")
                                    .format("HH:mm:ss.SS");
                                dispatch('productFilter', row);

                            }
                        }
                        favResult.push(row);
                    }
                    commit('SET_CUEFAVORITES', favResult);
                })
                .catch((err => {
                    console.log("getCueDayFav" + err);
                }));
        },
        //일일큐시트 저장
        async saveDayCue({ commit, state, dispatch }) {
            var pram = await dispatch('setCueConFav_save', true)
            var dayData = {
                brddate: state.cueInfo.detail[0].brddate,
                brdtime: state.cueInfo.detail[0].brdtime,
            };
            pram.cueParam = state.cueInfo;
            pram.cueParam.cueid = state.cueInfo.detail[0].cueid
            pram.dayParam = dayData;
            await axios
                .post(`/api/dayCueSheet/SavedayCue`, pram)
                .then(async (res) => {
                    await axios.post(`/api/Favorite/SetFavorites?personid=${state.cueInfo.personid}`, pram);
                    var newInfo = state.cueInfo
                    newInfo.detail[0].cueid = res.data.cueID
                    commit('SET_CUEINFO', newInfo)
                })
                .catch((err => {
                    console.log("saveDayCue" + err);
                }));

        },
        //기본큐시트 저장
        async saveDefCue({ state, dispatch }) {
            //디렉터가 라디오기술부로 저장됨 왜이럼
            var pram = await dispatch('setCueConFav_save', true)
            var defParams = [];
            state.cueInfo.detail.forEach((ele) => {
                defParams.push(ele.week);
            });
            pram.defParams = defParams;
            pram.cueParam = state.cueInfo;
            pram.cueParam.cueid = -1
            await axios
                .post(`/api/defCueSheet/SavedefCue`, pram)
                .then(async (res) => {
                    await axios.post(`/api/Favorite/SetFavorites?personid=${state.cueInfo.personid}`, pram);
                    console.log("저장완료")
                })
                .catch((err => {
                    console.log("saveDefCue" + err);
                }));
        },
        //템플릿 저장
        async saveTempCue({ state, dispatch }) {
            var pram = await dispatch('setCueConFav_save', true)
            pram.temParam = state.cueInfo;
            pram.temParam.cueid = state.cueInfo.detail[0].cueid
            pram.temParam.tmptitle = state.cueInfo.detail[0].tmptitle
            await axios
                .post(`/api/TempCueSheet/SaveTempCue`, pram)
                .then(async (res) => {
                    await axios.post(`/api/Favorite/SetFavorites?personid=${state.cueInfo.personid}`, pram);
                    console.log("저장완료")
                })
                .catch((err => {
                    console.log("saveTempCue" + err);
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
        //con + 출력용 가공 (가져오기)
        setCueConData({ commit, dispatch }, payload) {
            //출력용
            var printData = [];
            payload.conData.data.prints.forEach((ele, index) => {
                printData[index] = Object.assign({}, ele);
                printData[index].rowNum = index;
                printData[index].code = ele.code.trim();
                printData[index].duration = ""
                ele.contents == null ? printData[index].contents = "" : printData[index].contents = ele.contents;
                ele.etc == null ? printData[index].etc = "" : printData[index].etc = ele.etc;
                ele.starttime == null ? printData[index].starttime = "" : printData[index].starttime = ele.starttime;

                // printData[index].contents = ele.contents;
                // printData[index].etc = ele.etc;
                // printData[index].starttime = ele.starttime;
                delete printData[index].seqnum;
            });

            const cueSheetCons = payload.conData.data.cueSheetCons;
            var rowNum_ab = 0;
            var rowNum_c = 0;

            //AB채널
            var abData = cueSheetCons.filter((ele) => {
                if (ele.channeltype == "N") {
                    ele.rowNum = rowNum_ab;
                    ele.filePath = ele.cons[0].p_MASTERFILE
                    rowNum_ab = rowNum_ab + 1;
                    ele.duration = moment(ele.endposition)
                        .add(-9, "hours")
                        .format("HH:mm:ss.SS");
                    dispatch('productFilter', ele);
                    //this.productFilter(ele);
                    return ele;
                }
            });
            //C채널 -그룹
            var cDataGroup = cueSheetCons.filter((ele) => {
                if (ele.channeltype == "I") {
                    ele.rowNum = rowNum_c;
                    ele.filePath = ele.cons[0].p_MASTERFILE
                    ele.editTarget = true;
                    rowNum_c = rowNum_c + 1;
                    ele.duration = moment(ele.endposition)
                        .add(-9, "hours")
                        .format("HH:mm:ss.SS");
                    dispatch('productFilter', ele);
                    //   this.productFilter(ele);
                    return ele;
                }
            });

            //C채널 - 카트별
            var cDataResult = [];
            var row = {};
            for (var channelNum = 0; 4 > channelNum; channelNum++) {
                cDataResult = [];
                for (var i = 0; 16 > i; i++) {
                    for (var index = 0; cDataGroup.length > index; index++) {
                        if (
                            cDataGroup[index].seqnum ==
                            i + 16 * channelNum + 1
                        ) {
                            row = cDataGroup[index];
                            break;
                        } else {
                            row = {};
                        }
                    }
                    cDataResult.push(row);
                }
                commit('SET_CCHANNELDATA', {
                    type: "channel_" + (channelNum + 1),
                    value: cDataResult,
                })
            }
            commit('SET_PRINTARR', printData);
            commit('SET_ABCARTARR', abData);
            commit('SET_CUEINFO', payload.cueData);
        },
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
                printResult[index].seqnum = index + 1;
                printResult[index].starttime = ele.duration;
                delete printResult[index].rowNum;
                if (ele.code == "") {
                    printResult[index].code = "CSGP10";
                }
            });
            //AB채널
            var abDataResult = [];
            abData.forEach((ele, index) => {
                abDataResult[index] = Object.assign({}, ele);
                abDataResult[index].channeltype = "N";
                abDataResult[index].seqnum = index + 1;
            });
            //C채널
            var cDataGroup = [];
            var seqnum = 1;
            for (let i = 0; i <= 3; i++) {
                cData[Object.keys(cData)[i]].forEach((ele) => {
                    cDataGroup.push(Object.assign({}, ele));
                });
            }
            var cDataResult = [];
            cDataGroup.forEach((ele, index) => {
                if (Object.keys(ele).length !== 0) {
                    ele.channeltype = "I";
                    ele.seqnum = index + 1;
                    cDataResult.push(ele);
                }
                seqnum = seqnum + 1;
            });
            var conParams = abDataResult.concat(cDataResult);
            var pram = {
                conParams: conParams,
                printParams: printResult,
                // favConParam: favDataResult,
            };
            if (fav) {
                var favDataResult = [];
                var favSeqnum = 1;
                favData.forEach((ele) => {
                    if (Object.keys(ele).length !== 0) {
                        ele.seqnum = favSeqnum;
                        //불방처리부분 개발되면 변경하기 우선 Y로 해놓음
                        ele.useflag = "Y";
                        favDataResult.push(ele);
                    }
                    favSeqnum = favSeqnum + 1;
                });
                pram.favConParam = favDataResult
            }
            return pram
        },
        //con 모두 지우기
        setclearCon({ commit }, payload) {
            commit('SET_PRINTARR', [])
            commit('SET_ABCARTARR', [])
            commit('SET_CUEINFO', payload)
            for (var c = 0; 4 > c; c++) {
                var arr = [];
                for (var i = 0; 16 > i; i++) {
                    arr.push({})
                }
                commit('SET_CCHANNELDATA', {
                    type: "channel_" + (c + 1),
                    value: arr,
                })
            }

        },
        //즐겨찾기 모두 지우기
        setclearFav({ commit }) {
            var favArr = [];
            for (var i = 0; 16 > i; i++) {
                favArr.push({})
            }
            commit('SET_CUEFAVORITES', favArr)
        }
    }
}