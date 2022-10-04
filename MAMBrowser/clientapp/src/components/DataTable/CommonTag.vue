<template class="common-tag" >
  <div class="tag_col">
    <b-badge
      v-for="item in rowData.tag"
      :key="SetItems(item)"
      href="#"
      class="tag_badge mr-1 mb-1"
      variant="dark"
      @click.native="OnBadgeClick(item)"
      ># {{ SetItems(item) }}</b-badge
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
    SetItems(item) {
      return item.text ? item.text : item;
    },
    OnBadgeClick(item) {
      this.$emit("tagItemFromCommonTag", this.SetItems(item));
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