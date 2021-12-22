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
      <b-button size="sm" variant="danger" @click="cancel()"> 닫기 </b-button>
      <b-button size="sm" variant="secondary" @click="ok()"> 확인 </b-button>
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
    settings: {
      type: Object,
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
      if (!val) {
        (this.MenuSelected = ["print", "ab", "c1", "c2", "c3", "c4"]),
          this.MenuOptions.forEach((item) => {
            item.notEnabled = true;
          });
      }
    },
    selectedIds: function (val) {
      //ok,cancel 시 선택 해지해주기
      if (val != null && val.length > 0) {
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
    ...mapActions("cueList", ["setStartTime"]),

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
      if (this.selectedIds == null || this.selectedIds.length == 0) {
        window.$notify("error", `템플릿을 선택하세요.`, "", {
          duration: 10000,
          permanent: false,
        });
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
          window.$notify("error", `가져올 항목을 선택하세요.`, "", {
            duration: 10000,
            permanent: false,
          });
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
                if (resultPrintData.length > 99) {
                  resultPrintData.splice(100);
                  window.$notify("error", `최대 개수를 초과하였습니다.`, "", {
                    duration: 10000,
                    permanent: false,
                  });
                }
                this.SET_PRINTARR(resultPrintData);
                this.setStartTime();
                this.SET_CUEINFO(oldCueInfo);
                this.$emit("settings", oldCueInfo);
                eventBus.$emit("printDataSet");
              }
              if (this.MenuSelected.includes("ab")) {
                if (beforeAbData.length > 0) {
                  res.data.normalCon.forEach((ele) => {
                    ele.rownum = ele.rownum + beforeAbData.length;
                  });
                }
                var resultABData = beforeAbData.concat(res.data.normalCon);
                if (resultABData.length > 499) {
                  resultABData.splice(500);
                  window.$notify("error", `최대 개수를 초과하였습니다.`, "", {
                    duration: 10000,
                    permanent: false,
                  });
                }
                this.SET_ABCARTARR(resultABData);
                eventBus.$emit("abDataSet");
              }
              var pram = {
                data: res.data.instanceCon,
                items: this.MenuSelected,
              };
              eventBus.$emit("updateCData", pram);
              this.selectedIds = null;
              this.$refs["importTem"].hide();
            });
        }
      }
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