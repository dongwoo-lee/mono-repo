<template>
  <div id="additional_information">
    <div v-if="cueInfo.cuetype == 'D' || cueInfo.cuetype == 'A'">
      <Attachments />
    </div>
    <div class="itme">
      <div class="title_text">태그</div>
      <div class="component">
        <Tag :value_items="valueItems" />
      </div>
    </div>
    <div class="itme">
      <div class="title_text">메모</div>
      <div class="component">
        <DxTextBox
          :maxLength="25"
          placeholder="메모를 입력하세요."
          v-model="cueInfo.memo"
          :disabled="cueInfo.cuetype === 'A'"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import Attachments from "../../../../src/components/FileUpload/Attachments/Upload.vue";
import Tag from "../../../components/Common/Tag.vue";
import DxTextBox from "devextreme-vue/text-box";
import ButtonWidget from "./ButtonWidget.vue";
export default {
  props: {
    valueItems: Array,
  },
  data() {
    return {};
  },
  created() {},
  components: {
    Attachments,
    Tag,
    DxTextBox,
    ButtonWidget,
  },
  computed: {
    ...mapGetters("cueList", ["cueInfo"]),
  },
  methods: {},
};
</script>
<style>
#additional_information {
  overflow: auto;
  height: 580px;
}
.additional_information_search_toggle_on {
  height: 330px !important;
  padding-bottom: 10px;
}
#additional_information .itme {
  margin: 10px 10px 0px 10px;
  border: 1px solid #d7d7d7;
  border-radius: 2px;
}
#additional_information .title_text {
  padding: 10px;
  border-bottom: 1px solid #d7d7d7;
}
#additional_information .component {
  padding: 10px;
}
</style>