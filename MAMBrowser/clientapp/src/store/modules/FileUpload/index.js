import getters from "./getters.js";
import mutations from "./mutations.js";
import actions from "./actions.js";

export default {
  namespaced: true,
  state: {
    MetaModalTitle: "",
    localFiles: [],
    uploaderCustomData: {},
    MetaData: {
      title: "",
      memo: "",
      mediaCD: "",
      categoryCD: "",
      typeSelected: "null",
      mediaSelected: "a",
      duration: "",
      audioFormat: ""
    },
    connectionId: "",
    vueTableData: [],
    ProgramData: [
      {
        EventName: "건강한 아침",
        EventType: "N",
        ProductId: "PM0505NA",
        OnairTime: new Date(),
        SourceID: "PM202110281615NA",
        Duration: "00:53:06"
      },
      {
        EventName: "건강한 점심",
        EventType: "N",
        ProductId: "PM0505NB",
        OnairTime: new Date(),
        SourceID: "PM202110281615NA",
        Duration: "00:53:06"
      },
      {
        EventName: "건강한 저녁",
        EventType: "N",
        ProductId: "PM0505NC",
        OnairTime: new Date(),
        SourceID: "PM202110281615NA",
        Duration: "00:53:06"
      },
      {
        EventName: "건강한 어제",
        EventType: "N",
        ProductId: "PM0505ND",
        OnairTime: new Date(),
        SourceID: "PM202110281615NA",
        Duration: "00:53:06"
      },
      {
        EventName: "건강한 내일",
        EventType: "N",
        ProductId: "PM0505NE",
        OnairTime: new Date(),
        SourceID: "PM202110281615NA",
        Duration: "00:53:06"
      },
      {
        EventName: "건강한 오늘",
        EventType: "N",
        ProductId: "PM0505NF",
        OnairTime: new Date(),
        SourceID: "PM202110281615NA",
        Duration: "00:53:06"
      },
      {
        EventName: "건강한",
        EventType: "N",
        ProductId: "PM0505NG",
        OnairTime: new Date(),
        SourceID: "PM202110281615NA",
        Duration: "00:53:06"
      }
    ],
    FileUploadProgress: {}
  },
  getters,
  mutations,
  actions
};
