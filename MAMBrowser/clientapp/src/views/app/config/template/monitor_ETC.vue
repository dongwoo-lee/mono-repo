<template>
  <div class="monitor_container">
    <h3>DL3</h3>
    <br v-if="DL3DataSource.length == 0" />
    <h6
      v-if="DL3DataSource.length == 0"
      style="text-align: center"
    >장비 없음</h6>
    <br v-if="DL3DataSource.length == 0" />
    <div v-for="index in Math.ceil(DL3DataSource.length / itemCount)">
      <div class="monitor_group">
        <b-button
          v-for="i in itemCount"
          class="monitor_item"
          @click="
            toggleCollapse(
              $event,
              'collapse-' + index,
              i,
              DL3DataSource[itemCount * (index - 1) + (i - 1)],
              'DL3',
            )
            "
          v-if="DL3DataSource[itemCount * (index - 1) + (i - 1)]"
        >
          <div
            class="btn_header"
            :class="getDL3StatusHeaderColorClass(
              DL3DataSource[0]?.signalR_Info?.agent_status,
            )
              "
          >
            <span
              class="floor"
              :class="getDL3StatusFloorColorClass(
                DL3DataSource[0]?.signalR_Info?.agent_status,
              )
                "
            >
              {{ DL3DataSource[0]?.deviceInfo.location }}
            </span>
            <span class="name">
              {{ DL3DataSource[0]?.deviceInfo.alias_name }}
            </span>
          </div>
          <div class="btn_body">
            {{
              DL3DataSource[itemCount * (index - 1) + (i - 1)]?.agentInfo
                ?.watchProcessName
              ? DL3DataSource[itemCount * (index - 1) + (i - 1)]?.agentInfo
                ?.watchProcessName + " 감시 중"
              : "감시 프로세스 없음"
            }}
          </div>
        </b-button>
      </div>
      <div>
        <b-collapse
          :id="'collapse-' + index"
          class="collapse_detail"
        >
          <b-card
            class="detail_body"
            v-if="rowIndex && DL3DataSource[0]"
          >
            <dl class="group_content">
              <dt class="content_title">단말 모델명 :</dt>
              <dd class="content_text">
                {{ DL3DataSource[0]?.deviceInfo.device_model }}
              </dd>
              <dt class="content_title">단말 컴퓨터 이름 :</dt>
              <dd class="content_text">
                {{ DL3DataSource[0]?.deviceInfo.machine_name }}
              </dd>
              <dt class="content_title">윈도우 버전 :</dt>
              <dd class="content_text">
                {{ DL3DataSource[0]?.deviceInfo.os_version }}
              </dd>
              <dt class="content_title">프로세서 정보 :</dt>
              <dd class="content_text">
                {{ DL3DataSource[0]?.deviceInfo.processor_info }}
              </dd>
              <dt class="content_title">IP정보 :</dt>
              <dd class="content_text">
                {{ DL3DataSource[0]?.deviceInfo.ip_info }}
              </dd>
              <dt class="content_title">cpu 사용률 :</dt>
              <dd class="content_text">
                {{ DL3DataSource[0]?.healthPacket.resource.cpu }}
              </dd>
              <dt class="content_title">메모리 사용률 :</dt>
              <dd class="content_text">
                {{ DL3DataSource[0]?.healthPacket.resource.memory }}
              </dd>
              <dt class="content_title">디스크 사용률 :</dt>
              <dd class="content_text">
                {{ DL3DataSource[0]?.healthPacket.resource.disk }}
              </dd>
              <dt class="content_title">디스크 읽기/쓰기</dt>
              <dd class="content_text">
                {{ DL3DataSource[0]?.healthPacket.resource.disk_io_use_rate }}
              </dd>
              <dt class="content_title">네트워크 사용률 :</dt>
              <dd class="content_text">
                {{ DL3DataSource[0]?.healthPacket.resource.network_use_rate }}
              </dd>
            </dl>
            <dl class="group_content">
              <dt class="content_title">메인 오디오 서버 상태 :</dt>
              <dd class="content_text">
                {{
                  DL3DataSource[0]?.agentInfo?.dL3_INFO.audioServerMainStatus
                  ? "켜짐"
                  : "꺼짐"
                }}
              </dd>
              <dt class="content_title">서브 오디오 서버 상태 :</dt>
              <dd class="content_text">
                {{
                  DL3DataSource[0]?.agentInfo?.dL3_INFO.audioServerSubStatus
                  ? "켜짐"
                  : "꺼짐"
                }}
              </dd>
              <dt class="content_title">메인 파일 에이전트 상태 :</dt>
              <dd class="content_text">
                {{
                  DL3DataSource[0]?.agentInfo?.dL3_INFO.fileAgentMainStatus
                  ? "켜짐"
                  : "꺼짐"
                }}
              </dd>
              <dt class="content_title">서브 파일 에이전트 상태 :</dt>
              <dd class="content_text">
                {{
                  DL3DataSource[0]?.agentInfo?.dL3_INFO.fileAgentSubStatus
                  ? "켜짐"
                  : "꺼짐"
                }}
              </dd>
              <!-- <dt class="content_title">에이전트 상태 :</dt>
              <dd class="content_text">
                {{
                  DL3DataSource[0]
                    ?.signalR_Info?.agent_status
                }}
              </dd>
              <dt class="content_title">감시 프로세스 상태 :</dt>
              <dd class="content_text">
                {{
                  DL3DataSource[0]
                    ?.signalR_Info?.watch_service_status
                }}
              </dd> -->
            </dl>
          </b-card>
        </b-collapse>
      </div>
    </div>

    <hr />
    <h3>일반</h3>
    <br v-if="etcDataSource.length == 0" />
    <h6
      v-if="etcDataSource.length == 0"
      style="text-align: center"
    >장비 없음</h6>
    <br v-if="etcDataSource.length == 0" />
    <div v-for="index in Math.ceil(etcDataSource.length / itemCount)">
      <div class="monitor_group2">
        <b-button
          v-for="i in itemCount"
          class="monitor_item2"
          @click="
            toggleCollapse(
              $event,
              'collapse2-' + index,
              i,
              etcDataSource[itemCount * (index - 1) + (i - 1)],
              'etc',
            )
            "
          v-if="etcDataSource[itemCount * (index - 1) + (i - 1)]"
        >
          <div
            class="btn_header2"
            :class="getEtcStatusHeaderColorClass(
              etcDataSource[itemCount * (index - 1) + (i - 1)]?.signalR_Info
                ?.agent_status &&
              etcDataSource[itemCount * (index - 1) + (i - 1)]?.signalR_Info
                ?.watch_service_status,
            )
              "
          >
            <span
              class="floor2"
              :class="getEtcStatusFloorColorClass(
                etcDataSource[itemCount * (index - 1) + (i - 1)]?.signalR_Info
                  ?.agent_status &&
                etcDataSource[itemCount * (index - 1) + (i - 1)]
                  ?.signalR_Info?.watch_service_status,
              )
                "
            >
              {{
                etcDataSource[itemCount * (index - 1) + (i - 1)]?.deviceInfo
                  .location
              }}F
            </span>
            <span class="name2">
              {{
                etcDataSource[itemCount * (index - 1) + (i - 1)]?.deviceInfo
                  .alias_name
              }}
            </span>
          </div>
          <div class="btn_body2">
            {{
              etcDataSource[itemCount * (index - 1) + (i - 1)]?.agentInfo
                ?.watchProcessName
              ? etcDataSource[itemCount * (index - 1) + (i - 1)]?.agentInfo
                ?.watchProcessName + " 감시 중"
              : "감시 프로세스 없음"
            }}
          </div>
        </b-button>
      </div>
      <div>
        <b-collapse
          :id="'collapse2-' + index"
          class="collapse_detail2"
        >
          <b-card
            class="detail_body2"
            v-if="rowIndex &&
              etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
              "
          >
            <dl class="group_content2">
              <dt class="content_title2">단말 모델명 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.deviceInfo.device_model
                }}
              </dd>
              <dt class="content_title2">단말 컴퓨터 이름 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.deviceInfo.machine_name
                }}
              </dd>
              <dt class="content_title2">윈도우 버전 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.deviceInfo.os_version
                }}
              </dd>
              <dt class="content_title2">프로세서 정보 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.deviceInfo.processor_info
                }}
              </dd>
              <dt class="content_title2">IP정보 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.deviceInfo.ip_info
                }}
              </dd>
              <dt class="content_title2">cpu 사용률 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.healthPacket.resource.cpu
                }}
              </dd>
              <dt class="content_title2">메모리 사용률 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.healthPacket.resource.memory
                }}
              </dd>
              <dt class="content_title2">디스크 사용률 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.healthPacket.resource.disk
                }}
              </dd>
              <dt class="content_title2">디스크 읽기/쓰기</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.healthPacket.resource.disk_io_use_rate
                }}
              </dd>
              <dt class="content_title2">네트워크 사용률 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.healthPacket.resource.network_use_rate
                }}
              </dd>
            </dl>
            <!-- <dl class="group_content2">
              <dt class="content_title2">스튜디오명 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.agentInfo?.slap_info.studio_name
                }}
              </dd>
              <dt class="content_title2">SLAP이름 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.agentInfo?.slap_info.slap_name
                }}
              </dd>
              <dt class="content_title2">큐시트이름 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .signalR_Info.cuesheet_name
                }}
              </dd>
              <dt class="content_title2">로그인 사용자 이름 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .signalR_Info.user_name
                }}
              </dd>
              <dt class="content_title2">에이전트 상태 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.signalR_Info?.agent_status
                }}
              </dd>
              <dt class="content_title2">감시 프로세스 상태 :</dt>
              <dd class="content_text2">
                {{
                  etcDataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.signalR_Info?.watch_service_status
                }}
              </dd>
            </dl> -->
          </b-card>
        </b-collapse>
      </div>
    </div>
  </div>
