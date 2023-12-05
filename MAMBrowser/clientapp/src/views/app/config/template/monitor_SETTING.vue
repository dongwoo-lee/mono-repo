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

    <!-- <div style="
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
        
      </div>
    </div> -->
    <div style="margin-top: 20px; font-family: 'MBC 새로움 M'">
      <DxDataGrid
        style="font-family: 'MBC 새로움 M'"
        width="100%"
        height="60vh"
        @initialized="onActiveDeviceDataGridInitialized"
        @cellPrepared="onActiveCellPrepared"
        :show-borders="true"
        :data-source="activeDevice"
        :allow-column-resizing="true"
      >
        <DxToolbar>
          <DxItem location="after" template="searchDeviceTemplate" />
        </DxToolbar>
        <template #searchDeviceTemplate>
          <DxButton
            style="font-family: 'MBC 새로움 M'"
            icon="add"
            text="장비등록"
            type="white"
            styling-mode="outlined"
            @click="SearchDevice"
          />
        </template>
        <DxPaging :page-size="20" />
        <DxPager
          style="font-family: 'MBC 새로움 M'"
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
          cell-template="typeTemplate"
        />
        <template #typeTemplate="{ data: rowInfo }">
          {{ getType(rowInfo.data.device_type) }}
        </template>
        <DxColumn
          width="7vw"
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
          width="12vw"
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
          width="4vw"
          caption="액션"
          cell-template="infoTemplate"
        />

        <template #infoTemplate="{ data: rowInfo }">
          <div
            style="
              display: flex;
              justify-content: space-around;
              vertical-align: middle;
            "
          >
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
    <DxPopup
      style="font-family: 'MBC 새로움 M'"
      @initialized="onEditPopupInit($event)"
      :visible="activeDevicePopupVisible"
      :drag-enabled="false"
      :show-close-button="false"
      :show-title="true"
      width="90vw"
      height="80vh"
      container="#wrapper"
      title="장비 등록"
    >
      <DxDataGrid
        style="font-family: 'MBC 새로움 M'"
        width="100%"
        height="65vh"
        @initialized="onInactiveDeviceDataGridInitialized"
        @cellPrepared="onInactiveCellPrepared"
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
        <DxToolbar>
          <DxItem location="after" template="refreshDeviceTemplate" />
        </DxToolbar>
        <template #refreshDeviceTemplate>
          <DxButton
            icon="refresh"
            text="재검색"
            type="white"
            styling-mode="outlined"
            @click="SearchDeviceApi"
          />
        </template>
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
          cell-template="aliasTemplate"
        />
        <template #aliasTemplate="{ data: rowInfo }">
          <DxTextBox
            v-model:value="rowInfo.data.alias_name"
            mode="text"
            styling-mode="outlined"
          />
        </template>
        <DxColumn
          style="vertical-align: middle"
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
          cell-template="typeTemplate"
        />
        <template #typeTemplate="{ data: rowInfo }">
          {{ getType(rowInfo.data.device_type) }}
        </template>
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
          cell-template="locationTemplate"
        />
        <template #locationTemplate="{ data: rowInfo }">
          <DxTextBox
            v-model:value="rowInfo.data.location"
            mode="text"
            styling-mode="outlined"
          />
        </template>
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
          cell-template="agentCodeTemplate"
        />
        <template #agentCodeTemplate="{ data: rowInfo }">
          <DxTextBox
            v-model:value="rowInfo.data.agent_code"
            mode="text"
            styling-mode="outlined"
          />
        </template>
      </DxDataGrid>
      <div style="display: flex; justify-content: space-between">
        <DxButton
          text="등록"
          type="default"
          :disabled="selectedDevice.length == 0"
          styling-mode="outlined"
          @click="ActivateDevice"
        />
        <DxButton
          text="닫기"
          type="default"
          styling-mode="outlined"
          @click="activeDevicePopupOff"
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
        <DxScrollView width="100%" height="100%">
          <div
            style="
              width: 100%;
              height: 42vh;
              display: flex;
              flex-direction: column;
              justify-content: space-between;
            "
          >
            <DxTextBox
              style="font-family: 'MBC 새로움 M'"
              v-model:value="editData.alias_name"
              mode="text"
              styling-mode="outlined"
              label="장비명"
            />
            <DxTextBox
              :value="editData.device_id"
              mode="text"
              styling-mode="outlined"
              label="장비ID"
              :disabled="true"
            />
            <DxTextBox
              :value="editData.device_model"
              mode="text"
              styling-mode="outlined"
              label="모델명"
              :disabled="true"
            />

            <DxTextBox
              :value="getType(editData.device_type)"
              styling-mode="outlined"
              label="유형"
              :disabled="true"
            />
            <DxTextBox
              v-model:value="editData.ip_info"
              mode="text"
              styling-mode="outlined"
              label="IP 정보"
              :disabled="true"
            />
            <DxTextBox
              v-model:value="editData.location"
              mode="text"
              styling-mode="outlined"
              label="위치"
              :disabled="true"
            />
            <DxTextBox
              :value="editData.machine_name"
              mode="text"
              styling-mode="outlined"
              label="컴퓨터명"
              :disabled="true"
            />
            <DxTextBox
              :value="editData.os_version"
              mode="text"
              styling-mode="outlined"
              label="OS정보"
              :disabled="true"
            />
            <DxTextBox
              :value="editData.processor_info"
              mode="text"
              styling-mode="outlined"
              label="프로세서 정보"
              :disabled="true"
            />
            <DxTextBox
              :value="editData.agent_code"
              mode="text"
              styling-mode="outlined"
              label="장비 코드"
              :disabled="true"
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
      style="font-family: 'MBC 새로움 M'"
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
      <div style="font-family: 'MBC 새로움 M'"
        >장비 ID : {{ this.removeData.device_id }}</div
      >
      <div style="font-family: 'MBC 새로움 M'"
        >장비명 : {{ this.removeData.alias_name }}
      </div>
      <br />
      <span style="font-family: 'MBC 새로움 M'">
        장비를 제거하시겠습니까?
      </span>
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
  DxToolbar,
  DxItem,
  DxPaging,
  DxPager,
  DxSelection,
} from "devextreme-vue/data-grid"
import { DxTextBox } from "devextreme-vue/text-box"
import { DxNumberBox } from "devextreme-vue/number-box"
import DxButton from "devextreme-vue/button"
import { DxLoadPanel } from "devextreme-vue/load-panel"
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup"
import { DxScrollView } from "devextreme-vue/scroll-view"
import MixinBasicPage from "../../../../mixin/MixinBasicPage"
import axios from "axios"
import qs from "qs"

