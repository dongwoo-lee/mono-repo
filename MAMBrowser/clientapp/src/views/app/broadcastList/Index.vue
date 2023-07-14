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
        <!-- 검색 버튼
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >검색</b-button
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
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :isBroadcastConfigAction="true"
              :broadcastSearchParam="searchItems"
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
        brdDate: "20221001",
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
          name: "mainmachine",
          title: "메인소스",
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
          width: "6%",
          callback: (value) => {
            return value === null
              ? ""
              : moment.unix(value).subtract(9, "hours").format("HH:mm:SS");
          },
        },
        {
          name: "eventname",
          title: "프로그램이름",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
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
  mounted() {
    this.getMediaOptions();
    this.getData();
  },
  methods: {
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
      // const currentTime = new Date();
      //테스트
      const currentTime = new Date(2022, 9, 1, 11, 0, 3);
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
          return row.producttype == "P";
        });
        rowItems.forEach((ele) => {
          const targetRow = rows[ele.rowno - 1];
          targetRow.classList.add("pgm_tr");
        });
      }
    },
    onMusicSelectionList() {
      // let url = "/api/managementsystem/getGroup";
      // this.$http.post(url).then((res) => {
      //   if (res.status === 200) {
      //     console.log(res.data.resultObject);
      //     this.$bvModal.show(this.modalId);
      //   } else {
      //     this.$fn.notify("server-error", { message: "조회 에러" });
      //   }
      // });
      this.$bvModal.show(this.modalId);
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
