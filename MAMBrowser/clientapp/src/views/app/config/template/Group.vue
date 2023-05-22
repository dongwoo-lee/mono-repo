<template>
  <div>
    <BasicTable
      ref="basicTableRef"
      :data-source="dataSource"
      :fields="fields"
      :is-loading="isLoading"
      :config-actions="['add', 'delete', 'modify']"
      :add-btn-name="addBtnName"
      @modifyConfigRowData="onModifyConfigRowData"
      @deleteOk="onDeleteOk"
      @openAddPopup="openAddPopup"
    />
    <AddModal
      :items="addItems"
      :modal-title="addBtnName"
      @editOk="onAddOk"
      @checkGroupClick="onCheckItemClick"
      @otherValidation="onOtherValidation"
      modal-id="modal-config-add"
    />
    <ModifyModal
      :items="modifyItems"
      :modal-title="modifyBtnName"
      :textDisabledList="['role']"
      @editOk="onModifyOk"
      @checkGroupClick="onCheckItemClick"
      @otherValidation="onOtherValidation"
      modal-id="modal-config-modify"
    />
  </div>
</template>
<script>
import BasicTable from "../widget/table_basic.vue";
import AddModal from "../widget/popup_edit.vue";
import ModifyModal from "../widget/popup_edit.vue";
export default {
  data() {
    return {
      addBtnName: "그룹 추가",
      modifyBtnName: "그룹 편집",
      addItems: [],
      modifyItems: [],
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
          key: "role",
          label: "역할(인증 그룹)",
          sortable: true,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "dbid",
          label: "DB사용자 ID",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        // {
        //   key: "dbpasswd",
        //   label: "DBPasswd",
        //   sortable: true,
        //   thClass: "text-center",
        //   tdClass: "text-center",
        // },
        // {
        //   key: "approle",
        //   label: "App 인증그룹",
        //   sortable: true,
        //   thClass: "text-center",
        //   tdClass: "text-center",
        // },
        // {
        //   key: "sysrole",
        //   label: "시스템 인증그룹",
        //   sortable: true,
        //   thClass: "text-center",
        //   tdClass: "text-center",
        // },
        // {
        //   key: "serverrole",
        //   label: "서버 인증그룹",
        //   sortable: true,
        //   thClass: "text-center",
        //   tdClass: "text-center",
        // },
        {
          key: "code",
          label: "인증 상수",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "rolE_NAME",
          label: "인증 이름",
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
      items: [
        {
          key: "role",
          label: "역할(인증 그룹)",
          item: "",
          value: "",
          type: "text",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "dbid",
          label: "DB사용자 ID",
          item: "",
          value: "",
          type: "text",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "dbpasswd",
          label: "DBPasswd",
          item: "",
          value: "",
          type: "text",
          inputType: "password",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "dbpasswd_re",
          label: "DBPasswd_re",
          item: "",
          value: "",
          type: "text",
          inputType: "password",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "approle",
          label: "App 인증그룹",
          item: "",
          value: "",
          type: "text",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "sysrole",
          label: "시스템 인증그룹",
          item: "",
          value: "",
          type: "text",
          state: "notNull",
          maxLength: 20,
        },
        {
          key: "serverrole",
          label: "서버 인증그룹",
          item: "",
          value: "",
          type: "text",
          state: null,
          maxLength: 20,
        },
        {
          key: "rolE_NAME",
          label: "인증 이름",
          item: "",
          value: "",
          type: "text",
          state: null,
          maxLength: 20,
        },
        {
          key: "code",
          label: "인증 상수",
          item: "",
          value: "0000",
          type: "check",
          checkGroups: [
            {
              groupLabel: "광고&관리",
              options: [
                { text: "관리단말사용", value: 8 },
                { text: "Reserved", value: 4 },
                { text: "광고사용", value: 2 },
                { text: "광고관리", value: 1 },
              ],
              selected: [],
            },
            {
              groupLabel: "편성",
              options: [
                { text: "관리단말사용", value: 8 },
                { text: "Reserved", value: 4 },
                { text: "편성사용", value: 2 },
                { text: "편성관리", value: 1 },
              ],
              selected: [],
            },
            {
              groupLabel: "송출",
              options: [
                { text: "Reserved", value: 8 },
                { text: "Reserved", value: 4 },
                { text: "송출사용", value: 2 },
                { text: "송출관리", value: 1 },
              ],
              selected: [],
            },
            {
              groupLabel: "제작",
              options: [
                { text: "제작검색", value: 8 },
                { text: "제한된 제작사용", value: 4 },
                { text: "제작사용", value: 2 },
                { text: "제작관리", value: 1 },
              ],
              selected: [],
            },
          ],
          state: null,
        },
      ],
      get_data_url: "/api/managementsystem/getGroup",
      add_url: "/api/managementsystem/addGroup",
      update_url: "/api/managementsystem/updateGroup",
      delete_url: "/api/managementsystem/deleteGroup?role=",
    };
  },
  components: { BasicTable, AddModal, ModifyModal },
  created() {
    this.getData();
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
    setAddItems() {
      this.addItems = _.cloneDeep(this.items);
    },
    setModifyItems(rowData) {
      this.modifyItems = _.cloneDeep(this.items);
      Object.keys(rowData).forEach((ele) => {
        this.modifyItems.forEach((item) => {
          if (item.key === ele) {
            item.value = rowData[ele];
          }
          if (item.key === "dbpasswd_re") {
            item.value = rowData["dbpasswd"];
          }
        });
      });

      this.setHexCode();
    },
    setHexCode() {
      this.modifyItems.forEach((item) => {
        if (item.type === "check") {
          const hex_num = item.value.split("");
          const hex_list = [8, 4, 2, 1];
          hex_num.forEach((num, index) => {
            let digit = parseInt(num, 16).toString(10);
            let result_num = [];
            hex_list.forEach((hex) => {
              if (digit >= hex) {
                digit = digit - hex;
                result_num.push(hex);
              }
            });
            item.checkGroups[index].selected = result_num;
          });
        }
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
    async onDeleteOk(rowData) {
      this.$refs.basicTableRef.onOverlayValTrue();
      if (rowData.role) {
        await this.deleteData(rowData.role);
      }
      this.$refs.basicTableRef.onOverlayValFalse();
      this.$bvModal.hide("modal-config-del");
    },
    onAddOk(editVal) {
      const pram = {};
      editVal.forEach((ele) => {
        pram[ele.key] = ele.editedVal;
      });
      this.insertData(pram);
    },
    onCheckItemClick(event, item, index) {
      let digit = item.editedVal.split("");
      if (event.length > 0) {
        digit[index] = event.reduce((acc, curr) => acc + curr).toString(16);
      } else {
        digit[index] = 0;
      }
      item.editedVal = digit.join("");
    },
    async onOtherValidation(text, item, items_copy) {
      // const regex = /[ㄱ-ㅎㅏ-ㅣ가-힣]/g;
      // const nonKoreanColumns = [
      //   "role",
      //   "dbid",
      //   "approle",
      //   "sysrole",
      //   "serverrole",
      // ];
      if (item.key === "dbpasswd" || item.key === "dbpasswd_re") {
        const passwdItem = items_copy.filter((ele) => ele.key === "dbpasswd");
        const passwd_re = items_copy.filter((ele) => ele.key === "dbpasswd_re");
        if (
          passwdItem[0].editedVal === passwd_re[0].editedVal &&
          passwdItem[0].editedVal
        ) {
          passwd_re[0].isState = null;
        } else {
          passwd_re[0].isState = false;
        }
      }
      // if (nonKoreanColumns.includes(item.key)) {
      //   const colItem = items_copy.find((ele) => ele.key === item.key);
      //   if (regex.test(text)) {
      //     colItem.editedVal = "";
      //     console.log(colItem.editedVal);
      //     colItem.isState = false;
      //     colItem.editedVal = await colItem.editedVal.replace(regex, "");
      //     text = colItem.editedVal;
      //   } else {
      //     colItem.isState = null;
      //   }
      // }
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
    async deleteData(role) {
      if (role) {
        await this.$http.delete(this.delete_url + role).then((res) => {
          if (res.status === 200 && res.data.resultObject) {
            this.$fn.notify("primary", {
              message: "삭제 완료",
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
