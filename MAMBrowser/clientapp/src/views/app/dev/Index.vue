<template>
<div>
    <b-row>
        <b-colxx xxs="12">
            <piaf-breadcrumb heading="개발 컴포넌트 모음" />
            <div class="separator mb-5"></div>
        </b-colxx>
    </b-row>
    <b-row>
        <!-- 스크롤 페이징 테이블 -->
        <b-colxx xxs="12">
            <dev-scroll-paging-table></dev-scroll-paging-table>
        </b-colxx>
        <!-- 다운로드 & 업로드 -->
        <b-colxx xxs="12">
            <dev-file-upload></dev-file-upload>
        </b-colxx>
    </b-row>
</div>
</template>

<script>
import DevScrollPagingTable from './DevScrollPagingTable';
import DevFileUpload from './DevFileUpload';

export default {
    components: { DevScrollPagingTable, DevFileUpload }
}
</script>
