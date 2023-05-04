<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="시스템 설정" :noNav="true" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <b-row>
      <b-colxx>
        <b-card>
          <b-container fluid>
            <b-tabs content-class="mt-3" fill>
              <b-tab title="그룹 관리" @click="onTab('group')" active />
              <b-tab title="사용자 관리" @click="onTab('user')" />
              <b-tab title="코드 관리" @click="onTab('Code')" />
              <b-tab title="MIROS 관리" @click="onTab('miros')" />
              <component :is="tabName"></component>
            </b-tabs>
          </b-container>
        </b-card>
      </b-colxx>
    </b-row>
  </div>
</template>

<script>
import Group from "../template/Group";
import User from "../template/User";
import Code from "../template/Code";
import Miros from "../template/miros";

export default {
  components: { Group, User, Code, Miros },
  data() {
    return {
      tabName: "group",
    };
  },
  methods: {
    onTab(tabName) {
      this.tabName = tabName;
    },
  },
};
</script>
