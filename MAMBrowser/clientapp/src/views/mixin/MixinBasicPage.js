let mixinBasicPage = {
    data() {
        return {
            responseData: {
                data: null,
                rowPerPage: 16,
                selectPage: 1,
                totalRowCount: 0,
            },
            numRowsToBottom: 5,
            contextMenu: [
                { name: 'edit', text: '편집' },
                { name: 'throw', text: '휴지통으로 보내기' },
                { name: 'download', text: '다운로드' },
            ]
        }
    },
    created() {
        this.$nextTick(() => {
            this.getData();
        });
    },
    methods: {
        onScrollPerPage() {
            this.responseData.selectPage++;
            this.searchItems.selectPage++;
            this.getData();
        },
        onContextMenuAction(v) {
            switch(v) {
                case 'edit': console.log(v); break;
                case 'throw': console.log(v); break;
                case 'download':  console.log(v); break;
                case 'storage':  console.log(v); break;
                default: break;
            }
        },
        onSortableClick(sortKey) {
            this.searchItems.sortKey = sortKey;
            this.searchItems.sortValue = this.$fn.changeSortValue(this.searchItems.sortValue);
            this.getData();
        },
    }
}

export default mixinBasicPage;