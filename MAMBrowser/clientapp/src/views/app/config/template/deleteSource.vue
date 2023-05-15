<template>
  <div>
    <BasicTable
      ref="basicTableRef"
      :data-source="dataSource.data"
      :fields="fields"
      :is-loading="isLoading"
      :config-actions="['delete', 'file', 'select_del']"
      :filter-fileds="searchField"
      :filter="selectParm.name"
      :autoSearch="false"
      :select-box-menu="selectMenu"
      :placeholder-text="placeholderText"
      :start_dt="selectParm.startDate"
      :end_dt="selectParm.endDate"
      :brd_dt="selectParm.brdDate"
      :total-count="dataSource.totalRowCount"
      :page-options="pageOptions"
      @deleteOk="onDeleteOk"
      @deleteSelectedItems="OnDeleteSelectedItems"
      @changeSelectBox="onChangeSelectBox"
      @selectDate="onSelectDate"
      @searchEvent="onSearchEvent"
      @selectCurrentPage="onSelectCurrentPage"
      @changePerPage="onChangePerPage"
    />
  </div>
</template>
<script>
import BasicTable from "../widget/table_basic.vue";
import { USER_ID } from "@/constants/config";
import "moment/locale/ko";
const moment = require("moment");
const date = new Date();
function get_date_str(date) {
  var sYear = date.getFullYear();
  var sMonth = date.getMonth() + 1;
  var sDate = date.getDate();

  sMonth = sMonth > 9 ? sMonth : "0" + sMonth;
  sDate = sDate > 9 ? sDate : "0" + sDate;

  return sYear + "" + sMonth + "" + sDate;
}
const endDay = get_date_str(date);
date.setDate(date.getDate() - 365);
const startDay = get_date_str(date);
export default {
  data() {
    return {
      selectParm: {
        startDate: startDay,
        endDate: endDay,
        brdDate: endDay,
        name: "",
        media: "A",
        rowPerPage: 30,
        selectPage: 1,
      },
      pageOptions: [30, 50, 100],
      itemsData: [
        {
          key: "audio",
          fieldsList: [
            "selected",
            "no",
            "name",
            "audioclipid",
            "masterfile",
            // "lastonairdate",
            "editor",
            "mastertime",
            "edittime",
            "editfile",
            "energyfile",
            "callfile",
            "actions",
          ],
          selectItemList: ["audioClipItem", "startEndDate"],
        },
        {
          key: "spot",
          fieldsList: [
            "selected",
            "no",
            "name",
            "audioclipid",
            "masterfile",
            "oprspotid",
            "editor",
            "mastertime",
            "edittime",
            "editfile",
            "energyfile",
            "callfile",
            "actions",
          ],
          selectItemList: ["audioClipItem", "brdDate"],
        },
        {
          key: "etc",
          fieldsList: [
            "selected",
            "no",
            "name",
            "audioclipid",
            "masterfile",
            "editor",
            "mastertime",
            "edittime",
            "editfile",
            "energyfile",
            "callfile",
            "actions",
          ],
          selectItemList: ["audioClipItem"],
        },
        {
          key: "filler",
          fieldsList: [
            "selected",
            "no",
            "name",
            "audioclipid",
            "masterfile",
            "enddate",
            "editor",
            "mastertime",
            "edittime",
            "editfile",
            "energyfile",
            "callfile",
            "actions",
          ],
          selectItemList: ["audioClipItem", "startEndDate"],
        },
        {
          key: "report",
          fieldsList: [
            "selected",
            "no",
            "name",
            "audioclipid",
            "masterfile",
            "onairdate",
            "editor",
            "mastertime",
            "edittime",
            "editfile",
            "energyfile",
            "callfile",
            "actions",
          ],
          selectItemList: ["audioClipItem", "startEndDate"],
        },
        {
          key: "product",
          fieldsList: [
            "selected",
            "no",
            "name",
            "audioclipid",
            "masterfile",
            "onairdate",
            "editor",
            "mastertime",
            "edittime",
            "editfile",
            "energyfile",
            "callfile",
            "actions",
          ],
          selectItemList: ["audioClipItem", "media", "startEndDate"],
        },
        {
          key: "song",
          fieldsList: [
            "selected",
            "no",
            "name",
            "audioclipid",
            "masterfile",
            // "lastonairdate",
            "editor",
            "mastertime",
            "edittime",
            "editfile",
            "energyfile",
            "callfile",
            "actions",
          ],
          selectItemList: ["audioClipItem", "startEndDate"],
        },
      ],
      selectMenu: [],
      all_selectMenu: [
        {
          key: "startEndDate",
          startVal: "",
          endVal: "",
          type: "startEndDate",
        },
        {
          key: "brdDate",
          brdVal: "",
          type: "brdDate",
        },
        {
          key: "audioClipItem",
          label: "소재",
          type: "selectBox",
          selected: "product",
          options: [
            {
              value: "audio",
              text: "AUDIO",
            },
            {
              value: "spot",
              text: "SPOT",
            },
            {
              value: "etc",
              text: "ETC",
            },
            {
              value: "filler",
              text: "FILLER",
            },
            {
              value: "report",
              text: "REPORT",
            },
            {
              value: "product",
              text: "PRODUCT",
            },
            {
              value: "song",
              text: "SONG",
            },
          ],
        },
        {
          key: "media",
          label: "매체",
          type: "selectBox",
          selected: "A",
          options: [
            {
              value: "A",
              text: "AM",
            },
            {
              value: "F",
              text: "FM",
            },
            {
              value: "D",
              text: "DMB",
            },
          ],
        },
      ],
      dataSource: [],
      isLoading: false,
      placeholderText: "이름을 입력하세요.",
      filter: "",
      searchField: [],
      fields: [],
      all_fields: [
        { key: "selected", label: "__select", thStyle: { width: "20px" } },
        {
          key: "no",
          label: "순서",
          sortable: false,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "name",
          label: "Name",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "audioclipid",
          label: "CLIP ID",
          sortable: true,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "masterfile",
          label: "소재 파일 경로",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "onairdate",
          label: "삭제기준일",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
          formatter: (value, key, item) => {
            return moment(value).format("YYYY-MM-DD");
          },
        },
        {
          key: "lastonairdate",
          label: "삭제기준일",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
          formatter: (value, key, item) => {
            return moment(value).format("YYYY-MM-DD");
          },
        },
        {
          key: "oprspotid",
          label: "OPRSPOTID",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "editor",
          label: "제작자",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "mastertime",
          label: "마스터링",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
          thStyle: { width: "250px" },
          formatter: (value, key, item) => {
            return moment(value).format("YYYY-MM-DD");
          },
        },
        {
          key: "edittime",
          label: "편집 시각",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
          formatter: (value, key, item) => {
            return moment(value).format("YYYY-MM-DD");
          },
        },
        {
          key: "editfile",
          label: "편집파일경로",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "energyfile",
          label: "에너지파일경로",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "callfile",
          label: "CALL파일경로",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "actions",
          label: "작업",
          thClass: "text-center",
          tdClass: "text-center",
          thStyle: { width: "200px" },
        },
      ],
      get_data_url: "/api/managementdeleteproducts/getDel",
      delete_url: "/api/managementdeleteproducts/DeleteAudioClipFile",
    };
  },
  components: { BasicTable },
  created() {
    this.setProducts("product");
  },
  methods: {
    getData() {
      this.isLoading = true;
      const productItem = this.selectMenu.find(
        (item) => item.key === "audioClipItem"
      );
      this.$http
        .post(
          this.get_data_url + productItem.selected + "list",
          this.selectParm
        )
        .then((res) => {
          if (res.status === 200) {
            this.dataSource = res.data.resultObject;
          } else {
            this.$fn.notify("server-error", { message: "조회 에러" });
          }
          this.isLoading = false;
        });
    },
    inDateSet(date, format) {
      return moment(date).format(format);
    },
    onChangeSelectBox(event, item) {
      switch (item.key) {
        case "audioClipItem":
          this.setProducts(event);
          this.selectParm.name = "";
          this.$refs.basicTableRef.clearSearchText();
          break;

        case "media":
          this.selectParm.media = event;
          this.getData();
          break;

        default:
          break;
      }
    },
    setProducts(key) {
      const item = this.itemsData.find((item) => item.key === key);
      const fields = [];
      const selectMenus = [];

      item.selectItemList.forEach((ele) => {
        const selectItem = this.all_selectMenu.find((menu) => menu.key === ele);
        if (selectItem) {
          selectMenus.push(selectItem);
        }
      });

      item.fieldsList.forEach((ele) => {
        const field = this.all_fields.find((field) => field.key === ele);
        fields.push(field);
      });

      this.fields = fields;
      this.selectMenu = selectMenus;
      this.getData();
    },
    onSelectDate(itemData) {
      this.selectParm = { ...this.selectParm, ...itemData };
      this.getData();
    },
    onSearchEvent(text) {
      this.selectParm.name = text;
      this.getData();
    },
    onSelectCurrentPage(selectPage) {
      this.selectParm.selectPage = selectPage;
      this.getData();
    },
    onChangePerPage(perPage) {
      this.selectParm.rowPerPage = perPage;
      this.getData();
    },
    onDeleteOk(rowData, permanentlyVal) {
      if (permanentlyVal.length > 0) {
        this.deleteData([rowData], true);
      } else {
        this.deleteData([rowData], false);
      }
    },
    OnDeleteSelectedItems(items, permanentlyVal) {
      if (permanentlyVal.length > 0) {
        this.deleteData(items, true);
      } else {
        this.deleteData(items, false);
      }
    },
    deleteData(data, permanentlyVal) {
      if (data) {
        const param = {
          userId: sessionStorage.getItem(USER_ID),
          permanentlyVal: permanentlyVal,
          ids: data,
        };
        this.$http.delete(this.delete_url, { data: param }).then((res) => {
          if (res.status === 200 && res.data.resultObject) {
            this.$fn.notify("primary", {
              message:
                "작업 완료 : 자세한 내용은 '자동삭제규칙'탭에서 로그를 확인하세요.",
            });
            this.getData();
          } else {
            this.$fn.notify("server-error", { message: "추가 에러" });
          }
        });
      }
    },
  },
};
</script>
