<template>
  <div>
    <h6>소재명</h6>
    <b-form-input
      class="editTask"
      :value="rowData.name"
      disabled
      aria-describedby="input-live-help input-live-feedback"
      placeholder="소재명"
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
      name: this.rowData.name,
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
        memo: this.memo,
      };
      this.$emit("updateFillerTimeMeta", meta);
    },
  },
};
</script>

<style></style>
