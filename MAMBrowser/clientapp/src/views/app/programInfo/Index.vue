<template>
  <div id="program_info">
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="프로그램 정보" />
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
        <!-- 조회 버튼 -->
        <!-- <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >조회</b-button
          >
        </b-form-group> -->
      </template>
      <template slot="form-table-page-area">
        <div>
          <div>
            <div class="subtitle mt-0 mb-3">
              <span class="sub_text">
                <span class="subtitle_css">●</span>
                대표이미지 및 상세정보
              </span>
            </div>
            <div
              class="text-center text-primary my-2"
              style="height: 180px"
              v-if="isDetailLoading"
            >
              <b-spinner class="align-middle"></b-spinner>
              <strong>Loading...</strong>
            </div>
            <div
              v-if="!isDetailLoading && !dataSource.pgmcode"
              class="no_image_and_detail"
            >
              데이터가 없습니다.
            </div>
            <div v-if="!isDetailLoading && dataSource.pgmcode">
              <div class="image_and_detail">
                <div class="image_">
                  <img
                    :src="
                      dataSource.imagepath
                        ? dataSource.imagepath
                        : '/assets/img/login-backgound1.jpg'
                    "
                    onerror="this.src='/assets/img/login-backgound1.jpg'"
                    style="width: 100%"
                  />
                </div>
                <div class="detail_">
                  <div class="dx-field" v-if="dataSource.staffs">
                    <div class="dx-field-label">제작진</div>
                    <div class="dx-field-value">{{ dataSource.staffs }}</div>
                  </div>
                  <div class="dx-field" v-if="dataSource.keyword">
                    <div class="dx-field-label">키워드</div>
                    <div class="dx-field-value">{{ dataSource.keyword }}</div>
                  </div>
                  <div class="dx-field" v-if="dataSource.description">
                    <div class="dx-field-label">기본설명</div>
                    <div class="dx-field-value">
                      {{ dataSource.description }}
                    </div>
                  </div>
                  <div class="dx-field" v-if="dataSource.pgmcode">
                    <div class="dx-field-label">코드</div>
                    <div class="dx-field-value">{{ dataSource.pgmcode }}</div>
                  </div>
                  <div class="dx-field" v-if="dataSource.imagepath">
                    <div class="dx-field-label">대표이미지</div>
                    <div class="dx-field-value">{{ dataSource.imagepath }}</div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="separator mt-4 mb-4"></div>
          <div class="subtitle mt-3 mb-3">
            <span class="sub_text">
              <span class="subtitle_css">●</span>
              세부 프로그램 정보
            </span>
          </div>
        </div>
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <common-data-table-scroll-paging
          ref="scrollPaging"
          tableHeight="240px"
          :fields="fields"
          :rows="dataSource.productIdDetails"
          is-actions-slot
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :cueParam="searchItems"
              :isProgramInfoConfigAction="true"
            >
            </common-actions>
          </template>
        </common-data-table-scroll-paging>
      </template>
    </common-form>
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import "moment/locale/ko";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
const moment = require("moment");

