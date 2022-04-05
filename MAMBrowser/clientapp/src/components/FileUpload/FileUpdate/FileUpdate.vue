<template>
  <common-update-modal @close="MetaModalOff">
    <h3 slot="header">메타 데이터 수정</h3>

    <h4 v-if="updateScreenName == 'private'" slot="body" class="h320">
      <my-disk-update
        :rowData="rowData"
        @updateMyDiskMeta="getUpdateMeta"
      ></my-disk-update>
    </h4>

    <h4 v-if="updateScreenName == 'program'" slot="body" class="h240">
      <program-update
        :rowData="rowData"
        @updateProgramMeta="getUpdateMeta"
      ></program-update>
    </h4>

    <h4 v-if="updateScreenName == 'pro'" slot="body" class="h500">
      <pro-update
        :rowData="rowData"
        @updateProMeta="getUpdateMeta"
      ></pro-update>
    </h4>

    <h4 v-if="updateScreenName == 'mcr-spot'" slot="body" class="h240">
      <mcr-spot-update
        :rowData="rowData"
        @updateMcrSpotMeta="getUpdateMeta"
      ></mcr-spot-update>
    </h4>

    <h4 v-if="updateScreenName == 'scr-spot'" slot="body" class="h240">
      <scr-spot-update
        :rowData="rowData"
        @updateScrSpotMeta="getUpdateMeta"
      ></scr-spot-update>
    </h4>

    <h4 v-if="updateScreenName == 'report'" slot="body" class="h500">
      <report-update
        :rowData="rowData"
        @updateReportMeta="getUpdateMeta"
      ></report-update>
    </h4>

    <h4 v-if="updateScreenName == 'filler-time'" slot="body" class="h240">
      <filler-time-update
        :rowData="rowData"
        @updateFillerTimeMeta="getUpdateMeta"
      ></filler-time-update>
    </h4>

    <h4 v-if="updateScreenName == 'filler'" slot="body" class="h500">
      <filler-update
        :rowData="rowData"
        @updateFillerMeta="getUpdateMeta"
        :fillerType="fillerType"
      ></filler-update>
    </h4>

    <h3 slot="footer">
      <b-button
        variant="outline-primary"
        @click="updateFile"
        style="margin-right: 10px"
      >
        <span class="label">수정</span>
      </b-button>

      <b-button
        variant="outline-danger"
        @click="MetaModalOff"
        style="margin-right: 10px"
      >
        <span class="label">닫기</span>
      </b-button>
    </h3>
  </common-update-modal>
</template>

<script>
import CommonUpdateModal from "../../Modal/CommonUpdateModal.vue";
import MyDiskUpdate from "./updateComponent/myDiskUpdate.vue";
import ProgramUpdate from "./updateComponent/programUpdate.vue";
import ProUpdate from "./updateComponent/proUpdate.vue";
import McrSpotUpdate from "./updateComponent/mcrSpotUpdate.vue";
import ScrSpotUpdate from "./updateComponent/scrSpotUpdate.vue";
import ReportUpdate from "./updateComponent/reportUpdate.vue";
import FillerTimeUpdate from "./updateComponent/fillerTimeUpdate.vue";
import FillerUpdate from "./updateComponent/fillerUpdate.vue";
export default {
  components: {
    CommonUpdateModal,
    MyDiskUpdate,
    ProgramUpdate,
    ProUpdate,
    McrSpotUpdate,
    ScrSpotUpdate,
    ReportUpdate,
    FillerTimeUpdate,
    FillerUpdate,
  },
  props: {
    rowData: {
      type: [],
      default: "",
    },
    updateScreenName: {
      type: String,
      default: "",
    },
    fillerType: {
      type: String,
      default: "",
    },
  },
  data() {
    return {
      updateMeta: {},
    };
  },
  methods: {
    MetaModalOff() {
      this.$emit("UpdateModalClose");
    },
    updateFile() {
      this.$emit("updateFile", this.updateMeta);
    },
    getUpdateMeta(v) {
      this.updateMeta = v;
    },
  },
};
</script>

<style>
.h240 {
  padding-left: 20px;
  padding-top: 20px;
  margin-bottom: -20px;
  width: 400px;
  height: 240px;
  background-color: white;
}
.h320 {
  padding-left: 20px;
  padding-top: 20px;
  margin-bottom: -20px;
  width: 400px;
  height: 320px;
  background-color: white;
}
.h500 {
  padding-left: 20px;
  padding-top: 20px;
  margin-bottom: -20px;
  width: 400px;
  height: 500px;
  background-color: white;
}
</style>
