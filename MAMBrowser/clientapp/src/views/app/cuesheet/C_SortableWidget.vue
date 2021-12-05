<template>
  <div class="cartC_view">
    <DxSortable
      :data="fileData[index - 1]"
      group="tasksGroup"
      dropFeedbackMode="indicate"
      :allow-drop-inside-item="true"
      @add="onAdd($event, index)"
      @remove="onRemove($event, index)"
      v-for="index in widgetIndex"
      :key="index"
      class="cart_div"
      :class="sortableColor(index)"
    >
      <div style="height: 100%; border-radius: 10px">
        <div
          v-if="fileData[index - 1].cartcode != null"
          style="height: 100%; cursor: pointer"
        >
          <div class="top">
            <div style="font-size: 13px; padding-right: 5px; padding-left: 3px">
              {{ index }}
            </div>
            <div>
              <b-icon
                icon="box-seam"
                v-if="fileData[index - 1].cartcode == 'S01G01C011'"
              ></b-icon>
              <b-icon
                icon="trophy"
                v-if="fileData[index - 1].cartcode == 'S01G01C013'"
              ></b-icon>
              <b-icon
                icon="shield-check"
                v-if="fileData[index - 1].cartcode == 'S01G01C017'"
              ></b-icon>
              <b-icon
                icon="shield-lock"
                v-if="fileData[index - 1].cartcode == 'S01G01C010'"
              ></b-icon>
              <b-icon
                icon="shield-shaded"
                v-if="fileData[index - 1].cartcode == 'S01G01C018'"
              ></b-icon>
              <b-icon
                icon="shield-plus"
                v-if="fileData[index - 1].cartcode == 'S01G01C019'"
              ></b-icon>
              <b-icon
                icon="camera2"
                v-if="fileData[index - 1].cartcode == 'S01G01C012'"
              ></b-icon>
              <b-icon
                icon="hourglass"
                v-if="fileData[index - 1].cartcode == 'S01G01C021'"
              ></b-icon>
              <b-icon
                icon="hourglass-bottom"
                v-if="fileData[index - 1].cartcode == 'S01G01C022'"
              ></b-icon>
              <b-icon
                icon="hourglass-split"
                v-if="fileData[index - 1].cartcode == 'S01G01C023'"
              ></b-icon>
              <b-icon
                icon="hourglass-top"
                v-if="fileData[index - 1].cartcode == 'S01G01C024'"
              ></b-icon>
              <b-icon
                icon="camera-reels"
                v-if="fileData[index - 1].cartcode == 'S01G01C009'"
              ></b-icon>
              <b-icon
                icon="alarm"
                v-if="fileData[index - 1].cartcode == 'S01G01C016'"
              ></b-icon>
              <b-icon
                icon="clock"
                v-if="fileData[index - 1].cartcode == 'S01G01C020'"
              ></b-icon>
            </div>
            <div v-if="fileData[index - 1].startposition > 0">
              <b-icon icon="star"></b-icon>
            </div>
            <div
              v-if="
                fileData[index - 1].duration > fileData[index - 1].endposition
              "
            >
              <b-icon icon="star"></b-icon>
            </div>
            <div class="actionBtn">
              <b-icon
                icon="play-circle"
                class="btnIcon"
                @click="onPreview(fileData[index - 1])"
              ></b-icon>
              <b-icon
                icon="x-circle"
                class="btnIcon"
                @click="arrdelete(index)"
                v-if="cueInfo.cuetype != 'A'"
              ></b-icon>
            </div>
          </div>
          <div class="bottom">
            <div
              class="bottom_item maintitle"
              :class="{
                maintitle_red:
                  fileData[index - 1].onairdate != '' &&
                  (fileData[index - 1].onairdate != cueInfo.day ||
                    fileData[index - 1].onairdate != cueInfo.brddate),
              }"
              @dblclick="onTextEdit(index)"
              v-if="fileData[index - 1].edittarget"
            >
              {{ fileData[index - 1].maintitle }}
            </div>
            <div
              class="bottom_item"
              v-if="fileData[index - 1].edittarget == false"
            >
              <b-form-input
                :value="fileData[index - 1].maintitle"
                ref="inputText"
                spellcheck="false"
                @keyup.enter="
                  onValueChange($event, index, fileData[index - 1].maintitle)
                "
                @blur="onValueBlur(index)"
              />
            </div>
            <div
              class="bottom_item"
              v-if="searchToggleSwitch"
              style="font-size: 12px"
            >
              {{ fileData[index - 1].subtitle }}
            </div>
            <div class="bottom_item" style="font-size: 10px">
              {{
                $moment(
                  fileData[index - 1].endposition -
                    fileData[index - 1].startposition
                )
                  | moment("subtract", "9 hours")
                  | moment("HH:mm:ss")
              }}
            </div>
          </div>
        </div>
        <div v-else class="blankView">
          {{ index }}
        </div>
      </div>
    </DxSortable>
    <PlayerPopup
      :showPlayerPopup="showPlayerPopup"
      :title="soundItem.maintitle"
      :fileKey="soundItem.filetoken"
      :streamingUrl="streamingUrl"
      :waveformUrl="waveformUrl"
      :tempDownloadUrl="tempDownloadUrl"
      :rowNum="soundItem.rownum"
      :type="channelKey"
      :startPoint="soundItem.startposition"
      :endPoint="soundItem.endposition"
      requestType="token"
      @closePlayer="onClosePlayer"
    >
    </PlayerPopup>
  </div>
