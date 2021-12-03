<template>
  <b-modal
    id="commonImportTem"
    ref="importTem"
    size="xl"
    title="템플릿 가져오기"
    @hide="state = false"
    @show="state = true"
  >
    <div class="d-block text-center">
      <common-form
        :searchItems="searchItems"
        :isDisplayBtnArea="true"
        @changeRowPerpage="onChangeRowPerpage"
      >
        <!-- 검색 -->
        <template slot="form-search-area">
          <!-- 템플릿 이름 -->
          <b-form-group label="템플릿 이름" class="has-float-label">
            <common-input-text v-model="temtitle" />
          </b-form-group>
          <!-- 검색 버튼 -->
          <b-form-group>
            <b-button variant="outline-primary default" @click="onSearch"
              >검색</b-button
            >
          </b-form-group>
        </template>

        <!-- 테이블 페이지 -->
        <template slot="form-table-page-area">
          {{ getPageInfo() }}
        </template>
        <template slot="form-table-area">
          <!-- 테이블 -->
          <common-data-table-scroll-paging
            ref="scrollPaging"
            tableHeight="280px"
            :fields="fields"
            :rows="responseData.data"
            :per-page="responseData.rowPerPage"
            :totalCount="responseData.totalRowCount"
            is-actions-slot
            :multi-sort="false"
            :num-rows-to-bottom="5"
            :isTableLoading="isTableLoading"
            @scrollPerPage="onScrollPerPage"
            @selectedIds="onSelectedIds"
            @sortableclick="onSortable"
            @refresh="onRefresh"
          >
          </common-data-table-scroll-paging>
        </template>
      </common-form>
    </div>
    <template #modal-footer="{ cancel }">
      <b-form-checkbox-group
        v-model="MenuSelected"
        :options="MenuOptions"
        class="import-check-items"
        value-field="value"
        text-field="name"
        disabled-field="notEnabled"
        plain
      ></b-form-checkbox-group>
      <b-form-radio-group
        v-model="importSelected"
        :options="importOptions"
      ></b-form-radio-group>
      <b-button size="sm" @click="cancel()"> Cancel </b-button>
      <b-button size="sm" @click="ok()"> OK </b-button>
    </template>
  </b-modal>
