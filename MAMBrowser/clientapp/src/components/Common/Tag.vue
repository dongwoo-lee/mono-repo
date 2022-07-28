<template>
  <div>
    <div>
      <DxTagBox
        :items="tag_data_list"
        :search-enabled="true"
        :value="value_items"
        :hide-selected-items="true"
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
    tag_data_list: Array,
    value_items: Array,
  },
  data() {
    return {};
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
