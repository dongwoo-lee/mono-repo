import axios from "axios";
const qs = require("qs");

export default {
    namespaced: true,
    state: {
        userProOption: [], //리스트 옵션 프로그램 목록
        mediasOption: [], //리스트 옵션 미디어 목록
        userProList: [], //유저 전체 프로그램
        proUserList: "", //프로그램별 전체 유저 목록

        abChannelData: [], //AB채널
        cChannelData: [], //C채널
        cuePrint: [], //출력용
        cueConFile: [], //큐시트 첨부파일
        cueTag: [], //태그 첨부파일
        cueFavorites: [], //즐겨찾기

        searchListData: [], //소재
        seleDayCue: {}, //선택 큐시트
        weekData: []
    },
    getters: {
        userProOption: state => state.userProOption,
        cueConFile: state => state.cueConFile,
        cuePrint: state => state.cuePrint,
        cueTag: state => state.cueTag,
        cueFavorites: state => state.cueFavorites,
        abChannelData: state => state.abChannelData,
        cChannelData: state => state.cChannelData,
        searchListData: state => state.searchListData,
        mediasOption: state => state.mediasOption,
        userProList: state => state.userProList,
        seleDayCue: state => state.seleDayCue,
        proUserList: state => state.proUserList,
        weekData: state => state.weekData
    },
    mutations: {
        SET_USERPROOPTION(state, payload) {
            state.userProOption = payload;
        },
        SET_CUECONFILE(state, payload) {
            state.cueConFile = payload;
        },
        SET_CUEPRINT(state, payload) {
            state.cuePrint = payload;
        },
        SET_CUETAG(state, payload) {
            state.cueTag = payload;
        },
        SET_CUEFAVORITES(state, payload) {
            state.cueFavorites = payload;
        },
        SET_ABCHANNELDATA(state, payload) {
            state.abChannelData = payload;
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
        SET_SEARCHLISTDATA(state, payload) {
            state.searchListData = payload;
        },
        SET_MEDIASOPTION(state, payload) {
            state.mediasOption = payload;
        },
        SET_USERPROLIST(state, payload) {
            state.userProList = payload;
        },
        SET_SELEDAYCUE(state, payload) {
            state.seleDayCue = payload;
        },
        SET_PROUSERLIST(state, payload) {
            state.proUserList = payload;
        },
        SET_WEEKDATA(state, payload) {
            state.weekData = payload;
        },
    },
    actions: {
        //유저당 프로그램 목록 가져오기
        getuserProOption({ commit }, payload) {
            return axios.get(`/api/CueUserInfo/GetProgramList?personid=${payload.personid}&media=${payload.media}`)
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
                    console.log(err);
                }))
        },
        getMediasOption({ commit }, payload) {
            return axios.get(`/api/CueUserInfo/GetProgramList?personid=` + payload)
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
                    commit('SET_USERPROLIST', products)
                    commit('SET_MEDIASOPTION', medias)
                })
        },
        //프로그램당 전체 유저 목록 가져오기
        getProUserList({ commit }, payload) {
            axios.get(`/api/CueUserInfo/GetDirectorList?productid=` + payload)
                .then((res) => {
                    commit('SET_PROUSERLIST', res.data.directorName)
                })
        }
    }
}