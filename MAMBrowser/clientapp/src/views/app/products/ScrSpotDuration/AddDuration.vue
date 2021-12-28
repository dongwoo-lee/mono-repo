<template>
  <b-modal
    id="addRange"
    size="lg"
    v-model="show2"
    centered
    hide-header-close
    no-close-on-esc
    no-close-on-backdrop
    footer-class="scr-modal-footer"
  >
    <template slot="modal-title">
      <h5>기간 설정</h5>
    </template>
    <template slot="default">
      <b-form-group
        label="부조SPOT"
        class="has-float-label"
        style="width: 400px; float: left"
      >
        <b-form-input
          style="
            width: 243px;
            float: left;
            border-top-right-radius: 0;
            border-bottom-right-radius: 0;
            border-right: 0;
            height: 34px;
          "
          class="editTask"
          :value="this.selectedSpot.spotName"
          disabled
          aria-describedby="input-live-help input-live-feedback"
          placeholder="소재명"
          trim
        />

        <b-button
          variant="outline-primary default cutom-label"
          style="margin-top: 10px; float: left; width: 58px; height: 34px"
          @click="showSearchDuration"
        >
          <b-icon-search></b-icon-search>
        </b-button>
      </b-form-group>
      <b-form-group
        label="사용처"
        class="has-float-label"
        style="margin-top: 10px"
      >
        <common-vue-select
          :class="vSelectClass"
          style="font-size: 14px; width: 300px"
          :suggestions="ProgramOptions"
          @inputEvent="pgmSelect"
        ></common-vue-select>
      </b-form-group>

      <div style="margin-top: 30px">
        <b-form-group
          label="시작일"
          class="has-float-label"
          style="width: 300px; float: left"
        >
          <b-input-group style="width: 300px; float: left">
            <input
              style="height: 34px; font-size: 13px"
              id="sdateinput"
              type="text"
              class="form-control input-picker date-input"
              :value="setScrRangeData.StartDate"
              @input="onsInput"
            />
            <b-input-group-append>
              <b-form-datepicker
                style="height: 34px; float: left"
                :value="setScrRangeData.StartDate"
                @input="eventSInput"
                button-variant="outline-dark"
                button-only
                right
                aria-controls="example-input"
                @context="onContext"
              ></b-form-datepicker>
            </b-input-group-append>
          </b-input-group>
        </b-form-group>
        <b-form-group
          label="종료일"
          class="has-float-label"
          style="margin-left: 100px; width: 300px; font-size: 13px; float: left"
        >
          <b-input-group class="mb-3" style="width: 300px; float: left">
            <input
              style="height: 34px; font-size: 13px"
              id="edateinput"
              type="text"
              class="form-control input-picker date-input"
              :value="setScrRangeData.EndDate"
              @input="oneInput"
            />
            <b-input-group-append>
              <b-form-datepicker
                style="height: 34px"
                :value="setScrRangeData.EndDate"
                @input="eventEInput"
                button-only
                button-variant="outline-dark"
                right
                aria-controls="example-input"
                @context="onContext"
              ></b-form-datepicker>
            </b-input-group-append>
          </b-input-group>
        </b-form-group>
      </div>
    </template>
    <template v-slot:modal-footer>
      <b-button
        variant="outline-primary default cutom-label"
        size="sm"
        :disabled="!scrValid"
        class="float-right"
        @click="setScrDuration"
      >
        확인</b-button
      >
      <b-button
        variant="outline-danger default cutom-label-cancel"
        size="sm"
        class="float-right"
        @click="hideAddDuration"
      >
        취소</b-button
      >
    </template>
  </b-modal>
</template>

