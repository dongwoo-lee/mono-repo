<template>
  <div class="custom-vuetable-wrapper">
    <div class="text-center text-primary my-2" v-if="isTableLoading">
      <b-spinner class="align-middle"></b-spinner>
      <strong>Loading...</strong>
    </div>
    <vuetable
      v-else
      ref="vuetable"
      class="scrolltable order-with-arrow"
      :api-mode="false"
      :fields="fields"
      :data="rows"
      :row-class="onRowClass"
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
    classString: {
      type: String,
      default: "",
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
  },
  data() {
    return {
      selectedItems: [], // 선택 로우 데이터
      isRightClick: false,
    };
  },
  mounted() {
    this.$nextTick(() => {
      if (!this.$refs.vuetable) return;
      const rowElem = this.$refs.vuetable.$el.querySelectorAll(
        "tbody.vuetable-body tr"
      )[0];
      // [this.sortable] = this.$refs.vuetable.$el.getElementsByClassName('sortable');
      if (!rowElem) return;

      // sortable click event linstener
      this.sortable = this.$refs.vuetable.$el.querySelectorAll("th.sortable");
      if (!this.sortable) return;
      this.sortable.forEach((element) => {
        element.addEventListener("click", (e) => {
          this.onSortable(e);
        });
      });
    });
  },
  destroyed() {
    if (this.sortable != null) {
      this.sortable.forEach((element) => {
        element.removeEventListener("click", this.onSortable);
      });
    }
  },
  methods: {
    onRowClass(dataItem, index) {
      if (this.selectedItems.includes(dataItem.id)) {
        return "selected";
      }
      return "";
    },
    rowClicked(dataItem, event) {
      const itemId = dataItem.id;
      if (this.selectedItems.includes(itemId)) {
        this.selectedItems = this.selectedItems.filter((x) => x !== itemId);
        const findIndex = this.$refs.vuetable.selectedTo.findIndex(
          (x) => x === itemId
        );
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
      this.$emit("contextMenuAction", data);
    },
    onSortable(e) {
      console.info("onSortTable");
      const targetId = e.target.id.replace("_", "");
      this.$emit("sortableclick", targetId);
    },
  },
};
</script>