<template>
  <div id="additional_information">
    <div v-if="cueInfo.cuetype == 'D' || cueInfo.cuetype == 'A'">
      <div class="attachments_component">
        <div class="component_title">
          <span>{{ attachments_title_name }}</span>
          <span class="contents_counter ml-1">
            <b-badge
              pill
              variant="dark"
              :class="{
                max_background:
                  attachments.filter((item) => !item.delstate).length >=
                  max_attachments_count,
              }"
            >
              <span class="in_count">{{
                attachments.filter((item) => !item.delstate).length
              }}</span>
              /
              <span class="max_count">{{ max_attachments_count }}</span>
            </b-badge>
          </span>
        </div>
        <Attachments
          :maxAttachmentsCount="max_attachments_count"
          :disableValue="
            attachments.filter((item) => !item.delstate).length >=
            max_attachments_count
          "
        />
      </div>
    </div>
    <div class="itme">
      <div class="title_text">
        <span>태그</span>
        <span class="contents_counter m-1">
          <b-badge
            pill
            variant="dark"
            :class="{
              max_background: tags.length >= max_tag_count,
            }"
          >
            <span class="in_count">{{ tags.length }}</span> /
            <span class="max_count">{{ max_tag_count }}</span>
          </b-badge>
        </span>
        <span class="tag_item_error_msg" v-show="errorMsg_value">{{
          errorMsg
        }}</span>
      </div>
      <div class="component">
        <Tag
          :value_items="valueItems"
          :maxTagCount="max_tag_count"
          @sameTagError="OnTagErrorMsg"
        />
      </div>
    </div>
    <div class="itme">
      <div class="title_text">
        <span>메모</span>
        <span class="contents_counter ml-1">
          <b-badge
            pill
            variant="dark"
            :class="{
              max_background: cueInfo.memo.length >= max_memo_length,
            }"
          >
            <span class="in_count">{{
              cueInfo.memo.length >= max_memo_length
                ? max_memo_length
                : cueInfo.memo.length
            }}</span>
            /
            <span class="max_count">{{ max_memo_length }}</span>
          </b-badge>
        </span>
      </div>
      <div class="component">
        <DxTextBox
          :maxLength="max_memo_length"
          placeholder="메모를 입력하세요."
          v-model="cueInfo.memo"
          :value="cueInfo.memo"
          :disabled="cueInfo.cuetype === 'A'"
          @input="OnInputMemo"
          @change="OnChangeMemo"
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
    return {
      attachments_title_name: "첨부파일",
      max_tag_count: 15,
      max_attachments_count: 10,
      max_memo_length: 100,
      errorMsg: "동일한 태그가 추가되어 있습니다.",
      errorMsg_value: false,
    };
  },
  components: {
    Attachments,
    Tag,
    DxTextBox,
    ButtonWidget,
  },
  computed: {
    ...mapGetters("cueList", ["cueInfo"]),
    ...mapGetters("cueList", ["attachments"]),
    ...mapGetters("cueList", ["tags"]),
  },
  methods: {
    OnInputMemo(e) {
      this.cueInfo.memo = e.event.target.value;
    },
    OnChangeMemo() {
      const result = this.cueInfo.memo.replace(/[^\w\s|ㄱ-ㅎ|가-힣|, ]/gi, "");
      this.cueInfo.memo = result;
    },
    OnTagErrorMsg() {
      this.errorMsg_value = true;
      setTimeout(() => {
        this.errorMsg_value = false;
      }, 3000);
    },
  },
};
</script>
<style>
.red {
  color: red;
}
.tag_item_error_msg {
  color: #d9534f;
}
#additional_information {
  overflow: auto;
  height: 580px;
}
#additional_information .attachments_component {
  position: relative;
}
#additional_information .attachments_component .component_title {
  position: absolute;
  top: 32px;
  left: 20px;
  /* width: 80%; */
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
#additional_information .contents_counter > .badge.badge-pill {
  background-color: #6c757d;
  padding: 0.4em 0.6em 0.2em 0.6em;
}
.max_background {
  background-color: #d43f3a !important;
}
</style>