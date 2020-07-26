<template>
    <div class="d-flex">
        <b-input-group class="w-50 mr-2">
            <!-- 입력: 시작날짜 -->
            <b-form-input
                v-model="startDate"
                type="text"
                :placeholder="placeHolder"
            />
            <!-- 데이터 피커: 시작날짜 -->
            <b-input-group-append>
                <b-form-datepicker
                    v-model="startDate"
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
        <b-input-group class="w-50 mr-2">
            <!-- 입력: 종료날짜 -->
            <b-form-input
                v-model="endDate"
                type="text"
                :placeholder="placeHolder"
                autocomplete="off"
            />
            <!-- 데이터피커: 종료날짜 -->
            <b-input-group-append>
                <b-form-datepicker
                    v-model="endDate"
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
        defaultStartDate: {      // 시작 날짜(현재일)
            type: String,
            default: new Date().toISOString().substring(0, 10),
        },
        defaultEndDate: {       // 종료 날짜(현재일 + 1)
            type: String,
            default: new Date().toISOString().substring(0, 10),
            // default: () => {
            //     const date = new Date();
            //     const curDate = date.setDate(date.getDate() + 1);
            //     return new Date(curDate).toISOString().substring(0, 10);
            // }
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
        formatter: {
            type: String,
            default: 'YYYYMMDD',
        },
        size: {
            type: String,
            default: 'sm',
        }
    },
    data() {
        return {
            startDate: '',
            endDate: '',
        }
    },
    created() {
      this.load();  
    },
    watch: {
        startDate(v, o) {
            if (v !== o) {
                this.emitStartDate(v);
            }
        },
        endDate(v, o) {
            if (v !== o) {
                this.emitEndDate(v);
            }
        }
    },
    methods: {
        load() {
            this.startDate = this.defaultStartDate;
            this.endDate = this.defaultEndDate;
            this.emitStartDate(this.startDate);
            this.emitEndDate(this.endDate);
        },
        emitStartDate(v) {
            this.$emit('startDate', this.$fn.formatDate(v));
        },
        emitEndDate(v) {
            this.$emit('endDate', this.$fn.formatDate(v));
        },
    }
}
</script>
