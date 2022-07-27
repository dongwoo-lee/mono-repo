<template>
  <div>
    <div>
      <DxTagBox :items="dataList" :search-enabled="true" :value="value" />
    </div>
    <button @click="testClick">확인</button>
  </div>
</template>

<script>
import DxTagBox from "devextreme-vue/tag-box";
export default {
  props: {
    // dataList: {
    //   type: Array,
    //   default: ["유재석", "아이브"],
    // },
    // value: {
    //   type: Array,
    //   default: ["선택된 태그"],
    // },
    dataList: Array,
    value: Array,
  },
  data() {
    return {};
  },
  components: {
    DxTagBox,
  },
  methods: {
    testClick() {},
  },
};
</script>
