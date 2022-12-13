<template>
  <div class="search_view" v-bind:class="widthSizeClass">
    <div class="search_menu">
      <div class="menuList_view">
        <DxMenu
          id="menuId"
          class="item_size"
          :data-source="menuList_items"
          orientation="vertical"
          :selectedItem="menuList_items[0]"
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
              <div :key="refreshKey">
                <div v-if="index.type == 'S'">
                  <b-form-group
                    :label="index.text"
                    class="has-float-label pb-2"
                  >
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
                  <b-form-group
                    :label="index.text"
                    class="has-float-label pb-2"
                  >
                    <b-form-input
                      :value="index.value"
                      v-model="index.selectVal"
                    >
                    </b-form-input>
                  </b-form-group>
                </div>
                <div v-if="index.type == 'C'">
                  <fieldset class="form-group">
                    <div class="form-row">
                      <span class="bv-no-focus-ring col-form-label"
                        >{{ index.text }} :
                      </span>
                      <b-form-checkbox
                        class="custom-checkbox-group pt-1 ml-1 mr-2"
                        v-model="allSelected"
                        :indeterminate="indeterminate"
                        v-if="index.id == 'searchtype1'"
                        aria-describedby="selectedSearchType1"
                        aria-controls="selectedSearchType1"
                        @change="toggleAll($event, index)"
                      >
                        All
                      </b-form-checkbox>
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
                    <common-date-picker
                      v-model="index.selectVal"
                      :required="index.requiredVal"
                    />
                    <!-- <common-date-picker value="2019-01-04" required /> -->
                  </b-form-group>
                </div>
                <div v-if="index.type == 'SED'">
                  <!-- 시작일 ~ 종료일 -->
                  <common-start-end-date-picker
                    :startDateLabel="index.startText"
                    :endDateLabel="index.endText"
                    :startDate.sync="index.st_selectVal"
                    :endDate.sync="index.end_selectVal"
                    :maxPeriodMonth="index.maxMonth"
                    :required="index.requiredVal"
                    :isCurrentDate="false"
                    class="datepicket_startEnd"
                    style="margin-left: 0px"
                  />
                </div>
              </div>
            </div>
          </div>
          <div>
            <b-button
              variant="outline-primary default"
              type="submit"
              class="search_ok_btn"
              >검색
            </b-button>
          </div>
        </b-form>
      </div>
    </div>
    <div
      class="search_table"
      v-bind:class="search_table_size"
      style="overflow: overlay"
    >
      <div id="main_product_grid" style="overflow: overlay">
        <DxDataGrid
          id="search_data_grid"
          :data-source="searchtable_data.columns"
          :remote-operations="true"
          :cellHintEnabled="true"
          :height="gridHeight"
          :focused-row-enabled="false"
          :showColumnLines="true"
          :show-borders="true"
          :row-alternation-enabled="true"
          :columns="searchtable_columns"
          :showRowLines="true"
          :columnAutoWidth="true"
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
                v-if="data.data.filePath != null"
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

          <template #maxWidth_ellipsis_title="{ data }">
            <div
              :title="data.data.title"
              style="text-overflow: ellipsis; overflow: hidden"
            >
              {{ data.data.title }}
            </div>
          </template>

          <template #maxWidth_ellipsis_name="{ data }">
            <div
              :title="data.data.name"
              style="text-overflow: ellipsis; overflow: hidden"
            >
              {{ data.data.name }}
            </div>
          </template>

          <template #maxWidth_ellipsis_memo="{ data }">
            <div
              :title="data.data.memo"
              style="text-overflow: ellipsis; overflow: hidden"
            >
              {{ data.data.memo }}
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
              <div v-if="data.data.duration">
                {{ data.data.duration.substring(0, 8) }}
              </div>
            </div>
          </template>
          <DxRowDragging
            :show-drag-icons="false"
            group="tasksGroup"
            :on-drag-start="onDragStart"
            :on-drag-end="onDragEnd"
          />
          <DxLoadPanel
            :show-indicator="true"
            :show-pane="true"
            :shading="true"
            :enabled="true"
            :close-on-outside-click="false"
          />
          <DxSelection mode="multiple" showCheckBoxesMode="none" />
          <DxScrolling mode="infinite" />
          <DxPaging :page-size="pageSize" />
        </DxDataGrid>
      </div>
      <div
        class="sub_product_grid"
        v-if="subtableVal"
        style="overflow: overlay"
      >
        <DxDataGrid
          id="search_data_grid"
          :data-source="subtable_data"
          :ref="dataGridRef"
          :height="gridHeight"
          :focused-row-enabled="false"
          :showColumnLines="true"
          :show-borders="true"
          :row-alternation-enabled="true"
          :columns="subtable_columns"
          :showRowLines="true"
          :columnAutoWidth="true"
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
                v-if="data.data.filePath != null"
                @click="onPreview(data.data)"
              />
            </div>
          </template>
          <DxRowDragging
            :show-drag-icons="false"
            group="tasksGroup"
            :on-drag-start="onDragStart"
          />
          <DxLoadPanel
            :show-indicator="true"
            :show-pane="true"
            :shading="true"
            :enabled="true"
            :close-on-outside-click="false"
          />
          <DxSelection mode="multiple" showCheckBoxesMode="none" />
          <DxScrolling mode="infinite" />
        </DxDataGrid>
      </div>
      <PlayerPopup
        v-if="
          searchListData.cartcode != 'S01G01C014' &&
          searchListData.cartcode != 'S01G01C032'
        "
        :showPlayerPopup="showPlayerPopup"
        :title="goTitle"
        :fileKey="soundItem.fileToken"
        :streamingUrl="
          searchListData.cartcode != 'S01G01C015'
            ? streamingUrl
            : streamingUrl_music
        "
        :waveformUrl="
          searchListData.cartcode != 'S01G01C015'
            ? waveformUrl
            : waveformUrl_music
        "
        :tempDownloadUrl="
          searchListData.cartcode != 'S01G01C015'
            ? tempDownloadUrl
            : tempDownloadUrl_music
        "
        requestType="token"
        @closePlayer="onClosePlayer"
      >
      </PlayerPopup>

      <SongPlayerPopup
        v-else-if="searchListData.cartcode == 'S01G01C032'"
        :showPlayerPopup="showPlayerPopup"
        :music="soundItem"
        :streamingUrl="streamingUrl"
        :waveformUrl="waveformUrl"
        :tempDownloadUrl="tempDownloadUrl"
        requestType="token"
        @closePlayer="onClosePlayer"
      >
      </SongPlayerPopup>

      <MusicPlayerPopup
        v-else
        :showPlayerPopup="showPlayerPopup"
        :music="soundItem"
        :streamingUrl="streamingUrl_music"
        :waveformUrl="waveformUrl_music"
        :tempDownloadUrl="tempDownloadUrl_music"
        requestType="token"
        @closePlayer="onClosePlayer"
      >
      </MusicPlayerPopup>
    </div>
    <div class="contentsLength" v-if="width_size == 330">
      전체 : {{ searchItems.totalRowCount }}개 [{{ viewTableName }}]
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
  DxLoadPanel,
} from "devextreme-vue/data-grid";
import CustomStore from "devextreme/data/custom_store";
import { USER_ID } from "@/constants/config";
import DxButton from "devextreme-vue/button";
import DataGrid from "devextreme/ui/data_grid";

