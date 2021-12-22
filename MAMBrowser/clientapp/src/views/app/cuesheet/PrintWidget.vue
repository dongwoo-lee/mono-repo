<template>
  <div class="TabDiv">
    <div>
      <div
        class="mt-2 ml-1"
        :class="{
          print_total_num: cueInfo.cuetype != 'A',
          print_total_num_xs: cueInfo.cuetype == 'A',
        }"
      >
        {{ printArr.length }} / 100
      </div>
      <DxDataGrid
        :data-source="printArr"
        :ref="dataGridRef"
        :height="printHeight"
        :focusedRowEnabled="false"
        :showColumnLines="true"
        :show-borders="true"
        :showRowLines="true"
        @selection-changed="onSelectionChanged"
        @toolbar-preparing="viewtableOnToolbarPreparing($event)"
        @key-down="onKeyDownDel"
        keyExpr="rownum"
        noDataText="데이터가 없습니다."
      >
        <DxEditing
          :allow-adding="true"
          mode="cell"
          :allow-updating="true"
          start-edit-action="dblClick"
          v-if="cueInfo.cuetype != 'A'"
        />
        <DxSelection mode="multiple" showCheckBoxesMode="none" />
        <DxRowDragging
          dropFeedbackMode="indicate"
          :allow-reordering="true"
          :show-drag-icons="false"
          :on-add="onAddPrint"
          :on-reorder="onReorderPrint"
          :on-drag-start="onDragStart"
          group="tasksGroup"
          v-if="cueInfo.cuetype != 'A'"
        />
        <DxLoadPanel :enabled="true" />

        <DxColumn
          width="13%"
          cell-template="code_cell_Template"
          edit-cell-template="code_Template"
          alignment="center"
          :allowSorting="false"
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
          :allowSorting="false"
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
          data-field="usedtime"
          :allowSorting="false"
          caption="사용시간"
          alignment="center"
          :width="105"
          edit-cell-template="etc_usedtime_Template"
          cell-template="usedtime_Template"
        />
        <template #usedtime_Template="{ data }">
          <div v-if="data.data.usedtime != ''">
            {{
              $moment(data.data.usedtime)
                | moment("subtract", "9 hours")
                | moment("HH:mm:ss")
            }}
          </div>
        </template>
        <template #etc_usedtime_Template="{ data: cellInfo }">
          <DxTextBox
            :value="cellInfo.data.usedtime_text"
            mask="00:00:00"
            :on-value-changed="
              (value) => onValueChanged_usedtime(value, cellInfo)
            "
          />
        </template>
        <DxColumn
          css-class="durationTime"
          data-field="starttime"
          caption="시작시간"
          :allowSorting="false"
          alignment="center"
          :width="105"
          cell-template="starttime_Template"
          edit-cell-template="etc_starttime_Template"
        />
        <template #starttime_Template="{ data }">
          <div
            v-if="
              data.data.starttime > 0 &&
              (data.data.usedtime > 0 ||
                data.rowIndex == 0 ||
                data.rowIndex == printArr.length - 1)
            "
          >
            {{ $moment(data.data.starttime) | moment("HH:mm:ss") }}
          </div>
        </template>
        <template #etc_starttime_Template="{ data }">
          <div
            v-if="
              data.data.starttime > 0 &&
              (data.data.usedtime > 0 ||
                data.rowIndex == 0 ||
                data.rowIndex == printArr.length - 1)
            "
          >
            {{ $moment(data.data.starttime) | moment("HH:mm:ss") }}
          </div>
        </template>

        <DxColumn
          caption="비고"
          :width="130"
          :allowSorting="false"
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
        <DxScrolling showScrollbar="always" />
        <DxPaging :enabled="false" />
        <template #deleteTem>
          <div>
            <DxButton
              :height="34"
              @click="selectionDel"
              :disabled="!selectedItemKeys.length"
              icon="trash"
              hint="선택 행 삭제"
              v-if="cueInfo.cuetype != 'A'"
            />
          </div>
        </template>
        <template #printTem>
          <div>
            <DxButton
              id="gridDeleteSelected"
              :height="34"
              icon="print"
              hint="인쇄"
              @click="exportGrid('print')"
            />
          </div>
        </template>
        <template #settingTem>
          <div>
            <DxButton
              id="gridDeleteSelected"
              :height="34"
              icon="preferences"
              hint="추가설정"
              v-b-modal.modal-setting
            />
          </div>
        </template>
      </DxDataGrid>
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters, mapMutations } from "vuex";
import {
  DxDataGrid,
  DxColumn,
  DxEditing,
  DxScrolling,
  DxPaging,
  DxSelection,
  DxRowDragging,
  DxLoadPanel,
} from "devextreme-vue/data-grid";
import DxSelectBox from "devextreme-vue/select-box";
import DxTextBox from "devextreme-vue/text-box";
import DxButton from "devextreme-vue/button";
import { eventBus } from "@/eventBus";
import { jsPDF } from "jspdf";
import font from "../../../data/font";
import { Workbook } from "exceljs";
import "jspdf-autotable";
import { exportDataGrid as exportDataGridToPdf } from "devextreme/pdf_exporter";
import { exportDataGrid } from "devextreme/excel_exporter";
import { saveAs } from "file-saver";
import "moment/locale/ko";
import {
  HeadingLevel,
  AlignmentType,
  VerticalAlign,
  Document,
  Packer,
  Paragraph,
  Table,
  TableCell,
  TableRow,
  WidthType,
  HeightRule,
  Footer,
  Header,
} from "docx";

