<template>
  <div class="custom-vuetable-wrapper">
    <div class="text-center text-primary my-2" v-if="isTableLoading">
      <b-spinner class="align-middle"></b-spinner>
      <strong>Loading...</strong>
    </div>
    <vuetable
      v-show="!isTableLoading"
      ref="vuetable"
      class="scrolltable order-with-arrow"
      tableBodyClass="custom-vuetable-wrapper"
      :api-mode="false"
      :table-height="tableHeight"
      :fields="fields"
      :data="rows"
      :per-page="perPage"
      :row-class="onRowClass"
      :track-by="keyName"
      :noDataTemplate="noDataTemplate"
      :multi-sort="true"
      @vuetable:row-clicked="rowClicked"
      @vuetable:cell-rightclicked="rightClicked"
    >
      <template v-if="isActionsSlot" slot="actions" scope="props">
        <div class="table-button-container">
          <slot name="actions" :props="props"></slot>
        </div>
      </template>
      <template v-if="isWeeksSlot" slot="weeks" scope="props">
        <div class="table-button-container">
          <slot name="weeks" :props="props"></slot>
        </div>
      </template>
    </vuetable>
    <!-- 우클릭 컨텍스트 메뉴 -->
    <v-contextmenu ref="contextmenu">
      <v-contextmenu-item
        v-for="(item, i) in contextmenu"
        :key="i"
        @click="onContextMenuAction(item.name)"
      >
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
    keyName: {
      // rowData key
      type: String,
      default: "seq",
    },
    tableHeight: {
      // 테이블 높이
      type: String,
      default: "520px",
    },
    fields: {
      // header 데이터
      type: Array,
      default: () => [],
    },
    rows: {
      // rows 데이터
      type: Array,
      default: () => [],
    },
    totalCount: {
      type: Number,
      default: 0,
    },
    numRowsToBottom: {
      // 맨 아래 행수
      type: Number,
      default: 4,
    },
    perPage: {
      // 페이지당 보여줄 개수
      type: Number,
      default: 10,
    },
    isActionsSlot: {
      // actions: button 등 slot 사용 유무
      type: Boolean,
      default: false,
    },
    isWeeksSlot: {
      // actions: button 등 slot 사용 유무
      type: Boolean,
      default: false,
    },
    noDataTemplate: {
      type: String,
      default: "데이터가 없습니다.",
    },
    contextmenu: {
      // 우측 클릭 메뉴
      type: Array,
      default: () => [
        // {
        //     name: 'edit',
        //     text: '편집',
        // }
      ],
    },
    isTableLoading: {
      type: Boolean,
      default: false,
    },
    isScrollLodaing: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      tBodyWrapper: null, // vuetable 테이블 body-wrapper
      scrollTimeout: null, // 스크롤 동작 타임아웃
      rowElemHeight: 0, // 로우 높이
      selectedItems: [], // 선택 로우 데이터
      isChangeRowPerPage: false,
      sortables: [],
    };
  },
  mounted() {
    this.$watch(
      () => {
        return this.$refs.vuetable.selectedTo;
      },
      (selectedIds) => {
        this.$emit("selectedIds", selectedIds);
      }
    );

    this.$nextTick(() => {
      this.rowElem = this.$refs.vuetable.$el.querySelectorAll(
        "tbody.vuetable-body tr"
      )[0];
      [this.tBodyWrapper] = this.$refs.vuetable.$el.getElementsByClassName(
        "vuetable-body-wrapper"
      );
      if (!this.rowElem || !this.tBodyWrapper) return;
      this.createDivLastPage();
      this.createDivDataLoding();
      this.addListener();
    });
  },
  computed: {},
  destroyed() {
    if (this.tBody != null) {
      this.tBody.removeEventListener("scroll", this.handlerScroll);
    }

    if (this.sortable != null) {
      this.sortable.forEach((element) => {
        element.removeEventListener("click", this.onSortable);
      });
    }
  },
  methods: {
    existScrollBar() {
      const { clientHeight, scrollTop, scrollHeight } = this.tBodyWrapper;
      return (
        (scrollHeight === 0 && clientHeight === 0) ||
        scrollHeight > clientHeight
      );
    },
    addClassScroll() {
      if (this.existScrollBar()) {
        this.tBodyWrapper.classList.add("scroll");
      } else {
        this.tBodyWrapper.classList.remove("scroll");
      }
    },
    init() {
      const { scrollTop } = this.tBodyWrapper;
      if (scrollTop > 0) {
        this.tBodyWrapper.scrollTop = 0;
        this.isChangeRowPerPage = true;
      }
      this.initSelectedIds();
      this.addClassScroll();
    },
    initSelectedIds() {
      this.$refs.vuetable.selectedTo = [];
    },
    handlerScroll({ target }) {
      if (!this.existScrollBar()) return;
      if (this.isChangeRowPerPage) {
        this.isChangeRowPerPage = false;
        return;
      }
      const { clientHeight, scrollTop, scrollHeight } = target;
      if (
        this.perPage > this.totalCount ||
        this.totalCount === this.rows.length
      ) {
        this.displayLastPage(true);
        return;
      } else {
        this.displayLastPage(false);
      }

      if (
        clientHeight + scrollTop >
        scrollHeight - this.numRowsToBottom * this.rowElemHeight
      ) {
        this.loading(true);
        this.lastClientHeight = scrollTop;
        this.$emit("scrollPerPage", this.currentPage * 10);
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
      if (dataItem.tabletype == "modal") {
        this.$refs.vuetable.selectedTo.forEach((id) => {
          if (id != itemId) {
            this.$refs.vuetable.unselectId(id);
          }
        });
      }
    },
    rightClicked(dataItem, field, event) {
      event.preventDefault();
      this.$refs.contextmenu.show({ top: event.pageY, left: event.pageX });
    },
    onContextMenuAction(data) {
      this.$emit("contextMenuAction", data);
    },
    onSortable(e) {
      const targetId = e.target.id.replace("_", "");
      this.$emit("sortableclick", targetId);
    },
    createDivLastPage() {
      const node = document.createElement("div");
      node.classList.add("table-last-page-word");
      const dom = "마지막 페이지입니다.";
      node.innerHTML = dom;
      this.tBodyWrapper.appendChild(node);
    },
    createDivDataLoding() {
      const node = document.createElement("div");
      node.classList.add("data-loading-item");
      const dom = '<div class="data-loading"></div><strong>Loading...</strong>';
      node.innerHTML = dom;
      this.tBodyWrapper.appendChild(node);
    },
    addListener() {
      this.rowElemHeight = this.rowElem.clientHeight || 43;
      // scroll event linstener
      this.tBodyWrapper.addEventListener("scroll", (e) => {
        clearTimeout(this.scrollTimeout);
        this.scrollTimeout = setTimeout(() => {
          this.handlerScroll(e);
        }, 500);
      });

      // sortable click event linstener
      this.sortable = this.$refs.vuetable.$el.querySelectorAll("th.sortable");
      if (!this.sortable) return;
      this.sortable.forEach((element) => {
        element.addEventListener("click", (e) => {
          this.onSortable(e);
        });
      });
    },
    displayLastPage(isActive) {
      const divLastPage = this.tBodyWrapper.querySelector(
        "div.table-last-page-word"
      );
      if (isActive) {
        divLastPage.classList.add("last-page-active");
      } else {
        divLastPage.classList.remove("last-page-active");
      }
    },
    loading(isLoading = false) {
      const dataLoading = this.tBodyWrapper.querySelector(
        "div.data-loading-item"
      );
      if (isLoading) {
        dataLoading.classList.add("active");
      } else {
        dataLoading.classList.remove("active");
      }
    },
    tableRefresh() {
      this.$emit("refresh");
    },
  },
};
</script>
<style scope>
.black {
  background-color: black;
  color: white;
}
.week {
  padding: 2px;
}
</style>