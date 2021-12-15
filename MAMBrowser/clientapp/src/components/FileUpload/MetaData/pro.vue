<template>
  <div style="margin-top: 20px">
    <b-form-group
      label="분류"
      class="has-float-label"
      style="margin-right: 20px; font-size: 16px"
    >
      <common-vue-select
        class="h120"
        style="width: 350px; font-size: 14px"
        :value="proMedia"
        :suggestions="fileMediaOptions"
        @inputEvent="mediaChange"
      ></common-vue-select>
    </b-form-group>

    <b-form-group
      label="타입"
      class="has-float-label"
      style="font-size: 16px; margin-top: 20px"
    >
      <b-form-select
        id="program-media"
        class="media-select"
        style="width: 350px"
        :value="proType"
        :options="proTypeOptions"
        @input="proTypeChange"
      />
    </b-form-group>
    <div style="height: 50px">
      <b-form-group
        label="소재"
        class="has-float-label"
        style="font-size: 16px; margin-top: 20px"
      >
        <b-form-input
          class="editTask"
          v-model="MetaData.title"
          :state="titleState"
          :maxlength="200"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="소재"
          trim
        />
      </b-form-group>
      <button
        v-show="titleState"
        style="
          position: relative;
          left: 315px;
          top: -43px;
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
          left: 290px;
          top: -35px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.title.length }}/200
      </p>
    </div>

    <div style="height: 50px">
      <b-form-group
        label="메모"
        class="has-float-label"
        style="margin-top: 20px; font-size: 16px"
      >
        <b-form-input
          class="editTask"
          v-model="MetaData.memo"
          :state="memoState"
          :maxLength="30"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="메모"
          trim
        />
      </b-form-group>
      <button
        v-show="memoState"
        style="
          position: relative;
          left: 315px;
          top: -43px;
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
          left: 290px;
          top: -35px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.memo.length }}/30
      </p>
    </div>
    <div style="height: 50px">
      <b-form-group
        label="제작자"
        class="has-float-label"
        style="font-size: 16px; margin-top: 20px"
      >
        <b-form-input
          title="제작자"
          style="width: 350px; font-size: 14px"
          class="editTask"
          :value="userID"
          disabled
          aria-describedby="input-live-help input-live-feedback"
          placeholder="제작자"
          trim
        />
      </b-form-group>
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
      proMedia: "",
      proType: "0",
      proTypeOptions: [
        { value: "0", text: "Title Music" },
        { value: "1", text: "Effect" },
        { value: "2", text: "Cut" },
        { value: "3", text: "모음1" },
        { value: "4", text: "모음2" },
        { value: "5", text: "모음3" },
        { value: "6", text: "모음4" },
        { value: "7", text: "모음5" },
        { value: "8", text: "모음6" },
        { value: "9", text: "모음7" },
        { value: "10", text: "모음8" },
        { value: "11", text: "모음9" },
        { value: "12", text: "모음10" },
      ],
    };
  },
  created() {
    this.reset();
    this.getEditorForPd();
    this.resetFileMediaOptions();
    axios.get("/api/categories/pro").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this.setFileMediaOptions({
          id: e.id,
          name: e.name,
        });
      });
    });
    this.proMedia = "AC73";
    this.setMediaSelected(this.proMedia);
    this.setProType(0);
    this.setProTypeName("Title Music");
  },
  methods: {
    mediaChange(v) {
      this.setMediaSelected(v.id);
    },
    proTypeChange(v) {
      if (v == 0) {
        this.setProType(0);
        this.setProTypeName("Title Music");
      } else if (v == 1) {
        this.setProType(1);
        this.setProTypeName("Effect");
      } else if (v == 2) {
        this.setProType(2);
        this.setProTypeName("Cut");
      } else if (v == 3) {
        this.setProType(3);
        this.setProTypeName("모음1");
      } else if (v == 4) {
        this.setProType(4);
        this.setProTypeName("모음2");
      } else if (v == 5) {
        this.setProType(5);
        this.setProTypeName("모음3");
      } else if (v == 6) {
        this.setProType(6);
        this.setProTypeName("모음4");
      } else if (v == 7) {
        this.setProType(7);
        this.setProTypeName("모음5");
      } else if (v == 8) {
        this.setProType(8);
        this.setProTypeName("모음6");
      } else if (v == 9) {
        this.setProType(9);
        this.setProTypeName("모음7");
      } else if (v == 10) {
        this.setProType(10);
        this.setProTypeName("모음8");
      } else if (v == 11) {
        this.setProType(11);
        this.setProTypeName("모음9");
      } else if (v == 12) {
        this.setProType(12);
        this.setProTypeName("모음10");
      }
    },
  },
};
</script>

<style></style>
