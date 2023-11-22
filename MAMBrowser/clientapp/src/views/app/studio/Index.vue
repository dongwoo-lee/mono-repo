<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="스튜디오 정보" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      :isDisplayBtnArea="true"
      :isDisplayPageSize="false"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 스튜디오 -->
        <b-form-group label="스튜디오" class="has-float-label">
          <b-form-select
            style="width: 100px"
            v-model="searchItems.studioid"
            :options="studioInfoOptions"
            value-field="id"
            text-field="name"
            @change="onSearch"
          />
        </b-form-group>
        <!-- 방송일 -->
        <b-form-group label="조회일" class="has-float-label">
          <common-date-picker
            required
            v-model="searchItems.brdDate"
            @changeDatePicker="changeDate"
          />
        </b-form-group>
        <!-- 조회 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >조회</b-button
          >
        </b-form-group>
      </template>
      <template slot="form-table-area">
        <DxScheduler
          :showAllDayPanel="false"
          :showCurrentTimeIndicator="false"
          :firstDayOfWeek="1"
          :data-source="dataSource"
          :current-date="changeFormatStringDate(searchItems.brdDate)"
          :height="600"
          :editing="false"
          current-view="week"
          startDateExpr="startdate"
          endDateExpr="enddate"
          timeCellTemplate="timeCellTemplate"
          appointment-template="AppointmentTemplateSlot"
          dateCellTemplate="dateCellTemplateSlot"
          :onAppointmentClick="onAppointmentClick"
          :onAppointmentDblClick="onAppointmentDblClick"
        >
          <DxResource :data-source="resourcesData" field-expr="tdid" />
          <template #timeCellTemplate="{ data: dataCell }">
            <div style="font-family: 'MBC 새로움 M'; font-size: small">
              {{ setTimeTemplate(dataCell) }}
            </div>
          </template>
          <template #dateCellTemplateSlot="{ data: dataCell }">
            <div
              style="font-family: 'MBC 새로움 M'"
              :class="{ weekend_font_color: isWeekend(dataCell.date) }"
            >
              {{ dataCell ? subtractWeek(dataCell.date) : "" }}
            </div>
          </template>
          <template #AppointmentTemplateSlot="{ data }">
            <div class="studio_scheduler_app m-1">
              <span>
                <b-badge variant="dark" v-if="data.appointmentData.gubun == '1'"
                  >L</b-badge
                >
                <b-badge
                  variant="light"
                  v-if="data.appointmentData.gubun == '2'"
                  >R</b-badge
                >
              </span>
              <span
                style="font-size: larger"
                v-b-tooltip.hover.bottom
                :title="setAppointmentMainTitle(data)"
              >
                {{ setAppointmentMainTitle(data) }}
              </span>
            </div>
          </template>
        </DxScheduler>
      </template>
    </common-form>
  </div>
</template>

<script>
import "moment/locale/ko";
import { mapActions, mapGetters } from "vuex";
import { DxScheduler, DxResource } from "devextreme-vue/scheduler";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
const moment = require("moment");
// const date = new Date();
const date = new Date("2023-05-29");

function get_date_str(date) {
  let sYear = date.getFullYear();
  let sMonth = date.getMonth() + 1;
  let sDate = date.getDate();

  sMonth = sMonth > 9 ? sMonth : "0" + sMonth;
  sDate = sDate > 9 ? sDate : "0" + sDate;

  return sYear + "" + sMonth + "" + sDate;
}
const toDay = get_date_str(date);

