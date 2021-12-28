<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="부조 SPOT" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      @changeRowPerpage="onChangeRowPerpage"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 시작일 ~ 종료일 -->
        <common-start-end-date-picker
          :startDate.sync="searchItems.start_dt"
          :startDayAgo="7"
          :maxPeriodMonth="3"
          :endDate.sync="searchItems.end_dt"
          :required="true"
          @SEDateEvent="onSearch"
          @SDateError="SDateErrorLog"
        />
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            class="width-120"
            v-model="searchItems.media"
            :options="mediaOptions"
            value-field="id"
            text-field="name"
            @change="onSearch"
          />
        </b-form-group>
        <!-- 사용처명 -->
        <b-form-group label="사용처명" class="has-float-label">
          <common-input-text
            v-model="searchItems.pgmName"
            @inputEnterEvent="onSearch"
          />
        </b-form-group>
        <!-- 제작자 -->
        <b-form-group label="제작자" class="has-float-label">
          <common-vue-select
            :suggestions="editorOptions"
            @inputEvent="onEditorSelected"
          ></common-vue-select>
        </b-form-group>
        <!-- 소재명 -->
        <b-form-group label="소재명" class="has-float-label">
          <common-input-text
            v-model="searchItems.name"
            @inputEnterEvent="onSearch"
          />
        </b-form-group>
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >검색</b-button
          >
        </b-form-group>
      </template>
      <template slot="form-btn-area">
        <b-input-group>
          <b-button
            variant="outline-primary default"
            size="sm"
            @click="show = true"
            >기간 설정</b-button
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
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :downloadName="downloadName(props.props.rowData)"
              :behaviorData="behaviorList"
              :etcData="['delete', 'modify']"
              :isPossibleUpdate="authorityCheck(props.props.rowData)"
              :isPossibleDelete="authorityCheck(props.props.rowData)"
              @preview="onPreview"
              @download="onDownloadProduct"
              @mydiskCopy="onCopyToMySpacePopup"
              @modify="onMetaUpdatePopup"
              @MasteringDelete="onMetaDeletePopup"
            >
            </common-actions>
          </template>
        </common-data-table-scroll-paging>

        <CopyToMySpacePopup
          ref="refCopyToMySpacePopup"
          :show="copyToMySpacePopup"
          :MySpaceScreenName="MySpaceScreenName"
          @ok="onMyDiskCopyFromProduct"
          @close="copyToMySpacePopup = false"
        >
        </CopyToMySpacePopup>
      </template>
    </common-form>

    <!-- 마스터링 메타 데이터 수정 -->
    <transition name="slide-fade">
      <file-update
        v-if="metaUpdate"
        :rowData="rowData"
        :updateScreenName="updateScreenName"
        @updateFile="masteringUpdate"
        @UpdateModalClose="UpdateModalOff"
      ></file-update>
    </transition>

    <!-- 마스터링 파일 삭제 -->
    <transition name="slide-fade">
      <file-delete
        v-if="metaDelete"
        :rowData="rowData"
        :isScrSpot="true"
        @deleteFile="masteringDelete"
        @DeleteModalClose="DeleteModalOff"
      ></file-delete>
    </transition>

    <PlayerPopup
      :showPlayerPopup="showPlayerPopup"
      :title="soundItem.name"
      :fileKey="soundItem.fileToken"
      :streamingUrl="streamingUrl"
      :waveformUrl="waveformUrl"
      :tempDownloadUrl="tempDownloadUrl"
      requestType="token"
      @closePlayer="onClosePlayer"
    >
    </PlayerPopup>

    <b-modal
      id="setScrRange"
      size="lg"
      v-model="show"
      centered
      hide-header-close
      no-close-on-esc
      no-close-on-backdrop
      footer-class="scr-modal-footer"
    >
      <template slot="modal-title">
        <h5>부조SPOT 방송의뢰</h5>
      </template>
      <template slot="default">
        <DxDataGrid
          :data-source="requestScr"
          :show-borders="true"
          style="height: 200px"
          :hover-state-enabled="true"
          key-expr="event"
          :allow-column-resizing="true"
          :column-auto-width="true"
          no-data-text="No Data"
          @row-click="requestSelect"
        >
          <DxSelection mode="single" />
          <DxPager :visible="false" />
          <DxScrolling mode="standard" />
          <DxColumn data-field="spot" caption="SPOT명" />
          <DxColumn data-field="event" caption="사용처" />
          <DxColumn data-field="sDate" caption="시작일" />
          <DxColumn data-field="eDate" caption="종료일" />
        </DxDataGrid>
      </template>
      <template v-slot:modal-footer>
        <b-button
          variant="outline-primary default cutom-label"
          size="sm"
          class="float-left"
          style="margin-right: 5px"
          @click="show2 = true"
        >
          추가</b-button
        ><b-button
          variant="outline-primary default cutom-label-cancel"
          size="sm"
          class="float-left"
          style="margin-right: 480px"
          @click="deleteRequest"
        >
          삭제</b-button
        >
        <b-button
          variant="outline-primary default cutom-label"
          size="sm"
          class="float-right"
          @click="request"
        >
          방송의뢰</b-button
        >
        <b-button
          variant="outline-danger default cutom-label-cancel"
          size="sm"
          class="float-right"
          @click="show = false"
        >
          취소</b-button
        >
        <!-- 여기에다가 편집 저장 버튼 추가해야함 그리고 거기에 Click이벤트로 SOM, EOM 찍히는지 확인하기 -->
      </template>
    </b-modal>
    <b-modal
      id="addRange"
      size="lg"
      v-model="show2"
      centered
      hide-header-close
      no-close-on-esc
      no-close-on-backdrop
      footer-class="scr-modal-footer"
    >
      <template slot="modal-title">
        <h5>기간 설정</h5>
      </template>
      <template slot="default">
        <b-form-group
          label="부조SPOT"
          class="has-float-label"
          style="width: 400px; float: left"
        >
          <b-form-input
            style="
              width: 243px;
              float: left;
              border-top-right-radius: 0;
              border-bottom-right-radius: 0;
              border-right: 0;
              height: 34px;
            "
            class="editTask"
            :value="this.selectedSpot.spotName"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            placeholder="소재명"
            trim
          />

          <b-button
            variant="outline-primary default cutom-label"
            style="margin-top: 10px; float: left; width: 58px; height: 34px"
            @click="show3 = true"
          >
            <b-icon-search></b-icon-search>
          </b-button>
        </b-form-group>
        <b-form-group
          label="사용처"
          class="has-float-label"
          style="margin-top: 10px"
        >
          <common-vue-select
            :class="vSelectClass"
            style="font-size: 14px; width: 300px"
            :suggestions="ProgramOptions"
            @inputEvent="pgmSelect"
          ></common-vue-select>
        </b-form-group>

        <div style="margin-top: 30px">
          <b-form-group
            label="시작일"
            class="has-float-label"
            style="width: 300px; float: left"
          >
            <b-input-group style="width: 300px; float: left">
              <input
                style="height: 34px; font-size: 13px"
                id="sdateinput"
                type="text"
                class="form-control input-picker date-input"
                :value="setScrRangeData.sDate"
                @input="onsInput"
              />
              <b-input-group-append>
                <b-form-datepicker
                  style="height: 34px; float: left"
                  :value="setScrRangeData.sDate"
                  @input="eventSInput"
                  button-variant="outline-dark"
                  button-only
                  right
                  aria-controls="example-input"
                  @context="onContext"
                ></b-form-datepicker>
              </b-input-group-append>
            </b-input-group>
          </b-form-group>
          <b-form-group
            label="종료일"
            class="has-float-label"
            style="
              margin-left: 100px;
              width: 300px;
              font-size: 13px;
              float: left;
            "
          >
            <b-input-group class="mb-3" style="width: 300px; float: left">
              <input
                style="height: 34px; font-size: 13px"
                id="edateinput"
                type="text"
                class="form-control input-picker date-input"
                :value="setScrRangeData.eDate"
                @input="oneInput"
              />
              <b-input-group-append>
                <b-form-datepicker
                  style="height: 34px"
                  :value="setScrRangeData.eDate"
                  @input="eventEInput"
                  button-only
                  button-variant="outline-dark"
                  right
                  aria-controls="example-input"
                  @context="onContext"
                ></b-form-datepicker>
              </b-input-group-append>
            </b-input-group>
          </b-form-group>
        </div>
      </template>
      <template v-slot:modal-footer>
        <b-button
          variant="outline-primary default cutom-label"
          size="sm"
          :disabled="!scrValid"
          class="float-right"
          @click="setScrRange"
        >
          확인</b-button
        >
        <b-button
          variant="outline-danger default cutom-label-cancel"
          size="sm"
          class="float-right"
          @click="show2 = false"
        >
          취소</b-button
        >
      </template>
    </b-modal>
    <b-modal
      id="setSpot"
      v-model="show3"
      size="xl"
      centered
      hide-header-close
      no-close-on-esc
      no-close-on-backdrop
      footer-class="scr-modal-footer"
    >
      <template slot="modal-title"> 부조SPOT 검색 </template>
      <template slot="default">
        <div
          style="
            width: 1085px;
            height: 90px;
            border: 1px solid #d7d7d7;
            padding-top: 20px;
            padding-left: 20px;
          "
        >
          <b-form-group
            label="시작일(등록일자)"
            class="has-float-label"
            style="width: 180px; margin-top: 11px; float: left"
          >
            <b-input-group style="width: 180px; float: left">
              <input
                style="height: 34px; font-size: 13px"
                id="sdateinput"
                type="text"
                class="form-control input-picker date-input"
                :value="searchSpot.sDate"
                @input="onsInput2"
              />
              <b-input-group-append>
                <b-form-datepicker
                  style="height: 34px; float: left"
                  :value="searchSpot.sDate"
                  @input="eventSInput2"
                  button-variant="outline-dark"
                  button-only
                  left
                  aria-controls="example-input"
                  @context="onContext"
                ></b-form-datepicker>
              </b-input-group-append>
            </b-input-group>
          </b-form-group>
          <b-form-group
            label="종료일(등록일자)"
            class="has-float-label"
            style="
              margin-left: 20px;
              width: 180px;
              font-size: 13px;
              float: left;
              margin-top: 11px;
            "
          >
            <b-input-group class="mb-3" style="width: 180px; float: left">
              <input
                style="height: 34px; font-size: 13px"
                id="edateinput"
                type="text"
                class="form-control input-picker date-input"
                :value="searchSpot.eDate"
                @input="oneInput2"
              />
              <b-input-group-append>
                <b-form-datepicker
                  style="height: 34px"
                  :value="searchSpot.eDate"
                  @input="eventEInput2"
                  button-only
                  button-variant="outline-dark"
                  right
                  aria-controls="example-input"
                  @context="onContext"
                ></b-form-datepicker>
              </b-input-group-append>
            </b-input-group>
          </b-form-group>
          <b-form-group
            label="분류"
            class="has-float-label"
            style="margin-left: 20px; margin-top: 11px; float: left"
          >
            <b-form-select
              id="program-media"
              class="media-select"
              style="width: 200px; float: left"
              :value="searchSpot.media"
              :options="scrMediaOptions"
              @input="mediaChange"
            />
          </b-form-group>
          <b-form-group
            label="광고주"
            class="has-float-label"
            style="margin-left: 20px; margin-top: 10px; float: left"
          >
            <b-form-input
              class="memo"
              style="width: 150px; height: 34px; float: left"
              v-model="searchSpot.advertiser"
              aria-describedby="input-live-help input-live-feedback"
              placeholder="광고주 명"
              trim
            />
          </b-form-group>
          <b-form-group
            label="소재"
            class="has-float-label"
            style="margin-left: 20px; margin-top: 10px; float: left"
          >
            <b-form-input
              class="memo"
              style="width: 160px; height: 34px; float: left"
              v-model="searchSpot.title"
              aria-describedby="input-live-help input-live-feedback"
              placeholder="소재 명"
              trim
            />
          </b-form-group>
          <b-button
            style="margin-left: 20px; margin-top: -40px; height: 35px"
            variant="outline-primary default cutom-label"
            @click="getScrSpot"
            >검색</b-button
          >
        </div>

        <DxDataGrid
          :data-source="spotData"
          :show-borders="true"
          style="height: 400px; margin-top: 20px"
          :hover-state-enabled="true"
          key-expr="spotID"
          :allow-column-resizing="true"
          :column-auto-width="true"
          no-data-text="No Data"
          @row-click="spotSelect"
        >
          <DxPager :visible="true" />
          <DxScrolling mode="standard" />
          <DxSelection mode="single" />
          <DxColumn data-field="spotID" caption="spotID" />
          <DxColumn data-field="spotName" caption="spotName" />
          <DxColumn data-field="codeId" caption="codeId" />
          <DxColumn data-field="codeName" caption="codeName" />
          <DxColumn data-field="cmOwner" caption="cmOwner" />
        </DxDataGrid>
      </template>
      <template v-slot:modal-footer>
        <b-button
          variant="outline-primary default cutom-label"
          size="sm"
          class="float-right"
          :disabled="!selectRowValid"
          @click="show3 = false"
        >
          선택</b-button
        >
        <b-button
          variant="outline-danger default cutom-label-cancel"
          size="sm"
          class="float-right"
          @click="show3Cancel"
        >
          취소</b-button
        >
      </template>
    </b-modal>
  </div>
