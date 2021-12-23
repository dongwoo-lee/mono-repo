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
      <p
        v-show="titleState"
        style="
          position: relative;
          left: 290px;
          top: -15px;
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
      <p
        v-show="memoState"
        style="
          position: relative;
          left: 290px;
          top: -15px;
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
      if (this.role == "ADMIN") {
        res.data.resultObject.data.forEach((e) => {
          this.setFileMediaOptions({
            id: e.id,
            name: e.name,
          });
        });
      } else {
        axios
          .get(
            `/api/categories/user-audiocodes?userId=${sessionStorage.getItem(
              "user_id"
            )}`
          )
          .then((res2) => {
            res.data.resultObject.data.forEach((e) => {
              res2.data.resultObject.data.forEach((e2) => {
                if (e.id == e2.id && this.role != "ADMIN") {
                  this.setFileMediaOptions({
                    id: e.id,
                    name: e.name,
                  });
                }
              });
            });
          });
      }
    });
    this.proMedia = "";
    this.setMediaSelected(this.proMedia);
    this.setProType(0);
    this.setProTypeName("Title Music");
  },
  methods: {
    mediaChange(v) {
      this.setMediaSelected(v.id);
      var data = this.fileMediaOptions.find((dt) => dt.id == v.id);
      this.setMediaName(data.name);
    },
    proTypeChange(v) {
      var data = this.proTypeOptions.find((dt) => dt.value == v);
      this.setProType(data.value);
      this.setProTypeName(data.text);
    },
  },
};
</script>

<style></style>
