<template>
  <div style="text-align: center">
    <h3 class="m-3" v-if="isTotalValue">
      현재 DAP(A, B)에 추가된 소재들을 하나의 Wav 파일로 병합하여 내보냅니다.
    </h3>
    <h3 v-else class="m-3" style="color: red">
      소재 취합 후 길이는 {{ maxDuration / 3600000 }}시간을 넘을 수 없습니다.
    </h3>
    <div
      class="wav-ab-grid p-4 mt-3 mb-3"
      v-bind:class="{ 'max-duration-color': isTotalValue }"
    >
      <DxDataGrid
        :data-source="dataSource"
        :show-borders="true"
        :showRowLines="true"
        height="440px"
      >
        <DxScrolling mode="multiple" showCheckBoxesMode="none" />
        <DxPaging :enabled="false" />
        <DxColumn
          data-field="rownum"
          alignment="center"
          caption=""
          :width="45"
        />
        <DxColumn data-field="maintitle" alignment="center" caption="제목" />
        <DxColumn
          data-field="subtitle"
          alignment="center"
          caption="내용"
          width="20%"
        />
        <DxColumn
          cell-template="startPositionTemplate"
          alignment="center"
          caption="음원 시작점"
          :width="100"
        />
        <template #startPositionTemplate="{ data }">
          <div v-if="data.data.startposition">
            {{
              $moment(data.data.startposition)
                | moment("subtract", "9 hours")
                | moment("HH:mm:ss")
            }}
          </div>
        </template>
        <DxColumn
          cell-template="endPositionTemplate"
          alignment="center"
          caption="음원 끝점"
          :width="100"
        />
        <template #endPositionTemplate="{ data }">
          <div v-if="data.data.duration > data.data.endposition">
            {{
              $moment(data.data.endposition)
                | moment("subtract", "9 hours")
                | moment("HH:mm:ss")
            }}
          </div>
        </template>
        <DxColumn
          data-field="fadeintime"
          alignment="center"
          caption="FadeIn"
          :width="80"
        />
        <DxColumn
          data-field="fadeouttime"
          alignment="center"
          caption="FadeOut"
          :width="80"
        />
        <DxColumn
          cell-template="contentDuration"
          alignment="center"
          caption="소재 길이"
          :width="100"
        />
        <template #contentDuration="{ data }">
          <div>
            {{
              $moment(data.data.endposition - data.data.startposition)
                | moment("subtract", "9 hours")
                | moment("HH:mm:ss")
            }}
          </div>
        </template>
        <DxSummary>
          <DxTotalItem
            :customize-text="customizeDuration"
            show-in-column="소재 길이"
          />
        </DxSummary>
      </DxDataGrid>
    </div>
  </div>
</template>
<script>
import { mapGetters } from "vuex";
import {
  DxDataGrid,
  DxColumn,
  DxScrolling,
  DxPaging,
  DxSummary,
  DxTotalItem,
} from "devextreme-vue/data-grid";
import "moment/locale/ko";
import { formatDate } from "devextreme/localization";
const moment = require("moment");
export default {
  data() {
    return {
      dataSource: [],
      isTotalValue: true,
      maxDuration: 7200000, // 2시간
    };
  },
  mounted() {
    let i = 1;
    for (let item of this.abCartArr) {
      if (item.cartcode != "") {
        let content = { ...item };
        content.rownum = i++;
        this.dataSource.push(content);
      }
    }
  },
  components: {
    DxDataGrid,
    DxColumn,
    DxScrolling,
    DxPaging,
    DxSummary,
    DxTotalItem,
  },
  computed: {
    ...mapGetters("cueList", ["abCartArr"]),
  },
  methods: {
    formatDate,
    customizeDuration() {
      const max = moment(this.maxDuration)
        .subtract(9, "hour")
        .format("HH:mm:ss");
      const sum = this.dataSource.reduce(
        (sum, value) => (sum += value.endposition - value.startposition),
        0
      );
      const result = moment(sum).subtract(9, "hour").format("HH:mm:ss");
      this.isTotalValue = max < result ? false : true;
      this.$emit("isTotalDuration", this.isTotalValue);
      return result;
    },
  },
};
</script>
<style>
.max-duration-color .dx-datagrid-summary-item {
  color: #2d6da3 !important;
}
.wav-ab-grid .dx-datagrid-summary-item {
  color: red;
}
</style>