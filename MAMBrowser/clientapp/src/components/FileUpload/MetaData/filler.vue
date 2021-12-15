<template>
  <div>
    <transition name="fade">
      <div>
        <b-form-input
          style="position: absolute; top: 340px; left: -400px; z-index: 9999"
          class="editTask"
          v-model="MetaData.title"
          :state="titleState"
          :maxLength="200"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="소재 명"
          trim
        />
        <button
          v-show="titleState"
          style="
            position: relative;
            left: -85px;
            top: 323px;
            z-index: 9999;
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
            style="position: relative; top: 0px; right: 0px; z-index: 9999"
            variant="secondary"
            @click="resetTitle"
          />
        </button>
        <p
          v-show="titleState"
          style="
            position: relative;
            left: -90px;
            top: 330px;
            z-index: 9999;
            width: 30px;
            margin-right: 0px;
          "
        >
          {{ MetaData.title.length }}/200
        </p>
      </div>
    </transition>
    <transition name="fade">
      <div style="position: absolute; top: 395px; left: -400px; z-index: 9999">
        <b-form-input
          class="editTask"
          v-model="MetaData.memo"
          :state="memoState"
          :maxLength="30"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="메모"
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
          />
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
          {{ MetaData.memo.length }}/30
        </p>
      </div>
    </transition>
    <transition name="fade">
      <div>
        <b-form-group
          label="제작자"
          class="has-float-label"
          style="
            position: absolute;
            top: 460px;
            left: -400px;
            z-index: 9999;
            font-size: 16px;
          "
        >
          <common-vue-select
            style="font-size: 14px; width: 350px; border: 1px solid #008ecc"
            class="h105"
            :suggestions="editorOptions"
            @inputEvent="inputEditor"
          ></common-vue-select>
        </b-form-group>
      </div>
    </transition>
    <div style="position: absolute; top: 40px">
      <b-form-group
        label="방송일"
        class="has-float-label"
        style="position: absolute; z-index: 9989; font-color: black"
      >
        <b-input-group class="mb-3" style="width: 300px; float: left">
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
      <button
        v-show="!isActive"
        style="
          position: absolute;
          right: -220px;
          top: 7px;
          z-index: 9999;
          width: 3px;
          background-color: #ffffff;
          border: 0;
          outline: 0;
        "
      >
        <b-icon
          icon="x-circle"
          font-scale="1"
          style="position: absolute; z-index: 9999"
          variant="secondary"
          @click="resetDate"
        ></b-icon>
      </button>

      <b-form-group
        label="1차 분류"
        class="has-float-label"
        style="position: absolute; margin-left: 320px; z-index: 9999"
      >
        <b-form-select
          :disabled="isActive"
          id="program-media"
          class="media-select"
          style="width: 230px; height: 37px"
          :value="fillerMedia"
          :options="fillerOptions"
          @input="getSecondMedia"
        />
      </b-form-group>
      <b-form-group
        label="2차 분류"
        class="has-float-label"
        style="position: absolute; top: 60px; z-index: 999"
      >
        <b-form-select
          id="program-media"
          class="media-select"
          style="width: 200px; height: 37px"
          :value="selectedFillerMedia"
          :options="fileMediaOptions"
          @input="mediaChange"
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
    this.setMediaSelected(this.selectedFillerMedia);

    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.setDate(today);
  },
  methods: {
    mediaChange(v) {
      this.setMediaSelected(v);
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
