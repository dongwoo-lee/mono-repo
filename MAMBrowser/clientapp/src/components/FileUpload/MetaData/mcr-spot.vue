<template>
  <div>
    <transition name="fade">
      <div
        style="
          position: absolute;
          top: 350px;
          left: -400px;
          z-index: 9999;
          font-size: 16px;
        "
      >
        <b-form-group label="메모" class="has-float-label">
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
            top: -40px;
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
            left: 315px;
            top: -35px;
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
      <div
        style="
          position: absolute;
          top: 415px;
          left: -400px;
          z-index: 9999;
          font-size: 16px;
        "
      >
        <b-form-group label="광고주" class="has-float-label">
          <b-form-input
            class="editTask"
            v-model="MetaData.advertiser"
            :state="advertiserState"
            :maxLength="50"
            aria-describedby="input-live-help input-live-feedback"
            placeholder="광고주"
            trim
          />
        </b-form-group>
        <button
          v-show="advertiserState"
          style="
            position: relative;
            left: 315px;
            top: -40px;
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
            @click="resetAdvertiser"
          ></b-icon>
        </button>
        <p
          v-show="advertiserState"
          style="
            position: relative;
            left: 315px;
            top: -35px;
            z-index: 9999;
            width: 30px;
            margin-right: 0px;
          "
        >
          {{ MetaData.advertiser.length }}/50
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
            top: 485px;
            left: -400px;
            z-index: 9999;
            font-size: 16px;
          "
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
            class="form-control input-picker"
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
          :value="mcrMedia"
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
    <div
      v-show="this.MetaData.typeSelected == 'mcr-spot'"
      style="position: absolute; top: 100px"
    >
      <DxDataGrid
        name="mcrDxDataGrid"
        v-show="this.EventData.id != ''"
        style="
          height: 300px;
          border: 1px solid silver;
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
        <DxPager :visible="true" />
        <DxColumn data-field="name" caption="이벤트 명" />
        <DxColumn data-field="id" caption="이벤트 ID" />
        <DxColumn data-field="duration" caption="편성 분량" />
      </DxDataGrid>
    </div>
    <!-- 프로그램 -->
    <div
      v-show="!isActive && EventSelected.id != ''"
      style="
        position: absolute;
        top: 420px;
        width: 550px;
        height: 110px;
        padding-top: 10px;
        padding-left: 10px;
        padding-right: 10px;
        float: left;
        border: 1px solid silver;
        font-family: 'MBC 새로움 M';
      "
    >
      <div style="width: 200px; float: left">
        <b-form-group
          label="이벤트 명"
          class="has-float-label"
          style="margin-top: 20px"
        >
          <b-form-input
            style="width: 200px"
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
      <div style="width: 100px; margin-left: 20px; float: left">
        <b-form-group
          label="편성 분량"
          class="has-float-label"
          style="margin-top: 20px"
        >
          <b-form-input
            style="width: 100px"
            class="editTask"
            v-model="EventSelected.duration"
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
import MixinFillerPage from "../../../mixin/MixinFillerPage";
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
import { DxPager } from "devextreme-vue/data-grid";
import axios from "axios";
export default {
  components: {
    CommonVueSelect,
    DxPager,
  },
  mixins: [CommonFileFunction, MixinBasicPage, MixinFillerPage],
  data() {
    return {
      mcrMedia: "A",
      mediaName: "AM",
    };
  },
  created() {
    this.reset();
    this.getEditorForMd();
    this.resetFileMediaOptions();

    axios.get("/api/categories/media").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name,
        });
      });
    });
    this.mcrMedia = "A";
    this.setMediaSelected(this.mcrMedia);

    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.setDate(today);
    this.setTempDate(today);
    this.getPro();
  },
  methods: {
    mediaChange(v) {
      this.setMediaSelected(v);
      var data = this.fileMediaOptions.find((dt) => dt.value == v);
      this.mediaName = data.text;
    },
    getData() {},
  },
};
</script>

<style></style>