export default {
  components: {
    DxDataGrid,
    DxColumn,
    DxToolbar,
    DxItem,
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
  mixins: [MixinBasicPage],
  data() {
    return {
      monitoringServerInfo: "",
      loadingVisible: false,
      activeDevice: [],
      inactiveDevice: [],
      selectedDevice: [],
      activeDataGrid: null,
      inactiveDataGrid: null,
      activeDevicePopupVisible: false,
      editVisible: false,
      editData: {},
      editButtonOptions: {
        text: "저장",
        onClick: () => {
          this.UpdateDevice()
          this.editVisible = false
        },
      },
      editCloseButtonOptions: {
        text: "닫기",
        onClick: () => {
          this.editVisible = false
        },
      },
      editPopup: null,
      removeVisible: false,
      removeData: {},
      removeButtonOptions: {
        text: "제거",
        onClick: () => {
          this.RemoveDevice()
          this.removeVisible = false
        },
      },
      removeCloseButtonOptions: {
        text: "닫기",
        onClick: () => {
          this.removeVisible = false
        },
      },
      removePopup: null,
    }
  },
  async created() {
    await this.GetMonitoringServerInfo()
    await this.GetDevice()
  },
  methods: {
    async GetMonitoringServerInfo() {
      var res = await axios.get(`/api/GetMonitoringServerInfo`)
      this.monitoringServerInfo = await res.data.ResultObject
    },
    async GetDevice() {
      if (this.monitoringServerInfo == "") {
        await this.GetMonitoringServerInfo()
      }
      this.loadingVisible = true
      var res = await axios.get(
        `http://${this.monitoringServerInfo}/mntr/Monitoring/GetDevice`,
      )
      console.log("GetDevice :>> ", res)
      this.activeDevice = res.data
      this.activeDataGrid.refresh()
      this.loadingVisible = false
    },
    async SearchDevice() {
      this.loadingVisible = true
      await this.SearchDeviceApi()
      this.loadingVisible = false
      this.activeDevicePopupOn()
    },
    async SearchDeviceApi() {
      if (this.monitoringServerInfo == "") {
        await this.GetMonitoringServerInfo()
      }
      var res = await axios.get(
        `http://${this.monitoringServerInfo}/mntr/Monitoring/SearchDevice`,
      )
      console.log("SearchDevice :>> ", res)
      this.inactiveDevice = res.data
      this.inactiveDataGrid.repaint()
    },
    activeDevicePopupOn() {
      this.activeDevicePopupVisible = true
    },
    activeDevicePopupOff() {
      this.activeDevicePopupVisible = false
      this.inactiveDataGrid.deselectAll()
    },
    async ActivateDevice() {
      if (this.monitoringServerInfo == "") {
        await this.GetMonitoringServerInfo()
      }

      if (this.selectedDevice.length == 0) {
        this.$fn.notify("error", { title: "장비를 선택해주세요." })
        return
      }

      var name = 0
      var location = 0
      this.selectedDevice.map((item) => {
        var data = this.inactiveDevice.find((x) => x.device_id == item)
        if (
          data?.alias_name == null ||
          data?.alias_name == undefined ||
          data?.alias_name?.length == 0
        ) {
          name++
        }
        if (
          data?.location == null ||
          data?.location == undefined ||
          data?.location?.length == 0
        ) {
          location++
        }
      })
      if (name > 0) {
        this.$fn.notify("error", { title: "장비명을 입력해주세요." })
        return
      }
      if (location > 0) {
        this.$fn.notify("error", { title: "위치를 입력해주세요." })
        return
      }

      this.loadingVisible = true

      var validAgentCodeParam = []
      this.selectedDevice.map((item) => {
        var matchedItem = this.inactiveDevice.find((x) => x.device_id == item)
        if (
          matchedItem.agent_code != null &&
          matchedItem.agent_code != undefined &&
          matchedItem.agent_code.length > 0
        ) {
          validAgentCodeParam.push(matchedItem.agent_code)
        }
      })

      var res = await axios.get(
        `http://${this.monitoringServerInfo}/mntr/Monitoring/ValidAgentCode`,
        {
          params: validAgentCodeParam,
          paramsSerializer: (param) => {
            return qs.stringify({ param: param }, { arrayFormat: "repeat" })
          },
        },
      )

      if (!res.data["result"]) {
        res.data["deviceIds"].map((item) => {
          this.$fn.notify("error", {
            title: `${item} 장비의 장비 코드가 이미 존재합니다.`,
          })
        })
        this.loadingVisible = false
        return
      }

      var selectedDeviceInfo = []
      this.selectedDevice.map((item) => {
        var matchedItem = this.inactiveDevice.find((x) => x.device_id == item)
        var data = {
          device_id: item,
          alias_name: matchedItem.alias_name,
          location: matchedItem.location,
        }

        selectedDeviceInfo.push(data)
      })

      var res = await axios.post(
        `http://${this.monitoringServerInfo}/mntr/Monitoring/ActivateDevice`,
        selectedDeviceInfo,
      )
      this.activeDevicePopupOff()
      this.loadingVisible = false
      this.inactiveDataGrid.deselectAll()
      this.GetDevice()
    },
    async UpdateDevice() {
      if (this.monitoringServerInfo == "") {
        await this.GetMonitoringServerInfo()
      }
      this.loadingVisible = true
      var param = {
        device_id: this.editData.device_id,
        alias_name: this.editData.alias_name,
      }
      var res = await axios.patch(
        `http://${this.monitoringServerInfo}/mntr/Monitoring/UpdateDevice`,
        param,
      )
      console.log("UpdateDevice :>> ", res)
      this.GetDevice()
      this.loadingVisible = false
    },
    async RemoveDevice() {
      if (this.monitoringServerInfo == "") {
        await this.GetMonitoringServerInfo()
      }
      this.loadingVisible = true
      var res = await axios.delete(
        `http://${this.monitoringServerInfo}/mntr/Monitoring/RemoveDevice`,
        {
          data: { device_id: this.removeData.device_id },
        },
      )
      console.log("RemoveDevice :>> ", res)
      this.GetDevice()
      this.loadingVisible = false
    },
    onActiveDeviceDataGridInitialized(e) {
      this.activeDataGrid = e.component
    },
    onInactiveDeviceDataGridInitialized(e) {
      this.inactiveDataGrid = e.component
    },
    onSelectionChanged(e) {
      this.selectedDevice = e.selectedRowKeys
    },
    editPopupOn(data) {
      this.editVisible = true
      this.editData = JSON.parse(JSON.stringify(data))
    },
    onEditPopupInit(e) {
      this.editPopup = e.component

      this.editPopup.registerKeyHandler("escape", function (arg) {
        arg.stopPropagation()
      })
    },
    removePopupOn(data) {
      this.removeVisible = true
      this.removeData = data
    },
    onRemovePopupInit(e) {
      this.removePopup = e.component

      this.removePopup.registerKeyHandler("escape", function (arg) {
        arg.stopPropagation()
      })
    },
    onActiveCellPrepared(e) {
      if (e.rowType === "header") {
        e.cellElement.style.backgroundColor = "#2a4878"
        e.cellElement.style.color = "white"
      }
    },
    onInactiveCellPrepared(e) {
      if (e.rowType === "header") {
        e.cellElement.style.backgroundColor = "#2a4878"
        e.cellElement.style.color = "white"
      }
      if (e.rowType === "data") {
        e.cellElement.style.verticalAlign = "middle"
      }
    },
    getType(type) {
      if (type == 1) {
        return "SLAP"
      } else if (type == 2) {
        return "DL3"
      } else if (type == 4) {
        return "일반"
      }
    },
  },
}
</script>

<style lang="scss">
.dx-popup-title {
  font-family: "MBC 새로움 M";
}

.dx-button {
  font-family: "MBC 새로움 M";
}

.dx-info {
  font-family: "MBC 새로움 M";
}

.dx-texteditor-input {
  font-family: "MBC 새로움 M" !important;
}
</style>
