<template>
  <div class="common-actions">
    <b-button
      v-if="display(PREVIEW_CODE)"
      class="icon-buton"
      :disabled="!existFile()"
      :style="getDownloadStyle()"
      v-b-tooltip.hover.top="{
        title:
          isFilePathPreviewTitle && IS_ADMIN ? rowData.filePath : '미리듣기',
      }"
      @click.stop="onPreview()"
    >
      <b-icon icon="caret-right-square" class="icon"></b-icon>
    </b-button>
    <b-button
      v-if="display(DOWNLOAD_CODE)"
      :id="`download-${rowData.rowNO}`"
      :disabled="!existFile()"
      :style="getDownloadStyle()"
      class="icon-buton"
      v-b-tooltip.hover.top="{
        title: IS_ADMIN ? rowData.filePath : '다운로드',
      }"
      @click.stop="onDownload()"
    >
      <b-icon icon="download" class="icon"></b-icon>
    </b-button>
    <b-button
      v-if="displayEtc('modify')"
      class="icon-buton"
      :title="getTitle('modify')"
      @click.stop="onMetaModify()"
    >
      <b-icon icon="pencil-square" class="icon" variant="info"></b-icon>
    </b-button>
    <b-button
      v-if="displayEtc('delete')"
      class="icon-buton"
      :title="getTitle('delete')"
      :disabled="!isPossibleDelete || !isSystemTopAdmin"
      :style="getDeleteStyle()"
      @click.stop="onDelete()"
    >
      <b-icon icon="trash" class="icon" variant="danger"></b-icon>
    </b-button>
    <b-button
      v-if="displayMyDiskCopy()"
      class="icon-buton"
      :disabled="!existFile()"
      :title="getTitle('mydisk-copy')"
      @click.stop="onMyDiskCopy()"
    >
      <i class="iconsminds-shop i-custom-actions-shop" />
    </b-button>
    <b-button
      v-if="rowData.r_ONAIRTIME && rowData.cueid == -1"
      id="cueBtn"
      variant="outline-primary"
      @click="addCueDetail"
      >큐시트 작성</b-button
    >
    <b-button
      v-if="rowData.r_ONAIRTIME && rowData.cueid > 0"
      id="cueBtn_update"
      variant="outline-success"
      @click="getCueDetail('day')"
      >큐시트 수정</b-button
    >
    <b-button
      v-if="rowData.cuetype == 'B'"
      id="cueBtn"
      variant="outline-primary"
      @click="getCueDetail('def')"
      >기본큐시트 작성</b-button
    >
    <b-button
      v-if="rowData.cuetype == 'T'"
      id="cueBtn"
      variant="outline-primary"
      @click="getCueDetail('temp')"
      >템플릿 작성</b-button
    >
    <b-button
      v-if="rowData.broadcastStatus"
      id="cueBtn"
      variant="outline-primary"
      @click="$router.push({ path: '/app/cuesheet/detail' })"
      >큐시트 조회</b-button
    >
    <b-button v-if="!temData" id="cueBtn" variant="outline-primary"
      >선택</b-button
    >
  </div>
</template>
<script>
import {
  PREVIEW_CODE,
  DOWNLOAD_CODE,
  AUTHORITY,
  AUTHORITY_ADMIN,
  USER_ID,
  ROLE,
  MY_DISK_PAGE_ID,
  ROUTE_NAMES,
} from "@/constants/config";
import { mapActions, mapGetters, mapMutations } from "vuex";
const userId = sessionStorage.getItem(USER_ID);
import axios from "axios";
import "moment/locale/ko";
const moment = require("moment");
const qs = require("qs");

