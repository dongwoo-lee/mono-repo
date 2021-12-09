<template>
  <div class="search_view" v-bind:class="widthSizeClass">
    <div class="search_menu">
      <div class="menuList_view">
        <DxMenu
          id="menuId"
          class="item_size"
          :data-source="menuList_items"
          orientation="vertical"
          :selectedItem="menuList_items[2]"
          display-expr="name"
          :selectByClick="true"
          selectionMode="single"
          :onItemClick="itemclick"
          v-bind:class="item_size_option"
        />
      </div>
      <div class="selectList_view">
        <b-form @submit="onSubmit">
          <h4 class="m-3">{{ searchDataList.name }}</h4>
          <div v-bind:class="selectSizeClass_option">
            <div v-for="index in searchDataList.options" :key="index.num">
              <div v-if="index.type == 'S'">
                <b-form-group :label="index.text" class="has-float-label pb-2">
                  <b-form-select
                    v-model="index.selectVal"
                    :options="index.value"
                    value-field="value"
                    text-field="text"
                    @change="eventClick($event, index)"
                  />
                </b-form-group>
              </div>
              <div v-if="index.type == 'T'">
                <b-form-group :label="index.text" class="has-float-label pb-2">
                  <b-form-input :value="index.value" v-model="index.selectVal">
                  </b-form-input>
                </b-form-group>
              </div>
              <div v-if="index.type == 'C'">
                <fieldset class="form-group">
                  <div class="form-row">
                    <span class="bv-no-focus-ring col-form-label"
                      >{{ index.text }} :
                    </span>
                    <b-form-checkbox-group
                      class="custom-checkbox-group pt-1 ml-1"
                      :options="index.value"
                      value-field="value"
                      v-model="index.selectVal"
                      text-field="text"
                    />
                  </div>
                </fieldset>
              </div>
              <div v-if="index.type == 'D'">
                <b-form-group :label="index.text" class="has-float-label">
                  <common-date-picker v-model="index.selectVal" />
                </b-form-group>
              </div>
            </div>
          </div>
          <div>
            <b-button
              variant="outline-primary default"
              class="search_ok_btn"
              type="submit"
              >검색</b-button
            >
          </div>
        </b-form>
      </div>
    </div>
    <div v-bind:class="search_table_size">
      <div>
        <DxDataGrid
          id="search_data_grid"
          :data-source="searchtable_data.columns"
          :ref="dataGridRef"
          :height="gridHeight"
          :focused-row-enabled="false"
          :showColumnLines="true"
          :show-borders="true"
          :row-alternation-enabled="true"
          :columns="searchtable_columns"
          width="500"
          :showRowLines="true"
          @selection-changed="onSelectionChanged"
          keyExpr="rowNO"
          noDataText="데이터가 없습니다."
        >
          <template #play_Template="{ data }">
            <div>
              <DxButton
                icon="music"
                type="default"
                hint="미리듣기"
                styling-mode="outlined"
                @click="onPreview(data.data)"
              />
            </div>
          </template>
          <template #calculate_MB_Template="{ data }">
            <div>
              {{
                Math.round(
                  (data.data.fileSize / 1024 / 1024 + Number.EPSILON) * 100
                ) /
                  100 +
                "MB"
              }}
            </div>
          </template>
          <template #calculate_KB_Template="{ data }">
            <div>
              {{
                Math.round((data.data.fileSize / 1024 + Number.EPSILON) * 100) /
                  100 +
                "KB"
              }}
            </div>
          </template>
          <template #row_Template="{ data }">
            <div>
              <div>{{ data.rowIndex + 1 }}</div>
            </div>
          </template>
          <template #duration_Template="{ data }">
            <div>
              <div>{{ data.data.duration.substring(0, 8) }}</div>
            </div>
          </template>
          <DxRowDragging :show-drag-icons="false" group="tasksGroup" />
          <DxSelection mode="multiple" showCheckBoxesMode="none" />
          <DxPaging :enabled="false" />
          <DxScrolling column-rendering-mode="virtual" />
        </DxDataGrid>
      </div>
      <div v-if="subtableVal">
        <DxDataGrid
          id="search_data_grid"
          :data-source="subtable_data"
          :ref="dataGridRef"
          :height="gridHeight"
          :focusedRowEnabled="false"
          :showColumnLines="true"
          :show-borders="true"
          :row-alternation-enabled="true"
          :columns="subtable_columns"
          :showRowLines="true"
          column-auto-width="true"
          keyExpr="rowNO"
          noDataText="데이터가 없습니다."
        >
          <template #play_Template="{ data }">
            <div>
              <DxButton
                icon="music"
                type="default"
                hint="미리듣기"
                styling-mode="outlined"
                @click="onPreview(data.data)"
              />
            </div>
          </template>
          <DxRowDragging :show-drag-icons="false" group="tasksGroup" />
          <DxSelection mode="multiple" showCheckBoxesMode="none" />
          <DxScrolling column-rendering-mode="virtual" />
        </DxDataGrid>
      </div>
      <PlayerPopup
        :showPlayerPopup="showPlayerPopup"
        :title="soundItem.name"
        :fileKey="soundItem.fileToken"
        :streamingUrl="streamingUrl"
        :waveformUrl="waveformUrl"
        :tempDownloadUrl="tempDownloadUrl"
        requestType="token"
        @closePlayer="onClosePlayer"
      >
      </PlayerPopup>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex";
