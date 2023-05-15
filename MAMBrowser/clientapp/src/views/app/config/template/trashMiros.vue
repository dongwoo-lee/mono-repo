<template>
  <div>
    <BasicTable
      :data-source="dataSource.data"
      :fields="fields"
      :is-loading="isLoading"
      :config-actions="['delete', 'select_del']"
      :autoSearch="false"
      :select-box-menu="selectMenu"
      :start_dt="selectParm.startDate"
      :end_dt="selectParm.endDate"
      :total-count="dataSource.totalRowCount"
      :page-options="pageOptions"
      alCount="dataSource.totalRowCount"
      @deleteOk="onDeleteOk"
      @deleteSelectedItems="OnDeleteSelectedItems"
      @changeSelectBox="onChangeSelectBox"
      @selectDate="onSelectDate"
      @selectCurrentPage="onSelectCurrentPage"
      @changePerPage="onChangePerPage"
    />
  </div>
</template>
<script>
import BasicTable from "../widget/table_basic.vue";
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
        audioType: "",
        rowPerPage: 30,
        selectPage: 1,
      },
      pageOptions: [30, 50, 100],
      selectMenu: [
        {
          key: "audioClipItem",
          label: "소재",
          type: "selectBox",
          selected: "",
          options: [
            {
              value: "",
              text: "전체",
            },
            {
              value: "AC",
              text: "AUDIO",
            },
            {
              value: "ST",
              text: "SPOT",
            },
            {
              value: "EC",
              text: "ETC",
            },
            {
              value: "FC",
              text: "FILLER",
            },
            {
              value: "RC",
              text: "REPORT",
            },
            {
              value: "PM",
              text: "PRODUCT",
            },
            {
              value: "SS",
              text: "SONG",
            },
          ],
        },
        {
          key: "startEndDate",
          startVal: "",
          endVal: "",
          type: "startEndDate",
        },
      ],
      dataSource: [],
      isLoading: false,
      fields: [
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
          key: "deltime",
          label: "삭제일",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
          formatter: (value, key, item) => {
            return moment(value).format("YYYY-MM-DD");
          },
        },
        {
          key: "userid",
          label: "ID",
          sortable: true,
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
      get_data_url: "/api/managementdeleteproducts/GetRecycleList",
      delete_url: "/api/managementdeleteproducts/",
    };
  },
  components: { BasicTable },
  created() {
    // this.getData();
  },
  methods: {
    getData() {
      this.isLoading = true;
      this.$http.post(this.get_data_url, this.selectParm).then((res) => {
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
          this.selectParm.audiotype = event;
          this.getData();
          break;

        default:
          break;
      }
    },
    onSelectDate(itemData) {
      this.selectParm = { ...this.selectParm, ...itemData };
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
    onDeleteOk(rowData) {
      if (rowData.personid) {
        this.deleteData(rowData.personid);
      }
    },
    deleteData(role) {
      if (role) {
        this.$http.delete(this.delete_url + role).then((res) => {
          if (res.status === 200 && res.data.resultObject) {
            this.getData();
          } else {
            this.$fn.notify("server-error", { message: "추가 에러" });
          }
        });
      }
    },
    OnDeleteSelectedItems(items) {
      console.log(items);
    },
  },
};
</script>