</template>

<script>
import { USER_ID } from "@/constants/config";
import { DxSortable } from "devextreme-vue/sortable";
import DxButton from "devextreme-vue/button";
import { mapActions, mapGetters, mapMutations } from "vuex";
import { eventBus } from "@/eventBus";
import MixinCommon from "../../../mixin/MixinCommon";

import "moment/locale/ko";
const moment = require("moment");

export default {
  mixins: [MixinCommon],

  props: {
    widgetIndex: Number,
    searchToggleSwitch: Boolean,
    channelKey: String,
  },
  data() {
    return {
      fileData: [],
      rowData: {
        cartcode: "", //그룹코드
        cartid: "", // 소재ID
        duration: "", //string
        edittarget: true,
        endposition: 0, //millisecond
        fadeintime: 0,
        fadeouttime: 0,
        filetoken: "", //미리듣기 때문 바뀔수도있음
        filepath: "",
        maintitle: "",
        memo: "", //바뀔수도있음
        onairdate: "",
        //rownum: 0,
        startposition: 0,
        subtitle: "",
        transtype: "S",
        useflag: "Y",
      },
    };
  },
  async created() {
    if (this.channelKey == "channel_my") {
      this.fileData = this.cueFavorites;
      var userId = sessionStorage.getItem(USER_ID);
      //즐겨찾기
      await this.getCueDayFav(userId);
      this.fileData = this.cueFavorites;
    } else {
      // 일반 C카트
      this.fileData = this.cChannelData[this.channelKey];
    }
    eventBus.$on("clearCData", (val) => {
      var arr = [];
      var resultData = { ...this.cChannelData };

      for (var i = 0; i < 16; i++) {
        arr.push({ rownum: i + 1 });
      }
      val.forEach((item) => {
        switch (item) {
          case "c1":
            resultData["channel_1"] = arr;
            if (this.channelKey == "channel_1") {
              this.fileData = arr;
            }
            break;
          case "c2":
            resultData["channel_2"] = arr;
            if (this.channelKey == "channel_2") {
              this.fileData = arr;
            }
            break;
          case "c3":
            resultData["channel_3"] = arr;
            if (this.channelKey == "channel_3") {
              this.fileData = arr;
            }
            break;
          case "c4":
            resultData["channel_4"] = arr;
            if (this.channelKey == "channel_4") {
              this.fileData = arr;
            }
            break;

          default:
            break;
        }
      });
      this.SET_CCHANNELDATA(resultData);
      //eventBus.$off("clearCData");
    });

    eventBus.$on("updateCData", (val) => {
      var resultData = { ...this.cChannelData };
      val.items.forEach((item) => {
        switch (item) {
          case "c1":
            resultData["channel_1"] = val.data["channel_1"];
            if (this.channelKey == "channel_1") {
              this.fileData = val.data["channel_1"];
            }
            break;
          case "c2":
            resultData["channel_2"] = val.data["channel_2"];
            if (this.channelKey == "channel_2") {
              this.fileData = val.data["channel_2"];
            }
            break;
          case "c3":
            resultData["channel_3"] = val.data["channel_3"];
            if (this.channelKey == "channel_3") {
              this.fileData = val.data["channel_3"];
            }
            break;
          case "c4":
            resultData["channel_4"] = val.data["channel_4"];
            if (this.channelKey == "channel_4") {
              this.fileData = val.data["channel_4"];
            }
            break;
          default:
            break;
        }
      });
      this.SET_CCHANNELDATA(resultData);
      //eventBus.$off("updateCData");
    });
    eventBus.$on("clearFav", (val) => {
      this.fileData = this.cueFavorites;
    });
  },
  components: { DxSortable, DxButton },
  computed: {
    ...mapGetters("cueList", ["searchListData"]),
    ...mapGetters("cueList", ["cChannelData"]),
    ...mapGetters("cueList", ["cueFavorites"]),
    ...mapGetters("cueList", ["proUserList"]),
    ...mapGetters("cueList", ["cueInfo"]),
    sortableColor() {
      return (index) => {
        return {
          backColor_1:
            this.channelKey == "channel_1" &&
            this.fileData[index - 1].cartcode != null,
          backColor_2:
            this.channelKey == "channel_2" &&
            this.fileData[index - 1].cartcode != null,
          backColor_3:
            this.channelKey == "channel_3" &&
            this.fileData[index - 1].cartcode != null,
          backColor_4:
            this.channelKey == "channel_4" &&
            this.fileData[index - 1].cartcode != null,
          backColor_my:
            this.channelKey == "channel_my" &&
            this.fileData[index - 1].cartcode != null,
        };
      };
    },
  },
  methods: {
    ...mapMutations("cueList", ["SET_CCHANNELDATA"]),
    ...mapMutations("cueList", ["SET_CUEFAVORITES"]),
    ...mapActions("cueList", ["getCueDayFav"]),
    ...mapActions("cueList", ["cartCodeFilter"]),
    ...mapActions("cueList", ["setInstanceCon"]),
    ...mapActions("cueList", ["sponsorDataFun"]),
    onAdd(e, totalIndex) {
      if (this.cueInfo.cuetype == "A") {
        return;
      }
      if (e.fromData === undefined) {
        var selectedRowsData = this.sortSelectedRowsData(e);
        if (selectedRowsData.length > 1) {
          selectedRowsData.forEach((data, index) => {
            var row = { ...this.rowData };
            var search_row = data;
            if (Object.keys(search_row).includes("subtitle")) {
              if (search_row.subtitle == "") {
                return;
              }
              row = { ...search_row };
              row.rownum = totalIndex + index;
              row.edittarget = true;
            } else {
              row.rownum = totalIndex + index;
              //row.filetoken = [];
              // row.filetoken.push(search_row.fileToken);
              // row.filepath = [];
              // row.filepath.push(search_row.filePath);
              row.filetoken = search_row.fileToken;
              row.filepath = search_row.filePath;
              if (!search_row.intDuration) {
                row.endposition = 0;
                row.duration = 0;
              } else {
                row.endposition = search_row.intDuration;
                row.duration = search_row.intDuration;
              }
              row.cartid = search_row.id;
              row.cartcode = this.searchListData.cartcode;
              this.cartCodeFilter({
                row: row,
                search_row: search_row,
              });
            }
            this.fileData.splice(totalIndex - 1 + index, 1, row);
          });
          this.fileData = this.fileData.slice(0, 16);
        } else {
          var row = { ...this.rowData };
          var search_row = e.itemData;
          if (e.fromData !== undefined) {
            search_row = e.fromData;
          }
          if (Object.keys(search_row).includes("subtitle")) {
            if (search_row.subtitle == "") {
              return;
            }
            row = { ...search_row };
            row.rownum = this.fileData[totalIndex - 1].rownum;
            row.edittarget = true;
          } else {
            row.rownum = this.fileData[totalIndex - 1].rownum;
            row.filetoken = search_row.fileToken;
            row.filepath = search_row.filePath;
            if (!search_row.intDuration) {
              row.endposition = 0;
              row.duration = 0;
            } else {
              row.endposition = search_row.intDuration;
              row.duration = search_row.intDuration;
            }
            row.cartid = search_row.id;
            row.cartcode = this.searchListData.cartcode;
            this.cartCodeFilter({
              row: row,
              search_row: search_row,
            });
          }
          this.fileData.splice(totalIndex - 1, 1, row);
        }
      } else {
        e.fromData.rownum = this.fileData[totalIndex - 1].rownum;
        this.fileData.splice(totalIndex - 1, 1, e.fromData);
      }
      if (this.channelKey == "channel_my") {
        this.SET_CUEFAVORITES(this.fileData);
      } else {
        var resultData = { ...this.cChannelData };
        resultData[this.channelKey] = this.fileData;
        this.SET_CCHANNELDATA(resultData);
        console.log("this.cChannelData");
        console.log(this.cChannelData);
        // this.setInstanceCon({
        //   fileData: this.fileData,
        //   channelKey: this.channelKey,
        // });
        // var cChannelDataArr = this.cChannelData;
        // cChannelDataArr[this.channelKey] = this.fileData;
        // // this.SET_CCHANNELDATA({ type: this.channelKey, value: this.fileData });
        // //this.SET_CCHANNELDATA(cChannelDataArr);
      }
    },
    sortSelectedRowsData(e) {
      var selectedRowsData = e.fromComponent.getSelectedRowsData();
      if (selectedRowsData.length == 0) {
        return (selectedRowsData = []);
      } else if (Object.keys(selectedRowsData[0]).includes("rowNO")) {
        selectedRowsData.sort(function (a, b) {
          if (a.rowNO > b.rowNO) {
            return 1;
          }
          if (a.rowNO < b.rowNO) {
            return -1;
          }
          return 0;
        });
        return selectedRowsData;
      } else {
        selectedRowsData.forEach((selectindex) => {
          var index = e.fromComponent.getRowIndexByKey(selectindex.rownum);
          selectindex.rownum = index;
        });
        selectedRowsData.sort(function (a, b) {
          if (a.rownum > b.rownum) {
            return 1;
          }
          if (a.rownum < b.rownum) {
            return -1;
          }
          return 0;
        });
        selectedRowsData.forEach((selectindex) => {
          var index = e.fromComponent.getKeyByRowIndex(selectindex.rownum);
          selectindex.rownum = index;
        });
        return selectedRowsData;
      }
    },
    onRemove($event, index) {
      if (this.cueInfo.cuetype == "A") {
        return;
      }
      if ($event.toData != undefined) {
        // var rowData = { rownum: this.fileData[index - 1].rownum };
        this.fileData.splice(index - 1, 1, { rownum: index });
      }

      //AB채널에 넣을때 복사되게하려고
      // if ($event.dropInsideItem) {
      //   if (Object.keys($event.toData).includes("subtitle")) {
      //     this.fileData[index - 1] = $event.toData;
      //   } else {
      //     this.fileData.splice(index - 1, 1, {});
      //   }
      // }
      // if (this.channelKey == "channel_my") {
      //   //즐겨찾기 부분
      //   this.SET_CUEFAVORITES(this.fileData);
      // } else {
      //   this.SET_CCHANNELDATA({ type: this.channelKey, value: this.fileData });
      // }
    },
    onTextEdit(index) {
      if (this.cueInfo.cuetype == "A") {
        return;
      }
      this.fileData[index - 1].edittarget = false;
      this.$nextTick(() => {
        this.$refs.inputText[0].focus();
      });
      // if (this.channelKey == "channel_my") {
      //   //즐겨찾기 부분
      //   this.SET_CUEFAVORITES(this.fileData);
      // } else {
      //   this.SET_CCHANNELDATA({ type: this.channelKey, value: this.fileData });
      // }
    },
    onValueChange($event, index, data) {
      this.fileData[index - 1].maintitle = $event.target.value;
      if (this.fileData[index - 1].maintitle == "") {
        this.fileData[index - 1].maintitle = data;
      }
      this.fileData[index - 1].edittarget = true;
      // if (this.channelKey == "channel_my") {
      //   //즐겨찾기 부분
      // } else {
      //   this.SET_CCHANNELDATA({ type: this.channelKey, value: this.fileData });
      // }
    },
    onValueBlur(index) {
      this.fileData[index - 1].edittarget = true;
      // if (this.channelKey == "channel_my") {
      //   //즐겨찾기 부분
      //   this.SET_CUEFAVORITES(this.fileData);
      // } else {
      //   this.SET_CCHANNELDATA({ type: this.channelKey, value: this.fileData });
      // }
    },
    arrdelete(index) {
      this.fileData.splice(index - 1, 1, { rownum: index });
      // if (this.channelKey == "channel_my") {
      //   //즐겨찾기 부분
      //   this.SET_CUEFAVORITES(this.fileData);
      // } else {
      //   this.SET_CCHANNELDATA({ type: this.channelKey, value: this.fileData });
      // }
    },
    //시간 string > milliseconds
    millisecondsFuc(duration) {
      var itemTime = moment(duration, "HH:mm:ss.SS");
      var defTime = moment("00:00:00.0", "HH:mm:ss.SS");
      return moment.duration(itemTime.diff(defTime)).asMilliseconds();
    },
  },
};
</script>

