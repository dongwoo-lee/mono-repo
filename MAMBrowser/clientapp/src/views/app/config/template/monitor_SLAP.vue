<template>
  <div class="monitor_container">
    <div v-for="index in Math.ceil(dataSource.length / itemCount)">
      <div class="monitor_group">
        <b-button
          v-for="i in itemCount"
          class="monitor_item"
          @click="
            toggleCollapse(
              $event,
              'collapse-' + index,
              i,
              dataSource[itemCount * (index - 1) + (i - 1)],
            )
            "
          v-if="dataSource[itemCount * (index - 1) + (i - 1)]"
        >
          <div
            class="btn_header"
            :class="getStatusHeaderColorClass(
              dataSource[itemCount * (index - 1) + (i - 1)]?.signalR_Info
                ?.agent_status
                ? true
                : false,
            )
              "
          >
            <span
              class="floor"
              :class="getStatusFloorColorClass(
                dataSource[itemCount * (index - 1) + (i - 1)]?.signalR_Info
                  ?.agent_status
                  ? true
                  : false,
              )
                "
            >
              {{
                dataSource[itemCount * (index - 1) + (i - 1)].deviceInfo
                  .location
              }}F
            </span>
            <span class="name">
              {{
                dataSource[itemCount * (index - 1) + (i - 1)].deviceInfo
                  .alias_name
              }}
            </span>
            <span class="device">
              {{
                getDeviceName(
                  dataSource[itemCount * (index - 1) + (i - 1)].agentInfo
                    .agent_type,
                )
              }}
            </span>
          </div>
          <div class="btn_body">
            {{
              dataSource[itemCount * (index - 1) + (i - 1)].agentInfo?.slap_info
                ?.user_name
              ? dataSource[itemCount * (index - 1) + (i - 1)].agentInfo
                ?.slap_info?.user_name + " / "
              : ""
            }}
            {{
              dataSource[itemCount * (index - 1) + (i - 1)].agentInfo?.slap_info
                ?.cuesheet_name
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
            v-if="rowIndex && dataSource[itemCount * (index - 1) + (rowIndex - 1)]
              "
          >
            <dl class="group_content">
              <dt class="content_title">단말 모델명 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .deviceInfo.device_model
                }}
              </dd>
              <dt class="content_title">단말 컴퓨터 이름 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .deviceInfo.machine_name
                }}
              </dd>
              <dt class="content_title">윈도우 버전 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .deviceInfo.os_version
                }}
              </dd>
              <dt class="content_title">프로세서 정보 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .deviceInfo.processor_info
                }}
              </dd>
              <dt class="content_title">IP정보 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .deviceInfo.ip_info
                }}
              </dd>
              <dt class="content_title">cpu 사용률 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .healthPacket.dynamic_data.CPU
                }}
              </dd>
              <dt class="content_title">메모리 사용률 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .healthPacket.dynamic_data.MEMORY
                }}
              </dd>
              <dt class="content_title">디스크 사용률 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .healthPacket.dynamic_data.DISK
                }}
              </dd>
              <dt class="content_title">디스크 읽기/쓰기</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .healthPacket.dynamic_data.DISK_IO_USE_RATE
                }}
              </dd>
              <dt class="content_title">네트워크 사용률 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .healthPacket.dynamic_data.NETWORK_USE_RATE
                }}
              </dd>
            </dl>
            <dl class="group_content">
              <dt class="content_title">스튜디오명 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.agentInfo?.slap_info.studio_name
                }}
              </dd>
              <dt class="content_title">SLAP이름 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.agentInfo?.slap_info.slap_name
                }}
              </dd>
              <dt class="content_title">큐시트이름 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.agentInfo?.slap_info.cuesheet_name
                }}
              </dd>
              <dt class="content_title">로그인 사용자 이름 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.agentInfo?.slap_info?.user_name
                }}
              </dd>
              <dt class="content_title">감시 프로세스 상태 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    ?.healthPacket?.dynamic_data?.WATCH_SERVICE_STATUS
                }}
              </dd>
            </dl>
          </b-card>
        </b-collapse>
      </div>
    </div>
  </div>