import MixinCommon from "../../../mixin/MixinCommon";
import searchMenuList from "../../../data/searchMenuList";
import DxMenu from "devextreme-vue/menu";
import {
  DxDataGrid,
  DxScrolling,
  DxSelection,
  DxRowDragging,
  DxPaging,
} from "devextreme-vue/data-grid";
import { USER_ID } from "@/constants/config";
import DxButton from "devextreme-vue/button";
import axios from "axios";

const dataGridRef = "dataGrid";

export default {
  mixins: [searchMenuList, MixinCommon],
  props: {
    width_size: Number,
  },
  data() {
    return {
      gridHeight: 0,
      searchDataList: {
        id: "",
        name: "",
        options: [],
        columns: [],
      },
      searchtable_data: {
        cartcode: "",
        columns: [],
      },
      searchtable_columns: [],
      searchItems: {
        rowPerPage: 30,
        selectPage: 1,
      },
      dataGridRef,
      subtableVal: false,
      subtable_data: [],
      subtable_columns: [],
      sbFields: [
        {
          dataField: "rowNO",
          caption: "순서",
          width: "7.5%",
          alignment: "center",
        },
        {
          dataField: "categoryID",
          caption: "구분",
          width: "8%",
          alignment: "center",
        },
        {
          dataField: "categoryName",
          caption: "광고주명/분류명",
          width: "20%",
          alignment: "center",
        },
        {
          dataField: "id",
          caption: "소재ID",
          width: "17%",
          alignment: "center",
        },
        {
          dataField: "name",
          caption: "소재명",
          width: "30%",
          alignment: "center",
        },
        {
          dataField: "length",
          caption: "길이",
          width: "10%",
          alignment: "center",
        },
        {
          cellTemplate: "play_Template",
          caption: "작업",
          width: "7%",
          alignment: "center",
        },
      ],
      cmFields: [
        {
          dataField: "rowNO",
          caption: "순서",
          width: "7.5%",
          alignment: "center",
        },
        {
          dataField: "advertiser",
          caption: "광고주",
          width: "25%",
          alignment: "center",
        },
        {
          dataField: "name",
          caption: "소재명",
          width: "33%",
          alignment: "center",
        },
        {
          dataField: "length",
          caption: "길이(초)",
          width: "10%",
          alignment: "center",
        },
        {
          dataField: "codingDT",
          caption: "제작일",
          width: "90px",
          width: "18%",
          alignment: "center",
        },
        {
          cellTemplate: "play_Template",
          caption: "작업",
          width: "7%",
          alignment: "center",
        },
      ],
    };
  },
  created() {},
  mounted() {
    this.searchDataList = this.searchData[1];
    this.searchtable_columns = this.searchDataList.columns;
    this.gridHeight = this.width_size;
  },
  components: {
    DxMenu,
    DxDataGrid,
    DxScrolling,
    DxSelection,
    DxRowDragging,
    DxButton,
    mapMutations,
    DxPaging,
  },
  computed: {
    ...mapGetters("cueList", ["searchListData"]),
    widthSizeClass: function () {
      return {
        width_size_big: this.width_size == 330,
        width_size_sm: this.width_size != 330,
      };
    },
    selectSizeClass_option: function () {
      return {
        select_option_big: this.width_size == 330,
        select_option_sm: this.width_size != 330,
      };
    },
    item_size_option: function () {
      return {
        item_size_big: this.width_size == 330,
        item_size_sm: this.width_size != 330,
      };
    },
    search_table_size: function () {
      return {
        search_table_big: this.width_size == 330,
        search_table_sm: this.width_size != 330,
      };
    },
    dataGrid: function () {
      return this.$refs[dataGridRef].instance;
    },
  },
  methods: {
    ...mapMutations("cueList", ["SET_SEARCHLISTDATA"]),
    //아이템 소재 가져오기
    eventClick(newObjectState, object) {
      const url = `/api/SearchMenu/GetPublicSecond`;
      if (object.name == "medias") {
        const selectOptions = this.searchDataList.options.filter(
          (Val) => Val.name == "publicSecond"
        );
        if (selectOptions.length > 0) {
          this.getOptionsData(url, { media: newObjectState });
        }
      }
    },
    //검색
    async onSubmit(e) {
      const userId = sessionStorage.getItem(USER_ID);
      e.preventDefault();
      const selectOptions = this.searchDataList.options.filter(
        (Val) => Val.selectVal
      );
      const result = { userid: userId };
      selectOptions.forEach((ele) => {
        if (typeof ele.selectVal != "object") {
          result[ele.id] = ele.selectVal;
        } else {
          result[ele.id] = ele.selectVal[0];
        }
      });
      await this.getData(result);
      switch (this.searchDataList.id) {
        case "MCR_SB":
          this.subtableVal = true;
          if (this.width_size != 330) {
            this.gridHeight = 237;
          }
          this.subtable_columns = this.sbFields;
          break;

        case "SCR_SB":
          this.subtableVal = true;
          if (this.width_size != 330) {
            this.gridHeight = 237;
          }
          this.subtable_columns = this.sbFields;
          break;
        case "PGM_CM":
          this.subtableVal = true;
          if (this.width_size != 330) {
            this.gridHeight = 237;
          }
          this.subtable_columns = this.cmFields;
          break;
        case "CM":
          this.subtableVal = true;
          if (this.width_size != 330) {
            this.gridHeight = 237;
          }
          this.subtable_columns = this.cmFields;
          break;
        default:
          this.subtableVal = false;
          this.gridHeight = this.width_size;
          break;
      }
      this.subtable_data = [];
    },
    onSelectionChanged(e) {
      if (e.selectedRowsData.length > 0) {
        var selectRowData = e.selectedRowsData[0];
        switch (this.searchDataList.id) {
          case "MCR_SB":
            this.getSubData("sb", selectRowData.brdDT, selectRowData.id);
            break;

          case "SCR_SB":
            this.getSubData("sb", selectRowData.brdDT, selectRowData.id);
            break;

          case "PGM_CM":
            this.getSubData("cm", selectRowData.brdDT, selectRowData.id);
            break;

          case "CM":
            this.getSubData("cm", selectRowData.brdDT, selectRowData.id);
            break;

          default:
            break;
        }
      }
    },
    itemclick(e) {
      const url = `/api/SearchMenu/GetSearchOption/`;
      const result = this.searchData.filter((data) => {
        return data.name == e.itemData.name;
      });
      if (result[0]) {
        this.searchDataList = result[0];
        this.getOptionsData(url, { type: this.searchDataList.id });
      }
    },
    getOptionsData(url, pram) {
      axios(url, {
        params: pram,
      }).then((res) => {
        const resData = res.data;
        for (const [key, value] of Object.entries(resData)) {
          this.searchDataList.options.forEach((ele) => {
            if (key == ele.name && value != null) {
              ele.value = value.data.map((item) => {
                return {
                  text: item.name,
                  value: item.id,
                };
              });
            }
          });
        }
      });
    },
    // 서브 데이터 조회
    getSubData(apiType, brdDT, id) {
      //if (sbID === undefined) return;
      //this.isSubTableLoading = true;
      axios(`/api/products/${apiType}/contents/${brdDT}/${id}`).then((res) => {
        this.subtable_data = res.data.resultObject.data;
        //this.isSubTableLoading = false;
      });
    },
    async getData(Val) {
      var basedata = this.searchItems;
      const result = Object.assign({}, basedata, Val);
      await axios(`/api/SearchMenu/GetSearchTable/${this.searchDataList.id}`, {
        params: result,
      }).then((res) => {
        this.searchtable_data.cartcode = this.searchDataList.cartcode;
        this.searchtable_data.columns = res.data.result.data;
        this.searchtable_columns = this.searchDataList.columns;
        this.SET_SEARCHLISTDATA(this.searchtable_data);
      });
    },
    // onRowPrepared(e) {
    //   e.rowElement.css({ height: 100 });
    // },
  },
};
</script>