export default {
  mixins: [MixinBasicPage],
  data() {
    return {
      resourcesData: [],
      dataSource: [],
      searchItems: {
        brdDate: toDay,
        studioid: null,
      },
      studioInfoOptions: [],
    };
  },
  async mounted() {
    const queryParams = this.$route.query;
    const isQuery = queryParams.brdDate && queryParams.studioname;
    await this.getStudioInfoMenu();
    if (isQuery) {
      this.searchItems.brdDate = queryParams.brdDate;
      const studioOptionItem = this.studioInfoOptions.find(
        (item) => item.name === queryParams.studioname
      );
      this.searchItems.studioid = studioOptionItem.id;
    } else if (this.studioInfoOptions) {
      this.searchItems.studioid = this.studioInfoOptions[0].id;
    }
    await this.getData();
  },
  components: {
    DxScheduler,
    DxResource,
  },
  methods: {
    setTimeTemplate(cell) {
      if (cell.text) {
        return moment(cell.date).subtract(-5, "hours").format("HH:mm");
      } else {
        return "";
      }
    },
    async getData() {
      const [monday, sunday] = this.getWeekDates(this.searchItems.brdDate);
      const params = {
        as_from: monday,
        as_to: sunday,
        as_stid: this.searchItems.studioid,
      };
      const data = await this.getReturnList(params);
      if (data) {
        this.dataSource = data.data.resultObject.studioAssigns;
        this.resourcesData = data.data.resultObject.schedulerResources;
        const empColor = this.resourcesData.find((ele) => ele.id == "N");
        if (empColor) {
          empColor.color = "#C8C8C8";
        }
        this.resourcesData.push({ id: null, color: "#D1DFEC" });
      }
    },
    changeDate(date) {
      this.searchItems.brdDate = date;
      this.getData();
    },
    getReturnList(params) {
      const url = "/api/studioInfomation/GetSudioAssignList";
      return this.$http
        .get(
          url +
            "?as_from=" +
            params.as_from +
            "&as_to=" +
            params.as_to +
            "&as_stid=" +
            params.as_stid +
            "&as_pgmid="
        )
        .then((res) => {
          if (res.status === 200 && res.data.resultObject) {
            res.data.resultObject.studioAssigns.forEach((ele) => {
              ele.startdate = this.subtractDate(ele.startdate, 5);
              ele.enddate = this.subtractDate(ele.enddate, 5);
            });
            return res;
          }
        });
    },
    subtractDate(dateTimeString, hours) {
      const formattedDateTimeString = moment(dateTimeString)
        .subtract(hours, "hours")
        .format("YYYY-MM-DDTHH:mm:ss");
      return formattedDateTimeString;
    },
    subtractHours(timeString, hours) {
      const formattedTime = moment(timeString, "h:mm A")
        .subtract(hours, "hours")
        .format("h:mm A");
      return formattedTime;
    },
    subtractWeek(dateString) {
      const momentDate = moment(dateString, "ddd MMM D YYYY HH:mm:ss [GMT]ZZ");

      if (!momentDate.isValid()) {
        return ""; // 유효하지 않은 날짜인 경우 빈 문자열 반환 또는 원하는 에러 처리를 수행
      }

      const convertedDateString = momentDate
        .format("ddd D")
        .replace("Fri", "금")
        .replace("Sat", "토")
        .replace("Sun", "일")
        .replace("Mon", "월")
        .replace("Tue", "화")
        .replace("Wed", "수")
        .replace("Thu", "목");
      return convertedDateString;
    },
    isWeekend(dateString) {
      const momentDate = moment(dateString, "ddd MMM D YYYY HH:mm:ss [GMT]ZZ");

      if (!momentDate.isValid()) {
        return false; // 유효하지 않은 날짜인 경우 false 반환 또는 원하는 에러 처리를 수행
      }

      const dayOfWeek = momentDate.day();
      return dayOfWeek === 0 || dayOfWeek === 6; // 0: 일요일(Sunday), 6: 토요일(Saturday)
    },
    async getStudioInfoMenu() {
      const url = "/api/studioInfomation/GetSudioInfoMenu";
      await this.$http.get(url).then((res) => {
        if (res.status === 200 && res.data.resultObject) {
          this.studioInfoOptions = res.data.resultObject.data;
        }
      });
    },
    getWeekDates(dateString) {
      const year = dateString.substring(0, 4);
      const month = dateString.substring(4, 6) - 1; // 월은 0부터 시작하므로 1을 뺌
      const day = dateString.substring(6);
      const date = new Date(year, month, day);
      const firstDayOfWeek = new Date(
        date.getFullYear(),
        date.getMonth(),
        date.getDate() - date.getDay() + 1
      ); // 해당 주의 월요일

      const monday = this.formatDate(firstDayOfWeek); // 월요일 날짜
      const sunday = this.formatDate(
        new Date(
          firstDayOfWeek.getFullYear(),
          firstDayOfWeek.getMonth(),
          firstDayOfWeek.getDate() + 6
        )
      ); // 일요일 날짜

      return [monday, sunday];
    },
    formatDate(date) {
      const year = date.getFullYear();
      const month = String(date.getMonth() + 1).padStart(2, "0");
      const day = String(date.getDate()).padStart(2, "0");
      return year + month + day;
    },
    changeFormatStringDate(date) {
      return (
        date.substring(0, 4) +
        "-" +
        date.substring(4, 6) +
        "-" +
        date.substring(6)
      );
    },
    onAppointmentClick(e) {
      e.cancel = true;
    },
    onAppointmentDblClick(e) {
      e.cancel = true;
    },
    setAppointmentMainTitle(data) {
      let tdName = data.appointmentData.tdname
        ? " (" + data.appointmentData.tdname + ")"
        : "";
      return data.appointmentData.description + tdName;
    },
  },
};
</script>
<style>
.dx-scheduler-header {
  display: none;
}
.dx-scheduler-container {
  border-top: 1px solid rgba(0, 0, 0, 0.125);
}
.studio_scheduler_app {
  color: black;
  font-family: "MBC 새로움 M";
}
.weekend_font_color {
  color: red;
}
</style>
