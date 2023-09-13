<template>
  <div>
    <BasicTable
      ref="basicTableRef"
      :data-source="dataSource.data"
      :fields="fields"
      :is-loading="isLoading"
      :start_dt="selectParm.startDate"
      :end_dt="selectParm.endDate"
      :filter-fileds="searchField"
      :filter="selectParm.description"
      :placeholder-text="placeholderText"
      :autoSearch="false"
      :select-box-menu="selectMenu"
      :total-count="dataSource.totalRowCount"
      :page-options="pageOptions"
      :delete-option-button-title="deleteOptionTitle"
      @selectDate="onSelectDate"
      @selectCurrentPage="onSelectCurrentPage"
      @changePerPage="onChangePerPage"
      @searchEvent="onSearchEvent"
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
date.setDate(date.getDate() - 7);
const startDay = get_date_str(date);

export default {
  data() {
    return {
      selectParm: {
        systemCode: "S06",
        startDate: startDay,
        endDate: endDay,
        description: "",
        rowPerPage: 100,
        selectPage: 1,
      },
      pageOptions: [30, 50, 100],
      dataSource: [],
      isLoading: false,
      selectMenu: [
        {
          key: "startEndDate",
          startVal: "",
          endVal: "",
          type: "startEndDate",
        },
      ],
      placeholderText: "설명을 입력하세요.",
      searchField: [],
      fields: [
        {
          key: "no",
          label: "순서",
          sortable: false,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
          thStyle: { width: "60px" },
        },
        {
          key: "logLevel",
          label: "logLevel",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        // {
        //   key: "systemCode",
        //   label: "systemCode",
        //   sortable: true,
        //   thClass: "text-center",
        //   tdClass: "text-center",
        // },
        // {
        //   key: "category",
        //   label: "category",
        //   sortable: true,
        //   thClass: "text-center",
        //   tdClass: "text-center",
        // },
        {
          key: "description",
          label: "description",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "note",
          label: "note",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
          formatter: (value) => {
            if (value) {
              return "__filePath : " + value;
            }
          },
        },
        {
          key: "userID",
          label: "userID",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "userName",
          label: "userName",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "regDtm",
          label: "삭제일",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
          formatter: (value, key, item) => {
            return moment(value).format("YYYY-MM-DD HH:mm:ss");
          },
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
            value: 1,
            text:
              "1개월" +
              this.inDateSet(
                new Date().setMonth(new Date().getMonth() - 1),
                " (YYYY년 MM월 DD일 이전소재 삭제)"
              ),
          },
        ],
      },
      deleteCycleItem: { key: "CYCLE_TIME", label: "삭제 주기 옵션" },
      get_data_url: "/api/managementdeleteproducts/GetCommLogList",
      delete_url: "/api/managementdeleteproducts/DeleteRecycle",
    };
  },
  components: { BasicTable, DeleteOptionModal },
  created() {
    this.setSelectOptions();
    this.setDeleteOpions();
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
    setSelectOptions() {
      for (let index = 3; index <= 24; index = index + 3) {
        const test = {
          value: index,
          text:
            index +
            "개월" +
            this.inDateSet(
              new Date().setMonth(new Date().getMonth() - index),
              " (YYYY년 MM월 DD일 이전소재 삭제)"
            ),
        };
        this.deleteOptionKeyList.selectOptions.push(test);
      }
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
    onSearchEvent(text) {
      this.selectParm.description = text;
      this.getData();
    },
    async setDeleteOpions() {
      const resultItemList = [];
      const productList = [
        "DL3_MP3_DEL_CYCLE",
        "DL3_WAV_DEL_CYCLE",
        "MYDISK_TRASH_DEL_CYCLE",
      ];
      const deleteOptionData = await this.getDeleteOptionsData();
      productList.forEach((ele) => {
        const deleteOpionItem = { ...this.deleteOptionKeyList };
        const optionVal = deleteOptionData.find((data) => data.name === ele);
        deleteOpionItem.key = ele;
        switch (deleteOpionItem.key) {
          case "DL3_MP3_DEL_CYCLE":
            deleteOpionItem.label = "DL3 (MAP)";
            break;
          case "DL3_WAV_DEL_CYCLE":
            deleteOpionItem.label = "DL3 (WAV)";
            break;
          case "MYDISK_TRASH_DEL_CYCLE":
            deleteOpionItem.label = "MY 디스크 (휴지통)";
            break;
          default:
            break;
        }
        deleteOpionItem.value = optionVal.value;
        resultItemList.push(deleteOpionItem);
      });
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
