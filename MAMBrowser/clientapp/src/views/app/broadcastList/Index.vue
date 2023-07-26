<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="송출리스트" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      :isDisplayBtnArea="true"
      :isDisplayPageSize="false"
      @changeRowPerpage="onChangeRowPerpage"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 방송일 -->
        <b-form-group label="방송일" class="has-float-label">
          <common-date-picker
            required
            v-model="searchItems.brdDate"
            @changeDatePicker="changeDate"
          />
        </b-form-group>
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            style="width: 100px"
            v-model="searchItems.media"
            :options="mediaOptions"
            value-field="id"
            text-field="name"
            @change="onSearch"
          />
        </b-form-group>
        <!-- 조회 버튼
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >조회</b-button
          >
        </b-form-group> -->
        <b-form-checkbox
          id="checkbox-1"
          class="m-1"
          v-model="searchItems.productType"
          value="P"
          unchecked-value=""
          @change="onSearch"
        >
          프로그램만 보기
        </b-form-checkbox>
      </template>
      <!-- 테이블 페이지 -->
      <template slot="form-table-page-area">
        {{ getTotalRowCount() }}
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <common-data-table-scroll-paging
          id="broadcast_list_table"
          ref="scrollPaging"
          tableHeight="525px"
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          :totalCount="responseData.totalRowCount"
          is-actions-slot
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @refresh="onRefresh"
        >
          <template slot="rowNO" scope="props">
            <div>
              <span
                v-b-tooltip.hover
                :title="'SEQNUM : ' + props.props.rowData.seqnum"
              >
                {{ props.props.rowData.rowno }}</span
              >
            </div>
          </template>
          <template slot="studioId" scope="props">
            <div>
              <a href="#" v-if="props.props.rowData.studioid">{{
                props.props.rowData.mainmachine
              }}</a>
              <span v-else>{{ props.props.rowData.mainmachine }}</span>
            </div>
          </template>
          <template slot="productID" scope="props">
            <div>
              <span
                v-b-tooltip.hover
                :title="'SOURCEID : ' + props.props.rowData.sourceid"
              >
                {{ props.props.rowData.productid }}</span
              >
            </div>
          </template>
          <template slot="programName" scope="props">
            <div>
              <a
                href="#"
                v-if="
                  props.props.rowData.producttype == 'P' &&
                  props.props.rowData.eventmodf != 'C' &&
                  props.props.rowData.eventmodf != 'T'
                "
                @click="goProgramInfo(props.props.rowData)"
                >{{ props.props.rowData.eventname }}</a
              >
              <span v-else>{{ props.props.rowData.eventname }}</span>
            </div>
          </template>
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :isBroadcastConfigAction="true"
              :cueParam="searchItems"
              @preview="onPreview"
              @clickMusicSelectionListBtn="onMusicSelectionList"
            />
          </template>
        </common-data-table-scroll-paging>
      </template>
    </common-form>
    <PlayerPopup
      :showPlayerPopup="showPlayerPopup"
      :title="soundItem.eventname"
      :fileKey="soundItem.fileToken"
      :streamingUrl="streamingUrl"
      :waveformUrl="waveformUrl"
      :tempDownloadUrl="tempDownloadUrl"
      requestType="token"
      direct="Y"
      @closePlayer="onClosePlayer"
    >
    </PlayerPopup>
    <TableView
      title="선곡리스트"
      :id="modalId"
      :data-source="responseData.data"
      :column-data="fields"
    />
  </div>
</template>

<script>
import { mapActions } from "vuex";
import "moment/locale/ko";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import TableView from "../../../components/Modal/CommonTableViewModal.vue";
const moment = require("moment");

