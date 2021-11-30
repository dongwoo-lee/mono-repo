<template>
  <div class="sidebar" @click.stop="() => {}">
    <div class="main-menu">
      <vue-perfect-scrollbar
        class="scroll"
        :settings="{ suppressScrollX: true, wheelPropagation: false }"
      >
        <ul class="list-unstyled">
          <li
            v-for="item in menuList"
            :class="{
              active:
                (selectedParentMenu === item.id && viewingParentMenu === '') ||
                viewingParentMenu === item.id
            }"
            :key="`parent_${item.id}`"
            :data-flag="item.id"
          >
            <a
              v-if="
                item.children &&
                  item.children.length > 0 &&
                  item.visible === 'Y'
              "
              @click="openSubMenu($event, item)"
              :href="`#${getTo(item.to)}`"
            >
              <i :class="item.icon" />
              {{ item.name }}
            </a>
            <router-link
              v-if="item.children.length === 0 && item.visible === 'Y'"
              @click.native="changeSelectedParentHasNoSubmenu(item.id)"
              :to="getTo(item.to)"
            >
              <i :class="item.icon" />
              {{ item.name }}
            </router-link>
          </li>
        </ul>
      </vue-perfect-scrollbar>
    </div>

    <div class="sub-menu">
      <vue-perfect-scrollbar
        class="scroll"
        :settings="{ suppressScrollX: true, wheelPropagation: false }"
      >
        <ul
          v-for="item in menuList"
          :class="{
            'list-unstyled': true,
            'd-block':
              (selectedParentMenu === item.id && viewingParentMenu === '') ||
              viewingParentMenu === item.id
          }"
          :data-parent="item.id"
          :key="`sub_${item.id}`"
        >
          <li
            v-for="(sub, subIndex) in item.children"
            :key="subIndex"
            :class="{ active: $route.path === sub.to }"
          >
            <template v-if="sub.visible === 'Y' && sub.to">
              <router-link :to="getTo(sub.to)">
                <span>{{ sub.name }}</span>
              </router-link>
            </template>
          </li>
        </ul>
      </vue-perfect-scrollbar>
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters, mapMutations } from "vuex";
import {
  menuHiddenBreakpoint,
  subHiddenBreakpoint
} from "../../constants/config";