<style>
#search_data_grid .dx-row {
  height: 30px;
  line-height: 25px;
}
.search_view {
  height: 330px;
  display: grid;
}
.width_size_big {
  grid-template-columns: 2fr 5fr;
  column-gap: 15px;
}
.width_size_sm {
  grid-template-columns: 1fr;
  grid-template-rows: 250px auto;
  row-gap: 15px;
}
.search_menu {
  padding: 5px;
  display: grid;
  grid-template-rows: 1fr;
  grid-template-columns: 110px auto;
  border: solid 1px #ddd;
}
/* .dx-menu-item-content {
  background-color: rgb(99, 71, 71);
} */
.select_option_big {
  padding: 15px;
  overflow: auto;
  height: 207px;
}
.select_option_sm {
  padding: 15px;
  overflow: auto;
  height: 130px;
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 0px 15px;
}
.item_size_big .dx-item-content {
  line-height: 3.8;
  height: auto;
  width: 100px;
}
.item_size_sm .dx-item-content {
  line-height: 2.1;
  height: auto;
  width: 100px;
}
.dropdown-menu {
  position: fixed !important;
  left: auto !important;
  top: auto !important;
}
.search_table_big {
  display: flex;
  gap: 15px;
}
.search_table_sm {
  display: flex;
  gap: 15px;
  flex-direction: column;
}
.search_ok_btn {
  width: 90%;
  height: 35px;
  margin: 15px 0px 0px 15px;
  padding: 2;
}
#search_data_grid .dx-button-content {
  width: 25px;
  height: 25px;
  padding: 0;
}
/* select CSS */
.dx-rtl
  .dx-datagrid-rowsview
  .dx-selection.dx-row
  > td:not(.dx-focused).dx-datagrid-group-space,
