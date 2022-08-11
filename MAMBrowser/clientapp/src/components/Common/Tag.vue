<template>
  <div>
    <div>
      <DxTagBox
        :acceptCustomValue="true"
        :search-enabled="true"
        :value="value_items"
        :noDataText="tagDataText"
        @value-changed="onValueChanged"
      />
    </div>
  </div>
</template>

<script>
import DxTagBox from "devextreme-vue/tag-box";
import { mapGetters } from "vuex";
export default {
  props: {
    value_items: Array,
  },
  data() {
    return {
      tagDataText: "추가할 태그를 입력하세요",
    };
  },
  components: {
    DxTagBox,
  },
  computed: {
    ...mapGetters("cueList", ["cueInfo"]),
  },
  methods: {
    onValueChanged(e) {
      this.cueInfo.tagList = e.value;
    },
  },
};
</script>
