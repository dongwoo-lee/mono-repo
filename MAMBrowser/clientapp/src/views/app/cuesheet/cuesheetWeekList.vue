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
            :options="mediasOption"
            @change="eventClick($event, 'list')"
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
              v-model="cuesheetData.media"
              :options="modal_mediasoption"
              @change="eventClick($event, 'modal')"
            />
          </b-form-group>
          <b-form-group label="프로그램명" class="has-float-label ml-3">
            <b-form-select
              style="width: 400px"
              v-model="cuesheetData.productid"
              :options="modalProgramList"
              @change="getWeekList($event)"
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
      <template #modal-footer="{ cancel }">
        <DxButton :width="100" text="취소" @click="cancel()" />
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
import { mapActions, mapGetters, mapMutations } from "vuex";
import { USER_ID, ACCESS_GROP_ID, USER_NAME } from "@/constants/config";
import DxButton from "devextreme-vue/button";
import axios from "axios";
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
      isWeekSlot: true,
      programList: [{ value: "", text: "매체를 선택하세요" }],
      modalProgramList: [{ value: "", text: "매체를 선택하세요" }],
      searchItems: {
        media: "", // 매체
        productid: "", // 프로그램명
        cueid: -1, // 작성상태
        rowPerPage: 30,
        selectPage: 1,
      },
      weekButtons: [
        { caption: "월", value: "MON", state: false, disable: true },
        { caption: "화", value: "TUE", state: false, disable: true },
        { caption: "수", value: "WED", state: false, disable: true },
        { caption: "목", value: "THU", state: false, disable: true },
        { caption: "금", value: "FRI", state: false, disable: true },
        { caption: "토", value: "SAT", state: false, disable: true },
        { caption: "일", value: "SUN", state: false, disable: true },
      ],
      cuesheetData: {
        edittime: "",
        media: "",
        personid: "",
        productid: "",
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
        {
          name: "__slot:actions",
          title: "작업",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
        },
      ],
      modal_mediasoption: [],
    };
  },

  computed: {
    ...mapGetters("cueList", ["defCuesheetListArr"]),
    ...mapGetters("cueList", ["userProOption"]),
    ...mapGetters("cueList", ["mediasOption"]),
    ...mapGetters("cueList", ["userProList"]),
    btnWeekStates() {
      var result = this.weekButtons.map((btn) => {
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
    this.selectedIds = [];
    this.getData();
    await this.getProductName();
    this.programList = this.userProOption;
    this.searchItems.productid = this.userProList;
    this.modalProgramList = this.userProOption;
    this.modal_mediasoption = this.mediasOption.filter(
      (ele, index) => index > 0
    );
  },
  methods: {
    ...mapActions("cueList", ["getcuesheetListArrDef"]),
    ...mapActions("cueList", ["getMediasOption"]),
    ...mapActions("cueList", ["getuserProOption"]),
    async getData() {
      this.searchItems.rowPerPage = Number(this.searchItems.rowPerPage);
      const gropId = sessionStorage.getItem(ACCESS_GROP_ID);
      const userName = sessionStorage.getItem(USER_NAME);
      this.isTableLoading = this.isScrollLodaing ? false : true;
      if (this.searchItems.productid == "") {
        var pram = { person: userName, gropId: gropId };
        await this.getMediasOption(pram);
        this.searchItems.productid = this.userProList;
      }
      if (this.searchItems.productid == undefined) {
        this.searchItems.productid = this.userProList;
      }
      var params = {
        productids: this.searchItems.productid,
        row_per_page: this.searchItems.rowPerPage,
        select_page: this.searchItems.selectPage,
      };
      var arrListResult = await this.getcuesheetListArrDef(params);
      this.setResponseData(arrListResult);
      this.addScrollClass();
      this.isTableLoading = false;
      this.isScrollLodaing = false;
    },
    //매체 선택시 프로그램 목록 가져오기 (일반,modal)
    async eventClick(e, V) {
      await this.getProductName(e);
      if (V == "list") {
        this.programList = this.userProOption;
        this.searchItems.productid = this.userProList;
      } else if (V == "modal") {
        this.modalProgramList = this.userProOption;
        this.cuesheetData.productid = "";
      }
    },
    async getProductName(media) {
      const gropId = sessionStorage.getItem(ACCESS_GROP_ID);
      const userName = sessionStorage.getItem(USER_NAME);
      var pram = { person: userName, gropId: gropId, media: media };
      var proOption = await this.getuserProOption(pram);
    },
    async addModal() {
      this.cuesheetData.media = this.mediasOption[1].value;
      await this.getProductName(this.cuesheetData.media);
      this.modalProgramList = this.userProOption;
      this.cuesheetData.productid = "";
      this.weekButtons.forEach((ele) => {
        ele.state = false;
        ele.disable = true;
      });
      this.$refs["modal-add"].show();
    },
    selectDelDefCue() {
      if (this.selectedIds.length > 0) {
        this.$bvModal.show("modal-del");
      } else {
        window.$notify("error", `삭제할 기본 큐시트를 선택하세요.`, "", {
          duration: 10000,
          permanent: false,
        });
      }
    },
    // 기본큐시트 추가 (modal)
    async addWeekCue() {
      const userId = sessionStorage.getItem(USER_ID);
      var result = [];
      this.btnWeekStates.forEach((ele) => {
        var cueItem = {
          personid: userId,
          detail: [{ cueid: -1, week: ele }],
          media: this.cuesheetData.media,
          productid: this.cuesheetData.productid,
          djname: "",
          directorname: "",
          membername: "",
          headertitle: "",
          footertitle: "",
          memo: "",
        };
        result.push(cueItem);
      });
      if (result.length == 0) {
        window.$notify("error", `기본 큐시트를 적용할 요일을 선택하세요.`, "", {
          duration: 10000,
          permanent: false,
        });
      } else {
        var pram = {
          DefCueSheetDTO: result,
        };
        await this.$http
          .post(`/api/DefCueSheet/SaveDefCue`, pram)
          .then((res) => {
            window.$notify("info", `기본 큐시트 추가완료.`, "", {
              duration: 10000,
              permanent: false,
            });
          });
        this.getData();
        this.$refs["modal-add"].hide();
      }
    },
    // 요일확인 및 목록 가져오기 (modal)
    async getWeekList(e) {
      if (e == undefined || "" || null) {
        this.weekButtons.forEach((ele) => {
          ele.state = false;
          ele.disable = true;
        });
      } else {
        this.weekButtons.forEach((week) => {
          week.state = false;
        });
        var params = {
          productids: [e],
          row_per_page: this.searchItems.rowPerPage,
          select_page: this.searchItems.selectPage,
        };
        await axios.post(`/api/DefCueSheet/GetDefList`, params).then((res) => {
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
    //기본큐시트 삭제
    async delWeekCue(e) {
      var delcueidList = [];
      this.selectedIds.forEach((ids) => {
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
          window.$notify("info", `기본 큐시트 삭제완료.`, "", {
            duration: 10000,
            permanent: false,
          });
        })
        .catch(() => {
          window.$notify("error", `기본 큐시트 삭제실패.`, "", {
            duration: 10000,
            permanent: false,
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