<template>
  <div>
    <DxLoadPanel :visible.sync="loadingVisible" :position="position" />
    <div class="cartC_view">
      <DxSortable
        :data="fileData[index - 1]"
        group="tasksGroup"
        dropFeedbackMode="indicate"
        :allow-drop-inside-item="true"
        @add="onAdd($event, index)"
        @remove="onRemove($event, index)"
        :on-drag-start="onDragStart"
        :on-drag-end="onDragEnd"
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
              <div class="indexNumber">
                <b>
                  {{ index }}
                </b>
              </div>
              <div class="product_icon" style="width: 20px">
                <i
                  class="iconsminds-shop"
                  v-if="fileData[index - 1].cartcode == 'S01G01C007'"
                >
                </i>
                <i
                  class="iconsminds-big-data"
                  v-if="fileData[index - 1].cartcode == 'S01G01C006'"
                ></i>
                <i
                  class="iconsminds-cd-2"
                  v-if="
                    fileData[index - 1].cartcode == 'S01G01C014' ||
                    fileData[index - 1].cartcode == 'S01G01C015'
                  "
                ></i>
                <i
                  class="iconsminds-coins"
                  v-if="
                    fileData[index - 1].cartcode == 'S01G01C017' ||
                    fileData[index - 1].cartcode == 'S01G01C016' ||
                    fileData[index - 1].cartcode == 'S01G01C018' ||
                    fileData[index - 1].cartcode == 'S01G01C019'
                  "
                ></i>
                <i
                  class="iconsminds-film"
                  v-if="
                    fileData[index - 1].cartcode == 'S01G01C009' ||
                    fileData[index - 1].cartcode == 'S01G01C010' ||
                    fileData[index - 1].cartcode == 'S01G01C012' ||
                    fileData[index - 1].cartcode == 'S01G01C013'
                  "
                ></i>
                <i
                  class="iconsminds-engineering"
                  v-if="
                    fileData[index - 1].cartcode == 'S01G01C020' ||
                    fileData[index - 1].cartcode == 'S01G01C021' ||
                    fileData[index - 1].cartcode == 'S01G01C022' ||
                    fileData[index - 1].cartcode == 'S01G01C023' ||
                    fileData[index - 1].cartcode == 'S01G01C024'
                  "
                ></i>
              </div>
              <div style="width: 15px">
                <div
                  v-if="
                    !fileData[index - 1].fadeintime &&
                    fileData[index - 1].startposition > 0
                  "
                >
                  <b-icon icon="screwdriver"></b-icon>
                </div>
                <div
                  v-if="
                    fileData[index - 1].fadeintime &&
                    !fileData[index - 1].startposition > 0
                  "
                >
                  <b-icon
                    style="transform: rotate(90deg)"
                    icon="wrench"
                  ></b-icon>
                </div>
                <div
                  v-if="
                    fileData[index - 1].fadeintime &&
                    fileData[index - 1].startposition > 0
                  "
                >
                  <b-icon icon="tools"></b-icon>
                </div>
              </div>
              <div style="width: 15px">
                <div
                  v-if="
                    !fileData[index - 1].fadeouttime &&
                    fileData[index - 1].duration >
                      fileData[index - 1].endposition
                  "
                >
                  <b-icon icon="screwdriver"></b-icon>
                </div>
                <div
                  v-if="
                    fileData[index - 1].fadeouttime &&
                    (!fileData[index - 1].duration >
                      fileData[index - 1].endposition ||
                      fileData[index - 1].duration ==
                        fileData[index - 1].endposition)
                  "
                >
                  <b-icon
                    style="transform: rotate(90deg)"
                    icon="wrench"
                  ></b-icon>
                </div>
                <div
                  v-if="
                    fileData[index - 1].fadeouttime &&
                    fileData[index - 1].duration >
                      fileData[index - 1].endposition
                  "
                >
                  <b-icon icon="tools"></b-icon>
                </div>
              </div>
              <div class="actionBtn">
                <DxButton
                  icon="music"
                  type="default"
                  hint="미리듣기/음원편집"
                  @click="onPreview(fileData[index - 1])"
                  v-if="
                    fileData[index - 1].onairdate == '' &&
                    fileData[index - 1].filepath != null &&
                    fileData[index - 1].filepath != ''
                  "
                />
                <DxButton
                  icon="music"
                  type="success"
                  hint="그룹 미리듣기"
                  v-if="fileData[index - 1].onairdate != ''"
                  @click="
                    showGrpPlayerPopup({
                      grpType:
                        fileData[index - 1].cartcode == 'S01G01C017' ||
                        fileData[index - 1].cartcode == 'S01G01C016'
                          ? 'sb'
                          : 'cm',
                      brd_Dt: fileData[index - 1].onairdate,
                      grpId: fileData[index - 1].cartid,
                      title: fileData[index - 1].maintitle,
                    })
                  "
                />
                <DxButton
                  icon="remove"
                  type="danger"
                  styling-mode="outlined"
                  hint="소재삭제"
                  @click="arrdelete(index)"
                  v-if="cueInfo.cuetype != 'A'"
                />
              </div>
            </div>
            <div class="bottom">
              <div
                class="bottom_item maintitle"
                :class="{
                  maintitle_red:
                    fileData[index - 1].onairdate != '' &&
                    cueInfo.cuetype != 'A' &&
                    (fileData[index - 1].onairdate != cueInfo.brddate ||
                      cueInfo.pgmcode != fileData[index - 1].pgmcode),
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
      <CMGroupPlayerPopup
        :showPlayerPopup="showGrpPlayer"
        :title="grpParam.title"
        :grpType="grpParam.grpType"
        :brd_Dt="grpParam.brd_Dt"
        :grpId="grpParam.grpId"
        @closePlayer="closeGrpPlayerPopup"
      >
      </CMGroupPlayerPopup>

      <EditPlayerPopup
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
        :fadeIn="soundItem.fadeintime"
        :fadeOut="soundItem.fadeouttime"
        requestType="token"
        @closePlayer="onClosePlayer"
      >
      </EditPlayerPopup>
    </div>
  </div>