</template>
<script>
import { mapState } from "vuex";
import axios from "axios";

export default {
  data() {
    return {
      monitoringServerInfo: "",
      itemCount: 6,
      detailItem: {},
      collapseStates: [],
      rowData: {},
      rowIndex: null,
      etcDataSource: [],
      DL3DataSource: [],
    };
  },
  computed: {
    ...mapState("monitoring", {
      connection: (state) => state.connection,
    }),
  },
  async created() {
    await this.GetMonitoringServerInfo();
    await this.GetAllActiveDeviceInfo();
    await this.connectSignalR();
  },
  async beforeDestroy() {
    await this.disconnectSignalR();
    this.stopGetDevicePolling();
    this.etcDataSource = [];
  },
  methods: {
    async GetMonitoringServerInfo() {

      try {

        var res = await axios.get(`/api/GetMonitoringServerInfo`);
      } catch (err) {
        this.$fn.notify("error", { title: err.message });
      }
      this.monitoringServerInfo = await res.data.ResultObject;
    },
    async GetAllActiveDeviceInfo() {
      if (this.monitoringServerInfo == "") {
        await this.GetMonitoringServerInfo();
      }
      try {

        var res = await axios.get(
          `http://${this.monitoringServerInfo
          }/mntr/Monitoring/GetAllActiveDeviceInfoByType?deviceType=${2}`,
          null,
        );
      } catch (err) {
        this.$fn.notify("error", { title: err.message });
      }

      this.DL3DataSource = await res.data;
      try {

        var res = await axios.get(
          `http://${this.monitoringServerInfo
          }/mntr/Monitoring/GetAllActiveDeviceInfoByType?deviceType=${4}`,
          null,
        );
      } catch (err) {
        this.$fn.notify("error", { title: err.message });
      }
      this.etcDataSource = await res.data;
    },
    async GetActiveAgentInfoById(deviceID, type) {
      if (this.monitoringServerInfo == "") {
        await this.GetMonitoringServerInfo();
      }
      try {

        var res = await axios.get(
          `http://${this.monitoringServerInfo}/mntr/Monitoring/GetMonitoringInfoById?deviceId=${deviceID}`,
          null,
        );
      } catch (err) {
        this.$fn.notify("error", { title: err.message });
      }
      if (type === "DL3") {
        this.DL3DataSource[0].healthPacket.resource =
          res.data.healthPacket.resource;
        this.DL3DataSource[0].agentInfo.dL3_INFO = res.data.agentInfo.dL3_INFO;
      } else if (type == "etc") {
        let device = this.etcDataSource.find(
          (d) => d.deviceInfo.device_id == deviceID,
        );
        device.healthPacket.resource = await res.data.healthPacket.resource;
      }
    },
    async startGetDevicePolling(deviceID, type) {
      const pollingInterval = 1000;
      this.GetActiveAgentInfoById(deviceID, type);
      this.pollingTimer = setInterval(() => {
        this.GetActiveAgentInfoById(deviceID, type);
      }, pollingInterval);
    },
    stopGetDevicePolling() {
      clearInterval(this.pollingTimer);
    },
    connectSignalR() {
      this.connection.on("SIGNALRINFO", (status) => {
        var object = JSON.parse(status);
        if (object.DEVICE_TYPE == "2") {
          const device = this.DL3DataSource.find(
            (d) => d.deviceInfo.device_id == object.DEVICE_ID,
          );
          if (device == undefined) {
            return;
          }
          device.signalR_Info.agent_status = object.AGENT_STATUS;
          device.signalR_Info.watch_service_status = object.WATCH_SERVICE_STATUS;
          device.signalR_Info.slap_type = object.SLAP_TYPE;
          device.signalR_Info.user_name = object.USER_NAME;
          device.signalR_Info.cuesheet_name = object.CUESHEET_NAME;
        } else if ((object.DEVICE_TYPE = "4")) {
          const device = this.etcDataSource.find(
            (d) => d.deviceInfo.device_id == object.DEVICE_ID,
          );
          if (device == undefined) {
            return;
          }
          device.signalR_Info.agent_status = object.AGENT_STATUS;
          device.signalR_Info.watch_service_status = object.WATCH_SERVICE_STATUS;
          device.signalR_Info.slap_type = object.SLAP_TYPE;
          device.signalR_Info.user_name = object.USER_NAME;
          device.signalR_Info.cuesheet_name = object.CUESHEET_NAME;
        }
      });
      this.connection.onreconnecting((error) => {
        console.info("onreconnecting", error);
      });
      this.connection.onreconnected((connectionId) => {
        console.info("onreconnected", connectionId);
      });
    },
    async disconnectSignalR() {
      await this.connection.stop();
      await this.connection.off("SIGNALRINFO");
    },
    toggleCollapse(event, collapseId, colIndex, rowItem, type) {
      const elementsWithSpecificClass = document.querySelectorAll(
        ".highlight_border",
      );
      const clickedButtonElement = event.target;

      let monitorItemElement = null;
      if (type == "DL3") {
        monitorItemElement = clickedButtonElement.closest(".monitor_item");
      } else {
        monitorItemElement = clickedButtonElement.closest(".monitor_item2");
      }

      // 특정 클래스를 추가하거나 제거하기
      if (monitorItemElement) {
        if (!monitorItemElement.classList.contains("highlight_border")) {
          elementsWithSpecificClass.forEach((element) => {
            element.classList.remove("highlight_border");
          });
          // 특정 클래스가 없으면 추가
          monitorItemElement.classList.add("highlight_border");
        } else {
          // 특정 클래스가 있으면 제거
          monitorItemElement.classList.remove("highlight_border");
        }
      }

      if (this.collapseStates.length > 0) {
        this.stopGetDevicePolling();
        //이미 열린 탭 있을 때
        const colItemId = Object.keys(this.collapseStates[0])[0];
        if (colItemId === collapseId) {
          //같은 라인의 탭 눌렀을 때
          if (this.collapseStates[0][colItemId] === colIndex) {
            //같은 btn 눌렀을 때
            this.clearColArray(colItemId);
          } else {
            //다른 btn 눌렀을 때
            this.startGetDevicePolling(rowItem.deviceInfo.device_id, type);
            this.collapseStates[0][colItemId] = colIndex;
          }
        } else {
          // 다른 라인 탭 눌렀을 때
          this.clearColArray(colItemId);
          this.startGetDevicePolling(rowItem.deviceInfo.device_id, type);
          this.setColArray(collapseId, colIndex);
        }
      } else {
        //열린 탭 아무것도 없을 때
        this.startGetDevicePolling(rowItem.deviceInfo.device_id, type);
        this.setColArray(collapseId, colIndex);
      }
      // 선택한 item -> detail에 출력
      this.rowIndex = colIndex;
    },
    setColArray(colId, index) {
      if (colId && index) {
        this.toggleEvent(colId);
        this.collapseStates.push({ [colId]: index });
      }
    },
    clearColArray(colId) {
      this.toggleEvent(colId);
      this.collapseStates = [];
    },
    toggleEvent(colId) {
      this.$root.$emit("bv::toggle::collapse", colId);
    },
    getDL3StatusHeaderColorClass(status) {
      if (status) {
        return "status-online-header";
      } else {
        return "status-error-header";
      }
    },
    getDL3StatusFloorColorClass(status) {
      if (status) {
        return "status-online-floor";
      } else {
        return "status-error-floor";
      }
    },
    getEtcStatusHeaderColorClass(status) {
      if (status) {
        return "status-online-header2";
      } else {
        return "status-error-header2";
      }
    },
    getEtcStatusFloorColorClass(status) {
      if (status) {
        return "status-online-floor2";
      } else {
        return "status-error-floor2";
      }
    },
    getStatusTextColorClass(status, agentKey) {
      if (agentKey === "Status") {
        if (status === "Online") {
          return "status-online-color";
        } else if (status === "Offline") {
          return "status-offline-color";
        } else if (status === "Error") {
          return "status-error-color";
        }
      }
    },
    getDeviceName(name) {
      switch (name) {
        case 0:
          return "M";
        case 1:
          return "S";
        case 2:
          return "B";
        default:
          break;
      }
    },
  },
};
</script>
<style>
.monitor_container {
  height: 650px;
  overflow: auto;
}

