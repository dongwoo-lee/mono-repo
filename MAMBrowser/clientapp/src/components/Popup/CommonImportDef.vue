<template>
  <b-modal
    id="commonImportDef"
    ref="importDef"
    size="xl"
    title="기본 큐시트 가져오기"
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
          <b-form-group label="매체" class="has-float-label">
            <b-form-select
              style="width: 100px"
              :disabled="disabledVal"
              v-model="searchItems.media"
              :options="mediasOption"
              @change="eventClick($event, 'list')"
            />
          </b-form-group>
          <b-form-group label="프로그램명" class="has-float-label">
            <b-form-select
              style="width: 400px"
              :disabled="disabledVal"
              v-model="searchItems.productid"
              :options="programList"
            />
          </b-form-group>
          <b-form-group>
            <b-button
              variant="outline-primary default"
              @click="onSearch"
              :disabled="disabledVal"
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
            :isWeeksSlot="true"
            :num-rows-to-bottom="5"
            :isTableLoading="isTableLoading"
            @scrollPerPage="onScrollPerPage"
            @selectedIds="onSelectedIds"
            @sortableclick="onSortable"
            @refresh="onRefresh"
          >
            <template slot="weeks" scope="props">
              <common-weeks :rowData="props.props.rowData"> </common-weeks>
            </template>
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
import CommonWeeks from "../../components/DataTable/CommonWeeks.vue";
import { USER_ID } from "@/constants/config";
import { mapActions, mapGetters, mapMutations } from "vuex";
import MixinBasicPage from "../../mixin/MixinBasicPage";
import { eventBus } from "@/eventBus";
const userId = sessionStorage.getItem(USER_ID);
const qs = require("qs");
import axios from "axios";
import "moment/locale/ko";
const moment = require("moment");

export default {
  components: { CommonWeeks },
  mixins: [MixinBasicPage],
  props: {
    proid: {
      type: String,
      default: "",
    },
    type: {
      type: String,
    },
  },
  data() {
    return {
      state: false,
      disabledVal: true,
      searchItems: {
        media: "", // 매체
        productid: "", // 프로그램명
        rowPerPage: 30,
        selectPage: 1,
      },
      programList: [{ value: "", text: "매체를 선택하세요" }],
      fields: [
        {
          name: "__checkbox",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "3%",
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
          name: "eventname",
          title: "프로그램명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "eventname",
        },
        {
          name: "media",
          title: "매체",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          sortField: "media",
          width: "10%",
          callback: (value) => (value === "A" ? "표준FM" : "FM4U"),
        },
        {
          name: "__slot:weeks",
          title: "적용 요일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "20%",
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
  created() {
    if (this.type == "T") {
      this.disabledVal = false;
    } else {
      this.searchItems.media = this.cueInfo.media;
    }
  },
  computed: {
    ...mapGetters("cueList", ["abCartArr"]),
    ...mapGetters("cueList", ["printArr"]),
    ...mapGetters("cueList", ["defCuesheetListArr"]),
    ...mapGetters("cueList", ["userProOption"]),
    ...mapGetters("cueList", ["mediasOption"]),
    ...mapGetters("cueList", ["userProList"]),
    ...mapGetters("cueList", ["cueInfo"]),
  },
  methods: {
    ...mapMutations("cueList", ["SET_CUEINFO"]),
    ...mapMutations("cueList", ["SET_ABCARTARR"]),
    ...mapMutations("cueList", ["SET_CCHANNELDATA"]),
    ...mapMutations("cueList", ["SET_PRINTARR"]),
    ...mapMutations("cueList", ["SET_DEFCUESHEETLISTARR"]),
    ...mapActions("cueList", ["getuserProOption"]),
    ...mapActions("cueList", ["getMediasOption"]),
    ...mapActions("cueList", ["disableList"]),
    ...mapActions("cueList", ["setStartTime"]),

    async getData() {
      if (this.state) {
        //기본 큐시트 목록 가져오기
        this.isTableLoading = this.isScrollLodaing ? false : true;
        if (!this.searchItems.productid) {
          var mediaOption = await this.getMediasOption(userId);

          if (this.type != "T") {
            var temmedia = await this.eventClick(this.cueInfo.media);
            this.searchItems.productid = this.cueInfo.productid;
          } else {
            this.searchItems.productid = this.userProList;
          }
        }
        await axios
          .get(`/api/DefCueSheet/GetDefList`, {
            params: {
              productids: this.searchItems.productid,
              row_per_page: this.searchItems.rowPerPage,
              select_page: this.searchItems.selectPage,
            },
            paramsSerializer: (params) => {
              return qs.stringify(params);
            },
          })
          .then(async (res) => {
            var productWeekList = await this.disableList(
              res.data.resultObject.data
            );
            var seqnum = 0;
            res.data.resultObject.data.forEach((ele) => {
              var activeWeekList = [];
              var cueids = [];
              var weeks = [];
              ele.productWeekList = productWeekList.filter((week) => {
                return week.productid == ele.productid;
              });
              ele.detail.forEach((activeWeek) => {
                activeWeekList.push(activeWeek.week);
                cueids.push(activeWeek.cueid);
                weeks.push(activeWeek.week);
              });
              ele.activeWeekList = activeWeekList;
              ele.cueid = cueids;
              ele.weeks = weeks;
              ele.tabletype = "modal";
              ele.seq = seqnum;
              seqnum = seqnum + 1;
            });
            this.SET_DEFCUESHEETLISTARR(res.data.resultObject);
            this.setResponseData(res);
          });
        this.addScrollClass();
        this.isTableLoading = false;
        this.isScrollLodaing = false;
      }
    },
    async ok() {
      if (this.selectedIds.length == 0) {
        alert("큐시트를 선택하세요.");
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
          alert("가져오기할 항목들을 선택하세요.");
        } else {
          var seqnum = this.selectedIds[0];
          //var cueid = this.defCuesheetListArr.data[seqnum].cueid;
          var params = {
            productid: this.searchItems.productid,
            week: this.defCuesheetListArr.data[seqnum].weeks,
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
            .get(`/api/defcuesheet/GetdefCue`, {
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
                this.SET_ABCARTARR(resultABData);
                eventBus.$emit("abDataSet");
              }
              var pram = {
                data: res.data.instanceCon,
                items: this.MenuSelected,
              };
              eventBus.$emit("updateCData", pram);
              this.$refs["importDef"].hide();
            });
        }
      }
    },
    //매체 선택시 프로그램 목록 가져오기
    async eventClick(e) {
      var pram = { personid: userId, media: e };
      var proOption = await this.getuserProOption(pram);
      this.programList = this.userProOption;
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