<template>
  <div>
    <b-col
      class="my-1"
      style="display: flex; position: relative; align-items: center"
    >
      <b-form-group
        label-align="left"
        v-if="configActions.includes('select_del')"
        class="ml-3"
      >
        <b-button
          :disabled="selected.length === 0 ? true : false"
          variant="outline-danger default"
          @click="onDeleteSelectedItems"
          >➖ 선택삭제</b-button
        >
      </b-form-group>
      <b-form-group
        label-align="left"
        v-if="configActions.includes('add')"
        class="ml-3"
      >
        <b-button variant="outline-primary default" @click="openAddPopup"
          >➕ {{ addBtnName }}</b-button
        >
      </b-form-group>
      <div
        v-for="(item, index) in selectBoxMenu"
        :key="index"
        v-bind:class="{ 'select-menu': item.type === 'selectBox' }"
      >
        <b-form-group
          v-if="item.type === 'selectBox'"
          :label="item.label"
          class="select-box-menu has-float-label ml-3"
        >
          <b-form-select
            v-model="item.selected"
            :options="item.options"
            @change="onChangeSelectBox($event, item)"
          >
          </b-form-select>
        </b-form-group>
        <!-- :maxPeriodMonth="12" -->
        <common-start-end-date-picker
          v-if="item.type === 'startEndDate'"
          :startDate.sync="startDate"
          :endDate.sync="endDate"
          :maxLimitDate="maxLimitDate"
          :minLimitDate="minLimitDate"
          :editTextVal="true"
          :required="false"
          :isCurrentDate="false"
          class="start_end_date_picker"
          @SEDateEvent="onSearch(item)"
        />
        <b-form-group
          v-if="item.type === 'brdDate'"
          label="방송일"
          class="has-float-label ml-3"
        >
          <common-date-picker
            @input="onSearch(item)"
            v-model="brdDate"
            class="brd_date_picker"
          />
        </b-form-group>
      </div>

      <b-form-group v-if="filterFileds" class="ml-3" style="flex-grow: 0.1">
        <b-input-group>
          <b-form-input
            id="filter-input"
            type="search"
            v-model="searchText"
            :placeholder="placeholderText"
          ></b-form-input>
          <b-input-group-append v-if="!autoSearch">
            <b-button variant="outline-primary default" @click="searchEvent"
              >검색</b-button
            >
          </b-input-group-append>
        </b-input-group>
      </b-form-group>

      <b-form-group
        label-align="left"
        v-if="deleteOptionButtonTitle"
        class="ml-3"
      >
        <b-button variant="primary default" @click="deleteOptionEvent">
          {{ deleteOptionButtonTitle }}
        </b-button>
      </b-form-group>

      <b-form-group class="ml-3" v-if="isRevocationExcept">
        <b-form-checkbox
          id="checkbox-1"
          v-model="revocationExceptVal"
          value="(폐지)"
          unchecked-value=""
          @input="onRevocationInput()"
        >
          폐지된 프로그램 제외
        </b-form-checkbox>
      </b-form-group>
    </b-col>

    <b-table
      :ref="ref_table"
      sort-by="title"
      sort-desc.sync="false"
      selectable
      show-empty
      empty-text="데이터가 없습니다."
      select-mode="multi"
      selectedVariant="primary"
      :busy="isLoading"
      :fields="fields"
      :items="dataSource"
      :filter="autoSearch ? searchText : null"
      :filter-included-fields="filterFileds"
      @row-selected="onRowSelected"
      sticky-header="600px"
    >
      <template v-slot:head()="data">
        <b-form-checkbox
          v-if="data.label === '__select'"
          v-model="allSelected"
          @change="selectAllRows"
        ></b-form-checkbox>
        <div v-else style="margin: 3px">{{ data.label }}</div>
      </template>
      <template #cell()="{ item, value }">
        <div v-if="value.includes('__filePath :')">
          <b-icon
            icon="file-earmark"
            scale="1.3"
            v-b-tooltip.hover="value"
          ></b-icon>
        </div>
        <div v-else>{{ value }}</div>
      </template>
      <template #cell(selected)="{ item, index }">
        <b-form-checkbox
          v-model="selected"
          :value="item"
          @change="selectRow(item, index)"
        ></b-form-checkbox>
      </template>
      <template v-slot:cell(no)="{ index }">
        <div class="text-nowrap">
          {{ index + 1 + (currentPage - 1) * perPage }}
        </div>
      </template>
      <template v-slot:table-busy>
        <div class="text-center text-primary my-2">
          <b-spinner class="align-middle"></b-spinner>
          <strong>loading...</strong>
        </div>
      </template>
      <template v-slot:cell(actions)="{ item, index }">
        <common-actions
          :rowData="item"
          :config-actions="configActions"
          @modifyConfigRowData="onModifyConfigRowData"
          @deleteConfigRowData="onDeleteConfigRowData"
          @downloadConfigRowData="onDownloadConfigRowData"
        />
      </template>
    </b-table>

    <div v-if="pageOptions">
      <b-col
        md="12"
        class="my-1"
        style="position: absolute; bottom: 0; right: 0"
      >
        <b-pagination
          v-model="currentPage"
          :total-rows="totalCount"
          :per-page="perPage"
          class="my-0"
          style="justify-content: center"
          size="sm"
          @input="onSelectCurrentPage"
        ></b-pagination>
      </b-col>
      <b-col
        md="3"
        class="my-1"
        style="position: absolute; bottom: 0; right: 30px"
      >
        <b-form-group
          :label="'전체 :' + totalCount + '개'"
          label-for="per-page-select"
          label-cols-md="9"
          label-align-sm="right"
          label-size="sm"
          class="mb-0"
        >
          <b-form-select
            id="per-page-select"
            v-model="perPage"
            :options="pageOptions"
            @input="onChangePerPage"
          ></b-form-select>
        </b-form-group>
      </b-col>
    </div>
    <div>
      <b-modal
        id="modal-config-del"
        size="lg"
        centered
        no-close-on-backdrop
        ok-title="확인"
        cancel-title="취소"
        @show="permanentlyVal = []"
        @ok="deleteOk"
      >
        <b-overlay :show="overlayVal" no-wrap />
        <div class="text-center">
          <div v-if="delName && rowData[delName]">
            <span style="display: inline-flex">
              <h5>"{{ rowData[delName] }}" 을(를) 삭제하시겠습니까?</h5>
            </span>
          </div>
          <h5 v-else>{{ del_ask_message }}</h5>
          <b-form-checkbox-group
            v-if="permanentlyDeleteOption"
            class="custom-checkbox-group mt-3"
            style="font-size: 16px"
            v-model="permanentlyVal"
            :options="permanentlyDeleteOption"
            value-field="value"
            text-field="text"
          />
        </div>
      </b-modal>
      <b-modal
        id="modal-config-file-del"
        size="lg"
        centered
        no-close-on-backdrop
        ok-title="확인"
        cancel-title="취소"
        @show="permanentlyVal = []"
        @ok="fileDeleteOk"
      >
        <b-overlay :show="overlayVal" no-wrap />
        <div class="mt-3 text-center">
          <span>
            <h5 v-if="selected.length === 1 && delName && selected[0][delName]">
              "{{ selected[0][delName] }}" 항목을 삭제하시겠습니까?
            </h5>
            <h5 v-else>
              선택된 {{ selected.length }} 개의 항목을 삭제하시겠습니까?
            </h5>
          </span>
          <b-form-checkbox-group
            v-if="permanentlyDeleteOption"
            class="custom-checkbox-group mt-3"
            style="font-size: 16px"
            v-model="permanentlyVal"
            :options="permanentlyDeleteOption"
            value-field="value"
            text-field="text"
          />
        </div>
      </b-modal>
    </div>
  </div>
