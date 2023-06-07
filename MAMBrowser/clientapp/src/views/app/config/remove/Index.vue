<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="소재 삭제 관리" :noNav="true" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <b-row>
      <b-colxx>
        <b-card>
          <b-container fluid>
            <b-tabs content-class="mt-3" fill>
              <b-tab title="소재 삭제" @click="onTab('delete-source')" active />
              <b-tab title="MIROS 휴지통" @click="onTab('trash-miros')" />
              <b-tab title="삭제 로그" @click="onTab('auto-remove')" />
              <component :is="tabName"></component>
            </b-tabs>
          </b-container>
        </b-card>
      </b-colxx>
    </b-row>
  </div>
</template>

<script>
import DeleteSource from "../template/deleteSource";
import TrashMiros from "../template/trashMiros";
import AutoRemove from "../template/autoRemove";

export default {
  components: { DeleteSource, TrashMiros, AutoRemove },
  data() {
    return {
      tabName: "delete-source",
    };
  },
  methods: {
    onTab(tabName) {
      this.tabName = tabName;
    },
  },
};
</script>
