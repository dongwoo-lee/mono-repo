<template>
  <div>
    <b-col class="my-1" style="display: flex; align-items: center">
      <b-form-group
        label-align="left"
        v-if="configActions.includes('add')"
        class="ml-3"
      >
        <b-button variant="outline-primary default" @click="openAddPopup"
          >➕ {{ addBtnName }}</b-button
        >
      </b-form-group>
      <b-form-group v-if="filterFileds" class="ml-3" style="flex-grow: 0.1">
        <b-input-group size="sm">
          <b-form-input
            id="filter-input"
            v-model="filter"
            type="search"
            :placeholder="placeholderText"
          ></b-form-input>
        </b-input-group>
      </b-form-group>
      <b-form-group
        v-for="(item, index) in selectBoxMenu"
        :key="index"
        style="flex-grow: 0.1"
        :label="item.label"
        class="has-float-label ml-3"
      >
        <b-form-select
          v-model="item.selected"
          :options="item.options"
          @change="onChangeSelectBox($event, item)"
        >
        </b-form-select>
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
      selectedvariant="primary"
      :busy="isLoading"
      :fields="fields"
      :items="dataSource"
      :filter="filter"
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
        <div class="text-nowrap">{{ index + 1 }}</div>
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
    <div>
      <b-modal
        id="modal-config-del"
        size="s"
        centered
        ok-title="확인"
        cancel-title="취소"
        @ok="deleteOk"
      >
        <h5>{{ del_ask_message }}</h5>
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
    placeholderText: {
      type: String,
    },
  },
  data() {
    return {
      ref_table: "refTable",
      del_ask_message: "삭제하시겠습니까?",
      rowData: {},
      filter: "",
      selected: [],
      allSelected: false,
    };
  },
  methods: {
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
    deleteOk() {
      this.$emit("deleteOk", this.rowData);
    },
    onRowSelected(item) {
      this.selected = item;
    },
    selectRow(item, index) {
      if (this.selected) {
        if (this.selected.includes(item)) {
          this.$refs.refTable.selectRow(index);
          console.log(this.selected);
        } else {
          this.$refs.refTable.unselectRow(index);
          console.log(this.selected);
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
  },
};
</script>
