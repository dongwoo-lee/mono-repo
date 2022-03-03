<template>
  <div>
    <h6>소재명</h6>
    <b-form-input
      class="editTask"
      :value="rowData.name"
      @input="changeName"
      :maxLength="30"
      aria-describedby="input-live-help input-live-feedback"
      placeholder="소재명"
      trim
    />
    <p
      v-if="this.name != null"
      style="font-size: 14px; text-align: right; margin-right: 35px"
    >
      {{ this.name.length }}/30
    </p>
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