<script>
import CommonVueSelect from "../../../../components/Form/CommonVueSelect.vue";
import axios from "axios";
export default {
  components: {
    CommonVueSelect,
  },
  props: {
    show2: {
      type: Boolean,
      default: false,
    },
    spotData: {
      type: Object,
    },
  },
  data() {
    return {
      vSelectClass: "scrSpotVueSelect",
      setScrRangeData: {
        SpotID: "",
        spot: "",
        ProductID: null,
        StartDate: this.$fn.formatDate(new Date(), "yyyy-MM-dd"),
        EndDate: this.$fn.formatDate(new Date(), "yyyy-MM-dd"),
      },
      // 선택한 부조 스팟(row) 데이터
      selectedSpot: {
        spotID: "",
        spotName: "",
        codeID: "",
        codeName: "",
        CMOwner: "",
      },
      tempSDate: "",
      tempEDate: "",
      ProgramOptions: [],
    };
  },
  created() {
    this.getPgm();
  },
  watch: {
    spotData(v) {
      this.selectedSpot = v;
      this.setScrRangeData.spot = v.spotName;
      this.setScrRangeData.SpotID = v.spotID;
    },
  },
  computed: {
    scrValid() {
      if (
        this.setScrRangeData.SpotID != "" &&
        this.setScrRangeData.ProductID != null &&
        this.setScrRangeData.StartDate != "" &&
        this.setScrRangeData.EndDate != ""
      ) {
        return true;
      }
      return false;
    },
  },
  methods: {
    hideAddDuration() {
      this.$emit("hideAddDuration");
    },
    showSearchDuration() {
      this.$emit("showSearchDuration");
    },
    async getPgm() {
      var res = await axios.get(`/api/categories/pgmcodes`);
      this.ProgramOptions = res.data.resultObject.data;
    },
    pgmSelect(v) {
      this.setScrRangeData.ProductID = v.id;
    },
    setScrDuration() {
      this.$emit("setScrDuration", this.setScrRangeData);
      this.resetScrRangeData();
      this.hideAddDuration();
    },
    resetScrRangeData() {
      this.setScrRangeData = {
        SpotID: "",
        spot: "",
        ProductID: null,
        StartDate: this.$fn.formatDate(new Date(), "yyyy-MM-dd"),
        EndDate: this.$fn.formatDate(new Date(), "yyyy-MM-dd"),
      };
    },
    get7daysago() {
      var newDate = new Date();
      var dayOfMonth = newDate.getDate();
      newDate.setDate(dayOfMonth - 7);
      newDate = this.$fn.formatDate(newDate, "yyyy-MM-dd");
      return newDate;
    },
    eventSInput(value) {
      this.setScrRangeData.StartDate = value;
      this.tempSDate = value;

      const replaceAllFileSDate = this.setScrRangeData.StartDate.replace(
        /-/g,
        ""
      );
      const replaceAllFileEDate = this.setScrRangeData.EndDate.replace(
        /-/g,
        ""
      );
      if (
        replaceAllFileEDate < replaceAllFileSDate &&
        replaceAllFileEDate != ""
      ) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        return;
      }
    },
    eventEInput(value) {
      this.setScrRangeData.EndDate = value;
      this.tempEDate = value;

      const replaceAllFileSDate = this.setScrRangeData.StartDate.replace(
        /-/g,
        ""
      );
      const replaceAllFileEDate = this.setScrRangeData.EndDate.replace(
        /-/g,
        ""
      );
      if (replaceAllFileEDate < replaceAllFileSDate) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        return;
      }
    },

    onsInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.tempSDate == null) {
          event.target.value = this.get7daysago();
          return;
        }
        event.target.value = this.tempSDate;
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
            event.target.value = this.get7daysago();
            this.setScrRangeData.StartDate = this.get7daysago();
            this.tempSDate = this.get7daysago();

            const replaceAllFileSDate = this.setScrRangeData.StartDate.replace(
              /-/g,
              ""
            );
            const replaceAllFileEDate = this.setScrRangeData.EndDate.replace(
              /-/g,
              ""
            );
            if (
              replaceAllFileEDate < replaceAllFileSDate &&
              replaceAllFileEDate != ""
            ) {
              this.$fn.notify("error", {
                message: "시작 날짜가 종료 날짜보다 큽니다.",
              });
              return;
            }
            return;
          }
          this.setScrRangeData.StartDate = convertDate;
          this.tempSDate = convertDate;
          const replaceAllFileSDate = this.setScrRangeData.StartDate.replace(
            /-/g,
            ""
          );
          const replaceAllFileEDate = this.setScrRangeData.EndDate.replace(
            /-/g,
            ""
          );
          if (
            replaceAllFileEDate < replaceAllFileSDate &&
            replaceAllFileEDate != ""
          ) {
            this.$fn.notify("error", {
              message: "시작 날짜가 종료 날짜보다 큽니다.",
            });
            return;
          }
        }
      }
    },
    oneInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.tempEDate == null) {
          event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
          return;
        }
        event.target.value = this.tempEDate;
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
            event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
            this.setScrRangeData.EndDate = this.$fn.formatDate(
              new Date(),
              "yyyy-MM-dd"
            );
            this.tempEDate = this.$fn.formatDate(new Date(), "yyyy-MM-dd");

            const replaceAllFileSDate = this.setScrRangeData.StartDate.replace(
              /-/g,
              ""
            );
            const replaceAllFileEDate = this.setScrRangeData.EndDate.replace(
              /-/g,
              ""
            );
            if (replaceAllFileEDate < replaceAllFileSDate) {
              this.$fn.notify("error", {
                message: "시작 날짜가 종료 날짜보다 큽니다.",
              });
              return;
            }
            return;
          }
          this.setScrRangeData.EndDate = convertDate;
          this.tempEDate = convertDate;

          const replaceAllFileSDate = this.setScrRangeData.StartDate.replace(
            /-/g,
            ""
          );
          const replaceAllFileEDate = this.setScrRangeData.EndDate.replace(
            /-/g,
            ""
          );
          if (replaceAllFileEDate < replaceAllFileSDate) {
            this.$fn.notify("error", {
              message: "시작 날짜가 종료 날짜보다 큽니다.",
            });
            return;
          }
        }
      }
    },
    validDateType(value) {
      const dateRegex = /^(\d{0,4})[-]?\d{0,2}[-]?\d{0,2}$/;
      return !dateRegex.test(value);
    },
    onContext(ctx) {
      // The date formatted in the locale, or the `label-no-date-selected` string
      this.formatted = ctx.selectedFormatted;
      // The following will be an empty string until a valid date is entered
      this.dateSelected = ctx.selectedYMD;
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
