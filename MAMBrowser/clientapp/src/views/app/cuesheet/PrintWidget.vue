<template>
  <div class="TabDiv">
    <div>
      <DxDataGrid
        :data-source="cuePrint"
        :ref="dataGridRef"
        :height="printHeight"
        :focusedRowEnabled="false"
        :showColumnLines="true"
        :show-borders="true"
        :showRowLines="true"
        @selection-changed="onSelectionChanged"
        @toolbar-preparing="viewtableOnToolbarPreparing($event)"
        keyExpr="rowNum"
        noDataText="데이터가 없습니다."
      >
        <DxEditing
          :allow-adding="true"
          mode="cell"
          :allow-updating="true"
          start-edit-action="dblClick"
        />
        <DxSelection mode="multiple" showCheckBoxesMode="none" />
        <DxRowDragging
          dropFeedbackMode="indicate"
          :allow-reordering="true"
          :show-drag-icons="false"
          :on-add="onAddPrint"
          :on-reorder="onReorderPrint"
          group="tasksGroup"
        />
        <DxColumn
          width="13%"
          cell-template="code_cell_Template"
          edit-cell-template="code_Template"
          alignment="center"
          data-field="code"
          caption="코드"
        />
        <template #code_cell_Template="{ data: cellInfo }">
          <div>
            <div v-if="cellInfo.data.code == 'CSGP01'">오프닝</div>
            <div v-if="cellInfo.data.code == 'CSGP02'">CM</div>
            <div v-if="cellInfo.data.code == 'CSGP03'">SB</div>
            <div v-if="cellInfo.data.code == 'CSGP04'">SM</div>
            <div v-if="cellInfo.data.code == 'CSGP05'">BGM</div>
            <div v-if="cellInfo.data.code == 'CSGP06'">LOGO</div>
            <div v-if="cellInfo.data.code == 'CSGP07'">M</div>
            <div v-if="cellInfo.data.code == 'CSGP08'">Filler</div>
            <div v-if="cellInfo.data.code == 'CSGP09'">CODE</div>
            <div v-if="cellInfo.data.code == 'CSGP010'"></div>
          </div>
        </template>
        <template #code_Template="{ data: cellInfo }">
          <div>
            <DxSelectBox
              id="mySelectBox"
              :data-source="code_list"
              display-expr="text"
              value-expr="value"
              placeholder=""
              :on-value-changed="
                (value) => onValueChanged_code(value, cellInfo)
              "
              :value="cellInfo.data.code"
              :ref="selectBoxRef"
            />
          </div>
        </template>
        <DxColumn
          caption="내용"
          data-field="contents"
          alignment="center"
          edit-cell-template="content_Template"
        />
        <template #content_Template="{ data: cellInfo }">
          <div>
            <DxTextBox
              :value="cellInfo.data.contents"
              :on-value-changed="
                (value) => onValueChanged_contentsText(value, cellInfo)
              "
            />
          </div>
        </template>
        <DxColumn
          css-class="durationTime"
          data-field="duration"
          caption="사용시간"
          alignment="center"
          edit-cell-template="duration_Template"
          :width="105"
        />
        <template #duration_Template="{ data: cellInfo }">
          <div>
            <DxTextBox
              :value="cellInfo.data.duration"
              :on-value-changed="
                (value) => onValueChanged_duration(value, cellInfo)
              "
            />
          </div>
        </template>
        <DxColumn
          css-class="durationTime"
          data-field="startTime"
          caption="시작시간"
          alignment="center"
          :width="105"
        />
        <DxColumn
          caption="비고"
          :width="130"
          edit-cell-template="etc_Template"
          alignment="center"
          data-field="etc"
        />
        <template #etc_Template="{ data: cellInfo }">
          <div>
            <DxTextBox
              :value="cellInfo.data.etc"
              :on-value-changed="
                (value) => onValueChanged_etcText(value, cellInfo)
              "
            />
          </div>
        </template>
        <DxScrolling mode="virtual" />
        <template #totalGroupCount3>
          <div>
            <DxButton
              :height="34"
              @click="selectionDel"
              :disabled="!selectedItemKeys.length"
              icon="trash"
            />
          </div>
        </template>
        <template #totalGroupCount2>
          <div>
            <DxButton
              id="gridDeleteSelected"
              :height="34"
              icon="print"
              @click="exportGrid()"
            />
          </div>
        </template>
        <template #totalGroupCount_setting>
          <div>
            <DxButton
              id="gridDeleteSelected"
              :height="34"
              icon="preferences"
              v-b-modal.modal-setting
            />
          </div>
        </template>
      </DxDataGrid>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex";
