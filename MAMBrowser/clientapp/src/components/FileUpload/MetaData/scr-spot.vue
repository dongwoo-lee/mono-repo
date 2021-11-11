<template>
  <div>
    <b-form-group
      label="방송일"
      class="has-float-label"
      style="margin-top:20px; margin-bottom:0px;"
    >
      <b-input-group class="mb-3" style="width:300px; float:left;">
        <input
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
            button-variant="outline-primary"
            right
            aria-controls="example-input"
            @context="onContext"
          ></b-form-datepicker>
        </b-input-group-append>
      </b-input-group>
    </b-form-group>
    <button
      v-show="!isActive"
      style="position:absolute; right:330px; top:50px;  z-index:9999; width:3px;  background-color:#FFFFFF; border:0; outline:0;"
    >
      <b-icon
        icon="x-circle"
        font-scale="1"
        style="position:absolute; z-index:9999;"
        variant="secondary"
        @click="resetDate"
      ></b-icon>
    </button>

    <b-form-group label="매체" class="has-float-label">
      <b-form-select
        id="program-media"
        class="media-select"
        style=" width:200px; height:37px;"
        v-model="this.MetaData.proMediaSelected"
        :options="fileMediaOptions"
      />
    </b-form-group>

    <b-form-group
      label="제작자"
      class="has-float-label"
      style="margin-top:20px;"
    >
      <common-vue-select
        style="font-size:14px; width:200px; border: 1px solid #008ecc;"
        :suggestions="editorOptions"
        @inputEvent="inputEditor"
      ></common-vue-select>
    </b-form-group>

    <div style="height:50px;">
      <b-form-input
        class="editTask"
        v-model="MetaData.title"
        :state="titleState"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="소재 명"
        trim
      />
      <button
        v-show="titleState"
        style="position:relative; left:315px; top:-27px; z-index:99; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
      >
        <b-icon
          icon="x-circle"
          font-scale="1"
          style="position:relative; top:0px; right:0px; z-index:999;"
          variant="secondary"
          @click="resetTitle"
        ></b-icon>
      </button>
    </div>
    <div style="height:50px;">
      <b-form-input
        class="editTask"
        v-model="MetaData.usage"
        :state="usageState"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="사용처 명"
        trim
      />
      <button
        v-show="usageState"
        style="position:relative; left:315px; top:-27px; z-index:99; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
      >
        <b-icon
          icon="x-circle"
          font-scale="1"
          style="position:relative; top:0px; right:0px; z-index:999;"
          variant="secondary"
          @click="resetUsage"
        ></b-icon>
      </button>
    </div>
    <div style="height:50px;">
      <b-form-input
        class="editTask"
        v-model="MetaData.advertiser"
        :state="advertiserState"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="광고주 명"
        trim
      />
      <button
        v-show="advertiserState"
        style="position:relative; left:315px; top:-27px; z-index:99; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
      >
        <b-icon
          icon="x-circle"
          font-scale="1"
          style="position:relative; top:0px; right:0px; z-index:999;"
          variant="secondary"
          @click="resetAdvertiser"
        ></b-icon>
      </button>
    </div>
  </div>
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CommonVueSelect from "../../../components/Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
export default {
  components: {
    CommonVueSelect
  },
  mixins: [CommonFileFunction, MixinBasicPage],
  created() {
    this.getEditorForPd();
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
      "usageState",
      "advertiserState",
      "metaValid"
    ]),
    ...mapGetters("user", ["getMenuGrpName"])
  },
  methods: {
    ...mapMutations("FileIndexStore", ["setEditor"]),
    inputEditor(v) {
      this.setEditor(v.id);
    }
  }
};
</script>

<style></style>
