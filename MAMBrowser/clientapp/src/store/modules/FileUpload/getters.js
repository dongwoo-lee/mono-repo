export default {
  typeState(state) {
    return state.MetaData.typeSelected != "null" ? true : false;
  },
  dateState(state) {
    return state.date.length == 10 ? true : false;
  },
  SEDateState(state) {
    return state.fileSDate.length == 10 && state.fileEDate.length == 10
      ? true
      : false;
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
  reporterState(state) {
    return state.MetaData.reporter.length >= 1 ? true : false;
  },
  usageState(state) {
    return state.MetaData.usage.length >= 1 ? true : false;
  },
  advertiserState(state) {
    return state.MetaData.advertiser.length >= 1 ? true : false;
  },
  programState(state) {
    return state.programState ? true : false;
  },
  eventState(state) {
    return state.EventSelected.id != "" ? true : false;
  },
  metaValid(state, getters) {
    // TODO: dateState 추가
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
    } else if (state.MetaData.typeSelected == "scr-spot") {
      if (
        getters.titleState &&
        getters.memoState &&
        getters.usageState &&
        getters.advertiserState &&
        getters.editorState
      ) {
        return true;
      }
    } else if (state.MetaData.typeSelected == "static-spot") {
      if (
        getters.memoState &&
        getters.eventState &&
        getters.advertiserState &&
        getters.editorState &&
        getters.SEDateState
      ) {
        return true;
      }
    } else if (state.MetaData.typeSelected == "var-spot") {
      if (
        getters.memoState &&
        getters.eventState &&
        getters.advertiserState &&
        getters.editorState &&
        getters.SEDateState
      ) {
        return true;
      }
    } else if (state.MetaData.typeSelected == "report") {
      if (
        getters.memoState &&
        getters.eventState &&
        getters.reporterState &&
        getters.editorState
      ) {
        return true;
      }
    } else if (state.MetaData.typeSelected == "filler") {
      if (
        getters.titleState &&
        getters.memoState &&
        getters.editorState &&
        getters.dateState
      ) {
        return true;
      }
    }
    return false;
  },
  getBadge(state) {
    return state.vueTableData.length;
  }
};
