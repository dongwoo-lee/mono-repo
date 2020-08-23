<template>
    <div>
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
    </div>
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
        }
    },
    data() {
        return {
            date: '',
            inputValue: '',
        }
    },
    created() {
        if (!this.value) {
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
                this.inputValue = formatValue;
            }
        }
    },
    methods: {
        onInput(event) {
            let targetValue = event.target.value;
            // 값이 string.empty일경우
            if (!targetValue) {
                this.date = targetValue;
                return;
            }

            // 값 입력 체크
            if (this.inValidDate(targetValue)) {
                event.target.value = targetValue.slice(0, -1);
                return;
            }

            // 날짜 체크
            const replaceAllTargetValue = targetValue.replace(/-/g, '')
            if (replaceAllTargetValue.length === 8) {
                const yyyy = replaceAllTargetValue.substring(0, 4);
                const mm = replaceAllTargetValue.substring(4, 6);
                const dd = replaceAllTargetValue.substring(6, 8);
                const mergeDate = `${yyyy}-${mm}-${dd}`;
                event.target.value = mergeDate;
                this.date = mergeDate;
            }

            return;
        },
        inValidDate(value) {
            // 유효한 입력값인지 체크
            // ex) 20201230 || 2020-12-30 둘다 가능한 정규표현식
            const dateRegex = /^(\d{0,4})[-]?\d{0,2}[-]?\d{0,2}$/;
            return !dateRegex.test(value);
        },
    },
}
</script>