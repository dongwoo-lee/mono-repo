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
        {{ abCartArr.length }} / 500
      </div>
      <DxDataGrid
        id="channelAB"
        :focused-row-enabled="false"
        :ref="dataGridRef"
        :data-source="abCartArr"
        :height="abChannelHeight"
        :showColumnLines="false"
        :show-borders="true"
        :showRowLines="false"
        keyExpr="rownum"
        @selection-changed="onSelectionChanged"
        @toolbar-preparing="onToolbarPreparing($event)"
        @key-down="onKeyDownDel"
        :customize-columns="customizeColumns"
        :show-column-headers="false"
        noDataText="데이터가 없습니다."
      >
        <DxLoadPanel
          :show-indicator="true"
          :show-pane="true"
          :shading="true"
          :enabled="true"
          :close-on-outside-click="false"
        />
        <DxRowDragging
          dropFeedbackMode="indicate"
          :allow-reordering="true"
          :on-add="onAddChannelAB"
          :on-drag-end="onDragEndchannelAB"
          :on-reorder="onReorderChannelAB"
          :on-drag-start="onDragStart"
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
          :width="45"
          alignment="center"
        />
        <template #rowIndexTemplate="{ data }">
          <div>
            <div>{{ data.data.rownum }}</div>
          </div>
        </template>
        <DxColumn :width="60" cell-template="trans_Template" />
        <template #trans_Template="{ data }">
          <div v-if="data.data.cartcode != ''">
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
        <DxColumn :width="30" cell-template="product_Template" />
        <template #product_Template="{ data }">
          <div class="iconDiv" v-if="data.data.cartcode != ''">
            <i
              class="iconsminds-shop"
              v-if="data.data.cartcode == 'S01G01C007'"
            >
            </i>
            <i
              class="iconsminds-big-data"
              v-if="data.data.cartcode == 'S01G01C006'"
            ></i>
            <i
              class="iconsminds-cd-2"
              v-if="
                data.data.cartcode == 'S01G01C014' ||
                data.data.cartcode == 'S01G01C015'
              "
            ></i>
            <i
              class="iconsminds-coins"
              v-if="
                data.data.cartcode == 'S01G01C017' ||
                data.data.cartcode == 'S01G01C016' ||
                data.data.cartcode == 'S01G01C018' ||
                data.data.cartcode == 'S01G01C019'
              "
            ></i>
            <i
              class="iconsminds-film"
              v-if="
                data.data.cartcode == 'S01G01C009' ||
                data.data.cartcode == 'S01G01C010' ||
                data.data.cartcode == 'S01G01C012' ||
                data.data.cartcode == 'S01G01C013'
              "
            ></i>
            <i
              class="iconsminds-engineering"
              v-if="
                data.data.cartcode == 'S01G01C020' ||
                data.data.cartcode == 'S01G01C021' ||
                data.data.cartcode == 'S01G01C022' ||
                data.data.cartcode == 'S01G01C023' ||
                data.data.cartcode == 'S01G01C024'
              "
            ></i>
          </div>
        </template>
        <DxColumn
          edit-cell-template="text_edit_Template"
          cell-template="text_Template"
        />
        <template #text_edit_Template="{ data: cellInfo }">
          <div>
            <div v-if="cellInfo.data.cartcode != ''">
              <DxTextBox
                :value="cellInfo.data.maintitle"
                :maxLength="40"
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
                :maxLength="40"
                :on-value-changed="
                  (value) => onValueChanged_memoText(value, cellInfo)
                "
              />
            </div>
          </div>
        </template>
        <template #text_Template="{ data }">
          <div>
            <div v-if="data.data.cartcode != ''">
              <div
                style="text-overflow: ellipsis; overflow: hidden"
                :class="{
                  maintitle_red:
                    data.data.onairdate != '' &&
                    cueInfo.cuetype != 'A' &&
                    (data.data.onairdate != cueInfo.brddate ||
                      cueInfo.pgmcode != data.data.pgmcode),
                }"
              >
                {{ data.data.maintitle }}
              </div>
              <div
                style="color: #959595; font-size: 12px text-overflow: ellipsis; overflow: hidden"
              >
                {{ data.data.subtitle }}
              </div>
            </div>

            <div v-else>
              <div style="text-overflow: ellipsis; overflow: hidden">
                {{ data.data.memo }}
              </div>
            </div>
          </div>
        </template>
        <DxColumn
          alignment="center"
          format="hh:mm:ss"
          dataType="date"
          :width="130"
          cell-template="totalTime_Template"
        />
        <template #totalTime_Template="{ data }">
          <div v-if="data.data.cartcode != ''" style="color: #333333">
            {{
              $moment(data.data.endposition - data.data.startposition)
                | moment("subtract", "9 hours")
                | moment("HH:mm:ss")
            }}
          </div>
        </template>
        <DxColumn
          alignment="center"
          :width="25"
          cell-template="start_Template"
        />
        <template #start_Template="{ data }">
          <div>
            <div v-if="!data.data.fadeintime && data.data.startposition > 0">
              <b-icon icon="screwdriver"></b-icon>
            </div>
            <div v-if="data.data.fadeintime && !data.data.startposition > 0">
              <b-icon style="transform: rotate(90deg)" icon="wrench"></b-icon>
            </div>
            <div v-if="data.data.fadeintime && data.data.startposition > 0">
              <b-icon icon="tools"></b-icon>
            </div>
          </div>
        </template>
        <DxColumn alignment="center" :width="25" cell-template="end_Template" />
        <template #end_Template="{ data }">
          <div>
            <div
              v-if="
                !data.data.fadeouttime &&
                data.data.duration > data.data.endposition
              "
            >
              <b-icon icon="screwdriver"></b-icon>
            </div>
            <div
              v-if="
                data.data.fadeouttime &&
                (!data.data.duration > data.data.endposition ||
                  data.data.duration == data.data.endposition)
              "
            >
              <b-icon style="transform: rotate(90deg)" icon="wrench"></b-icon>
            </div>
            <div
              v-if="
                data.data.fadeouttime &&
                data.data.duration > data.data.endposition
              "
            >
              <b-icon icon="tools"></b-icon>
            </div>
          </div>
        </template>
        <DxColumn :width="60" cell-template="play_Template" />
        <template #play_Template="{ data }">
          <div>
            <div v-if="data.data.cartcode != '' && data.data.onairdate == ''">
              <DxButton
                icon="music"
                type="default"
                hint="미리듣기/음원편집"
                v-if="data.data.filepath != null && data.data.filepath != ''"
                @click="onPreview(data.data)"
              />
            </div>
            <div v-if="data.data.cartcode != '' && data.data.onairdate != ''">
              <DxButton
                icon="music"
                type="success"
                hint="그룹 미리듣기"
                @click="
                  showGrpPlayerPopup({
                    grpType:
                      data.data.cartcode == 'S01G01C017' ||
                      data.data.cartcode == 'S01G01C016'
                        ? 'sb'
                        : 'cm',
                    brd_Dt: data.data.onairdate,
                    grpId: data.data.cartid,
                    title: data.data.maintitle,
                  })
                "
              />
            </div>
          </div>
        </template>
        <DxScrolling showScrollbar="always" />
        <DxPaging :enabled="false" />
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
      :parentName="playTem_name"
      @closePlayer="closeGrpPlayerPopup"
    >
    </CMGroupPlayerPopup>

    <EditPlayerPopup
      :showPlayerPopup="showPlayerPopup"
      :title="soundItem.maintitle"
      :fileKey="soundItem.filetoken"
      :streamingUrl="streamingUrl"
      :waveformUrl="waveformUrl"
      :tempDownloadUrl="tempDownloadUrl"
      :rowNum="soundItem.rownum"
      :startPoint="soundItem.startposition"
      :endPoint="soundItem.endposition"
      :fadeIn="soundItem.fadeintime"
      :fadeOut="soundItem.fadeouttime"
      type="A"
      requestType="token"
      :parentName="playTem_name"
      @closePlayer="onClosePlayer"
    >
    </EditPlayerPopup>
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
  DxPaging,
  DxSelection,
  DxRowDragging,
  DxLoadPanel,
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
      lengthCheck: false,
      grpParam: {},
      maxLength: 500,
      playTem_name: "DAP (A,B)",
      rowData: {
        carttype: "",
        onairdate: "",
        cartid: "", // 소재ID
        cartcode: "", //그룹코드
        startposition: 0,
        endposition: 0, //millisecond
        fadeintime: false,
        fadeouttime: false,
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
    eventBus.$on("abDataSet", (val) => {
      this.rowData.rownum = this.abCartArr.length + 1;
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
    DxTextBox,
    DxLoadPanel,
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
    ...mapMutations("cueList", ["SET_ABCARTARR"]),
    ...mapActions("cueList", ["setContents"]),
    ...mapActions("cueList", ["maxLengthChecker"]),
    async onAddChannelAB(e) {
      var rowArray = [];
      const arrData = _.cloneDeep(this.abCartArr);
      var checkIndex = arrData.length;
      //최대 개수 확인
      var lengthCheck = await this.maxLengthChecker({
        arrLength: checkIndex,
        maxLength: this.maxLength,
      });
      if (lengthCheck) {
        this.dataGrid.beginCustomLoading("Loading...");
        //선택한 row 하나의 배열로 합치기 (start)
        if (e.fromData === undefined) {
          // 소재검색 + print
          var selectedRowsData = this.sortSelectedRowsData(e, "data");
          // 단일 선택
          if (e.itemElement.ariaSelected == "false") {
            selectedRowsData = [e.itemData];
          }
          for (const data of selectedRowsData) {
            lengthCheck = await this.maxLengthChecker({
              arrLength: checkIndex,
              maxLength: this.maxLength,
            });
            if (!lengthCheck) {
              break;
            } else {
              if (Object.keys(data).includes("contents")) {
                //print
                data.contentType = "P";
                rowArray.push(data);
              } else {
                //소재검색
                data.contentType = "S";
                rowArray.push(data);
              }
              checkIndex++;
            }
          }
        } else if (e.fromData.cartcode != undefined) {
          //C
          e.fromData.contentType = "C";
          rowArray.push(e.fromData);
        }
        //선택한 row 하나의 배열로 합치기 (end)

        //합친 배열 store에 추가
        var index = 0;
        for await (const ele of rowArray) {
          var rowData = await this.setContents({
            type: "ab",
            search_row: ele,
            formRowData: this.rowData,
            cartcode: this.searchListData.cartcode,
          });
          arrData.splice(e.toIndex + index, 0, rowData);
          index++;
        }
        arrData.forEach((row, index) => {
          row.rownum = index + 1;
        });
        this.SET_ABCARTARR(arrData);
        this.dataGrid.endUpdate();
        this.dataGrid.endCustomLoading();
      }
    },
    setRowNum() {
      this.abCartArr.forEach((ele, index) => {
        ele.rownum = index + 1;
      });
    },
    maxLengthCheck() {
      var result = true;
      if (this.abCartArr.length > 499) {
        result = false;
        this.lengthCheck = true;
      }
      return result;
    },
    onDragStart() {
      document.getElementById("app-container").classList.add("drag_");
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
        selectedRowsData.forEach((ele, index) => {
          var row = { ...ele };
          row.rownum = arrData.length + index + 1;
          if (startindex > e.toIndex) {
            arrData.splice(newindex + index, 0, row);
          } else {
            arrData.splice(newindex + index + 1, 0, row);
          }
        });
        this.selectionDel();
        e.component.clearSelection();
      } else {
        arrData.splice(e.fromIndex, 1);
        arrData.splice(e.toIndex, 0, e.itemData);

        this.setRowNum();
      }
      //e.component.clearSelection();
    },
    onDragEndchannelAB(e) {
      document.getElementById("app-container").classList.remove("drag_");
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
      var totalList = [...this.abCartArr];
      this.selectedItemKeys.forEach((item) => {
        var result = totalList.filter((ele) => {
          return ele.rownum != item.rownum;
        });
        totalList = result;
      });
      this.SET_ABCARTARR(totalList);
      this.setRowNum();
      this.dataGrid.clearSelection();
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
            onClick: async () => {
              this.lengthCheck = false;
              var arrData = this.abCartArr;
              var row = { ...this.rowData };
              var SelectedRowKeys = this.dataGrid.getSelectedRowKeys();
              var rastkey = SelectedRowKeys[SelectedRowKeys.length - 1];
              var index = this.dataGrid.getRowIndexByKey(rastkey);
              row.rownum = this.rowData.rownum;
              if (rastkey != -1) {
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
              await this.setRowNum();
              if (this.lengthCheck) {
                window.$notify("error", `최대 개수를 초과하였습니다.`, "", {
                  duration: 10000,
                  permanent: false,
                });
              } else {
                //빈칸 추가 후 memo Cell 편집
                await this.dataGrid.refresh();
                if (index != -1) {
                  this.dataGrid.editCell(rastkey, 3);
                } else {
                  this.dataGrid.editCell(0, 3);
                }
              }
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
    showGrpPlayerPopup(data) {
      this.grpParam = data;
      this.showGrpPlayer = true;
    },
    closeGrpPlayerPopup() {
      this.showGrpPlayer = false;
    },
    onKeyDownDel(e) {
      if (e.event.key == "Delete") {
        this.selectionDel();
      }
    },
  },
};
</script>

<style>
.drag_ {
  position: fixed;
  height: 100%;
  overflow: auto;
}
.abchannel_view {
  padding: 10px;
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
#channelAB td:nth-child(3) {
  padding: 0;
}
.editTemplateSubTitle {
  color: #2a4878;
}
.maintitle_red {
  color: red;
}
.iconDiv i {
  font-size: 17px;
}
</style>