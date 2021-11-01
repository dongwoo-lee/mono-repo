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
  metaValid(state) {
    if (
      state.MetaData.typeSelected != "null" &&
      state.MetaData.title.length >= 1 &&
      state.MetaData.memo.length >= 1
    ) {
      return true;
    } else return false;
  }
};
