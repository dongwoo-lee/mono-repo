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
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >조회</b-button
          >
        </b-form-group>
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
                <div class="image_box">
                  <div class="image_">
                    <img v-if="imageUrl" :src="imageUrl" alt="Image" />
                    <div class="noImg">No Image</div>
                  </div>
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
                  <div class="dx-field">
                    <div class="dx-field-label mt-2">대표이미지</div>
                    <div class="dx-field-value">
                      <b-button
                        variant="outline-secondary"
                        size="sm"
                        @click="openModal()"
                        >파일 편집</b-button
                      >
                    </div>
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
              :isProgramInfoConfigAction="true"
              @goCueSheetDate="onGoCueSheetDate"
            >
            </common-actions>
          </template>
        </common-data-table-scroll-paging>
      </template>
    </common-form>
    <FileUploadModal
      :modalId="pgmModalId"
      modalTitle="대표이미지 파일 편집"
      :is-save-loading="isSaveLoading"
      @uploadOk="onUploadOk"
    />
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import "moment/locale/ko";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import FileUploadModal from "../config/widget/popup_file_upload.vue";
const moment = require("moment");

export default {
  mixins: [MixinBasicPage],
  data() {
    return {
      date: new Date(),
      pgmModalId: "pgmInfoFileUploadModal",
      isDetailLoading: false,
      imageUrl: "",
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
      isSaveLoading: false,
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
    this.isTableLoading = this.isScrollLodaing ? false : true;
    this.isDetailLoading = true;
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
  components: { FileUploadModal },
  methods: {
    ...mapActions("cueList", ["GetDateString"]),
    ...mapActions("cueList", ["SetMediaOption"]),
    ...mapActions("cueList", ["SetProgramCodeOption"]),
    ...mapActions("cueList", ["GetPgmListByBrdDate"]),
    ...mapActions("cueList", ["getcuesheetListArr"]),
    ...mapActions("cueList", ["getarchiveCuesheetListArr"]),
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
      this.getPgmImg();
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
    getPgmImg() {
      this.imageUrl = "";
      const url = "/api/programInfomation/GetPgmImgFile";
      if (this.dataSource.imagepath) {
        this.$http
          .get(url + "?imgPath=" + this.dataSource.imagepath)
          .then((res) => {
            if (res.status === 200) {
              this.imageUrl = "data:image/png;base64," + res.data.resultObject;
            }
          });
      }
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
    openModal() {
      this.$bvModal.show(this.pgmModalId);
    },
    async setNowPgmcode() {
      const params = {
        // 최신데이터 없어서 TEST
        brdDate: this.toDay,
        // brdDate: "20221001",
        media: this.searchItems.media,
        producTtype: "P",
        rowPerPage: 30,
        selectPage: 1,
      };
      const transList = await this.getToDayTransMissionList(params);
      if (transList) {
        // 최신데이터 없어서 TEST
        const currentTime = new Date();
        // const currentTime = new Date(2022, 9, 1, 11, 6, 0);
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
    async onUploadOk(file) {
      this.isSaveLoading = true;
      const url = "/api/programInfomation/UpdatePgmImgFile";
      const formData = new FormData();
      formData.append("file", file);
      formData.append("pgmcode", this.dataSource.pgmcode);
      await this.$http.post(url, formData).then((res) => {
        if (res.status === 200 && res.data.resultObject) {
          //버튼 로딩 이미지 하고
        }
      });
      this.imageUrl = "";
      this.isSaveLoading = false;
      this.$bvModal.hide(this.pgmModalId);
      this.getData();
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
        sessionStorage.setItem(
          "USER_INFO",
          JSON.stringify(archiveItem.data.resultObject.data[0])
        );
        window.open("/app/cuesheet/previous/detail", "_blank");
      } else {
        //일일큐시트 목록에서 확인 후 Go Page
        const cueItem = await this.getcuesheetListArr(param);
        if (cueItem.data.resultObject.data.length == 1) {
          sessionStorage.setItem(
            "USER_INFO",
            JSON.stringify(cueItem.data.resultObject.data[0])
          );
          window.open("/app/cuesheet/day/detail", "_blank");
        }
      }
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
.image_and_detail .image_box {
  flex-shrink: 0;
  width: 350px;
}
.image_and_detail .image_ {
  position: relative;
  width: 330px;
  height: 185px;
}
.image_and_detail img {
  position: absolute;
  z-index: 1;
  width: 100%;
  height: 100%;
}
.image_and_detail .image_ .noImg {
  position: absolute;
  width: 100%;
  height: 100%;
  text-align: center;
  line-height: 180px;
  background-color: rgba(183, 183, 183, 0.1);
  border-width: 2px;
  border-color: darkgray;
  border-style: dashed;
  color: darkgray;
  box-sizing: border-box;
}
.image_and_detail .detail_ {
  flex-grow: 1;
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
