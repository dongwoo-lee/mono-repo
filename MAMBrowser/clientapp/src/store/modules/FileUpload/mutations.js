export default {
  addLocalFiles(state, payload) {
    state.localFiles.push(payload);
  },
  setDate(state, payload) {
    state.date = payload;
  },
  setFileSDate(state, payload) {
    state.fileSDate = payload;
  },
  setFileEDate(state, payload) {
    state.fileEDate = payload;
  },
  setIsActive(state, payload) {
    state.isActive = payload;
  },
  setProcessing(state, payload) {
    state.processing = payload;
  },
  setFileUploading(state, payload) {
    state.fileUploading = payload;
  },
  setMetaModalTitle(state, payload) {
    state.MetaModalTitle = payload;
  },
  setUploaderCustomData(state, payload) {
    state.uploaderCustomData = payload;
  },
  setMasteringListData(state, payload) {
    state.masteringListData = payload;
  },
  setMasteringLogData(state, payload) {
    state.masteringLogData = payload;
  },
  setFileMediaOptions(state, payload) {
    state.fileMediaOptions.push(payload);
  },
  setMediaSelected(state, payload) {
    state.MetaData.mediaSelected = payload;
  },
  setDuration(state, payload) {
    state.MetaData.duration = payload;
  },
  setAudioFormat(state, payload) {
    state.MetaData.audioFormat = payload;
  },
  setProgramData(state, payload) {
    state.ProgramData = payload;
  },
  setProgramSelected(state, payload) {
    state.ProgramSelected = payload;
  },
  setEventSelected(state, payload) {
    state.EventSelected = payload;
  },
  setEventData(state, payload) {
    state.EventData = payload;
  },
  setEditor(state, payload) {
    state.MetaData.editor = payload;
  },
  setProgramState(state, payload) {
    state.programState = payload;
  },
  resetDate(state) {
    state.date = "";
  },
  resetFileSDate(state) {
    state.fileSDate = "";
  },
  resetFileEDate(state) {
    state.fileEDate = "";
  },
  resetLocalFiles(state) {
    state.localFiles = [];
  },
  resetTitle(state) {
    state.MetaData.title = "";
  },
  resetMemo(state) {
    state.MetaData.memo = "";
  },
  resetMediaSelected(state) {
    state.MetaData.mediaSelected = "";
  },
  resetEditor(state) {
    state.MetaData.editor = "";
  },
  resetReporter(state) {
    state.MetaData.reporter = "";
  },
  resetUsage(state) {
    state.MetaData.usage = "";
  },
  resetAdvertiser(state) {
    state.MetaData.advertiser = "";
  },
  resetType(state) {
    state.MetaData.typeSelected = "null";
  },
  resetFileMediaOptions(state) {
    state.fileMediaOptions = [];
  },
  resetProgramData(state) {
    state.ProgramData = [
      {
        eventName: "",
        eventType: "",
        productId: "",
        onairTime: "",
        durationSec: "",
      },
    ];
  },
  resetProgramSelected(state) {
    state.ProgramSelected = {
      eventName: "",
      eventType: "",
      productId: "",
      onairTime: "",
      durationSec: "",
    };
  },
  resetEventSelected(state) {
    state.EventSelected = {
      id: "",
      name: "",
    };
  },
  resetEventData(state) {
    state.EventData = [
      {
        name: "",
        id: "",
      },
    ];
  },
};
