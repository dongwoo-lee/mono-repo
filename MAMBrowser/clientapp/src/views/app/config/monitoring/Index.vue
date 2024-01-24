<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb
          heading="관리자 모니터링"
          :noNav="true"
        />
        <span
          v-if="GetMonitoringServerStatus"
          class="text-primary"
          style="padding: 0"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="2vh"
            height="2vh"
            fill="currentColor"
            class="bi bi-plug-fill"
            viewBox="0 0 16 16"
          >
            <path
              d="M6 0a.5.5 0 0 1 .5.5V3h3V.5a.5.5 0 0 1 1 0V3h1a.5.5 0 0 1 .5.5v3A3.5 3.5 0 0 1 8.5 10c-.002.434-.01.845-.04 1.22-.041.514-.126 1.003-.317 1.424a2.083 2.083 0 0 1-.97 1.028C6.725 13.9 6.169 14 5.5 14c-.998 0-1.61.33-1.974.718A1.922 1.922 0 0 0 3 16H2c0-.616.232-1.367.797-1.968C3.374 13.42 4.261 13 5.5 13c.581 0 .962-.088 1.218-.219.241-.123.4-.3.514-.55.121-.266.193-.621.23-1.09.027-.34.035-.718.037-1.141A3.5 3.5 0 0 1 4 6.5v-3a.5.5 0 0 1 .5-.5h1V.5A.5.5 0 0 1 6 0"
            />
          </svg>
        </span>
        <span
          v-else
          class="text-secondary"
          style="padding: 0"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="2vh"
            height="2vh"
            fill="currentColor"
            class="bi bi-plug"
            viewBox="0 0 16 16"
          >
            <path
              d="M6 0a.5.5 0 0 1 .5.5V3h3V.5a.5.5 0 0 1 1 0V3h1a.5.5 0 0 1 .5.5v3A3.5 3.5 0 0 1 8.5 10c-.002.434-.01.845-.04 1.22-.041.514-.126 1.003-.317 1.424a2.083 2.083 0 0 1-.97 1.028C6.725 13.9 6.169 14 5.5 14c-.998 0-1.61.33-1.974.718A1.922 1.922 0 0 0 3 16H2c0-.616.232-1.367.797-1.968C3.374 13.42 4.261 13 5.5 13c.581 0 .962-.088 1.218-.219.241-.123.4-.3.514-.55.121-.266.193-.621.23-1.09.027-.34.035-.718.037-1.141A3.5 3.5 0 0 1 4 6.5v-3a.5.5 0 0 1 .5-.5h1V.5A.5.5 0 0 1 6 0M5 4v2.5A2.5 2.5 0 0 0 7.5 9h1A2.5 2.5 0 0 0 11 6.5V4z"
            />
          </svg>
        </span>
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <b-row>
      <b-colxx>
        <b-card>
          <b-container fluid>
            <b-tabs
              content-class="mt-3"
              fill
            >
              <!-- <b-tab
                title="DL3"
                @click="onTab('dl3')"
              /> -->
              <b-tab
                title="SLAP"
                @click="onTab('slap')"
                active
              />
              <b-tab
                title="DL3 / 일반"
                @click="onTab('etc')"
              />
              <b-tab
                title="장비 관리"
                @click="onTab('setting')"
              />
              <component :is="tabName"></component>
            </b-tabs>
          </b-container>
        </b-card>
      </b-colxx>
    </b-row>
  </div>
</template>

<script>
// import Dl3 from "../template/monitor_DL3";
import Slap from "../template/monitor_SLAP";
import Etc from "../template/monitor_ETC";
import Setting from "../template/monitor_SETTING";
import MixinBasicPage from "../../../../mixin/MixinBasicPage";
import { mapState, mapGetters, mapMutations } from "vuex";
import * as signalR from "@microsoft/signalr";
import axios from "axios";
export default {
  components: { Slap, Etc, Setting },
  data() {
    return {
      // tabName: "dl3",
      tabName: "slap",
    };
  },
  mixins: [MixinBasicPage],
  computed: {
    ...mapState("monitoring", {
      connection: (state) => state.connection,
    }),
    ...mapGetters("monitoring", ["GetMonitoringServerStatus"]),
  },
  watch: {
    GetMonitoringServerStatus: function (val) {
      if (!val) {
        this.$fn.notify("error", {
          title: "모니터링 서버 연결 오류",
        });
      }
    },
  },
  async created() {
    await this.GetMonitoringServerInfo();
    await this.createSignalR();
  },
  methods: {
    ...mapMutations("monitoring", ["SET_CONNECTION"]),
    onTab(tabName) {
      this.tabName = tabName;
    },
    async GetMonitoringServerInfo() {
      try {
        var res = await axios.get(`/api/GetMonitoringServerInfo`);
      } catch (err) {
        this.$fn.notify("error", { title: err.message });
      }
      this.monitoringServerInfo = await res.data.ResultObject;
    },
    async createSignalR() {
      if (
        this.monitoringServerInfo == "" ||
        this.monitoringServerInfo == null ||
        this.monitoringServerInfo == undefined
      ) {
        await this.GetMonitoringServerInfo();
      }
      try {
        const connection = await new signalR.HubConnectionBuilder()
          .withUrl(`http://${this.monitoringServerInfo}/mntr/hub`)
          .withAutomaticReconnect({
            nextRetryDelayInMilliseconds: () => {
              return 1000;
            },
          })
          .configureLogging(signalR.LogLevel.Error)
          .build();
        connection.start();
                await this.SET_CONNECTION(connection);
      } catch (err) {
        this.$fn.notify("error", { title: err.message });
      }
    },
  },
};
</script>
