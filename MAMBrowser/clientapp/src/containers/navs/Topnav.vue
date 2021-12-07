<template>
  <nav class="navbar fixed-top">
    <div class="d-flex align-items-center navbar-left">
      <clock className="system ml-2" style="font-weight: 500"></clock>
      <div class="system" style="color: darkblue; opacity: 0.8">
        {{ conNetworkName }}
      </div>
      <div class="system" :style="getConDBNameStyle()">{{ conDBName }}</div>
      <!-- 메뉴 네비 -->
      <!-- <a
        href="#"
        class="menu-button d-none d-md-block"
        @click.prevent.stop="changeSideMenuStatus({step :menuClickCount+1,classNames:menuType,selectedMenuHasSubItems})"
      >
        <menu-icon />
      </a> -->
      <a
        href="#"
        class="menu-button-mobile d-xs-block d-sm-block d-md-none"
        @click.prevent.stop="changeSideMenuForMobile(menuType)"
      >
        <mobile-menu-icon />
      </a>
    </div>
    <!-- 로고 -->
    <router-link class="navbar-logo" tag="a" :to="getTo()">
      <span class="logo d-none d-xs-block"></span>
      <span class="logo-mobile d-block d-xs-none"></span>
    </router-link>

    <div class="navbar-right">
      <b-row style="justify-content: flex-end">
        <div class="user d-inline-block">
          <table class="topnav-right-table">
            <tr>
              <td rowspan="2">
                <b-button
                  class="btn btn-outline-primary btn-sm default cutom-label mr-2"
                  id="fileuploadbutton"
                  @click="openFileModal"
                  style="
                    padding: 7px !important;
                    padding-right: 0px !important;
                    border-color: #008ecc;
                    color: #008ecc;
                    background-color: white;
                  "
                >
                  <b-icon
                    icon="file-earmark-music"
                    style="margin-right: 15px"
                    aria-hidden="true"
                  >
                  </b-icon>
                  마스터링
                  <b-badge
                    v-show="getBadge != 0"
                    style="
                      position: relative;
                      top: -17px;
                      right: -10px;
                      z-index: 1030;
                      bordercolor: red;
                      color: red;
                      background-color: white;
                      border-radius: 80%;
                    "
                    variant="outline-danger"
                    >{{ getBadge }}</b-badge
                  >
                </b-button>
              </td>
              <!-- 타이머 -->
              <td rowspan="2">
                <timer
                  :timerProccessing="timerProccessing"
                  :expires="tokenExpires"
                  @resetTimer="resetTimer"
                >
                </timer>
              </td>
              <!-- 디스크 용량 정보 -->
              <td>
                <div>
                  <span class="current">
                    {{ $fn.formatMBBytes(currentUser.diskUsed) }} /
                    {{ currentUser.diskMax }} GB</span
                  >
                  <span
                    :class="
                      currentUser.diskAvailable <= 1000 * 1000 * 100
                        ? 'free-space-red'
                        : 'free-space-blue'
                    "
                    >여유
                    {{
                      $fn.formatMBBytes(currentUser.diskAvailable, 1048000)
                    }}</span
                  >
                </div>
              </td>
              <!-- 사용자 정보 -->
              <td rowspan="2">
                <b-dropdown
                  class="dropdown-menu-right"
                  right
                  variant="empty"
                  toggle-class="p-0"
                  menu-class="mt-3"
                  no-caret
                >
                  <template slot="button-content">
                    <div style="margin-top: 6px">
                      <i class="iconsminds-administrator"></i>
                      <span class="name mr-1">
                        {{ currentUser.name }}({{ currentUser.menuGrpName }})
                      </span>
                    </div>
                    <div
                      style="
                        float: right;
                        color: red;
                        font-size: 12px;
                        margin-right: 6px;
                      "
                    >
                      v1.0.211020
                    </div>
                  </template>
                  <div v-if="isDisplaySetting()">
                    <b-dropdown-item @click="$router.push({ path: '/app/log' })"
                      >사용자 로그보기</b-dropdown-item
                    >
                    <b-dropdown-item
                      @click="$router.push({ path: '/app/config' })"
                      >설정</b-dropdown-item
                    >
                    <b-dropdown-divider />
                  </div>
                  <b-dropdown-item @click="logout">로그아웃</b-dropdown-item>
                </b-dropdown>
              </td>
            </tr>
            <tr>
              <td>
                <!-- 디스크 용량 프로그래스바 -->
                <b-progress
                  class=""
                  :value="currentUser.diskUsed"
                  :max="currentUser.diskMax * (1024 * 1024 * 1024)"
                  animated
                >
                </b-progress>
              </td>
            </tr>
          </table>
        </div>
      </b-row>
    </div>
  </nav>
