<template>
  <div>
    <div style="margin-top: 37px; font-size:15px">
      <b-form-group label="방송일" class="has-float-label">
        <b-input-group class="mb-3" style="width: 425px; float: left">
          <input
            :disabled="isActive"
            id="dateinput"
            type="text"
            class="form-control input-picker date-input"
            :value="date"
            @input="onInput"
          />
          <b-input-group-append>
            <b-form-datepicker
              :value="date"
              @input="eventInput"
              button-only
              :disabled="isActive"
              :button-variant="getVariant"
              right
              aria-controls="example-input"
              @context="onContext"
            ></b-form-datepicker>
          </b-input-group-append>
        </b-input-group>
      </b-form-group>

      <b-form-group
        label="1차 분류"
        class="has-float-label"
        style="margin-top: 13px"
      >
        <b-form-select
          :disabled="isActive"
          id="program-media"
          class="media-select"
          style="width: 425px; height: 37px"
          :value="fillerMedia"
          :options="fillerOptions"
          @input="getSecondMedia"
        />
      </b-form-group>
      <b-form-group
        label="2차 분류"
        class="has-float-label"
        style="margin-top: 32px"
      >
        <b-form-select
          id="program-media"
          class="media-select"
          style="width: 425px; height: 37px"
          :value="selectedFillerMedia"
          :options="fileMediaOptions"
          @input="mediaChange"
        />
      </b-form-group>
    </div>
    <div style="font-size: 16px; margin-top: 25px; height: 50px">
      <b-form-group label="소재명" class="has-float-label">
        <b-form-input
          class="editTask"
          v-model="MetaData.title"
          :state="titleState"
          :maxLength="30"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="소재명"
          trim
        />
      </b-form-group>
      <p
        v-show="titleState"
        style="
          position: relative;
          left: 390px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.title.length }}/30
      </p>
    </div>

    <div style="font-size: 16px; margin-top: 15px; height: 50px">
      <b-form-group label="메모" class="has-float-label">
        <b-form-input
          style="width: 425px"
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
          left: 390px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.memo.length }}/30
      </p>
    </div>
  </div>
  <!-- <div>
    
  </div> -->
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
import axios from "axios";
export default {
  components: {
    CommonVueSelect,
  },
  mixins: [CommonFileFunction, MixinBasicPage],
  data() {
    return {
      fillerOptions: [
        { value: "pr", text: "Filler(PR)" },
        { value: "general", text: "Filler(일반)" },
        { value: "etc", text: "Filler(기타)" },
      ],
      fillerMedia: "pr",
      prMedia: "",
      generallMedia: "",
      etcMedia: "",
      selectedFillerMedia: "",
      selectedFillerMediaName: "",
    };
  },
  created() {
    this.reset();
    this.getEditorForPd();
    this.resetFileMediaOptions();
    axios.get("/api/categories/filler/pro").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name,
        });
      });
    });
    this.selectedFillerMedia = "FC05";
    this.selectedFillerMediaName = "공통PR-프로그램";

    this.setMediaSelected(this.selectedFillerMedia);
    this.setMediaName(this.selectedFillerMediaName);

    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.setDate(today);
    this.setTempDate(today);
  },
  methods: {
    mediaChange(v) {
      this.setMediaSelected(v);
      var data = this.fileMediaOptions.find((dt) => dt.value == v);
      this.setMediaName(data.text);
    },
    getSecondMedia(v) {
      this.resetFileMediaOptions();
      if (v == "pr") {
        axios.get("/api/categories/filler/pro").then((res) => {
          res.data.resultObject.data.forEach((e) => {
            this.setFileMediaOptions({
              value: e.id,
              text: e.name,
            });
          });
        });
        this.selectedFillerMedia = "FC05";
      } else if (v == "general") {
        axios.get("/api/categories/filler/general").then((res) => {
          res.data.resultObject.data.forEach((e) => {
            this.setFileMediaOptions({
              value: e.id,
              text: e.name,
            });
          });
        });
        this.selectedFillerMedia = "FC01";
      } else if (v == "etc") {
        axios.get("/api/categories/filler/etc").then((res) => {
          res.data.resultObject.data.forEach((e) => {
            this.setFileMediaOptions({
              value: e.id,
              text: e.name,
            });
          });
        });
        this.selectedFillerMedia = "FC20";
      }
    },
  },
};
</script>

<style></style>
