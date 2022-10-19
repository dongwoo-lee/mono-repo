<template>
  <b-modal
    id="commonImportDef"
    ref="importDef"
    size="xl"
    title="기본 큐시트 가져오기"
    @hidden="cancel"
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
              :options="mediaOptions"
              @change="onMediaChange($event)"
            />
          </b-form-group>
          <b-form-group label="프로그램명" class="has-float-label">
            <b-form-select
              style="width: 400px"
              :disabled="disabledVal"
              v-model="searchItems.pgmcode"
              :options="programOptions"
              @change="onPgmChange($event)"
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
    <template #modal-footer="">
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
      <DxButton
        :width="100"
        text="취소"
        :disabled="loadingIconVal"
        @click="cancel()"
      />
      <DxButton
        type="default"
        text="확인"
        styling-mode="outlined"
        :disabled="loadingIconVal"
        :width="100"
        @click="ok()"
      >
      </DxButton>
    </template>
  </b-modal>
</template>
<script>
import CommonWeeks from "../../components/DataTable/CommonWeeks.vue";
import { ACCESS_GROP_ID, USER_NAME } from "@/constants/config";
import { mapActions, mapGetters, mapMutations } from "vuex";
import MixinBasicPage from "../../mixin/MixinBasicPage";
import DxButton from "devextreme-vue/button";
import { eventBus } from "@/eventBus";
const qs = require("qs");
import "moment/locale/ko";
const moment = require("moment");

