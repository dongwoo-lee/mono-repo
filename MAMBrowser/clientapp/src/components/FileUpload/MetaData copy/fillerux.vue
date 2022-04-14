<template>
  <div>
    <div style="margin-top: 37px; font-size: 15px">
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
          @input="getSecondType"
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
          :value="selectedFillerType"
          :options="fillerTypeOptions"
          @input="secondTypeChange"
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
          top: -15px;
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
          top: -15px;
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
        { value: "general", text: "Filler(소재)" },
        { value: "etc", text: "Filler(기타)" },
      ],
      fillerMedia: "pr",
      prMedia: "",
      generallMedia: "",
      etcMedia: "",
      selectedFillerType: "",
    };
  },
  created() {
    this.reset();
    this.setTitle(this.sliceExt(30));
    this.getEditorForPd();
    this.resetFillerTypeOptions();

    axios.get("/api/categories/filler/pro").then((res) => {
      this.selectedFillerType = res.data.resultObject.data[0].id;
      this.setFillerTypeSelected(this.selectedFillerType);

      res.data.resultObject.data.forEach((e) => {
        this.setFillerTypeOptions({
          value: e.id,
          text: e.name,
        });
      });
    });

    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.setDate(today);
    this.setTempDate(today);
  },
  methods: {
    secondTypeChange(v) {
      this.setFillerTypeSelected(v);
    },
    getSecondType(v) {
      this.resetFillerTypeOptions();
      if (this.findValue(0, v)) {
        axios.get("/api/categories/filler/pro").then((res) => {
          this.selectedFillerType = res.data.resultObject.data[0].id;
          this.setFillerTypeSelected(this.selectedFillerType);
          res.data.resultObject.data.forEach((e) => {
            this.setFillerTypeOptions({
              value: e.id,
              text: e.name,
            });
          });
        });
      } else if (this.findValue(1, v)) {
        axios.get("/api/categories/filler/general").then((res) => {
          this.selectedFillerType = res.data.resultObject.data[0].id;
          this.setFillerTypeSelected(this.selectedFillerType);
          res.data.resultObject.data.forEach((e) => {
            this.setFillerTypeOptions({
              value: e.id,
              text: e.name,
            });
          });
        });
      } else if (this.findValue(2, v)) {
        axios.get("/api/categories/filler/etc").then((res) => {
          this.selectedFillerType = res.data.resultObject.data[0].id;
          this.setFillerTypeSelected(this.selectedFillerType);
          res.data.resultObject.data.forEach((e) => {
            this.setFillerTypeOptions({
              value: e.id,
              text: e.name,
            });
          });
        });
      }
    },
    findValue(num, v) {
      if (this.fillerOptions[num].value == v) {
        return true;
      } else {
        return false;
      }
    },
  },
};
</script>

<style></style>
