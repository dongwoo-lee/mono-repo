<template>
  <div>
    <b-form-group
      label="방송일"
      class="has-float-label"
      style="margin-top:20px;font-color:black;"
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
      style="position:absolute; right:160px; top:140px;  z-index:9999; width:3px;  background-color:#FFFFFF; border:0; outline:0;"
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
        :disabled="isActive"
        id="program-media"
        class="media-select"
        style=" width:200px; height:37px;"
        v-model="this.MetaData.proMediaSelected"
        :options="this.proMediaOptions"
      />
    </b-form-group>

    <b-form-group
      label="제작자"
      class="has-float-label"
      style="margin-top:10px;"
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
        :state="memoState"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="소재 명"
        trim
      />
    </div>
    <div style="height:50px;">
      <b-form-input
        class="editTask"
        v-model="MetaData.title"
        :state="memoState"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="사용처 명"
        trim
      />
    </div>
    <div style="height:50px;">
      <b-form-input
        class="editTask"
        v-model="MetaData.title"
        :state="memoState"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="광고주 명"
        trim
      />
    </div>
  </div>
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
import CommonVueSelect from "../../../components/Form/CommonVueSelect.vue";
import { mapState, mapGetters } from "vuex";
export default {
  // props: {
  //   mcrMediaOptions: {
  //     type: [],
  //     default: []
  //   }
  // },
  components: {
    CommonVueSelect
  },
  mixins: [CommonFileFunction],
  computed: {
    ...mapState("FileIndexStore", {
      MetaModalTitle: state => state.MetaModalTitle,
      localFiles: state => state.localFiles,
      MetaData: state => state.MetaData,
      connectionId: state => state.connectionId,
      vueTableData: state => state.vueTableData,
      ProgramData: state => state.ProgramData,
      EventData: state => state.EventData
    }),
    ...mapGetters("FileIndexStore", [
      "typeState",
      "titleState",
      "memoState",
      "editorState",
      "metaValid"
    ]),
    ...mapGetters("user", ["getMenuGrpName"])
  }
};
</script>

<style></style>
