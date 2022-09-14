<template class="common-tag" >
  <div class="tag_col">
    <b-badge
      v-for="item in rowData.tag"
      :key="item"
      href="#"
      class="tag_badge mr-1 mb-1"
      variant="dark"
      @click.native="OnBadgeClick(item)"
      ># {{ item }}</b-badge
    >
  </div>
</template>
<script>
export default {
  props: {
    rowData: {
      type: Object,
      default: () => {},
    },
  },
  data() {
    return {};
  },
  methods: {
    OnBadgeClick(item) {
      this.$emit("tagItemFromCommonTag", item);
    },
  },
};
</script>

<style>
.tag_col .badge {
  font-size: 95%;
  font-weight: 100;
  background: #6c757d;
}
</style>