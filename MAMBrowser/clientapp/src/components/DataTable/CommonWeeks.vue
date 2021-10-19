<template>
  <div class="common-weeks">
    <span
      v-bind:class="{
        'common-weeks-disable':
          rowData.productWeekList[0].weekList.includes('MON') &&
          !rowData.activeWeekList.includes('MON'),
        'common-weeks-active': rowData.activeWeekList.includes('MON'),
      }"
      >월</span
    >
    <span
      v-bind:class="{
        'common-weeks-disable':
          rowData.productWeekList[0].weekList.includes('TUE') &&
          !rowData.activeWeekList.includes('TUE'),
        'common-weeks-active': rowData.activeWeekList.includes('TUE'),
      }"
      >화</span
    >
    <span
      v-bind:class="{
        'common-weeks-disable':
          rowData.productWeekList[0].weekList.includes('WED') &&
          !rowData.activeWeekList.includes('WED'),
        'common-weeks-active': rowData.activeWeekList.includes('WED'),
      }"
      >수</span
    >
    <span
      v-bind:class="{
        'common-weeks-disable':
          rowData.productWeekList[0].weekList.includes('THU') &&
          !rowData.activeWeekList.includes('THU'),
        'common-weeks-active': rowData.activeWeekList.includes('THU'),
      }"
      >목</span
    >
    <span
      v-bind:class="{
        'common-weeks-disable':
          rowData.productWeekList[0].weekList.includes('FRI') &&
          !rowData.activeWeekList.includes('FRI'),
        'common-weeks-active': rowData.activeWeekList.includes('FRI'),
      }"
      >금</span
    >
    <span
      v-bind:class="{
        'common-weeks-disable':
          rowData.productWeekList[0].weekList.includes('SAT') &&
          !rowData.activeWeekList.includes('SAT'),
        'common-weeks-active': rowData.activeWeekList.includes('SAT'),
      }"
      >토</span
    >
    <span
      v-bind:class="{
        'common-weeks-disable':
          rowData.productWeekList[0].weekList.includes('SUN') &&
          !rowData.activeWeekList.includes('SUN'),
        'common-weeks-active': rowData.activeWeekList.includes('SUN'),
      }"
      >일</span
    >

    <button v-if="edit" class="ml-1">변경</button>
  </div>
</template>
<script>
export default {
  props: {
    rowData: {
      type: Object,
      default: () => {},
    },
    productWeekList: {
      type: Array,
      default: () => [],
    },
    edit: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      weekListData: [
        {
          weekList: [],
        },
      ],
      activeList: [],
    };
  },
  mounted() {},
  computed: {},
  created() {},
  watch: {},
  methods: {},
};
</script>
<style>
.common-weeks > span {
  border: solid 1px #009eef;
  color: #009eef;
  font-size: 12px;
  padding: 3px;
  margin: 1px;
}
.common-weeks-active {
  background-color: #009eef !important;
  color: white !important;
}

.common-weeks-disable {
  border: solid 1px rgb(172, 170, 170) !important;
  background-color: #ebebeb !important;
  color: rgb(172, 170, 170) !important;
}
</style>