</template>
<script>
import * as signalR from "@microsoft/signalr";
import axios from "axios";

export default {
  data() {
    return {
      connection: null,
      itemCount: 6,
      detailItem: {},
      collapseStates: [],
      rowData: {},
      rowIndex: null,
      dataSource: [],
    };
  },
  components: {},
  async created() {
    await this.GetAllActiveDeviceInfo();
    await this.createSignalR();
    await this.connectSignalR();
  },
  async beforeDestroy() {
    await this.disconnectSignalR();
    this.stopGetDevicePolling();
    this.dataSource = [];
  },
  methods: {
    async GetAllActiveDeviceInfo() {
      var res = await axios.get(`/mntr/Monitoring/GetAllActiveDeviceInfo`, null);
      this.dataSource = await res.data;
    },
    async GetActiveAgentInfoById(deviceID) {
      var res = await axios.get(
        `/mntr/Monitoring/GetMonitoringInfoById?deviceId=${deviceID}`,
        null,
      );
      const device = this.dataSource.find(
        (d) => d.deviceInfo.device_id == deviceID,
      );
      device = res.data;
    },
    async startGetDevicePolling(deviceID) {
      const pollingInterval = 5000;
      this.GetActiveAgentInfoById(deviceID);
      this.pollingTimer = setInterval(() => {
        this.GetActiveAgentInfoById(deviceID);
      }, pollingInterval);
    },
    stopGetDevicePolling() {
      clearInterval(this.pollingTimer);
    },
    createSignalR() {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl("/mntr/hub")
        .withAutomaticReconnect({
          nextRetryDelayInMilliseconds: () => {
            return 1000;
          },
        })
        .configureLogging(signalR.LogLevel.Error)
        .build();
    },
    connectSignalR() {
      this.connection.on("SIGNALRINFO", (status) => {
        var object = JSON.parse(status);

        const device = this.dataSource.find(
          (d) => d.deviceInfo.device_id == object.DEVICE_ID,
        );
        console.log("device :>> ", device);
        // device.healthPacket.agent_status = object.AGENT_STATUS;
        // device.healthPacket.dynamic_data = object.DynamicData;
      });
      this.connection.onreconnecting((error) => {
        console.info("onreconnecting", error);
      });
      this.connection.onreconnected((connectionId) => {
        console.info("onreconnected", connectionId);
      });
      this.connection.start();
    },
    async disconnectSignalR() {
      await this.connection.stop();
      await this.connection.off("/HealthPacket");
      this.connection = null;
    },
    toggleCollapse(event, collapseId, colIndex, rowItem) {
      const elementsWithSpecificClass = document.querySelectorAll(
        ".highlight_border",
      );
      const clickedButtonElement = event.target;
      const monitorItemElement = clickedButtonElement.closest(".monitor_item");

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
            this.startGetDevicePolling(rowItem.deviceInfo.device_id);
            this.collapseStates[0][colItemId] = colIndex;
          }
        } else {
          // 다른 라인 탭 눌렀을 때
          this.clearColArray(colItemId);
          this.startGetDevicePolling(rowItem.deviceInfo.device_id);
          this.setColArray(collapseId, colIndex);
        }
      } else {
        //열린 탭 아무것도 없을 때
        this.startGetDevicePolling(rowItem.deviceInfo.device_id);
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
    getStatusHeaderColorClass(status) {
      if (status) {
        return "status-online-header";
      } else {
        return "status-error-header";
      }
      //  if (status === "Online") {
      //   return "status-online-header";
      // } else if (status === "Offline") {
      //   return "status-offline-header";
      // } else if (status === "Error") {
      //   return "status-error-header";
      // }
    },
    getStatusFloorColorClass(status) {
      if (status) {
        return "status-online-floor";
      } else {
        return "status-error-floor";
      }
      // if (status === "Online") {
      //   return "status-online-floor";
      // } else if (status === "Offline") {
      //   return "status-offline-floor";
      // } else if (status === "Error") {
      //   return "status-error-floor";
      // }
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
}</style>
