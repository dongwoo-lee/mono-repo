<template>
  <div>
    <h6>제목</h6>
    <b-form-input
      class="editTask"
      :value="rowData.title"
      @input="changeTitle"
      :maxLength="200"
      aria-describedby="input-live-help input-live-feedback"
      placeholder="제목"
      trim
    />
    <p
      v-if="this.title != null"
      style="font-size: 14px; text-align: right; margin-right: 35px"
    >
      {{ this.title.length }}/200
    </p>

    <h6>메모</h6>
    <b-form-input
      class="editTask"
      :value="rowData.memo"
      @input="changeMemo"
      :maxLength="200"
      aria-describedby="input-live-help input-live-feedback"
      placeholder="메모"
      trim
    />
    <p
      v-if="this.memo != null"
      style="font-size: 14px; text-align: right; margin-right: 35px"
    >
      {{ this.memo.length }}/200
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
      title: this.rowData.title,
      memo: this.rowData.memo,
    };
  },
  created() {
    this.update();
  },
  methods: {
    changeTitle(v) {
      this.title = v;
      this.update();
    },
    changeMemo(v) {
      this.memo = v;
      this.update();
    },
    update() {
      var meta = {
        ID: this.rowData.id,
        title: this.title,
        memo: this.memo,
      };
      this.$emit("updateMyDiskMeta", meta);
    },
  },
};
</script>

<style></style>
