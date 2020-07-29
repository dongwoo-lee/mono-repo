<template>
    <div>
        <b-input-group :class="groupClass">
            <!-- 입력-->
           <input
                type="text" 
                class="form-control" 
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
import CInputText from './CInputText';

export default {
    components: { CInputText },
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

            if (!v) {
                this.inputValue = v;
                this.$emit('input', v);
            }
        }
    },
    methods: {
        onInput(event) {
            let targetValue = event.target.value;
            if (!targetValue) {
                this.date = targetValue;
                return;
            }

            if (this.inValidDate(targetValue)) {
                event.target.value = targetValue.slice(0, -1);
                return;
            }

            const replaceTargetValue = targetValue.replace(/-/g, '');
            if (replaceTargetValue.length === 8) {
                const yyyy = replaceTargetValue.substring(0, 4);
                const mm = replaceTargetValue.substring(4, 6);
                const dd = replaceTargetValue.substring(6, 8);
                const mergeDate = `${yyyy}-${mm}-${dd}`;
                event.target.value = mergeDate;
                this.date = mergeDate;
            }
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