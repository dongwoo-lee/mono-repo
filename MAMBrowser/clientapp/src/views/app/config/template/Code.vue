<template>
  <div>
    <BasicTable
      ref="basicTableRef"
      :data-source="dataSource"
      :fields="fields"
      :is-loading="isLoading"
      :config-actions="['add', 'delete', 'modify']"
      :add-btn-name="addBtnName"
      :select-box-menu="selectMenu"
      @modifyConfigRowData="onModifyConfigRowData"
      @deleteOk="onDeleteOk"
      @openAddPopup="openAddPopup"
      @changeSelectBox="onChangeSelectBox"
    />
    <AddModal
      :items="addItems"
      :modal-title="addBtnName"
      :textDisabledList="['creator']"
      @editOk="onAddOk"
      modal-id="modal-config-add"
    />
    <ModifyModal
      :items="modifyItems"
      :modal-title="modifyBtnName"
      :textDisabledList="['codeid', 'creator', 'scodeid', 'spotid', 'media']"
      @editOk="onModifyOk"
      modal-id="modal-config-modify"
    />
  </div>
</template>
<script>
import BasicTable from "../widget/table_basic.vue";
import AddModal from "../widget/popup_edit.vue";
import ModifyModal from "../widget/popup_edit.vue";
import { USER_ID } from "@/constants/config";
export default {
  data() {
    return {
      selectMenu: [
        {
          key: "product",
          label: "소재",
          type: "selectBox",
          selected: "stsp",
          options: [
            {
              value: "stsp",
              text: "부조용 SPOT",
            },
            {
              value: "songm",
              text: "노래 대분류",
            },
            {
              value: "songs",
              text: "노래 소분류",
            },
            {
              value: "rpt",
              text: "리포트 분류",
            },
            {
              value: "audio",
              text: "오디오 소재분류",
            },
            {
              value: "filler",
              text: "Filler 분류",
            },
            {
              value: "etc",
              text: "기타소재 분류",
            },
            {
              value: "spot",
              text: "SPOT CODE",
            },
          ],
        },
      ],
      selectedProduct: "stsp",
      addBtnName: "코드 추가",
      modifyBtnName: "코드 편집",
      addItems: [],
      modifyItems: [],
      dataSource: [],
      isLoading: false,
      filter: "",
      all_fields: [
        {
          key: "no",
          label: "순서",
          sortable: false,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "codeid",
          label: "분류 ID",
          sortable: true,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "codename",
          label: "분류 이름",
          sortable: true,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "mcodeid",
          label: "대분류 ID",
          sortable: true,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "scodeid",
          label: "소분류 ID",
          sortable: true,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "scodename",
          label: "소분류 이름",
          sortable: true,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "spotid",
          label: "분류 ID",
          sortable: true,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "spotname",
          label: "분류 이름",
          sortable: true,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "media",
          label: "매체 구분",
          sortable: true,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },

        {
          key: "creator",
          label: "생성자",
          sortable: true,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "pd",
          label: "PD",

          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "ad",
          label: "AD",

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
      fields: [],
      all_items: [
        {
          key: "codeid",
          label: "분류 ID",
          item: "",
          value: "",
          type: "text",
          state: "notNull",
          maxLength: 10,
        },
        {
          key: "codename",
          label: "분류 이름",
          item: "",
          value: "",
          type: "text",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "mcodeid",
          label: "대분류 ID",
          item: "",
          value: "",
          type: "select",
          selectOptions: [],
          state: "notNull",
          maxLength: 10,
        },
        {
          key: "scodeid",
          label: "소분류 ID",
          item: "",
          value: "",
          type: "text",
          state: "notNull",
          maxLength: 10,
        },
        {
          key: "scodename",
          label: "소분류 이름",
          item: "",
          value: "",
          type: "text",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "spotid",
          label: "분류 ID",
          item: "",
          value: "",
          type: "text",
          state: "notNull",
          maxLength: 10,
        },
        {
          key: "spotname",
          label: "분류 이름",
          item: "",
          value: "",
          type: "text",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "media",
          label: "매체 구분",
          item: "",
          value: "",
          type: "select",
          selectOptions: [],
          state: "notNull",
          maxLength: 10,
        },
        {
          key: "creator",
          label: "생성자",
          item: "",
          value: "",
          type: "text",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "pd",
          label: "PD",
          item: "",
          value: "",
          type: "text",
          state: null,
          maxLength: 20,
        },
        {
          key: "ad",
          label: "AD",
          item: "",
          value: "",
          type: "text",
          state: null,
          maxLength: 10,
        },
      ],
      items: [],
      get_data_url: "/api/managementsystem/get",
      add_url: "/api/managementsystem/add",
      update_url: "/api/managementsystem/update",
      delete_url: "/api/managementsystem/delete",
    };
  },
  components: { BasicTable, AddModal, ModifyModal },
  created() {
    this.getData();
    this.getItemOptions();
  },
  methods: {
    getData() {
      this.isLoading = true;
      this.$http
        .post(this.get_data_url + this.selectedProduct + "code")
        .then((res) => {
          if (res.status === 200 && res.data.resultObject) {
            const cols = ["no", ...Object.keys(res.data.resultObject[0])];
            const result_fields = [];
            cols.push("actions");
            this.all_fields.forEach((field) => {
              if (cols.includes(field.key)) {
                result_fields.push(field);
              }
            });
            this.dataSource = res.data.resultObject;
            this.fields = result_fields;
          } else {
            this.$fn.notify("server-error", { message: "조회 에러" });
          }
          this.isLoading = false;
        });
    },
    getItemOptions() {
      this.getCodeIdOptions();
      this.getMediaOptions();
    },
    getCodeIdOptions() {
      const url = "/api/Managementsystem/GetCodeIdOptions";
      this.$http.get(url).then((res) => {
        if (res.status === 200) {
          const options = res.data.resultObject;
          Object.keys(options).forEach((key) => {
            this.all_items.forEach((item) => {
              if (item.key === key) {
                item.selectOptions = options[key];
              }
            });
          });
        }
      });
    },
    getMediaOptions() {
      const url = "/api/Categories/media/mcrspot";
      this.$http.get(url).then((res) => {
        if (res.status === 200) {
          const options = res.data.resultObject.data;
          options.forEach((op) => {
            op.text = op.name;
            op.value = op.id;
            delete op.name;
            delete op.id;
          });
          this.all_items.forEach((item) => {
            if (item.key === "media") {
              item.selectOptions = options;
            }
          });
        }
      });
    },
    changeAudioCodeType(items) {
      items.forEach((ele) => {
        if (this.selectedProduct === "audio" && ele.key === "codename") {
          ele.type = "codename_check";
        }
      });
    },
    setAudioCodeItem(items) {
      const codenameItem = items.find((item) => item.key === "codename");
      if (
        codenameItem.value.includes("(폐지) ") &&
        codenameItem.value.substring(0, 5) === "(폐지) "
      ) {
        codenameItem.editedVal = codenameItem.value.substring(5);
        codenameItem.isStop = true;
      } else {
        codenameItem.editedVal = codenameItem.value;
        codenameItem.isStop = false;
      }
    },
    setAddItems() {
      const result_items = [];
      this.all_items.forEach((item) => {
        const isCheckItem = this.fields.some((field) => field.key === item.key);
        if (item.key === "creator") {
          item.value = sessionStorage.getItem(USER_ID);
        }
        if (isCheckItem) result_items.push({ ...item });
      });
      this.changeAudioCodeType(result_items);
      this.addItems = result_items;
      if (this.selectedProduct === "audio") {
        this.setAudioCodeItem(this.addItems);
      }
    },
    setModifyItems(rowData) {
      const result_items = [];
      this.all_items.forEach((item) => {
        const isCheckItem = this.fields.some((field) => field.key === item.key);
        if (isCheckItem) result_items.push({ ...item });
      });
      this.changeAudioCodeType(result_items);
      this.modifyItems = result_items;

      Object.keys(rowData).forEach((ele) => {
        this.modifyItems.forEach((item) => {
          if (item.key === ele) {
            item.value = rowData[ele];
          }
        });
      });
      if (this.selectedProduct === "audio") {
        this.setAudioCodeItem(this.modifyItems);
      }
    },
    async openAddPopup() {
      await this.setAddItems();
      this.$bvModal.show("modal-config-add");
    },
    async onModifyConfigRowData(rowData) {
      await this.setModifyItems(rowData);
      this.$bvModal.show("modal-config-modify");
    },
    onChangeSelectBox(event, item) {
      this.selectedProduct = event;
      this.getData();
    },
    async onDeleteOk(rowData) {
      this.$refs.basicTableRef.onOverlayValTrue();
      if (rowData) {
        await this.deleteData(rowData);
      }
      this.$refs.basicTableRef.onOverlayValFalse();
      this.$bvModal.hide("modal-config-del");
    },
    onAddOk(editVal) {
      const pram = {};
      editVal.forEach((ele) => {
        if (ele.isStop) {
          pram[ele.key] = "(폐지) " + ele.editedVal;
        } else {
          pram[ele.key] = ele.editedVal;
        }
      });
      this.insertData(pram);
    },
    onModifyOk(editVal) {
      const pram = {};
      editVal.forEach((ele) => {
        if (ele.isStop) {
          pram[ele.key] = "(폐지) " + ele.editedVal;
        } else {
          pram[ele.key] = ele.editedVal;
        }
      });
      this.updataData(pram);
    },
    insertData(pram) {
      this.$http
        .post(this.add_url + this.selectedProduct + "code", pram)
        .then((res) => {
          if (res.status === 200 && res.data.resultObject) {
            this.getData();
            this.$bvModal.hide("modal-config-add");
          } else {
            this.$fn.notify("server-error", { message: "추가 에러" });
          }
        });
    },
    updataData(pram) {
      if (Object.keys(pram).includes("creator")) {
        pram.creator = sessionStorage.getItem(USER_ID);
      }
      this.$http
        .post(this.update_url + this.selectedProduct + "code", pram)
        .then((res) => {
          if (res.status === 200 && res.data.resultObject) {
            this.getData();
            this.$bvModal.hide("modal-config-modify");
          } else {
            this.$fn.notify("server-error", { message: "추가 에러" });
          }
        });
    },
    async deleteData(rowData) {
      await this.$http
        .delete(this.delete_url + this.selectedProduct + "code", {
          data: rowData,
        })
        .then((res) => {
          if (res.status === 200 && res.data.resultObject) {
            this.$fn.notify("primary", {
              message: "삭제 완료",
            });
            this.getData();
          } else {
            this.$fn.notify("server-error", { message: "추가 에러" });
          }
        });
    },
  },
};
</script>
