<template>
  <div>
    <b-modal :id="id" size="xl" centered :title="title">
      <div>
        <DxDataGrid
          :data-source="dataSource"
          :columns="columnData"
          height="500px"
          key="rowno"
          :showColumnLines="false"
          :show-borders="true"
          :showRowLines="true"
          noDataText="데이터가 없습니다."
        >
          <DxScrolling mode="virtual" />
          <template #playtime_template="{ data }">
            <div>
              {{
                $moment(data.data.playtime)
                  | moment("subtract", "9 hours")
                  | moment("HH:mm:ss")
              }}
            </div>
          </template>
          <template #totaltime_template="{ data }">
            <div>
              {{
                $moment(data.data.totaltime)
                  | moment("subtract", "9 hours")
                  | moment("HH:mm:ss")
              }}
            </div>
          </template>
          <!-- <DxColumn
            v-for="(column, index) in columnData"
            :key="index"
            :caption="column.caption"
            :data-field="column.dataField"
          /> -->
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
import "moment/locale/ko";
const moment = require("moment");
export default {
  props: {
    title: {
      type: String,
      default: "",
    },
    id: {
      type: String,
      default: "table_view_modal",
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
