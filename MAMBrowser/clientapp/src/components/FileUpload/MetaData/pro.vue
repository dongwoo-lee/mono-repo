<template>
  <div style="margin-top: 40px">
    <b-form-group label="분류" class="has-float-label" style="font-size: 16px">
      <common-vue-select
        class="h270"
        style="width: 425px; height: 36px; font-size: 14px"
        :value="proMetaData.category"
        :suggestions="proCategoryOptions"
        @inputEvent="categoryChanged"
      ></common-vue-select>
    </b-form-group>

    <b-form-group
      label="타입"
      class="has-float-label"
      style="font-size: 16px; margin-top: 27px"
    >
      <b-form-select
        id="program-media"
        class="media-select"
        style="width: 425px"
        :value="proType"
        :options="proTypeOptions"
        @input="proTypeChange"
      />
    </b-form-group>
    <div style="height: 50px">
      <b-form-group
        label="소재"
        class="has-float-label"
        style="font-size: 16px; margin-top: 27px"
      >
        <b-form-input
          class="editTask"
          v-model="proMetaData.title"
          :state="proTitleState"
          :maxlength="30"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="소재"
          trim
        />
      </b-form-group>
      <p
        v-show="proTitleState"
        style="
          position: relative;
          left: 390px;
          top: -15px;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ proMetaData.title.length }}/30
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
          v-model="proMetaData.memo"
          :maxLength="30"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="메모"
          trim
        />
      </b-form-group>
      <p
        v-show="proMemoState"
        style="
          position: relative;
          left: 390px;
          top: -15px;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ proMetaData.memo.length }}/30
      </p>
    </div>
  </div>
</template>

<script>
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
import axios from "axios";
export default {
  components: {
    CommonVueSelect,
  },
  data() {
    return {
      role: "",
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
  async created() {
    this.RESET_PRO();
    this.role = sessionStorage.getItem("authority");
    this.SET_PRO_TITLE(this.sliceExt(30));
    this.RESET_PRO_CATEGORY_OPTIONS();

    var res = await axios.get("/api/categories/pro");

    res.data.resultObject.data.forEach((e) => {
      this.SET_PRO_CATEGORY_OPTIONS({
        id: e.id,
        name: e.name,
      });
    });

    this.SET_PRO_CATEGORY(this.proCategory);
    this.SET_PRO_TYPE(0);
    this.SET_PRO_TYPE_NAME("Title Music");
  },
  computed: {
    ...mapState("FileIndexStore", {
      MetaModalTitle: (state) => state.MetaModalTitle,
      proMetaData: (state) => state.proMetaData,
      proCategoryOptions: (state) => state.proCategoryOptions,
    }),
    ...mapGetters("FileIndexStore", ["proTitleState", "proMemoState"]),
  },
  methods: {
    ...mapMutations("FileIndexStore", [
      "SET_PRO_TITLE",
      "SET_PRO_CATEGORY",
      "SET_PRO_TYPE",
      "SET_PRO_TYPE_NAME",
      "SET_PRO_CATEGORY_OPTIONS",
      "RESET_PRO_CATEGORY_OPTIONS",
      "RESET_PRO",
    ]),
    categoryChanged(v) {
      this.SET_PRO_CATEGORY(v.id);
    },
    proTypeChange(v) {
      var data = this.proTypeOptions.find((dt) => dt.value == v);
      if (data) {
        this.SET_PRO_TYPE(data.value);
        this.SET_PRO_TYPE_NAME(data.text);
      }
    },
    sliceExt(maxLength) {
      var result = this.MetaModalTitle.replace(/(.wav|.mp3)$/, "");
      return result.substring(0, maxLength);
    },
  },
};
</script>

<style></style>