</template>

<script>
import { USER_ID } from "@/constants/config";
import { DxSortable } from "devextreme-vue/sortable";
import DxButton from "devextreme-vue/button";
import { DxLoadPanel } from "devextreme-vue/load-panel";
import { mapActions, mapGetters, mapMutations } from "vuex";
import { eventBus } from "@/eventBus";
import MixinCommon from "../../../mixin/MixinCommon";
import axios from "axios";

import "moment/locale/ko";
const moment = require("moment");
const date = new Date();

function get_date_str(date) {
  var sYear = date.getFullYear();
  var sMonth = date.getMonth() + 1;
  var sDate = date.getDate();

  sMonth = sMonth > 9 ? sMonth : "0" + sMonth;
  sDate = sDate > 9 ? sDate : "0" + sDate;

  return sYear + "" + sMonth + "" + sDate;
}

var toDay = get_date_str(date);

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
      position: { of: ".cartC_view" },
      loadingVisible: false,
      showGrpPlayer: false,
      groupFilterVal: false,
      grpParam: {},
      rowData: {
        carttype: "",
        cartcode: "", //그룹코드
        cartid: "", // 소재ID
        duration: "", //string
        edittarget: true,
        endposition: 0, //millisecond
        fadeintime: false,
        fadeouttime: false,
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
      var params = {
        personid: userId,
        pgmcode: this.cueInfo.pgmcode,
      };
      if (this.cueInfo.cuetype == "D") {
        if (Object.keys(this.cueInfo).includes("detail")) {
          params.brd_dt = this.cueInfo.brddate;
        } else {
          params.brd_dt = this.cueInfo.day;
        }
      }
      //즐겨찾기
      await this.getCueDayFav(params);
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
          default:
            break;
        }
      });
      this.SET_CCHANNELDATA(resultData);
    });
    eventBus.$on("clearFav", (val) => {
      this.fileData = this.cueFavorites;
    });
  },
  components: { DxSortable, DxButton, DxLoadPanel },
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
    async onAdd(e, totalIndex) {
      this.loadingVisible = true;
      this.groupFilterVal = false;
      //아카이브 수정불가
      if (this.cueInfo.cuetype == "A") {
        this.loadingVisible = false;
        return;
      }
      if (e.fromData === undefined) {
        var selectedRowsData = this.sortSelectedRowsData(e);
        //ab > c 빈칸 제거
        selectedRowsData = selectedRowsData.filter((ele) => {
          if (Object.keys(ele).includes("cartcode")) {
            return ele.cartcode != "";
          } else {
            return (ele = ele);
          }
        });
        if (selectedRowsData.length > 1) {
          var index = 0;
          for (const data of selectedRowsData) {
            var row = { ...this.rowData };
            var search_row = data;
            if (Object.keys(search_row).includes("subtitle")) {
              if (search_row.subtitle == "") {
                this.loadingVisible = false;
                return;
              }
              row = { ...search_row };
              row.rownum = totalIndex + index;
              row.edittarget = true;
            } else {
              row.rownum = totalIndex + index;
              //테스트 중
              // if (this.searchListData.cartcode == "S01G01C021") {
              //   search_row = await axios
              //     .post(`/api/SearchMenu/test`)
              //     .then((res) => {
              //       return { filetoken: "ddd", filepath: "dddd" };
              //     });
              // }
              if (this.searchListData.cartcode == "S01G01C014") {
                search_row = await axios
                  .post(`/api/SearchMenu/GetSongItem`, search_row)
                  .then((res) => {
                    return res.data;
                  });
              }
              if (this.searchListData.cartcode == "S01G01C015") {
                search_row = await axios
                  .post(`/api/SearchMenu/GetEffectItem`, search_row)
                  .then((res) => {
                    return res.data;
                  });
              }
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
            var filterVal = this.groupFilter(row);
            if (filterVal) {
              this.fileData.splice(totalIndex - 1 + index, 1, row);
            } else {
              totalIndex--;
            }
            index++;
          }
          this.fileData = this.fileData.slice(0, 16);
        } else {
          var row = { ...this.rowData };
          var search_row = e.itemData;
          if (e.fromData !== undefined) {
            search_row = e.fromData;
          }
          if (Object.keys(search_row).includes("subtitle")) {
            if (search_row.subtitle == "") {
              this.loadingVisible = false;
              return;
            }
            row = { ...search_row };
            row.rownum = this.fileData[totalIndex - 1].rownum;
            row.edittarget = true;
          } else {
            row.rownum = this.fileData[totalIndex - 1].rownum;
            //테스트 중
            // if (this.searchListData.cartcode == "S01G01C021") {
            //   search_row = await axios
            //     .post(`/api/SearchMenu/test`)
            //     .then((res) => {
            //       return { filetoken: "ddd", filepath: "dddd" };
            //     });
            // }
            if (this.searchListData.cartcode == "S01G01C014") {
              search_row = await axios
                .post(`/api/SearchMenu/GetSongItem`, search_row)
                .then((res) => {
                  return res.data;
                });
            }
            if (this.searchListData.cartcode == "S01G01C015") {
              search_row = await axios
                .post(`/api/SearchMenu/GetEffectItem`, search_row)
                .then((res) => {
                  return res.data;
                });
            }
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
          var filterVal = this.groupFilter(row);
          if (filterVal) {
            this.fileData.splice(totalIndex - 1, 1, row);
          }
        }
      } else {
        e.fromData.rownum = this.fileData[totalIndex - 1].rownum;
        var filterVal = this.groupFilter(e.fromData);
        if (filterVal) {
          this.fileData.splice(totalIndex - 1, 1, e.fromData);
        }
      }
      if (this.channelKey == "channel_my") {
        this.SET_CUEFAVORITES(this.fileData);
      } else {
        var resultData = { ...this.cChannelData };
        resultData[this.channelKey] = this.fileData;
        this.SET_CCHANNELDATA(resultData);
      }
      if (this.groupFilterVal) {
        window.$notify(
          "error",
          `CM, SB 소재는 즐겨찾기에 추가할 수 없습니다.`,
          "",
          {
            duration: 10000,
            permanent: false,
          }
        );
      }
      this.loadingVisible = false;
    },
    //여기 하는중
    groupFilter(row) {
      var result = true;
      if (
        this.channelKey == "channel_my" &&
        row.onairdate != "" &&
        row.cartcode != null
      ) {
        result = false;
        this.groupFilterVal = true;
      }
      return result;
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
      if ($event.toData) {
        if (!$event.toData.cartcode) {
          this.fileData.splice(index - 1, 1, { rownum: index });
        } else {
          var data = $event.toData;
          data.rownum = index;
          this.fileData.splice(index - 1, 1, data);
        }
      }
    },
    onDragStart() {
      document.getElementById("app-container").classList.add("drag_");
    },
    onTextEdit(index) {
      if (this.cueInfo.cuetype == "A") {
        return;
      }
      this.fileData[index - 1].edittarget = false;
      this.$nextTick(() => {
        this.$refs.inputText[0].focus();
      });
    },
    onValueChange($event, index, data) {
      this.fileData[index - 1].maintitle = $event.target.value;
      if (this.fileData[index - 1].maintitle == "") {
        this.fileData[index - 1].maintitle = data;
      }
      this.fileData[index - 1].edittarget = true;
    },
    onValueBlur(index) {
      this.fileData[index - 1].edittarget = true;
    },
    arrdelete(index) {
      this.fileData.splice(index - 1, 1, { rownum: index });
    },
    showGrpPlayerPopup(data) {
      this.grpParam = data;
      this.showGrpPlayer = true;
    },
    closeGrpPlayerPopup() {
      this.showGrpPlayer = false;
    },
    onDragEnd() {
      document.getElementById("app-container").classList.remove("drag_");
    },
  },
};
</script>

<style >
.drag_ {
  position: fixed;
  height: 100%;
  overflow: auto;
}
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
.actionBtn .dx-button-content {
  width: 20px;
  height: 20px;
  padding: 0;
}
.product_icon i {
  font-size: 14px;
}
.indexNumber {
  text-align: center;
  font-size: 15px;
  padding-right: 5px;
  padding-left: 3px;
  width: 28px;
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
  border: solid 3px rgb(211, 145, 145);
  box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  background-color: white;
}
.backColor_2 {
  padding: 5px;
  border: solid 3px rgb(153, 211, 145);

  box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  background-color: white;
}
.backColor_3 {
  padding: 5px;
  border: solid 3px rgb(149, 145, 211);

  box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  background-color: white;
}
.backColor_4 {
  padding: 5px;
  border: solid 3px rgb(211, 145, 205);

  box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  background-color: white;
}
.backColor_my {
  padding: 5px;
  border: solid 3px rgb(155, 161, 63);

  box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  background-color: white;
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