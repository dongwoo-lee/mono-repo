<template>
  <div>
    <b-modal :id="id" size="lg" centered :title="title">
      <div>
        <DxDataGrid
          :data-source="dataSource"
          height="500px"
          key="rowno"
          :showColumnLines="false"
          :show-borders="true"
          :showRowLines="false"
          noDataText="데이터가 없습니다."
        >
          <DxScrolling mode="virtual" />
          <DxColumn
            v-for="(column, index) in columnData"
            :key="index"
            :caption="column.title"
            :data-field="column.name"
          />
        </DxDataGrid>
      </div>
      <template #modal-footer>
    <b-button @click="$bvModal.hide(id)">닫기</b-button>
  </template>
    </b-modal>
  </div>
</template>

<script>
import { DxDataGrid, DxScrolling, DxColumn } from "devextreme-vue/data-grid";
export default {
  props: {
    id: {
      type: String,
      default: "table_view_modal",
    },
    title: {
      type: String,
      default: "",
    },
    dataSource: {
      type: Array,
      default: () => [],
    },
    columnData: {
      type: Array,
      default: () => [],
    },
  },
  data() {
    return {};
  },
  components: { DxDataGrid, DxScrolling, DxColumn },
  methods: {},
};
</script>
