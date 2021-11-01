export default {
  typeState(state) {
    return state.MetaData.typeSelected != "null" ? true : false;
  },
  titleState(state) {
    return state.MetaData.title.length >= 1 ? true : false;
  },
  memoState(state) {
    return state.MetaData.memo.length >= 1 ? true : false;
  },
  metaValid(state, getters) {
    if (getters.typeState && getters.titleState && getters.memoState) {
      return true;
    } else return false;
  },
  getBadge(state) {
    return state.vueTableData.length;
  }
};
