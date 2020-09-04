<template>
  <nav class="navbar fixed-top">
    <div class="d-flex align-items-center navbar-left">
      <!-- 메뉴 네비 -->
      <a
        href="#"
        class="menu-button d-none d-md-block"
        @click.prevent.stop="changeSideMenuStatus({step :menuClickCount+1,classNames:menuType,selectedMenuHasSubItems})"
      >
        <menu-icon />
      </a>
      <a
        href="#"
        class="menu-button-mobile d-xs-block d-sm-block d-md-none"
        @click.prevent.stop="changeSideMenuForMobile(menuType)"
      >
        <mobile-menu-icon />
      </a>
    </div>
    <!-- 로고 -->
    <router-link class="navbar-logo" tag="a" to="/app">
      <span class="logo d-none d-xs-block"></span>
      <span class="logo-mobile d-block d-xs-none"></span>
    </router-link>

    <div class="navbar-right">
      <div class="user d-inline-block">
        <table>
            <tr>
              <td>
                <span class="current col-sm-6 col-md-offset-2"> {{currentUser.diskUsed}} / {{currentUser.diskMax}}</span>
                <span class="free-space col-sm-6">{{ $fn.formatBytes(currentUser.diskAvailable) }}</span>
              </td>
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
                    <span class="name mr-1">
                      {{currentUser.name}}({{currentUser.menuGrpName}})
                      <i class="iconsminds-administrator"></i>
                      </span>

                  </template>
                  <b-dropdown-item @click="$router.push({ path: '/app/log' })">사용자 로그보기</b-dropdown-item>
                  <b-dropdown-item @click="$router.push({ path: '/app/config' })">설정</b-dropdown-item>
                  <b-dropdown-divider />
                  <b-dropdown-item @click="logout">로그아웃</b-dropdown-item>
                </b-dropdown>
              </td>
            </tr>
            <tr>
              <td>
                <b-progress 
                  class=""
                  :value="2"
                  :max="10"
                  animated>
                </b-progress>
              </td>
            </tr>
        </table>
      </div>
    </div>
  </nav>
</template>

<script>
import { mapGetters, mapMutations, mapActions } from "vuex";
import { MenuIcon, MobileMenuIcon } from "../../components/Svg";
import {
  menuHiddenBreakpoint,
  localeOptions,
  buyUrl,
  defaultColor,
  themeSelectedColorStorageKey
} from "../../constants/config";
import { getDirection, setDirection } from "../../utils";
export default {
  components: {
    "menu-icon": MenuIcon,
    "mobile-menu-icon": MobileMenuIcon
  },
  data() {
    return {
      fullScreen: false,
      menuHiddenBreakpoint,
      localeOptions,
      buyUrl,
      isDarkActive: false
    };
  },
  created() {
    this.$nextTick(() => {
      this.getUser();
    })
  },
  methods: {
    ...mapMutations("menu", [
      "changeSideMenuStatus",
      "changeSideMenuForMobile"
    ]),
    ...mapActions("user", ["getUser", "setLang", "signOut"]),
    logout() {
      this.signOut().then(() => {
        this.$router.push("/user/login");
      });
    }
  },
  computed: {
    ...mapGetters("user", ["currentUser"]),
    ...mapGetters("menu", {
      menuType: "getMenuType",
      menuClickCount: "getMenuClickCount",
      selectedMenuHasSubItems: "getSelectedMenuHasSubItems"
    })
  },
  watch: {
    "$i18n.locale"(to, from) {
      if (from !== to) {
        this.$router.go(this.$route.path);
      }
    }
  }
};
</script>
<style lang="scss" scoped>
table { border-collapse:collapse; }  
td {
  .dropdown {
     margin-left: 10px;
  }
}

</style>