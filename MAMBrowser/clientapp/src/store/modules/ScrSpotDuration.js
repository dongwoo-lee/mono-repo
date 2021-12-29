import axios from "axios";
import $fn from "../../utils/CommonFunctions";

export default {
  namespaced: true,
  state: {
    Duration: false,
    Add: false,
    Search: false,
    selectedSpot: {
      spotID: "",
      spotName: "",
      codeID: "",
      codeName: "",
      CMOwner: "",
    },
    setScrRangeData: {
      SpotID: "",
      spotName: "",
      ProductID: null,
      StartDate: $fn.formatDate(new Date(), "yyyy-MM-dd"),
      EndDate: $fn.formatDate(new Date(), "yyyy-MM-dd"),
    },
    requestScr: [],
    selectedScr: "",
  },
  getters: {
    scrValid(state) {
      if (
        state.setScrRangeData.SpotID != "" &&
        state.setScrRangeData.ProductID != null &&
        state.setScrRangeData.StartDate != "" &&
        state.setScrRangeData.EndDate != ""
      ) {
        return true;
      }
      return false;
    },
  },
  mutations: {
    showDuration(state) {
      state.Duration = true;
    },
    hideDuration(state) {
      state.Duration = false;
      state.requestScr = [];
    },
    showAdd(state) {
      state.Add = true;
    },
    hideAdd(state) {
      state.Add = false;
    },
    showSearch(state) {
      state.Search = true;
    },
    hideSearch(state) {
      state.Search = false;
    },
    setSelectedSpot(state, payload) {
      state.selectedSpot = payload;
      state.setScrRangeData.SpotID = payload.spotID;
      state.setScrRangeData.spotName = payload.spotName;
    },
    setScrRangeDataProductID(state, payload) {
      state.setScrRangeData.ProductID = payload;
    },
    setRequestScr(state, payload) {
      state.requestScr.push(payload);
    },
    setSelectedScr(state, payload) {
      state.selectedScr = payload;
    },
    deleteRequest(state) {
      state.requestScr.splice(state.selectedScr, 1);
    },
    resetScrRangeData(state) {
      state.selectedSpot = {
        spotID: "",
        spotName: "",
        codeID: "",
        codeName: "",
        CMOwner: "",
      };
      state.setScrRangeData = {
        SpotID: "",
        spotName: "",
        ProductID: null,
        StartDate: $fn.formatDate(new Date(), "yyyy-MM-dd"),
        EndDate: $fn.formatDate(new Date(), "yyyy-MM-dd"),
      };
    },
  },
  actions: {},
};
