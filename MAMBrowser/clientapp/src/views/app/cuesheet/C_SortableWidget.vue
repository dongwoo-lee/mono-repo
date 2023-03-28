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
                    fileData[index - 1].cartcode == 'S01G01C015' ||
                    fileData[index - 1].cartcode == 'S01G01C032'
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
              <div style="width: 17px">
                <div
                  v-if="
                    !fileData[index - 1].fadeintime &&
                    fileData[index - 1].startposition > 0
                  "
                >
                  <img src="/assets/img/play-edit.png" />
                </div>
                <div
                  v-if="
                    fileData[index - 1].fadeintime &&
                    !fileData[index - 1].startposition > 0
                  "
                >
                  <img src="/assets/img/play-fadein.png" />
                </div>
                <div
                  v-if="
                    fileData[index - 1].fadeintime &&
                    fileData[index - 1].startposition > 0
                  "
                >
                  <img src="/assets/img/play-som-fadein.png" />
                </div>
              </div>
              <div style="width: 17px">
                <div
                  v-if="
                    !fileData[index - 1].fadeouttime &&
                    fileData[index - 1].duration >
                      fileData[index - 1].endposition
                  "
                >
                  <img src="/assets/img/play-edit.png" />
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
                  <img src="/assets/img/play-fadeout.png" />
                </div>
                <div
                  v-if="
                    fileData[index - 1].fadeouttime &&
                    fileData[index - 1].duration >
                      fileData[index - 1].endposition
                  "
                >
                  <img src="/assets/img/play-eom-fadeout.png" />
                </div>
              </div>
              <div class="actionBtn">
                <DxButton
                  icon="music"
                  type="default"
                  hint="미리듣기/음원편집"
                  @click="onPreview(fileData[index - 1])"
                  v-if="fileData[index - 1].existFile"
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
                  maxlength="40"
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
        :parentName="playTem_name"
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
        :parentName="playTem_name"
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
      playTem_name: "",
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
        existFile: false,
        maintitle: "",
        memo: "", //바뀔수도있음
        onairdate: "",
        startposition: 0,
        subtitle: "",
        transtype: "S",
        useflag: "Y",
      },
    };
  },
  async created() {
    switch (this.channelKey) {
      case "channel_1":
        this.playTem_name = "C1";
        break;
      case "channel_2":
        this.playTem_name = "C2";
        break;
      case "channel_3":
        this.playTem_name = "C3";
        break;
      case "channel_4":
        this.playTem_name = "C4";
        break;
      case "channel_my":
        this.playTem_name = "즐겨찾기";
        break;
      default:
        break;
    }
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
    ...mapActions("cueList", ["cartCodeFilter"]),
    ...mapActions("cueList", ["setInstanceCon"]),
    ...mapActions("cueList", ["sponsorDataFun"]),
    ...mapActions("cueList", ["setContents"]),
    ...mapActions("cueList", ["enableNotification"]),
    // 드래그 추가 시
    async onAdd(e, totalIndex) {
      this.loadingVisible = true;
      var rowArray = [];
      const arrData = this.fileData;
      if (e.fromData === undefined) {
        //ab, 소재검색
        var selectedRowsData = this.sortSelectedRowsData(e);
        // 단일 선택
        if (e.itemElement.ariaSelected == "false") {
          selectedRowsData = [e.itemData];
        }
        // 즐겨찾기일 경우 광고 그룹 제거
        if (this.channelKey == "channel_my") {
          selectedRowsData = this.sponsorFilter_Fav(selectedRowsData);
        }
        // 빈칸 제거
        selectedRowsData = this.blankFilter(selectedRowsData);
        //모든 필터확인해서 남은 개수 보다 넘는 배열은 잘라내기
        selectedRowsData = this.checkMaxWidgetIndex(selectedRowsData);
        for (const data of selectedRowsData) {
          if (Object.keys(data).includes("subtitle")) {
            //ab
            data.contentType = "AB";
            rowArray.push(data);
          } else {
            //소재검색
            data.contentType = "S";
            rowArray.push(data);
          }
        }
      } else {
        // sortable widget
        e.fromData.rownum = this.fileData[totalIndex - 1].rownum;
        this.fileData.splice(totalIndex - 1, 1, e.fromData);
      }
      //store 추가
      if (rowArray.length > 0) {
        var index = 0;
        for await (const ele of rowArray) {
          if (ele.existFile) {
            var rowData = await this.setContents({
              type: "c",
              search_row: ele,
              formRowData: this.rowData,
              cartcode: this.searchListData.cartcode,
              index: index,
              toIndex: totalIndex,
            });
          }
          if (rowData) {
            arrData.splice(totalIndex - 1 + index, 1, rowData);
            index++;
          } else {
            this.enableNotification({
              type: "error",
              message: `사용할 수 없는 소재입니다.`,
            });
          }
        }
        if (this.channelKey == "channel_my") {
          //즐겨찾기
          this.SET_CUEFAVORITES(arrData);
        } else {
          // C channel
          var resultData = { ...this.cChannelData };
          resultData[this.channelKey] = arrData;
          this.SET_CCHANNELDATA(resultData);
        }
      }
      this.loadingVisible = false;
    },
    searchTabIndex(text) {
      switch (text) {
        case "C1":
          this.$emit("tabItemMove", 2);
          break;
        case "C2":
          this.$emit("tabItemMove", 3);
          break;
        case "C3":
          this.$emit("tabItemMove", 4);
          break;
        case "C4":
          this.$emit("tabItemMove", 5);
          break;
        case "즐겨찾기":
          this.$emit("tabItemMove", 6);
          break;
        default:
          break;
      }
    },
    getCoordinates(e) {
      const mouseX = e.clientX;
      const mouseY = e.clientY;
      const element = document.elementsFromPoint(mouseX, mouseY);
      element.forEach((node) => {
        node.tagName === "SPAN" && this.searchTabIndex(node.innerText);
      });
    },
    onDragStart(e) {
      document.addEventListener("mousemove", this.getCoordinates);
      document.getElementById("app-container").classList.add("drag_");
      if (
        this.cueInfo.cuetype == "A" ||
        !Object.keys(e.fromData).includes("cartcode")
      ) {
        e.cancel = true;
      } else {
        if (e.fromData.cartcode == null) {
          e.cancel = true;
        }
      }
    },
    // 드래그 종료 시
    onDragEnd() {
      document.getElementById("app-container").classList.remove("drag_");
      document.removeEventListener("mousemove", this.getCoordinates);
    },
    // 즐겨찾기 광고 그룹 필터
    sponsorFilter_Fav(obj) {
      var groupBool = false;
      //광고 그룹 제외
      obj = obj.filter((ele) => {
        //ab 소재
        if (Object.keys(ele).includes("cartcode")) {
          if (ele.onairdate != "" && ele.cartcode != null) {
            groupBool = true;
          }
          return ele.onairdate == "" && ele.cartcode != null;
        } else {
          //소재검색 소재
          if (Object.keys(ele).includes("capacity")) {
            groupBool = true;
          }
          return !Object.keys(ele).includes("capacity");
        }
      });

      //광고 그룹 있을 시
      if (groupBool) {
        this.enableNotification({
          type: "error",
          message: `CM, SB 소재는 즐겨찾기에 추가할 수 없습니다.`,
        });
      }
      return obj;
    },
    // 빈칸제거
    blankFilter(obj) {
      var result = [];
      result = obj.filter((ele) => {
        if (Object.keys(ele).includes("cartcode")) {
          return ele.cartcode != "";
        } else {
          return (ele = ele);
        }
      });
      return result;
    },
    // 배열 초과 객체 제거
    checkMaxWidgetIndex(obj) {
      if (obj.length > this.widgetIndex) {
        obj = obj.slice(0, this.widgetIndex);
      }
      return obj;
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
    // 드래그 관련 객체 삭제
    onRemove($event, index) {
      const arrData = this.fileData;
      if (this.cueInfo.cuetype == "A") {
        return;
      }
      if ($event.toData) {
        if (!$event.toData.cartcode) {
          arrData.splice(index - 1, 1, { rownum: index });
        } else {
          var data = $event.toData;
          data.rownum = index;
          arrData.splice(index - 1, 1, data);
        }
      }
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
  },
};
</script>

<style>
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
  overflow: auto;
  border-radius: 5px;
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
.backColor_1 {
  padding: 5px;
  border: solid 3px #009efa;
  box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  background-color: white;
}
.backColor_2 {
  padding: 5px;
  border: solid 3px #00c9a7;
  box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  background-color: white;
}
.backColor_3 {
  padding: 5px;
  border: solid 3px #d83121;
  box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  background-color: white;
}
.backColor_4 {
  padding: 5px;
  border: solid 3px #ffc75f;
  box-shadow: rgba(0, 0, 0, 0.35) 0px 5px 15px;
  background-color: white;
}
.backColor_my {
  padding: 5px;
  border: solid 3px #b0a8b9;
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
