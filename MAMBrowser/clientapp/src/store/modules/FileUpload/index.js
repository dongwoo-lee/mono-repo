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
    fileMediaOptions: [],
    MetaModalTitle: "",
    localFiles: [],
    uploaderCustomData: {},
    date: "",
    MetaData: {
      title: "",
      memo: "",
      editor: "",
      usage: "",
      advertiser: "",
      mediaCD: "",
      categoryCD: "",
      typeSelected: "null",
      mediaSelected: "A",
      duration: "",
      audioFormat: ""
    },
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
    ProgramSelected: {
      eventName: "",
      eventType: "",
      productId: "",
      onairTime: "",
      durationSec: ""
    },
    programState: false,
    EventData: [
      {
        name: "",
        id: ""
      }
    ],
    EventSelected: {
      id: "",
      name: ""
    },
    FileUploadProgress: {}
  },
  getters,
  mutations,
  actions
};
