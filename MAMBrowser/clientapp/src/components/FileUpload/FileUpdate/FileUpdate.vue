<template>
  <common-update-modal @close="MetaModalOff">
    <h3 slot="header">메타 데이터 수정</h3>
    <h4
      v-if="updateScreenName == 'private'"
      slot="body"
      style="
        padding-left: 20px;
        padding-top: 20px;
        margin-bottom: -20px;
        width: 400px;
        height: 240px;
        background-color: white;
      "
    >
      <my-disk-update
        :rowData="rowData"
        @updateMyDiskMeta="getMyDiskMeta"
      ></my-disk-update>
    </h4>
    <h4
      v-if="updateScreenName == 'scr-spot'"
      slot="body"
      style="
        padding-left: 20px;
        padding-top: 20px;
        margin-bottom: -20px;
        width: 400px;
        height: 240px;
        background-color: white;
      "
    >
      <scr-spot-update
        :rowData="rowData"
        @updateScrSpotMeta="getScrSpotMeta"
      ></scr-spot-update>
    </h4>
    <h3 slot="footer">
      <b-button
        variant="outline-primary"
        @click="updateFile"
        style="margin-left: 45px"
      >
        <span class="label">수정</span>
      </b-button>
    </h3>
  </common-update-modal>
</template>

<script>
import CommonUpdateModal from "../../Modal/CommonUpdateModal.vue";
import MyDiskUpdate from "./updateComponent/myDiskUpdate.vue";
import ScrSpotUpdate from "./updateComponent/scrSpotUpdate.vue";
export default {
  components: {
    CommonUpdateModal,
    MyDiskUpdate,
    ScrSpotUpdate,
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
    getMyDiskMeta(v) {
      this.updateMeta = v;
    },
    getScrSpotMeta(v) {
      this.updateMeta = v;
    },
  },
};
</script>

<style></style>
