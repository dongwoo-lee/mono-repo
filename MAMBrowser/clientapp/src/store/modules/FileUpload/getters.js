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
  proMediaState(state) {
    return state.MetaData.mediaSelected != null &&
      state.MetaData.mediaSelected != ""
      ? true
      : false;
  },
  titleState(state) {
    return state.MetaData.title.length >= 1 ? true : false;
  },
  memoState(state) {
    return state.MetaData.memo.length >= 1 ? true : false;
  },
  reporterState(state) {
    return state.MetaData.reporter.length >= 1 ? true : false;
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
  durationState(state) {
    var dh = state.MetaData.duration.slice(0, 2);
    var dm = state.MetaData.duration.slice(3, 5);
    var ds = state.MetaData.duration.slice(6, 8);
    var calcD = dh * 60 * 60 + dm * 60 + ds * 1;

    if (
      state.MetaData.typeSelected == "program" &&
      state.ProgramSelected.durationSec != ""
    ) {
      var ph = state.ProgramSelected.durationSec.slice(0, 2);
      var pm = state.ProgramSelected.durationSec.slice(3, 5);
      var ps = state.ProgramSelected.durationSec.slice(6, 8);
      var calcP = ph * 60 * 60 + pm * 60 + ps * 1;
      if (5 < calcD - calcP) {
        return false;
      }
    } else if (
      state.MetaData.typeSelected == "mcr-spot" &&
      state.EventSelected.id != ""
    ) {
      if (state.EventSelected.duration == null) {
        return true;
      }
      var eh = state.EventSelected.duration.slice(0, 2);
      var em = state.EventSelected.duration.slice(3, 5);
      var es = state.EventSelected.duration.slice(6, 8);
      var calcE = eh * 60 * 60 + em * 60 + es * 1;
      if (5 < calcD - calcE) {
        return false;
      }
    } else if (
      state.MetaData.typeSelected == "static-spot" &&
      state.EventSelected.id != ""
    ) {
      if (state.EventSelected.duration == null) {
        return true;
      }
      var eh = state.EventSelected.duration.slice(0, 2);
      var em = state.EventSelected.duration.slice(3, 5);
      var es = state.EventSelected.duration.slice(6, 8);
      var calcE = eh * 60 * 60 + em * 60 + es * 1;
      if (1 < calcD - calcE) {
        return false;
      }
    } else if (
      state.MetaData.typeSelected == "var-spot" &&
      state.EventSelected.id != ""
    ) {
      if (state.EventSelected.duration == null) {
        return true;
      }
      var eh = state.EventSelected.duration.slice(0, 2);
      var em = state.EventSelected.duration.slice(3, 5);
      var es = state.EventSelected.duration.slice(6, 8);
      var calcE = eh * 60 * 60 + em * 60 + es * 1;
      if (1 < calcD - calcE) {
        return false;
      }
    }
    return true;
  },
  metaValid(state, getters) {
    // TODO: dateState 추가
    if (state.MetaData.typeSelected == "my-disk") {
      if (getters.typeState && getters.titleState && getters.memoState)
        return true;
    } else if (state.MetaData.typeSelected == "program") {
      if (getters.memoState && getters.programState) {
        return true;
      }
    } else if (state.MetaData.typeSelected == "pro") {
      if (getters.memoState && getters.titleState && getters.proMediaState) {
        return true;
      }
    } else if (state.MetaData.typeSelected == "mcr-spot") {
      if (getters.memoState && getters.eventState) {
        return true;
      }
    } else if (state.MetaData.typeSelected == "scr-spot") {
      if (
        getters.titleState &&
        getters.memoState &&
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
        getters.SEDateState
      ) {
        return true;
      }
    } else if (state.MetaData.typeSelected == "var-spot") {
      if (
        getters.memoState &&
        getters.eventState &&
        getters.advertiserState &&
        getters.SEDateState
      ) {
        return true;
      }
    } else if (state.MetaData.typeSelected == "report") {
      if (getters.memoState && getters.eventState) {
        return true;
      }
    } else if (state.MetaData.typeSelected == "filler") {
      if (getters.titleState && getters.memoState && getters.dateState) {
        return true;
      }
    }
    return false;
  },
  getBadge(state) {
    return state.masteringListData.length;
  },
};