.monitor_container .monitor_group {
  display: grid;
  grid-template-columns: repeat(6, 1fr);
}

.monitor_container .monitor_item {
  padding: 0;
  margin: 10px 10px 10px 10px;
  border: solid silver 1px;
}

.monitor_container .collapse_detail {
  margin: 0px 10px;
  padding: 10px;
  /* box-shadow: rgba(3, 102, 214, 0.3) 0px 0px 0px 3px; */
  border-radius: 3px;
  /* border: solid silver 1.5px; */
  box-shadow: rgba(3, 102, 214, 0.3) 0px 0px 5px 3px !important;
  /* background-color: #f8f9fa; */
}

.collapse_detail .card-body {
  padding: 1rem;
}

.monitor_container .detail_body {
  height: 300px;
  overflow: auto;
  background-color: #f8f9fa;
}

.monitor_container .btn_header {
  position: relative;
  /* background-color: #008eca !important; */
  padding: 5px;
}

.monitor_container .floor {
  position: absolute;
  top: 0;
  left: 0;
  height: 100%;
  padding: 5px;
  /* background-color: #44546a; */
}

.monitor_container .device {
  position: absolute;
  top: 5px;
  right: 5px;
  padding: 0px 5px 0px 5px;
  background-color: white;
  border-radius: 3px;
  color: black;
}

