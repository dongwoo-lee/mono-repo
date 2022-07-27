<template>
  <div id="additional_information">
    <div>
      <Attachments />
    </div>
    <div class="itme">
      <div class="title">태그</div>
      <div class="component">
        <Tag :dataList="tagDataList" :value="cueInfo.tagList" />
      </div>
    </div>
    <div class="itme">
      <div class="title">메모</div>
      <div class="component">
        <DxTextArea
          :height="35"
          :maxLength="25"
          placeholder="메모를 입력하세요."
        />
        <button @click="test">확인</button>
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
      tagDataList: ["데이터"],
    };
  },
  mounted() {
    this.cueInfo.tagList = [];
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
    test() {
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
#additional_information .title {
  padding: 10px;
  border-bottom: 1px solid #d7d7d7;
}
#additional_information .component {
  padding: 10px;
}
</style>