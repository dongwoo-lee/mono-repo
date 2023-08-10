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
              <!-- <b-tab title="DL3 / DCM" @click="onTab('monitor-dl3-dcm')" active /> -->
              <!-- <b-tab title="SLAP" @click="onTab('monitor-slap')" /> -->
              <!-- <b-tab title="기타" @click="onTab('monitor-etc')" /> -->
              <component :is="tabName"></component>
            </b-tabs>
          </b-container>
        </b-card>
      </b-colxx>
    </b-row>
  </div>
</template>

<script>
// import DeleteSource from "../template/deleteSource";
// import TrashMiros from "../template/trashMiros";
// import AutoRemove from "../template/autoRemove";

export default {
  components: { DeleteSource, TrashMiros, AutoRemove },
  data() {
    return {
      tabName: "monitor-dl3-dcm",
    };
  },
  methods: {
    onTab(tabName) {
      this.tabName = tabName;
    },
  },
};
</script>
