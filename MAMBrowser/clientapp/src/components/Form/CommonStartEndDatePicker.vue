<template>
  <div style="margin-left: 20px">
    <div class="row" style="margin-right: 0">
      <b-form-group :label="startDateLabel" class="has-float-label">
        <!-- 등록일: 시작일 -->
        <common-date-picker
          :value="sDate"
          :dayAgo="startDayAgo"
          :monthAgo="startMonthAgo"
          :yearAgo="startYearAgo"
          :isCurrentDate="isCurrentDate"
          :required="required"
          :disabVal="disabVal"
          @input="onSInput"
          @commonDateEvent="commonDateEvent"
        />
      </b-form-group>
      <!-- 등록일: 종료일 -->
      <b-form-group :label="endDateLabel" class="has-float-label">
        <common-date-picker
          :value="eDate"
          :isCurrentDate="isCurrentDate"
          :maxDate="maxDate"
          :required="required"
          @input="onEInput"
          @commonDateEvent="commonDateEvent"
        />
      </b-form-group>
    </div>
    <div v-if="isPeriodDate()" style="position: relative; margin: 5px 0 0 5px">
      {{ getPeriodDescription() }}
    </div>
  </div>
</template>
<script>
import { MAXIMUM_SEARCH_DATE } from "@/constants/config";
export default {
  props: {
    pageProps: {
      type: String,
    },
    startDate: {
      type: String,
      default: "",
    },
    endDate: {
      type: String,
      default: "",
    },
    startDateLabel: {
      type: String,
      default: "시작일",
    },
    endDateLabel: {
      type: String,
      default: "종료일",
    },
    startDayAgo: {
      type: Number,
      defaut: 0,
    },
    startMonthAgo: {
      type: Number,
      defaut: 0,
    },
    startYearAgo: {
      type: Number,
      defaut: 0,
    },
    maxPeriodDay: {
      type: Number,
      default: 0,
    },
    maxPeriodMonth: {
      type: Number,
      default: 0,
    },
    maxPeriodYear: {
      type: Number,
      default: 0,
    },
    isCurrentDate: {
      type: Boolean,
      default: true,
    },
    required: {
      type: Boolean,
      default: true,
    },
    disabVal: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      sDate: "",
      eDate: "",
      maxDate: "",
    };
  },
  watch: {
    startDate: {
      handler(date) {
        this.sDate = date;
        this.checkMaxPeriodDate(date);
      },
      immediate: true,
    },
    endDate: {
      handler(date) {
        this.eDate = date;
      },
      immediate: true,
    },
  },
  methods: {
    commonDateEvent() {
      this.$emit("SEDateEvent");
    },
    onSInput(v) {
      this.$emit("update:startDate", v);
      if (v == "") {
        this.$emit("SEDateNullEvent");
      }
    },
    onEInput(v) {
      this.$emit("update:endDate", v);
      if (v == "") {
        this.$emit("SEDateNullEvent");
      }
      if (this.sDate != "" && v < this.sDate) {
        this.$emit("SDateError");
        return;
      }
      if (!this.required && v == "") {
        return;
      }
      this.$emit("SEDateEvent");
    },
    checkMaxPeriodDate(selectDate) {
      if (!selectDate) {
        return;
      }
      // 선택된 날짜 + periodDate
      const formatStartDate = this.$fn.dateStringTohaipun(selectDate);
      const periodDate = new Date(Date.parse(formatStartDate));
      if (this.maxPeriodDay > 0) {
        periodDate.setDate(periodDate.getDate() + this.maxPeriodDay);
      } else if (this.maxPeriodMonth > 0) {
        periodDate.setMonth(periodDate.getMonth() + this.maxPeriodMonth);
      } else if (this.maxPeriodYear > 0) {
        periodDate.setFullYear(periodDate.getFullYear() + this.maxPeriodYear);
      } else if (!this.required) {
        this.$emit("SEDateEvent");
        return;
      }

      // 종료일 확인
      const formatEndDate = this.$fn.dateStringTohaipun(this.endDate);
      const endDate = new Date(Date.parse(formatEndDate));

      // 날짜 비교
      if (endDate > periodDate) {
        this.$emit("update:endDate", this.$fn.formatDate(periodDate));
      }
      this.maxDate = periodDate;
      if (this.eDate != "") {
        if (this.sDate > this.eDate) {
          this.$emit("SDateError");
          return;
        }
      }
      this.$emit("SEDateEvent");
    },
    isPeriodDate() {
      return (
        this.maxPeriodYear > 0 ||
        this.maxPeriodMonth > 0 ||
        this.maxPeriodDay > 0
      );
    },
    getPeriodDescription() {
      if (!this.isPeriodDate()) return "";

      let dateString = "";
      if (this.maxPeriodYear > 0) {
        dateString = `${this.maxPeriodYear}년`;
      } else if (this.maxPeriodMonth > 0) {
        dateString = `${this.maxPeriodMonth}개월`;
      } else if (this.maxPeriodDay > 0) {
        dateString = `${this.maxPeriodDay}일`;
      }
      return `※시작일로부터 최대 ${dateString}이내 선택 가능합니다.`;
    },
  },
};
</script>
<style>
.bg-light {
  background-color: #d4d4d4 !important;
}
</style>
