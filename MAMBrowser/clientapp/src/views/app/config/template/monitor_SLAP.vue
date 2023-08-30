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
              dataSource[itemCount * (index - 1) + (i - 1)]
            )
          "
          v-if="dataSource[itemCount * (index - 1) + (i - 1)]"
        >
          <div
            class="btn_header"
            :class="
              getStatusHeaderColorClass(
                dataSource[itemCount * (index - 1) + (i - 1)].agentInfo
                  .agentStatus
              )
            "
          >
            <span
              class="floor"
              :class="
                getStatusFloorColorClass(
                  dataSource[itemCount * (index - 1) + (i - 1)].agentInfo
                    .agentStatus
                )
              "
            >
              {{ dataSource[itemCount * (index - 1) + (i - 1)].location }}
            </span>
            <span class="name">
              {{ dataSource[itemCount * (index - 1) + (i - 1)].deviceName }}
            </span>
            <span class="device">
              {{
                getDeviceName(
                  dataSource[itemCount * (index - 1) + (i - 1)].extInfo.slapType
                )
              }}
            </span>
          </div>
          <div class="btn_body">
            {{
              dataSource[itemCount * (index - 1) + (i - 1)].extInfo
                .logonUserName
                ? dataSource[itemCount * (index - 1) + (i - 1)].extInfo
                    .logonUserName + " / "
                : ""
            }}
            {{ dataSource[itemCount * (index - 1) + (i - 1)].agentInfo.title }}
          </div>
        </b-button>
      </div>
      <div>
        <b-collapse :id="'collapse-' + index" class="collapse_detail">
          <b-card
            class="detail_body"
            v-if="
              rowIndex && dataSource[itemCount * (index - 1) + (rowIndex - 1)]
            "
          >
            <dl class="group_content">
              <dt class="content_title">단말 모델명 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .deviceModel
                }}
              </dd>
              <dt class="content_title">단말 컴퓨터 이름 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .machineName
                }}
              </dd>
              <dt class="content_title">윈도우 버전 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)].osVersion
                }}
              </dd>
              <dt class="content_title">프로세서 정보 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .processorInfo
                }}
              </dd>
              <dt class="content_title">IP정보 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)].ipInfo
                }}
              </dd>
              <dt class="content_title">cpu 사용률 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .deviceStatus.cpu
                }}
              </dd>
              <dt class="content_title">메모리 사용률 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .deviceStatus.memory
                }}
              </dd>
              <dt class="content_title">디스크 사용률 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .deviceStatus.disk
                }}
              </dd>
              <dt class="content_title">디스크 읽기/쓰기</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .deviceStatus.diskIOUseRate
                }}
              </dd>
              <dt class="content_title">네트워크 사용률 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)]
                    .deviceStatus.networkUseRate
                }}
              </dd>
              <!-- <div
                v-for="(value, key) in dataSource[
                  itemCount * (index - 1) + (rowIndex - 1)
                ]"
              >
                <div v-if="!(typeof value === 'object')">
                  <dt class="content_title">{{ key }}</dt>
                  <dd class="content_text">{{ value }}</dd>
                </div>
              </div> -->
            </dl>
            <dl class="group_content">
              <dt class="content_title">스튜디오명 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)].extInfo
                    .studioName
                }}
              </dd>
              <dt class="content_title">SLAP이름 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)].extInfo
                    .slapName
                }}
              </dd>
              <dt class="content_title">큐시트이름 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)].extInfo
                    .cueSheetName
                }}
              </dd>
              <dt class="content_title">로그인 사용자 이름 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)].extInfo
                    .logonUserName
                }}
              </dd>
              <dt class="content_title">Agent 작동상태 :</dt>
              <dd class="content_text">
                {{
                  dataSource[itemCount * (index - 1) + (rowIndex - 1)].agentInfo
                    .agentStatus
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
const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:8005/Hub1")
  .withAutomaticReconnect({
    nextRetryDelayInMilliseconds: () => {
      return 1000;
    },
  })
  .configureLogging(signalR.LogLevel.Error)
  .build();
export default {
  data() {
    return {
      itemCount: 6,
      detailItem: {},
      collapseStates: [],
      rowData: {},
      rowIndex: null,
      dataSource: [],
    };
  },
  components: {},
  async mounted() {
    await this.getData();
    await this.connectSignalR();
  },
  async beforeDestroy() {
    await this.disconnectSignalR();
    this.stopGetDevicePolling();
  },
  methods: {
    getData() {
      this.$http
        .get(`http://localhost:8005/Monitoring/device/slap`, null)
        .then((res) => {
          // console.info("getAll", res.data);
          this.dataSource = res.data;
        });
    },
    getDevice(deviceID) {
      this.$http
        .get(`http://localhost:8005/Monitoring/device/slap/${deviceID}`, null)
        .then((res) => {
          const device = this.dataSource.find((d) => d.deviceID == deviceID);
          device.agentInfo = res.data.agentInfo;
          device.deviceStatus = res.data.deviceStatus;
          device.extInfo = res.data.extInfo;
        });
    },
    async startGetDevicePolling(deviceID) {
      const pollingInterval = 5000;
      this.getDevice(deviceID);
      this.pollingTimer = setInterval(() => {
        this.getDevice(deviceID);
      }, pollingInterval);
    },
    stopGetDevicePolling() {
      clearInterval(this.pollingTimer);
    },
    connectSignalR() {
      connection.on("ChangedAgentStatus", (status) => {
        // console.info("ChangedAgentStatus", status);
        const device = this.dataSource.find(
          (d) => d.deviceID == status.deviceID
        );
        device.agentInfo = status;
      });
      connection.onreconnecting((error) => {
        console.info("onreconnecting", error);
      });
      connection.onreconnected((connectionId) => {
        console.info("onreconnected", connectionId);
      });
      connection.start();
    },
    async disconnectSignalR() {
      if (connection) {
        await connection.stop();
      }
    },
    toggleCollapse(event, collapseId, colIndex, rowItem) {
      const elementsWithSpecificClass =
        document.querySelectorAll(".highlight_border");
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
            this.startGetDevicePolling(rowItem.deviceID);
            this.collapseStates[0][colItemId] = colIndex;
          }
        } else {
          // 다른 라인 탭 눌렀을 때
          this.clearColArray(colItemId);
          this.startGetDevicePolling(rowItem.deviceID);
          this.setColArray(collapseId, colIndex);
        }
      } else {
        //열린 탭 아무것도 없을 때
        this.startGetDevicePolling(rowItem.deviceID);
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
}
</style>
