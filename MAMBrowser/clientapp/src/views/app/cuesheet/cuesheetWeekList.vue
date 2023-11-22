<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="기본큐시트" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      :isDisplayBtnArea="true"
      @changeRowPerpage="onChangeRowPerpage"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            style="width: 100px"
            v-model="searchItems.media"
            :options="mediaOptions"
            @change="onMediaChange($event)"
          />
        </b-form-group>
        <!-- 프로그램명 -->
        <b-form-group label="프로그램명" class="has-float-label">
          <b-form-select
            style="width: 400px"
            v-model="searchItems.pgmcode"
            :options="programOptions"
            @change="onPgmChange($event)"
          />
        </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >검색</b-button
          >
        </b-form-group>
      </template>

      <!-- 버튼 -->
      <template slot="form-btn-area">
        <b-input-group>
          <b-button
            variant="outline-primary default"
            size="sm"
            @click="addModal"
            >기본 큐시트 추가</b-button
          >
        </b-input-group>
        <b-input-group>
          <b-button
            variant="outline-secondary default"
            size="sm"
            @click="selectDelDefCue"
            >선택 항목 삭제</b-button
          >
        </b-input-group>
      </template>

      <!-- 테이블 페이지 -->
      <template slot="form-table-page-area">
        {{ getPageInfo() }}
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <common-data-table-scroll-paging
          ref="scrollPaging"
          tableHeight="525px"
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
          <template slot="actions" scope="props">
            <common-actions :rowData="props.props.rowData"> </common-actions>
          </template>
          <template slot="weeks" scope="props">
            <common-weeks :rowData="props.props.rowData"> </common-weeks>
          </template>
        </common-data-table-scroll-paging>
      </template>
    </common-form>

    <!-- 기본큐시트 추가 modal -->
    <b-modal
      ref="modal-add"
      size="lg"
      centered
      title="기본 큐시트 추가"
      ok-title="확인"
      cancel-title="취소"
      @hidden="cancel_modal"
      @ok="addWeekCue"
    >
      <div id="modelDiv" class="d-block text-center">
        <div class="mb-4" style="font-size: 20px">
          <div>기본 큐시트를 추가할 프로그램을 선택하세요.</div>
        </div>
        <div class="modal_search">
          <b-form-group label="매체" class="has-float-label">
            <b-form-select
              style="width: 150px"
              v-model="searchItems_add_def_modal.media"
              :options="mediaOptions_modal"
              @change="onMediaChange_modal($event)"
            />
          </b-form-group>
          <b-form-group label="프로그램명" class="has-float-label ml-3">
            <b-form-select
              style="width: 400px"
              v-model="searchItems_add_def_modal.items"
              :options="programOptions_modal"
              @change="onPgmChange_modal($event)"
            />
          </b-form-group>
        </div>
        <div class="modal_week_form">
          <b-button-group size="sm">
            <b-button
              v-for="(btn, idx) in weekButtons"
              :key="idx"
              :pressed.sync="btn.state"
              :disabled="btn.disable"
              class="m-2 p-3"
              style="font-size: 16px; border-radius: 20% !important"
              variant="outline-primary"
            >
              {{ btn.caption }}
            </b-button>
          </b-button-group>
        </div>
      </div>
      <template #modal-footer>
        <DxButton :width="100" text="취소" @click="cancel_modal()" />
        <DxButton
          type="default"
          text="확인"
          styling-mode="outlined"
          :width="100"
          @click="addWeekCue()"
        >
        </DxButton>
      </template>
    </b-modal>

    <!-- 기본큐시트 삭제 modal -->
    <b-modal
      id="modal-del"
      size="lg"
      centered
      title="기본 큐시트 삭제"
      ok-title="확인"
      cancel-title="취소"
      @ok="delWeekCue"
    >
      <div id="modelDiv" class="d-block text-center">
        <div class="m-3" style="font-size: 20px">
          <div>선택된 기본 큐시트를 삭제하시겠습니까?</div>
        </div>
      </div>
      <template #modal-footer="{ cancel }">
        <DxButton :width="100" text="취소" @click="cancel()" />
        <DxButton
          type="default"
          text="확인"
          styling-mode="outlined"
          :width="100"
          @click="delWeekCue()"
        >
        </DxButton>
      </template>
    </b-modal>
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import { USER_ID, USER_NAME, CUESHEET_CODE } from "@/constants/config";
import DxButton from "devextreme-vue/button";
import "moment/locale/ko";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CommonWeeks from "../../../components/DataTable/CommonWeeks.vue";
const moment = require("moment");
const qs = require("qs");

