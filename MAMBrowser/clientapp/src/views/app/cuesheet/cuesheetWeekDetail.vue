<template>
  <div id="overView">
    <b-row style="marin-top: -10px">
      <b-card class="w-100" id="cardView">
        <div v-if="loadingVisible" style="height: 750px"></div>
        <div v-else class="detail_view">
          <div class="left_view">
            <div id="left_top" v-show="searchToggleSwitch">
              <div class="listTitle mb-3">
                <piaf-breadcrumb />
              </div>
              <div class="MainTilte">
                <h1>
                  <span v-if="cueInfo.media == 'A'">[AM]</span>
                  <span v-if="cueInfo.media == 'F'">[FM]</span>
                  <span v-if="cueInfo.media == 'D'">[DMB]</span>
                  <span v-if="cueInfo.media == 'C'">[공통]</span>
                  <span v-if="cueInfo.media == 'Z'">[기타]</span>
                  {{ cueInfo.title }}
                </h1>
              </div>
              <div class="separator mb-3 mt-0"></div>
              <div class="subtitle ml-2">
                <span class="sub_text">
                  <span class="subtitle_css">●</span>
                  적용요일 :
                  <span style="display: inline-block">
                    <common-weeks :rowData="cueInfo" :edit="true">
                    </common-weeks>
                  </span>
                </span>
                <!-- <span class="sub_text">
                  <span class="subtitle_css">●</span>
                  매체 :
                  <span v-if="cueInfo.media == 'A'">AM</span>
                  <span v-if="cueInfo.media == 'F'">FM</span>
                  <span v-if="cueInfo.media == 'D'">DMB</span>
                  <span v-if="cueInfo.media == 'C'">공통</span>
                  <span v-if="cueInfo.media == 'Z'">기타</span>
                </span> -->
                <span class="sub_text">
                  <span class="subtitle_css">●</span>
                  담당자 :
                  <span>{{ proUserList }}</span>
                </span>
                <span class="sub_text">
                  <span class="subtitle_css">●</span>
                  최종 편집 일시 :
                  <span>{{
                    $moment(cueInfo.edittime).format("YYYY-MM-DD")
                  }}</span>
                </span>
                <span class="autosave">
                  <b-form-checkbox-group
                    :options="options"
                    v-model="autosaveValue"
                    @change="toggleChange"
                    switches
                    style="float: right"
                  ></b-form-checkbox-group>
                </span>
              </div>
              <div
                id="button_view"
                :class="{ button_view_vertical: !searchToggleSwitch }"
              >
                <ButtonWidget
                  :type="cueInfo.cuetype"
                  :saveText="searchToggleSwitch ? '저장' : ''"
                />
              </div>
            </div>
            <div class="left_bottom">
              <div style="height: 100%">
                <DxTabPanel id="tabPanel" :defer-rendering="false">
                  <DxItem title="출력용">
                    <template #default>
                      <div>
                        <PrintWidget :printHeight="printHeight" />
                      </div>
                    </template>
                  </DxItem>
                  <DxItem title="C1">
                    <template #default>
                      <div class="c_channel_panel">
                        <SortableWidget
                          :widgetIndex="16"
                          :searchToggleSwitch="searchToggleSwitch"
                          channelKey="channel_1"
                        />
                      </div>
                    </template>
                  </DxItem>
                  <DxItem title="C2">
                    <template #default>
                      <div class="c_channel_panel">
                        <SortableWidget
                          :widgetIndex="16"
                          :searchToggleSwitch="searchToggleSwitch"
                          channelKey="channel_2"
                        />
                      </div>
                    </template>
                  </DxItem>
                  <DxItem title="C3">
                    <template #default>
                      <div>
                        <SortableWidget
                          :widgetIndex="16"
                          :searchToggleSwitch="searchToggleSwitch"
                          channelKey="channel_3"
                        />
                      </div>
                    </template>
                  </DxItem>
                  <DxItem title="C4">
                    <template #default>
                      <div>
                        <SortableWidget
                          :widgetIndex="16"
                          :searchToggleSwitch="searchToggleSwitch"
                          channelKey="channel_4"
                        />
                      </div>
                    </template>
                  </DxItem>
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
                </DxTabPanel>
              </div>
            </div>
          </div>
          <div class="right_view">
            <div class="ab_channel">
              <AbchannelWidget :abChannelHeight="abChannelHeight" />
            </div>
          </div>
        </div>
      </b-card>
    </b-row>
    <b-row class="search_toggle mt-1" v-show="!searchToggleSwitch">
      <b-card class="w-100">
        <div>
          <SearchWidget :width_size="330" />
        </div>
      </b-card>
    </b-row>
    <DxSpeedDialAction
      v-if="!loadingVisible"
      icon="search"
      @click="searchToggleEvent"
    />
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
import CommonWeeks from "../../../components/DataTable/CommonWeeks.vue";
import { eventBus } from "@/eventBus";
import axios from "axios";
const qs = require("qs");

