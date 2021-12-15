<template>
  <div>
    <h6>소재명</h6>
    <b-form-input
      class="editTask"
      :value="rowData.name"
      @input="changeName"
      aria-describedby="input-live-help input-live-feedback"
      placeholder="소재명"
      trim
    />
    <br />
    <h6>분류</h6>
    <common-vue-select
      class="h300"
      style="width: 350px; font-size: 14px"
      :value="proMedia"
      :suggestions="mediaOptions"
      @inputEvent="changeMedia"
    ></common-vue-select>

    <br />
    <h6>타입</h6>
    <b-form-select
      id="program-media"
      class="media-select"
      style="width: 350px"
      :value="proType"
      :options="proTypeOptions"
      @input="proTypeChange"
    />
  </div>
</template>

<script>
import axios from "axios";
import CommonVueSelect from "../../../Form/CommonVueSelect.vue";
export default {
  components: { CommonVueSelect },
  props: {
    rowData: {
      type: [],
      default: "",
    },
    fillerType: {
      type: String,
      default: "",
    },
  },
  data() {
    return {
      category: this.rowData.categoryID,
      memo: this.rowData.memo,
      proMedia: "",
      name: this.rowData.name,
      proType: "0",
      proTypeName: "",
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
      mediaOptions: [],
    };
  },
  created() {
    axios.get("/api/categories/pro").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this.mediaOptions.push({ id: e.id, name: e.name });
      });
    });
    this.proMedia = "AC73";
    console.log(this.rowData);
  },
  methods: {
    changeMedia(v) {
      this.proMedia = v.id;
      this.update();
    },
    changeMemo(v) {
      this.memo = v;
      this.update();
    },
    changeName(v) {
      this.name = v;
      this.update();
    },
    proTypeChange(v) {
      var data = this.proTypeOptions.find((dt) => dt.value == v);
      this.proTypeName = data.text;
      this.proType = data.value;
      this.update();
    },
    update() {
      var meta = {
        ID: this.rowData.id,
        name: this.name,
        category: this.proMedia,
        proType: this.proType,
        proTypeName: this.proTypeName,
      };
      console.log(this.proTypeName);
      this.$emit("updateProMeta", meta);
    },
  },
};
</script>

<style></style>
