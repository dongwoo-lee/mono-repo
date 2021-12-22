<template>
  <common-report-modal @close="reportModalOff">
    <h3 slot="header">사용처 수정</h3>

    <h4 slot="body">
      <div style="margin-left: 25px; margin-top: 20px">
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
      </div>
      <br />
      <br />
      <div style="margin-top: 20px; margin-left: 25px">
        <DxDataGrid
          style="
            width: 350px;
            height: 200px;
            border: 1px solid silver;
            font-family: 'MBC 새로움 M';
          "
          :data-source="pgmData"
          :selection="{ mode: 'single' }"
          :show-borders="true"
          :hover-state-enabled="true"
          key-expr="id"
          :allow-column-resizing="true"
          :column-auto-width="true"
          no-data-text="No Data"
          @row-click="onRowClick"
        >
          <DxLoadPanel :enabled="true" />
          <DxScrolling mode="virtual" />
          <DxColumn data-field="name" caption="이벤트 명" />
          <DxColumn data-field="id" caption="이벤트 ID" />
        </DxDataGrid>
        <div style="width: 180px; margin-top: 10px; float: left">
          <b-form-group
            label="이벤트 명"
            class="has-float-label"
            style="margin-top: 20px"
          >
            <b-form-input
              style="width: 180px"
              class="editTask"
              v-model="this.pgmName"
              disabled
              aria-describedby="input-live-help input-live-feedback"
              trim
            />
          </b-form-group>
        </div>
        <div
          style="width: 170px; margin-left: 20px; margin-top: 10px; float: left"
        >
          <b-form-group
            label="이벤트 ID"
            class="has-float-label"
            style="margin-top: 20px"
          >
            <b-form-input
              style="width: 150px"
              class="editTask"
              v-model="this.pgmid"
              disabled
              aria-describedby="input-live-help input-live-feedback"
              trim
            />
          </b-form-group>
        </div>
      </div>
    </h4>

    <h3 slot="footer">
      <b-button
        variant="outline-primary"
        @click="selectPgm"
        style="margin-right: 10px"
      >
        <span class="label">수정</span>
      </b-button>
      <b-button
        variant="outline-danger"
        @click="reportModalOff"
        style="margin-right: 10px"
      >
        <span class="label">닫기</span>
      </b-button>
    </h3>
  </common-report-modal>
</template>

<script>
import {
  DxDataGrid,
  DxColumn,
  DxScrolling,
  DxLoadPanel,
} from "devextreme-vue/data-grid";
import CommonReportModal from "../../../Modal/CommonReportModal.vue";
import axios from "axios";
export default {
  components: {
    CommonReportModal,
    DxDataGrid,
    DxColumn,
    DxScrolling,
    DxLoadPanel,
  },
  props: {
    rowData: {
      type: [],
      default: "",
    },
  },
  data() {
    return {
      pgmData: [],
      pgmid: "",
      pgmName: "",
      tempDate: this.convertDateSTH(this.rowData.brdDT),
      brdDT: this.convertDateSTH(this.rowData.brdDT),
    };
  },
  created() {
    this.getEvent();
  },
  methods: {
    log() {
      console.log("this.brdDT :>> ", this.brdDT);
      console.log("this.pgmid :>> ", this.pgmid);
      console.log("this.pgmName :>> ", this.pgmName);
    },
    getEvent() {
      const replaceVal = this.brdDT.replace(/-/g, "");
      const yyyy = replaceVal.substring(0, 4);
      const mm = replaceVal.substring(4, 6);
      const dd = replaceVal.substring(6, 8);
      var date = yyyy + "" + mm + "" + dd;
      this.pgmData = [];
      axios.get(`/api/categories/pgmcodes?brd_dt=${date}`).then((res) => {
        this.pgmData = res.data.resultObject.data;
      });
    },
    reportModalOff() {
      this.$emit("reportModalClose");
    },
    selectPgm() {
      var pgm = {
        pgmid: this.pgmid,
        pgmName: this.pgmName,
      };
      this.$emit("changePgm", pgm);
      this.reportModalOff();
    },
    onRowClick(v) {
      this.pgmid = v.data.id;
      this.pgmName = v.data.name;
    },
    resetPgm() {
      this.pgmid = "";
      this.pgmName = "";
    },
    eventInput(event) {
      this.brdDT = event;
      this.tempDate = event;
      this.getEvent();
      this.resetPgm();
      this.$emit("changeBrdDT", event);
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
            this.getEvent();
            this.resetPgm();
            this.$emit("changeBrdDT", this.convertDateSTH(this.rowData.brdDT));
            return;
          }
          this.brdDT = convertDate;
          this.tempDate = convertDate;
          this.getEvent();
          this.resetPgm();
          this.$emit("changeBrdDT", convertDate);
        }
      }
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
