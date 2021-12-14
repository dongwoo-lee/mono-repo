<template>
  <div>
    <h6>제목</h6>
    <b-form-input
      class="editTask"
      :value="rowData.title"
      disabled
      aria-describedby="input-live-help input-live-feedback"
      placeholder="제목"
      trim
    />

    <br />
    <h6>메모</h6>
    <b-form-input
      class="editTask"
      :value="rowData.memo"
      @input="changeMemo"
      aria-describedby="input-live-help input-live-feedback"
      placeholder="메모"
      trim
    />
  </div>
</template>

<script>
export default {
  props: {
    rowData: {
      type: [],
      default: "",
    },
  },
  data() {
    return {
      memo: "",
    };
  },
  methods: {
    changeMemo(v) {
      this.memo = v;
      this.update();
    },
    update() {
      var meta = {
        title: this.rowData.title,
        memo: this.memo,
      };
      this.$emit("updateMyDiskMeta", meta);
    },
  },
};
</script>

<style></style>