.dx-rtl
  .dx-datagrid-rowsview
  .dx-selection.dx-row:hover
  > td:not(.dx-focused).dx-datagrid-group-space {
  border-left-color: #cce8f5 !important;
}

.dx-datagrid-rowsview .dx-selection.dx-row:not(.dx-row-lines) > td,
.dx-datagrid-rowsview .dx-selection.dx-row:hover:not(.dx-row-lines) > td {
  border-bottom: 1px solid #cce8f5 !important;
  border-top: 1px solid #cce8f5 !important;
}

.dx-datagrid-rowsview .dx-selection.dx-row > td.dx-datagrid-group-space,
.dx-datagrid-rowsview .dx-selection.dx-row:hover > td.dx-datagrid-group-space {
  border-right-color: #cce8f5 !important;
}

.dx-datagrid-rowsview .dx-selection.dx-row > td,
.dx-datagrid-rowsview .dx-selection.dx-row:hover > td {
  background-color: #cce8f5 !important;
  color: #232323 !important;
}

.dx-datagrid-table
  .dx-data-row.dx-state-hover:not(.dx-selection):not(.dx-row-inserted):not(.dx-row-removed):not(.dx-edit-row)
  > td:not(.dx-focused) {
  background-color: #bf4e6a !important;
  color: #333 !important;
}
</style>