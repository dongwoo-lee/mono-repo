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
    <b-form-select
      style="width: 350px"
      class="media-select"
      :value="category"
      :options="mediaOptions"
      @input="changeMedia"
    />
    <br />
    <br />
    <h6>방송유효일</h6>
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
    <h6>메모</h6>
    <b-form-input
      class="editTask"
      :value="rowData.memo"
      @input="changeMemo"
      aria-describedby="input-live-help input-live-feedback"
      placeholder="메모"
      trim
    />
  </div>
</template>

<script>
import axios from "axios";
export default {
  components: {},
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
      date: this.convertDateSTH(this.rowData.brdDT),
      brdDT: this.convertDateSTH(this.rowData.brdDT),
      name: this.rowData.name,
      mediaOptions: [],
    };
  },
  created() {
    axios.get(`/api/Categories/filler/${this.fillerType}`).then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this.mediaOptions.push({
          value: e.id,
          text: e.name,
        });
      });
    });
    console.log(this.rowData);
  },
  methods: {
    changeMedia(v) {
      this.media = v;
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
    update() {
      var meta = {
        media: this.media,
        reporter: this.reporter,
        brdDT: this.brdDT.replace(/-/g, ""),
        pgmName: this.pgmName,
        pgmid: this.pgmid,
        name: this.name,
      };
      this.$emit("updateFillerMeta", meta);
    },
    eventInput(event) {
      this.brdDT = event;
      this.date = event;
    },
    onInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");
      if (this.validDateType(targetValue)) {
        event.target.value = this.date;

        this.$fn.notify("error", { message: "날짜 형식 오류입니다." });
        return;
      }

      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          var convertDate = this.convertDateSTH(replaceAllTargetValue);
          this.brdDT = convertDate;
          this.date = convertDate;
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
        this.$fn.notify("error", { message: "날짜 형식 오류" });
        this.brdDT = "";
      } else if (31 < dd) {
        this.$fn.notify("error", { message: "날짜 형식 오류" });
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
