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
                  방송 예정일 :
                  <span>{{
                    $moment(cueInfo.brdtime).format("YYYY-MM-DD")
                  }}</span>
                </span>
                <span class="sub_text">
                  <span class="subtitle_css">●</span>
                  담당자 :
                  <span>{{ proUserList }}</span>
                </span>
                <span class="sub_text">
                  <span class="subtitle_css">●</span>
                  최종 편집 일시 :
                  <span>{{
                    cueInfo.edittime == null
                      ? ""
                      : $moment(cueInfo.edittime).format("YYYY-MM-DD")
                  }}</span>
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
                          :widgetIndex="widgetIndex"
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
                          :widgetIndex="widgetIndex"
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
                          :widgetIndex="widgetIndex"
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
                          :widgetIndex="widgetIndex"
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
import { mapActions, mapGetters, mapMutations } from "vuex";
import { USER_ID } from "@/constants/config";
import SearchWidget from "./SearchWidget.vue";
import ButtonWidget from "./ButtonWidget.vue";
import AbchannelWidget from "./AbchannelWidget.vue";
import PrintWidget from "./PrintWidget.vue";
import SortableWidget from "./C_SortableWidget.vue";
import DxTabPanel, { DxItem } from "devextreme-vue/tab-panel";
import DxSpeedDialAction from "devextreme-vue/speed-dial-action";
import { DxLoadPanel } from "devextreme-vue/load-panel";
import { eventBus } from "@/eventBus";
import "moment/locale/ko";
const moment = require("moment");
const qs = require("qs");

export default {
  beforeRouteLeave(to, from, next) {
    if (this.timer > 1) {
      const answer = window.confirm(
        "저장하지 않은 데이터는 손실됩니다. 현재 페이지를 벗어나시겠습니까?"
      );
      if (answer) {
        eventBus.$off();
        next();
      }
    } else {
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
      onload: null,
      searchToggleSwitch: true,
      printHeight: 560,
      abChannelHeight: 734,
      widgetIndex: 16,
    };
  },
  async created() {
    this.loadingVisible = true;
    //큐시트 상세내용 가져오기
    await this.getCueCon();
  },
  computed: {
    ...mapGetters("cueList", ["cueInfo"]),
    ...mapGetters("cueList", ["proUserList"]),
    ...mapGetters("cueList", ["defCuesheetListArr"]),
    ...mapGetters("user", ["timer"]),
  },
  methods: {
    ...mapActions("cueList", ["saveDayCue"]),
    ...mapActions("cueList", ["getProUserList"]),
    ...mapActions("cueList", ["setCueConData"]),
    ...mapActions("cueList", ["setclearCon"]),
    ...mapActions("cueList", ["setSponsorList"]),
    ...mapActions("cueList", ["getcuesheetListArrDef"]),
    ...mapMutations("cueList", ["SET_CUEINFO"]),
    ...mapActions("cueList", ["getCueDayFav"]),
    //상세내용 가져오기
    async getCueCon() {
      let rowData = JSON.parse(sessionStorage.getItem("USER_INFO"));
      const userId = sessionStorage.getItem(USER_ID);
      var params = {
        productid: rowData.productid,
        pgmcode: rowData.pgmcode,
        brd_dt: rowData.day,
      };
      let cueDataObj = {
        detail: [{ cueid: -1 }],
        brddate: rowData.day,
        brdtime: rowData.r_ONAIRTIME,
        cuetype: "D",
        title: rowData.eventname,
        media: rowData.media,
        productid: rowData.productid,
        personid: userId,
        pgmcode: rowData.pgmcode,
        //구DB 및 기타 데이터
        //여기 추가하던 중이엇음 내일 와가지고 이거 테스트 해봐야함
        day: rowData.day,
        liveflag: rowData.liveflag,
        onairday: rowData.onairday,
        r_ONAIRTIME: rowData.r_ONAIRTIME,
        seqnum: rowData.seqnum,
        startdate: rowData.startdate,
        day: rowData.day,
      };
      await this.$http
        .get(`/api/daycuesheet/GetDayCue`, {
          params: params,
          paramsSerializer: (params) => {
            return qs.stringify(params);
          },
        })
        .then(async (res) => {
          await this.getProUserList(rowData.productid);
          if (!res.data.resultObject) {
            //작성된 기본큐시트 있는지 확인
            var defCueId = [];
            await this.getcuesheetListArrDef({
              productids: rowData.productid,
              row_per_page: 300,
              select_page: 1,
            });
            var defCueList = this.defCuesheetListArr;
            defCueList.data.forEach((ele) => {
              var result = ele.detail.filter((v) => {
                return (
                  v.week ==
                  moment(rowData.r_ONAIRTIME, "YYYY-MM-DD'T'HH:mm:ss")
                    .lang("en")
                    .format("ddd")
                    .toUpperCase()
                );
              });
              if (result.length > 0) {
                defCueId = result;
              }
            });
            if (defCueId.length > 0) {
              //기본큐시트 가져오기
              var params = {
                productid: rowData.productid,
                week: defCueId[0].week,
                pgmcode: rowData.pgmcode,
                brd_dt: rowData.day,
              };
              await this.$http
                .get(`/api/defcuesheet/GetdefCue`, {
                  params: params,
                  paramsSerializer: (params) => {
                    return qs.stringify(params);
                  },
                })
                .then((res) => {
                  var defcueData = res.data.resultObject.cueSheetDTO;
                  cueDataObj.r_ONAIRTIME = defcueData.detail[0].onairtime;
                  cueDataObj.directorname = defcueData.directorname;
                  cueDataObj.djname = defcueData.djname;
                  cueDataObj.footertitle = defcueData.footertitle;
                  cueDataObj.headertitle = defcueData.headertitle;
                  cueDataObj.membername = defcueData.membername;
                  cueDataObj.memo = defcueData.memo;
                  this.settingInfo(cueDataObj);
                  this.SET_CUEINFO(cueDataObj);
                  this.setCueConData(res.data.resultObject);
                });
            } else {
              //큐시트 작성
              this.settingInfo(cueDataObj);
              this.SET_CUEINFO(cueDataObj);
              this.setclearCon();
              await this.setSponsorList({
                pgmcode: rowData.pgmcode,
                brd_dt: rowData.day,
              });
            }
          } else {
            //큐시트 수정 데이터 채움
            var dayCueData = res.data.resultObject.cueSheetDTO;
            dayCueData.liveflag = cueDataObj.liveflag;
            dayCueData.onairday = cueDataObj.onairday;
            dayCueData.seqnum = cueDataObj.seqnum;
            dayCueData.startdate = cueDataObj.startdate;
            dayCueData.day = cueDataObj.day;
            this.settingInfo(dayCueData);
            this.SET_CUEINFO(dayCueData);
            this.setCueConData(res.data.resultObject);
          }
          //즐겨찾기 가져오기
          var params = {
            personid: userId,
            pgmcode: "",
            brd_dt: "",
          };
          await this.getCueDayFav(params);
          this.loadingVisible = false;
        });
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
    nextOk() {
      this.nextgo = true;
    },
    onTextEdit() {
      this.$refs.inputText.focus();
    },
    searchToggleEvent() {
      // var parent = document.getElementById("overView");
      //var content = content.parentNode;
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
/* loadPanel */
.dx-loadpanel-wrapper {
  font-family: "MBC 새로움 M";
  z-index: 6 !important;
}
</style>