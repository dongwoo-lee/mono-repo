<template>
  <div>
    <BasicTable
      ref="basicTableRef"
      :data-source="dataSource.data"
      :fields="fields"
      :is-loading="isLoading"
      :config-actions="['delete', 'select_del', 'download']"
      :autoSearch="false"
      del-name="audioclipid"
      :select-box-menu="selectMenu"
      :start_dt="selectParm.startDate"
      :end_dt="selectParm.endDate"
      :total-count="dataSource.totalRowCount"
      :page-options="pageOptions"
      :delete-option-button-title="deleteOptionTitle"
      @deleteOk="onDeleteOk"
      @downloadConfigRowData="onDownloadConfigRowData"
      @deleteSelectedItems="OnDeleteSelectedItems"
      @changeSelectBox="onChangeSelectBox"
      @selectDate="onSelectDate"
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
      deleteOptionTitle: "영구 삭제 옵션",
      deleteOptionsItems: [],
      deleteOptionKeyList: {
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
          {
            value: 14,
            text:
              "14일" +
              this.inDateSet(
                new Date().setDate(new Date().getDate() - 14),
                " (YYYY년 MM월 DD일 이전소재 삭제)"
              ),
          },
          {
            value: 21,
            text:
              "21일" +
              this.inDateSet(
                new Date().setDate(new Date().getDate() - 21),
                " (YYYY년 MM월 DD일 이전소재 삭제)"
              ),
          },
          {
            value: 30,
            text:
              "30일" +
              this.inDateSet(
                new Date().setDate(new Date().getDate() - 30),
                " (YYYY년 MM월 DD일 이전소재 삭제)"
              ),
          },
        ],
      },
      deleteCycleItem: { key: "CYCLE_TIME_TRASH", label: "삭제 주기 옵션" },
      get_data_url: "/api/managementdeleteproducts/GetRecycleList",
      delete_url: "/api/managementdeleteproducts/DeleteRecycle",
    };
  },
  components: { BasicTable, DeleteOptionModal },
  created() {
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
    async setDeleteOpions() {
      const resultItemList = [];
      const productList = this.selectMenu.find(
        (item) => item.key === "audioClipItem"
      );
      const deleteOptionData = await this.getDeleteOptionsData();
      productList.options.forEach((ele) => {
        if (ele.value !== "") {
          const deleteOpionItem = { ...this.deleteOptionKeyList };
          const optionVal = deleteOptionData.find(
            (data) => data.name === ele.text + "_DEL_CYCLE"
          );
          deleteOpionItem.key = ele.text + "_DEL_CYCLE";
          deleteOpionItem.label = ele.text;
          deleteOpionItem.value = optionVal.value;
          resultItemList.push(deleteOpionItem);
        }
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
    async onDeleteOk(rowData) {
      this.$refs.basicTableRef.onOverlayValTrue();
      if (rowData) {
        await this.deleteData([rowData]);
      }
      this.$refs.basicTableRef.onOverlayValFalse();
      this.$bvModal.hide("modal-config-del");
    },
    async deleteData(data) {
      if (data) {
        const param = {
          userId: sessionStorage.getItem(USER_ID),
          systemCd: "S06",
          ids: data,
        };
        await this.$http
          .delete(this.delete_url, { data: param })
          .then((res) => {
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
    onDownloadConfigRowData(rowData) {
      this.$http
        .get(
          `/api/managementdeleteproducts/RecycleFileDownload?guid=${
            rowData.masterfile
          }&userid=${sessionStorage.getItem(USER_ID)}`
        )
        .then((res) => {
          if (res.status === 200) {
            const link = document.createElement("a");
            link.href = `/api/managementdeleteproducts/RecycleFileDownload?guid=${
              rowData.masterfile
            }&userid=${sessionStorage.getItem(USER_ID)}`;
            document.body.appendChild(link);
            link.click();
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
    async OnDeleteSelectedItems(items) {
      this.$refs.basicTableRef.onOverlayValTrue();
      if (items) {
        await this.deleteData(items);
      }
      this.$refs.basicTableRef.onOverlayValFalse();
      this.$bvModal.hide("modal-config-file-del");
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