DataGrid.defaultOptions({
  options: {
    scrolling: {
      legacyMode: true,
    },
  },
});

const dataGridRef = "dataGrid";
//var main_table_width_size = document.getElementById("main_table").clientWidth;
export default {
  mixins: [searchMenuList, MixinCommon],
  props: {
    width_size: Number,
  },
  data() {
    return {
      streamingUrl_music: "/api/musicsystem/streaming",
      waveformUrl_music: "/api/musicsystem/waveform",
      tempDownloadUrl_music: "/api/musicsystem/temp-download",
      refreshKey: 0,
      pageSize: 30,
      gridHeight: 0,
      allSelected: false,
      indeterminate: false,
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
        totalRowCount: 0,
      },
      dataGridRef,
      viewTableName: "음반 기록실",
      subtableVal: false,
      subtable_data: [],
      subtable_columns: [],
      sbFields: [
        {
          dataField: "rowNO",
          caption: "순서",
          alignment: "center",
        },
        {
          dataField: "categoryID",
          caption: "구분",
          alignment: "center",
        },
        {
          dataField: "categoryName",
          caption: "광고주명/분류명",
          alignment: "center",
        },
        {
          dataField: "id",
          caption: "소재ID",
          alignment: "center",
        },
        {
          dataField: "name",
          caption: "소재명",
          alignment: "center",
        },
        {
          dataField: "length",
          caption: "길이",
          alignment: "center",
        },
        {
          cellTemplate: "play_Template",
          caption: "작업",
          alignment: "center",
        },
      ],
      cmFields: [
        {
          dataField: "rowNO",
          caption: "순서",
          alignment: "center",
        },
        {
          dataField: "advertiser",
          caption: "광고주",
          alignment: "center",
        },
        {
          dataField: "name",
          caption: "소재명",
          alignment: "center",
        },
        {
          dataField: "length",
          caption: "길이(초)",
          alignment: "center",
        },
        {
          dataField: "codingDT",
          caption: "제작일",
          alignment: "center",
        },
        {
          cellTemplate: "play_Template",
          caption: "작업",
          alignment: "center",
        },
      ],
    };
  },
  mounted() {
    this.searchDataList = this.searchData[0];
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
    DxLoadPanel,
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
    goTitle() {
      switch (this.searchDataList.cartcode) {
        case "S01G01C007":
          return this.soundItem.title;
        case "S01G01C006":
          return this.soundItem.recName;

        default:
          return this.soundItem.name;
      }
    },
  },
  methods: {
    ...mapMutations("cueList", ["SET_SEARCHLISTDATA"]),
    //아이템 소재 가져오기
    eventClick(newObjectState, object) {
      const url_public = `/api/SearchMenu/GetPublicSecond`;
      const url_pgm = `/api/SearchMenu/GetPgmcodes`;
      if (object.name == "medias") {
        const selectOptions_public = this.searchDataList.options.filter(
          (Val) => Val.name == "publicSecond"
        );
        var brd_dt = "";
        const selectOptions_pgmcode = this.searchDataList.options.filter(
          (Val) => {
            if (Val.name == "brd_dt") {
              brd_dt = Val.value;
            }
            return Val.name == "pgmCodes";
          }
        );
        if (selectOptions_public.length > 0) {
          this.getOptionsData(url_public, { media: newObjectState });
        }
        if (selectOptions_pgmcode.length > 0) {
          this.getOptionsData(url_pgm, {
            media: newObjectState,
            brd_dt: brd_dt,
          });
        }
      }
    },
    onDragStart() {
      document.getElementById("app-container").classList.add("drag_");
    },
    //검색
    async onSubmit(e) {
      this.loadpanelVal = true;
      this.searchtable_columns = [];
      const userId = sessionStorage.getItem(USER_ID);
      e.preventDefault();
      const selectOptions = this.searchDataList.options.filter(
        (Val) => Val.selectVal
      );
      const selectOptionsStartEndDate = this.searchDataList.options.filter(
        (Val) => Val.st_selectVal || Val.end_selectVal
      );
      const result = { userid: userId };

      selectOptions.forEach((ele) => {
        if (typeof ele.selectVal != "object") {
          result[ele.id] = ele.selectVal;
        } else {
          if (typeof ele.selectVal[0] == "string") {
            result[ele.id] = ele.selectVal[0];
          } else {
            if (ele.selectVal.length > 0) {
              var sum = ele.selectVal.reduce((a, b) => a + b);
              result[ele.id] = sum;
            }
          }
        }
      });
      if (selectOptionsStartEndDate.length > 0) {
        result["startDate"] = selectOptionsStartEndDate[0].st_selectVal;
        result["endDate"] = selectOptionsStartEndDate[0].end_selectVal;
      }
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
      var setDataList = await this.getData(result);
      this.subtable_data = [];
    },
    async onSelectionChanged(e) {
      e.component.beginCustomLoading("Loading...");
      if (e.selectedRowsData.length > 0) {
        var selectRowData = e.selectedRowsData[0];
        switch (this.searchDataList.id) {
          case "MCR_SB":
            await this.getSubData("sb", selectRowData.brdDT, selectRowData.id);
            break;
          case "SCR_SB":
            await this.getSubData("sb", selectRowData.brdDT, selectRowData.id);
            break;
          case "PGM_CM":
            await this.getSubData("cm", selectRowData.brdDT, selectRowData.id);
            break;
          case "CM":
            await this.getSubData("cm", selectRowData.brdDT, selectRowData.id);
            break;
          default:
            break;
        }
      }
      e.component.endUpdate();
      e.component.endCustomLoading();
    },
    itemclick(e) {
      const url = `/api/SearchMenu/GetSearchOption/`;
      const result = this.searchData.filter((data) => {
        return data.name == e.itemData.name;
      });
      if (result[0]) {
        this.searchDataList = result[0];
        this.refreshKey++;
        this.getOptionsData(url, { type: this.searchDataList.id });
      }
    },
    getOptionsData(url, pram) {
      this.$http(url, {
        params: pram,
      }).then((res) => {
        const resData = res.data.resultObject;
        if (resData) {
          for (const [key, value] of Object.entries(resData)) {
            this.searchDataList.options.forEach((ele) => {
              if (key == ele.name && value != null) {
                ele.value = value.data.map((item) => {
                  return {
                    text: item.name,
                    value: item.id,
                  };
                });
                if (
                  ele.name == "medias" ||
                  ele.name == "cm" ||
                  ele.name == "report" ||
                  ele.name == "dlDevice"
                ) {
                  ele.selectVal = ele.value[0].value;
                }
              }
            });
          }
        }
      });
    },
    // 서브 데이터 조회
    getSubData(apiType, brdDT, id) {
      this.$http(`/api/products/${apiType}/contents/${brdDT}/${id}`).then(
        (res) => {
          res.data.resultObject.data.forEach((ele, index) => {
            ele.rowNO = index + 1;
          });
          this.subtable_data = res.data.resultObject.data;
        }
      );
    },
    async getData(Val) {
      var basedata = this.searchItems;
      this.searchtable_data.cartcode = this.searchDataList.cartcode;
      if (
        this.searchDataList.cartcode == "S01G01C016" ||
        this.searchDataList.cartcode == "S01G01C017" ||
        this.searchDataList.cartcode == "S01G01C018" ||
        this.searchDataList.cartcode == "S01G01C019"
      ) {
        this.pageSize = 200;
      } else {
        this.pageSize = 30;
      }
      const result = Object.assign({}, basedata, Val);
      this.searchtable_data.columns = await new CustomStore({
        key: "rowNO",
        load: (loadOptions) => {
          loadOptions.requireGroupCount = false;
          if (loadOptions.skip >= 0 && loadOptions.skip % this.pageSize == 0) {
            result.selectPage = loadOptions.skip / this.pageSize + 1;
            return this.$http(
              `/api/SearchMenu/GetSearchTable/${this.searchDataList.id}`,
              {
                params: result,
              }
            ).then((res) => {
              var resultData = res.data.resultObject.result;
              if (
                this.searchDataList.id == "MCR_SB" ||
                this.searchDataList.id == "SCR_SB" ||
                this.searchDataList.id == "PGM_CM" ||
                this.searchDataList.id == "CM"
              ) {
                resultData.data.forEach((ele, index) => {
                  ele.rowNO = index + 1;
                });
              }
              this.viewTableName = this.searchDataList.name;
              this.searchItems.totalRowCount = resultData.totalRowCount;
              return resultData.data;
            });
          }
        },
        pageSize: this.pageSize,
      });
      this.searchtable_columns = this.searchDataList.columns;
      this.SET_SEARCHLISTDATA(this.searchtable_data);
    },
    toggleAll(event, value) {
      value.selectVal = event ? [1, 2, 4] : [];
    },
    onDragEnd() {
      document.getElementById("app-container").classList.remove("drag_");
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
.contentsLength {
  position: absolute;
  bottom: 3px;
  right: 30px;
}
#search_data_grid .dx-row {
  height: 30px;
  line-height: 25px;
}
.search_view {
  /* height: 330px; */
  width: 100%;
  display: grid;
}
.width_size_big {
  grid-template-columns: 2.1fr 5fr;
  /* grid-template-columns: 500px 1135px; */
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
  line-height: 2.52;
  height: auto;
  width: 100px;
}
.item_size_sm .dx-item-content {
  line-height: 1.7;
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
  float: right;
  width: 150px;
  height: 35px;
  margin: 15px 30px 0px 15px;
  padding: 2;
}
#search_data_grid .dx-button-content {
  width: 25px;
  height: 25px;
  padding: 0;
}
.datepicket_startEnd .form-group {
  margin-left: 15px;
  width: 100%;
}
.datepicket_startEnd .periodDateText {
  margin: 0px 0px 30px 0px !important;
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

.search_view
  .dx-datagrid-table
  .dx-data-row.dx-state-hover:not(.dx-selection):not(.dx-row-inserted):not(.dx-row-removed):not(.dx-edit-row)
  > td:not(.dx-focused) {
  background-color: #bf4e6a !important;
  color: #333 !important;
}
/* scroll > mode : infinite > empty row 문제 해결 CSS */
.search_view .dx-freespace-row {
  display: none !important;
}
</style>