<template>
  <div>
    <transition name="fade">
      <div>
        <b-form-group
          label="제작자"
          class="has-float-label"
          style="position:absolute; top:380px; left:-400px; z-index:9999; font-size:16px;"
        >
          <common-vue-select
            style="font-size:14px; width:200px; border: 1px solid #008ecc;"
            :suggestions="editorOptions"
            @inputEvent="inputEditor"
          ></common-vue-select>
        </b-form-group>
      </div>
    </transition>
  </div>
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
import axios from "axios";
export default {
  components: {
    CommonVueSelect
  },
  mixins: [CommonFileFunction, MixinBasicPage],
  data() {
    return {
      mcrMedia: "A"
    };
  },
  created() {
    this.reset();
    this.getEditorForPd();
    this.resetFileMediaOptions();

    axios.get("/api/categories/media/mcrspot").then(res => {
      res.data.resultObject.data.forEach(e => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name
        });
      });
    });
    this.mcrMedia = "A";
    this.setMediaSelected(this.mcrMedia);
  },
  methods: {
    ...mapMutations("FileIndexStore", ["setEditor"]),
    inputEditor(v) {
      this.setEditor(v.id);
    },
    mediaChange(v) {
      this.setMediaSelected(v);
    }
  }
};
</script>

<style></style>