export default {
  mixins: [MixinBasicPage],
  data() {
    return {
      date: new Date(),
      isDetailLoading: false,
      dataSource: {
        staffs: "",
        keyword: "",
        description: "",
        pgmcode: "",
        imagepath: "",
        productIdDetails: [],
      },
      toDay: "",
      searchItems: {
        brdDate: null, // 방송일
        media: "A", // 매체
        pgmcode: "",
      },
      pgmList: [],
      mediaOptions: [],
      programOptions: [],
      fields: [
        {
          name: "productid",
          title: "프로그램 ID",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "20%",
        },
        {
          name: "eventname",
          title: "프로그램 명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "25%",
        },
        {
          name: "onairtime",
          title: "방송시작시간",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%",
          callback: (value) => {
            return value === null || value === "0001-01-01T00:00:00"
              ? ""
              : moment(value, "YYYY-MM-DD'T'HH:mm:ss").format("HH:mm");
          },
        },
        {
          name: "tdname",
          title: "기술감독",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
        },
        {
          name: "mainmachine",
          title: "송출라인",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
          callback: (value) => {
            return value && value.includes("DOS") ? value + "[녹음]" : value;
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
  async mounted() {
    const queryParams = this.$route.query;
    const isQuery =
      queryParams.brdDate && queryParams.pgmcode && queryParams.media;

    this.toDay = await this.GetDateString(this.date);
    const param = {
      person: null,
      brd_dt: this.toDay,
      media: null,
      pgmcode: null,
    };
    this.pgmList = await this.GetPgmListByBrdDate(param);
    const selectMediaObj = this.pgmList.filter(
      (pgm) => pgm.media === this.searchItems.media
    );
    [this.mediaOptions, this.programOptions] = await Promise.all([
      this.SetMediaOption(this.pgmList),
      this.SetProgramCodeOption(selectMediaObj),
    ]);
    this.mediaOptions = this.mediaOptions.filter((ele) => ele.value !== "");
    this.programOptions = this.programOptions.filter((ele) => ele.value !== "");

    if (isQuery) {
      this.searchItems.brdDate = queryParams.brdDate;
      this.searchItems.media = queryParams.media;
      this.searchItems.pgmcode = queryParams.pgmcode;
    } else {
      await this.setNowPgmcode();
    }
    await this.getData();
  },
  methods: {
    ...mapActions("cueList", ["GetDateString"]),
    ...mapActions("cueList", ["SetMediaOption"]),
    ...mapActions("cueList", ["SetProgramCodeOption"]),
    ...mapActions("cueList", ["GetPgmListByBrdDate"]),
    async getData() {
      this.isTableLoading = this.isScrollLodaing ? false : true;
      this.isDetailLoading = true;
      const params = {
        media: this.searchItems.media,
        brddate: this.searchItems.brdDate,
        pgmcode: this.searchItems.pgmcode,
      };
      const dataList = await this.getReturnList(params);
      if (dataList) {
        this.dataSource = dataList.data.resultObject;
      }
      this.addScrollClass();
      this.isTableLoading = false;
      this.isScrollLodaing = false;
      this.isDetailLoading = false;
    },
    getReturnList(params) {
      const url = "/api/programInfomation/getprogramInfo";
      return this.$http.post(url, params).then((res) => {
        if (res.status === 200 && res.data.resultObject) {
          return res;
        }
      });
    },
    async onMediaChange(e) {
      const selectMediaObj = this.pgmList.filter((pgm) => pgm.media === e);
      this.programOptions = await this.SetProgramCodeOption(selectMediaObj);
      this.programOptions = this.programOptions.filter(
        (ele) => ele.value !== ""
      );
      this.searchItems.pgmcode = "";
      // this.onSearch();
    },
    async onPgmChange(e) {
      this.searchItems.pgmcode = e;
      await this.getData();
    },
    async changeDate(date) {
      this.searchItems.brdDate = date;
      await this.getData();
    },
    async setNowPgmcode() {
      const params = {
        // brdDate: this.toDay,
        brdDate: "20221001",
        media: this.searchItems.media,
        producTtype: "P",
        rowPerPage: 30,
        selectPage: 1,
      };
      const transList = await this.getToDayTransMissionList(params);
      if (transList) {
        const currentTime = new Date(2022, 9, 1, 11, 6, 0);
        const rowItems = transList.data.resultObject.data.filter((row) => {
          return new Date(row.onairtime) < currentTime;
        });
        if (rowItems.length > 0) {
          const nowPgmItem = rowItems[rowItems.length - 1];
          const codes = this.pgmList.filter(
            (pgm) => pgm.media === this.searchItems.media
          );
          for (const ele of transList.data.resultObject.data) {
            if (
              ele.rowno >= nowPgmItem.rowno &&
              this.searchItems.pgmcode === ""
            ) {
              for (const item of codes) {
                if (this.searchItems.pgmcode === "") {
                  for (const id of item.pgmItem) {
                    if (id.productid === ele.productid) {
                      this.searchItems.pgmcode = item.pgmcode;
                      return;
                    }
                  }
                }
              }
            }
          }
        }
      }
    },
    getToDayTransMissionList(params) {
      const url = "/api/TransMissionList/GetTransMissionList";
      return this.$http.post(url, params).then((res) => {
        if (res.status === 200 && res.data.resultObject) {
          return res;
        }
      });
    },
  },
};
</script>
<style>
#program_info .page-info-group {
  display: inline;
  text-align: left;
}
#program_info .col-auto {
  /* display: none; */
  width: 100%;
}
.subtitle_css {
  color: #008ecc;
}
.sub_text {
  margin-right: 15px;
}
.image_and_detail {
  display: flex;
  align-items: stretch;
  height: 180px;
}
.image_and_detail .image_ {
  flex: 2;
}
.image_and_detail .detail_ {
  flex: 8;
}
.image_and_detail .detail_ .dx-field {
  margin: 0;
}
.image_and_detail .detail_ .dx-field-label {
  font-family: "MBC 새로움 M" !important;
  text-align: start;
  width: 140px;
  font-size: small;
  color: #808080;
  padding: 10px 20px 0px 40px;
}
.image_and_detail .detail_ .dx-field-value {
  font-family: "MBC 새로움 M" !important;
  float: none;
  text-align: start;
  padding: 10px 20px 0px 40px;
}
.no_image_and_detail {
  height: 180px;
  line-height: 150px;
  text-align: center;
}
</style>