export default {
  props: {
    temData: {
      type: Boolean,
      default: true,
    },
    rowData: {
      type: Object,
      default: () => {},
    },
    etcData: {
      type: Array,
      default: () => [], // ['delete', 'modify']
    },
    etcTitles: {
      type: Object,
      default: () => {}, // {'delete': '삭제', 'modify': '정보편집'}
    },
    behaviorData: {
      type: Array,
      default: () => [],
    },
    isPossibleDelete: {
      type: Boolean,
      default: true,
    },
    isFilePathPreviewTitle: {
      type: Boolean,
      default: false,
    },
    downloadName: {
      type: String,
      default: "",
    },
    widgetIndex: Number,
    productWeekList: {
      type: Array,
      default: () => [],
    },
  },
  data() {
    return {
      PREVIEW_CODE,
      DOWNLOAD_CODE,
      MY_DISK_PAGE_ID,
      ROUTE_NAMES,
      currentPageName: "",
      IS_ADMIN: sessionStorage.getItem(AUTHORITY) === AUTHORITY_ADMIN,
    };
  },
  computed: {
    ...mapGetters("user", ["roleList", "isSystemTopAdmin"]),
    ...mapGetters("cuesheet", ["cChannelData"]),
  },
  watch: {
    $route: {
      handler(to, from) {
        this.currentPageName = this.$route.name;
      },
      immediate: true,
    },
  },
  methods: {
    ...mapMutations("cuesheet", ["SET_SELEDAYCUE"]),
    ...mapMutations("cuesheet", ["SET_ABCHANNELDATA"]),
    ...mapMutations("cuesheet", ["SET_CCHANNELDATA"]),
    ...mapMutations("cuesheet", ["SET_CUEPRINT"]),
    ...mapMutations("cuesheet", ["SET_WEEKDATA"]),
    logout() {
      this.SET_LOGOUT();
      this.$router.push("/user/Login");
    },
    display(value) {
      return this.behaviorData.some(
        (data) => data.id === value && data.visible === "Y"
      );
    },
    displayMyDiskCopy() {
      const exceptPageNames = [
        this.ROUTE_NAMES.PRIVATE,
        this.ROUTE_NAMES.WASTE_BASKET,
      ];
      if (
        exceptPageNames.includes(this.currentPageName) ||
        this.rowData.cueid != undefined ||
        this.rowData.detail ||
        this.rowData.addDate ||
        this.rowData.broadcastStatus
      ) {
        return false;
      }
      return this.roleList.some(
        (data) => data.id === this.MY_DISK_PAGE_ID && data.visible === "Y"
      );
    },
    displayEtc(value) {
      return this.etcData.some((data) => data === value);
    },
    onPreview() {
      this.$emit("preview", this.rowData);
    },
    onDownload() {
      this.$emit("download", this.rowData, this.downloadName);
    },
    onDelete() {
      if (!this.isPossibleDelete || !this.isSystemTopAdmin) {
        return;
      }
      this.$emit("delete", this.rowData);
    },
    onMetaModify() {
      this.$emit("modify", this.rowData);
    },
    onMyDiskCopy() {
      if (!this.existFile()) {
        return;
      }
      this.$emit("mydiskCopy", this.rowData);
    },
    getTitle(type) {
      if (this.etcTitles && this.etcTitles[type]) {
        return this.etcTitles[type];
      }
      if (type === "delete") {
        return "휴지통";
      }
      if (type === "modify") {
        return "정보편집";
      }
      if (type === "mydisk-copy") {
        return "My 공간으로 복사";
      }
      return "";
    },
    getDeleteStyle() {
      return {
        opacity: this.isPossibleDelete ? 1 : 0.2,
      };
    },
    existFile() {
      return this.rowData.existFile;
    },
    getDownloadStyle() {
      return {
        opacity: this.rowData.existFile ? 1 : 0.2,
      };
    },
    //상세내용 가져오기
    getCueDetail(V) {
      axios
        .get(`/api/${V}cuesheet/Get${V}Cue`, {
          params: {
            productid: this.rowData.productid,
            cueid: this.rowData.cueid,
          },
          paramsSerializer: (params) => {
            return qs.stringify(params);
          },
        })
        .then((res) => {
          //요기에 템플릿 전용 만들기
          // if (V == "temp") {
          //   console.log(res);
          // }
          //출력용
          var printData = [];
          res.data.prints.forEach((ele, index) => {
            printData[index] = Object.assign({}, ele);
            printData[index].rowNum = index;
            printData[index].code = ele.code.trim();
            printData[index].contents = ele.contents;
            printData[index].etc = ele.etc;
            printData[index].starttime = ele.starttime;
            delete printData[index].seqnum;
          });
          const cueSheetCons = res.data.cueSheetCons;
          var rowNum_ab = 0;
          var rowNum_c = 0;

          //AB채널
          var abData = cueSheetCons.filter((ele) => {
            if (ele.channeltype == "N") {
              ele.rowNum = rowNum_ab;
              rowNum_ab = rowNum_ab + 1;
              ele.duration = moment(ele.endposition)
                .add(-9, "hours")
                .format("HH:mm:ss.SS");
              this.productFilter(ele);
              return ele;
            }
          });
          //C채널 -그룹
          var cDataGroup = cueSheetCons.filter((ele) => {
            if (ele.channeltype == "I") {
              ele.rowNum = rowNum_c;
              ele.editTarget = true;
              rowNum_c = rowNum_c + 1;
              ele.duration = moment(ele.endposition)
                .add(-9, "hours")
                .format("HH:mm:ss.SS");
              this.productFilter(ele);
              return ele;
            }
          });

          //C채널 - 카트별
          var cDataResult = [];
          var row = {};
          for (var channelNum = 0; 4 > channelNum; channelNum++) {
            cDataResult = [];
            for (var i = 0; this.widgetIndex > i; i++) {
              for (var index = 0; cDataGroup.length > index; index++) {
                if (
                  cDataGroup[index].seqnum ==
                  i + this.widgetIndex * channelNum + 1
                ) {
                  row = cDataGroup[index];
                  break;
                } else {
                  row = {};
                }
              }
              cDataResult.push(row);
            }
            this.SET_CCHANNELDATA({
              type: "channel_" + (channelNum + 1),
              value: cDataResult,
            });
          }

          //추가
          // abData = abData.filter(function (el) {
          //   return el != null;
          // });

          this.SET_CUEPRINT(printData);
          this.SET_ABCHANNELDATA(abData);
          switch (V) {
            case "day":
              this.SET_SELEDAYCUE(res.data.cueSheet);
              this.$router.push({ path: "/app/cuesheet/day/detail" });
              break;
            case "def":
              var cueids = [];
              this.SET_WEEKDATA({
                rowData: this.rowData,
                productWeekList: this.productWeekList,
              });
              res.data.cueSheet.detail.forEach((ele) => {
                cueids.push(ele.cueid);
              });
              this.SET_SELEDAYCUE(res.data.cueSheet);
              this.$router.push({ path: "/app/cuesheet/week/detail" });
              break;
            case "temp":
              this.SET_SELEDAYCUE(res.data.template);
              this.$router.push({ path: "/app/cuesheet/template/detail" });
              break;
            default:
              break;
          }
        });
    },
    addCueDetail() {
      //큐시트작성
      var pram = {
        brddate: this.rowData.day,
        brdtime: this.rowData.r_ONAIRTIME,
        cueid: this.rowData.cueid,
        eventname: this.rowData.eventname,
        media: this.rowData.media,
        personid: userId,
        productid: this.rowData.productid,
        servicename: this.rowData.servicename,
        headertitle: this.rowData.servicename,
      };
      this.SET_SELEDAYCUE(pram);
      this.SET_ABCHANNELDATA([]);
      this.SET_CCHANNELDATA([]);
      this.SET_CUEPRINT([]);
      this.$router.push({ path: "/app/cuesheet/day/detail" });
    },
    productFilter(arr) {
      switch (arr.cartcode) {
        case "S01G01C011":
          arr.productType = "PUBLIC_FILE";
          break;
        case "S01G01C013":
          arr.productType = "OLD_PRO";
          break;
        case "S01G01C017":
          arr.productType = "SCR_SB";
          break;
        case "S01G01C010":
          arr.productType = "SCR_SPOT";
          break;
        case "S01G01C018":
          arr.productType = "PGM_CM";
          break;
        case "S01G01C019":
          arr.productType = "CM";
          break;
        case "S01G01C012":
          arr.productType = "REPOTE";
          break;
        case "S01G01C021":
          arr.productType = "FILLER_PR";
          break;
        case "S01G01C022":
          arr.productType = "FILLER_MT";
          break;
        case "S01G01C023":
          arr.productType = "FILLER_TIME";
          break;
        case "S01G01C024":
          arr.productType = "FILLER_ETC";
          break;
        case "S01G01C009":
          arr.productType = "PGM";
          break;
        case "S01G01C016":
          arr.productType = "MCR_SB";
          break;
        case "S01G01C020":
          arr.productType = "MCR_SPOT";
          break;

        default:
          break;
      }
      return arr;
    },
  },
};
</script>
<style>
#cueBtn_update {
  border-color: #28a745;
  color: #28a745;
}
#cueBtn {
  color: rgb(0, 110, 229);
  border-color: #007bff;
}
#cueBtn:active,
#cueBtn_update:active {
  color: white;
}
</style>