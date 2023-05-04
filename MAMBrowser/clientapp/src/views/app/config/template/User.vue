<template>
  <div>
    <BasicTable
      :data-source="dataSource"
      :fields="fields"
      :is-loading="isLoading"
      :config-actions="['add', 'delete', 'modify']"
      :add-btn-name="addBtnName"
      :filter-fileds="searchField"
      :placeholder-text="placeholderText"
      @modifyConfigRowData="onModifyConfigRowData"
      @deleteOk="onDeleteOk"
      @openAddPopup="openAddPopup"
    />
    <AddModal
      :items="addItems"
      :modal-title="addBtnName"
      @editOk="onAddOk"
      @otherValidation="onOtherValidation"
      modal-id="modal-config-add"
    />
    <ModifyModal
      :items="modifyItems"
      :modal-title="modifyBtnName"
      :textDisabledList="['personid']"
      @editOk="onModifyOk"
      @otherValidation="onOtherValidation"
      modal-id="modal-config-modify"
    />
  </div>
</template>
<script>
import BasicTable from "../widget/table_basic.vue";
import AddModal from "../widget/popup_edit.vue";
import ModifyModal from "../widget/popup_edit.vue";
import "moment/locale/ko";
const moment = require("moment");
export default {
  data() {
    return {
      addBtnName: "사용자 추가",
      modifyBtnName: "사용자 편집",
      addItems: [],
      modifyItems: [],
      dataSource: [],
      isLoading: false,
      placeholderText: "이름, ID, 역할(인증 그룹)을 입렵하세요.",
      filter: "",
      searchField: ["personid", "personname", "role"],
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
          key: "personid",
          label: "ID",
          sortable: true,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        // {
        //   key: "passwd",
        //   label: "비밀번호",
        //   sortable: true,
        //   thClass: "text-center",
        //   tdClass: "text-center",
        // },
        {
          key: "personname",
          label: "이름",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "devision",
          label: "소속국실",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "departmant",
          label: "소속부서",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "indate",
          label: "등록일자",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
          thStyle: { width: "250px" },
          formatter: (value, key, item) => {
            return moment(value).format("YYYY-MM-DD");
          },
        },
        {
          key: "tel1",
          label: "사내 전화번호",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        // {
        //   key: "tel2",
        //   label: "개인 전화번호",
        //   sortable: true,
        //   thClass: "text-center",
        //   tdClass: "text-center",
        // },
        // {
        //   key: "emailid",
        //   label: "Email",
        //   thClass: "text-center",
        //   tdClass: "text-center",
        // },
        // {
        //   key: "rank",
        //   label: "직위",
        //   thClass: "text-center",
        //   tdClass: "text-center",
        // },
        {
          key: "role",
          label: "역할(인증 그룹)",

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
      items: [
        {
          key: "personid",
          label: "ID",
          item: "",
          value: "",
          type: "text",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "passwd",
          label: "비밀번호",
          item: "",
          value: "",
          type: "text",
          inputType: "password",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "passwd_re",
          label: "비밀번호 확인",
          item: "",
          value: "",
          type: "text",
          inputType: "password",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "personname",
          label: "이름",
          item: "",
          value: "",
          type: "text",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "devision",
          label: "소속국실",
          item: "",
          value: "",
          type: "text",
          inputType: "password",
          state: null,
          maxLength: 20,
        },
        {
          key: "department",
          label: "소속부서",
          item: "",
          value: "",
          type: "text",
          state: null,
          maxLength: 20,
        },
        {
          key: "indate",
          label: "등록일자",
          item: "",
          value: "",
          type: "text",
          inputType: "date",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "tel1",
          label: "사내 전화번호",
          item: "",
          value: "",
          type: "text",
          inputType: "tel",
          state: null,
          maxLength: 20,
        },
        {
          key: "tel2",
          label: "개인 전화번호",
          item: "",
          value: "",
          type: "text",
          inputType: "tel",
          state: null,
          maxLength: 20,
        },
        {
          key: "emailid",
          label: "Email",
          item: "",
          value: "",
          type: "text",
          inputType: "email",
          state: null,
          maxLength: 20,
        },
        {
          key: "rank",
          label: "직위",
          item: "",
          value: "",
          type: "text",
          state: null,
          maxLength: 20,
        },
        {
          key: "role",
          label: "역할(인증 그룹)",
          item: "",
          value: "",
          type: "select",
          selectOptions: [],
          state: "notNull",
          maxLength: 20,
        },
      ],
      get_data_url: "/api/managementsystem/getUser",
      add_url: "/api/managementsystem/addUser",
      update_url: "/api/managementsystem/updateUser",
      delete_url: "/api/managementsystem/deleteUser?personid=",
      options_url: "/api/Managementsystem/GetRoleOptions",
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
      this.$http.post(this.get_data_url).then((res) => {
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
    getItemOptions() {
      const url = "/api/Managementsystem/GetRoleOptions";
      this.$http.get(url).then((res) => {
        if (res.status === 200) {
          const options = res.data.resultObject;
          Object.keys(options).forEach((key) => {
            this.items.forEach((item) => {
              if (item.key === key) {
                item.selectOptions = options[key];
              }
            });
          });
        }
      });
    },
    setAddItems() {
      this.addItems = _.cloneDeep(this.items);
      this.addItems.forEach((ele) => {
        if (ele.key === "indate") {
          ele.value = this.inDateSet(new Date(), "YYYY-MM-DD");
        }
      });
    },
    setModifyItems(rowData) {
      this.modifyItems = _.cloneDeep(this.items);
      Object.keys(rowData).forEach((ele) => {
        this.modifyItems.forEach((item) => {
          if (item.key === ele) {
            if (item.key === "indate") {
              item.value = this.inDateSet(rowData["indate"], "YYYY-MM-DD");
            } else {
              item.value = rowData[ele];
            }
          }
          if (item.key === "passwd_re") {
            item.value = rowData["passwd"];
          }
        });
      });
    },
    async openAddPopup() {
      await this.setAddItems();
      this.$bvModal.show("modal-config-add");
    },
    async onModifyConfigRowData(rowData) {
      await this.setModifyItems(rowData);
      this.$bvModal.show("modal-config-modify");
    },
    onDeleteOk(rowData) {
      if (rowData.personid) {
        this.deleteData(rowData.personid);
      }
    },
    onAddOk(editVal) {
      const pram = {};
      editVal.forEach((ele) => {
        pram[ele.key] = ele.editedVal;
      });
      this.insertData(pram);
    },
    onOtherValidation(text, item, items_copy) {
      if (item.key === "passwd" || item.key === "passwd_re") {
        const passwdItem = items_copy.filter((ele) => ele.key === "passwd");
        const passwd_re = items_copy.filter((ele) => ele.key === "passwd_re");
        if (
          passwdItem[0].editedVal === passwd_re[0].editedVal &&
          passwdItem[0].editedVal
        ) {
          passwd_re[0].isState = null;
        } else {
          passwd_re[0].isState = false;
        }
      }
    },
    onModifyOk(editVal) {
      const pram = {};
      editVal.forEach((ele) => {
        pram[ele.key] = ele.editedVal;
      });
      this.updataData(pram);
    },
    insertData(pram) {
      this.$http.post(this.add_url, pram).then((res) => {
        if (res.status === 200 && res.data.resultObject) {
          this.getData();
          this.$bvModal.hide("modal-config-add");
        } else {
          this.$fn.notify("server-error", { message: "추가 에러" });
        }
      });
    },
    updataData(pram) {
      this.$http.post(this.update_url, pram).then((res) => {
        if (res.status === 200 && res.data.resultObject) {
          this.getData();
          this.$bvModal.hide("modal-config-modify");
        } else {
          this.$fn.notify("server-error", { message: "추가 에러" });
        }
      });
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
  },
};
</script>
