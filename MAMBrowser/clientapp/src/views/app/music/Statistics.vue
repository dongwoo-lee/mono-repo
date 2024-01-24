<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="전체 선곡 순위" />
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
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            style="width: 100px"
            v-model="searchItems.media"
            :options="mediaOptions"
            @change="onSearch"
          />
        </b-form-group>
        <!-- 날짜 형식 -->
        <b-form-radio-group
          v-model="searchItems.period"
          :options="periodOptions"
          name="radio-options"
          @input="onSelectRadio"
        />
        <!-- 년도 -->
        <b-form-group label="년도" class="has-float-label">
          <b-form-select
            style="width: 100px"
            :options="endDateOptionGroup.yearOptions"
            v-model="endDateOptionGroup.year"
            @change="onYearChange($event)"
          />
        </b-form-group>
        <!-- 월 -->
        <b-form-group
          label="월"
          class="has-float-label"
          v-if="searchItems.period === 'MONTH' || searchItems.period === 'WEEK'"
        >
          <b-form-select
            style="width: 60px"
            :options="endDateOptionGroup.monthOptions"
            v-model="endDateOptionGroup.month"
            @change="onMonthChange($event)"
          />
        </b-form-group>
        <!-- 주 -->
        <b-form-group
          label="주"
          class="has-float-label"
          v-if="searchItems.period === 'WEEK'"
        >
          <b-form-select
            style="width: 250px"
            :options="endDateOptionGroup.weekOptions"
            v-model="endDateOptionGroup.week"
            @change="onWeekChange($event)"
          />
        </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="btn-outline-primary default" @click="onSearch"
            >검색</b-button
          >
        </b-form-group>
      </template>
      <template slot="form-btn-area">
        <span>
          <span style="color: #008ecc">●</span>
          <span>{{ setSearchDateStr() }}</span>
        </span>
      </template>
      <!-- 테이블 페이지 -->
      <template slot="form-table-page-area">
        {{ getTotalRowCount() }}
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <common-data-table-scroll-paging
          ref="scrollPaging"
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          :totalCount="responseData.totalRowCount"
          is-actions-slot
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @selectedIds="onSelectedIds"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
          <template slot="rankHoverMusicId" scope="props">
            <div>
              <span
                v-b-tooltip.hover
                :title="'MUSICID : ' + props.props.rowData.musicid"
              >
                {{ props.props.rowData.rank }}</span
              >
            </div>
          </template>
          <template slot="songnameHoverAlbum" scope="props">
            <div>
              <span
                v-b-tooltip.hover
                :title="'앨범명 : ' + props.props.rowData.albumname"
              >
                {{ props.props.rowData.songname }}</span
              >
            </div>
          </template>
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :behaviorData="behaviorList"
              :etcData="['down-excel']"
              @preview="onPreview"
              @download="
                onDownloadProduct(
                  props.props.rowData,
                  downloadName(props.props.rowData)
                )
              "
              @mydiskCopy="onCopyToMySpacePopup"
              @downExcel="onDownExcel"
            >
            </common-actions>
          </template>
        </common-data-table-scroll-paging>
      </template>
      <!-- 알림 -->
      <template slot="form-confirm-area">
        <CopyToMySpacePopup
          ref="refCopyToMySpacePopup"
          :show="copyToMySpacePopup"
          :MySpaceScreenName="MySpaceScreenName"
          @ok="onMyDiskCopyFromMusicRanking"
          @close="copyToMySpacePopup = false"
        >
        </CopyToMySpacePopup>
      </template>
    </common-form>
    <PlayerPopup
      :showPlayerPopup="showPlayerPopup"
      :title="soundItem.songname"
      :fileKey="soundItem.fileToken"
      :streamingUrl="streamingUrl"
      :waveformUrl="waveformUrl"
      :tempDownloadUrl="tempDownloadUrl"
      requestType="token"
      direct="Y"
      @closePlayer="onClosePlayer"
    >
    </PlayerPopup>
  </div>
</template>

<script>
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CopyToMySpacePopup from "../../../components/Popup/CopyToMySpacePopup";
import { Workbook } from "exceljs";
import "moment/locale/ko";
const moment = require("moment");

