<template>
  <common-update-modal @close="MetaModalOff">
    <h3 slot="header">메타 데이터 수정</h3>
    <h4 slot="body">
      <h6>
        제목
      </h6>
      <b-form-input
        class="editTask"
        :value="rowData.title"
        @input="changeTitle"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="제목"
        trim
      />
      <h6>
        메모
      </h6>
      <b-form-input
        v-if="screenName == 'private'"
        class="editTask"
        :value="rowData.memo"
        @input="changeMemo"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="메모"
        trim
      />
      <b-form-input
        v-if="screenName == 'scr-spot'"
        class="editTask"
        v-model="rowData.advertiser"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="메모"
        trim
      />
      <b-form-input
        v-if="screenName == 'report'"
        class="editTask"
        v-model="rowData.reporterName"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="메모"
        trim
      />
    </h4>
    <h3 slot="footer">
      <b-button
        variant="outline-primary"
        @click="updateFile"
        style="margin-left:45px;"
      >
        <span class="label">업로드</span>
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
      default: ""
    },
    screenName: {
      type: String,
      default: ""
    }
  },
  data() {
    return {
      title: "",
      memo: ""
    };
  },
  components: {
    CommonUpdateModal
  },
  methods: {
    MetaModalOff() {
      this.$emit("UpdateModalClose");
    },
    updateFile() {
      var update = {
        title: this.title,
        memo: this.memo
      };
      this.$emit("updateFile", update);
    },
    changeTitle(v) {
      this.title = v;
    },
    changeMemo(v) {
      this.memo = v;
    }
  }
};
</script>

<style></style>
