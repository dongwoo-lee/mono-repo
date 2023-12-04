<template>
  <div id="wrapper">
    <DxLoadPanel
      position="#wrapper"
      :visible="loadingVisible"
      :show-indicator="true"
      :show-pane="true"
      :shading="true"
      :hide-on-outside-click="false"
      shading-color="rgba(0,0,0,0.4)"
    />
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb
          heading="모니터링 설정"
          :noNav="true"
        />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <b-row>
      <b-colxx>
        <b-card style="height: 80vh">
          <b-container fluid>
            <div style="
                width: 84.6vw;
                height: 7vh;
                border: 1px solid #ddd;
                display: flex;
                flex-direction: row;
                align-items: center;
                justify-content: space-between;
                padding-left: 20px;
                padding-right: 20px;
              ">
              <div style="display: flex; align-items: baseline">
                <DxButton
                  text="장비등록"
                  type="white"
                  styling-mode="outlined"
                  @click="SearchDevice"
                />
              </div>
            </div>
            <div style="margin-top: 20px">
              <DxDataGrid
                width="100%"
                height="65vh"
                @initialized="onActiveDeviceDataGridInitialized"
                :show-borders="true"
                :data-source="activeDevice"
                :allow-column-resizing="true"
              >
                <DxPaging :page-size="20" />
                <DxPager
                  :allowed-page-sizes="[20]"
                  :show-page-size-selector="false"
                  :show-info="true"
                  :visible="true"
                  info-text="전체 {2}개"
                />
                <!--<DxScrolling mode='standard' row-rendering-mode='standard' />
              <DxColumnFixing :enabled='true' />
              <DxHeaderFilter :visible='true' />
              <DxFilterRow :visible='true' apply-filter='auto' />
              <DxSelection mode='multiple' show-check-boxes-mode='always' select-all-mode='allPages' /> -->
                <DxColumn
                  width="8vw"
                  data-field="alias_name"
                  data-type="string"
                  caption="장비명"
                />

                <DxColumn
                  width="10vw"
                  data-field="device_id"
                  data-type="string"
                  caption="장비ID"
                />
                <DxColumn
                  width="12vw"
                  data-field="device_model"
                  data-type="string"
                  caption="모델명"
                />

                <DxColumn
                  width="3vw"
                  data-field="device_type"
                  data-type="string"
                  caption="유형"
                />
                <DxColumn
                  width="6vw"
                  data-field="ip_info"
                  data-type="string"
                  caption="IP 정보"
                />
                <DxColumn
                  width="3vw"
                  data-field="location"
                  data-type="string"
                  caption="위치"
                />
                <DxColumn
                  width="8vw"
                  data-field="machine_name"
                  data-type="string"
                  caption="컴퓨터명"
                />
                <DxColumn
                  width="14vw"
                  data-field="os_version"
                  data-type="string"
                  caption="OS정보"
                />
                <DxColumn
                  width="12vw"
                  data-field="processor_info"
                  data-type="string"
                  caption="프로세서 정보"
                />
                <DxColumn
                  width="4vw"
                  data-field="agent_code"
                  data-type="string"
                  caption="장비 코드"
                />
                <DxColumn
                  css-class="cell-button"
                  width="3vw"
                  caption="액션"
                  cell-template="infoTemplate"
                />

                <template #infoTemplate="{ data: rowInfo }">
                  <div style="
                      display: flex;
                      justify-content: space-around;
                      vertical-align: middle;
                    ">
                    <DxButton
                      icon="edit"
                      styling-mode="outlined"
                      @click="editPopupOn(rowInfo.data)"
                    />
                    <DxButton
                      icon="trash"
                      styling-mode="outlined"
                      @click="removePopupOn(rowInfo.data)"
                    />
                  </div>
                </template>
              </DxDataGrid>
            </div>
          </b-container>
        </b-card>
      </b-colxx>
    </b-row>
    <DxPopup
      @initialized="onEditPopupInit($event)"
      :visible="searchVisible"
      :drag-enabled="false"
      :show-close-button="false"
      :show-title="true"
      width="90vw"
      height="80vh"
      container="#wrapper"
      title="장비 등록"
    >
      <DxDataGrid
        width="100%"
        height="65vh"
        @initialized="onInactiveDeviceDataGridInitialized"
        key-expr="device_id"
        :show-borders="true"
        :data-source="inactiveDevice"
        :allow-column-resizing="true"
        :onSelectionChanged="onSelectionChanged"
      >
        <DxPaging :page-size="20" />
        <DxPager
          :allowed-page-sizes="[20]"
          :show-page-size-selector="false"
          :show-info="true"
          :visible="true"
          info-text="전체 {2}개"
        />
        <!--<DxScrolling mode='standard' row-rendering-mode='standard' />
              <DxColumnFixing :enabled='true' />
              <DxHeaderFilter :visible='true' />
              <DxFilterRow :visible='true' apply-filter='auto' />
               -->
        <DxSelection
          mode="multiple"
          show-check-boxes-mode="always"
          select-all-mode="allPages"
        />
        <DxColumn
          width="8vw"
          data-field="alias_name"
          data-type="string"
          caption="장비명"
        />

        <DxColumn
          width="10vw"
          data-field="device_id"
          data-type="string"
          caption="장비ID"
        />
        <DxColumn
          width="12vw"
          data-field="device_model"
          data-type="string"
          caption="모델명"
        />

        <DxColumn
          width="3vw"
          data-field="device_type"
          data-type="string"
          caption="유형"
        />
        <DxColumn
          width="6vw"
          data-field="ip_info"
          data-type="string"
          caption="IP 정보"
        />
        <DxColumn
          width="3vw"
          data-field="location"
          data-type="string"
          caption="위치"
        />
        <DxColumn
          width="8vw"
          data-field="machine_name"
          data-type="string"
          caption="컴퓨터명"
        />
        <DxColumn
          width="14vw"
          data-field="os_version"
          data-type="string"
          caption="OS정보"
        />
        <DxColumn
          width="12vw"
          data-field="processor_info"
          data-type="string"
          caption="프로세서 정보"
        />
        <DxColumn
          width="4vw"
          data-field="agent_code"
          data-type="string"
          caption="장비 코드"
        />
      </DxDataGrid>
      <div>
        <DxButton
          text="등록"
          type="default"
          styling-mode="outlined"
          @click="ActivateDevice"
        />
        <DxButton
          text="닫기"
          type="default"
          styling-mode="outlined"
          @click="searchVisible = false"
        />
      </div>
    </DxPopup>

    <DxPopup
      @initialized="onEditPopupInit($event)"
      :visible="editVisible"
      :drag-enabled="false"
      :show-close-button="false"
      :show-title="true"
      width="30vw"
      height="60vh"
      container="#wrapper"
      title="장비 편집"
    >
      <template #content>
        <DxScrollView
          width="100%"
          height="100%"
        >
          <div style="
              width: 100%;
              height: 42vh;
              display: flex;
              flex-direction: column;
              justify-content: space-between;
            ">
            <DxTextBox
              v-model:value="editData.alias_name"
              mode="text"
              styling-mode="outlined"
              label="단말명"
            />
            <DxTextBox
              :value="editData.device_id"
              mode="text"
              styling-mode="outlined"
              label="장비ID"
              :read-only="true"
            />
            <DxTextBox
              :value="editData.device_model"
              mode="text"
              styling-mode="outlined"
              label="모델명"
              :read-only="true"
            />

            <DxNumberBox
              :value="editData.device_type"
              styling-mode="outlined"
              label="유형"
              :read-only="true"
            />
            <DxTextBox
              v-model:value="editData.ip_info"
              mode="text"
              styling-mode="outlined"
              label="IP 정보"
            />
            <DxTextBox
              v-model:value="editData.location"
              mode="text"
              styling-mode="outlined"
              label="위치"
            />
            <DxTextBox
              :value="editData.machine_name"
              mode="text"
              styling-mode="outlined"
              label="컴퓨터명"
              :read-only="true"
            />
            <DxTextBox
              :value="editData.os_version"
              mode="text"
              styling-mode="outlined"
              label="OS정보"
              :read-only="true"
            />
            <DxTextBox
              :value="editData.processor_info"
              mode="text"
              styling-mode="outlined"
              label="프로세서 정보"
              :read-only="true"
            />
            <DxTextBox
              :value="editData.agent_code"
              mode="text"
              styling-mode="outlined"
              label="장비 코드"
              :read-only="true"
            />
          </div>
        </DxScrollView>
      </template>
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="before"
        :options="editButtonOptions"
      />
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="editCloseButtonOptions"
      />
    </DxPopup>

    <DxPopup
      @initialized="onRemovePopupInit($event)"
      :visible="removeVisible"
      :drag-enabled="false"
      :show-close-button="false"
      :show-title="true"
      width="20vw"
      height="25vh"
      container="#wrapper"
      title="장비 제거"
    >
      <div>장비 ID : {{ this.removeData.device_id }}</div>
      <div>단말명 : {{ this.removeData.device_name }}</div>
      <br />
      장비를 제거하시겠습니까?
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="before"
        :options="removeButtonOptions"
      />
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="removeCloseButtonOptions"
      />
    </DxPopup>
  </div>
