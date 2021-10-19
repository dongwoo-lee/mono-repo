<template>
  <div id="overView">
    <b-row style="marin-top: -10px">
      <b-card class="w-100">
        <div class="detail_view">
          <div class="left_view">
            <div class="left_top" v-show="searchToggleSwitch">
              <div class="listTitle mb-3">
                <piaf-breadcrumb />
              </div>
              <div class="MainTilte">
                <h1>{{ cuesheetData.eventname }}</h1>
              </div>
              <div class="separator mb-3 mt-0"></div>
              <div class="subtitle">
                <span class="sub_text">
                  <span class="subtitle_css">●</span>
                  매체 :
                  <span v-if="cuesheetData.media == 'A'">AM</span>
                  <span v-if="cuesheetData.media == 'F'">FM</span>
                  <span v-if="cuesheetData.media == 'D'">DMB</span>
                  <span v-if="cuesheetData.media == 'C'">공통</span>
                  <span v-if="cuesheetData.media == 'Z'">기타</span>
                </span>
                <span class="sub_text">
                  <span class="subtitle_css">●</span>
                  담당자 :
                  <span>{{ directors }}</span>
                </span>
                <span class="sub_text">
                  <span class="subtitle_css">●</span>
                  수정일 :
                  <span> {{ cuesheetData.edittime }} </span>
                </span>
                <span class="sub_text">
                  <span class="subtitle_css">●</span>
                  적용요일 :
                  <span style="display: inline-block">
                    <common-weeks
                      :rowData="weekData.rowData"
                      :productWeekList="weekData.productWeekList"
                      :edit="true"
                    >
                    </common-weeks>
                  </span>
                </span>
                <span class="autosave">
                  <b-form-checkbox-group
                    :options="options"
                    switches
                    style="float: right"
                  ></b-form-checkbox-group>
                </span>
              </div>
              <div class="button_view">
                <ButtonWidget :cuesheetData="cuesheetData" />
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
                          :colorValue="(colorValue = 1)"
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
                          :colorValue="(colorValue = 2)"
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
                          :colorValue="(colorValue = 3)"
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
                          :colorValue="(colorValue = 4)"
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
                          :colorValue="(colorValue = 0)"
                          channelKey="channel_my"
                        />
                      </div>
                    </template>
                  </DxItem>
                  <!-- <DxItem title="부가정보">
                    <template #default>
                      <div>ddd</div>
                    </template>
                  </DxItem> -->
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
    <DxSpeedDialAction icon="search" @click="searchToggleEvent" />
  </div>
</template>

<script>
import { mapActions, mapGetters, mapMutations } from "vuex";
import SearchWidget from "./SearchWidget.vue";
import ButtonWidget from "./ButtonWidget.vue";
import AbchannelWidget from "./AbchannelWidget.vue";
import PrintWidget from "./PrintWidget.vue";
import SortableWidget from "./C_SortableWidget.vue";
import DxTabPanel, { DxItem } from "devextreme-vue/tab-panel";
import DxSpeedDialAction from "devextreme-vue/speed-dial-action";
import axios from "axios";
import CommonWeeks from "../../../components/DataTable/CommonWeeks.vue";

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
    CommonWeeks,
  },
  data() {
    return {
      directors: "", //수정불가 directors
      cuesheetData: {
        brddate: "",
        brdtime: "",
        cueid: -1,
        cuetype: "",
        directorname: "", //수정가능
        djname: "",
        edittime: "",
        eventname: "",
        footertitle: "참여방법: #8001번 단문 50원",
        headertitle: "",
        media: "",
        membername: "",
        memo: "",
        productid: "",
        servicename: "",
        startdate: "",
        week: "",
      },
      options: [{ text: "자동저장", value: false }],
      searchToggleSwitch: true,
      printHeight: 560,
      abChannelHeight: 734,
    };
  },
  async mounted() {
    document.getElementById("app-container").classList.add("drag_");
  },
  created() {
    this.cuesheetData = Object.assign(this.cuesheetData, this.seleDayCue);
    if (this.cuesheetData.directorname == "") {
      this.getProUserList(this.cuesheetData.productid);
    }
    console.log(this.weekData);
  },
  computed: {
    ...mapGetters("cuesheet", ["seleDayCue"]),
    ...mapGetters("cuesheet", ["weekData"]),
  },
  methods: {
    getProUserList(productid) {
      axios
        .get(`/api/CueUserInfo/GetDirectorList?productid=` + productid)
        .then((res) => {
          this.directors = res.data;
          this.cuesheetData.directorname = res.data;
        });
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
.drag_ {
  position: fixed;
  height: 100%;
  overflow: auto;
}
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
  width: 280px;
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
</style>