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
      :maxLength="30"
      aria-describedby="input-live-help input-live-feedback"
      placeholder="메모"
      trim
    />
    <p
      v-if="this.memo != null"
      style="font-size: 14px; text-align: right; margin-right: 35px"
    >
      {{ this.memo.length }}/30
    </p>
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
      memo: this.rowData.memo,
    };
  },
  created() {
    this.update();
  },
  methods: {
    changeMemo(v) {
      this.memo = v;
      this.update();
    },
    update() {
      var meta = {
        ID: this.rowData.id,
        memo: this.memo,
      };
      this.$emit("updateProgramMeta", meta);
    },
  },
};
</script>

<style></style>
