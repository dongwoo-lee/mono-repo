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
            >
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
                />
            </b-input-group-append>
        </b-input-group>
    </b-form>
</template>

<script>
export default {
    props: {
        value: {
            type: String,
            default: '',
        },
        groupClass: {
            type: String,
            default: '',
        },
        hideHeader: {
            type: Boolean,
            default: true,
        },
        placeHolder: {
            type: String,
            default: 'YYYY-MM-DD'
        },
        labelNextMonth: {
            type: String,
            default: '다음달',
        },
        size: {
            type: String,
            default: 'sm',
        },
        isCurrentDate: {
            type: Boolean,
            default: true,
        },
        dayAgo: {
            type: Number,
            defaut: 0,
        },
        required: {
            type: Boolean,
            default: false,
        }
    },
    data() {
        //TODO: 시작일 min값 2020/12/01 , 종료일 max값 검색 당일 + 1일 (D+1)
        const now = new Date();
        const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        const minDate = new Date(2020, 11, 1); // 범위: 0~11(11 값이->12월달)
        const maxDate = new Date(today);
        maxDate.setDate(maxDate.getDate() + 1);

        return {
            date: '',
            inputValue: '',
            validBeforeDate: this.getValidBeforeDate(),
            minDate: minDate,
            maxDate: maxDate
        }
    },
    created() {
        if (this.dayAgo > 0) {
            const newDate = new Date();
            newDate.setDate(newDate.getDate() - this.dayAgo);
            this.date = newDate;
        } else if (!this.value && this.isCurrentDate) {
            const currentDate = new Date().toISOString().substring(0, 10);
            this.date = currentDate;
        } else {
            this.date = this.value;
        }
    },
    watch: {
        date(v, o) {
            if (v !== o) {
                const formatValue = this.$fn.formatDate(v);
                this.$emit('input', formatValue);
                if (v) this.validBeforeDate = formatValue;
                this.inputValue = formatValue;
            }
        }
    },
    methods: {
        onInput(event) {
            const targetValue = event.target.value;
            
            // 필수 입력값일 경우
            if (this.required && (!targetValue || !/^[\d-]+$/.test(targetValue))) {
                const convertDate = this.convertDateStringToHaipun(this.validBeforeDate);
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

            const replaceAllTargetValue = targetValue.replace(/-/g, '')
            if (replaceAllTargetValue.length === 8) {
                const convertDate = this.convertDateStringToHaipun(replaceAllTargetValue);
                 // 유효한 날짜인지 체크
                if (this.$fn.validDate(convertDate)) {
                    event.target.value = convertDate;
                    this.validBeforeDate = convertDate;
                    this.date = convertDate;
                } else {
                    const convertBeforeDate = this.convertDateStringToHaipun(this.validBeforeDate);
                    event.target.value = convertBeforeDate;
                    this.date = convertBeforeDate;
                }
            }
        },
        validDateType(value) {
            // 유효한 입력값인지 체크
            // ex) 20201230 || 2020-12-30 둘다 가능한 정규표현식
            const dateRegex = /^(\d{0,4})[-]?\d{0,2}[-]?\d{0,2}$/;
            return !dateRegex.test(value);
        },
        convertDateStringToHaipun(value) {
            const replaceVal = value.replace(/-/g, '')
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

            return new Date().toISOString().substring(0, 10);
        }
    },
}
</script>