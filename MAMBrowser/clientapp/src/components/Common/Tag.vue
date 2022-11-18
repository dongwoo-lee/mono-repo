<template>
  <div>
    <div id="cuesheet_tag_container">
      <DxTagBox
        v-if="cueInfo.cuetype != 'A'"
        :openOnFieldClick="false"
        :acceptCustomValue="true"
        :value="value_items"
        :placeholder="placeholderText"
        :element-attr="accordionAttributes"
        tag-template="tagTemplate"
        field-template="field"
        @value-changed="onValueChanged"
        @customItemCreating="onCustomItemCreating"
      >
        <template #field>
          <div class="dx-tag">
            <DxTextBox :maxLength="maxTagTextLength" :readOnly="isAccept" />
          </div>
        </template>
        <div slot="tagTemplate" slot-scope="tagData" class="dx-tag-content">
          {{ tagData.data.text }}
          <div class="dx-tag-remove-button"></div>
        </div>
      </DxTagBox>
      <TagBadge
        v-else
        :rowData="{ tag: value_items }"
        class="cue_tag"
        @tagItemFromCommonTag="OnClickCommonTagItem"
      ></TagBadge>
    </div>
  </div>
</template>

<script>
import TagBadge from "../DataTable/CommonTag.vue";
import DxTagBox from "devextreme-vue/tag-box";
import DxTextBox from "devextreme-vue/text-box";
import { mapGetters, mapMutations } from "vuex";
export default {
  props: {
    value_items: Array,
    maxTagCount: Number,
  },
  data() {
    return {
      tagItemMessage: "해당 태그로 이전 큐시트 목록을 검색 하시겠습니까?",
      placeholderText: "추가된 태그가 없습니다.",
      maxTagTextLength: 20,
      accordionAttributes: { class: "cue_tag " },
      isAccept: false,
    };
  },
  components: {
    DxTagBox,
    TagBadge,
    DxTextBox,
  },
  computed: {
    ...mapGetters("cueList", ["cueInfo"]),
    ...mapGetters("cueList", ["tags"]),
  },
  methods: {
    ...mapMutations("cueList", ["SET_TAGS"]),
    onValueChanged(e) {
      if (this.tags !== e.value) {
        const result = new Set();
        let emp = true;
        e.value.filter((tag) => {
          const item = tag.text
            .replace(/[^\w\s|ㄱ-ㅎ|가-힣|,]/gi, "")
            .replace(/ /g, "");
          if (!!item) {
            result.add(item);
          } else {
            emp = false;
          }
        });
        const resultArr = Array.from(result);
        if (emp && this.tags.length === resultArr.length) {
          this.$emit("sameTagError");
        }
        this.SET_TAGS(resultArr);
        this.isAccept = result.size >= this.maxTagCount ? true : false;
      }
    },
    OnClickCommonTagItem(item) {
      if (confirm(this.tagItemMessage)) {
        this.$router.push({
          name: "cuesheet-previous-list",
          params: { tagItem: item },
        });
      }
    },
    onCustomItemCreating(args) {
      const newProduct = {
        num: this.tags.length + 1,
        text: args.text,
      };
      args.customItem = newProduct;
    },
  },
};
</script>
<style>
#cuesheet_tag_container > .cue_tag {
  overflow: auto !important;
  max-height: 120px !important;
}
</style>