export default {
  data() {
    return {
      selectedParentMenu: "",
      viewingParentMenu: ""
    };
  },
  mounted() {
    this.selectMenu();
    window.addEventListener("resize", this.handleWindowResize);
    document.addEventListener("click", this.handleDocumentClick);
    this.handleWindowResize();
  },
  beforeDestroy() {
    document.removeEventListener("click", this.handleDocumentClick);
    window.removeEventListener("resize", this.handleWindowResize);
  },
  methods: {
    ...mapMutations("menu", [
      "changeSideMenuStatus",
      "addMenuClassname",
      "changeSelectedMenuHasSubItems"
    ]),
    selectMenu() {
      const currentParentUrl = this.$route.path
        .split("/")
        .filter(x => x !== "")[1];
      if (currentParentUrl !== undefined || currentParentUrl !== null) {
        this.selectedParentMenu = currentParentUrl.toLowerCase();
      } else {
        this.selectedParentMenu = "dashboards";
      }
      this.isCurrentMenuHasSubItem();
    },
    isCurrentMenuHasSubItem() {
      const menuItem = this.menuList.find(
        x => x.id === this.selectedParentMenu
      );
      const isCurrentMenuHasSubItem =
        menuItem && menuItem.subs && menuItem.subs.length > 0 ? true : false;
      if (isCurrentMenuHasSubItem != this.selectedMenuHasSubItems) {
        if (!isCurrentMenuHasSubItem) {
          this.changeSideMenuStatus({
            step: 0,
            classNames: this.menuType,
            selectedMenuHasSubItems: false
          });
        } else {
          this.changeSideMenuStatus({
            step: 0,
            classNames: this.menuType,
            selectedMenuHasSubItems: true
          });
        }
      }

      return isCurrentMenuHasSubItem;
    },

    changeSelectedParentHasNoSubmenu(parentMenu) {
      this.selectedParentMenu = parentMenu;
      this.viewingParentMenu = parentMenu;
      this.changeSelectedMenuHasSubItems(false);
      this.changeSideMenuStatus({
        step: 0,
        classNames: this.menuType,
        selectedMenuHasSubItems: false
      });
    },

    openSubMenu(e, menuItem) {
      if (this.viewingParentMenu == menuItem.id) {
        this.handleDocumentClick();
        return;
      }
      const selectedParent = menuItem.id;
      const hasSubMenu =
        menuItem.children &&
        menuItem.children.length > 0 &&
        menuItem.enable === "Y";
      this.changeSelectedMenuHasSubItems(hasSubMenu);
      if (!hasSubMenu) {
        this.viewingParentMenu = selectedParent;
        this.selectedParentMenu = selectedParent;
        this.toggle();
      } else {
        const currentClasses = this.menuType
          ? this.menuType.split(" ").filter(x => x !== "")
          : "";

        if (!currentClasses.includes("menu-mobile")) {
          if (
            currentClasses.includes("menu-sub-hidden") &&
            (this.menuClickCount === 2 || this.menuClickCount === 0)
          ) {
            this.changeSideMenuStatus({
              step: 3,
              classNames: this.menuType,
              selectedMenuHasSubItems: hasSubMenu
            });
          } else if (
            currentClasses.includes("menu-hidden") &&
            (this.menuClickCount === 1 || this.menuClickCount === 3)
          ) {
            this.changeSideMenuStatus({
              step: 2,
              classNames: this.menuType,
              selectedMenuHasSubItems: hasSubMenu
            });
          } else if (
            currentClasses.includes("menu-default") &&
            !currentClasses.includes("menu-sub-hidden") &&
            (this.menuClickCount === 1 || this.menuClickCount === 3)
          ) {
            this.changeSideMenuStatus({
              step: 0,
              classNames: this.menuType,
              selectedMenuHasSubItems: hasSubMenu
            });
          }
        } else {
          this.addMenuClassname({
            classname: "sub-show-temporary",
            currentClasses: this.menuType
          });
        }
        this.viewingParentMenu = selectedParent;
      }
    },
    handleDocumentClick(e) {
      this.viewingParentMenu = "";
      this.selectMenu();
      this.toggle();
    },
    toggle() {
      const currentClasses = this.menuType.split(" ").filter(x => x !== "");
      if (
        currentClasses.includes("menu-sub-hidden") &&
        this.menuClickCount === 3
      ) {
        this.changeSideMenuStatus({
          step: 2,
          classNames: this.menuType,
          selectedMenuHasSubItems: this.selectedMenuHasSubItems
        });
      } else if (
        currentClasses.includes("menu-hidden") ||
        currentClasses.includes("menu-mobile")
      ) {
        if (!(this.menuClickCount === 1 && !this.selectedMenuHasSubItems)) {
          this.changeSideMenuStatus({
            step: 0,
            classNames: this.menuType,
            selectedMenuHasSubItems: this.selectedMenuHasSubItems
          });
        }
      }
    },
    // Resize
    handleWindowResize(event) {
      if (event && !event.isTrusted) {
        return;
      }
      let nextClasses = this.getMenuClassesForResize(this.menuType);
      this.changeSideMenuStatus({
        step: 0,
        classNames: nextClasses.join(" "),
        selectedMenuHasSubItems: this.selectedMenuHasSubItems
      });
    },
    getMenuClassesForResize(classes) {
      let nextClasses = classes.split(" ").filter(x => x !== "");
      const windowWidth = window.innerWidth;

      if (windowWidth < menuHiddenBreakpoint) {
        nextClasses.push("menu-mobile");
      } else if (windowWidth < subHiddenBreakpoint) {
        nextClasses = nextClasses.filter(x => x !== "menu-mobile");
        if (
          nextClasses.includes("menu-default") &&
          !nextClasses.includes("menu-sub-hidden")
        ) {
          nextClasses.push("menu-sub-hidden");
        }
      } else {
        nextClasses = nextClasses.filter(x => x !== "menu-mobile");
        if (
          nextClasses.includes("menu-default") &&
          nextClasses.includes("menu-sub-hidden")
        ) {
          nextClasses = nextClasses.filter(x => x !== "menu-sub-hidden");
        }
      }
      return nextClasses;
    },
    getTo(to) {
      return to ? to : "";
    }
  },
  computed: {
    ...mapGetters("menu", {
      menuType: "getMenuType",
      menuClickCount: "getMenuClickCount",
      selectedMenuHasSubItems: "getSelectedMenuHasSubItems"
    }),
    ...mapGetters("user", ["menuList"])
  },
  watch: {
    $route(to, from) {
      if (to.path !== from.path) {
        const toParentUrl = to.path.split("/").filter(x => x !== "")[1];
        if (toParentUrl !== undefined || toParentUrl !== null) {
          this.selectedParentMenu = toParentUrl.toLowerCase();
        } else {
          this.selectedParentMenu = "dashboards";
        }
        this.viewingParentMenu = "";
        this.selectMenu();
        this.toggle();
        this.changeSideMenuStatus({
          step: 0,
          classNames: this.menuType,
          selectedMenuHasSubItems: this.selectedMenuHasSubItems
        });
        window.scrollTo(0, top);
      }
    }
  }
};
</script>
