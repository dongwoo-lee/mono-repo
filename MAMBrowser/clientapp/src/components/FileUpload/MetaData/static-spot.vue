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
    <div style="position:absolute; top:40px;">
      <div>
        <b-form-group
          label="시작일"
          class="has-float-label"
          style="width:200px; float:left; margin-right:20px;"
        >
          <b-input-group class="mb-3" style="width:200px; float:left;">
            <input
              style="height:33px; font-size:13px;"
              id="dateinput"
              type="text"
              class="form-control input-picker date-input"
              :value="fileSDate"
              @input="onsInput"
            />
            <b-input-group-append>
              <b-form-datepicker
                style="height:33px;"
                v-model="fileSDate"
                button-only
                button-variant="outline-primary"
                right
                aria-controls="example-input"
                @context="onContext"
              ></b-form-datepicker>
            </b-input-group-append>
          </b-input-group>
        </b-form-group>
        <b-form-group
          label="종료일"
          class="has-float-label"
          style="width:200px;"
        >
          <b-input-group class="mb-3" style="width:200px; float:left;">
            <input
              style="height:33px; font-size:13px;"
              id="dateinput"
              type="text"
              class="form-control input-picker date-input"
              :value="fileEDate"
              @input="oneInput"
            />
            <b-input-group-append>
              <b-form-datepicker
                style="height:33px;"
                v-model="fileEDate"
                button-only
                button-variant="outline-primary"
                right
                aria-controls="example-input"
                @context="onContext"
              ></b-form-datepicker>
            </b-input-group-append>
          </b-input-group>
        </b-form-group>
      </div>
    </div>
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
    return { staticMedia: "" };
  },
  created() {
    this.reset();
    this.getEditorForPd();
    this.resetFileMediaOptions();
    axios.get("/api/categories/media").then(res => {
      res.data.resultObject.data.forEach(e => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name
        });
      });
    });
    this.staticMedia = "A";
    this.setMediaSelected(this.staticaMedia);
  },
  methods: {
    ...mapMutations("FileIndexStore", ["setEditor"]),
    inputEditor(v) {
      this.setEditor(v.id);
    },
    changeMedia(v) {
      this.setMediaSelected(v);
    }
  }
};
</script>

<style></style>
