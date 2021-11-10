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
      editor: "",
      mediaCD: "",
      categoryCD: "",
      typeSelected: "null",
      proMediaSelected: "A",
      mcrMediaSelected: "A",
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
    EventData: [
      {
        name: "",
        id: ""
      }
    ],
    FileUploadProgress: {}
  },
  getters,
  mutations,
  actions
};
