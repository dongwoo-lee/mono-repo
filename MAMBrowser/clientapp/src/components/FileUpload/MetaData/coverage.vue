<template>
  <div>
    <transition name="fade">
      <div style="position: absolute; top: 340px; left: -400px; z-index: 9999">
        <b-form-input
          class="editTask"
          v-model="MetaData.memo"
          :state="memoState"
          :maxLength="200"
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
    </transition>
    <transition name="fade">
      <div>
        <b-form-input
          style="position: absolute; top: 395px; left: -400px; z-index: 9999"
          class="editTask"
          v-model="MetaData.reporter"
          :state="reporterState"
          :maxLength="50"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="취재인 명"
          trim
        />
        <button
          v-show="reporterState"
          style="
            position: relative;
            left: -86px;
            top: 379px;
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
            @click="resetReporter"
          ></b-icon>
        </button>
        <p
          v-show="reporterState"
          style="
            position: relative;
            left: -90px;
            top: 385px;
            z-index: 9999;
            width: 30px;
            margin-right: 0px;
          "
        >
          {{ MetaData.reporter.length }}/50
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
        label="매체"
        class="has-float-label"
        style="position: absolute; margin-left: 320px; z-index: 9999"
      >
        <b-form-select
          :disabled="isActive"
          id="program-media"
          class="media-select"
          style="width: 140px; height: 37px"
          :value="coverageMedia"
          :options="fileMediaOptions"
          @input="mediaChange"
        />
      </b-form-group>
      <b-button
        :disabled="isActive"
        :variant="getVariant"
        style="position: absolute; width: 70px; right: -550px; z-index: 9989"
        @click="getPro"
        >검색</b-button
      >
    </div>
    <div style="position: absolute; top: 85px">
      <DxDataGrid
        name="mcrDxDataGrid"
        v-show="this.EventData.id != ''"
        style="
          height: 280px;
          border: 1px solid #008ecc;
          font-family: 'MBC 새로움 M';
        "
        :data-source="EventData"
        :selection="{ mode: 'single' }"
        :show-borders="true"
        :hover-state-enabled="true"
        key-expr="id"
        :allow-column-resizing="true"
        :column-auto-width="true"
        no-data-text="No Data"
        @row-click="onRowClick"
      >
        <DxPager :visible="false" />
        <DxColumn data-field="name" caption="이벤트 명" />
        <DxColumn data-field="id" caption="이벤트 ID" />
      </DxDataGrid>
    </div>
    <!-- 프로그램 -->
    <div
      v-show="!isActive && EventSelected.id != ''"
      style="
        position: absolute;
        top: 385px;
        width: 550px;
        height: 110px;
        padding-top: 10px;
        padding-left: 10px;
        padding-right: 10px;
        float: left;
        border: 1px solid #008ecc;
        font-family: 'MBC 새로움 M';
      "
    >
      <div style="width: 180px; float: left">
        <b-form-group
          label="이벤트 명"
          class="has-float-label"
          style="margin-top: 20px"
        >
          <b-form-input
            style="width: 180px"
            class="editTask"
            v-model="EventSelected.name"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 170px; margin-left: 20px; float: left">
        <b-form-group
          label="이벤트 ID"
          class="has-float-label"
          style="margin-top: 20px"
        >
          <b-form-input
            style="width: 170px"
            class="editTask"
            v-model="EventSelected.id"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
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
import { DxPager } from "devextreme-vue/data-grid";
import axios from "axios";
export default {
  components: {
    CommonVueSelect,
    DxPager,
  },
  mixins: [CommonFileFunction, MixinBasicPage],
  data() {
    return {
      coverageMedia: "",
    };
  },
  created() {
    this.reset();
    this.getEditorForReporter();
    this.resetFileMediaOptions();

    axios.get("/api/categories/report").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name,
        });
      });
    });
    this.coverageMedia = "RC07";
    this.setMediaSelected(this.coverageMedia);

    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.setDate(today);

    this.getPro();
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
