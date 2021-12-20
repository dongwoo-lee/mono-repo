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
    <h6>방송일</h6>
    <b-input-group class="mb-3" style="width: 350px; float: left">
      <input
        type="text"
        class="form-control input-picker"
        :value="brdDT"
        @input="onInput"
      />
      <b-input-group-append>
        <b-form-datepicker
          :value="brdDT"
          @input="eventInput"
          button-only
          button-variant="outline-dark"
          right
          aria-controls="example-input"
          @context="onContext"
        ></b-form-datepicker>
      </b-input-group-append>
    </b-input-group>
    <br />
    <br />
    <br />
    <h6>취재인</h6>
    <b-form-input
      class="editTask"
      :value="rowData.reporter"
      @input="changeReporter"
      aria-describedby="input-live-help input-live-feedback"
      placeholder="소재명"
      trim
    />

    <br />
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
    <h6>사용처</h6>
    <b-form-input
      style="width: 265px; float: left"
      class="editTask"
      :value="this.pgmName"
      disabled
      aria-describedby="input-live-help input-live-feedback"
      placeholder="소재명"
      trim
    />
    <b-button style="margin-left: 20px; margin-top: 10px" @click="getPro"
      >검색</b-button
    >
    <report-modal
      v-if="reportModal"
      :pgmData="pgmData"
      @reportModalClose="reportModalOff"
      @changePgm="changePgm"
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
      tempDate: this.convertDateSTH(this.rowData.brdDT),
      brdDT: this.convertDateSTH(this.rowData.brdDT),
      pgmName: this.rowData.pgmName,
      pgmid: this.rowData.pgmid,
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

    const replaceVal = this.brdDT.replace(/-/g, "");
    const yyyy = replaceVal.substring(0, 4);
    const mm = replaceVal.substring(4, 6);
    const dd = replaceVal.substring(6, 8);
    var date = yyyy + "" + mm + "" + dd;
    this.pgmData = [];
    axios.get(`/api/categories/pgmcodes?brd_dt=${date}`).then((res) => {
      this.pgmData = res.data.resultObject.data;
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
    eventInput(event) {
      this.brdDT = event;
      this.tempDate = event;
      this.update();
    },
    onInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");
      if (this.validDateType(targetValue)) {
        if (this.tempDate == null) {
          event.target.value = this.convertDateSTH(this.rowData.brdDT);
          return;
        }

        event.target.value = this.tempDate;

        return;
      }

      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          if (
            convertDate == "" ||
            convertDate == null ||
            convertDate == "undefined"
          ) {
            this.brdDT = this.convertDateSTH(this.rowData.brdDT);
            this.tempDate = this.convertDateSTH(this.rowData.brdDT);
            return;
          }
          this.brdDT = convertDate;
          this.tempDate = convertDate;
        }
      }
      this.update();
    },
    validDateType(value) {
      const dateRegex = /^(\d{0,4})[-]?\d{0,2}[-]?\d{0,2}$/;
      return !dateRegex.test(value);
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
    onContext(ctx) {
      // The date formatted in the locale, or the `label-no-date-selected` string
      this.formatted = ctx.selectedFormatted;
      // The following will be an empty string until a valid date is entered
      this.dateSelected = ctx.selectedYMD;
    },
  },
};
</script>

<style></style>
