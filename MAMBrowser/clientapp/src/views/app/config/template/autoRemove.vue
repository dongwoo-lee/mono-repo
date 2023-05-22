<template>
  <div>
    <BasicTable
      ref="basicTableRef"
      :data-source="dataSource.data"
      :fields="fields"
      :is-loading="isLoading"
      :start_dt="selectParm.startDate"
      :end_dt="selectParm.endDate"
      :total-count="dataSource.totalRowCount"
      :page-options="pageOptions"
      :delete-option-button-title="deleteOptionTitle"
      @selectCurrentPage="onSelectCurrentPage"
      @changePerPage="onChangePerPage"
      @deleteOptionEvent="onDeleteOptionEvent"
    />
    <DeleteOptionModal
      :items="deleteOptionsItems"
      :modal-title="deleteOptionTitle"
      :cycle-time="deleteCycleItem"
      @editOk="onDeleteOptionModalOk"
    />
  </div>
</template>
<script>
import BasicTable from "../widget/table_basic.vue";
import DeleteOptionModal from "../widget/popup_delete_option.vue";
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
const toDay = new Date();

export default {
  data() {
    return {
      selectParm: {
        startDate: startDay,
        endDate: endDay,
        rowPerPage: 30,
        selectPage: 1,
      },
      pageOptions: [30, 50, 100],
      dataSource: [],
      isLoading: false,
      fields: [
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
      deleteOptionTitle: "자동 삭제 옵션",
      deleteOptionsItems: [],
      deleteOptionKeyList: {
        //여기 month 기준 & 3의 배수로 24개월까지 변경
        key: "",
        label: "",
        item: "",
        value: 0,
        selectOptions: [
          {
            value: 7,
            text:
              "7일" +
              this.inDateSet(
                new Date().setDate(new Date().getDate() - 7),
                " (YYYY년 MM월 DD일 이전소재 삭제)"
              ),
          },
        ],
      },
      deleteCycleItem: { key: "CYCLE_TIME", label: "삭제 주기 옵션" },
      get_data_url: "/api/managementdeleteproducts/GetRecycleList",
      delete_url: "/api/managementdeleteproducts/DeleteRecycle",
    };
  },
  components: { BasicTable, DeleteOptionModal },
  created() {
    this.setDeleteOpions();
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
    async setDeleteOpions() {
      const resultItemList = [];
      const deleteOptionData = await this.getDeleteOptionsData();
      // productList.options.forEach((ele) => {
      //   if (ele.value !== "") {
      //     const deleteOpionItem = { ...this.deleteOptionKeyList };
      //     const optionVal = deleteOptionData.find(
      //       (data) => data.name === ele.text + "_DEL_CYCLE"
      //     );
      //     deleteOpionItem.key = ele.text + "_DEL_CYCLE";
      //     deleteOpionItem.label = ele.text;
      //     deleteOpionItem.value = optionVal.value;
      //     resultItemList.push(deleteOpionItem);
      //   }
      // });
      this.deleteOptionsItems = resultItemList;
    },
    async getDeleteOptionsData() {
      const url = "/api/options/S01G07C001";
      return await this.$http.get(url).then((res) => {
        if (res.status === 200) {
          const data = res.data.resultObject.data;
          const cycleData = data.find(
            (item) => item.name === this.deleteCycleItem.key
          );
          if (cycleData) {
            this.deleteCycleItem.value = cycleData.value;
          }
          return data;
        }
      });
    },
    inDateSet(date, format) {
      return moment(date).format(format);
    },
    onSelectCurrentPage(selectPage) {
      this.selectParm.selectPage = selectPage;
      this.getData();
    },
    onChangePerPage(perPage) {
      this.selectParm.rowPerPage = perPage;
      this.getData();
    },
    updateDeleteOptions(pram) {
      const url = "/api/options/S01G07C001";
      this.$http.post(url, pram).then((res) => {
        if (res.status === 200) {
          this.$fn.notify("primary", {
            message: "변경 완료",
          });
          this.setDeleteOpions();
          this.$bvModal.hide("modal-delete-option");
        } else {
          this.$fn.notify("server-error", { message: "추가 에러" });
        }
      });
    },
    onDeleteOptionEvent() {
      this.$bvModal.show("modal-delete-option");
    },
    onDeleteOptionModalOk(editVal, cycleTimeVal) {
      const pram = [];
      editVal.forEach((ele) => {
        pram.push({ name: ele.key, value: String(ele.editedVal) });
      });
      pram.push({ name: cycleTimeVal.key, value: cycleTimeVal.editedVal });
      this.updateDeleteOptions(pram);
    },
  },
};
</script>
