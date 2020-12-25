<template>
    <div class="row" style="margin-right:0;">
        <!-- 등록일: 시작일 -->
        <b-form-group :label="startDateLabel"
          class="has-float-label">
          <common-date-picker v-model="sDate" 
            :dayAgo="startDayAgo"
            :monthAgo="startMonthAgo"
            :yearAgo="startYearAgo"
            :isCurrentDate="false" 
            :required="true" />
        </b-form-group>
      <!-- 등록일: 종료일 -->
        <b-form-group :label="endDateLabel" 
          class="has-float-label">
          <common-date-picker v-model="eDate" 
          :required="true" />
        </b-form-group>
    </div>
</template>
<script>
import { MAXIMUM_SEARCH_DATE } from '@/constants/config';
export default {
    props: {
        startDate: {
            type: String,
            default: '',
        },
        endDate: {
            type: String,
            default: '',
        },
        startDateLabel: {
            type: String,
            default: '시작일'
        },
        endDateLabel: {
            type: String,
            default: '종료일'
        },
        checkMaxSearchDay: {
            type: Boolean,
            default: false,
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
        }
    },
    data() {
        return {
            sDate: '',
            eDate: '',
        }
    },
    watch: {
        sDate(date) {
            this.$emit('update:startDate', date);
            if (date && this.checkMaxSearchDay) {
                console.info('checkMaxSearchDay')
                const result = this.checkMiximumEndDate(date);
            }
        },
        eDate(date) {
            this.$emit('update:endDate', date);
            if (date && this.checkMaxSearchDay) {
                console.info('checkMaxSearchDay')
                const result = this.checkMiximumStartDate(date);
                // TODO: 체크
            }
        }
    },
    mounted() {
        this.sDate = this.startDate;
        this.eDate = this.endDate;
    },
    methods: {
        checkMiximumStartDate(date) {
            const selectedDate = Date.parse(this.$fn.formatDate(date, 'yyyy-MM-dd'));
            const maxSearchDate = new Date(selectedDate);
            const endDate = Date.parse(this.$fn.formatDate(this.eDate, 'yyyy-MM-dd'));
            maxSearchDate.setDate(maxSearchDate.getDate() - MAXIMUM_SEARCH_DATE);
            return selectedDate <= maxSearchDate;
        },
        checkMiximumEndDate(date) {
            const selectedDate = Date.parse(this.$fn.formatDate(date, 'yyyy-MM-dd'));
            const maxSearchDate = new Date(selectedDate);
            maxSearchDate.setDate(maxSearchDate.getDate() + MAXIMUM_SEARCH_DATE);
            return selectedDate <= maxSearchDate;
        }
    }
}
</script>