</template>

<script>
import {
  DxDataGrid,
  DxScrolling,
  DxLoadPanel,
  DxColumn,
  DxPager,
  DxSelection,
} from "devextreme-vue/data-grid";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CopyToMySpacePopup from "../../../components/Popup/CopyToMySpacePopup";
import CommonVueSelect from "../../../components/Form/CommonVueSelect.vue";
import FileUpdate from "../../../components/FileUpload/FileUpdate/FileUpdate.vue";
import FileDelete from "../../../components/FileUpload/FileUpdate/FileDelete.vue";
import axios from "axios";
export default {
  components: {
    CopyToMySpacePopup,
    CommonVueSelect,
    FileUpdate,
    FileDelete,
    DxScrolling,
    DxLoadPanel,
    DxPager,
    DxDataGrid,
    DxColumn,
    DxSelection,
  },
  mixins: [MixinBasicPage],
  data() {
    return {
      vSelectClass: "scrSpotVueSelect",
      requestScr: [],
      setScrRangeData: {
        spot: "",
        event: null,
        sDate: this.$fn.formatDate(new Date(), "yyyy-MM-dd"),
        eDate: this.$fn.formatDate(new Date(), "yyyy-MM-dd"),
      },
      ProgramOptions: [],
      // 검색 파라미터
      searchSpot: {
        title: "",
        advertiser: "",
        media: "",
        sDate: "",
        eDate: this.$fn.formatDate(new Date(), "yyyy-MM-dd"),
      },
      //부조SPOT검색 모달 그리드 데이터
      spotData: {
        spotID: "",
        spotName: "",
        codeID: "",
        codeName: "",
        CMOwner: "",
      },
      // 선택한 부조 스팟(row) 데이터
      selectedSpot: {
        spotID: "",
        spotName: "",
        codeID: "",
        codeName: "",
        CMOwner: "",
      },
      scrMediaOptions: [], // 분류 옵션 - created 에서 데이터 push
      tempSDate: "",
      tempEDate: "",
      tempSDate2: "",
      tempEDate2: "",
      deleteId: "",
      show: false,
      show2: false,
      show3: false,
      metaUpdate: false,
      updateScreenName: "",
      rowData: "",
      MySpaceScreenName: "[부조SPOT]",
      metaDelete: false,
      searchItems: {
        start_dt: "", // 시작일
        end_dt: "", // 종료일
        editor: "", // 제작자ID
        editorName: "", // 제작자이름
        name: "", // 소재명
        media: "A", // 매체
        pgmName: "", // 사용처명
        rowPerPage: 30,
        selectPage: 1,
        sortKey: "",
        sortValue: "DESC",
      },
      isTableLoading: false,
      fields: [
        {
          name: "rowNO",
          title: "순서",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "4%",
        },
        {
          name: "name",
          title: "소재명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "name",
        },
        {
          name: "categoryName",
          title: "분류",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "13%",
          sortField: "categoryName",
        },
        {
          name: "duration",
          title: "길이(초)",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%",
          sortField: "duration",
          callback: (v) => {
            return this.$fn.splitFirst(v);
          },
        },
        {
          name: "brdDT",
          title: "방송일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "10%",
          sortField: "brdDT",
          callback: (v) => {
            return this.$fn.dateStringTohaipun(v);
          },
        },
        {
          name: "editorName",
          title: "제작자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "5%",
          sortField: "editorName",
        },
        {
          name: "pgmName",
          title: "사용처명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "pgmName",
        },
        {
          name: "masteringDtm",
          title: "마스터링 일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%",
          sortField: "masteringDtm",
        },
        {
          name: "__slot:actions",
          title: "추가작업",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
        },
      ],
    };
  },
  created() {
    var newDate = new Date();
    var dayOfMonth = newDate.getDate();
    newDate.setDate(dayOfMonth - 14);
    newDate = this.$fn.formatDate(newDate, "yyyy-MM-dd");
    this.searchSpot.sDate = newDate;
    // 매체 목록 조회
    this.getMediaOptions();
    // 사용자 목록 조회
    this.getEditorOptions();
    axios.get("/api/categories/scr/spot").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this.scrMediaOptions.push({ value: e.id, text: e.name });
      });
    });
    this.searchSpot.media = "ST01";
    this.getPgm();
    this.$nextTick(() => {
      this.getData();
    });
  },
  computed: {
    scrValid() {
      if (
        this.setScrRangeData.spot != "" &&
        this.setScrRangeData.event != null &&
        this.setScrRangeData.sDate != "" &&
        this.setScrRangeData.eDate != ""
      ) {
        return true;
      }
      return false;
    },
    selectRowValid() {
      if (this.selectedSpot.spotID != "") {
        return true;
      }
      return false;
    },
  },
  methods: {
    setScrRange() {
      console.log(this.setScrRangeData);
      this.requestScr.push(this.setScrRangeData);
      console.log(this.requestScr);
      this.show2 = false;
    },
    requestSelect(v) {
      console.log(v);
    },
    request() {
      console.log("방송의뢰 버튼");
    },
    deleteRequest() {
      console.log("삭제버튼");
    },
    show3Cancel() {
      this.show3 = false;
      this.selectedSpot = {
        spotID: "",
        spotName: "",
        codeID: "",
        codeName: "",
        CMOwner: "",
      };
    },
    async getPgm() {
      var res = await axios.get(`/api/categories/pgmcodes`);
      this.ProgramOptions = res.data.resultObject.data;
    },
    pgmSelect(v) {
      this.setScrRangeData.event = v.id;
    },
    spotSelect(v) {
      this.selectedSpot = v.data;
      this.setScrRangeData.spot = v.data.spotName;
      console.log(this.selectedSpot);
    },
    resetSpotData() {
      this.spotData = {
        spotID: "",
        spotName: "",
        codeID: "",
        codeName: "",
        CMOwner: "",
      };
    },
    getScrSpot() {
      this.resetSpotData();
      axios
        .get(
          `/api/categories/scr-spot?spotName=${this.searchSpot.title}&codeId=${
            this.searchSpot.media
          }&cmOwner=${this.searchSpot.advertiser}
          &startDate=${this.searchSpot.sDate.replace(
            /-/g,
            ""
          )}&endDate=${this.searchSpot.eDate.replace(/-/g, "")}`
        )
        .then((res) => {
          this.spotData = res.data.resultObject.data;
        });
    },
    SDateErrorLog() {
      this.$fn.notify("error", {
        message: "시작 날짜가 종료 날짜보다 큽니다.",
      });
      this.hasErrorClass = true;
    },
    authorityCheck(e) {
      if (
        e.editorID == sessionStorage.getItem("user_id") ||
        sessionStorage.getItem("authority") == "ADMIN"
      ) {
        return true;
      } else {
        return false;
      }
    },
    getData() {
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

      this.isTableLoading = this.isScrollLodaing ? false : true;
      this.$http
        .get(`/api/products/spot/scr/${this.searchItems.media}`, {
          params: this.searchItems,
        })
        .then((res) => {
          console.log("res");
          console.log(res);
          this.setResponseData(res);
          this.addScrollClass();
          this.isTableLoading = false;
          this.isScrollLodaing = false;
        });
    },
    downloadName(rowData) {
      var tmpName = `${rowData.name}_${rowData.brdDT}`;
      return tmpName;
    },

    onMetaUpdatePopup(rowData) {
      this.metaUpdate = true;
      this.updateScreenName = "scr-spot";
      this.rowData = rowData;
    },

    UpdateModalOff() {
      this.metaUpdate = false;
    },
    masteringUpdate(e) {
      axios.patch("/api/Mastering/scr-spot", e).then((res) => {
        if (res && res.status === 200 && !res.data.errorMsg) {
          this.UpdateModalOff();
          this.$fn.notify("primary", {
            title: "메타 데이터 수정 성공",
          });
          this.getData();
        } else {
          this.UpdateModalOff();
          $fn.notify("error", {
            message: "파일 업로드 실패: " + res.data.errorMsg,
          });
          this.getData();
        }
      });
    },
    DeleteModalOff() {
      this.metaDelete = false;
    },
    onMetaDeletePopup(rowData) {
      this.metaDelete = true;
      this.rowData = rowData;
    },
    masteringDelete(e) {
      axios
        .delete(
          `/api/Mastering/scr-spot?spotID=${e.spotID}&productID=${e.productID}&brdDT=${e.brdDT}&filetoken=${e.fileToken}`
        )
        .then((res) => {
          if (res && res.status === 200 && !res.data.errorMsg) {
            this.DeleteModalOff();
            this.$fn.notify("primary", {
              title: "파일 삭제 성공",
            });
            this.getData();
          } else {
            this.UpdateModalOff();
            $fn.notify("error", {
              message: "파일 삭제 실패: " + res.data.errorMsg,
            });
            this.getData();
          }
        });
    },
    log() {
      console.log("this.setScrRangeData :>> ", this.setScrRangeData);
    },
    mediaChange(v) {
      this.searchSpot.media = v;
    },
    get7daysago() {
      var newDate = new Date();
      var dayOfMonth = newDate.getDate();
      newDate.setDate(dayOfMonth - 7);
      newDate = this.$fn.formatDate(newDate, "yyyy-MM-dd");
      return newDate;
    },
    //#region 1
    eventSInput(value) {
      this.setScrRangeData.sDate = value;
      this.tempSDate = value;

      const replaceAllFileSDate = this.setScrRangeData.sDate.replace(/-/g, "");
      const replaceAllFileEDate = this.setScrRangeData.eDate.replace(/-/g, "");
      if (
        replaceAllFileEDate < replaceAllFileSDate &&
        replaceAllFileEDate != ""
      ) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        return;
      }
    },
    eventEInput(value) {
      this.setScrRangeData.eDate = value;
      this.tempEDate = value;

      const replaceAllFileSDate = this.setScrRangeData.sDate.replace(/-/g, "");
      const replaceAllFileEDate = this.setScrRangeData.eDate.replace(/-/g, "");
      if (replaceAllFileEDate < replaceAllFileSDate) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        return;
      }
    },

    onsInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.tempSDate == null) {
          event.target.value = this.get7daysago();
          return;
        }
        event.target.value = this.tempSDate;
        return;
      }

      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);

          if (
            convertDate == "" ||
            convertDate == null ||
            convertDate == "undefined"
          ) {
            event.target.value = this.get7daysago();
            this.setScrRangeData.sDate = this.get7daysago();
            this.tempSDate = this.get7daysago();

            const replaceAllFileSDate = this.setScrRangeData.sDate.replace(
              /-/g,
              ""
            );
            const replaceAllFileEDate = this.setScrRangeData.eDate.replace(
              /-/g,
              ""
            );
            if (
              replaceAllFileEDate < replaceAllFileSDate &&
              replaceAllFileEDate != ""
            ) {
              this.$fn.notify("error", {
                message: "시작 날짜가 종료 날짜보다 큽니다.",
              });
              return;
            }
            return;
          }
          this.setScrRangeData.sDate = convertDate;
          this.tempSDate = convertDate;
          const replaceAllFileSDate = this.setScrRangeData.sDate.replace(
            /-/g,
            ""
          );
          const replaceAllFileEDate = this.setScrRangeData.eDate.replace(
            /-/g,
            ""
          );
          if (
            replaceAllFileEDate < replaceAllFileSDate &&
            replaceAllFileEDate != ""
          ) {
            this.$fn.notify("error", {
              message: "시작 날짜가 종료 날짜보다 큽니다.",
            });
            return;
          }
        }
      }
    },
    oneInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.tempEDate == null) {
          event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
          return;
        }
        event.target.value = this.tempEDate;
        return;
      }

      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          if (
            convertDate == "" ||
            convertDate == null ||
            convertDate == "undefined"
          ) {
            event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
            this.setScrRangeData.eDate = this.$fn.formatDate(
              new Date(),
              "yyyy-MM-dd"
            );
            this.tempEDate = this.$fn.formatDate(new Date(), "yyyy-MM-dd");

            const replaceAllFileSDate = this.setScrRangeData.sDate.replace(
              /-/g,
              ""
            );
            const replaceAllFileEDate = this.setScrRangeData.eDate.replace(
              /-/g,
              ""
            );
            if (replaceAllFileEDate < replaceAllFileSDate) {
              this.$fn.notify("error", {
                message: "시작 날짜가 종료 날짜보다 큽니다.",
              });
              return;
            }
            return;
          }
          this.setScrRangeData.eDate = convertDate;
          this.tempEDate = convertDate;

          const replaceAllFileSDate = this.setScrRangeData.sDate.replace(
            /-/g,
            ""
          );
          const replaceAllFileEDate = this.setScrRangeData.eDate.replace(
            /-/g,
            ""
          );
          if (replaceAllFileEDate < replaceAllFileSDate) {
            this.$fn.notify("error", {
              message: "시작 날짜가 종료 날짜보다 큽니다.",
            });
            return;
          }
        }
      }
    },
    //#endregion
    //#region 2
    get14daysago() {
      var newDate = new Date();
      var dayOfMonth = newDate.getDate();
      newDate.setDate(dayOfMonth - 14);
      newDate = this.$fn.formatDate(newDate, "yyyy-MM-dd");
      return newDate;
    },
    eventSInput2(value) {
      this.searchSpot.sDate = value;
      this.tempSDate2 = value;

      const replaceAllFileSDate = this.searchSpot.sDate.replace(/-/g, "");
      const replaceAllFileEDate = this.searchSpot.eDate.replace(/-/g, "");
      if (
        replaceAllFileEDate < replaceAllFileSDate &&
        replaceAllFileEDate != ""
      ) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        return;
      }
    },
    eventEInput2(value) {
      this.searchSpot.eDate = value;
      this.tempEDate2 = value;

      const replaceAllFileSDate = this.searchSpot.sDate.replace(/-/g, "");
      const replaceAllFileEDate = this.searchSpot.eDate.replace(/-/g, "");
      if (replaceAllFileEDate < replaceAllFileSDate) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        return;
      }
    },

    onsInput2(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.tempSDate2 == null) {
          event.target.value = this.get14daysago();
          return;
        }
        event.target.value = this.tempSDate2;
        return;
      }

      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);

          if (
            convertDate == "" ||
            convertDate == null ||
            convertDate == "undefined"
          ) {
            event.target.value = this.get14daysago();
            this.searchSpot.sDate = this.get14daysago();
            this.tempSDate2 = this.get14daysago();

            const replaceAllFileSDate = this.searchSpot.sDate.replace(/-/g, "");
            const replaceAllFileEDate = this.searchSpot.eDate.replace(/-/g, "");
            if (
              replaceAllFileEDate < replaceAllFileSDate &&
              replaceAllFileEDate != ""
            ) {
              this.$fn.notify("error", {
                message: "시작 날짜가 종료 날짜보다 큽니다.",
              });
              return;
            }
            return;
          }
          this.searchSpot.sDate = convertDate;
          this.tempSDate2 = convertDate;
          const replaceAllFileSDate = this.searchSpot.sDate.replace(/-/g, "");
          const replaceAllFileEDate = this.searchSpot.eDate.replace(/-/g, "");
          if (
            replaceAllFileEDate < replaceAllFileSDate &&
            replaceAllFileEDate != ""
          ) {
            this.$fn.notify("error", {
              message: "시작 날짜가 종료 날짜보다 큽니다.",
            });
            return;
          }
        }
      }
    },
    oneInput2(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.tempEDate2 == null) {
          event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
          return;
        }
        event.target.value = this.tempEDate2;
        return;
      }

      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          if (
            convertDate == "" ||
            convertDate == null ||
            convertDate == "undefined"
          ) {
            event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
            this.searchSpot.eDate = this.$fn.formatDate(
              new Date(),
              "yyyy-MM-dd"
            );
            this.tempEDate2 = this.$fn.formatDate(new Date(), "yyyy-MM-dd");

            const replaceAllFileSDate = this.searchSpot.sDate.replace(/-/g, "");
            const replaceAllFileEDate = this.searchSpot.eDate.replace(/-/g, "");
            if (replaceAllFileEDate < replaceAllFileSDate) {
              this.$fn.notify("error", {
                message: "시작 날짜가 종료 날짜보다 큽니다.",
              });
              return;
            }
            return;
          }
          this.searchSpot.eDate = convertDate;
          this.tempEDate2 = convertDate;

          const replaceAllFileSDate = this.searchSpot.sDate.replace(/-/g, "");
          const replaceAllFileEDate = this.searchSpot.eDate.replace(/-/g, "");
          if (replaceAllFileEDate < replaceAllFileSDate) {
            this.$fn.notify("error", {
              message: "시작 날짜가 종료 날짜보다 큽니다.",
            });
            return;
          }
        }
      }
    },
    //#endregion
    validDateType(value) {
      const dateRegex = /^(\d{0,4})[-]?\d{0,2}[-]?\d{0,2}$/;
      return !dateRegex.test(value);
    },
    onContext(ctx) {
      // The date formatted in the locale, or the `label-no-date-selected` string
      this.formatted = ctx.selectedFormatted;
      // The following will be an empty string until a valid date is entered
      this.dateSelected = ctx.selectedYMD;
    },
    convertDateSTH(value) {
      const replaceVal = value.replace(/-/g, "");
      const yyyy = replaceVal.substring(0, 4);
      const mm = replaceVal.substring(4, 6);
      const dd = replaceVal.substring(6, 8);
      if (12 < mm) {
        this.brdDT = "";
      } else if (31 < dd) {
        this.brdDT = "";
      } else {
        return `${yyyy}-${mm}-${dd}`;
      }
    },
  },
};
</script>
<style>
.scr-modal-footer {
  padding-left: 1.5rem !important;
}
</style>