<style >
.cartC_view {
  padding: 10px;
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  grid-template-rows: repeat(4, 1fr);
  align-items: stretch;
  gap: 10px;
}
.cart_div {
  background-color: #ededed;
  /* background-color: #008ecc; */
  /* opacity: 0.7; */
  overflow: auto;
  border-radius: 5px;

  /* background-color: #EFF0F2; */
  /* border-radius: 10px; */
}
/* .sortableView {
  background: linear-gradient(45deg, #f5709d, #f0a39a);
  background: linear-gradient(45deg, #f791b1, #ed9671);
  box-shadow: rgba(60, 64, 67, 0.3) 0px 1px 2px 0px,
    rgba(60, 64, 67, 0.15) 0px 2px 6px 2px !important;
  color: white;
  padding: 5px;
  box-shadow: rgba(14, 30, 37, 0.12) 0px 2px 4px 0px,
    rgba(14, 30, 37, 0.32) 0px 2px 16px 0px !important;
} */
.backColor_1 {
  padding: 5px;

  box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  background-color: white;

  /* color: white;
  box-shadow: rgba(14, 30, 37, 0.12) 0px 2px 4px 0px,
    rgba(14, 30, 37, 0.32) 0px 2px 16px 0px !important;
  background: linear-gradient(45deg, #23bfd3, #5177d0); */

  /* background: linear-gradient(45deg, #4fc9da, #7293d8); */
  /* background: linear-gradient(45deg, #e8dae3, white); */
  /* background-color: #b0b0b0; */
}
.backColor_2 {
  color: white;
  padding: 5px;
  /* box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  background-color: white; */

  box-shadow: rgba(14, 30, 37, 0.12) 0px 2px 4px 0px,
    rgba(14, 30, 37, 0.32) 0px 2px 16px 0px !important;
  /* background: linear-gradient(45deg, #896de3, #407ac2); */
  background: linear-gradient(45deg, #9f8ae8, #6995cf);
}
.backColor_3 {
  color: white;
  padding: 5px;

  /* box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  background-color: white; */

  box-shadow: rgba(14, 30, 37, 0.12) 0px 2px 4px 0px,
    rgba(14, 30, 37, 0.32) 0px 2px 16px 0px !important;
  /* background: linear-gradient(45deg, #f3b128, #ef8256); */
  /* background: linear-gradient(45deg, #f29d77, #f5be55); */

  background: linear-gradient(45deg, #4fc9da, #7293d8);
}
.backColor_4 {
  color: white;
  padding: 5px;

  /* box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  background-color: white; */

  box-shadow: rgba(14, 30, 37, 0.12) 0px 2px 4px 0px,
    rgba(14, 30, 37, 0.32) 0px 2px 16px 0px !important;
  /* background: linear-gradient(45deg, #eb4282, #b3509c); */
  background: linear-gradient(45deg, #ec699d, #c272af);
}
.backColor_my {
  color: white !important;
  padding: 5px;

  /* box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  background-color: white; */

  box-shadow: rgba(14, 30, 37, 0.12) 0px 2px 4px 0px,
    rgba(14, 30, 37, 0.32) 0px 2px 16px 0px !important;
  /* background: linear-gradient(45deg, #f3825b, #f56f86); */
  background: linear-gradient(45deg, #f58e6e, #f57f90);
}
.top {
  width: 100%;
  height: 23px;
  font-size: 11px;
}
.top div {
  display: inline-block;
}
.actionBtn {
  float: right;
}
.blankView {
  /* height: 100%; */
  color: white;
  font-size: 40px;
  padding-left: 8px;
}
.bottom {
  padding-left: 5px;
  height: 83%;
  display: flex;
  align-content: center;
  flex-direction: column;
  flex-wrap: nowrap;
}
.bottom_item:nth-child(1) {
  padding-top: 10px;
  flex: 1.5;
  font-size: 110%;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  width: 100%;
}
.bottom_item:nth-child(1) input {
  height: 30px;
  font-size: 15px;
}
.bottom_item:nth-child(2) {
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  width: 100%;
  flex: 1;
}
.bottom_item:nth-child(3) {
  flex: 1;
}
.cartC_view_search_toggle_on .top {
  height: 20px;
}
.cartC_view_search_toggle_on .bottom {
  height: 70%;
}
.cartC_view_search_toggle_on .bottom_item:nth-child(1) {
  padding-top: 0px;
  font-size: 110%;
  flex: 1.5;
}
.cartC_view_search_toggle_on .bottom_item:nth-child(1) input {
  height: 25px;
  font-size: 15px;
}
.btnIcon {
  font-size: 18px;
  padding-left: 2px;
}
.MusicIcon {
  font-size: 18px;
}
.btnIcon:hover {
  color: #2a4878;
  cursor: pointer;
}
.maintitle_red {
  color: red;
}
</style>