export default {
  components: { CommonWeeks, DxButton },
  mixins: [MixinBasicPage],
  data() {
    return {
      date: new Date(),
      pramObj: { person: null, brd_dt: null, media: null, pgmcode: "NEW" },
      pgmList: [],
      programOptions: [],
      mediaOptions: [],
      productIds: [],
      selectMediaAllProductIds: [],
      searchItems: {
        media: "", // 매체
        pgmcode: "",
        productid: "", // 프로그램명
        cueid: -1, // 작성상태
        rowPerPage: 300,
        selectPage: 1,
      },
      mediaOptions_modal: [],
      programOptions_modal: [],
      searchItems_add_def_modal: {
        media: "",
        items: {},
      },
      selectAllMedia: "",
      isWeekSlot: true,
      weekButtons: [
        { caption: "월", value: "MON", state: false, disable: true },
        { caption: "화", value: "TUE", state: false, disable: true },
        { caption: "수", value: "WED", state: false, disable: true },
        { caption: "목", value: "THU", state: false, disable: true },
        { caption: "금", value: "FRI", state: false, disable: true },
        { caption: "토", value: "SAT", state: false, disable: true },
        { caption: "일", value: "SUN", state: false, disable: true },
      ],
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
        {
          name: "__slot:actions",
          title: "작업",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
        },
      ],
    };
  },

  computed: {
    ...mapGetters("cueList", ["defCuesheetListArr"]),
    ...mapGetters("user", ["behaviorList"]),
    btnWeekStates() {
      const result = this.weekButtons.map((btn) => {
        if (btn.state == true) {
          return btn.value;
        }
      });
      return result.filter((ele) => {
        return ele !== undefined;
      });
    },
  },
  async mounted() {
    this.isTableLoading = true;
    const toDay = await this.GetDateString(this.date);
    const userName = sessionStorage.getItem(USER_NAME);
    await this.renewal();
    const isCueAdmin = this.behaviorList.some(
      (data) => data.id === CUESHEET_CODE && data.visible === "Y"
    );
    if (!isCueAdmin) this.pramObj.person = userName;
    this.pramObj.brd_dt = toDay;
    this.searchItems.brd_dt = toDay;
    this.pgmList = await this.GetPgmListByBrdDate(this.pramObj);
    [
      this.mediaOptions,
      this.programOptions,
      this.programOptions_modal,
      this.productIds,
    ] = await Promise.all([
      this.SetMediaOption(this.pgmList),
      this.SetProgramCodeOption(this.pgmList),
      this.SetProgramProductIdOption(this.pgmList),
      this.SetProductIds(this.pgmList),
    ]);
    this.mediaOptions_modal = [...this.mediaOptions];
    this.getData();
    this.selectedIds = [];
  },
  methods: {
    ...mapActions("cueList", ["GetDateString"]),
    ...mapActions("cueList", ["getcuesheetListArrDef"]),
    ...mapActions("cueList", ["GetPgmListByBrdDate"]),
    ...mapActions("cueList", ["SetMediaOption"]),
    ...mapActions("cueList", ["SetProgramCodeOption"]),
    ...mapActions("cueList", ["SetProgramProductIdOption"]),
    ...mapActions("cueList", ["SetProductIds"]),
    ...mapActions("cueList", ["enableNotification"]),
    ...mapActions("user", ["renewal"]),
    async getData() {
      this.searchItems.rowPerPage = Number(this.searchItems.rowPerPage);
      this.isTableLoading = this.isScrollLodaing ? false : true;
      if (this.searchItems.productid === "")
        this.searchItems.productid = this.productIds;
      const params = {
        productids: this.searchItems.productid,
        row_per_page: this.searchItems.rowPerPage,
        select_page: this.searchItems.selectPage,
      };
      const arrListResult = await this.getcuesheetListArrDef(params);
      if (arrListResult) {
        this.setResponseData(arrListResult);
      }
      this.addScrollClass();
      this.isTableLoading = false;
      this.isScrollLodaing = false;
    },
    async onMediaChange(e) {
      if (e === "") {
        this.programOptions = await this.SetProgramCodeOption(this.pgmList);
        this.searchItems.pgmcode = "";
        this.searchItems.productid = this.productIds;
      } else {
        const selectMediaObj = this.pgmList.filter((pgm) => pgm.media === e);
        this.selectMediaAllProductIds = await this.SetProductIds(
          selectMediaObj
        );
        this.programOptions = await this.SetProgramCodeOption(selectMediaObj);
        this.searchItems.pgmcode = "";
        this.searchItems.productid = this.selectMediaAllProductIds;
      }
      this.onSearch();
    },
    async onMediaChange_modal(e) {
      if (e === "") {
        this.programOptions_modal = await this.SetProgramProductIdOption(
          this.pgmList
        );
        this.searchItems_add_def_modal.items = {};
      } else {
        const selectMediaObj = this.pgmList.filter((pgm) => pgm.media === e);
        this.programOptions_modal = await this.SetProgramProductIdOption(
          selectMediaObj
        );
        this.searchItems_add_def_modal.items = {};
      }
    },
    async onPgmChange(e) {
      if (e) {
        const selectPgmCodeObj = this.pgmList.filter(
          (pgm) => pgm.pgmcode === e
        );
        this.searchItems.productid = await this.SetProductIds(selectPgmCodeObj);
      } else {
        if (this.searchItems.media) {
          this.searchItems.productid = this.selectMediaAllProductIds;
        } else {
          this.searchItems.productid = this.productIds;
        }
      }
    },
    async onPgmChange_modal(e) {
      if (!e) {
        this.weekButtons.forEach((ele) => {
          ele.state = false;
          ele.disable = true;
        });
      } else {
        this.selectAllMedia = e.media;
        this.weekButtons.forEach((week) => {
          week.state = false;
        });
        var params = {
          productids: [e.productid],
          row_per_page: this.searchItems.rowPerPage,
          select_page: this.searchItems.selectPage,
        };
        await this.$http
          .post(`/api/DefCueSheet/GetDefList`, params)
          .then((res) => {
            var weekArr = [];
            res.data.resultObject.data.forEach((ele) => {
              ele.detail.forEach((week) => {
                weekArr.push(week.week);
              });
            });
            this.weekButtons.forEach((week) => {
              if (weekArr.includes(week.value)) {
                week.disable = true;
              } else {
                week.disable = false;
              }
            });
            if (weekArr.length == 7) {
              //이미 모든요일이 설정되어 있을때 해야함
            }
          });
      }
    },
    async addModal() {
      this.weekButtons.forEach((ele) => {
        ele.state = false;
        ele.disable = true;
      });
      this.$refs["modal-add"].show();
    },
    selectDelDefCue() {
      if (this.selectedIds?.length > 0) {
        this.$bvModal.show("modal-del");
      } else {
        this.enableNotification({
          type: "error",
          message: `삭제할 기본 큐시트를 선택하세요.`,
        });
      }
    },
    async addWeekCue() {
      const userId = sessionStorage.getItem(USER_ID);
      let result = [];
      this.btnWeekStates?.forEach((ele) => {
        var cueItem = {
          personid: userId,
          detail: [{ cueid: -1, week: ele }],
          productid: this.searchItems_add_def_modal.items.productid,
          djname: "",
          directorname: "",
          membername: "",
          headertitle: "",
          footertitle: "",
          memo: "",
        };
        this.searchItems_add_def_modal.media
          ? (cueItem.media = this.searchItems_add_def_modal.media)
          : (cueItem.media = this.selectAllMedia);
        result.push(cueItem);
      });
      if (result.length == 0) {
        this.enableNotification({
          type: "error",
          message: `기본 큐시트를 적용할 요일을 선택하세요.`,
        });
      } else {
        const pram = {
          DefCueSheetDTO: result,
        };
        await this.$http
          .post(`/api/DefCueSheet/SaveDefCue`, pram)
          .then((res) => {
            this.enableNotification({
              type: "info",
              message: `기본 큐시트 추가완료.`,
            });
          });
        this.getData();
        this.$refs["modal-add"].hide();
      }
    },
    async cancel_modal() {
      this.searchItems_add_def_modal.items = {};
      this.searchItems_add_def_modal.media = "";
      this.programOptions_modal = await this.SetProgramProductIdOption(
        this.pgmList
      );
      this.$refs["modal-add"].hide();
    },
    async delWeekCue(e) {
      const delcueidList = [];
      this.selectedIds?.forEach((ids) => {
        this.defCuesheetListArr.data[ids].detail.forEach((ele) => {
          delcueidList.push(ele.cueid);
        });
      });
      await this.$http
        .delete(`/api/DefCueSheet/DelDefCue`, {
          params: {
            delParams: delcueidList,
          },
          paramsSerializer: (params) => {
            return qs.stringify(params);
          },
        })
        .then((res) => {
          this.enableNotification({
            type: "info",
            message: `기본 큐시트 삭제완료.`,
          });
        })
        .catch(() => {
          this.enableNotification({
            type: "error",
            message: `기본 큐시트 삭제실패.`,
          });
        });
      this.getData();
      this.initSelectedIds();
      this.$bvModal.hide("modal-del");
    },
  },
};
</script>
<style>
.modal_search {
  width: 700px;
  text-align: center;
  margin: auto;
}
.modal_search fieldset {
  display: inline-block;
}
.modal_week_form {
  margin-top: 10px;
  height: 150px;
  border: 1px solid #d7d7d7;
  display: table-cell;
  vertical-align: middle;
  width: 800px;
}
.modal_week_form .btn-outline-primary:disabled {
  border: solid 1px #757575;
  background-color: rgb(223, 222, 222);
  color: #757575;
}
.dx-button {
  font-family: "MBC 새로움 M";
}
</style>