export default {
  mixins: [MixinBasicPage],
  data() {
    const now = new Date();
    const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    const nextMonth = new Date(today);
    nextMonth.setMonth(nextMonth.getMonth() + 1);

    return {
      date: new Date(),
      modalId: "music_selection_list_modal",
      searchItems: {
        // 최신 데이터 없음으로 특정 송출리스트로 테스트 중
        brdDate: "20221001",
        // brdDate: null,
        media: "A",
        productType: "P",
        rowPerPage: 30,
        selectPage: 1,
      },
      fields: [
        {
          name: "__slot:rowNO",
          title: "순서",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "4%",
        },
        // {
        //   name: "producttype",
        //   title: "타입",
        //   titleClass: "center aligned text-center",
        //   dataClass: "center aligned text-center",
        //   width: "6%",
        // },
        {
          name: "__slot:studioId",
          // name: "mainmachine",
          title: "메인소스",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%",
        },
        {
          // name: "__slot:studioId",
          name: "studioid",
          title: "스튜디오ID",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%",
        },
        {
          name: "__slot:productID",
          title: "프로그램 ID",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
        },
        {
          name: "onairtime",
          title: "방송시작시간",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "8%",
          callback: (value) => {
            return value === null
              ? ""
              : moment(value, "YYYY-MM-DD'T'HH:mm:ss").format("HH:mm:ss");
          },
        },
        {
          name: "duration",
          title: "방송길이",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
          callback: (value) => {
            return value === null
              ? ""
              : moment.unix(value).subtract(9, "hours").format("HH:mm:SS");
          },
        },
        {
          name: "__slot:programName",
          title: "프로그램이름",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
        },
        {
          name: "tdname",
          title: "기술감독",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
          callback: (value) => {
            return value === "N" ? "" : value;
          },
        },
        {
          name: "__slot:actions",
          title: "작업",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "28%",
        },
      ],
    };
  },
  components: {
    TableView,
  },
  async mounted() {
    const toDay = await this.GetDateString(this.date);
    // 최신 데이터 없음으로 특정 송출리스트로 테스트 중
    // this.searchItems.brdDate = toDay;
    this.getMediaOptions();
    this.getData();
  },
  methods: {
    ...mapActions("cueList", ["GetDateString"]),
    async getData() {
      await this.setData();
      this.nowPgmRowMoveFocus();
      this.pgmRowColor();
    },
    async setData() {
      this.searchItems.rowPerPage = Number(this.searchItems.rowPerPage);
      this.isTableLoading = this.isScrollLodaing ? false : true;
      const params = {
        brdDate: this.searchItems.brdDate,
        media: this.searchItems.media,
        producTtype: this.searchItems.productType,
        rowPerPage: this.searchItems.rowPerPage,
        selectPage: this.searchItems.selectPage,
      };
      const dataList = await this.getReturnList(params);
      if (dataList) {
        this.setResponseData(dataList);
      }
      this.addScrollClass();
      this.isTableLoading = false;
      this.isScrollLodaing = false;
    },
    getReturnList(params) {
      const url = "/api/TransMissionList/GetTransMissionList";
      return this.$http.post(url, params).then((res) => {
        if (res.status === 200 && res.data.resultObject) {
          return res;
        }
      });
    },
    async changeDate(date) {
      this.searchItems.brdDate = date;
      await this.getData();
    },
    nowPgmRowMoveFocus() {
      // 최신 데이터 없음으로 특정 송출리스트로 테스트 중
      // const currentTime = new Date();
      const currentTime = new Date(2022, 9, 1, 11, 6, 0);

      const rowItems = this.responseData.data.filter((row) => {
        return new Date(row.onairtime) < currentTime;
      });
      if (
        rowItems.length > 0 &&
        new Date(
          this.responseData.data[this.responseData.data.length - 1].onairtime
        )
          .toISOString()
          .slice(0, 10) == currentTime.toISOString().slice(0, 10)
      ) {
        const tableElement = this.$refs.scrollPaging.$el;
        const vuetableElement = tableElement.querySelector(
          `.vuetable-body-wrapper`
        );
        const rows = tableElement.querySelectorAll(`tbody tr`);
        const targetRow = rows[rowItems[rowItems.length - 1].rowno];
        targetRow.classList.add("focus_tr");
        vuetableElement.scrollTo({
          top:
            targetRow.offsetTop -
            vuetableElement.offsetHeight / 2 +
            targetRow.offsetHeight / 2,
          behavior: "smooth",
        });
      } else {
        const elements = document.getElementsByClassName("focus_tr");
        for (let i = 0; i < elements.length; i++) {
          elements[i].classList.remove("focus_tr");
        }
        const tableElement = this.$refs.scrollPaging.$el;
        const vuetableElement = tableElement.querySelector(
          `.vuetable-body-wrapper`
        );
        const rows = tableElement.querySelectorAll(`tbody tr`);
        const targetRow = rows[0];
        vuetableElement.scrollTo({
          top: targetRow.offsetTop,
        });
      }
    },
    pgmRowColor() {
      const tableElement = this.$refs.scrollPaging.$el;
      const rows = tableElement.querySelectorAll(`tbody tr`);
      this.responseData.data.forEach((ele) => {
        const targetRow = rows[ele.rowno - 1];
        targetRow.classList.remove("pgm_tr");
      });
      if (this.searchItems.productType != "P") {
        const rowItems = this.responseData.data.filter((row) => {
          return (
            row.producttype == "P" &&
            row.eventmodf != "C" &&
            row.eventmodf != "T"
          );
        });
        rowItems.forEach((ele) => {
          const targetRow = rows[ele.rowno - 1];
          targetRow.classList.add("pgm_tr");
        });
      }
    },
    //선곡리스트 가져오기
    onMusicSelectionList() {
      this.$bvModal.show(this.modalId);
    },
    goProgramInfo(rowData) {
      console.log(rowData);
      // window.open("/app/monitoring/programInfo", "_blank");
    },
  },
};
</script>
<style>
#broadcast_list_table table {
  border-collapse: collapse;
}
.focus_tr {
  border: 2px solid darkgray !important;
}
.pgm_tr {
  background-color: #cce8f5 !important;
}
</style>