export default {
  components: { CommonWeeks, DxButton },
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
      date: new Date(),
      state: false,
      loadingIconVal: false,
      disabledVal: true,
      pramObj: { person: null, brd_dt: null, media: null },
      pgmList: [],
      programOptions: [],
      mediaOptions: [],
      productIds: [],
      searchItems: {
        media: "", // 매체
        productid: "", // 프로그램명
        pgmcode: "",
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
          name: "media",
          title: "매체",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
          callback: (value) => {
            switch (value) {
              case "A":
                return "AM";
              case "F":
                return "FM";
              case "D":
                return "DMB";
              case "C":
                return "공통";
              case "Z":
                return "기타";
              default:
                break;
            }
          },
        },
        {
          name: "eventname",
          title: "프로그램명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
        },
        {
          name: "__slot:weeks",
          title: "적용 요일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "18%",
        },
        {
          name: "edittime",
          title: "최종 편집 일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "20%",
          callback: (value) => {
            return value === null
              ? ""
              : moment(value, "YYYYMMDDHH:mm:ss").format("YYYY-MM-DD HH:mm:ss");
          },
        },
      ],
      allCheck: true,
      MenuSelected: ["print", "ab", "c1", "c2", "c3", "c4", "tags", "memo"],
      importSelected: "add",
      MenuOptions: [
        { name: "출력용", value: "print", notEnabled: true },
        { name: "DAP(A, B)", value: "ab", notEnabled: true },
        { name: "태그", value: "tags", notEnabled: true },
        { name: "메모", value: "memo", notEnabled: true },
        { name: "C1", value: "c1", notEnabled: true },
        { name: "C2", value: "c2", notEnabled: true },
        { name: "C3", value: "c3", notEnabled: true },
        { name: "C4", value: "c4", notEnabled: true },
      ],
      importOptions: [
        { text: "덮어쓰기", value: "add" },
        { text: "붙여넣기", value: "update" },
      ],
    };
  },
  watch: {
    state: function (val) {
      this.getData();
      if (!val) {
        (this.MenuSelected = [
          "print",
          "ab",
          "c1",
          "c2",
          "c3",
          "c4",
          "tags",
          "memo",
        ]),
          this.MenuOptions.forEach((item) => {
            item.notEnabled = true;
          });
      }
    },
    selectedIds: function (val) {
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
  async created() {
    this.isTableLoading = true;
    const toDay = await this.GetDateString(this.date);
    const userName = sessionStorage.getItem(USER_NAME);
    const gropId = sessionStorage.getItem(ACCESS_GROP_ID);
    if (gropId === "S01G04C004") this.pramObj.person = userName;

    this.pramObj.brd_dt = toDay;
    this.searchItems.brd_dt = toDay;
    this.searchItems.pgmcode = this.cueInfo.pgmcode;
    this.pgmList = await this.GetPgmListByBrdDate(this.pramObj);

    let pgmCodeObj = [];
    if (this.type == "T") {
      pgmCodeObj = [...this.pgmList];
      this.disabledVal = false;
    } else {
      pgmCodeObj = this.pgmList.filter(
        (pgm) => pgm.pgmcode === this.cueInfo.pgmcode
      );
    }
    [this.mediaOptions, this.programOptions] = await Promise.all([
      this.SetMediaOption(this.pgmList),
      this.SetProgramCodeOption(this.pgmList),
    ]);

    this.searchItems.productid = await this.SetProductIds(pgmCodeObj);
    this.getData();
    this.selectedIds = [];
    this.searchItems.media = this.cueInfo.media;
  },
  computed: {
    ...mapGetters("cueList", ["abCartArr"]),
    ...mapGetters("cueList", ["printArr"]),
    ...mapGetters("cueList", ["defCuesheetListArr"]),
    ...mapGetters("cueList", ["cueInfo"]),
  },
  methods: {
    ...mapMutations("cueList", ["SET_CUEINFO"]),
    ...mapMutations("cueList", ["SET_ABCARTARR"]),
    ...mapMutations("cueList", ["SET_CCHANNELDATA"]),
    ...mapMutations("cueList", ["SET_PRINTARR"]),
    ...mapMutations("cueList", ["SET_TAGS"]),
    ...mapMutations("cueList", ["SET_DEFCUESHEETLISTARR"]),
    ...mapActions("cueList", ["disableList"]),
    ...mapActions("cueList", ["setStartTime"]),

    ...mapActions("cueList", ["GetDateString"]),
    ...mapActions("cueList", ["GetPgmListByBrdDate"]),
    ...mapActions("cueList", ["SetMediaOption"]),
    ...mapActions("cueList", ["SetProgramCodeOption"]),
    ...mapActions("cueList", ["SetProgramProductIdOption"]),
    ...mapActions("cueList", ["SetProductIds"]),

    async getData() {
      if (this.state) {
        //기본 큐시트 목록 가져오기
        this.searchItems.rowPerPage = Number(this.searchItems.rowPerPage);
        this.isTableLoading = this.isScrollLodaing ? false : true;
        var params = {
          productids: this.searchItems.productid,
          row_per_page: this.searchItems.rowPerPage,
          select_page: this.searchItems.selectPage,
        };
        await this.$http
          .post(`/api/DefCueSheet/GetDefList`, params)
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
      this.loadingIconVal = true;
      if (this.selectedIds == null || this.selectedIds.length == 0) {
        window.$notify("error", `기본큐시트를 선택하세요.`, "", {
          duration: 10000,
          permanent: false,
        });
        this.loadingIconVal = false;
      } else {
        let rowNum_ab = 0;
        let rowNum_print = 0;
        let beforePrintData = [];
        let beforeAbData = [];
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
          this.loadingIconVal = false;
        } else {
          var seqnum = this.selectedIds[0];
          var params = {
            productid: this.defCuesheetListArr.data[seqnum].productid,
            week: this.defCuesheetListArr.data[seqnum].weeks,
            pgmcode: this.cueInfo.pgmcode,
          };
          if (this.cueInfo.cuetype == "D") {
            if (Object.keys(this.cueInfo).includes("detail")) {
              params.brd_dt = this.cueInfo.brddate;
            } else {
              params.brd_dt = this.cueInfo.day;
            }
          }
          await this.$http
            .get(`/api/defcuesheet/GetdefCue`, {
              params: params,
              paramsSerializer: (params) => {
                return qs.stringify(params);
              },
            })
            .then((res) => {
              var responseCuesheetCollection = res.data.resultObject;
              if (this.MenuSelected.includes("print")) {
                if (beforePrintData.length > 0) {
                  responseCuesheetCollection.printDTO.forEach((ele) => {
                    ele.rownum = ele.rownum + beforePrintData.length;
                  });
                }
                var oldCueInfo = { ...this.cueInfo };
                oldCueInfo.r_ONAIRTIME = oldCueInfo.detail[0].onairtime;
                oldCueInfo.directorname =
                  responseCuesheetCollection.cueSheetDTO.directorname;
                oldCueInfo.djname =
                  responseCuesheetCollection.cueSheetDTO.djname;
                oldCueInfo.footertitle =
                  responseCuesheetCollection.cueSheetDTO.footertitle;
                oldCueInfo.headertitle =
                  responseCuesheetCollection.cueSheetDTO.headertitle;
                oldCueInfo.membername =
                  responseCuesheetCollection.cueSheetDTO.membername;
                var resultPrintData = beforePrintData.concat(
                  responseCuesheetCollection.printDTO
                );
                if (resultPrintData.length > 100) {
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
                  responseCuesheetCollection.normalCon.forEach((ele) => {
                    ele.rownum = ele.rownum + beforeAbData.length;
                  });
                }
                var resultABData = beforeAbData.concat(
                  responseCuesheetCollection.normalCon
                );
                if (resultABData.length > 500) {
                  resultABData.splice(500);
                  window.$notify("error", `최대 개수를 초과하였습니다.`, "", {
                    duration: 10000,
                    permanent: false,
                  });
                }
                this.SET_ABCARTARR(resultABData);
                eventBus.$emit("abDataSet");
              }
              //태그
              this.MenuSelected.includes("tags") &&
                this.SET_TAGS(responseCuesheetCollection.tags);
              //메모
              if (this.MenuSelected.includes("memo")) {
                this.cueInfo.memo = responseCuesheetCollection.cueSheetDTO.memo;
                this.SET_CUEINFO(this.cueInfo);
              }
              var pram = {
                data: responseCuesheetCollection.instanceCon,
                items: this.MenuSelected,
              };
              eventBus.$emit("updateCData", pram);
              this.selectedIds = null;
              this.loadingIconVal = false;
              this.cancel();
            });
        }
      }
    },
    async onMediaChange(e) {
      if (e === "") {
        this.programOptions = await this.SetProgramCodeOption(this.pgmList);
        this.searchItems.pgmcode = "";
        this.searchItems.productid = this.productIds;
      } else {
        const selectMediaObj = this.pgmList.filter((pgm) => pgm.media === e);
        const selectProgramList = await this.SetProductIds(selectMediaObj);
        this.programOptions = await this.SetProgramCodeOption(selectMediaObj);
        this.searchItems.pgmcode = "";
        this.searchItems.productid = selectProgramList;
      }
    },
    async onPgmChange(e) {
      if (e) {
        const selectPgmCodeObj = this.pgmList.filter(
          (pgm) => pgm.pgmcode === e
        );
        this.searchItems.productid = await this.SetProductIds(selectPgmCodeObj);
      }
    },
    async cancel() {
      this.state = false;
      if (this.type == "T") {
        this.searchItems.media = "";
        this.onMediaChange("");
      }
      this.$refs["importDef"].hide();
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