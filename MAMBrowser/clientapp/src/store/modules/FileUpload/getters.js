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
  editorState(state) {
    return state.MetaData.editor.length >= 1 ? true : false;
  },
  programState(state) {
    return state.programState ? true : false;
  },
  eventState(state) {
    return state.EventData.id != "" ? true : false;
  },
  metaValid(state, getters) {
    if (state.MetaData.typeSelected == "my-disk") {
      if (getters.typeState && getters.titleState && getters.memoState)
        return true;
    } else if (state.MetaData.typeSelected == "program") {
      if (getters.memoState && getters.editorState && getters.programState) {
        return true;
      }
    } else if (state.MetaData.typeSelected == "mcr-spot") {
      if (getters.memoState && getters.editorState && getters.eventState) {
        return true;
      }
    }
    return false;
  },
  getBadge(state) {
    return state.vueTableData.length;
  }
};
