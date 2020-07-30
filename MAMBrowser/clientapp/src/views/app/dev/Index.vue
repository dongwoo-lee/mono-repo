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
        <!-- 파일 업로드 - vue-upload-component -->
        <b-colxx xxs="12">
            <dev-file-upload-component></dev-file-upload-component>
        </b-colxx>
        <!-- dropdown menu + input + 순간 검색 -->
        <b-colxx xxs="4">
            <dev-dropdown-menu-input></dev-dropdown-menu-input>
        </b-colxx>
        <!-- check box -->
        <b-colxx xxs="4">
            <b-card class="mb-4">
                <b-form-group label="추가 검색">
                    <b-form-checkbox-group v-model="checkboxSelected">
                    <b-form-checkbox value="orange">히트곡</b-form-checkbox>
                    <b-form-checkbox value="apple">금지곡</b-form-checkbox>
                    <b-form-checkbox value="pineapple">주의</b-form-checkbox>
                    <b-form-checkbox value="grape">청소년유해</b-form-checkbox>
                    <b-form-checkbox value="banana">뮤직비디오</b-form-checkbox>
                    </b-form-checkbox-group>
                </b-form-group>
            </b-card>
        </b-colxx>
        <!-- date -->
        <b-colxx xxs="4">
        </b-colxx>
    </b-row>
</div>
</template>

<script>
import DevTable from './DevTable';
import DevScrollPagingTable from './DevScrollPagingTable';
import DevFileUpload from './DevFileUpload';
import DevFileUploadComponent from './DevFileUploadComponent';
import DevDropdownMenuInput from './DevDropdownMenuInput'

export default {
    components: {
        DevTable,
        DevScrollPagingTable,
        DevFileUpload,
        DevFileUploadComponent,
        DevDropdownMenuInput,
    },
    data() {
        return {
            checkboxSelected: [],
        }
    }
}
</script>
