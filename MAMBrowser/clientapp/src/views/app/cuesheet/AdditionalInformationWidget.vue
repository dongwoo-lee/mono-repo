<template>
  <div id="additional_information">
    <div v-if="cueInfo.cuetype=='D'||cueInfo.cuetype=='A'">
      <Attachments />
    </div>
    <div class="itme">
      <div class="title_text">태그</div>
      <div class="component">
        <Tag :value_items="valueItems" />
      </div>
      <!-- <button @click="testClick">확인</button> -->
    </div>
    <div class="itme">
      <div class="title_text">메모</div>
      <div class="component">
        <DxTextArea
          :height="35"
          :maxLength="25"
          placeholder="메모를 입력하세요."
        />
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import Attachments from "../../../../src/components/FileUpload/Attachments/Upload.vue";
import Tag from "../../../components/Common/Tag.vue";
import DxTextArea from "devextreme-vue/text-area";
import ButtonWidget from "./ButtonWidget.vue";
export default {
  data() {
    return {
      valueItems: ["신혜림", "저스트팝", "JustPop"],
    };
  },
  created() {
    if (this.cueInfo.tagList != undefined) {
      this.valueItems = this.cueInfo.tagList;
    }
  },
  components: {
    Attachments,
    Tag,
    DxTextArea,
    ButtonWidget,
  },
  computed: {
    ...mapGetters("cueList", ["cueInfo"]),
  },
  methods: {
    testClick() {
      console.log(this.cueInfo.tagList);
    },
  },
};
</script>
<style>
#additional_information {
  height: 580px;
}
#additional_information .itme {
  margin: 15px 10px 0px 10px;
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