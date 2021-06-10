//import axios from 'axios'

export default {
    namespaced: true,
    state: {
        cuesheetSchedule: [
            { broadcastDate: "2021-06-10(목)", broadcastTime: "18:05", programName: "김이나의 별이 빛나는 밤에", responsibility: "김은비", state: "작성", channel: "FM4U" },
            { broadcastDate: "2021-06-10(목)", broadcastTime: "14:05", programName: "두시의 데이트 뮤지, 안영미입니다", responsibility: "김은비", state: "수정", channel: "표준FM" },
            { broadcastDate: "2021-06-11(금)", broadcastTime: "18:05", programName: "김이나의 별이 빛나는 밤에", responsibility: "김은비", state: "수정", channel: "FM4U" },
            { broadcastDate: "2021-06-11(금)", broadcastTime: "14:05", programName: "두시의 데이트 뮤지, 안영미입니다", responsibility: "김은비", state: "수정", channel: "표준FM" },
            { broadcastDate: "2021-06-12(토)", broadcastTime: "18:05", programName: "김이나의 별이 빛나는 밤에", responsibility: "김은비", state: "수정", channel: "FM4U" },
            { broadcastDate: "2021-06-12(토)", broadcastTime: "14:05", programName: "두시의 데이트 뮤지, 안영미입니다", responsibility: "김은비", state: "작성", channel: "표준FM" },
            { broadcastDate: "2021-06-13(일)", broadcastTime: "18:05", programName: "김이나의 별이 빛나는 밤에", responsibility: "김은비", state: "작성", channel: "FM4U" },
            { broadcastDate: "2021-06-13(일)", broadcastTime: "14:05", programName: "두시의 데이트 뮤지, 안영미입니다", responsibility: "김은비", state: "작성", channel: "표준FM" }
        ],
        cuesheetSchedule_d: [{ channel: "FM4U", programName: "김이나의 별이 빛나는 밤에", updateDate: "2021-05-13 22:15", weekData: "tue thu fri sat sun" }, { channel: "FM4U", programName: "김이나의 별이 빛나는 밤에", updateDate: "2021-05-13 08:52", weekData: "mon wed " }],
        templateList: [{ title: "이름없는 템플릿", addDate: "2021-05-15(토)", updateDate: "2021-05-15(토)" }, { title: "신규 및 개편 프로그램 작성 중", addDate: "2021-05-16(일)", updateDate: "2021-05-16(일)" }],
        cuesheet: [
            { broadcastDate: "2021-05-13(목)", broadcastTime: "18:05", programName: "김이나의 별이 빛나는 밤에", responsibility: "김은비", channel: "FM4U", broadcastStatus: "Y" },
            { broadcastDate: "2021-05-13(목)", broadcastTime: "14:05", programName: "두시의 데이트 뮤지, 안영미입니다", responsibility: "김은비", channel: "표준FM", broadcastStatus: "Y" },
            { broadcastDate: "2021-05-14(금)", broadcastTime: "18:05", programName: "김이나의 별이 빛나는 밤에", responsibility: "김은비", channel: "FM4U", broadcastStatus: "Y" },
            { broadcastDate: "2021-05-14(금)", broadcastTime: "14:05", programName: "두시의 데이트 뮤지, 안영미입니다", responsibility: "김은비", channel: "표준FM", broadcastStatus: "Y" },
            { broadcastDate: "2021-05-15(토)", broadcastTime: "18:05", programName: "김이나의 별이 빛나는 밤에", responsibility: "김은비", channel: "FM4U", broadcastStatus: "Y" },
            { broadcastDate: "2021-05-15(토)", broadcastTime: "14:05", programName: "두시의 데이트 뮤지, 안영미입니다", responsibility: "김은비", channel: "표준FM", broadcastStatus: "Y" },
            { broadcastDate: "2021-05-16(일)", broadcastTime: "18:05", programName: "김이나의 별이 빛나는 밤에", responsibility: "김은비", channel: "FM4U", broadcastStatus: "Y" },
            { broadcastDate: "2021-05-16(일)", broadcastTime: "14:05", programName: "두시의 데이트 뮤지, 안영미입니다", responsibility: "김은비", channel: "표준FM", broadcastStatus: "Y" }
        ],
    },













    getters: {
        cuesheetSchedule: state => state.cuesheetSchedule,
        cuesheetSchedule_d: state => state.cuesheetSchedule_d,
        templateList: state => state.templateList,
        cuesheet: state => state.cuesheet,
    },
    mutations: {

    },
    actions: {
    }
}