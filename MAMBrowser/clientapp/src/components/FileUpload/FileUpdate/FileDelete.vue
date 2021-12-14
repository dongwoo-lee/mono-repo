<template>
  <common-update-modal @close="MetaModalOff">
    <h3 slot="header">파일 삭제</h3>
    <h4 slot="body" style="margin-left: 20px; margin-top: 20px">
      <h6>파일 명 : {{ rowData.name }}</h6>
      <br />
      <h6>삭제하시겠습니까?</h6>
    </h4>
    <h3 slot="footer">
      <b-button
        variant="outline-danger"
        @click="deleteFile"
        style="margin-left: 45px"
      >
        <span class="label">삭제</span>
      </b-button>
    </h3>
  </common-update-modal>
</template>

<script>
import CommonUpdateModal from "../../Modal/CommonUpdateModal.vue";
export default {
  props: {
    rowData: {
      type: [],
      default: "",
    },
  },
  data() {
    return {};
  },
  components: {
    CommonUpdateModal,
  },
  methods: {
    MetaModalOff() {
      this.$emit("DeleteModalClose");
    },
    deleteFile() {
      var deleteInfo = {
        deleteId: this.rowData.rowNO,
      };
      this.$emit("deleteFile", deleteInfo);
    },
  },
};
</script>

<style></style>
