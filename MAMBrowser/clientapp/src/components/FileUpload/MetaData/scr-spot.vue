<template>
  <div style="margin-top: 20px">
    <b-form-group
      label="분류"
      class="has-float-label"
      style="float: left; margin-right: 20px; margin-top: 10px"
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
      style="font-size: 16px"
    >
      <b-form-input
        title="제작자"
        style="width: 160px; font-size: 14px"
        class="editTask"
        :value="userID"
        disabled
        aria-describedby="input-live-help input-live-feedback"
        placeholder="제작자"
        trim
      />
    </b-form-group>
    <div style="height: 50px">
      <b-form-input
        class="editTask"
        v-model="MetaData.title"
        :state="titleState"
        :maxlength="30"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="소재 명"
        trim
      />
      <p
        v-show="titleState"
        style="
          position: relative;
          left: 310px;
          top: 0px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.title.length }}/30
      </p>
    </div>
    <div style="height: 50px">
      <b-form-input
        class="editTask"
        v-model="MetaData.advertiser"
        :state="advertiserState"
        :maxLength="15"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="광고주 명"
        trim
      />
      <p
        v-show="advertiserState"
        style="
          position: relative;
          left: 310px;
          top: 0px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.advertiser.length }}/15
      </p>
    </div>
    <div style="height: 50px">
      <b-form-input
        class="editTask"
        v-model="MetaData.memo"
        :state="memoState"
        :maxLength="30"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="메모"
        trim
      />
      <p
        v-show="memoState"
        style="
          position: relative;
          left: 310px;
          top: 0px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.memo.length }}/30
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
      scrMedia: "",
      scrMediaName: "",
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
    this.scrMediaName = "우리의 소리를 찾아서";
    this.setMediaSelected(this.scrMedia);
    this.setMediaName(this.scrMediaName);
  },
  methods: {
    mediaChange(v) {
      this.setMediaSelected(v);
      var data = this.fileMediaOptions.find((dt) => dt.value == v);
      this.setMediaName(data.text);
    },
  },
};
</script>

<style></style>
