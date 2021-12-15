<template>
  <b-modal
    id="CommonImportArchive"
    ref="importArchive"
    size="xl"
    title="이전큐시트 가져오기"
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
          <!-- 시작일 ~ 종료일 -->
          <common-start-end-date-picker
            startDateLabel="방송 시작일"
            endDateLabel="방송 종료일"
            :startDate.sync="searchItems.start_dt"
            :endDate.sync="searchItems.end_dt"
            :required="false"
            :isCurrentDate="false"
          />
          <!-- 매체 -->
          <b-form-group label="매체" class="has-float-label">
            <b-form-select
              style="width: 100px"
              v-model="searchItems.media"
              :options="mediasOption"
              @change="eventClick($event)"
            />
          </b-form-group>
          <!-- 프로그램명 -->
          <b-form-group label="프로그램명" class="has-float-label">
            <b-form-select
              style="width: 400px"
              v-model="searchItems.productid"
              :options="programList"
            />
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
import { USER_ID } from "@/constants/config";
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

var endDay = get_date_str(date);

date.setDate(date.getDate() - 60); //2달
var startDay = get_date_str(date);
export default {
  mixins: [MixinBasicPage],
  data() {
    return {
      state: false,
      date,
      programList: [{ value: "", text: "매체를 선택하세요" }],
      searchItems: {
        start_dt: "", // 시작일
        end_dt: "", // 종료일
        media: "", // 매체
        productid: "", // 프로그램명
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
          name: "brdtime",
          title: "방송완료일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "15%",
          callback: (value) => {
            return value === null
              ? ""
              : moment(value, "YYYYMMDD").format("YYYY-MM-DD (ddd)");
          },
        },
        {
          name: "title",
          title: "프로그램명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "title",
        },
        {
          name: "media",
          title: "매체",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          sortField: "media",
          width: "15%",
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
          name: "brdtime",
          title: "방송시간",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%",
          callback: (value) => {
            return value === null
              ? ""
              : moment(value, "YYYY-MM-DD'T'HH:mm:ss").format("HH:mm");
          },
        },
        {
          name: "__slot:actions",
          title: "",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
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
    ...mapGetters("cueList", ["archiveCuesheetListArr"]),
    ...mapGetters("cueList", ["userProOption"]),
    ...mapGetters("cueList", ["mediasOption"]),
    ...mapGetters("cueList", ["userProList"]),
  },
  mounted() {
    this.searchItems.start_dt = startDay;
    this.searchItems.end_dt = endDay;
    //this.getData();
  },
  methods: {
    ...mapMutations("cueList", ["SET_CUEINFO"]),
    ...mapMutations("cueList", ["SET_ABCARTARR"]),
    ...mapMutations("cueList", ["SET_CCHANNELDATA"]),
    ...mapMutations("cueList", ["SET_PRINTARR"]),
    ...mapMutations("cueList", ["SET_ARCHIVECUESHEETLISTARR"]),
    ...mapActions("cueList", ["getarchiveCuesheetListArr"]),
    ...mapActions("cueList", ["getMediasOption"]),
    ...mapActions("cueList", ["getuserProOption"]),
    ...mapActions("cueList", ["setStartTime"]),

    async getData() {
      const userId = sessionStorage.getItem(USER_ID);
      if (this.state) {
        this.isTableLoading = this.isScrollLodaing ? false : true;
        if (
          this.$fn.checkGreaterStartDate(
            this.searchItems.start_dt,
            this.searchItems.end_dt
          )
        ) {
          this.$fn.notify("error", {
            message: "시작 날짜가 종료 날짜보다 큽니다.",
          });
          this.hasErrorClass = true;
          return;
        }
        if (this.searchItems.productid == "") {
          await this.getMediasOption({
            brd_dt: this.searchItems.start_dt,
            personid: userId,
            gropId: null,
          });
          this.searchItems.productid = this.userProList;
        }
        await axios
          .get(`/api/ArchiveCueSheet/GetArchiveCueList`, {
            params: {
              start_dt: this.searchItems.start_dt,
              end_dt: this.searchItems.end_dt,
              products: this.searchItems.productid,
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
            this.SET_ARCHIVECUESHEETLISTARR(res.data.resultObject);
            this.setResponseData(res);
            this.addScrollClass();
            this.isTableLoading = false;
            this.isScrollLodaing = false;
          });
      }
    },
    //매체 선택시 프로그램 목록 가져오기
    async eventClick(e) {
      const userId = sessionStorage.getItem(USER_ID);

      var proOption = await this.getuserProOption({
        brd_dt: this.searchItems.start_dt,
        personid: userId,
        gropId: null,
        media: e,
      });
      this.programList = this.userProOption;
    },
    async ok() {
      if (this.selectedIds == null || this.selectedIds.length == 0) {
        window.$notify("error", `큐시트를 선택하세요.`, "", {
          duration: 10000,
          permanent: false,
        });
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
          window.$notify("error", `가져올 항목을 선택하세요.`, "", {
            duration: 10000,
            permanent: false,
          });
        } else {
          var seqnum = this.selectedIds[0];
          var cueid = this.archiveCuesheetListArr.data[seqnum].cueid;
          await axios
            .get(`/api/ArchiveCueSheet/GetArchiveCue`, {
              params: {
                cueid: cueid,
              },
              paramsSerializer: (params) => {
                return qs.stringify(params);
              },
            })
            .then((res) => {
              //const cueSheetCons = res.data.cueSheetCons;
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
              this.selectedIds = null;
              this.$refs["importArchive"].hide();
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