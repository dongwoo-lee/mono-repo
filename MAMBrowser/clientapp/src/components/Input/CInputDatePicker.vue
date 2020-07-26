<template>
    <div>
        <b-input-group :class="groupClass">
            <!-- 입력-->
            <b-form-input
                :value="date | yyyyMMdd"
                @input="onInput"
                type="text"
                :placeholder="placeHolder"
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
                this.$emit('input', this.$fn.formatDate(v));
            }
        }
    },
    methods: {
        onInput(value) {
            const datatimeRegex = /^([0-9]{4})+(-?)([0-9]{2})+(-?)([0-9]{2})/;

            if (!datatimeRegex.test(value)) {
                return false;
            }

            this.date = value;
        }
    }
}
</script>