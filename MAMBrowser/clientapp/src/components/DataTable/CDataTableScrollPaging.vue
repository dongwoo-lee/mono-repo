<template>
    <div>
        <vuetable ref="vuetable"
            class="scrolltable order-with-arrow"
            :api-mode="false"
            :table-height="tableHeight"
            :fields="fields"
            :data="rows"
            :per-page="perPage"
            :row-class="onRowClass"
            :track-by="keyName"
            :noDataTemplate="noDataTemplate"
            @vuetable:row-clicked="rowClicked"
            @vuetable:cell-rightclicked="rightClicked"
        >
            <template v-if="isActionsSlot" slot="actions" scope="props">
                <div class="table-button-container">
                    <slot name="actions" :props="props"></slot>
                </div>
            </template>
        </vuetable>
        <!-- 우클릭 컨텍스트 메뉴 -->
        <v-contextmenu ref="contextmenu">
            <v-contextmenu-item v-for="(item, i) in contextmenu" :key="i" @click="onContextMenuAction(item.name)">
                <i class="simple-icon-docs" />
                <span>{{ item.text }}</span>
            </v-contextmenu-item>
        </v-contextmenu>
    </div>
</template>

<script>
import Vuetable from "vuetable-2/src/components/Vuetable";

export default {
    components: { Vuetable },
    props: {
        keyName: {                  // rowData key
            type: String,
            default: 'seq',
        },
        tableHeight: {          // 테이블 높이
            type: String,
            default: '500px',
        },
        fields: {               // header 데이터
            type: Array,
            default: () => [],
        },
        rows: {                  // rows 데이터
            type: Array,
            default: () => [],
        },
        numRowsToBottom: {       // 맨 아래 행수
            type: Number,
            default: 4,
        },
        perPage: {               // 페이지당 보여줄 개수
            type: Number,
            default: 10,
        },
        isActionsSlot: {         // actions: button 등 slot 사용 유무
            type: Boolean,  
            default: false,
        },
        noDataTemplate: {
            type: String,
            default: '데이터가 없습니다.'
        },
        contextmenu: {            // 우측 클릭 메뉴
            type: Array,
            default: () => [
                // {
                //     name: 'edit',
                //     text: '편집',
                // }
            ]
        },
    },
    data() {
        return {
            tBody: null,         // vuetable 테이블 body
            scrollTimeout: null, // 스크롤 동작 타임아웃
            rowElemHeight: 0,    // 로우 높이
            selectedItems: [],   // 선택 로우 데이터
        }
    },
    mounted() {
        this.$watch(
            () => {
                return this.$refs.vuetable.selectedTo;
            },
            (selectedIds) => {
                this.$emit('selectedIds', selectedIds);
            }
        )

        this.$nextTick(() => {
            const rowElem = this.$refs.vuetable.$el.querySelectorAll('tbody.vuetable-body tr')[0];
            [this.tBody] = this.$refs.vuetable.$el.getElementsByClassName('vuetable-body-wrapper');
            if (!rowElem || !this.tBody) return;
            this.rowElemHeight = rowElem.clientHeight;
            // scroll event linstener
            this.tBody.addEventListener('scroll', e => {
                clearTimeout(this.scrollTimeout);
                this.scrollTimeout = setTimeout(() => {
                    this.handlerScroll(e);
                }, 150)
            });

            // sortable click event linstener
            [this.sortable] = this.$refs.vuetable.$el.getElementsByClassName('sortable');
            if (!this.sortable) return;
            this.sortable.addEventListener('click', e => {
                this.onSortable(e);
            });
        });
    },
    destroyed() {
      if (this.tBody != null) {
        this.tBody.removeEventListener('scroll', this.handlerScroll);
      }

      if (this.sortable != null) {
          this.sortable.removeEventListener('click', this.onSortable);
      }
    },
    methods: {
        handlerScroll({ target }) {
            const { clientHeight, scrollTop, scrollHeight, scrollY } = target;
            if (clientHeight + scrollTop > scrollHeight - (this.numRowsToBottom * this.rowElemHeight)) {
                this.tBody.classList.add('scroll');
                this.lastClientHeight = scrollTop;
                this.$emit('scrollPerPage', this.currentPage * 10)
            }
            
        },
        onRowClass(dataItem, index) {
            if (this.selectedItems.includes(dataItem[this.keyName])) {
                return "selected";
            }
            return "";
        },
        rowClicked(dataItem, field) {
            const itemId = dataItem[this.keyName];
            const duplicateCheck = this.$refs.vuetable.selectedTo.includes(itemId);
            if (duplicateCheck) {
                this.$refs.vuetable.unselectId(itemId);
            } else {
                this.$refs.vuetable.selectId(itemId);
            }
        },
        rightClicked(dataItem, field, event) {
            event.preventDefault();
            this.$refs.contextmenu.show({ top: event.pageY, left: event.pageX });
        },
        onContextMenuAction(data) {
            this.$emit('contextMenuAction', data);
        },
        onSortable(e) {
            const targetId = e.target.id.replace('_', '');
            this.$emit('sortableclick', targetId);
        },
        tableRefresh() {
            this.$emit('refresh');
        }
    }
}
</script>