</template>
<script>
import { mapActions, mapGetters, mapMutations } from "vuex";
import MixinBasicPage from "../../mixin/MixinBasicPage";
import { eventBus } from "@/eventBus";
import axios from "axios";
import "moment/locale/ko";
const moment = require("moment");
const qs = require("qs");
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
  mixins: [MixinBasicPage],
  props: {
    id: {
      type: String,
      default: "",
    },
  },
  data() {
    return {
      state: false,
      temtitle: "",
      searchItems: {
        rowPerPage: 30,
        selectPage: 1,
      },
      fields: [
        {
          name: "__checkbox",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "3%",
        },
        {
          name: "createtime",
          title: "생성일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "20%",
          callback: (value) => {
            return value === null
              ? ""
              : moment(value, "YYYYMMDDHH:mm:ss").format(
                  "YYYY-MM-DD : HH시 mm분"
                );
          },
        },

        {
          name: "edittime",
          title: "수정일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "20%",
          callback: (value) => {
            return value === null
              ? ""
              : moment(value, "YYYYMMDDHH:mm:ss").format(
                  "YYYY-MM-DD : HH시 mm분"
                );
          },
        },
        {
          name: "tmptitle",
          title: "템플릿 이름",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
        },
      ],
      allCheck: true,
      MenuSelected: ["print", "ab", "c1", "c2", "c3", "c4"],
      importSelected: "add",
      MenuOptions: [
        { name: "출력용", value: "print", notEnabled: true },
        { name: "DAP(A, B)", value: "ab", notEnabled: true },
        { name: "C1", value: "c1", notEnabled: true },
        { name: "C2", value: "c2", notEnabled: true },
        { name: "C3", value: "c3", notEnabled: true },
        { name: "C4", value: "c4", notEnabled: true },
      ],
      importOptions: [
        { text: "덮어쓰기", value: "add" },
        { text: "붙여넣기 (C채널 제외)", value: "update" },
      ],
    };
  },
  watch: {
    state: function (val) {
      this.getData();
    },
    selectedIds: function (val) {
      //ok,cancel 시 선택 해지해주기
      if (val.length > 0) {
        this.MenuOptions.forEach((item) => {
          item.notEnabled = false;
        });
      } else {
        this.MenuOptions.forEach((item) => {
          item.notEnabled = true;
        });
      }
    },
  },
  computed: {
    ...mapGetters("cueList", ["cueInfo"]),
    ...mapGetters("cueList", ["abCartArr"]),
    ...mapGetters("cueList", ["printArr"]),
    ...mapGetters("cueList", ["tempCuesheetListArr"]),
  },
  methods: {
    ...mapMutations("cueList", ["SET_CUEINFO"]),
    ...mapMutations("cueList", ["SET_ABCARTARR"]),
    ...mapMutations("cueList", ["SET_CCHANNELDATA"]),
    ...mapMutations("cueList", ["SET_PRINTARR"]),
    ...mapMutations("cueList", ["SET_TEMPCUESHEETLISTARR"]),
    ...mapActions("cueList", ["sponsorDataFun"]),

    async getData() {
      if (this.state) {
        this.isTableLoading = this.isScrollLodaing ? false : true;
        await axios
          .get(`/api/TempCueSheet/GetTempList`, {
            params: {
              personid: this.id,
              title: this.temtitle,
              row_per_page: this.searchItems.rowPerPage,
              select_page: this.searchItems.selectPage,
            },
            paramsSerializer: (params) => {
              return qs.stringify(params);
            },
          })
          .then((res) => {
            var seqnum = 0;
            res.data.resultObject.data.forEach((ele) => {
              ele.tabletype = "modal";
              ele.seq = seqnum;
              seqnum = seqnum + 1;
            });
            this.SET_TEMPCUESHEETLISTARR(res.data.resultObject);
            this.setResponseData(res);
            this.addScrollClass();
            this.isTableLoading = false;
            this.isScrollLodaing = false;
          });
      }
    },
    async ok() {
      if (this.selectedIds.length == 0) {
        alert("템플릿을 선택하세요.");
      } else {
        var rowNum_ab = 0;
        var rowNum_print = 0;

        var rowNum_c = 0;

        var beforePrintData = [];
        var beforeAbData = [];
        if (this.importSelected == "update") {
          beforePrintData = this.printArr;
          if (beforePrintData.length > 0) {
            rowNum_print =
              Math.max.apply(
                Math,
                beforePrintData.map((i) => {
                  return i.rownum;
                })
              ) + 1;
          }
          beforeAbData = this.abCartArr;
          if (beforeAbData.length > 0) {
            rowNum_ab =
              Math.max.apply(
                Math,
                beforeAbData.map((i) => {
                  return i.rownum;
                })
              ) + 1;
          }
        }
        if (this.MenuSelected.length == 0) {
          alert("가져오기할 항목들을 선택하세요.");
        } else {
          var seqnum = this.selectedIds[0];
          //var cueid = this.tempCuesheetListArr.data[seqnum].cueid;
          var params = {
            cueid: this.tempCuesheetListArr.data[seqnum].cueid,
            pgmcode: this.cueInfo.pgmcode,
          };
          if (this.cueInfo.cuetype == "D") {
            if (Object.keys(this.cueInfo).includes("detail")) {
              params.brd_dt = this.cueInfo.brddate;
            } else {
              params.brd_dt = this.cueInfo.day;
            }
          } else if (this.cueInfo.cuetype == "B") {
            params.brd_dt = toDay;
          }
          await axios
            .get(`/api/tempcuesheet/GettempCue`, {
              params: params,
              paramsSerializer: (params) => {
                return qs.stringify(params);
              },
            })
            .then((res) => {
              if (this.MenuSelected.includes("print")) {
                if (beforePrintData.length > 0) {
                  res.data.printDTO.forEach((ele) => {
                    ele.rownum = ele.rownum + beforePrintData.length;
                  });
                }
                var oldCueInfo = { ...this.cueInfo };
                oldCueInfo.directorname = res.data.cueSheetDTO.directorname;
                oldCueInfo.djname = res.data.cueSheetDTO.djname;
                oldCueInfo.footertitle = res.data.cueSheetDTO.footertitle;
                oldCueInfo.headertitle = res.data.cueSheetDTO.headertitle;
                oldCueInfo.membername = res.data.cueSheetDTO.membername;
                oldCueInfo.memo = res.data.cueSheetDTO.memo;

                var resultPrintData = beforePrintData.concat(res.data.printDTO);
                this.SET_PRINTARR(resultPrintData);
                this.SET_CUEINFO(oldCueInfo);
                eventBus.$emit("printDataSet");
              }
              if (this.MenuSelected.includes("ab")) {
                if (beforeAbData.length > 0) {
                  res.data.normalCon.forEach((ele) => {
                    ele.rownum = ele.rownum + beforeAbData.length;
                  });
                }
                var resultABData = beforeAbData.concat(res.data.normalCon);
                this.SET_ABCARTARR(resultABData);
                eventBus.$emit("abDataSet");
              }
              var pram = {
                data: res.data.instanceCon,
                items: this.MenuSelected,
              };
              eventBus.$emit("updateCData", pram);
              // if (this.MenuSelected.includes("c1")) {
              //   eventBus.$emit("update_channel_1", res.data.instanceCon["channel_1"]);
              // }
              // if (this.MenuSelected.includes("c2")) {
              //   eventBus.$emit("channel_2", res.data.instanceCon["channel_2"]);
              // }
              // if (this.MenuSelected.includes("c3")) {
              //   eventBus.$emit("channel_3", res.data.instanceCon["channel_3"]);
              // }
              // if (this.MenuSelected.includes("c4")) {
              //   eventBus.$emit("channel_4", res.data.instanceCon["channel_4"]);
              // }

              // if (
              //   this.MenuSelected.includes("c1") ||
              //   this.MenuSelected.includes("c2") ||
              //   this.MenuSelected.includes("c3") ||
              //   this.MenuSelected.includes("c4")
              //) {
              // //C채널 -그룹
              // var cDataGroup = cueSheetCons.filter((ele) => {
              //   if (ele.channeltype == "I") {
              //     ele.rowNum = rowNum_c;
              //     ele.editTarget = true;
              //     rowNum_c = rowNum_c + 1;
              //     ele.duration = moment(ele.endposition)
              //       .add(-9, "hours")
              //       .format("HH:mm:ss.SS");
              //     this.productFilter(ele);
              //     return ele;
              //   }
              // });
              // //C채널 - 카트별
              // var cDataResult = [];
              // var row = {};
              // for (var channelNum = 0; 4 > channelNum; channelNum++) {
              //   cDataResult = [];
              //   var cartNum = "c" + (channelNum + 1);
              //   var setResult = false;
              //   for (var i = 0; 16 > i; i++) {
              //     for (var index = 0; cDataGroup.length > index; index++) {
              //       if (cDataGroup[index].seqnum == i + 16 * channelNum + 1) {
              //         row = cDataGroup[index];
              //         break;
              //       } else {
              //         row = {};
              //       }
              //     }
              //     cDataResult.push(row);
              //   }
              //   this.MenuSelected.forEach((cart) => {
              //     if (cart == cartNum) {
              //       return (setResult = true);
              //     }
              //   });
              //   if (setResult) {
              //     this.SET_CCHANNELDATA({
              //       type: "channel_" + (channelNum + 1),
              //       value: cDataResult,
              //     });
              //     eventBus.$emit("update_channel_" + (channelNum + 1));
              //   }
              // }
              //}
              this.$refs["importTem"].hide();
            });
        }
      }
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
.import-check-items {
  position: absolute;
  left: 40px;
  font-size: 13px;
}
.import-check-items .form-check-inline {
  margin-right: 20px !important;
}
</style>