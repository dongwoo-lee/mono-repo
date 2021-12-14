<template>
  <common-report-modal @close="reportModalOff">
    <h3 slot="header">사용처 수정</h3>

    <h4 slot="body">
      <div style="margin-top: 20px; margin-left: 25px">
        <DxDataGrid
          style="
            width: 350px;
            height: 300px;
            border: 1px solid silver;
            font-family: 'MBC 새로움 M';
          "
          :data-source="pgmData"
          :selection="{ mode: 'single' }"
          :show-borders="true"
          :hover-state-enabled="true"
          key-expr="id"
          :allow-column-resizing="true"
          :column-auto-width="true"
          no-data-text="No Data"
          @row-click="onRowClick"
        >
          <DxPager :visible="true" />
          <DxColumn data-field="name" caption="이벤트 명" />
          <DxColumn data-field="id" caption="이벤트 ID" />
        </DxDataGrid>
        <div style="width: 180px; float: left">
          <b-form-group
            label="이벤트 명"
            class="has-float-label"
            style="margin-top: 20px"
          >
            <b-form-input
              style="width: 180px"
              class="editTask"
              v-model="this.pgmName"
              disabled
              aria-describedby="input-live-help input-live-feedback"
              trim
            />
          </b-form-group>
        </div>
        <div style="width: 170px; margin-left: 20px; float: left">
          <b-form-group
            label="이벤트 ID"
            class="has-float-label"
            style="margin-top: 20px"
          >
            <b-form-input
              style="width: 150px"
              class="editTask"
              v-model="this.pgmid"
              disabled
              aria-describedby="input-live-help input-live-feedback"
              trim
            />
          </b-form-group>
        </div>
      </div>
    </h4>

    <h3 slot="footer">
      <b-button
        variant="outline-primary"
        @click="selectPgm"
        style="margin-right: 10px"
      >
        <span class="label">수정</span>
      </b-button>
    </h3>
  </common-report-modal>
</template>

<script>
import { DxDataGrid, DxColumn, DxPager } from "devextreme-vue/data-grid";
import CommonReportModal from "../../../Modal/CommonReportModal.vue";
export default {
  components: {
    CommonReportModal,
    DxDataGrid,
    DxColumn,
    DxPager,
  },
  props: {
    rowData: {
      type: [],
      default: "",
    },
    pgmData: {
      type: [],
    },
  },
  data() {
    return {
      pgmid: "",
      pgmName: "",
    };
  },
  methods: {
    reportModalOff() {
      this.$emit("reportModalClose");
    },
    selectPgm() {
      var pgm = {
        pgmid: this.pgmid,
        pgmName: this.pgmName,
      };
      this.$emit("changePgm", pgm);
      this.reportModalOff();
    },
    onRowClick(v) {
      this.pgmid = v.data.id;
      this.pgmName = v.data.name;
    },
  },
};
</script>

<style></style>
