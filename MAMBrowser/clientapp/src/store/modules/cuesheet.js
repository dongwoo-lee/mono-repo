//import axios from 'axios'

export default {
    namespaced: true,
    state: {
        cuesheetSchedule: [
            { broadcastDate: "2021-05-13(목)", broadcastTime: "18:05", programName: "김이나의 별이 빛나는 밤에", responsibility: "김은비", state: "작성", channel: "FM4U" },
            { broadcastDate: "2021-05-13(목)", broadcastTime: "14:05", programName: "두시의 데이트 뮤지, 안영미입니다", responsibility: "김은비", state: "수정", channel: "표준FM" },
            { broadcastDate: "2021-05-14(금)", broadcastTime: "18:05", programName: "김이나의 별이 빛나는 밤에", responsibility: "김은비", state: "작성", channel: "FM4U" },
            { broadcastDate: "2021-05-14(금)", broadcastTime: "14:05", programName: "두시의 데이트 뮤지, 안영미입니다", responsibility: "김은비", state: "작성", channel: "표준FM" },
            { broadcastDate: "2021-05-15(토)", broadcastTime: "18:05", programName: "김이나의 별이 빛나는 밤에", responsibility: "김은비", state: "작성", channel: "FM4U" },
            { broadcastDate: "2021-05-15(토)", broadcastTime: "14:05", programName: "두시의 데이트 뮤지, 안영미입니다", responsibility: "김은비", state: "작성", channel: "표준FM" },
            { broadcastDate: "2021-05-16(일)", broadcastTime: "18:05", programName: "김이나의 별이 빛나는 밤에", responsibility: "김은비", state: "작성", channel: "FM4U" },
            { broadcastDate: "2021-05-16(일)", broadcastTime: "14:05", programName: "두시의 데이트 뮤지, 안영미입니다", responsibility: "김은비", state: "작성", channel: "표준FM" }
        ],
    },
    getters: {
        cuesheetSchedule: state => state.cuesheetSchedule,
    },
    mutations: {

    },
    actions: {
    }
}