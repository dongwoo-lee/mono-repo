<template>
<div>
    <b-row>
        <b-colxx xxs="12">
            <piaf-breadcrumb heading="개발 컴포넌트 모음" />
            <div class="separator mb-5"></div>
        </b-colxx>
    </b-row>
    <b-row>
        <b-colxx xxs="12">
            <!-- 스크롤 페이징 테이블 -->
            <dev-scroll-paging-table></dev-scroll-paging-table>
        </b-colxx>
    </b-row>
</div>
</template>

<script>
import DevScrollPagingTable from './DevScrollPagingTable';

export default {
    components: { DevScrollPagingTable }
}
</script>
