<template>
  <div>
    <h6>프로그램 명</h6>
    <b-form-input
      class="editTask"
      :value="rowData.name"
      disabled
      aria-describedby="input-live-help input-live-feedback"
      placeholder="프로그램명"
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
  created() {
    console.log(this.rowData);
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
      this.$emit("updateProgramMeta", meta);
    },
  },
};
</script>

<style></style>
