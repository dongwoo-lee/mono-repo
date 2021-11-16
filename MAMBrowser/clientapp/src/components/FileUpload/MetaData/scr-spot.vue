<template>
  <div style="margin-top:20px;">
    <b-form-group label="매체" class="has-float-label">
      <b-form-select
        id="program-media"
        class="media-select"
        style=" width:200px; height:37px;"
        :value="scrMedia"
        :options="fileMediaOptions"
        @input="mediaChange"
      />
    </b-form-group>

    <b-form-group
      label="제작자"
      class="has-float-label"
      style="margin-top:20px;"
    >
      <common-vue-select
        style="font-size:14px; width:200px; border: 1px solid #008ecc;"
        :suggestions="editorOptions"
        @inputEvent="inputEditor"
      ></common-vue-select>
    </b-form-group>

    <div style="height:50px;">
      <b-form-input
        class="editTask"
        v-model="MetaData.title"
        :state="titleState"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="소재 명"
        trim
      />
      <button
        v-show="titleState"
        style="position:relative; left:315px; top:-27px; z-index:99; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
      >
        <b-icon
          icon="x-circle"
          font-scale="1"
          style="position:relative; top:0px; right:0px; z-index:999;"
          variant="secondary"
          @click="resetTitle"
        ></b-icon>
      </button>
    </div>
    <div style="height:50px;">
      <b-form-input
        class="editTask"
        v-model="MetaData.advertiser"
        :state="advertiserState"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="광고주 명"
        trim
      />
      <button
        v-show="advertiserState"
        style="position:relative; left:315px; top:-27px; z-index:99; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
      >
        <b-icon
          icon="x-circle"
          font-scale="1"
          style="position:relative; top:0px; right:0px; z-index:999;"
          variant="secondary"
          @click="resetAdvertiser"
        ></b-icon>
      </button>
    </div>
    <div style="height:50px;">
      <b-form-input
        class="editTask"
        v-model="MetaData.memo"
        :state="memoState"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="설명"
        trim
      />

      <button
        v-show="memoState"
        style="position:relative; left:315px; top:-27px; z-index:99; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
      >
        <b-icon
          icon="x-circle"
          font-scale="1"
          style="position:relative; top:0px; right:0px; z-index:999;"
          variant="secondary"
          @click="resetMemo"
        ></b-icon>
      </button>
    </div>
  </div>
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CommonVueSelect from "../../../components/Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
import axios from "axios";
export default {
  components: {
    CommonVueSelect
  },
  mixins: [CommonFileFunction, MixinBasicPage],
  data() {
    return {
      scrMedia: "A"
    };
  },
  created() {
    this.getEditorForPd();
    this.resetFileMediaOptions();
    axios.get("/api/categories/scr/spot").then(res => {
      console.log(res.data.resultObject.data);
      res.data.resultObject.data.forEach(e => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name
        });
      });
    });
    this.scrMedia = "ST01";
    this.setMediaSelected(this.scrMedia);
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