export default {
  beforeRouteLeave(to, from, next) {
    if (this.timer > 1) {
      const answer = window.confirm(
        "저장하지 않은 데이터는 손실됩니다. 현재 페이지를 벗어나시겠습니까?"
      );
      if (answer) {
        clearInterval(this.autoSaveFun);
        eventBus.$off();
        next();
      }
    } else {
      clearInterval(this.autoSaveFun);
      eventBus.$off();
      next();
    }
  },
  components: {
    DxLoadPanel,
    SearchWidget,
    ButtonWidget,
    DxTabPanel,
    DxItem,
    PrintWidget,
    AbchannelWidget,
    SortableWidget,
    DxSpeedDialAction,
    CommonWeeks,
  },
  data() {
    return {
      loadingVisible: false,
      loadPanelMessage: "데이터를 가져오는 중 입니다...",
      position: { of: "#cardView" },
      showIndicator: true,
      shading: true,
      showPane: true,
      closeOnOutsideClick: false,

      options: [{ text: "자동저장(5분 마다)", value: true }],
      autosaveValue: [true],
      autoSaveFun: null,
      searchToggleSwitch: true,
      printHeight: 560,
      abChannelHeight: 734,
    };
  },
  async created() {
    this.loadingVisible = true;
    //큐시트 상세내용 가져오기
    await this.getCueCon();
    //자동저장
    this.autoSaveFun = setInterval(() => {
      if (this.cueSheetAutoSave && this.timer > 1) {
        this.saveDefCue();
      }
    }, 300000); //15분마다 저장
    await this.getautosave(this.cueInfo.personid);
    if (!this.cueSheetAutoSave) {
      this.autosaveValue = [];
    }
  },
  computed: {
    ...mapGetters("cueList", ["cueInfo"]),
    ...mapGetters("cueList", ["proUserList"]),
    ...mapGetters("cueList", ["cueSheetAutoSave"]),
    ...mapGetters("user", ["timer"]),
  },
  methods: {
    ...mapActions("cueList", ["getautosave"]),
    ...mapActions("cueList", ["setautosave"]),
    ...mapActions("cueList", ["saveDefCue"]),
    ...mapActions("cueList", ["getProUserList"]),
    ...mapActions("cueList", ["setCueConData"]),
    ...mapActions("cueList", ["setclearCon"]),
    ...mapActions("cueList", ["setSponsorList"]),
    ...mapMutations("cueList", ["SET_CUESHEETAUTOSAVE"]),
    ...mapMutations("cueList", ["SET_CUEINFO"]),
    ...mapActions("cueList", ["getCueDayFav"]),
    //상세내용 가져오기
    async getCueCon() {
      let rowData = JSON.parse(sessionStorage.getItem("USER_INFO"));
      var userId = sessionStorage.getItem(USER_ID);
      var params = {
        productid: rowData.productid,
        pgmcode: rowData.pgmcode,
        week: rowData.weeks,
      };
      await axios
        .get(`/api/defcuesheet/GetdefCue`, {
          params: params,
          paramsSerializer: (params) => {
            return qs.stringify(params);
          },
        })
        .then(async (res) => {
          await this.getProUserList(rowData.productid);
          var cueData = res.data.cueSheetDTO;
          cueData.r_ONAIRTIME = cueData.detail[0].onairtime;
          cueData.activeWeekList = rowData.activeWeekList;
          cueData.cueid = rowData.cueid;
          cueData.weeks = rowData.weeks;
          cueData.productWeekList = rowData.productWeekList;
          //cueDataObj = cueData
          this.settingInfo(cueData);
          this.SET_CUEINFO(cueData);
          this.setCueConData(res.data);
          var dataVal = false;
          for (var i = 1; i < 5; i++) {
            res.data.instanceCon["channel_" + i].forEach((ele) => {
              if (ele.cartcode != null) {
                dataVal = true;
                return;
              }
            });
          }
          if (res.data.normalCon.length == 0 && !dataVal) {
            this.setSponsorList({
              pgmcode: rowData.pgmcode,
            });
          }
        });
      var params = {
        personid: userId,
        pgmcode: "",
        brd_dt: "",
      };
      await this.getCueDayFav(params);
      this.loadingVisible = false;
    },
    toggleChange(value) {
      if (value.length == 0) {
        this.setautosave({ ID: this.cueInfo.personid, CueSheetAutoSave: "N" });
        this.SET_CUESHEETAUTOSAVE(false);
      } else {
        this.setautosave({ ID: this.cueInfo.personid, CueSheetAutoSave: "Y" });
        this.SET_CUESHEETAUTOSAVE(true);
      }
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
          .insertBefore(document.getElementById("button_view"), null);
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
          .getElementById("left_top")
          .insertBefore(document.getElementById("button_view"), null);
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
    settingInfo(cueDataObj) {
      if (!cueDataObj.directorname || cueDataObj.directorname == "") {
        cueDataObj.directorname =
          this.proUserList.length < 20
            ? this.proUserList
            : this.proUserList.substr(0, 20);
      }
      if (!cueDataObj.headertitle || cueDataObj.headertitle == "") {
        cueDataObj.headertitle = cueDataObj.title;
      }
      if (!cueDataObj.footertitle || cueDataObj.footertitle == "") {
        cueDataObj.footertitle =
          "참여방법 : #8001번 단문 50원, 장문&포토문자 100원 / 미니 무료 / (03925)서울시 마포구 성암로 267";
      }
      return cueDataObj;
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
#left_top {
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
#button_view {
  /* width: 316px; */
  height: 30px;
  position: absolute;
  top: 0px;
  right: 0px;
}
.button_view_vertical {
  width: 50px;
  position: absolute;
  top: 0px;
  left: -68px;
  z-index: 5;
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
.modal_search {
  width: 700px;
  text-align: center;
  margin: auto;
}
.modal_search fieldset {
  display: inline-block;
}
.modal_week_form {
  margin-top: 10px;
  height: 150px;
  border: 1px solid #d7d7d7;
  display: table-cell;
  vertical-align: middle;
  width: 800px;
}
.modal_week_form .btn-outline-primary:disabled {
  border: solid 1px #757575;
  background-color: rgb(223, 222, 222);
  color: #757575;
}
/* loadPanel */
.dx-loadpanel-wrapper {
  font-family: "MBC 새로움 M";
  z-index: 6 !important;
}
</style>