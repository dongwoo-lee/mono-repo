<template>
  <div style="margin-top: 20px">
    <b-form-group
      label="매체"
      class="has-float-label"
      style="float: left; margin-right: 20px"
    >
      <b-form-select
        id="program-media"
        class="media-select"
        style="width: 170px"
        :value="scrMedia"
        :options="fileMediaOptions"
        @input="mediaChange"
      />
    </b-form-group>

    <b-form-group
      label="제작자"
      class="has-float-label"
      style="margin-top: 10px; margin-bottom: 20px"
    >
      <common-vue-select
        style="font-size: 14px; width: 160px; border: 1px solid #008ecc"
        class="h105"
        :suggestions="editorOptions"
        @inputEvent="inputEditor"
      ></common-vue-select>
    </b-form-group>
    <div style="height: 50px">
      <b-form-input
        class="editTask"
        v-model="MetaData.title"
        :state="titleState"
        :maxlength="200"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="소재 명"
        trim
      />
      <button
        v-show="titleState"
        style="
          position: relative;
          left: 315px;
          top: -27px;
          z-index: 99;
          width: 3px;
          heigth: 3px;
          background-color: #ffffff;
          border: 0;
          outline: 0;
        "
      >
        <b-icon
          icon="x-circle"
          font-scale="1"
          style="position: relative; top: 0px; right: 0px; z-index: 999"
          variant="secondary"
          @click="resetTitle"
        ></b-icon>
      </button>
      <p
        v-show="titleState"
        style="
          position: relative;
          left: 310px;
          top: -20px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.title.length }}/200
      </p>
    </div>
    <div style="height: 50px">
      <b-form-input
        class="editTask"
        v-model="MetaData.advertiser"
        :state="advertiserState"
        :maxLength="50"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="광고주 명"
        trim
      />
      <button
        v-show="advertiserState"
        style="
          position: relative;
          left: 315px;
          top: -27px;
          z-index: 99;
          width: 3px;
          heigth: 3px;
          background-color: #ffffff;
          border: 0;
          outline: 0;
        "
      >
        <b-icon
          icon="x-circle"
          font-scale="1"
          style="position: relative; top: 0px; right: 0px; z-index: 999"
          variant="secondary"
          @click="resetAdvertiser"
        ></b-icon>
      </button>
      <p
        v-show="advertiserState"
        style="
          position: relative;
          left: 310px;
          top: -20px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.advertiser.length }}/50
      </p>
    </div>
    <div style="height: 50px">
      <b-form-input
        class="editTask"
        v-model="MetaData.memo"
        :state="memoState"
        :maxLength="200"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="설명"
        trim
      />

      <button
        v-show="memoState"
        style="
          position: relative;
          left: 315px;
          top: -27px;
          z-index: 99;
          width: 3px;
          heigth: 3px;
          background-color: #ffffff;
          border: 0;
          outline: 0;
        "
      >
        <b-icon
          icon="x-circle"
          font-scale="1"
          style="position: relative; top: 0px; right: 0px; z-index: 999"
          variant="secondary"
          @click="resetMemo"
        ></b-icon>
      </button>
      <p
        v-show="memoState"
        style="
          position: relative;
          left: 310px;
          top: -20px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.memo.length }}/200
      </p>
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
    CommonVueSelect,
  },
  mixins: [CommonFileFunction, MixinBasicPage],
  data() {
    return {
      scrMedia: "A",
    };
  },
  created() {
    this.reset();
    this.getEditorForPd();
    this.resetFileMediaOptions();
    axios.get("/api/categories/scr/spot").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name,
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
    },
  },
};
</script>

<style></style>
