export default {
  namespaced: true,
  state: {
    MetaModal: false,
    MetaModalTitle: "",
    // MetaData: {
    //   title: "",
    //   memo: "",
    //   type: "",
    //   mediaCD: "",
    //   categoryCD: ""
    // },
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
  getters: {},
  mutations: {
    MetaModalOn(state) {
      state.MetaModal = true;
    },
    MetaModalOff(state) {
      state.MetaModal = false;
      state.localFiles = [];
    },
    addLocalFiles(state, payload) {
      state.localFiles.push(payload);
    },
    resetLocalFiles(state) {
      state.localFiles = [];
    },
    setMetaModalTitle(state, payload) {
      state.MetaModalTitle = payload;
    },
    setConnectionId(state, payload) {
      state.connectionId = payload;
    },
    setUploaderCustomData(state, payload) {
      state.uploaderCustomData = payload;
    },
    forEachVueTableData(state, payload) {
      state.vueTableData.forEach(element => {
        if (payload.fileName == element.fileName) {
          element.step = payload.step;
        }
      });
    }
  },
  actions: {}
};
