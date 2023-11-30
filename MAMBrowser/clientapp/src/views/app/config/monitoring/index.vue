<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb
          heading="관리자 모니터링"
          :noNav="true"
        />
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
                title="일반"
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

export default {
  components: { Slap, Etc, Setting },
  data() {
    return {
      // tabName: "dl3",
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
