<template>
  <div class="TabDiv">
    <div>
      <div class="print_total_num mt-2 ml-1">
        전체 : {{ printArr.length }}개
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
        keyExpr="rownum"
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
          data-field="usedtime"
          caption="사용시간"
          alignment="center"
          edit-cell-template="duration_Template"
          :width="105"
        />
        <template #duration_Template="{ data: cellInfo }">
          <div>
            <DxTextBox
              :value="cellInfo.data.usedtime"
              :on-value-changed="
                (value) => onValueChanged_duration(value, cellInfo)
              "
            />
          </div>
        </template>
        <DxColumn
          css-class="durationTime"
          data-field="starttime"
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
        <template #deleteTem>
          <div>
            <DxButton
              :height="34"
              @click="selectionDel"
              :disabled="!selectedItemKeys.length"
              icon="trash"
              hint="선택 행 삭제"
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
import { jsPDF } from "jspdf";
import font from "../../../data/font";
import { Workbook } from "exceljs";
import "jspdf-autotable";
import { exportDataGrid as exportDataGridToPdf } from "devextreme/pdf_exporter";
import { exportDataGrid } from "devextreme/excel_exporter";
import { saveAs } from "file-saver";
import "moment/locale/ko";
import {
  AlignmentType,
  Document,
  Packer,
  Paragraph,
  Table,
  TableCell,
  TableRow,
  WidthType,
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
    };
  },
  mounted() {
    if (this.printArr.length > 0) {
      this.rowData.rownum = this.printArr.length + 1;
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
    DxRowDragging,
    DxSelection,
    DxButton,
    DxSelectBox,
    DxTextBox,
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
    //...mapMutations("cueList", ["SET_PRINTARR"]),
    onAddPrint(e) {
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
              row.duration = search_row.duration;
            }
          } else {
            row.usedtime = search_row.intDuration;
            if (this.searchListData.cartcode == "S01G01C011") {
              row.contents = search_row.title;
            } else {
              row.contents = search_row.name;
            }
          }
          arrData.splice(e.toIndex + index, 0, row);
          this.rowData.rownum = this.rowData.rownum + 1;
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
          row.usedtime = search_row.intDuration;
          if (this.searchListData.cartcode == "S01G01C011") {
            row.contents = search_row.title;
          } else {
            row.contents = search_row.name;
          }
        }
        arrData.splice(e.toIndex, 0, row);
        this.rowData.rownum = this.rowData.rownum + 1;
      }
      //this.SET_PRINTARR(arrData);
      // e.fromComponent.clearSelection();
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
      //this.SET_PRINTARR(arrData);

      //e.component.clearSelection();
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
      //this.SET_PRINTARR(arrData);
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
            hint: "행 추가",
            onClick: () => {
              var arrData = this.printArr;
              var row = { ...this.rowData };
              var SelectedRowKeys = this.dataGrid.getSelectedRowKeys();
              var rastkey = SelectedRowKeys[SelectedRowKeys.length - 1];
              row.rownum = this.rowData.rownum;
              if (rastkey != -1) {
                var index = this.dataGrid.getRowIndexByKey(rastkey);
                arrData.splice(index + 1, 0, row);
              } else {
                arrData.splice(1, 0, row);
              }
              this.rowData.rownum = this.rowData.rownum + 1;
              //this.SET_PRINTARR(arrData);
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
                  size: 2000,
                  type: WidthType.DXA,
                },
                children: [new Paragraph(code)],
              }),
              new TableCell({
                width: {
                  size: 5000,
                  type: WidthType.DXA,
                },
                children: [new Paragraph(this.nullChecker(i.contents))],
              }),
              new TableCell({
                width: {
                  size: 1500,
                  type: WidthType.DXA,
                },
                children: [new Paragraph(this.nullChecker(i.duration))],
              }),
              new TableCell({
                width: {
                  size: 1500,
                  type: WidthType.DXA,
                },
                children: [new Paragraph(this.nullChecker(i.starttime))],
              }),
              new TableCell({
                width: {
                  size: 3000,
                  type: WidthType.DXA,
                },
                children: [new Paragraph(this.nullChecker(i.etc))],
              }),
            ],
          })
        );
      });
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
                  new Paragraph(this.nullChecker(this.cueInfo.headertitle)),
                ],
              }),
            },
            footers: {
              default: new Footer({
                children: [
                  new Paragraph(this.nullChecker(this.cueInfo.footertitle)),
                ],
              }),
            },
            children: [
              new Paragraph({
                text: moment(
                  this.cueInfo.detail[0].brdtime,
                  "YYYY-MM-DD'T'HH:mm:ss"
                ).format("YYYY년 MM월 DD일 (ddd)"),
                alignment: AlignmentType.RIGHT,
              }),
              new Paragraph({
                text:
                  "진행 : " +
                  this.cueInfo.djname +
                  " / 연출 : " +
                  this.cueInfo.membername +
                  " / 구성 : " +
                  this.cueInfo.directorname,
                alignment: AlignmentType.RIGHT,
              }),
              table,
            ],
          },
        ],
      });

      Packer.toBlob(doc).then((blob) => {
        saveAs(blob, this.nullChecker(this.cueInfo.headertitle) + ".docx");
      });
    },
    exportGrid(v) {
      var num = 1;
      const doc = new jsPDF();
      this.dataGrid.columnOption("소재명", "visible", false);
      doc.addFileToVFS("MBC NEW M-normal.ttf", font);
      doc.addFont("MBC NEW M-normal.ttf", "MBC NEW M", "normal");
      exportDataGridToPdf({
        jsPDFDocument: doc,
        component: this.dataGrid,
        customizeCell: function (options) {
          const { gridCell, pdfCell } = options;
          pdfCell.styles = {
            font: "MBC NEW M",
            fontSize: 10,
            showHead: "firstPage",
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
        },
        autoTableOptions: {
          margin: { top: 30 },
        },
      }).then(() => {
        this.dataGrid.columnOption("소재명", "visible", true);
        const pageSize = doc.internal.pageSize;
        const pageWidth = pageSize.width ? pageSize.width : pageSize.getWidth();
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
          moment(
            this.cueInfo.detail[0].brdtime,
            "YYYY-MM-DD'T'HH:mm:ss"
          ).format("YYYY년 MM월 DD일 (ddd)"),
          pageWidth - 52,
          20,
          {}
        );
        doc.setFontSize(9);
        doc.text(
          "진행 : " +
            this.cueInfo.djname +
            " / 연출 : " +
            this.cueInfo.membername +
            " / 구성 : " +
            this.cueInfo.directorname,
          pageWidth - 76,
          25,
          {}
        );
        doc.text(
          this.nullChecker(this.cueInfo.footertitle),
          pageWidth / 2 -
            doc.getTextWidth(this.nullChecker(this.cueInfo.footertitle)) / 2,
          pageHeight - 5,
          { baseline: "bottom" }
        );
        if (v == "print") {
          doc.autoPrint();
        } else {
          doc.save(this.nullChecker(this.cueInfo.headertitle) + ".pdf");
        }
        //doc.output("dataurlnewwindow");

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
        customizeCell: function (options) {
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
        },
      }).then(() => {
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
  },
};
</script>
<style lang="scss" scoped>
.TabDiv {
  padding: 10px;
}
.print_total_num {
  right: 180px;
  padding: 0;
  position: absolute;
  z-index: 1;
}
</style>
