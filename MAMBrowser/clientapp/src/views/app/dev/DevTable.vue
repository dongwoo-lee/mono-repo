<template>
    <b-card class="mb-4" title="스크롤 페이지 - 사용안할듯">
        <b-form class="mb-3" inline>
            <b-input-group class="mr-2">
                <b-button class="mb-1" variant="primary default" size="sm">파일 업로드</b-button>
            </b-input-group>
            <b-input-group class="mr-2">
                <b-button class="mb-1" variant="danger default" size="sm">휴지통비우기</b-button>
            </b-input-group>
            <b-input-group class="mr-2">
                <b-button class="mb-1" variant="secondary default" size="sm">복원</b-button>
            </b-input-group>
        </b-form>
        <vuetable
            table-height="420px"
            ref="vuetable"
            :api-url="apiBase"
            class="order-with-arrow"
            :query-params="makeQueryParams"
            :per-page="perPage"
            :reactive-api-url="true"
            :fields="fields"
            pagination-path
            :row-class="onRowClass"
            @vuetable:row-clicked="rowClicked"
            @vuetable:cell-rightclicked="rightClicked"
            >
            <!-- <template slot="checkbox" slot-scope="props">
                <b-form-checkbox
                :checked="selectedItems.includes(props.rowData.id)"
                class="itemCheck mb-0"
                ></b-form-checkbox>
            </template> -->
        </vuetable>
    </b-card>
</template>
<script>
import Vuetable from "vuetable-2/src/components/Vuetable";
import { apiUrl } from "../../../constants/config";

export default {
    components: { Vuetable },
    data() {
        return {
            apiBase: apiUrl + "/cakes/fordatatable",
            page: 1,
            perPage: 12,
            fields: [
                {
                    name: "__checkbox",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: "5%"
                },
                {
                    name: "title",
                    title: "파일명",
                    titleClass: "",
                    dataClass: "list-item-heading",
                    width: "10%"
                },
                {
                    name: "sales",
                    title: "메모",
                    titleClass: "",
                    dataClass: "text-muted",
                },
                {
                    name: "stock",
                    title: "파일형식",
                    titleClass: "",
                    dataClass: "text-muted",
                    width: "10%"
                },
                {
                    name: "category",
                    title: "상세정보",
                    titleClass: "",
                    dataClass: "text-muted",
                    width: "25%"
                },
                {
                    name: "writeDate",
                    title: "등록일시",
                    titleClass: "",
                    dataClass: "text-muted",
                    width: "5%"
                },
            ]
        }
    },
    methods: {
        makeQueryParams(sortOrder, currentPage, perPage) {
            this.selectedItems = [];
            return sortOrder[0]
                ? {
                    sort: sortOrder[0]
                        ? sortOrder[0].field + "|" + sortOrder[0].direction
                        : "",
                    page: currentPage,
                    per_page: this.perPage,
                    search: this.search
                }
                : {
                    page: currentPage,
                    per_page: this.perPage,
                    search: this.search
                };
        },
        onRowClass(dataItem, index) {
            if (this.selectedItems.includes(dataItem.id)) {
                return "selected";
            }
            return "";
        },
        rowClicked(dataItem, event) {
            const itemId = dataItem.id;
            if (event.shiftKey && this.selectedItems.length > 0) {
                let itemsForToggle = this.items;
                var start = this.getIndex(itemId, itemsForToggle, "id");
                var end = this.getIndex(
                    this.selectedItems[this.selectedItems.length - 1],
                    itemsForToggle,
                    "id"
                );
                itemsForToggle = itemsForToggle.slice(
                    Math.min(start, end),
                    Math.max(start, end) + 1
                );
                this.selectedItems.push(
                    ...itemsForToggle.map(item => {
                    return item.id;
                    })
                );
                this.selectedItems = [...new Set(this.selectedItems)];
            } else {
                if (this.selectedItems.includes(itemId)) {
                    this.selectedItems = this.selectedItems.filter(x => x !== itemId);
                } else this.selectedItems.push(itemId);
            }
        },
        rightClicked(dataItem, field, event) {
            event.preventDefault();
            if (!this.selectedItems.includes(dataItem.id)) {
                this.selectedItems = [dataItem.id];
            }
            this.$refs.contextmenu.show({ top: event.pageY, left: event.pageX });
        },
    }
}
</script>