<template>
    <b-card class="mb-4" title="스크롤페이징">
        <data-table-scroll-paging
            ref="scrollPaging"
            :table-height="'500px'"
            :fields="fields"
            :rows="localData"
            :per-page="perPage"
            :is-actions-slot="true"
            @scrollPerPage="scrollPerPage"
        >
            <template slot="actions">
                <b-button class="mb-1" variant="primary default" @click.stop="onPreview">미리듣기</b-button>
            </template>
        </data-table-scroll-paging>
    </b-card>
</template>
<script>
import DataTableScrollPaging from '../../../components/DataTable/DataTableScrollPaging';
import mockData from '../../../data/tableData.js'

const MOCK_DATA = () => {
    return JSON.parse(JSON.stringify(mockData));
}

export default {
    components: { DataTableScrollPaging },
    data() {
        return {
            localData: [],
            currentPage: 3,
            perPage: 50,
            startRowNum : 0,
            fields: [
                {
                    name: "__checkbox",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: "5%"
                },
                {
                    name: "id",
                    title: "id",
                    titleClass: "",
                    dataClass: "list-item-heading",
                    width: "10%"
                },
                {
                    name: "name",
                    title: "name",
                    titleClass: "",
                    dataClass: "list-item-heading",
                    width: "10%"
                },
                {
                    name: "email",
                    title: "email",
                    titleClass: "",
                    dataClass: "list-item-heading",
                    width: "20%"
                },
                {
                    name: "birthdate",
                    title: "birthdate",
                    titleClass: "",
                    dataClass: "list-item-heading",
                    width: "10%"
                },
                {
                    name: "nickname",
                    title: "nickname",
                    titleClass: "",
                    dataClass: "list-item-heading",
                    width: "10%"
                },
                {
                    name: '__slot:actions',
                    title: '미리듣기',
                    dataClass: "list-item-heading",
                    width: "10%"
                }   
            ]
        }
    },
    mounted() {
        this.getData();
    },
    methods: {
        getData() {
            this.localData = this.getRows();
        },
        scrollPerPage() {
            this.startRowNum = this.currentPage * this.perPage;
            this.currentPage++;
            const nextRows = this.getRows();
            nextRows.forEach(row => {
                this.localData.push(row);
            });
        },
        getRows() {
            return MOCK_DATA().data.splice(this.startRowNum, this.perPage);
        },
        onPreview() {
            console.info("tesetpreview");
        }
    }
}
</script>