import {
  DxDataGrid,
  DxColumn,
  DxEditing,
  DxScrolling,
  DxSelection,
  DxRowDragging,
} from "devextreme-vue/data-grid";
import DxSelectBox from "devextreme-vue/select-box";
import DxTextBox from "devextreme-vue/text-box";
import DxButton from "devextreme-vue/button";
import { eventBus } from "@/eventBus";

const dataGridRef = "dataGrid";
const selectBoxRef = "selectBox";

export default {
  props: {
    printHeight: Number,
  },
  data() {
    return {
      selectedItemKeys: [],
      printdata: [],
      rowData: {
        rowNum: 0,
        code: "",
        contents: "",
        duration: "",
        startTime: "",
        etc: "",
      },
      code_list: [
        { text: "오프닝", value: "CSGP01" },
        { text: "CM", value: "CSGP02" },
        { text: "SB", value: "CSGP03" },
        { text: "SM", value: "CSGP04" },
        { text: "BGM", value: "CSGP05" },
        { text: "LOGO", value: "CSGP06" },
        { text: "M", value: "CSGP07" },
        { text: "Filler", value: "CSGP08" },
        { text: "CODE", value: "CSGP09" },
        { text: "", value: "CSGP10" },
      ],
      dataGridRef,
      selectBoxRef,
    };
  },
  mounted() {
    if (this.cuePrint.length > 0) {
      this.printdata = this.cuePrint;
      this.rowData.rowNum = this.cuePrint.length;
    }
  },
  created() {
    eventBus.$on("printDataSet", (val) => {
      this.printdata = val;
      this.rowData.rowNum = val.length;
    });
  },
  components: {
    DxDataGrid,
    DxColumn,
    DxEditing,
    DxScrolling,
    DxRowDragging,
    DxSelection,
    DxButton,
    DxSelectBox,
    DxTextBox,
  },
  computed: {
    ...mapGetters("cuesheet", ["searchListData"]),
    ...mapGetters("cuesheet", ["cuePrint"]),
    dataGrid: function () {
      return this.$refs[dataGridRef].instance;
    },
    selectBox: function () {
      return this.$refs[selectBoxRef].instance;
    },
  },
  methods: {
    ...mapMutations("cuesheet", ["SET_CUEPRINT"]),
    onAddPrint(e) {
      console.log(this.cuePrint);
      var selectedRowsData = this.sortSelectedRowsData(e, "data");
      if (selectedRowsData.length > 1) {
        selectedRowsData.forEach((data, index) => {
          var row = { ...this.rowData };
          var search_row = data;
          if (Object.keys(search_row).includes("subtitle")) {
            if (search_row.subtitle == "") {
              row.contents = search_row.memo;
            } else {
              row.contents = search_row.maintitle;
              row.duration = search_row.duration;
            }
          } else {
            switch (this.searchListData.productType) {
              case "PUBLIC_FILE":
                row.contents = search_row.title;
                break;
              case "OLD_PRO":
                row.contents = search_row.name;
                row.duration = search_row.duration;
                break;
              case "SCR_SB":
                row.contents = search_row.name;
                row.duration = search_row.length;
                break;
              case "SCR_SPOT":
                row.contents = search_row.name;
                row.duration = search_row.duration;
                break;
              case "PGM_CM":
                row.contents = search_row.name;
                row.duration = search_row.length;
                break;
              case "CM":
                row.contents = search_row.name;
                row.duration = search_row.length;
                break;
              case "REPOTE":
                row.contents = search_row.name;
                row.duration = search_row.duration;
                break;
              case "FILLER_PR":
                row.contents = search_row.name;
                row.duration = search_row.duration;
                break;
              case "FILLER_MT":
                row.contents = search_row.name;
                row.duration = search_row.duration;
                break;
              case "FILLER_TIME":
                row.contents = search_row.name;
                row.duration = search_row.duration;
                break;
              case "FILLER_ETC":
                row.contents = search_row.name;
                row.duration = search_row.duration;
                break;
              case "PGM":
                row.contents = search_row.name;
                row.duration = search_row.duration;
                break;
              case "MCR_SB":
                row.contents = search_row.name;
                row.duration = search_row.length;
                break;
              case "MCR_SPOT":
                row.contents = search_row.name;
                row.duration = search_row.duration;
                break;

              default:
                break;
            }
          }
          this.printdata.splice(e.toIndex + index, 0, row);
          this.rowData.rowNum = this.rowData.rowNum + 1;
        });
      } else {
        var row = { ...this.rowData };
        var search_row = e.itemData;
        if (e.fromData !== undefined) {
          search_row = e.fromData;
        }
        if (Object.keys(search_row).includes("subtitle")) {
          if (search_row.subtitle == "") {
            row.contents = search_row.memo;
          } else {
            row.contents = search_row.maintitle;
            row.duration = search_row.duration;
          }
        } else {
          switch (this.searchListData.productType) {
            case "PUBLIC_FILE":
              row.contents = search_row.title;
              break;
            case "OLD_PRO":
              row.contents = search_row.name;
              row.duration = search_row.duration;
              break;
            case "SCR_SB":
              row.contents = search_row.name;
              row.duration = search_row.length;
              break;
            case "SCR_SPOT":
              row.contents = search_row.name;
              row.duration = search_row.duration;
              break;
            case "PGM_CM":
              row.contents = search_row.name;
              row.duration = search_row.length;
              break;
            case "CM":
              row.contents = search_row.name;
              row.duration = search_row.length;
              break;
            case "REPOTE":
              row.contents = search_row.name;
              row.duration = search_row.duration;
              break;
            case "FILLER_PR":
              row.contents = search_row.name;
              row.duration = search_row.duration;
              break;
            case "FILLER_MT":
              row.contents = search_row.name;
              row.duration = search_row.duration;
              break;
            case "FILLER_TIME":
              row.contents = search_row.name;
              row.duration = search_row.duration;
              break;
            case "FILLER_ETC":
              row.contents = search_row.name;
              row.duration = search_row.duration;
              break;
            case "PGM":
              row.contents = search_row.name;
              row.duration = search_row.duration;
              break;
            case "MCR_SB":
              row.contents = search_row.name;
              row.duration = search_row.length;
              break;
            case "MCR_SPOT":
              row.contents = search_row.name;
              row.duration = search_row.duration;
              break;

            default:
              break;
          }
        }
        this.printdata.splice(e.toIndex, 0, row);
        this.rowData.rowNum = this.rowData.rowNum + 1;
      }
      this.SET_CUEPRINT(this.printdata);
      // e.fromComponent.clearSelection();
    },
    onReorderPrint(e) {
      this.printdata = [...this.printdata];
      var selectedRowsData = this.sortSelectedRowsData(e, "data");
      var selectedRowsKey = this.sortSelectedRowsData(e, "key");
      var testIndex = [];
      selectedRowsKey.forEach((selectindex) => {
        var index = e.component.getRowIndexByKey(selectindex);
        testIndex.push(index);
      });
      if (!testIndex.includes(e.fromIndex)) {
        selectedRowsData = [];
      }
      if (selectedRowsData.length > 1) {
        var startindex = e.fromIndex;
        var newindex = e.toIndex;
        this.selectionDel();

        if (startindex > e.toIndex) {
          selectedRowsKey.forEach((selectindex) => {
            var index = e.component.getRowIndexByKey(selectindex);
            if (index < e.toIndex) {
              newindex = newindex - 1;
            }
          });
          selectedRowsData.forEach((obj, index) => {
            this.printdata.splice(newindex + index, 0, obj);
          });
        } else {
          selectedRowsKey.forEach((selectindex) => {
            var index = e.component.getRowIndexByKey(selectindex);
            if (index < e.toIndex) {
              newindex = newindex - 1;
            }
          });
          newindex = newindex + 1;
          selectedRowsData.forEach((obj, index) => {
            this.printdata.splice(newindex + index, 0, obj);
          });
        }
      } else {
        this.printdata.splice(e.fromIndex, 1);
        this.printdata.splice(e.toIndex, 0, e.itemData);
      }
      this.SET_CUEPRINT(this.printdata);

      //e.component.clearSelection();
    },
    selectionDel() {
      let a = this.printdata;
      let b = this.selectedItemKeys;
      for (let i = 0; i < b.length; i++) {
        for (let j = 0; j < a.length; j++) {
          if (b[i].rowNum == a[j].rowNum) {
            a.splice(j, 1);
            break;
          }
        }
        this.printdata = a;
      }
      this.SET_CUEPRINT(this.printdata);
    },
    sortSelectedRowsData(e, dataType) {
      var selectedRowsData = e.fromComponent.getSelectedRowsData();
      var selectedRowsKey = this.dataGrid.getSelectedRowKeys();
      if (selectedRowsData.length == 0) {
        return (selectedRowsData = []);
      } else if (Object.keys(selectedRowsData[0]).includes("rowNO")) {
        selectedRowsData.sort(function (a, b) {
          if (a.rowNO > b.rowNO) {
            return 1;
          }
          if (a.rowNO < b.rowNO) {
            return -1;
          }
          return 0;
        });
        return selectedRowsData;
      } else {
        selectedRowsData.forEach((selectindex) => {
          var index = e.fromComponent.getRowIndexByKey(selectindex.rowNum);
          selectindex.rowNum = index;
        });
        selectedRowsData.sort(function (a, b) {
          if (a.rowNum > b.rowNum) {
            return 1;
          }
          if (a.rowNum < b.rowNum) {
            return -1;
          }
          return 0;
        });
        selectedRowsData.forEach((selectindex) => {
          var index = e.fromComponent.getKeyByRowIndex(selectindex.rowNum);
          selectindex.rowNum = index;
        });
        if (selectedRowsKey.length != 0) {
          selectedRowsKey.sort((a, b) => {
            return a - b;
          });
        }
        if (dataType == "data") {
          return selectedRowsData;
        } else if (dataType == "key") {
          return selectedRowsKey;
        }
      }
    },
    onSelectionChanged(e) {
      const selectedRowsData = e.selectedRowsData;
      selectedRowsData.forEach((selectindex) => {
        var index = e.component.getRowIndexByKey(selectindex.rowNum);
        selectindex.rowNum = index;
      });
      selectedRowsData.sort(function (a, b) {
        if (a.rowNum > b.rowNum) {
          return 1;
        }
        if (a.rowNum < b.rowNum) {
          return -1;
        }
        return 0;
      });
      selectedRowsData.forEach((selectindex) => {
        var index = e.component.getKeyByRowIndex(selectindex.rowNum);
        selectindex.rowNum = index;
      });
      this.selectedItemKeys = selectedRowsData;
      //this.selectedItemKeys = e.selectedRowsData;
    },
    onValueChanged_code(value, cellInfo) {
      cellInfo.data.code = value.value;
    },
    onValueChanged_contentsText(value, cellInfo) {
      cellInfo.data.contents = value.value;
    },
    onValueChanged_duration(value, cellInfo) {
      cellInfo.data.duration = value.value;
    },
    onValueChanged_etcText(value, cellInfo) {
      cellInfo.data.etc = value.value;
    },
    viewtableOnToolbarPreparing(e) {
      let toolbarItems = e.toolbarOptions.items;
      toolbarItems.forEach((item) => {
        if (item.name === "addRowButton") {
          item.location = "after";
        }
        if (item.name === "addRowButton") {
          item.options = {
            icon: "add",
            onClick: () => {
              var row = { ...this.rowData };
              var SelectedRowKeys = this.dataGrid.getSelectedRowKeys();
              var rastkey = SelectedRowKeys[SelectedRowKeys.length - 1];
              row.rowNum = this.rowData.rowNum;
              if (rastkey != -1) {
                var index = this.dataGrid.getRowIndexByKey(rastkey);
                this.printdata.splice(index + 1, 0, row);
              } else {
                this.printdata.splice(1, 0, row);
              }
              this.rowData.rowNum = this.rowData.rowNum + 1;
              this.SET_CUEPRINT(this.printdata);
            },
          };
        }
      });

      toolbarItems.push({
        location: "after",
        template: "totalGroupCount2",
      });
      toolbarItems.push({
        location: "after",
        template: "totalGroupCount_setting",
      });
      toolbarItems.push({
        location: "after",
        template: "totalGroupCount3",
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.TabDiv {
  padding: 10px;
}
</style>
