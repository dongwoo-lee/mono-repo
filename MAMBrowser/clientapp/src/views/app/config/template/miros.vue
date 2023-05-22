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
      :text-disabled-list="['maP_CD', 'systeM_CD']"
      @editOk="onAddOk"
      modal-id="modal-config-add"
    />
    <ModifyModal
      :items="modifyItems"
      :modal-title="modifyBtnName"
      :textDisabledList="[
        'code',
        'maP_CD',
        'grP_CD',
        'systeM_CD',
        'parenT_CODE',
      ]"
      @editOk="onModifyOk"
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
      all_codeAndName: [],
      selectMenu: [],
      all_selectMenu: [
        {
          key: "table",
          label: "구분",
          type: "selectBox",
          selected: "CommCode",
          options: [
            {
              value: "CommCode",
              text: "M30_COMM_CODE",
            },
            {
              value: "CommMenuMap",
              text: "M30_COMM_MENU_MAP",
            },
          ],
        },
        {
          key: "mapCd",
          label: "코드",
          type: "selectBox",
          selected: "",
          options: [],
        },
      ],
      selectedTable: "CommCode",
      selectedMapCode: "",
      addBtnName: "추가",
      modifyBtnName: "편집",
      addItems: [],
      modifyItems: [],
      dataSource: [],
      isLoading: false,
      fields: [],
      items: [],
      dataList: [
        {
          key: "CommCode",
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
              key: "code",
              label: "코드",
              sortable: true,
              sortDirection: "desc",
              thClass: "text-center",
              tdClass: "text-center",
            },
            {
              key: "parenT_CODE",
              label: "부모코드",
              sortable: true,
              sortDirection: "desc",
              thClass: "text-center",
              tdClass: "text-center",
              formatter: (value, item) => {
                const result = this.all_codeAndName.find(
                  (ele) => ele.value === value
                );
                return result ? result.text : "";
              },
            },
            {
              key: "name",
              label: "이름",
              sortable: true,
              sortDirection: "desc",
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
              key: "code",
              label: "CODE",
              item: "",
              value: "",
              type: "text",
              state: "notNull",
              maxLength: 10,
            },
            {
              key: "parenT_CODE",
              label: "부모코드",
              item: "",
              value: "",
              type: "select",
              selectOptions: [],
              state: null,
              maxLength: 20,
            },
            {
              key: "name",
              label: "이름",
              item: "",
              value: "",
              type: "text",
              state: null,
              maxLength: 10,
            },
          ],
        },
        {
          key: "CommMenuMap",
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
              key: "systeM_CD",
              label: "시스템코드",
              sortable: true,
              sortDirection: "desc",
              thClass: "text-center",
              tdClass: "text-center",
            },
            {
              key: "maP_CD",
              label: "MAP_CODE",
              sortable: true,
              sortDirection: "desc",
              thClass: "text-center",
              tdClass: "text-center",
            },
            {
              key: "grP_CD",
              label: "GRP_CODE",
              sortable: true,
              sortDirection: "desc",
              thClass: "text-center",
              tdClass: "text-center",
              formatter: (value, item) => {
                const result = this.all_codeAndName.find(
                  (ele) => ele.value === value
                );
                return result ? result.text : "";
              },
            },
            {
              key: "code",
              label: "CODE",
              sortable: true,
              sortDirection: "desc",
              thClass: "text-center",
              tdClass: "text-center",
              formatter: (value, item) => {
                const result = this.all_codeAndName.find(
                  (ele) => ele.value === value
                );
                return result ? result.text : "";
              },
            },
            {
              key: "visible",
              label: "VISIBLE",
              sortable: true,
              sortDirection: "desc",
              thClass: "text-center",
              tdClass: "text-center",
            },
            {
              key: "enable",
              label: "ENABLE",
              sortable: true,
              sortDirection: "desc",
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
              key: "systeM_CD",
              label: "시스템코드",
              item: "",
              value: "",
              type: "text",
              state: "notNull",
              maxLength: 10,
            },
            {
              key: "maP_CD",
              label: "MAP_CODE",
              item: "",
              value: "",
              type: "select",
              selectOptions: [],
              state: "notNull",
              maxLength: 20,
            },
            {
              key: "grP_CD",
              label: "GRP_CD",
              item: "",
              value: "",
              type: "select",
              selectOptions: [],
              state: "notNull",
              maxLength: 10,
            },
            {
              key: "code",
              label: "CODE",
              item: "",
              value: "",
              type: "select",
              selectOptions: [],
              state: "notNull",
              maxLength: 10,
            },
            {
              key: "visible",
              label: "VISIBLE",
              item: "",
              value: "",
              type: "select",
              selectOptions: [
                {
                  text: "Y",
                  value: "Y",
                },
                {
                  text: "N",
                  value: "N",
                },
              ],
              state: "notNull",
              maxLength: 20,
            },
            {
              key: "enable",
              label: "ENABLE",
              item: "",
              value: "",
              type: "select",
              selectOptions: [
                {
                  text: "Y",
                  value: "Y",
                },
                {
                  text: "N",
                  value: "N",
                },
              ],
              state: "notNull",
              maxLength: 20,
            },
          ],
        },
      ],
      get_data_url: "/api/managementsystem/get",
      add_url: "/api/managementsystem/add",
      update_url: "/api/managementsystem/update",
      delete_url: "/api/managementsystem/delete",
      options_url: "/api/Managementsystem/GetGroupByMirosCode",
    };
  },
  components: { BasicTable, AddModal, ModifyModal },
  created() {
    this.getData();
    this.selectMenu.push(this.all_selectMenu[0]);
    this.getItemOptions();
  },
  methods: {
    getData() {
      this.isLoading = true;
      let url = this.get_data_url + this.selectedTable;
      const data = this.dataList.find((ele) => ele.key === this.selectedTable);
      this.$http.post(url, this.selectedMapCode).then((res) => {
        if (res.status === 200 && res.data.resultObject) {
          this.fields = data.fields;
          this.items = data.items;
          this.dataSource = res.data.resultObject;
        } else {
          this.$fn.notify("server-error", { message: "조회 에러" });
        }
        this.isLoading = false;
      });
    },
    getItemOptions() {
      this.setMapCodeSelectMenuOptions();
      this.setCommCodeSelectMenuOptions();
      this.setMenuMapSelectMenuOptions();
    },
    async setCommCodeSelectMenuOptions() {
      const pram = {};
      const options = await this.getCodeOptions(pram);
      if (options) {
        this.all_codeAndName = options.code;
        const code = this.dataList.find((ele) => ele.key === "CommCode");
        code.items.find((item) => item.key === "parenT_CODE").selectOptions =
          options.code;
      }
    },
    async setMenuMapSelectMenuOptions() {
      const pram = { code: "S01", maxLength: 10 };
      const options = await this.getCodeOptions(pram);
      if (options) {
        const code = this.dataList.find((ele) => ele.key === "CommMenuMap");
        code.items.forEach((item) => {
          if (item.key === "grP_CD" || item.key === "code") {
            item.selectOptions = options.code;
          }
        });
      }
    },
    async setMapCodeSelectMenuOptions() {
      const pram = { parentCode: "S00G01C" };
      const options = await this.getCodeOptions(pram);
      if (options) {
        const mapCodes = this.all_selectMenu.find((ele) => ele.key === "mapCd");
        const code = this.dataList.find((ele) => ele.key === "CommMenuMap");
        mapCodes.options = options.code;
        mapCodes.selected = options.code[0].value;
        this.selectedMapCode = options.code[0].value;
        code.items.forEach((item) => {
          if (item.key === "maP_CD") {
            item.selectOptions = options.code;
          }
        });
      }
    },
    getCodeOptions(param) {
      return this.$http.post(this.options_url, param).then((res) => {
        if (res.status === 200 && res.data.resultObject) {
          return res.data.resultObject;
        } else {
          this.$fn.notify("server-error", { message: "추가 에러" });
        }
      });
    },
    setAddItems() {
      this.addItems = _.cloneDeep(this.items);
      this.addItems.forEach((ele) => {
        if (ele.key === "maP_CD") {
          ele.value = this.selectedMapCode;
        }
        if (ele.key === "systeM_CD") {
          ele.value = "S01";
        }
      });
    },
    setModifyItems(rowData) {
      this.modifyItems = _.cloneDeep(this.items);
      Object.keys(rowData).forEach((ele) => {
        this.modifyItems.forEach((item) => {
          if (item.key === ele) {
            item.value = rowData[ele];
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
    async onChangeSelectBox(event, item) {
      await this.switchSelectBox(event, item);
      this.getData();
    },
    switchSelectBox(event, item) {
      switch (item.key) {
        case "table":
          this.selectedTable = event;
          switch (event) {
            case "CommCode":
              const result = [];
              result.push(this.selectMenu[0]);
              this.selectMenu = result;
              break;
            case "CommMenuMap":
              const mapCodeMenu = this.all_selectMenu.find(
                (ele) => ele.key === "mapCd"
              );
              this.selectMenu.push(mapCodeMenu);
            default:
              break;
          }
          break;
        case "mapCd":
          this.selectedMapCode = event;
        default:
          break;
      }
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
        pram[ele.key] = ele.editedVal;
      });
      this.insertData(pram);
    },
    onModifyOk(editVal) {
      const pram = {};
      editVal.forEach((ele) => {
        pram[ele.key] = ele.editedVal;
      });
      this.updataData(pram);
    },
    insertData(pram) {
      this.$http.post(this.add_url + this.selectedTable, pram).then((res) => {
        if (res.status === 200 && res.data.resultObject) {
          this.getItemOptions();
          this.getData();
          this.$bvModal.hide("modal-config-add");
        } else {
          this.$fn.notify("server-error", { message: "추가 에러" });
        }
      });
    },
    updataData(pram) {
      this.$http
        .post(this.update_url + this.selectedTable, pram)
        .then((res) => {
          if (res.status === 200 && res.data.resultObject) {
            this.getItemOptions();
            this.getData();
            this.$bvModal.hide("modal-config-modify");
          } else {
            this.$fn.notify("server-error", { message: "추가 에러" });
          }
        });
    },
    async deleteData(rowData) {
      await this.$http
        .delete(this.delete_url + this.selectedTable, { data: rowData })
        .then((res) => {
          if (res.status === 200 && res.data.resultObject) {
            this.$fn.notify("primary", {
              message: "삭제 완료",
            });
            this.getItemOptions();
            this.getData();
          } else {
            this.$fn.notify("server-error", { message: "추가 에러" });
          }
        });
    },
  },
};
</script>
