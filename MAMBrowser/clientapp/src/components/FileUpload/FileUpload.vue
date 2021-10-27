<template>
  <div id="file-container">
    <div
      id="dropzone-external"
      class="dropzone"
      v-show="dropzone"
      style="position: fixed; z-index: 9800; top: -80px; left: -80px; width: 2000px; height: 120%; background-color: rgba(0, 0, 0, 0.5); display: table;"
      :class="[
        isDropZoneActive
          ? 'dx-theme-accent-as-border-color dropzone-active'
          : 'dx-theme-border-color'
      ]"
    >
      <p
        style="margin-top:500px; margin-left:200px; text-align:center;color:white; font-size:48px;"
      >
        음원 파일 업로드
      </p>
      <div id="dropzone-text" class="flex-box" style="margin-top:80px;">
        <span
          style="position:absolute; z-index:1; color:white;  margin-top:700px;font-size:48px;"
        >
        </span>
      </div>
    </div>
    <b-button
      class="btn btn-outline-primary btn-sm default cutom-label mr-2"
      id="fileuploadbutton"
      @click="this.openFileModal"
      style="position:absolute; top:-80px; right:580px; z-index:1030; border-color:#008ECC; color:#008ECC; background-color:white;"
      >파일 업로드
    </b-button>

    <FileModal></FileModal>
    <MetaModal></MetaModal>
  </div>
</template>

<script>
import CommonFile from "./CommonFile";
import FileModal from "./FileModal";
import MetaModal from "./MetaModal";
import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
export default {
  components: {
    FileModal,
    MetaModal
  },
  mixins: [CommonFile],
  data() {
    return {};
  },
  methods: {
    ...mapMutations("FileStore", ["openFileModal"])
  }
};
</script>
<style>
@import "./FileUploadCSS.css";
</style>
<style scoped>
.progress__wrapper {
  margin: 0px !important;
}
.vuetable .thead .tr {
  height: 33px !important;
}
.vuetable-head-wrapper {
  height: 33px !important;
}
.card {
  height: 675px !important;
}
.date-input:focus {
  border: 1px solid #4475c4 !important;
}
.date-input {
  border: 1px solid #008ecc !important;
}
.media-select {
  border: 1px solid #008ecc !important;
}
.media-select:focus {
  border: 1px solid #4475c4 !important;
}
</style>
