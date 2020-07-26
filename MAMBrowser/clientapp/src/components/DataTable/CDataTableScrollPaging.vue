<template>
    <div>
        <vuetable ref="vuetable"
            class="order-with-arrow"
            :api-mode="false"
            :table-height="tableHeight"
            :fields="fields"
            :data="rows"
            :per-page="perPage"
            :row-class="onRowClass"
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
            isRightClick: false,
        }
    },
    mounted() {
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
                this.onSortableClick(e);
            });
        });
    },
    destroyed() {
      if (this.tBody != null) {
        this.tBody.removeEventListener('scroll', this.handlerScroll);
      }

      if (this.sortable != null) {
          this.sortable.removeEventListener('click', this.onSortableClick);
      }
    },
    methods: {
        handlerScroll({ target }) {
            const { clientHeight, scrollTop, scrollHeight } = target;
            if (clientHeight + scrollTop > scrollHeight - (this.numRowsToBottom * this.rowElemHeight)) {
                this.lastClientHeight = scrollTop;
                this.$emit('scrollPerPage', this.currentPage * 10)
            }
            
        },
        onRowClass(dataItem, index) {
            if (this.selectedItems.includes(dataItem.id)) {
                return "selected";
            }
            return "";
        },
        rowClicked(dataItem, event) {
            const itemId = dataItem.id;
            if (this.selectedItems.includes(itemId)) {
                this.selectedItems = this.selectedItems.filter(x => x !== itemId);
                const findIndex = this.$refs.vuetable.selectedTo.findIndex(x => x === itemId);
                this.$refs.vuetable.selectedTo.splice(findIndex, 1);
            } else {
                this.selectedItems.push(itemId);
                this.$refs.vuetable.selectedTo[itemId] = itemId;
            }
        },
        rightClicked(dataItem, field, event) {
            event.preventDefault();
            this.isRightClick = true;
            if (!this.selectedItems.includes(dataItem.id)) {
                this.selectedItems = [dataItem.id];
            }
            this.$refs.contextmenu.show({ top: event.pageY, left: event.pageX });
        },
        onContextMenuAction(data) {
            this.$emit('contextMenuAction', data);
        },
        onSortableClick(e) {
            const targetId = e.target.id.replace('_', '');
            this.$emit('sortableclick', targetId);
        }
    }
}
</script>