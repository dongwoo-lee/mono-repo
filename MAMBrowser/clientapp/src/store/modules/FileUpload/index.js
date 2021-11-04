import getters from "./getters.js";
import mutations from "./mutations.js";
import actions from "./actions.js";

export default {
  namespaced: true,
  state: {
    MetaModalTitle: "",
    localFiles: [],
    uploaderCustomData: {},
    type: "null",
    MetaData: {
      title: "",
      memo: "",
      mediaCD: "",
      categoryCD: "",
      typeSelected: "null",
      mediaSelected: "A",
      duration: "",
      audioFormat: ""
    },
    connectionId: "",
    vueTableData: [],
    ProgramData: [
      {
        eventName: "",
        eventType: "",
        productId: "",
        onairTime: "",
        durationSec: ""
      }
    ],
    FileUploadProgress: {}
  },
  getters,
  mutations,
  actions
};
