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
import { USER_ID, ACCESS_GROP_ID, USER_NAME } from "@/constants/config";
import { mapActions, mapGetters, mapMutations } from "vuex";
import MixinBasicPage from "../../mixin/MixinBasicPage";
import DxButton from "devextreme-vue/button";
import { eventBus } from "@/eventBus";
const qs = require("qs");
import axios from "axios";
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
      state: false,
      loadingIconVal: false,
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
          callback: (value) => {
            switch (value) {
              case "A":
                return "표준FM";
              case "F":
                return "FM4U";
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
      if (!val) {
        (this.MenuSelected = ["print", "ab", "c1", "c2", "c3", "c4"]),
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
        const userName = sessionStorage.getItem(USER_NAME);
        const gropId = sessionStorage.getItem(ACCESS_GROP_ID);
        this.isTableLoading = this.isScrollLodaing ? false : true;
        if (this.searchItems.productid == "") {
          var pram = { person: userName, gropId: gropId };
          var mediaOption = await this.getMediasOption(pram);
          if (this.type != "T") {
            var temmedia = await this.eventClick(this.cueInfo.media);
            this.searchItems.productid = this.cueInfo.productid;
          } else {
            this.searchItems.productid = this.userProList;
          }
        }
        if (this.searchItems.productid == undefined) {
          this.searchItems.productid = this.userProList;
        }

        var params = {
          productids: this.searchItems.productid,
          row_per_page: this.searchItems.rowPerPage,
          select_page: this.searchItems.selectPage,
        };
        if (typeof params.productids == "string") {
          params.productids = [params.productids];
        }
        await axios
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
          this.loadingIconVal = false;
        } else {
          var seqnum = this.selectedIds[0];
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
                oldCueInfo.r_ONAIRTIME = oldCueInfo.detail[0].onairtime;
                oldCueInfo.directorname = res.data.cueSheetDTO.directorname;
                oldCueInfo.djname = res.data.cueSheetDTO.djname;
                oldCueInfo.footertitle = res.data.cueSheetDTO.footertitle;
                oldCueInfo.headertitle = res.data.cueSheetDTO.headertitle;
                oldCueInfo.membername = res.data.cueSheetDTO.membername;
                oldCueInfo.memo = res.data.cueSheetDTO.memo;

                var resultPrintData = beforePrintData.concat(res.data.printDTO);
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
                  res.data.normalCon.forEach((ele) => {
                    ele.rownum = ele.rownum + beforeAbData.length;
                  });
                }
                var resultABData = beforeAbData.concat(res.data.normalCon);
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
              var pram = {
                data: res.data.instanceCon,
                items: this.MenuSelected,
              };
              eventBus.$emit("updateCData", pram);
              this.selectedIds = null;
              this.loadingIconVal = false;
              this.$refs["importDef"].hide();
            });
        }
      }
    },
    //매체 선택시 프로그램 목록 가져오기
    async eventClick(e) {
      const gropId = sessionStorage.getItem(ACCESS_GROP_ID);
      const userName = sessionStorage.getItem(USER_NAME);
      var pram = { person: userName, gropId: gropId, media: e };
      var proOption = await this.getuserProOption(pram);
      this.programList = this.userProOption;
      this.searchItems.productid = this.userProList;
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