export default {
  mixins: [MixinBasicPage],
  data() {
    return {
      minEndDate: "20200812",
      rowData: "",
      mediaOptions: [],
      searchItems: {
        media: "",
        period: "WEEK",
        personid: "",
        enddate: "",
        rowPerPage: 50,
        selectPage: 1,
        sortKey: "",
        sortValue: "",
      },
      periodOptions: [
        {
          text: "연간",
          value: "YEAR",
        },
        {
          text: "월간",
          value: "MONTH",
        },
        {
          text: "주간",
          value: "WEEK",
        },
      ],
      endDateOptionGroup: {
        yearOptions: [],
        year: "",
        monthOptions: [],
        month: "",
        weekOptions: [],
        week: "",
      },
      isTableLoading: false,
      fields: [
        {
          name: "__slot:rankHoverMusicId",
          // name: "rank",
          title: "순위",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "4%",
        },
        {
          name: "__slot:songnameHoverAlbum",
          title: "제목",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center memo-ellipsis",
        },
        {
          name: "artist",
          title: "가수",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center memo-ellipsis",
        },
        {
          name: "playcnt",
          title: "재생 횟수",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "5%",
        },
        {
          name: "summarydate",
          title: "등록일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          sortField: "regDtm",
          width: "12%",
          callback: (value) => {
            return value === null
              ? ""
              : moment(value, "YYYYMMDD").format("YYYY-MM-DD");
          },
        },
        {
          name: "__slot:actions",
          title: "추가작업",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
        },
      ],
      playlist_fields: [
        { rowno: "순서" },
        { seq: "SEQ" },
        { schdate: "등록일" },
        { audioclipid: "오디오 ID" },
        { musicid: "음악 ID" },
        { encodedate: "" },
        { audiofiletype: "순서" },
        { productid: "프로그램ID" },
        { productname: "프로그램 명" },
        { pgmcode: "그룹 코드" },
        { pgmname: "그룹 이름" },
        { startdtm: "송출 시작 일시" },
        { enddtm: "송출 종료 일시" },
        { studioname: "스튜디오" },
        { slapname: "단말" },
        { brdctype: "방송구분" },
        { songname: "곡명" },
        { artist: "가수" },
        { albumname: "앨범명" },
        { productyear: "발매일" },
        { playtime: "송출 길이" },
        { totaltime: "음원 길이" },
        { userid: "유저 ID" },
        { username: "유저 이름" },
        { regdtm: "등록 일시" },
      ],
      MySpaceScreenName: "[전체 선곡 순위]",
    };
  },
  components: { CopyToMySpacePopup },
  async created() {
    await this.getMediaOptions(this.pgmList);
    await this.onSelectRadio();
    await this.getData();
  },
  methods: {
    async getData() {
      this.searchItems.rowPerPage = Number(this.searchItems.rowPerPage);
      this.isTableLoading = this.isScrollLodaing ? false : true;
      const params = {
        enddate: this.searchItems.enddate,
        period: this.searchItems.period,
        media: this.searchItems.media,
        personid: this.searchItems.personid,
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
    downloadName(rowData) {
      var tmpName = `${rowData.songname}_${rowData.albumname}_${rowData.musicid}`;
      return tmpName;
    },
    getReturnList(params) {
      const url = "/api/PlaylistPerBrdProgram/GetPlaylistStatistics";
      return this.$http.post(url, params).then((res) => {
        if (res.status === 200 && res.data.resultObject) {
          return res;
        }
      });
    },
    getMediaOptions() {
      const url = "/api/Categories/media";
      this.$http.get(url).then((res) => {
        if (res.status === 200) {
          const options = [{ value: "", text: "전체" }];
          res.data.resultObject.data.forEach((op) => {
            const item = { text: op.name, value: op.id };
            options.push(item);
          });
          this.mediaOptions = options;
        }
      });
    },
    async onSelectRadio() {
      switch (this.searchItems.period) {
        case "YEAR":
          await this.setYearOptions(true);
          this.searchItems.enddate = this.endDateOptionGroup.year;
          this.searchItems.rowPerPage = 100;
          await this.onSearch();
          break;

        case "MONTH":
          await this.setYearOptions();
          await this.setMonthOptions(this.endDateOptionGroup.year);
          this.searchItems.enddate = this.endDateOptionGroup.month;
          this.searchItems.rowPerPage = 100;
          await this.onSearch();
          break;

        case "WEEK":
          await this.setYearOptions();
          await this.setMonthOptions(this.endDateOptionGroup.year);
          await this.setWeekOptions(this.endDateOptionGroup.month);
          this.searchItems.enddate = this.endDateOptionGroup.week;
          this.searchItems.rowPerPage = 50;
          await this.onSearch();
          break;

        default:
          break;
      }
    },
    formatDate(date) {
      const year = date.getFullYear();
      const month = String(date.getMonth() + 1).padStart(2, "0");
      const day = String(date.getDate()).padStart(2, "0");
      return `${year}${month}${day}`;
    },
    getLastDayOfMonth(year, month) {
      return new Date(year, month + 1, 0).getDate();
    },
    async onYearChange(e) {
      switch (this.searchItems.period) {
        case "YEAR":
          this.searchItems.enddate = this.endDateOptionGroup.year;
          await this.onSearch();
          break;
        case "MONTH":
          this.setMonthOptions(e);
          this.searchItems.enddate = this.endDateOptionGroup.month;
          await this.onSearch();
          break;
        case "WEEK":
          await this.setMonthOptions(e);
          await this.setWeekOptions(this.endDateOptionGroup.month);
          this.searchItems.enddate = this.endDateOptionGroup.week;
          await this.onSearch();
          break;

        default:
          break;
      }
    },
    async onMonthChange(e) {
      switch (this.searchItems.period) {
        case "YEAR":
          this.searchItems.enddate = this.endDateOptionGroup.year;
          await this.onSearch();
          break;
        case "MONTH":
          this.searchItems.enddate = this.endDateOptionGroup.month;
          await this.onSearch();
          break;
        case "WEEK":
          await this.setWeekOptions(e);
          this.searchItems.enddate = this.endDateOptionGroup.week;
          await this.onSearch();
          break;

        default:
          break;
      }
    },
    async onWeekChange(e) {
      if (this.searchItems.period === "WEEK") {
        this.searchItems.enddate = e;
        await this.onSearch();
      }
    },
    setYearOptions(isSub = false) {
      const startDateStr = this.minEndDate;
      const startDate = new Date(
        `${startDateStr.slice(0, 4)}-${startDateStr.slice(
          4,
          6
        )}-${startDateStr.slice(6)}`
      );
      const currentYear = new Date().getFullYear();
      let yearList = [];

      for (let year = currentYear; year >= startDate.getFullYear(); year--) {
        const lastDayOfYear = new Date(year, 11, 31);
        yearList.push({
          text: year.toString(),
          value: this.formatDate(lastDayOfYear),
        });
      }
      if (yearList) {
        if (isSub) {
          yearList = yearList.filter(
            (year) => year.text !== currentYear.toString()
          );
          this.endDateOptionGroup.yearOptions = yearList;
          this.endDateOptionGroup.year = yearList[0].value;
        } else {
          this.endDateOptionGroup.yearOptions = yearList;
          this.endDateOptionGroup.year = yearList[0].value;
        }
      }
    },
    setMonthOptions(yearString) {
      const toDay = moment();
      const year = toDay.year();
      const month = toDay.month() + 1;
      let firstDayOfMonth = moment(`${year}-${month}-01`, "YYYY-MM-DD");

      if (firstDayOfMonth.day() !== 1) {
        firstDayOfMonth.add(1, "week").startOf("isoWeek");
      }
      const monDay = firstDayOfMonth.clone();
      const endDay = monDay.clone().add(1, "week").startOf("week");

      let lastDateValCalIndex = -1;
      if (this.searchItems.period === "WEEK" && toDay.isAfter(endDay)) {
        lastDateValCalIndex = 0;
      }
      const yearRegex = /^(\d{4})(\d{2})(\d{2})$/;
      const match = yearString.match(yearRegex);
      const yearNum = Number(match[1]);
      const currentDate = new Date();
      const currentYear = currentDate.getFullYear();
      const currentMonth = currentDate.getMonth() + 1; // getMonth()는 0부터 시작하므로 1을 더해줌

      const startDateStr = this.minEndDate;
      const startYear = Number(startDateStr.slice(0, 4));
      const startMonth = Number(startDateStr.slice(4, 6));

      const monthList = [];
      let start = 1;
      let end = 12;

      if (yearNum === startYear) {
        start = startMonth;
        end = 12;
      } else if (yearNum === currentYear) {
        start = 1;
        end = currentMonth + lastDateValCalIndex;
      }
      for (let month = end; month >= start; month--) {
        const lastDay = this.getLastDayOfMonth(yearNum, month - 1);
        const value = `${yearNum}${String(month).padStart(2, "0")}${String(
          lastDay
        ).padStart(2, "0")}`;
        monthList.push({
          text: month.toString(),
          value: value,
        });
      }
      if (monthList.length > 0) {
        this.endDateOptionGroup.monthOptions = monthList;
        this.endDateOptionGroup.month = monthList[0].value;
      } else {
        const options = this.endDateOptionGroup.yearOptions;
        options.shift();
        this.endDateOptionGroup.year = options[0].value;
        this.setMonthOptions(options[0].value);
      }
    },
    setWeekOptions(yearString) {
      const toDay = moment();
      const year = Number(yearString.slice(0, 4));
      const month = Number(yearString.slice(4, 6));
      let firstDayOfMonth = moment(`${year}-${month}-01`, "YYYY-MM-DD");

      if (firstDayOfMonth.day() !== 1) {
        firstDayOfMonth.add(1, "week").startOf("isoWeek");
      }
      const monDay = firstDayOfMonth.clone();
      const endDay = monDay.clone().add(1, "week").startOf("week");

      const weekData = [];
      while (monDay.month() + 1 === month && toDay.isAfter(endDay)) {
        const weekText = `${monDay.format("MM월 DD일(ddd)")} ~ ${endDay.format(
          "MM월 DD일(ddd)"
        )}`;
        const value = endDay.format("YYYYMMDD");
        weekData.unshift({
          text: weekText,
          value: value,
        });

        monDay.add(1, "week");
        endDay.add(1, "week");
      }
      this.endDateOptionGroup.weekOptions = weekData;
      this.endDateOptionGroup.week = weekData[0].value;
    },
    getPlaylist(params) {
      const url = "/api/PlaylistPerBrdProgram/GetPlaylistProgram";
      return this.$http.post(url, params).then((res) => {
        if (res.status === 200 && res.data.resultObject) {
          return res;
        }
      });
    },
    getExcelFileName(rowData) {
      let name = "[전체 선곡 순위_";
      if (this.searchItems.media) {
        name = name + this.searchItems.media + "_";
      }
      if (this.searchItems.period === "WEEK") {
        let e_dateStr = moment(this.searchItems.enddate, "YYYYMMDD").format(
          "YYYYMMDD"
        );
        let s_dateStr = moment(this.searchItems.enddate, "YYYYMMDD")
          .subtract(6, "day")
          .format("YYYYMMDD");
        name = name + "주간]" + s_dateStr + "_" + e_dateStr;
      }
      if (this.searchItems.period === "MONTH") {
        let dateStr = moment(this.searchItems.enddate, "YYYYMMDD").format(
          " YYYYMM"
        );
        name = name + "월간]" + dateStr;
      }
      if (this.searchItems.period === "YEAR") {
        let dateStr = moment(this.searchItems.enddate, "YYYYMMDD").format(
          " YYYY"
        );
        name = name + "연간]" + dateStr;
      }
      if (rowData.artist) {
        name = name + "_" + rowData.artist;
      }
      if (rowData.songname) {
        name = name + "_" + rowData.songname;
      }
      return name;
    },
    async onDownExcel(rowData) {
      const params = {
        period: this.searchItems.period,
        userid: this.searchItems.personid,
        audioclipid: rowData.audioclipid,
        enddate: this.searchItems.enddate,
      };
      const playlist = await this.getPlaylist(params);
      if (
        playlist.data.resultObject.data &&
        playlist.data.resultObject.data.length > 1
      ) {
        const dataGrid = playlist.data.resultObject.data;
        const workbook = new Workbook();
        const worksheet = workbook.addWorksheet("vuetable");

        const colKeys = Object.keys(dataGrid[0]);

        worksheet.columns = colKeys.map((name) => {
          const fieldObject = this.playlist_fields.find((field) =>
            field.hasOwnProperty(name)
          );
          const displayName = fieldObject ? fieldObject[name] : "";
          return { header: displayName, key: fieldObject };
        });

        dataGrid.forEach((data) => {
          const values = Object.values(data);
          worksheet.addRow(values);
        });

        const buffer = await workbook.xlsx.writeBuffer();
        const blob = new Blob([buffer], {
          type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement("a");
        const fileName = this.getExcelFileName(rowData);
        link.href = url;
        link.setAttribute("download", fileName + ".xlsx");
        document.body.appendChild(link);
        link.click();

        window.URL.revokeObjectURL(url);
      } else {
        this.$fn.notify("error", {
          title: "내려받을 데이터가 없습니다.",
        });
      }
    },
    setSearchDateStr() {
      let e_date = moment(this.searchItems.enddate, "YYYYMMDD");
      let s_date = "";
      switch (this.searchItems.period) {
        case "WEEK":
          s_date = e_date.clone().subtract(7, "days").add(1, "days");
          break;
        case "MONTH":
          s_date = e_date
            .clone()
            .subtract(e_date.daysInMonth(), "days")
            .add(1, "days");
          break;
        case "YEAR":
          s_date = e_date.clone().subtract(1, "year").add(1, "days");
          break;

        default:
          break;
      }
      return (
        s_date.format("YYYY년 MM월 DD일") +
        " ~ " +
        e_date.format("YYYY년 MM월 DD일")
      );
    },
  },
};
</script>

<style></style>
