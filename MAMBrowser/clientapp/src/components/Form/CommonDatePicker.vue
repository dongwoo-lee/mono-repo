<template>
  <b-form @submit.prevent>
    <b-input-group :class="groupClass">
      <!-- 입력-->
      <input
        type="text"
        class="form-control input-picker"
        :placeholder="placeHolder"
        :value="inputValue | yyyyMMdd"
        @input="onInput"
      />
      <!-- 데이터 피커 -->
      <b-input-group-append>
        <b-form-datepicker
          v-model="date"
          button-only
          left
          aria-controls="example-input"
          button-variant="secondary default"
          :label-next-month="labelNextMonth"
          today-variant
          :hide-header="hideHeader"
          :size="size"
          :max="maxDate"
        />
      </b-input-group-append>
    </b-input-group>
  </b-form>
</template>

<script>
import { MINIMUM_DATE } from "@/constants/config";

export default {
  props: {
    value: {
      type: String,
      default: ""
    },
    groupClass: {
      type: String,
      default: ""
    },
    hideHeader: {
      type: Boolean,
      default: true
    },
    placeHolder: {
      type: String,
      default: "YYYY-MM-DD"
    },
    labelNextMonth: {
      type: String,
      default: "다음달"
    },
    size: {
      type: String,
      default: "sm"
    },
    isCurrentDate: {
      type: Boolean,
      default: true
    },
    yearAgo: {
      type: Number,
      defaut: 0
    },
    monthAgo: {
      type: Number,
      defaut: 0
    },
    dayAgo: {
      type: Number,
      defaut: 0
    },
    required: {
      type: Boolean,
      default: false
    },
    checkMinValue: {
      type: Boolean,
      default: false
    },
    maxDate: null
  },
  data() {
    return {
      date: "",
      inputValue: "",
      validBeforeDate: this.getValidBeforeDate()
      // minDate: MINIMUM_DATE,
      // maxDate: this.$fn.getMaxDate()
    };
  },
  created() {
    if (this.dayAgo > 0) {
      const newDate = new Date();
      newDate.setDate(newDate.getDate() - this.dayAgo);
      this.date = newDate;
    } else if (this.monthAgo > 0) {
      const newDate = new Date();
      newDate.setMonth(newDate.getMonth() - this.monthAgo);
      this.date = newDate;
    } else if (this.yearAgo > 0) {
      const newDate = new Date();
      newDate.setFullYear(newDate.getFullYear() - this.yearAgo);
      this.date = newDate;
    } else if (!this.value && this.isCurrentDate) {
      const currentDate = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
      this.date = currentDate;
    } else {
      this.date = this.value;
    }
  },
  watch: {
    date(v, o) {
      if (v !== o) {
        const formatValue = this.$fn.formatDate(v);
        this.$emit("input", formatValue);
        if (v) this.validBeforeDate = formatValue;
        this.inputValue = formatValue;
      }
    },
    value(v) {
      if (v) this.validBeforeDate = v;
      this.inputValue = v;
    }
  },
  methods: {
    onInput(event) {
      const targetValue = event.target.value;
      // 필수 입력값 & 데이터가 없을 경우 체크
      if (this.required && (!targetValue || !/^[\d-]+$/.test(targetValue))) {
        const convertDate = this.convertDateStringToHaipun(
          this.validBeforeDate
        );
        event.target.value = convertDate;
        this.date = convertDate;
        return;
      }

      // 값이 string.empty일경우
      if (!targetValue) {
        this.date = targetValue;
        return;
      }

      // 날짜 타입 체크
      if (this.validDateType(targetValue)) {
        event.target.value = targetValue.slice(0, -1);
        return;
      }

      const replaceAllTargetValue = targetValue.replace(/-/g, "");
      if (replaceAllTargetValue.length === 8) {
        const convertDate = this.convertDateStringToHaipun(
          replaceAllTargetValue
        );
        // 유효한 날짜인지 확인
        if (!this.$fn.validDate(convertDate)) {
          return this.revertDate(event);
        }

        // 검색 가능 최소일 확인
        if (this.checkMinValue && !this.$fn.checkBetweenDate(convertDate)) {
          return this.revertDate(event);
        }

        // 검색 가능 최대일 확인
        const inputDate = new Date(Date.parse(convertDate));
        if (inputDate > this.maxDate) {
          return this.revertDate(event);
        }

        // 값 업데이트
        event.target.value = convertDate;
        this.validBeforeDate = convertDate;
        this.date = convertDate;
      }
    },
    validDateType(value) {
      // 유효한 입력값인지 체크
      // ex) 20201230 || 2020-12-30 둘다 가능한 정규표현식
      const dateRegex = /^(\d{0,4})[-]?\d{0,2}[-]?\d{0,2}$/;
      return !dateRegex.test(value);
    },
    convertDateStringToHaipun(value) {
      const replaceVal = value.replace(/-/g, "");
      const yyyy = replaceVal.substring(0, 4);
      const mm = replaceVal.substring(4, 6);
      const dd = replaceVal.substring(6, 8);
      return `${yyyy}-${mm}-${dd}`;
    },
    getValidBeforeDate() {
      if (this.value) {
        return this.value;
      }

      if (this.dayAgo > 0) {
        const newDate = new Date();
        return newDate.setDate(newDate.getDate() - this.dayAgo);
      }

      return this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    },
    revertDate(event) {
      const convertBeforeDate = this.convertDateStringToHaipun(
        this.validBeforeDate
      );
      event.target.value = convertBeforeDate;
      this.date = convertBeforeDate;
    }
  }
};
</script>
