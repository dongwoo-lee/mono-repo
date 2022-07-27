<template>
  <div>
    <DxTagBox :items="dataList" :search-enabled="true" :value="value" />
  </div>
</template>

<script>
import DxTagBox from "devextreme-vue/tag-box";
export default {
  props: {
    dataList: {
      type: Array,
      default: ["유재석", "아이브"],
    },
    value: {
      type: Array,
      default: ["상자", "망치"],
    },
  },
  data() {
    return {};
  },
  components: {
    DxTagBox,
  },
};
</script>