.monitor_container .btn_body {
  background-color: white;
  color: black;
  padding: 5px;
}

.monitor_container .monitor_items {
  display: flex;
  flex-wrap: wrap;
  align-items: baseline;
}

.highlight_border {
  /* border-radius: 3px !important; */
  box-shadow: rgba(3, 102, 214, 0.3) 0px 0px 5px 5px !important;
  /* box-shadow: rgba(0, 0, 0, 0.16) 0px 1px 4px, rgb(51, 51, 51) 0px 0px 0px 3px !important; */

  /* box-shadow: silver 0px 0px 0px 1px !important; */
}

.detail_body .group_content {
  float: left;
  width: 45%;
  margin: 0% 1.6%;
  /* border-right: solid silver 1px; */
}

.detail_body .content_title {
  display: inline-block;
  vertical-align: top;
  width: 25%;
  margin-right: 10px;
}

.detail_body .content_text {
  display: inline-block;
  width: 70%;
}

.detail_body dt {
  margin-bottom: 10px;
}

.status-online-header {
  background-color: #008eca !important;
}

.status-offline-header {
  background-color: darkgray !important;
}

.status-error-header {
  background-color: #c43d4b !important;
}

.status-online-floor {
  background-color: #44546a;
}

.status-offline-floor {
  background-color: #5d5959;
}

