<template>
  <div>
    <div class="abchannel_view">
      <div class="abchannel_title mt-2 ml-1">DAP (A,B)</div>
      <div
        class="mt-2 ml-1"
        :class="{
          abchannel_total_num: cueInfo.cuetype != 'A',
          abchannel_total_num_xs: cueInfo.cuetype == 'A',
        }"
      >
        전체 : {{ abCartArr.length }}개
      </div>
      <DxDataGrid
        id="channelAB"
        :focused-row-enabled="false"
        :ref="dataGridRef"
        :data-source="abCartArr"
        :height="abChannelHeight"
        :showColumnLines="false"
        :show-borders="true"
        :showRowLines="true"
        keyExpr="rownum"
        @selection-changed="onSelectionChanged"
        @toolbar-preparing="onToolbarPreparing($event)"
        :customize-columns="customizeColumns"
        :show-column-headers="false"
        noDataText="데이터가 없습니다."
      >
        <DxRowDragging
          dropFeedbackMode="indicate"
          :allow-reordering="true"
          :on-add="onAddChannelAB"
          :on-reorder="onReorderChannelAB"
          :show-drag-icons="false"
          group="tasksGroup"
          v-if="cueInfo.cuetype != 'A'"
        />
        <DxEditing
          :allow-adding="true"
          :allow-updating="true"
          mode="cell"
          start-edit-action="dblClick"
          v-if="cueInfo.cuetype != 'A'"
        />
        <DxSelection mode="multiple" showCheckBoxesMode="none" />
        <DxColumn
          cell-template="rowIndexTemplate"
          caption="순서"
          :width="45"
          alignment="center"
        />
        <template #rowIndexTemplate="{ data }">
          <div>
            <div>{{ data.rowIndex + 1 }}</div>
          </div>
        </template>
        <DxColumn :width="30" cell-template="product_Template" />
        <template #product_Template="{ data }">
          <div v-if="data.data.subTitle != ''">
            <b-icon
              icon="box-seam"
              v-if="data.data.cartcode == 'S01G01C011'"
            ></b-icon>
            <b-icon
              icon="trophy"
              v-if="data.data.cartcode == 'S01G01C013'"
            ></b-icon>
            <b-icon
              icon="shield-check"
              v-if="data.data.cartcode == 'S01G01C017'"
            ></b-icon>
            <b-icon
              icon="shield-lock"
              v-if="data.data.cartcode == 'S01G01C010'"
            ></b-icon>
            <b-icon
              icon="shield-shaded"
              v-if="data.data.cartcode == 'S01G01C018'"
            ></b-icon>
            <b-icon
              icon="shield-plus"
              v-if="data.data.cartcode == 'S01G01C019'"
            ></b-icon>
            <b-icon
              icon="camera2"
              v-if="data.data.cartcode == 'S01G01C012'"
            ></b-icon>
            <b-icon
              icon="hourglass"
              v-if="data.data.cartcode == 'S01G01C021'"
            ></b-icon>
            <b-icon
              icon="hourglass-bottom"
              v-if="data.data.cartcode == 'S01G01C022'"
            ></b-icon>
            <b-icon
              icon="hourglass-split"
              v-if="data.data.cartcode == 'S01G01C023'"
            ></b-icon>
            <b-icon
              icon="hourglass-top"
              v-if="data.data.cartcode == 'S01G01C024'"
            ></b-icon>
            <b-icon
              icon="camera-reels"
              v-if="data.data.cartcode == 'S01G01C009'"
            ></b-icon>
            <b-icon
              icon="alarm"
              v-if="data.data.cartcode == 'S01G01C016'"
            ></b-icon>
            <b-icon
              icon="clock"
              v-if="data.data.cartcode == 'S01G01C020'"
            ></b-icon>
          </div>
        </template>
        <DxColumn :width="60" cell-template="trans_Template" />
        <template #trans_Template="{ data }">
          <div v-if="data.data.subtitle != ''">
            <DxButton
              hint="운행정보 변경"
              @click="iconClick(data)"
              v-show="data.data.transtype == 'S'"
            >
              <b-icon
                class="dx-icon"
                icon="suit-diamond-fill"
                style="color: #ffc107"
              ></b-icon>
            </DxButton>
            <DxButton
              hint="운행정보 변경"
              @click="iconClick(data)"
              v-show="data.data.transtype == 'L'"
            >
              <b-icon
                class="dx-icon"
                icon="arrow-repeat"
                style="color: #f44336; font-weight: 300"
              ></b-icon>
            </DxButton>
            <DxButton
              hint="운행정보 변경"
              @click="iconClick(data)"
              v-if="data.data.transtype == 'C'"
            >
              <b-icon
                class="dx-icon"
                icon="arrow-down"
                style="color: #03a9f4"
              ></b-icon>
            </DxButton>
          </div>
        </template>
        <DxColumn
          caption="소재명"
          edit-cell-template="text_edit_Template"
          cell-template="text_Template"
        />
        <template #text_edit_Template="{ data: cellInfo }">
          <div>
            <div v-if="cellInfo.data.subtitle != ''">
              <DxTextBox
                :value="cellInfo.data.maintitle"
                :on-value-changed="
                  (value) => onValueChanged_mainText(value, cellInfo)
                "
              />
              <div class="ml-2" style="color: #959595; font-size: 12px">
                {{ cellInfo.data.subtitle }}
              </div>
            </div>
            <div v-else>
              <DxTextBox
                :value="cellInfo.data.memo"
                :on-value-changed="
                  (value) => onValueChanged_memoText(value, cellInfo)
                "
              />
            </div>
          </div>
        </template>

        <template #text_Template="{ data }">
          <div>
            <div v-if="data.data.subtitle != ''">
              <div
                :class="{
                  maintitle_red:
                    data.data.onairdate != '' &&
                    (data.data.onairdate != cueInfo.day ||
                      data.data.onairdate != cueInfo.brddate),
                }"
              >
                {{ data.data.maintitle }}
              </div>
              <div style="color: #959595; font-size: 12px">
                {{ data.data.subtitle }}
              </div>
            </div>

            <div v-else>
              <div>
                {{ data.data.memo }}
              </div>
            </div>
          </div>
        </template>
        <DxColumn
          caption="시간"
          alignment="center"
          format="hh:mm:ss"
          dataType="date"
          :width="130"
          cell-template="totalTime_Template"
        />
        <template #totalTime_Template="{ data }">
          <div v-if="data.data.subtitle != ''" style="color: #333333">
            {{
              $moment(data.data.endposition - data.data.startposition)
                | moment("subtract", "9 hours")
                | moment("HH:mm:ss")
            }}
          </div>
        </template>
        <DxColumn
          caption="시작점"
          alignment="center"
          :width="25"
          cell-template="start_Template"
        />
        <template #start_Template="{ data }">
          <div v-if="data.data.startposition > 0">
            <b-icon icon="star"></b-icon>
          </div>
        </template>
        <DxColumn
          caption="끝점"
          alignment="center"
          :width="25"
          cell-template="end_Template"
        />
        <template #end_Template="{ data }">
          <div v-if="data.data.duration > data.data.endposition">
            <b-icon icon="star"></b-icon>
          </div>
        </template>
        <DxColumn :width="60" cell-template="play_Template" />
        <template #play_Template="{ data }">
          <div>
            <div v-if="data.data.subtitle != '' && data.data.onairdate == ''">
              <DxButton
                icon="music"
                type="default"
                hint="미리듣기/음원편집"
                @click="onPreview(data.data)"
              />
            </div>
            <div v-if="data.data.subtitle != '' && data.data.onairdate != ''">
              <DxButton
                icon="music"
                type="success"
                hint="그룹 미리듣기"
                @click="
                  showGrpPlayerPopup({
                    grpType: 'cm',
                    brd_Dt: data.data.onairdate,
                    grpId: data.data.cartid,
                    title: data.data.maintitle,
                  })
                "
              />
            </div>
          </div>
        </template>
        <DxScrolling mode="infinite" />
        <template #totalGroupCount>
          <div>
            <DxButton
              icon="trash"
              @click="selectionDel"
              :disabled="!selectedItemKeys.length"
              hint="선택 행 삭제"
              v-if="cueInfo.cuetype != 'A'"
            />
          </div>
        </template>
      </DxDataGrid>
    </div>
    <CMGroupPlayerPopup
      :showPlayerPopup="showGrpPlayer"
      :title="grpParam.title"
      :grpType="grpParam.grpType"
      :brd_Dt="grpParam.brd_Dt"
      :grpId="grpParam.grpId"
      @closePlayer="closeGrpPlayerPopup"
    >
    </CMGroupPlayerPopup>

    <PlayerPopup
      :showPlayerPopup="showPlayerPopup"
      :title="soundItem.maintitle"
      :fileKey="soundItem.filetoken"
      :streamingUrl="streamingUrl"
      :waveformUrl="waveformUrl"
      :tempDownloadUrl="tempDownloadUrl"
      :rowNum="soundItem.rownum"
      :startPoint="soundItem.startposition"
      :endPoint="soundItem.endposition"
      type="A"
      requestType="token"
      @closePlayer="onClosePlayer"
    >
    </PlayerPopup>
  </div>
