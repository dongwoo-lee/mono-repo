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
          :value="mcrMedia"
          :options="fileMediaOptions"
          @input="mediaChange"
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
      v-show="this.MetaData.typeSelected == 'mcr-spot'"
      style="position:absolute; width:550px; top:90px; height: 210px;
    border: 1px solid #008ecc;"
    >
      <!-- //TODO: Data Binding -->
      <DxDataGrid
        name="mcrDxDataGrid"
        v-show="this.EventData.id != ''"
        style="height:208px;"
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
        <DxColumn data-field="name" caption="이벤트 명" />
        <DxColumn data-field="id" caption="이벤트 ID" />
      </DxDataGrid>
    </div>
    <!-- 프로그램 -->
    <div
      v-show="!isActive && EventSelected.id != ''"
      style="width: 550px; height:110px; margin-top:280px; padding-top:10px; padding-left:10px; padding-right:10px; float:left; border:1px solid #008ecc;"
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
            v-model="EventSelected.name"
            readonly
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width:170px; margin-left:20px; float:left;">
        <b-form-group
          label="이벤트 ID"
          class="has-float-label"
          style="margin-top:20px;"
        >
          <b-form-input
            style="width:170px;"
            class="editTask"
            v-model="EventSelected.id"
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
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
import axios from "axios";
export default {
  components: {
    CommonVueSelect
  },
  mixins: [CommonFileFunction, MixinBasicPage],
  data() {
    return {
      mcrMedia: "A"
    };
  },
  created() {
    this.reset();
    this.getEditorForPd();
    this.resetFileMediaOptions();

    axios.get("/api/categories/media/mcrspot").then(res => {
      res.data.resultObject.data.forEach(e => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name
        });
      });
    });
    this.mcrMedia = "A";
    this.setMediaSelected("A");
  },
  computed: {
    ...mapState("FileIndexStore", {
      MetaModalTitle: state => state.MetaModalTitle,
      localFiles: state => state.localFiles,
      MetaData: state => state.MetaData,
      vueTableData: state => state.vueTableData,
      ProgramData: state => state.ProgramData,
      EventData: state => state.EventData,
      isActive: state => state.isActive,
      processing: state => state.processing,
      fileUploading: state => state.fileUploading
    }),
    ...mapGetters("FileIndexStore", [
      "typeState",
      "titleState",
      "memoState",
      "editorState",
      "metaValid"
    ]),
    ...mapGetters("user", ["getMenuGrpName"]),
    getVariant() {
      return this.isActive ? "outline-dark" : "outline-primary";
    }
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
