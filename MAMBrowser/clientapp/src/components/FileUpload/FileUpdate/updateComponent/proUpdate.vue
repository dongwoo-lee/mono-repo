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
      if (v == 0) {
        this.proType = 0;
        this.proTypeName = "Title Music";
      } else if (v == 1) {
        this.proType = 1;
        this.proTypeName = "Effect";
      } else if (v == 2) {
        this.proType = 2;
        this.proTypeName = "Cut";
      } else if (v == 3) {
        this.proType = 3;
        this.proTypeName = "모음1";
      } else if (v == 4) {
        this.proType = 4;
        this.proTypeName = "모음2";
      } else if (v == 5) {
        this.proType = 5;
        this.proTypeName = "모음3";
      } else if (v == 6) {
        this.proType = 6;
        this.proTypeName = "모음4";
      } else if (v == 7) {
        this.proType = 7;
        this.proTypeName = "모음5";
      } else if (v == 8) {
        this.proType = 8;
        this.proTypeName = "모음6";
      } else if (v == 9) {
        this.proType = 9;
        this.proTypeName = "모음7";
      } else if (v == 10) {
        this.proType = 1;
        this.proTypeName = "모음8";
      } else if (v == 11) {
        this.proType = 1;
        this.proTypeName = "모음9";
      } else if (v == 12) {
        this.proType = 1;
        this.proTypeName = "모음10";
      }
    },
    update() {
      var meta = {
        ID: this.rowData.id,
        name: this.name,
        category: this.proMedia,
        proType: this.proType,
        ProTypeName: this.proTypeName,
      };
      this.$emit("updateProMeta", meta);
    },
  },
};
</script>

<style></style>
