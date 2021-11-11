import getters from "./getters.js";
import mutations from "./mutations.js";
import actions from "./actions.js";

export default {
  namespaced: true,
  state: {
    processing: false,
    fileUploading: false,
    isActive: false,
    typeOptions: [{ value: "null", text: "소재 유형" }],
    MetaModalTitle: "",
    localFiles: [],
    uploaderCustomData: {},
    MetaData: {
      title: "",
      memo: "",
      editor: "",
      advertiser: "",
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
    programState: false,
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
