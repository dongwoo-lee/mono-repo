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
        <piaf-breadcrumb heading="모니터링 설정" :noNav="true" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <b-row>
      <b-colxx>
        <b-card style="height: 80vh">
          <b-container fluid>
            <div
              style="
                width: 84.6vw;
                height: 7vh;
                border: 1px solid #ddd;
                display: flex;
                flex-direction: row;
                align-items: center;
                justify-content: space-between;
                padding-left: 20px;
                padding-right: 20px;
              "
            >
              <div style="display: flex; align-items: baseline">
                <DxTextBox
                  value="192.168.1.236"
                  mode="text"
                  styling-mode="outlined"
                  label="IP"
                />
                <DxButton
                  style="margin-left: 20px"
                  text="장비찾기"
                  type="white"
                  styling-mode="outlined"
                  @click="SearchDevice"
                />
              </div>
              <div>
                <DxButton
                  style="margin-left: 20px"
                  text="중지"
                  type="white"
                  styling-mode="outlined"
                  @click="log"
                />
                <DxButton
                  style="margin-left: 20px"
                  text="재시작"
                  type="white"
                  styling-mode="outlined"
                />
              </div>
            </div>
            <div style="margin-top: 20px">
              <DxDataGrid
                width="100%"
                height="65vh"
                @initialized="onDeviceDataGridInitialized"
                :show-borders="true"
                :data-source="deviceList"
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
                  width="8vw"
                  data-field="device_name"
                  data-type="string"
                  caption="단말명"
                />
                <DxColumn
                  width="4vw"
                  data-field="device_type"
                  data-type="string"
                  caption="장비 유형"
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
                  width="3vw"
                  data-field="watch_service_status_risk"
                  data-type="string"
                  caption="감시 정보"
                />
                <DxColumn
                  css-class="cell-button"
                  width="3vw"
                  caption="정보"
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
                    <DxButton icon="edit" styling-mode="outlined" />
                  </div>
                </template>
              </DxDataGrid>
            </div>
          </b-container>
        </b-card>
      </b-colxx>
    </b-row>
  </div>
</template>

<script>
import {
  DxDataGrid,
  DxColumn,
  DxPaging,
  DxPager,
} from "devextreme-vue/data-grid"
import { DxTextBox } from "devextreme-vue/text-box"
import DxButton from "devextreme-vue/button"
import { DxLoadPanel } from "devextreme-vue/load-panel"
import axios from "axios"

export default {
  components: {
    DxLoadPanel,
    DxDataGrid,
    DxColumn,
    DxPaging,
    DxPager,
    DxTextBox,
    DxButton,
  },
  data() {
    return {
      loadingVisible: false,
      deviceList: [
        {
          device_id: "2C- F0 - 5D - D1 - 0C - B3",
          device_name: "CDS",
          location: "10",
          device_type: 0,
          device_model: "ms - 7879",
          machine_name: "ADSOFT",
          os_version: "WINDOWS11",
          processor_info: "I7 - 10400",
          ip_info: "192.168.1.236",
          watch_service_status_risk: "Y",
        },
      ],
      deviceDataGrid: null,
    }
  },
  methods: {
    async SearchDevice() {
      this.loadingVisible = true
      var res = await axios.get(`/mntr/Monitoring/SearchDevice`)
      this.deviceList = res.data
      this.deviceDataGrid.refresh()
      this.loadingVisible = false
    },
    log() {
      this.loadingVisible = true
      setTimeout(() => {
        this.loadingVisible = false
      }, 1000)
    },
    onDeviceDataGridInitialized(e) {
      this.deviceDataGrid = e.component
    },
  },
}
</script>

<style lang="scss" scoped></style>