</template>

<script>
import { mapGetters, mapMutations, mapActions, mapState } from "vuex";
import { MenuIcon, MobileMenuIcon } from "../../components/Svg";
import { SYSTEM_MANAGEMENT_CODE } from "../../constants/config";
import { getDirection, setDirection } from "../../utils";

export default {
  components: {
    "menu-icon": MenuIcon,
    "mobile-menu-icon": MobileMenuIcon,
  },
  data() {
    return {
      SYSTEM_MANAGEMENT_CODE: SYSTEM_MANAGEMENT_CODE,
    };
  },
  created() {
    this.renewal();
  },
  methods: {
    ...mapMutations("menu", [
      "changeSideMenuStatus",
      "changeSideMenuForMobile",
    ]),
    ...mapActions("user", ["setLang", "signOut", "renewal"]),
    ...mapMutations("user", ["SET_INIT_CALL_LOGIN_AUTH_TRY_CNT", "SET_LOGOUT"]),
    ...mapMutations("FileIndexStore", ["setFileModal", "startDBConnection"]),
    openFileModal() {
      this.setFileModal(true);
    },
    logout() {
      this.SET_LOGOUT();
      this.$router.push("/user/Login");
    },
    isDisplaySetting() {
      return this.behaviorList.some(
        (item) => item.id === SYSTEM_MANAGEMENT_CODE && item.visible === "Y"
      );
    },
    getTo() {
      if (this.roleList) {
        const firstVisibleIndex = this.roleList.findIndex(
          (role) => role.visible === "Y"
        );
        if (this.roleList[firstVisibleIndex]) {
          return this.roleList[firstVisibleIndex].to;
        }
      }

      return "";
    },
    resetTimer() {
      this.SET_INIT_CALL_LOGIN_AUTH_TRY_CNT();
      this.renewal().then((res) => {
        if (res && res.data && res.data.resultCode === 0) {
          this.$notify("primary", "로그인 연장되었습니다.");
        }
      });
    },
    getConDBNameStyle() {
      if (!this.conDBName) return {};
      if (this.conDBName.indexOf("운영") > -1) {
        return { color: "darkblue", opacity: 0.8 };
      }

      return { color: "darkred", opacity: 0.8 };
    },
  },
  computed: {
    ...mapGetters("menu", {
      menuType: "getMenuType",
      menuClickCount: "getMenuClickCount",
      selectedMenuHasSubItems: "getSelectedMenuHasSubItems",
    }),
    ...mapGetters("user", [
      "currentUser",
      "behaviorList",
      "roleList",
      "timerProccessing",
      "tokenExpires",
      "conDBName",
      "conNetworkName",
    ]),
    ...mapGetters("FileIndexStore", ["getBadge"]),
    ...mapState("FileIndexStore", {
      FileModal: (state) => state.FileModal,
    }),
  },
  watch: {
    "$i18n.locale"(to, from) {
      if (from !== to) {
        this.$router.go(this.$route.path);
      }
    },
  },
};
</script>
<style scoped>
.free-space-blue {
  color: darkblue;
  font-weight: 600;
  margin-left: 20px;
}
.free-space-red {
  color: red;
  font-weight: 600;
  margin-left: 20px;
}
</style>
