<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="편성표" />
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
        <!-- 조회 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >조회</b-button
          >
        </b-form-group>
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
          <template slot="studioName" scope="props">
            <div>
              <a
                href="#"
                v-if="props.props.rowData.studioname"
                @click="goStudioInfo(props.props.rowData)"
                >{{ props.props.rowData.mainmachine }}</a
              >
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
              @preview="onPreview"
              @clickMusicSelectionListBtn="onMusicSelectionList"
              @goCueSheetDate="onGoCueSheetDate"
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
      :title="playlist_modal_title"
      :id="modalId"
      :data-source="playlistDataSource"
      :column-data="playlist_fields"
    />
  </div>
</template>

<script>
import { mapGetters, mapActions, mapMutations } from "vuex";
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
        // brdDate: "20221001",
        brdDate: null,
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
        {
          name: "__slot:studioName",
          title: "메인소스",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%",
        },
        {
          name: "studioname",
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
      playlist_fields: [
        {
          dataField: "rowno",
          width: "50",
          caption: "순서",
          alignment: "center",
          allowSorting: false,
        },
        {
          dataField: "musicid",
          caption: "음악ID",
          width: "130",
          alignment: "center",
          allowSorting: false,
        },
        {
          dataField: "startdtm",
          caption: "송출 시작 일시",
          width: "200",
          alignment: "center",
          allowSorting: false,
        },
        {
          dataField: "songname",
          caption: "곡명",
          alignment: "center",
          allowSorting: false,
        },
        {
          dataField: "artist",
          caption: "가수",
          alignment: "center",
          allowSorting: false,
        },
        {
          cellTemplate: "totaltime_template",
          caption: "음원 길이",
          width: "100",
          alignment: "center",
          allowSorting: false,
        },
        {
          cellTemplate: "playtime_template",
          caption: "송출 길이",
          width: "100",
          alignment: "center",
          allowSorting: false,
        },
      ],
      playlist_modal_title: "",
      playlistDataSource: [],
    };
  },
  components: {
    TableView,
  },
  computed: {
    ...mapGetters("cueList", ["cueInfo"]),
  },
  async mounted() {
    const toDay = await this.GetDateString(this.date);
    // 최신 데이터 없음으로 특정 송출리스트로 테스트 중
    this.searchItems.brdDate = toDay;
    this.getMediaOptions();
    this.getData();
  },
  methods: {
    ...mapActions("cueList", ["GetDateString"]),
    ...mapActions("cueList", ["getcuesheetListArr"]),
    ...mapActions("cueList", ["getarchiveCuesheetListArr"]),
    ...mapMutations("cueList", ["SET_CUEINFO"]),
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
        if (res.status === 200) {
          return res;
        }
      });
    },
    async changeDate(date) {
      this.searchItems.brdDate = date;
      await this.getData();
    },
    nowPgmRowMoveFocus() {
      // 이전에 남아있는 포커스 css 삭제
      const focus_ele = document.querySelector(".focus_tr");
      if (focus_ele) {
        focus_ele.classList.remove("focus_tr");
      }
      // 최신 데이터 없음으로 특정 송출리스트로 테스트 중
      const currentTime = new Date();
      // const currentTime = new Date(2022, 9, 1, 11, 6, 0);

      if (this.responseData.data) {
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
          const targetRow = rows[rowItems[rowItems.length - 2].rowno];
          targetRow.classList.add("focus_tr");
          vuetableElement.scrollTo({
            top:
              targetRow.offsetTop -
              vuetableElement.offsetHeight / 2 +
              targetRow.offsetHeight / 2,
            behavior: "smooth",
          });
        }
      }
    },
    pgmRowColor() {
      const tableElement = this.$refs.scrollPaging.$el;
      const rows = tableElement.querySelectorAll(`tbody tr`);
      if (this.responseData.data) {
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
      }
    },
    //선곡리스트 가져오기
    async onMusicSelectionList(rowData) {
      const url = "/api/PlaylistPerBrdProgram/GetPlaylistProgram";
      this.playlist_modal_title =
        "[" +
        moment(this.searchItems.brdDate, "YYYYMMDD").format("YYYY-MM-DD") +
        "] " +
        rowData.eventname +
        " 선곡리스트";

      const param = {
        brddate: this.searchItems.brdDate,
        productid: rowData.productid,
      };
      await this.$http.post(url, param).then((res) => {
        if (res.status === 200 && res.data.resultObject) {
          this.playlistDataSource = res.data.resultObject.data;
        }
      });
      // playlistDataSource
      this.$bvModal.show(this.modalId);
    },
    goProgramInfo(rowData) {
      window.open(
        "/app/monitoring/programInfo?brdDate=" +
          this.searchItems.brdDate +
          "&media=" +
          this.searchItems.media +
          "&pgmcode=" +
          rowData.pgmcode,
        "_blank"
      );
    },
    goStudioInfo(rowData) {
      window.open(
        "/app/monitoring/studio?brdDate=" +
          this.searchItems.brdDate +
          "&studioname=" +
          rowData.studioname,
        "_blank"
      );
    },
    async onGoCueSheetDate(rowData) {
      const param = {
        start_dt: this.searchItems.brdDate,
        end_dt: this.searchItems.brdDate,
        brd_dt: this.searchItems.brdDate,
        products: [rowData.productid],
        media: this.searchItems.media,
        row_per_page: 30,
        select_page: 1,
      };
      //이전큐시트 목록에서 확인 후 Go Page
      const archiveItem = await this.getarchiveCuesheetListArr(param);
      if (archiveItem.data.resultObject.data.length == 1) {
        this.SET_CUEINFO(archiveItem.data.resultObject.data[0]);
        sessionStorage.setItem(
          "USER_INFO",
          JSON.stringify(archiveItem.data.resultObject.data[0])
        );
        window.open("/app/cuesheet/previous/detail", "_blank");
      } else {
        //일일큐시트 목록에서 확인 후 Go Page
        const cueItem = await this.getcuesheetListArr(param);
        if (cueItem.data.resultObject.data.length == 1) {
          this.SET_CUEINFO(cueItem.data.resultObject.data[0]);
          sessionStorage.setItem(
            "USER_INFO",
            JSON.stringify(cueItem.data.resultObject.data[0])
          );
          window.open("/app/cuesheet/day/detail", "_blank");
          // this.$router.push({ path: "/app/cuesheet/day/detail" });
        }
      }
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
