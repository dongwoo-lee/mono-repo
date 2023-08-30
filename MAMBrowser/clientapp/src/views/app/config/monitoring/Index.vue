<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="관리자 모니터링" :noNav="true" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <b-row>
      <b-colxx>
        <b-card>
          <b-container fluid>
            <b-tabs content-class="mt-3" fill>
              <b-tab title="DL3 / DCM" @click="onTab('dl3-dcm')" />
              <b-tab title="SLAP" @click="onTab('slap')" active />
              <b-tab title="기타" @click="onTab('etc')" />
              <component :is="tabName"></component>
            </b-tabs>
          </b-container>
        </b-card>
      </b-colxx>
    </b-row>
  </div>
</template>

<script>
import Dl3Dcm from "../template/monitor_DL3_DCM";
import Slap from "../template/monitor_SLAP";
import Etc from "../template/monitor_ETC";

export default {
  components: { Dl3Dcm, Slap, Etc },
  data() {
    return {
      // tabName: "dl3-dcm",
      tabName: "slap",
    };
  },
  methods: {
    onTab(tabName) {
      this.tabName = tabName;
    },
  },
};
</script>
