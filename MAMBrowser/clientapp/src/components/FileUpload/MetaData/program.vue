<template>
  <div>
    <transition name="fade">
      <div>
        <b-form-group
          label="제작자"
          class="has-float-label"
          style="position:absolute; top:380px; left:-400px; z-index:9999; font-size:16px;"
        >
          <common-vue-select
            style="font-size:14px; width:200px; border: 1px solid #008ecc;"
            deselectFromDropdown
            :suggestions="editorOptions"
            @inputEvent="inputEditor"
          ></common-vue-select>
        </b-form-group>
      </div>
    </transition>
    <div style="position:absolute; top:40px;">
      <b-form-group
        label="방송일"
        class="has-float-label"
        style="position:absolute; z-index:9989; font-color:black;"
      >
        <b-input-group class="mb-3" style="width:300px; float:left;">
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
              v-model="date"
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
        style="position:absolute; right:-220px; top:7px;  z-index:9999; width:3px;  background-color:#FFFFFF; border:0; outline:0;"
      >
        <b-icon
          icon="x-circle"
          font-scale="1"
          style="position:absolute; z-index:9999;"
          variant="secondary"
          @click="resetDate"
        ></b-icon>
      </button>

      <b-form-group
        label="매체"
        class="has-float-label"
        style="position:absolute; margin-left:320px; z-index:9999;"
      >
        <b-form-select
          :disabled="isActive"
          id="program-media"
          class="media-select"
          style=" width:140px; height:37px;"
          :value="this.proMedia"
          @input="mediaChange"
          :options="fileMediaOptions"
        />
      </b-form-group>
      <b-button
        :disabled="isActive"
        :variant="getVariant"
        style="position:absolute; width:70px; right:-550px; z-index:9989; "
        @click="getPro"
        >검색</b-button
      >
    </div>
    <div
      v-show="this.MetaData.typeSelected == 'program'"
      style="position:absolute; width:550px; top:90px; height: 210px;
    border: 1px solid #008ecc;"
    >
      <!-- //TODO: Data Binding -->
      <DxDataGrid
        name="proDxDataGrid"
        v-show="this.ProgramData.eventName != ''"
        style="height:208px;"
        :data-source="ProgramData"
        :selection="{ mode: 'single' }"
        :show-borders="true"
        :hover-state-enabled="true"
        key-expr="productId"
        :allow-column-resizing="true"
        :column-auto-width="true"
        no-data-text="No Data"
        @row-click="onRowClick"
      >
        <DxColumn data-field="eventName" caption="이벤트 명" />
        <DxColumn :width="60" data-field="eventType" caption="타입" />
        <DxColumn data-field="productId" caption="프로그램 ID" />
        <DxColumn data-field="onairTime" caption="방송 시간" />
        <DxColumn data-field="durationSec" caption="편성 분량" />
      </DxDataGrid>
    </div>
    <!-- 프로그램 -->
    <div
      v-show="!isActive && this.ProgramSelected.eventName != ''"
      style="width: 550px; height:140px; margin-top:280px; padding-left:10px; padding-right:10px; float:left; border:1px solid #008ecc;"
    >
      <div style="width:180px; float:left;">
        <b-form-group
          label="이벤트 명"
          class="has-float-label"
          style="margin-top:20px;"
        >
          <b-form-input
            style="width:180px;"
            class="editTask"
            v-model="this.ProgramSelected.eventName"
            readonly
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width:170px; margin-left:20px; float:left;">
        <b-form-group
          label="프로그램 ID"
          class="has-float-label"
          style="margin-top:20px;"
        >
          <b-form-input
            style="width:170px;"
            class="editTask"
            v-model="this.ProgramSelected.productId"
            readonly
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width:120px; margin-left:400px;">
        <b-form-group
          label="이벤트 타입"
          class="has-float-label"
          style="margin-top:20px;"
        >
          <b-form-input
            style="width:120px;"
            class="editTask"
            v-model="this.ProgramSelected.eventType"
            readonly
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>

      <div style="width:200px; float:left;">
        <b-form-group label="방송 시간" class="has-float-label">
          <b-form-input
            style="width:200px;"
            class="editTask"
            v-model="this.ProgramSelected.onairTime"
            readonly
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width:200px; margin-left:20px; float:left;">
        <b-form-group label="편성 분량" class="has-float-label">
          <b-form-input
            style="width:120px;"
            class="editTask"
            v-model="this.ProgramSelected.durationSec"
            readonly
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
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import { mapState, mapGetters, mapMutations } from "vuex";
import axios from "axios";
export default {
  components: {
    CommonVueSelect
  },
  mixins: [CommonFileFunction, MixinBasicPage],
  data() {
    return {
      proMedia: "A"
    };
  },
  created() {
    this.reset();
    this.getEditorForPd();
    this.resetFileMediaOptions();
    axios.get("/api/categories/media").then(res => {
      res.data.resultObject.data.forEach(e => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name
        });
      });
    });
    this.proMedia = "A";
    this.setMediaSelected(this.proMedia);
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
