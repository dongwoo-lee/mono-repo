<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="설정" :noNav="true" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <b-row>
      <b-colxx>
        <b-card>
          <b-container fluid>
            <b-tabs content-class="mt-3" fill>
              <b-tab title="사용자 목록" @click="onTab('userList')" active />
              <b-tab title="역할 목록" @click="onTab('role')" />
              <b-tab title="방송의뢰 옵션" @click="onTab('masteringOption')" />
              <!-- <b-tab title="시스템" @click="onTab('system')" /> -->
              <component :is="tabName"></component>
            </b-tabs>
          </b-container>
        </b-card>
      </b-colxx>
    </b-row>
  </div>
</template>

<script>
import UserList from "./template/UserList";
import Role from "./template/Role";
import masteringOption from "./template/masteringOption";
import System from "./template/System";

export default {
  components: { UserList, Role, System, masteringOption },
  data() {
    return {
      tabName: "userList",
    };
  },
  methods: {
    onTab(tabName) {
      this.tabName = tabName;
    },
  },
};
</script>