</template>

<script>
import { mapActions, mapGetters, mapMutations } from "vuex";
import MixinCommon from "../../../mixin/MixinCommon";
import {
  DxDataGrid,
  DxColumn,
  DxEditing,
  DxScrolling,
  DxSelection,
  DxRowDragging,
} from "devextreme-vue/data-grid";
import DxButton from "devextreme-vue/button";
import DxTextBox from "devextreme-vue/text-box";
import "moment/locale/ko";
import { eventBus } from "@/eventBus";

const moment = require("moment");
const dataGridRef = "dataGrid";

export default {
  mixins: [MixinCommon],
  props: {
    abChannelHeight: Number,
  },
  data() {
    return {
      dataGridRef,
      showGrpPlayer: false,
      grpParam: {},
      rowData: {
        onairdate: "",
        cartid: "", // 소재ID
        cartcode: "", //그룹코드
        startposition: 0,
        endposition: 0, //millisecond
        fadeintime: 0,
        fadeouttime: 0,
        transtype: "S",
        maintitle: "",
        subtitle: "",
        memo: "", //바뀔수도있음
        rownum: 1,
        filetoken: "", //미리듣기 때문 바뀔수도있음
        filepath: "",
        duration: 0, //string
        useflag: "Y",
      },
      selectedItemKeys: [],
    };
  },
  mounted() {
    if (this.abCartArr.length > 0) {
      this.rowData.rownum = this.abCartArr.length + 1;
    }
  },
  created() {
    // if(this.cueInfo.cueid!=-1){
    // }else{
    //   this.SET_ABCARTARR([]);
    // }
    eventBus.$on("abDataSet", (val) => {
      this.rowData.rownum = this.abCartArr.length + 1;
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
    DxTextBox,
  },
  computed: {
    ...mapGetters("cueList", ["searchListData"]),
    ...mapGetters("cueList", ["cueInfo"]),
    ...mapGetters("cueList", ["abCartArr"]),
    dataGrid: function () {
      return this.$refs[dataGridRef].instance;
    },
  },
  methods: {
    //...mapMutations("cueList", ["SET_ABCARTARR"]),
    ...mapActions("cueList", ["cartCodeFilter"]),

    showGrpPlayerPopup(data) {
      this.grpParam = data;
      this.showGrpPlayer = true;
    },
    closeGrpPlayerPopup() {
      this.showGrpPlayer = false;
    },

    onAddChannelAB(e) {
      console.log("e.itemData");
      console.log(e.itemData);
      var arrData = this.abCartArr;
      if (e.fromData === undefined) {
        var selectedRowsData = this.sortSelectedRowsData(e, "data");
        if (selectedRowsData.length > 1) {
          selectedRowsData.forEach((data, index) => {
            var row = { ...this.rowData };
            var search_row = data;
            if (Object.keys(search_row).includes("contents")) {
              row.memo = search_row.contents;
            } else {
              row.filetoken = search_row.fileToken;
              row.filepath = search_row.filePath;
              if (!search_row.intDuration) {
                row.endposition = 0;
                row.duration = 0;
              } else {
                row.endposition = search_row.intDuration;
                row.duration = search_row.intDuration;
              }
              row.cartid = search_row.id;
              row.cartcode = this.searchListData.cartcode;

              this.cartCodeFilter({
                row: row,
                search_row: search_row,
              });
            }
            arrData.splice(e.toIndex + index, 0, row);
            this.rowData.rownum = this.rowData.rownum + 1;
          });
        } else {
          var row = { ...this.rowData };
          var search_row = e.itemData;
          if (Object.keys(search_row).includes("contents")) {
            row.memo = search_row.contents;
          } else {
            row.filetoken = search_row.fileToken;
            row.filepath = search_row.filePath;
            if (!search_row.intDuration) {
              row.endposition = 0;
              row.duration = 0;
            } else {
              row.endposition = search_row.intDuration;
              row.duration = search_row.intDuration;
            }
            row.cartid = search_row.id;
            row.cartcode = this.searchListData.cartcode;
            this.cartCodeFilter({
              row: row,
              search_row: search_row,
            });
          }
          arrData.splice(e.toIndex, 0, row);
          this.rowData.rownum = this.rowData.rownum + 1;
        }
      } else if (e.fromData.subtitle !== undefined) {
        var search_row = e.fromData;
        var row = { ...search_row };
        row.rownum = this.rowData.rownum;
        row.transtype = "S";
        delete row.editTarget;
        arrData.splice(e.toIndex, 0, row);
        this.rowData.rownum = this.rowData.rownum + 1;
      }
      console.log(this.abCartArr);
      // e.fromComponent.clearSelection();
      //this.SET_ABCARTARR(arrData);
    },
    onReorderChannelAB(e) {
      var arrData = this.abCartArr;
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
      //e.component.clearSelection();
      //this.SET_ABCARTARR(arrData);
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
    selectionDel() {
      var arrData = this.abCartArr;
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
      //this.SET_ABCARTARR(arrData);
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
    onToolbarPreparing(e) {
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
              var arrData = this.abCartArr;
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
              //this.SET_ABCARTARR(arrData);
            },
          };
        }
      });
      toolbarItems.push({
        location: "after",
        template: "totalGroupCount",
      });
    },
    customizeColumns(columns) {
      columns[3].dataField = "maintitle";
    },
    onValueChanged_mainText(value, cellInfo) {
      cellInfo.data.maintitle = value.value;
    },
    onValueChanged_memoText(value, cellInfo) {
      cellInfo.data.memo = value.value;
      console.log("this.abCartArr");
      console.log(this.abCartArr);
    },
    iconClick(e) {
      if (this.cueInfo.cuetype == "A") {
        return;
      }
      switch (e.data.transtype) {
        case "S":
          e.data.transtype = "C";
          break;
        case "C":
          e.data.transtype = "L";
          break;
        case "L":
          e.data.transtype = "S";
          break;
        default:
          break;
      }
    },
    //시간 string > milliseconds
    millisecondsFuc(duration) {
      var itemTime = moment(duration, "HH:mm:ss.SS");
      var defTime = moment("00:00:00.00", "HH:mm:ss.SS");
      return moment.duration(itemTime.diff(defTime)).asMilliseconds();
    },
  },
};
</script>

<style>
.abchannel_view {
  /* position: absolute; */
  /* border: solid 0.5px #ddd; */
  /* top: 10px; */
  padding: 10px;
  /* margin-right: 10px; */
  border-radius: 2px;
  background-color: #2a4878;
}
.abchannel_view .dx-datagrid-borders > .dx-datagrid-header-panel,
.abchannel_view .dx-datagrid-header-panel .dx-toolbar {
  background-color: #2a4878;
}
.abchannel_title {
  padding: 0;
  position: absolute;
  color: white;
  z-index: 1;
}
.abchannel_total_num {
  right: 100px;
  padding: 0;
  position: absolute;
  color: white;
  z-index: 1;
}
.abchannel_total_num_xs {
  right: 20px;
  padding: 0;
  position: absolute;
  color: white;
  z-index: 1;
}
#channelAB td {
  vertical-align: middle;
}
.editTemplateSubTitle {
  color: #2a4878;
}
.maintitle_red {
  color: red;
}
</style>