</template>

<script>
import {
  DxDataGrid,
  DxColumn,
  DxPaging,
  DxPager,
  DxSelection,
} from "devextreme-vue/data-grid";
import { DxTextBox } from "devextreme-vue/text-box";
import { DxNumberBox } from "devextreme-vue/number-box";
import DxButton from "devextreme-vue/button";
import { DxLoadPanel } from "devextreme-vue/load-panel";
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import { DxScrollView } from "devextreme-vue/scroll-view";
import axios from "axios";

export default {
  components: {
    DxDataGrid,
    DxColumn,
    DxPaging,
    DxPager,
    DxSelection,
    DxTextBox,
    DxNumberBox,
    DxButton,
    DxLoadPanel,
    DxPopup,
    DxToolbarItem,
    DxScrollView,
  },
  data() {
    return {
      loadingVisible: false,
      activeDevice: [],
      inactiveDevice: [],
      selectedDevice: [],
      activeDataGrid: null,
      inactiveDataGrid: null,
      searchVisible: false,
      editVisible: false,
      editData: {},
      editButtonOptions: {
        text: "저장",
        onClick: () => {
          this.UpdateDevice();
          this.editVisible = false;
        },
      },
      editCloseButtonOptions: {
        text: "닫기",
        onClick: () => {
          this.editVisible = false;
        },
      },
      editPopup: null,
      removeVisible: false,
      removeData: {},
      removeButtonOptions: {
        text: "제거",
        onClick: () => {
          this.RemoveDevice();
          this.removeVisible = false;
        },
      },
      removeCloseButtonOptions: {
        text: "닫기",
        onClick: () => {
          this.removeVisible = false;
        },
      },
      removePopup: null,
    };
  },
  async created() {
    this.GetDevice();
  },
  methods: {
    async GetDevice() {
      this.loadingVisible = true;
      var res = await axios.get(`/mntr/Monitoring/GetDevice`);
      this.activeDevice = res.data;
      this.activeDataGrid.refresh();
      this.loadingVisible = false;
    },
    async SearchDevice() {
      this.loadingVisible = true;
      var res = await axios.get(`/mntr/Monitoring/SearchDevice`);
      console.log("SearchDevice :>> ", res);
      this.inactiveDevice = res.data;
      this.loadingVisible = false;
      this.searchVisible = true;
    },
    async ActivateDevice() {
      this.loadingVisible = true;
      var param = {
        deviceIds: this.selectedDevice,
      };
      var res = await axios.post(`/mntr/Monitoring/ActivateDevice`, param);
      this.searchVisible = false;
      this.loadingVisible = false;
      this.inactiveDataGrid.deselectAll();
      this.GetDevice();
    },
    async UpdateDevice() {
      this.loadingVisible = true;
      var param = {
        device_id: this.editData.device_id,
        alias_name: this.editData.alias_name,
      };
      var res = await axios.patch(`/mntr/Monitoring/UpdateDevice`, param);
      console.log("UpdateDevice :>> ", res);
      this.GetDevice();
      this.loadingVisible = false;
    },
    async RemoveDevice() {
      this.loadingVisible = true;
      var res = await axios.delete(`/mntr/Monitoring/RemoveDevice`, {
        data: { device_id: this.removeData.device_id },
      });
      console.log("RemoveDevice :>> ", res);
      this.GetDevice();
      this.loadingVisible = false;
    },
    onActiveDeviceDataGridInitialized(e) {
      this.activeDataGrid = e.component;
    },
    onInactiveDeviceDataGridInitialized(e) {
      this.inactiveDataGrid = e.component;
    },
    onSelectionChanged(e) {
      this.selectedDevice = e.selectedRowKeys;
    },
    editPopupOn(data) {
      this.editVisible = true;
      this.editData = data;
    },

    onEditPopupInit(e) {
      this.editPopup = e.component;

      this.editPopup.registerKeyHandler("escape", function (arg) {
        arg.stopPropagation();
      });
    },
    removePopupOn(data) {
      this.removeVisible = true;
      this.removeData = data;
    },
    onRemovePopupInit(e) {
      this.removePopup = e.component;

      this.removePopup.registerKeyHandler("escape", function (arg) {
        arg.stopPropagation();
      });
    },
  },
};
</script>

<style lang="scss" scoped></style>
