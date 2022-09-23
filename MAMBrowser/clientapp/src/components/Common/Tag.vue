<template>
  <div>
    <div id="cuesheet_tag_container">
      <DxTagBox
        v-if="cueInfo.cuetype != 'A'"
        :acceptCustomValue="true"
        :value="value_items"
        :noDataText="tagDataText"
        :placeholder="placeholderText"
        :element-attr="accordionAttributes"
        tag-template="tagTemplate"
        @value-changed="onValueChanged"
      >
        <div slot="tagTemplate" slot-scope="tagData" class="dx-tag-content">
          {{ tagData.data }}
          <div class="dx-tag-remove-button"></div>
        </div>
      </DxTagBox>
      <TagBadge
        v-else
        :rowData="{ tag: value_items }"
        @tagItemFromCommonTag="OnClickCommonTagItem"
      ></TagBadge>
    </div>
  </div>
</template>

<script>
import TagBadge from "../DataTable/CommonTag.vue";
import DxTagBox from "devextreme-vue/tag-box";
import { mapGetters, mapMutations } from "vuex";
export default {
  props: {
    value_items: Array,
  },
  data() {
    return {
      tagDataText: "태그를 입력하세요",
      tagItemMessage: "해당 태그로 이전 큐시트 목록을 검색 하시겠습니까?",
      placeholderText: "추가된 태그가 없습니다.",
      maxTagCount: 15,
      maxTagTextLength: 20,
      accordionAttributes: { class: "cue_tag " },
    };
  },
  components: {
    DxTagBox,
    TagBadge,
  },
  computed: {
    ...mapGetters("cueList", ["cueInfo"]),
    ...mapGetters("cueList", ["tags"]),
  },
  methods: {
    ...mapMutations("cueList", ["SET_TAGS"]),
    onValueChanged(e) {
      if (this.tags !== e.value) {
        const result = [];
        e.value.filter((tag) => {
          const item = tag
            .replace(/[^\w\s|ㄱ-ㅎ|가-힣|,]/gi, "")
            .replace(/ /g, "");
          if (!!item) {
            !this.MaxTagTextLengthChecker(item) &&
              !this.MaxTagCounter(result) &&
              result.push(item);
          }
        });
        this.SET_TAGS(result);
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
    MaxTagCounter(tags) {
      let isResult = false;
      const maxCountMessage = `태그는 ${this.maxTagCount}개 이상 추가할 수 없습니다.`;
      if (tags.length >= this.maxTagCount) {
        isResult = true;
        alert(maxCountMessage);
      }
      return isResult;
    },
    MaxTagTextLengthChecker(tag) {
      let isResult = false;
      const maxTextLengthMessage = `태그 글자 수는 ${this.maxTagTextLength}을 초과할 수 없습니다.`;
      if (tag.length > this.maxTagTextLength) {
        isResult = true;
        alert(maxTextLengthMessage);
      }
      return isResult;
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