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
      :filter="autoSearch ? filter : null"
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
      <template v-slot:cell(codeAndName)="{ item }">
        <div>{{ item.codeAndNameText }}</div>
      </template>
      <template v-slot:cell(actions)="{ item, index }">
        <common-actions
          :rowData="item"
          :config-actions="configActions"
          @modifyConfigRowData="onModifyConfigRowData"
          @deleteConfigRowData="onDeleteConfigRowData"
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
        size="s"
        centered
        ok-title="확인"
        cancel-title="취소"
        @ok="deleteOk"
      >
        <div class="text-center">
          <h5>{{ del_ask_message }}</h5>
        </div>
      </b-modal>
      <b-modal
        id="modal-config-file-del"
        size="lg"
        centered
        ok-title="확인"
        cancel-title="취소"
        @show="permanentlyVal = []"
        @ok="fileDeleteOk"
      >
        <div class="mt-4 text-center">
          <span style="display: inline-flex">
            <h5 v-if="isSelectDelete">선택된 {{ selected.length }} 개의</h5>
            <h5>음원을 삭제하시겠습니까?</h5>
          </span>
          <b-form-checkbox-group
            class="custom-checkbox-group mt-5"
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
    filter: {
      type: String,
      defalut: "",
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
    totalCount: {
      type: Number,
      default: 0,
    },
  },
  data() {
    return {
      ref_table: "refTable",
      del_ask_message: "삭제하시겠습니까?",
      isSelectDelete: false,
      permanentlyDeleteOption: [
        {
          text: "해당 음원을 'MIROS 휴지통'으로 이동하지 않고 영구적으로 삭제합니다.",
          value: false,
        },
      ],
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
        if (this.configActions.includes("file")) {
          this.isSelectDelete = false;
          this.$bvModal.show("modal-config-file-del");
        } else {
          this.$bvModal.show("modal-config-del");
        }
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
    deleteOk() {
      this.$emit("deleteOk", this.rowData, this.permanentlyVal);
    },
    fileDeleteOk() {
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
      this.isSelectDelete = true;
      this.$bvModal.show("modal-config-file-del");
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
</style>
