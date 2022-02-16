<template>
  <div id="overView">
    <b-row style="marin-top: -10px">
      <b-card class="w-100" id="cardView">
        <div v-if="loadingVisible" style="height: 750px"></div>
        <div v-else class="detail_view">
          <div class="left_view">
            <div class="left_top" v-show="searchToggleSwitch">
              <div class="listTitle mb-3">
                <piaf-breadcrumb />
              </div>
              <div class="MainTilte">
                <h1>즐겨찾기 편집</h1>
              </div>
              <div class="separator mb-3 mt-0"></div>
              <div class="button_view_fav">
                <ButtonWidget :fav="true" :type="type" />
              </div>
            </div>
            <div class="left_bottom">
              <div style="height: 100%">
                <DxTabPanel id="tabPanel" :defer-rendering="false">
                  <DxItem title="즐겨찾기">
                    <template #default>
                      <div>
                        <SortableWidget
                          :widgetIndex="16"
                          :searchToggleSwitch="searchToggleSwitch"
                          channelKey="channel_my"
                        />
                      </div>
                    </template>
                  </DxItem>
                  <!-- <DxItem title="부가정보">
                    <template #default>
                    </template>
                  </DxItem> -->
                </DxTabPanel>
              </div>
            </div>
          </div>
          <div class="right_view">
            <div>
              <SearchWidget :width_size="490" />
            </div>
          </div>
        </div>
      </b-card>
    </b-row>
    <DxLoadPanel
      :position="position"
      :visible.sync="loadingVisible"
      :show-indicator="showIndicator"
      :shading="true"
      :show-pane="showPane"
      :message="loadPanelMessage"
      :close-on-outside-click="closeOnOutsideClick"
      shading-color="rgba(0,0,0,0.4)"
    />
  </div>
</template>

<script>
import { USER_ID } from "@/constants/config";
import { mapActions, mapGetters, mapMutations } from "vuex";
import SearchWidget from "./SearchWidget.vue";
import ButtonWidget from "./ButtonWidget.vue";
import AbchannelWidget from "./AbchannelWidget.vue";
import PrintWidget from "./PrintWidget.vue";
import SortableWidget from "./C_SortableWidget.vue";
import DxTabPanel, { DxItem } from "devextreme-vue/tab-panel";
import DxSpeedDialAction from "devextreme-vue/speed-dial-action";
import { DxLoadPanel } from "devextreme-vue/load-panel";

export default {
  components: {
    SearchWidget,
    ButtonWidget,
    DxTabPanel,
    DxItem,
    PrintWidget,
    AbchannelWidget,
    SortableWidget,
    DxSpeedDialAction,
    DxLoadPanel,
  },
  data() {
    return {
      loadingVisible: false,
      loadPanelMessage: "큐시트를 가져오는 중 입니다...",
      position: { of: "#cardView" },
      showIndicator: true,
      shading: true,
      showPane: true,
      closeOnOutsideClick: false,

      type: "F",
      classText: "",
      options: [{ text: "자동저장", value: false }],
      searchToggleSwitch: true,
      printHeight: 560,
      abChannelHeight: 734,
    };
  },
  //mounted() {
  //},
  async created() {
    this.loadingVisible = true;
    await this.getCueCon();
    var ele = document.getElementById("app-container");
    this.classText = ele.classList.item(1);
  },
  computed: {},
  methods: {
    ...mapActions("cueList", ["getCueDayFav"]),
    async getCueCon() {
      var userId = sessionStorage.getItem(USER_ID);
      var params = {
        personid: userId,
        pgmcode: "",
        brd_dt: "",
      };
      await this.getCueDayFav(params);
      this.loadingVisible = false;
    },
    onTextEdit() {
      this.$refs.inputText.focus();
    },
    searchToggleEvent() {
      if (this.searchToggleSwitch) {
        this.printHeight = 310;
        this.abChannelHeight = 354;
        document
          .querySelector(".detail_view")
          .classList.add("detail_view_search_toggle_on");
        var allQuery = document.querySelectorAll(".cartC_view");
        allQuery.forEach((item) => {
          item.classList.add("cartC_view_search_toggle_on");
        });
      } else {
        this.printHeight = 560;
        this.abChannelHeight = 734;
        document
          .querySelector(".detail_view")
          .classList.remove("detail_view_search_toggle_on");
        var allQuery = document.querySelectorAll(".cartC_view");
        allQuery.forEach((item) => {
          item.classList.remove("cartC_view_search_toggle_on");
        });
      }
      this.searchToggleSwitch = !this.searchToggleSwitch;
    },
  },
};
</script>
<style>
.detail_view {
  position: relative;
  width: 100%;
  height: 750px;
}
.left_view {
  position: absolute;
  width: 49.5%;
  height: 100%;
  float: left;
}
.right_view {
  width: 49.5%;
  height: 100%;
  float: right;
}
.left_top {
  position: relative;
  width: 100%;
  height: 130px;
  float: top;
}
.left_bottom {
  width: 100%;
  height: 618px;
  float: bottom;
}
/* 도구 버튼 모음 */
.button_view_fav {
  height: 30px;
  position: absolute;
  top: 10px;
  right: 0px;
}

.separator {
  border-bottom: 1px solid #d7d7d7;
}

.subtitle_css {
  color: #008ecc;
}
.sub_text {
  margin-right: 15px;
}

/* 저장, 소재검색 버튼 색상 */
.dx-button-mode-contained.dx-button-default,
.dx-fa-button.dx-fa-button-main .dx-overlay-content {
  background-color: #008ecc;
}
/* C채널 높이(SortableWidget) */
.cartC_view {
  height: 580px;
}
/* 소재검색 토글창 (활성화) */
.search_toggle {
  height: 370px;
}
.detail_view_search_toggle_on {
  height: 370px;
}
.cartC_view_search_toggle_on {
  height: 330px;
}
.dx-widget,
input {
  font-family: "MBC 새로움 M" !important;
}
.listTitle .breadcrumb {
  margin: 0;
  padding: 0;
}
/* loadPanel */
.dx-loadpanel-wrapper {
  font-family: "MBC 새로움 M";
  z-index: 6 !important;
}
</style>