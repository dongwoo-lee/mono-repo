<template>
  <div>
    <h6>소재명</h6>
    <b-form-input
      class="editTask"
      :value="rowData.name"
      @input="changeName"
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
      memo: this.rowData.memo,
    };
  },
  created() {
    this.update();
  },
  methods: {
    changeName(v) {
      this.name = v;
      this.update();
    },
    changeMemo(v) {
      this.memo = v;
      this.update();
    },
    update() {
      var meta = {
        ID: this.rowData.id,
        name: this.name,
        memo: this.memo,
      };
      this.$emit("updateScrSpotMeta", meta);
    },
  },
};
</script>

<style></style>
