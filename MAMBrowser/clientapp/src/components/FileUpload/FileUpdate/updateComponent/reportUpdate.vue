<template>
  <div>
    <h6>분류</h6>
    <b-form-select
      style="width: 350px"
      class="media-select"
      :value="media"
      :options="mediaOptions"
      @input="changeMedia"
    />
    <br />

    <br />
    <h6>취재인</h6>
    <b-form-input
      class="editTask"
      :value="rowData.reporter"
      @input="changeReporter"
      :maxLength="10"
      aria-describedby="input-live-help input-live-feedback"
      placeholder="취재인"
      trim
    />
    <p
      v-if="this.reporter != null"
      style="font-size: 14px; text-align: right; margin-right: 35px"
    >
      {{ this.reporter.length }}/10
    </p>
    <h6>소재명</h6>
    <b-form-input
      class="editTask"
      :value="rowData.name"
      @input="changeName"
      :maxLength="30"
      aria-describedby="input-live-help input-live-feedback"
      placeholder="소재명"
      trim
    />
    <p
      v-if="this.name != null"
      style="font-size: 14px; text-align: right; margin-right: 35px"
    >
      {{ this.name.length }}/30
    </p>
    <h6>방송일</h6>
    <b-form-input
      style="width: 350px"
      class="editTask"
      :value="this.brdDT"
      disabled
      aria-describedby="input-live-help input-live-feedback"
      placeholder="방송일"
      trim
    />
    <br />
    <h6>사용처</h6>
    <b-form-input
      style="width: 265px; float: left"
      class="editTask"
      :value="this.pgmName"
      disabled
      aria-describedby="input-live-help input-live-feedback"
      placeholder="사용처"
      trim
    />

    <b-button
      variant="outline-primary"
      style="margin-left: 20px; margin-top: 10px"
      @click="getPro"
      >검색</b-button
    >

    <report-modal
      v-if="reportModal"
      :rowData="rowData"
      @reportModalClose="reportModalOff"
      @changePgm="changePgm"
      @changeBrdDT="changeBrdDT"
    ></report-modal>
  </div>
</template>

<script>
import axios from "axios";
import ReportModal from "./reportModal.vue";
export default {
  components: {
    ReportModal,
  },
  props: {
    rowData: {
      type: [],
      default: "",
    },
  },
  data() {
    return {
      reportModal: false,
      media: this.rowData.categoryName,
      reporter: this.rowData.reporter,
      pgmName: this.rowData.pgmName,
      pgmid: this.rowData.pgmid,
      brdDT: this.convertDateSTH(this.rowData.brdDT),
      name: this.rowData.name,
      mediaOptions: [],
      pgmData: [
        {
          name: "",
          id: "",
        },
      ],
      pgmSelected: {},
    };
  },
  created() {
    axios.get("/api/categories/report").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        if (e.name == this.rowData.categoryName) {
          this.media = e.id;
        }
        this.mediaOptions.push({
          value: e.id,
          text: e.name,
        });
      });
    });

    this.update();
  },
  methods: {
    getPro() {
      this.reportModal = true;
    },
    reportModalOff() {
      this.reportModal = false;
    },
    changeMedia(v) {
      this.media = v;
      this.update();
    },
    changeReporter(v) {
      this.reporter = v;
      this.update();
    },
    changeName(v) {
      this.name = v;
      this.update();
    },
    changePgm(v) {
      this.pgmName = v.pgmName;
      this.pgmid = v.pgmid;
      this.update();
    },
    changeBrdDT(v) {
      this.brdDT = v;
      this.update();
    },
    update() {
      var meta = {
        ID: this.rowData.id,
        category: this.media,
        reporter: this.reporter,
        brdDT: this.brdDT.replace(/-/g, ""),
        pgmName: this.pgmName,
        pgmid: this.pgmid,
        name: this.name,
      };
      this.$emit("updateReportMeta", meta);
    },
    convertDateSTH(value) {
      const replaceVal = value.replace(/-/g, "");
      const yyyy = replaceVal.substring(0, 4);
      const mm = replaceVal.substring(4, 6);
      const dd = replaceVal.substring(6, 8);
      if (12 < mm) {
        this.brdDT = "";
      } else if (31 < dd) {
        this.brdDT = "";
      } else {
        return `${yyyy}-${mm}-${dd}`;
      }
    },
  },
};
</script>

<style></style>