const dataGridRef = "dataGrid";
const selectBoxRef = "selectBox";
const moment = require("moment");
export default {
  props: {
    printHeight: Number,
  },
  data() {
    return {
      selectedItemKeys: [],
      lengthCheck: false,
      rowData: {
        rownum: 1,
        code: "",
        contents: "",
        usedtime: 0,
        starttime: 0,
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
      brdDate: 0,
    };
  },
  mounted() {
    if (this.printArr.length > 0) {
      this.rowData.rownum = this.printArr.length + 1;
      this.setStartTime();
    }
  },
  created() {
    eventBus.$on("printDataSet", (val) => {
      this.rowData.rownum = this.printArr.length + 1;
    });
    eventBus.$on("exportGo", (val) => {
      switch (val) {
        case ".docx":
          this.exportWord();
          break;
        case ".pdf":
          this.exportGrid("save");
          break;
        case ".excel":
          this.exportExcel();
          break;
        default:
          break;
      }
    });
  },
  components: {
    DxDataGrid,
    DxColumn,
    DxEditing,
    DxScrolling,
    DxPaging,
    DxRowDragging,
    DxSelection,
    DxButton,
    DxSelectBox,
    DxTextBox,
    DxLoadPanel,
  },
  computed: {
    ...mapGetters("cueList", ["searchListData"]),
    ...mapGetters("cueList", ["cueInfo"]),
    ...mapGetters("cueList", ["printArr"]),

    dataGrid: function () {
      return this.$refs[dataGridRef].instance;
    },
    selectBox: function () {
      return this.$refs[selectBoxRef].instance;
    },
  },
  methods: {
    ...mapMutations("cueList", ["SET_PRINTARR"]),
    ...mapActions("cueList", ["setStartTime"]),
    onAddPrint(e) {
      this.lengthCheck = false;
      var arrData = this.printArr;
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
              row.usedtime = search_row.endposition - search_row.startposition;
            }
          } else {
            row.usedtime = search_row.intDuration;
            row.contents = search_row.name;
          }

          var checkValue = this.maxLengthCheck();
          if (checkValue) {
            arrData.splice(e.toIndex + index, 0, row);
            this.rowData.rownum = this.rowData.rownum + 1;
          }
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
            row.usedtime = search_row.endposition - search_row.startposition;
          }
        } else {
          row.usedtime = search_row.intDuration;
          switch (this.searchListData.cartcode) {
            case "S01G01C007":
              row.contents = search_row.title;
              break;
            case "S01G01C006":
              row.contents = search_row.recName;
              break;
            default:
              row.contents = search_row.name;
              break;
          }
        }

        var checkValue = this.maxLengthCheck();
        if (checkValue) {
          arrData.splice(e.toIndex, 0, row);
          this.rowData.rownum = this.rowData.rownum + 1;
        }
      }
      this.setStartTime();
      if (this.lengthCheck) {
        window.$notify("error", `최대 개수를 초과하였습니다.`, "", {
          duration: 10000,
          permanent: false,
        });
      }
    },
    maxLengthCheck() {
      var result = true;
      if (this.printArr.length > 99) {
        result = false;
        this.lengthCheck = true;
      }
      return result;
    },
    onReorderPrint(e) {
      var arrData = this.printArr;
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
            arrData.splice(newindex + index, 0, obj);
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
            arrData.splice(newindex + index, 0, obj);
          });
        }
      } else {
        arrData.splice(e.fromIndex, 1);
        arrData.splice(e.toIndex, 0, e.itemData);
      }
      this.setStartTime();
    },
    selectionDel() {
      var arrData = this.printArr;
      let a = arrData;
      let b = this.selectedItemKeys;
      for (let i = 0; i < b.length; i++) {
        for (let j = 0; j < a.length; j++) {
          if (b[i].rownum == a[j].rownum) {
            a.splice(j, 1);
            break;
          }
        }
        arrData = a;
      }
      this.SET_PRINTARR(arrData);
      if (this.printArr.length > 0) {
        this.setStartTime();
      }
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
          var index = e.fromComponent.getRowIndexByKey(selectindex.rownum);
          selectindex.rownum = index;
        });
        selectedRowsData.sort(function (a, b) {
          if (a.rownum > b.rownum) {
            return 1;
          }
          if (a.rownum < b.rownum) {
            return -1;
          }
          return 0;
        });
        selectedRowsData.forEach((selectindex) => {
          var index = e.fromComponent.getKeyByRowIndex(selectindex.rownum);
          selectindex.rownum = index;
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
        var index = e.component.getRowIndexByKey(selectindex.rownum);
        selectindex.rownum = index;
      });
      selectedRowsData.sort(function (a, b) {
        if (a.rownum > b.rownum) {
          return 1;
        }
        if (a.rownum < b.rownum) {
          return -1;
        }
        return 0;
      });
      selectedRowsData.forEach((selectindex) => {
        var index = e.component.getKeyByRowIndex(selectindex.rownum);
        selectindex.rownum = index;
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
    onValueChanged_usedtime(value, cellInfo) {
      if (value.value != "") {
        cellInfo.data.usedtime_text = value.value;
        var time1 = moment("00:00:00", "HH:mm:ss");
        var time2 = moment(value.value, "HH:mm:ss");
        cellInfo.data.usedtime = moment
          .duration(time2.diff(time1))
          .asMilliseconds();
      } else {
        cellInfo.data.usedtime = 0;
      }
      this.setStartTime();
    },
    onValueChanged_etcText(value, cellInfo) {
      cellInfo.data.etc = value.value;
    },
    onDragStart() {
      document.getElementById("app-container").classList.add("drag_");
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
            hint: "행 추가",
            onClick: () => {
              this.lengthCheck = false;
              var arrData = this.printArr;
              var row = { ...this.rowData };
              var SelectedRowKeys = this.dataGrid.getSelectedRowKeys();
              var rastkey = SelectedRowKeys[SelectedRowKeys.length - 1];
              row.rownum = this.rowData.rownum;
              if (rastkey != -1) {
                var index = this.dataGrid.getRowIndexByKey(rastkey);
                var checkValue = this.maxLengthCheck();
                if (checkValue) {
                  arrData.splice(index + 1, 0, row);
                  this.rowData.rownum = this.rowData.rownum + 1;
                }
              } else {
                var checkValue = this.maxLengthCheck();
                if (checkValue) {
                  arrData.splice(1, 0, row);
                  this.rowData.rownum = this.rowData.rownum + 1;
                }
              }
              this.setStartTime();
              if (this.lengthCheck) {
                window.$notify("error", `최대 개수를 초과하였습니다.`, "", {
                  duration: 10000,
                  permanent: false,
                });
              }
            },
          };
        }
      });

      toolbarItems.push({
        location: "after",
        template: "printTem",
      });
      toolbarItems.push({
        location: "after",
        template: "settingTem",
      });
      toolbarItems.push({
        location: "after",
        template: "deleteTem",
      });
    },
    exportWord() {
      const rows = [];
      var djname = this.cueInfo.djname != undefined ? this.cueInfo.djname : "";
      var membername =
        this.cueInfo.membername != undefined ? this.cueInfo.membername : "";
      var directorname =
        this.cueInfo.directorname != undefined ? this.cueInfo.directorname : "";

      rows.push(
        new TableRow({
          children: [
            new TableCell({
              width: {
                size: 2000,
                type: WidthType.DXA,
              },
              children: [
                new Paragraph({
                  text: "코드",
                  alignment: AlignmentType.CENTER,
                }),
              ],
              verticalAlign: VerticalAlign.CENTER,
            }),
            new TableCell({
              width: {
                size: 5000,
                type: WidthType.DXA,
              },
              children: [
                new Paragraph({
                  text: this.nullChecker("내용"),
                  alignment: AlignmentType.CENTER,
                }),
              ],
              verticalAlign: VerticalAlign.CENTER,
            }),
            new TableCell({
              width: {
                size: 1500,
                type: WidthType.DXA,
              },
              children: [
                new Paragraph({
                  text: this.nullChecker("사용시간"),
                  alignment: AlignmentType.CENTER,
                }),
              ],
              verticalAlign: VerticalAlign.CENTER,
            }),
            new TableCell({
              width: {
                size: 1500,
                type: WidthType.DXA,
              },
              children: [
                new Paragraph({
                  text: this.nullChecker("시작시간"),
                  alignment: AlignmentType.CENTER,
                }),
              ],
              verticalAlign: VerticalAlign.CENTER,
            }),
            new TableCell({
              width: {
                size: 3000,
                type: WidthType.DXA,
              },
              children: [
                new Paragraph({
                  text: this.nullChecker("비고"),
                  alignment: AlignmentType.CENTER,
                }),
              ],
              verticalAlign: VerticalAlign.CENTER,
            }),
          ],
          tableHeader: true,
          height: { value: 500, rule: HeightRule.ATLEAST },
        })
      );

      var num = 1;
      this.printArr.forEach((i) => {
        var code = this.exportCode(i.code);
        if (code == "M") {
          code = code + num;
          num = num + 1;
        }
        rows.push(
          new TableRow({
            children: [
              new TableCell({
                width: {
                  size: 1900,
                  type: WidthType.DXA,
                },
                children: [
                  new Paragraph({
                    text: code,
                    alignment: AlignmentType.CENTER,
                  }),
                ],
                verticalAlign: VerticalAlign.CENTER,
              }),
              new TableCell({
                width: {
                  size: 5500,
                  type: WidthType.DXA,
                },
                children: [
                  new Paragraph(
                    this.nullChecker({
                      text: i.contents,
                      alignment: AlignmentType.CENTER,
                    })
                  ),
                ],
                verticalAlign: VerticalAlign.CENTER,
              }),
              new TableCell({
                width: {
                  size: 1400,
                  type: WidthType.DXA,
                },
                children: [
                  new Paragraph({
                    text:
                      i.usedtime == 0
                        ? ""
                        : moment(i.usedtime)
                            .subtract(9, "hours")
                            .format("HH:mm:ss"),
                    alignment: AlignmentType.CENTER,
                  }),
                ],
                verticalAlign: VerticalAlign.CENTER,
              }),
              new TableCell({
                width: {
                  size: 1400,
                  type: WidthType.DXA,
                },
                children: [
                  new Paragraph({
                    text:
                      i.starttime > 0 &&
                      (i.usedtime > 0 ||
                        this.printArr[0].rownum == i.rownum ||
                        i.rownum ==
                          this.printArr[this.printArr.length - 1].rownum)
                        ? moment(i.starttime).format("HH:mm:ss")
                        : "",
                    alignment: AlignmentType.CENTER,
                  }),
                ],
                verticalAlign: VerticalAlign.CENTER,
              }),
              new TableCell({
                width: {
                  size: 2800,
                  type: WidthType.DXA,
                },
                children: [
                  new Paragraph({
                    text: this.nullChecker(i.etc),
                    alignment: AlignmentType.CENTER,
                  }),
                ],
                verticalAlign: VerticalAlign.CENTER,
              }),
            ],
            height: { value: 500, rule: HeightRule.ATLEAST },
          })
        );
      });
      //rows[1].height = { height: 2500, rule: HeightRule.ATLEAST };
      const table = new Table({
        columnWidths: [3505, 5505],
        rows: rows,
      });
      const doc = new Document({
        sections: [
          {
            headers: {
              default: new Header({
                children: [
                  new Paragraph({
                    text: this.nullChecker(this.cueInfo.headertitle),
                    heading: HeadingLevel.HEADING_2,
                    alignment: AlignmentType.CENTER,
                  }),
                ],
              }),
            },
            footers: {
              default: new Footer({
                children: [
                  new Paragraph({
                    text: this.nullChecker(this.cueInfo.footertitle),
                    heading: HeadingLevel.HEADING_6,
                    alignment: AlignmentType.CENTER,
                  }),
                ],
              }),
            },
            children: [
              new Paragraph({
                text: Object.keys(this.cueInfo).includes("detail")
                  ? moment(
                      this.cueInfo.brdtime,
                      "YYYY-MM-DD'T'HH:mm:ss"
                    ).format("YYYY년 MM월 DD일 (ddd)")
                  : moment(this.cueInfo.day, "YYYY-MM-DD'T'HH:mm:ss").format(
                      "YYYY년 MM월 DD일 (ddd)"
                    ),
                alignment: AlignmentType.RIGHT,
              }),
              new Paragraph({
                text:
                  "진행 : " +
                  djname +
                  " / 연출 : " +
                  membername +
                  " / 구성 : " +
                  directorname,
                alignment: AlignmentType.RIGHT,
              }),
              table,
            ],
          },
        ],
        styles: {
          default: {
            heading2: {
              run: {
                size: 28,
                bold: true,
                font: "MBC 새로움 M",
                color: "black",
              },
            },
            heading6: {
              run: {
                size: 18,
                bold: false,
                font: "MBC 새로움 M",
                color: "black",
              },
            },
          },
          paragraphStyles: [
            {
              name: "Normal",
              run: {
                font: "MBC 새로움 M",
              },
            },
          ],
        },
      });

      Packer.toBlob(doc).then((blob) => {
        saveAs(blob, this.nullChecker(this.cueInfo.headertitle) + ".docx");
      });
    },
    exportGrid(v) {
      var num = 1;
      var djname = this.cueInfo.djname != undefined ? this.cueInfo.djname : "";
      var membername =
        this.cueInfo.membername != undefined ? this.cueInfo.membername : "";
      var directorname =
        this.cueInfo.directorname != undefined ? this.cueInfo.directorname : "";
      const doc = new jsPDF();
      //this.dataGrid.columnOption("소재명", "visible", false);
      doc.addFileToVFS("MBC NEW M-normal.ttf", font);
      doc.addFont("MBC NEW M-normal.ttf", "MBC NEW M", "normal");
      this.dataGrid.columnOption("소재명", "visible", true);

      exportDataGridToPdf({
        jsPDFDocument: doc,
        component: this.dataGrid,
        customizeCell: (options) => {
          const { gridCell, pdfCell } = options;
          pdfCell.styles = {
            font: "MBC NEW M",
            fontSize: 10,
            showHead: "firstPage",
            valign: "middle",
            halign: "center",
          };
          switch (pdfCell.content) {
            case "CSGP01":
              pdfCell.content = "오프닝";
              break;
            case "CSGP02":
              pdfCell.content = "CM";
              break;
            case "CSGP03":
              pdfCell.content = "SB";
              break;
            case "CSGP04":
              pdfCell.content = "SM";
              break;
            case "CSGP05":
              pdfCell.content = "BGM";
              break;
            case "CSGP06":
              pdfCell.content = "LOGO";
              break;
            case "CSGP07":
              pdfCell.content = "M" + num;
              num = num + 1;
              break;
            case "CSGP08":
              pdfCell.content = "Filler";
              break;
            case "CSGP09":
              pdfCell.content = "CODE";
              break;
            case "CSGP10":
              pdfCell.content = "";
              break;
            default:
              break;
          }
          if (
            gridCell.rowType == "data" &&
            gridCell.column.dataField == "usedtime"
          ) {
            gridCell.data.usedtime == 0
              ? (pdfCell.content = "")
              : (pdfCell.content = moment(gridCell.data.usedtime)
                  .subtract(9, "hours")
                  .format("HH:mm:ss"));
          }
          if (
            gridCell.rowType == "data" &&
            gridCell.column.dataField == "starttime"
          ) {
            gridCell.data.starttime > 0 &&
            (gridCell.data.usedtime > 0 ||
              this.printArr[0].rownum == gridCell.data.rownum ||
              gridCell.data.rownum ==
                this.printArr[this.printArr.length - 1].rownum)
              ? (pdfCell.content = moment(gridCell.data.starttime).format(
                  "HH:mm:ss"
                ))
              : (pdfCell.content = "");
          }
        },
        autoTableOptions: {
          margin: { top: 30 },
        },
      }).then(() => {
        const pageCount = doc.internal.getNumberOfPages();

        for (var i = 1; i <= pageCount; i++) {
          doc.setPage(i);
          const pageSize = doc.internal.pageSize;
          const pageWidth = pageSize.width
            ? pageSize.width
            : pageSize.getWidth();
          const pageHeight = pageSize.height
            ? pageSize.height
            : pageSize.getHeight();
          pageHeight;
          doc.setFontSize(18);
          doc.setFont("MBC NEW M");
          doc.text(
            this.nullChecker(this.cueInfo.headertitle),
            pageWidth / 2 -
              doc.getTextWidth(this.nullChecker(this.cueInfo.headertitle)) / 2,
            5,
            {
              baseline: "top",
            }
          );
          doc.setFontSize(10);
          doc.text(
            Object.keys(this.cueInfo).includes("detail")
              ? moment(this.cueInfo.brdtime, "YYYY-MM-DD'T'HH:mm:ss").format(
                  "YYYY년 MM월 DD일 (ddd)"
                )
              : moment(this.cueInfo.day, "YYYY-MM-DD'T'HH:mm:ss").format(
                  "YYYY년 MM월 DD일 (ddd)"
                ),
            pageWidth - 15,
            20,
            { align: "right" }
          );
          doc.setFontSize(9);
          doc.text(
            "진행 : " +
              djname +
              " / 연출 : " +
              membername +
              " / 구성 : " +
              directorname,
            pageWidth - 15,
            25,
            { align: "right" }
          );
          doc.text(
            this.nullChecker(this.cueInfo.footertitle),
            pageWidth / 2 -
              doc.getTextWidth(this.nullChecker(this.cueInfo.footertitle)) / 2,
            pageHeight - 5,
            { baseline: "bottom" }
          );
        }
        if (v == "print") {
          doc.autoPrint();
        } else {
          doc.save(this.nullChecker(this.cueInfo.headertitle) + ".pdf");
        }

        const hiddFrame = document.createElement("iframe");
        hiddFrame.style.position = "fixed";
        hiddFrame.style.width = "1px";
        hiddFrame.style.height = "1px";
        hiddFrame.style.opacity = "0.01";
        const isSafari = /^((?!chrome|android).)*safari/i.test(
          window.navigator.userAgent
        );
        if (isSafari) {
          hiddFrame.onload = () => {
            try {
              hiddFrame.contentWindow.document.execCommand(
                "print",
                false,
                null
              );
            } catch (e) {
              hiddFrame.contentWindow.print();
            }
          };
        }
        hiddFrame.src = doc.output("bloburl");
        document.body.appendChild(hiddFrame);
      });
    },
    exportExcel() {
      var num = 1;
      const workbook = new Workbook();
      const worksheet = workbook.addWorksheet("dataGrid");

      exportDataGrid({
        component: this.dataGrid,
        worksheet: worksheet,
        autoFilterEnabled: false,
        customizeCell: (options) => {
          const { excelCell, gridCell } = options;
          switch (excelCell.value) {
            case "CSGP01":
              excelCell.value = "오프닝";
              break;
            case "CSGP02":
              excelCell.value = "CM";
              break;
            case "CSGP03":
              excelCell.value = "SB";
              break;
            case "CSGP04":
              excelCell.value = "SM";
              break;
            case "CSGP05":
              excelCell.value = "BGM";
              break;
            case "CSGP06":
              excelCell.value = "LOGO";
              break;
            case "CSGP07":
              excelCell.value = "M" + num;
              num = num + 1;
              break;
            case "CSGP08":
              excelCell.value = "Filler";
              break;
            case "CSGP09":
              excelCell.value = "CODE";
              break;
            case "CSGP10":
              excelCell.value = "";
              break;
            default:
              break;
          }
          if (
            gridCell.rowType == "data" &&
            gridCell.column.dataField == "usedtime"
          ) {
            gridCell.data.usedtime == 0
              ? (excelCell.value = "")
              : (excelCell.value = moment(gridCell.data.usedtime)
                  .subtract(9, "hours")
                  .format("HH:mm:ss"));
          }
          if (
            gridCell.rowType == "data" &&
            gridCell.column.dataField == "starttime"
          ) {
            gridCell.data.starttime > 0 &&
            (gridCell.data.usedtime > 0 ||
              this.printArr[0].rownum == gridCell.data.rownum ||
              gridCell.data.rownum ==
                this.printArr[this.printArr.length - 1].rownum)
              ? (excelCell.value = moment(gridCell.data.starttime).format(
                  "HH:mm:ss"
                ))
              : (excelCell.value = "");
          }
        },
      }).then(() => {
        worksheet.columns.forEach((column) => {
          column.border = {
            top: { style: "thin" },
            left: { style: "thin" },
            bottom: { style: "thin" },
            right: { style: "thin" },
          };
        });
        workbook.xlsx.writeBuffer().then((buffer) => {
          saveAs(
            new Blob([buffer], { type: "application/octet-stream" }),
            this.nullChecker(this.cueInfo.headertitle) + "xlsx.xlsx"
          );
        });
      });
    },
    nullChecker(text) {
      if (text == null) {
        return "";
      } else {
        return text;
      }
    },
    exportCode(code) {
      var codeText;
      this.code_list.filter((ele) => {
        if (ele.value == code) {
          codeText = ele.text;
        }
      });
      if (codeText == null) {
        codeText = "";
      }
      return codeText;
    },
    onKeyDownDel(e) {
      if (e.event.key == "Delete") {
        this.selectionDel();
      }
    },
  },
};
</script>
<style lang="scss" scoped>
.drag_ {
  position: fixed;
  height: 100%;
  overflow: auto;
}
.TabDiv {
  padding: 10px;
}
.print_total_num {
  right: 180px;
  padding: 0;
  position: absolute;
  z-index: 1;
}
.print_total_num_xs {
  right: 105px;
  padding: 0;
  position: absolute;
  z-index: 1;
}
</style>