.status-error-floor {
  background-color: #833c0c;
}

.status-online-color {
  color: blue;
}

.status-offline-color {
  color: gray;
}

.status-error-color {
  color: red;
}

.monitor_container .device_total_count {
  position: absolute;
  bottom: 8px;
  right: 50px;
}

.monitor_container {
  height: 650px;
  overflow: auto;
}

.monitor_container .monitor_group2 {
  display: grid;
  grid-template-columns: repeat(6, 1fr);
}

.monitor_container .monitor_item2 {
  padding: 0;
  margin: 10px 10px 10px 10px;
  border: solid silver 1px;
}

.monitor_container .collapse_detail2 {
  margin: 0px 10px;
  padding: 10px;
  /* box-shadow: rgba(3, 102, 214, 0.3) 0px 0px 0px 3px; */
  border-radius: 3px;
  /* border: solid silver 1.5px; */
  box-shadow: rgba(3, 102, 214, 0.3) 0px 0px 5px 3px !important;
  /* background-color: #f8f9fa; */
}

.collapse_detail2 .card-body {
  padding: 1rem;
}

.monitor_container .detail_body2 {
  height: 300px;
  overflow: auto;
  background-color: #f8f9fa;
}

.monitor_container .btn_header2 {
  position: relative;
  /* background-color: #008eca !important; */
  padding: 5px;
}

