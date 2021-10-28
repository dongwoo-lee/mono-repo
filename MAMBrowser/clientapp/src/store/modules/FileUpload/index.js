import getters from "./getters.js";
import mutations from "./mutations.js";
import actions from "./actions.js";

export default {
  namespaced: true,
  state: {
    MetaModal: false,
    MetaModalTitle: "",
    localFiles: [],
    uploaderCustomData: {},
    connectionId: "",
    vueTableData: [
      {
        fileName: "숀 (SHAUN) - Way Back Home",
        fileSize: 8858948,
        title: "건강한 아침 오프닝",
        memo: "12시까지 확인",
        step: 1
      },
      {
        fileName: "모모랜드 (MOMOLAND) - 뿜뿜",
        fileSize: 8858948,
        title: "생생 정보통 오프닝",
        memo: "오늘 칼퇴",
        step: 2
      },
      {
        fileName: "CHRISTOPHER - Monogamy (Lyrics)",
        fileSize: 8858948,
        title: "ddd",
        memo: "sss",
        step: 1
      }
    ]
  },
  getters,
  mutations,
  actions
};
