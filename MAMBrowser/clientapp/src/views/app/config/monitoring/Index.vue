<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="관리자 모니터링" :noNav="true" />
        <span v-if="GetMonitoringServerStatus">ㅇ</span>
        <span v-if="!GetMonitoringServerStatus">X</span>
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <b-row>
      <b-colxx>
        <b-card>
          <b-container fluid>
            <b-tabs content-class="mt-3" fill>
              <!-- <b-tab
                title="DL3"
                @click="onTab('dl3')"
              /> -->
              <b-tab title="SLAP" @click="onTab('slap')" active />
              <b-tab title="DL3 / 일반" @click="onTab('etc')" />
              <b-tab title="장비 관리" @click="onTab('setting')" />
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
import Slap from "../template/monitor_SLAP"
import Etc from "../template/monitor_ETC"
import Setting from "../template/monitor_SETTING"
import MixinBasicPage from "../../../../mixin/MixinBasicPage"
import { mapGetters, mapMutations } from "vuex"
import * as signalR from "@microsoft/signalr"
import axios from "axios"
export default {
  components: { Slap, Etc, Setting },
  data() {
    return {
      // tabName: "dl3",
      tabName: "slap",
    }
  },
  mixins: [MixinBasicPage],
  computed: {
    ...mapGetters("monitoring", ["GetMonitoringServerStatus"]),
  },
  async created() {
    await this.GetMonitoringServerInfo()
    await this.createSignalR()
  },
  methods: {
    ...mapMutations("monitoring", ["SET_CONNECTION"]),
    onTab(tabName) {
      this.tabName = tabName
    },
    async GetMonitoringServerInfo() {
      try {
        var res = await axios.get(`/api/GetMonitoringServerInfo`)
      } catch (err) {
        this.$fn.notify("error", { title: err.message })
      }
      this.monitoringServerInfo = await res.data.ResultObject
    },
    async createSignalR() {
      if (this.monitoringServerInfo == "") {
        await this.GetMonitoringServerInfo()
      }
      try {
        const connection = await new signalR.HubConnectionBuilder()
          .withUrl(`http://${this.monitoringServerInfo}/mntr/hub`)
          .withAutomaticReconnect({
            nextRetryDelayInMilliseconds: () => {
              return 1000
            },
          })
          .configureLogging(signalR.LogLevel.Error)
          .build()
        connection.start()
        await this.SET_CONNECTION(connection)
      } catch (err) {
        this.$fn.notify("error", { title: err.message })
      }
    },
  },
}
</script>