.monitor_container .floor2 {
  position: absolute;
  top: 0;
  left: 0;
  height: 100%;
  padding: 5px;
  /* background-color: #44546a; */
}

.monitor_container .device {
  position: absolute;
  top: 5px;
  right: 5px;
  padding: 0px 5px 0px 5px;
  background-color: white;
  border-radius: 3px;
  color: black;
}

.monitor_container .btn_body2 {
  background-color: white;
  color: black;
  padding: 5px;
}

.monitor_container .monitor_items2 {
  display: flex;
  flex-wrap: wrap;
  align-items: baseline;
}

.highlight_border {
  /* border-radius: 3px !important; */
  box-shadow: rgba(3, 102, 214, 0.3) 0px 0px 5px 5px !important;
  /* box-shadow: rgba(0, 0, 0, 0.16) 0px 1px 4px, rgb(51, 51, 51) 0px 0px 0px 3px !important; */

  /* box-shadow: silver 0px 0px 0px 1px !important; */
}

.detail_body2 .group_content2 {
  float: left;
  width: 45%;
  margin: 0% 1.6%;
  /* border-right: solid silver 1px; */
}

.detail_body2 .content_title2 {
  display: inline-block;
  vertical-align: top;
  width: 25%;
  margin-right: 10px;
}

.detail_body2 .content_text2 {
  display: inline-block;
  width: 70%;
}

.detail_body2 dt {
  margin-bottom: 10px;
}

.status-online-header2 {
  background-color: #008eca !important;
}

.status-offline-header2 {
  background-color: darkgray !important;
}

.status-error-header2 {
  background-color: #c43d4b !important;
}

.status-online-floor2 {
  background-color: #44546a;
}

.status-offline-floor2 {
  background-color: #5d5959;
}

.status-error-floor2 {
  background-color: #833c0c;
}

.status-online-color2 {
  color: blue;
}

.status-offline-color2 {
  color: gray;
}

.status-error-color2 {
  color: red;
}

.monitor_container .device_total_count {
  position: absolute;
  bottom: 8px;
  right: 50px;
}</style>