</template>
<script>
export default {
  props: {
    fields: {
      type: Array,
      default: () => [],
    },
    dataSource: {
      type: Array,
      default: () => [],
    },
    isLoading: {
      type: Boolean,
      default: false,
    },
    configActions: {
      type: Array,
      default: () => [],
    },
    selectBoxMenu: {
      type: Array,
      defalut: () => [],
    },
    addBtnName: {
      type: String,
    },
    filterFileds: {
      type: Array,
      defalut: () => [],
    },
    autoSearch: {
      type: Boolean,
      default: true,
    },
    placeholderText: {
      type: String,
    },
    start_dt: {
      type: String,
      default: "",
    },
    end_dt: {
      type: String,
      default: "",
    },
    brd_dt: {
      type: String,
      default: "",
    },
    selectActions: {
      type: Array,
      defalut: () => [],
    },
    pageOptions: {
      type: Array,
      defalut: () => [],
    },
    deleteOptionButtonTitle: {
      type: String,
      default: "",
    },
    delName: {
      type: String,
      default: "",
    },
    permanentlyDeleteOption: {
      type: Array,
      default: () => [],
    },
    totalCount: {
      type: Number,
      default: 0,
    },
    isRevocationExcept: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      ref_table: "refTable",
      del_ask_message: "삭제하시겠습니까?",
      permanentlyVal: [],
      rowData: {},
      searchText: "",
      selected: [],
      allSelected: false,
      startDate: "",
      endDate: "",
      brdDate: "",
      currentPage: 1,
      perPage: 0,
      maxLimitDate: null,
      minLimitDate: null,
      overlayVal: false,
      revocationExceptVal: "(폐지)",
    };
  },
  created() {
    this.startDate = this.start_dt;
    this.endDate = this.end_dt;
    this.brdDate = this.brd_dt;
    if (this.pageOptions) {
      this.perPage = this.pageOptions[0];
    }
  },

  methods: {
    onRevocationInput() {
      this.$emit("revocationInput", this.revocationExceptVal);
      this.revocationExceptVal = "(폐지)"
    },
    clearSearchText() {
      this.searchText = "";
    },
    searchEvent() {
      this.currentPage = 1;
      this.$emit("searchEvent", this.searchText);
    },
    setLimitDate(item) {
      const year = item.substr(0, 4);
      const month = item.substr(4, 2) - 1; // 월은 0부터 시작하므로 1을 뺍니다.
      const day = item.substr(6, 2);
      return new Date(year, month, day);
    },
    onSearch(item) {
      const itemData = {};
      switch (item.key) {
        case "brdDate":
          itemData.brdDate = this.brdDate;
          break;
        case "startEndDate":
          itemData.startDate = this.startDate;
          itemData.endDate = this.endDate;
          this.maxLimitDate = this.setLimitDate(this.endDate);
          this.minLimitDate = this.setLimitDate(this.startDate);
          break;
        default:
          break;
      }
      this.$emit("selectDate", itemData);
    },
    onModifyConfigRowData(rowData) {
      this.$emit("modifyConfigRowData", rowData);
    },
    onDeleteConfigRowData(rowData) {
      if (rowData) {
        this.rowData = rowData;
        this.$bvModal.show("modal-config-del");
      }
    },
    onChangeSelectBox(event, item) {
      this.$emit("changeSelectBox", event, item);
    },
    openAddPopup() {
      this.$emit("openAddPopup");
    },
    onSelectCurrentPage() {
      this.$emit("selectCurrentPage", this.currentPage);
    },
    onChangePerPage() {
      this.$emit("changePerPage", this.perPage);
    },
    deleteOk(e) {
      e.preventDefault();
      this.$emit("deleteOk", this.rowData, this.permanentlyVal);
    },
    fileDeleteOk(e) {
      e.preventDefault();
      this.$emit("deleteSelectedItems", this.selected, this.permanentlyVal);
    },
    onRowSelected(item) {
      this.selected = item;
    },
    selectRow(item, index) {
      if (this.selected) {
        if (this.selected.includes(item)) {
          this.$refs.refTable.selectRow(index);
        } else {
          this.$refs.refTable.unselectRow(index);
        }
      }
    },
    selectAllRows() {
      if (this.allSelected) {
        this.$refs.refTable.selectAllRows();
      } else {
        this.$refs.refTable.clearSelected();
      }
    },
    onDeleteSelectedItems() {
      this.$bvModal.show("modal-config-file-del");
    },
    onDownloadConfigRowData(rowData) {
      this.$emit("downloadConfigRowData", rowData);
    },
    deleteOptionEvent() {
      this.$emit("deleteOptionEvent");
    },
    onOverlayValTrue() {
      this.overlayVal = true;
    },
    onOverlayValFalse() {
      this.overlayVal = false;
    },
  },
};
</script>
<style>
.select-menu {
  flex-grow: 0.08;
}
.start_end_date_picker .form-group {
  margin-left: 15px;
}
.start_end_date_picker .row {
  flex-wrap: inherit;
}
.start_end_date_picker .input-group {
  width: 180px;
}
.brd_date_picker .input-group {
  width: 180px;
}
#modal-config-file-del .modal-body {
  position: unset;
}
#modal-config-del .modal-body {
  position: unset;
}
</style>
