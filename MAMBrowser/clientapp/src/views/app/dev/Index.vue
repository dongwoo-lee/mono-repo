<template>
<div>
    <b-row>
        <b-colxx xxs="12">
            <piaf-breadcrumb heading="개발 컴포넌트 모음" />
            <div class="separator mb-5"></div>
        </b-colxx>
    </b-row>
    <b-row>
        <!-- 스크롤 테이블 -->
        <b-colxx xxs="12">
            <dev-table></dev-table>
        </b-colxx>
        <!-- 스크롤 페이징 테이블 -->
        <b-colxx xxs="12">
            <dev-scroll-paging-table></dev-scroll-paging-table>
        </b-colxx>
        <!-- 파일 멀티 업로드 -->
        <b-colxx xxs="12">
            <dev-file-upload></dev-file-upload>
        </b-colxx>
        <!-- dropdown menu + input + 순간 검색 -->
        <b-colxx xxs="4">
            <dev-dropdown-menu-input></dev-dropdown-menu-input>
        </b-colxx>
        <!-- check box -->
        <b-colxx xxs="4">
            <b-card class="mb-4" title="체크 박스">
                <b-form-group label="추가 검색">
                    <b-form-checkbox-group v-model="checkboxSelected">
                    <b-form-checkbox value="orange">히트곡</b-form-checkbox>
                    <b-form-checkbox value="apple">금지곡</b-form-checkbox>
                    <b-form-checkbox value="pineapple">주의</b-form-checkbox>
                    <b-form-checkbox value="grape">청소년유해</b-form-checkbox>
                    <b-form-checkbox value="banana">뮤직비디오</b-form-checkbox>
                    </b-form-checkbox-group>
                </b-form-group>
                <div>결과: {{ checkboxSelected }}</div>
            </b-card>
        </b-colxx>
        <!-- date picker -->
        <b-colxx xxs="4">
            <b-card class="mb-4" title="데이트피커(시작-종료 날짜)">
                <b-form-group label="날짜(시작-종료)" class="inline-block">
                    <c-input-date-picker-group
                        @startDate="onDatePickerStartDate"
                        @endDate="onDatePickerEndDate"
                    />
                </b-form-group>
                <div>결과: {{ this.localSearchItems }}</div>
                <div>기본 포맷방식은 'yyyyMMdd'입니다.</div>
            </b-card>
        </b-colxx>
    </b-row>
</div>
</template>

<script>
import DevTable from './DevTable';
import DevScrollPagingTable from './DevScrollPagingTable';
import DevFileUpload from './DevFileUpload';
import DevDropdownMenuInput from './DevDropdownMenuInput'
import CInputDatePickerGroup from '../../../components/Input/CInputDatePickerGroup';

export default {
    components: {
        DevTable,
        DevScrollPagingTable,
        DevFileUpload,
        DevDropdownMenuInput,
        CInputDatePickerGroup,
    },
    data() {
        return {
            checkboxSelected: [],
             localSearchItems: {              // 검색
                start_dt: '',
                end_dt: '',
            },
        }
    },
    methods: {
        onDatePickerStartDate(data) {
            this.localSearchItems.start_dt = data;
        },
        onDatePickerEndDate(data) {
            this.localSearchItems.end_dt = data
        }
    }
}
</script>
