<template>
  <b-modal
    id="commonImportTem"
    ref="importTem"
    size="xl"
    title="템플릿 가져오기"
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
            :rows="tempCuesheetListArr.data"
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
    id: function (val) {
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
    ...mapGetters("cueList", ["abCartArr"]),
    ...mapGetters("cueList", ["printArr"]),
    ...mapGetters("cueList", ["tempCuesheetListArr"]),
  },
  methods: {
    ...mapMutations("cueList", ["SET_ABCARTARR"]),
    ...mapMutations("cueList", ["SET_CCHANNELDATA"]),
    ...mapMutations("cueList", ["SET_PRINTARR"]),
    ...mapMutations("cueList", ["SET_TEMPCUESHEETLISTARR"]),
    async getData() {
      if (this.id != "") {
        this.isTableLoading = this.isScrollLodaing ? false : true;
        await axios
          .get(
            `/api/TempCueSheet/GetTempList?personid=${this.id}&title=${this.temtitle}`
          )
          .then((res) => {
            var seqnum = 0;
            res.data.forEach((ele) => {
              ele.tabletype = "modal";
              ele.seq = seqnum;
              seqnum = seqnum + 1;
            });
            this.SET_TEMPCUESHEETLISTARR(res);
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
        var rowNum_c = 0;
        var rowNum_print = 0;
        var beforePrintData = [];
        var beforeAbData = [];
        if (this.importSelected == "update") {
          beforePrintData = this.printArr;
          if (beforePrintData.length > 0) {
            rowNum_print =
              Math.max.apply(
                Math,
                beforePrintData.map((i) => {
                  return i.rowNum;
                })
              ) + 1;
          }
          beforeAbData = this.abCartArr;
          if (beforeAbData.length > 0) {
            rowNum_ab =
              Math.max.apply(
                Math,
                beforeAbData.map((i) => {
                  return i.rowNum;
                })
              ) + 1;
          }
        }
        if (this.MenuSelected.length == 0) {
          alert("가져오기할 항목들을 선택하세요.");
        } else {
          var seqnum = this.selectedIds[0];
          var cueid = this.tempCuesheetListArr.data[seqnum].cueid;
          await axios
            .get(`/api/tempcuesheet/GettempCue?cueid=${cueid}`)
            .then((res) => {
              const cueSheetCons = res.data.cueSheetCons;
              if (this.MenuSelected.includes("print")) {
                //출력용
                var printData = [];
                res.data.prints.forEach((ele, index) => {
                  printData[index] = Object.assign({}, ele);
                  printData[index].rowNum = rowNum_print;
                  rowNum_print = rowNum_print + 1;
                  printData[index].code = ele.code.trim();
                  printData[index].contents = ele.contents;
                  printData[index].etc = ele.etc;
                  printData[index].starttime = ele.starttime;
                  delete printData[index].seqnum;
                });

                var resultPrintData = beforePrintData.concat(printData);
                this.SET_PRINTARR(resultPrintData);
                eventBus.$emit("printDataSet");
              }
              if (this.MenuSelected.includes("ab")) {
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

                var resultABData = beforeAbData.concat(abData);
                this.SET_ABCARTARR(resultABData);
                eventBus.$emit("abDataSet");
              }
              if (
                this.MenuSelected.includes("c1") ||
                this.MenuSelected.includes("c2") ||
                this.MenuSelected.includes("c3") ||
                this.MenuSelected.includes("c4")
              ) {
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
                  var cartNum = "c" + (channelNum + 1);
                  var setResult = false;
                  for (var i = 0; 16 > i; i++) {
                    for (var index = 0; cDataGroup.length > index; index++) {
                      if (cDataGroup[index].seqnum == i + 16 * channelNum + 1) {
                        row = cDataGroup[index];
                        break;
                      } else {
                        row = {};
                      }
                    }
                    cDataResult.push(row);
                  }
                  this.MenuSelected.forEach((cart) => {
                    if (cart == cartNum) {
                      return (setResult = true);
                    }
                  });
                  if (setResult) {
                    this.SET_CCHANNELDATA({
                      type: "channel_" + (channelNum + 1),
                      value: cDataResult,
                    });
                    eventBus.$emit("update_channel_" + (channelNum + 1));
                  }
                }
                //추가정보들 가지고올꺼도 추가해야함
              }
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