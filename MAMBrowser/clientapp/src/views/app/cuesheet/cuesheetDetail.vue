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
                  방송 완료일 :
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
              <div class="button_view">
                <ButtonWidget :type="cueInfo.cuetype" />
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
                  <DxItem title="부가정보">
                    <template #default>
                      <div>
                        <AdditionalWidget :valueItems="tags" />
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
import ButtonWidget from "./ButtonWidget.vue";
import AbchannelWidget from "./AbchannelWidget.vue";
import PrintWidget from "./PrintWidget.vue";
import SortableWidget from "./C_SortableWidget.vue";
import AdditionalWidget from "./AdditionalInformationWidget.vue";
import DxTabPanel, { DxItem } from "devextreme-vue/tab-panel";
import { DxLoadPanel } from "devextreme-vue/load-panel";
import { eventBus } from "@/eventBus";
const qs = require("qs");

export default {
  beforeRouteLeave(to, from, next) {
    eventBus.$off();
    next();
  },
  components: {
    ButtonWidget,
    DxTabPanel,
    DxItem,
    PrintWidget,
    AbchannelWidget,
    SortableWidget,
    DxLoadPanel,
    AdditionalWidget,
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
    ...mapGetters("cueList", ["tags"]),
  },
  methods: {
    ...mapActions("cueList", ["setCueConData"]),
    ...mapActions("cueList", ["getProUserList"]),
    ...mapActions("cueList", ["getarchiveCuesheetCon"]),
    ...mapMutations("cueList", ["SET_CUEINFO"]),
    async getCueCon() {
      let rowData = JSON.parse(sessionStorage.getItem("USER_INFO"));
      var params = {
        cueid: rowData.cueid,
      };
      var archiveConData = await this.getarchiveCuesheetCon(params);
      if (archiveConData) {
        this.SET_CUEINFO(archiveConData.cueSheetDTO);
        this.setCueConData(archiveConData);
        this.getProUserList(rowData.productid);
        this.loadingVisible = false;
      } else {
        console.log("데이터를 가져올 수 없습니다."); //여기에 팝업창 뜨도록 확인해야함
      }
      // await this.$http
      //   .get(`/api/ArchiveCueSheet/GetArchiveCue`, {
      //     params: params,
      //     paramsSerializer: (params) => {
      //       return qs.stringify(params);
      //     },
      //   })
      //   .then((res) => {
      //     if (res.data.resultObject) {
      //     }
      //   });
    },
    nextOk() {
      this.nextgo = true;
    },
    onTextEdit() {
      this.$refs.inputText.focus();
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
.button_view {
  /* width: 280px; */
  height: 30px;
  position: absolute;
  top: 0px;
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