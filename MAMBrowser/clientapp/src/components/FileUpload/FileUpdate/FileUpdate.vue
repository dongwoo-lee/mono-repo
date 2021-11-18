<template>
  <common-update-modal @close="MetaModalOff">
    <h3 slot="header">메타 데이터 수정</h3>
    <h4 slot="body" style="margin-left: 20px; margin-top: 20px">
      <div v-if="rowData.name == null">
        <h6>제목</h6>
        <b-form-input
          class="editTask"
          :value="rowData.title"
          @input="changeTitle"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="제목"
          trim
        />
      </div>
      <div v-if="rowData.title == null">
        <h6>소재명</h6>
        <b-form-input
          class="editTask"
          :value="rowData.name"
          @input="changeName"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="소재명"
          trim
        />
      </div>
      <div v-if="updateScreenName == 'private'">
        <h6 style="margin-top: 20px">메모</h6>
        <b-form-input
          class="editTask"
          :value="rowData.memo"
          @input="changeMemo"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="메모"
          trim
        />
      </div>
      <!-- <div v-if="updateScreenName == 'scr-spot'">
        <h6 style="margin-top:20px;">
          광고주
        </h6>
        <b-form-input
          class="editTask"
          :value="rowData.advertiser"
          @input="changeAdvertiser"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="광고주"
          trim
        />
      </div> -->
      <div v-if="updateScreenName == 'report'">
        <h6 style="margin-top: 20px">취재인</h6>
        <b-form-input
          class="editTask"
          :value="rowData.reporter"
          @input="changeReporter"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="취재인"
          trim
        />
      </div>
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
export default {
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
      title: this.rowData.title,
      fileName: this.rowData.name,
      memo: this.rowData.memo,
      reporter: this.rowData.reporter,
      // advertiser: this.rowData.advertiser
    };
  },
  components: {
    CommonUpdateModal,
  },
  methods: {
    MetaModalOff() {
      this.$emit("UpdateModalClose");
    },
    updateFile() {
      if (this.updateScreenName == "private") {
        var update = {
          title: this.title,
          memo: this.memo,
        };
      } else if (this.updateScreenName == "report") {
        var update = {
          fileName: this.fileName,
          reporter: this.reporter,
        };
      } else if (this.updateScreenName == "pr") {
        var update = {
          fileName: this.fileName,
        };
      } else if (this.updateScreenName == "pro") {
        var update = {
          fileName: this.fileName,
        };
      }
      // else if (this.updateScreenName == "scr-spot") {
      //   var update = {
      //     fileName: this.fileName
      //     // advertiser: this.advertiser
      //   };
      // }
      this.$emit("updateFile", update);
    },
    changeName(v) {
      this.fileName = v;
    },
    changeTitle(v) {
      this.title = v;
    },
    changeReporter(v) {
      this.reporter = v;
    },
    // changeAdvertiser(v) {
    //   this.advertiser = v;
    // },
    changeMemo(v) {
      this.memo = v;
    },
  },
};
</script>

<style></style>
