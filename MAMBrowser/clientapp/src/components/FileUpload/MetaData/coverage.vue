<template>
  <div>
    <div style="margin-top: 35px; font-size: 15px">
      <b-form-group
        label="방송일"
        class="has-float-label"
        style="width: 190px; float: left"
      >
        <b-input-group class="mb-3" style="width: 190px">
          <input
            :disabled="isActive"
            id="dateinput"
            type="text"
            class="form-control input-picker date-input"
            :value="reportMetaData.date"
            @input="onInput"
          />
          <b-input-group-append>
            <b-form-datepicker
              :value="reportMetaData.date"
              @input="eventInput"
              button-only
              :disabled="isActive"
              :button-variant="getVariant"
              left
              aria-controls="example-input"
              @context="onContext"
            ></b-form-datepicker>
          </b-input-group-append>
        </b-input-group>
      </b-form-group>

      <b-form-group
        label="분류"
        class="has-float-label"
        style="margin-left: 20px; float: left"
      >
        <b-form-select
          :disabled="isActive"
          id="program-media"
          class="media-select"
          style="width: 120px; height: 37px"
          :value="reportMetaData.category"
          :options="reportCategoryOptions"
          @input="categoryChanged"
        />
      </b-form-group>
      <b-button
        :disabled="isActive"
        :variant="getVariant"
        style="width: 70px; margin-left: 20px"
        @click="onSearch"
        >검색</b-button
      >
    </div>
    <div style="font-family: 'MBC 새로움 M'; font-size: 15px">
      <div style="width: 425px; float: left">
        <b-form-group
          label="이벤트 명"
          class="has-float-label"
          style="margin-top: -10px"
        >
          <b-form-input
            style="width: 425px"
            class="editTask"
            v-model="reportSelected.eventName"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 425px; float: left; margin-top: 5px">
        <b-form-group
          label="이벤트 ID"
          class="has-float-label"
          style="margin-top: 5px"
        >
          <b-form-input
            style="width: 425px"
            class="editTask"
            v-model="reportSelected.productId"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
    </div>

    <div style="font-size: 15px; margin-top: 165px; margin-bottom: 0px">
      <b-form-group label="소재명" class="has-float-label">
        <b-form-input
          class="editTask"
          v-model="reportMetaData.title"
          :state="reportTitleState"
          :maxLength="30"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="소재명"
          trim
        />
      </b-form-group>
      <p
        v-show="reportTitleState"
        style="
          position: relative;
          left: 390px;
          top: -15px;
          width: 30px;
          margin-bottom: -18px;
          margin-right: 0px;
        "
      >
        {{ reportMetaData.title.length }}/30
      </p>
    </div>
    <div style="font-size: 15px; margin-top: 0px; float: left">
      <b-form-group label="메모" class="has-float-label">
        <b-form-input
          style="width: 200px"
          class="editTask"
          v-model="reportMetaData.memo"
          :maxLength="30"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="메모"
          trim
        />
      </b-form-group>
      <p
        v-show="reportMemoState"
        style="
          position: relative;
          left: 165px;
          top: -15px;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ reportMetaData.memo.length }}/30
      </p>
    </div>
    <div
      style="font-size: 15px; margin-top: 0px; float: left; margin-left: 25px"
    >
      <b-form-group label="취재인" class="has-float-label">
        <b-form-input
          style="width: 200px"
          class="editTask"
          v-model="reportMetaData.reporter"
          :state="reportReporterState"
          :maxLength="10"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="취재인"
          trim
        />
      </b-form-group>
      <p
        v-show="reportReporterState"
        style="
          position: relative;
          left: 165px;
          top: -15px;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ reportMetaData.reporter.length }}/10
      </p>
    </div>
    <b-modal
      size="lg"
      v-model="modal"
      centered
      hide-header-close
      no-close-on-esc
      no-close-on-backdrop
      footer-class="scr-modal-footer"
    >
      <template slot="modal-title">
        <h5>프로그램 선택</h5>
      </template>
      <template slot="default">
        <div>
          <b-form-group
            label="매체"
            class="has-float-label"
            style="font-size: 15px"
          >
            <b-form-select
              :disabled="isActive"
              id="program-media"
              class="media-select"
              style="width: 95px; height: 37px"
              :value="reportMetaData.media"
              @input="mediaChanged"
              :options="reportMediaOptions"
            />
          </b-form-group>
          <DxDataGrid
            name="mcrDxDataGrid"
            style="
              height: 295px;
              border: 1px solid silver;
              font-family: 'MBC 새로움 M';
            "
            :data-source="reportDataOptions"
            :selection="{ mode: 'single' }"
            :show-borders="true"
            :hover-state-enabled="true"
            key-expr="id"
            :allow-column-resizing="true"
            :column-auto-width="true"
            no-data-text="No Data"
            @row-click="onRowClick"
          >
            <DxLoadPanel :enabled="true" />
            <DxScrolling mode="virtual" />
            <DxColumn data-field="eventName" caption="이벤트 명" />
            <DxColumn data-field="productId" caption="이벤트 ID" />
            <DxColumn data-field="onairTime" caption="방송시작 시간" />
          </DxDataGrid>
        </div>
      </template>
      <template v-slot:modal-footer>
        <b-button
          style="margin-left: 20px; height: 34px"
          variant="outline-primary"
          @click="modalOff"
        >
          확인
        </b-button>
        <b-button
          variant="outline-danger default cutom-label-cancel"
          size="sm"
          class="float-right"
          @click="modalReset"
        >
          닫기</b-button
        >
      </template>
    </b-modal>
  </div>
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
import { DxScrolling, DxLoadPanel } from "devextreme-vue/data-grid";
import axios from "axios";
export default {
  components: {
    CommonVueSelect,
    DxScrolling,
    DxLoadPanel,
  },
  mixins: [CommonFileFunction],
  data() {
    return {
      modal: false,
    };
  },
  async created() {
    this.RESET_REPORT();
    this.SET_REPORT_TITLE(this.sliceExt(30));

    this.RESET_REPORT_CATEGORY_OPTIONS();

    var res = await axios.get("/api/categories/report");

    this.SET_REPORT_CATEGORY(res.data.resultObject.data[0].id);

    res.data.resultObject.data.forEach((e) => {
      this.SET_REPORT_CATEGORY_OPTIONS({
        value: e.id,
        text: e.name,
      });
    });

    this.RESET_REPORT_MEDIA_OPTIONS();
    var res = await axios.get("/api/categories/media");

    this.SET_REPORT_MEDIA(res.data.resultObject.data[0].id);

    res.data.resultObject.data.forEach((e) => {
      this.SET_REPORT_MEDIA_OPTIONS({
        value: e.id,
        text: e.name,
      });
    });

    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.SET_REPORT_DATE(today);
    this.SET_REPORT_TEMP_DATE(today);
  },
  computed: {
    ...mapState("FileIndexStore", {
      MetaModalTitle: (state) => state.MetaModalTitle,
      reportMetaData: (state) => state.reportMetaData,
      reportMediaOptions: (state) => state.reportMediaOptions,
      reportCategoryOptions: (state) => state.reportCategoryOptions,
      reportDataOptions: (state) => state.reportDataOptions,
      reportSelected: (state) => state.reportSelected,
    }),
    ...mapGetters("FileIndexStore", [
      "reportTitleState",
      "reportMemoState",
      "reportReporterState",
    ]),
  },
  methods: {
    ...mapMutations("FileIndexStore", [
      "SET_REPORT_TITLE",
      "SET_REPORT_MEDIA",
      "SET_REPORT_CATEGORY",
      "SET_REPORT_DATE",
      "SET_REPORT_TEMP_DATE",
      "SET_REPORT_MEDIA_OPTIONS",
      "SET_REPORT_CATEGORY_OPTIONS",
      "SET_REPORT_DATA_OPTIONS",
      "SET_REPORT_SELECTED",
      "RESET_REPORT_MEDIA_OPTIONS",
      "RESET_REPORT_CATEGORY_OPTIONS",
      "RESET_REPORT_DATA_OPTIONS",
      "RESET_REPORT_SELECTED",
      "RESET_REPORT",
    ]),
    onSearch() {
      this.modalOn();
      this.getPro();
    },
    categoryChanged(v) {
      this.SET_REPORT_CATEGORY(v);
    },
    mediaChanged(v) {
      this.SET_REPORT_MEDIA(v);
      this.getPro();
    },
    onRowClick(v) {
      this.SET_REPORT_SELECTED(v.data);
    },
    async getPro() {
      if (this.reportMetaData.date == "") {
        const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
        this.SET_REPORT_DATE(today);
        this.SET_REPORT_TEMP_DATE(today);
      }

      const replaceVal = this.reportMetaData.date.replace(/-/g, "");
      const yyyy = replaceVal.substring(0, 4);
      const mm = replaceVal.substring(4, 6);
      const dd = replaceVal.substring(6, 8);
      var date = yyyy + "" + mm + "" + dd;

      this.RESET_REPORT_DATA_OPTIONS();

      var res = await axios.get(
        `/api/categories/pgm-sch?media=${this.reportMetaData.media}&date=${date}`
      );

      this.SET_REPORT_DATA_OPTIONS(res.data.resultObject.data);

      this.RESET_REPORT_SELECTED();
    },
    eventInput(event) {
      this.SET_REPORT_DATE(event);
      this.SET_REPORT_TEMP_DATE(event);
      this.getPro();
    },
    onInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.reportMetaData.tempDate == null) {
          event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
        }
        event.target.value = this.reportMetaData.tempDate;
        return;
      }

      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 0) {
          event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
          this.SET_REPORT_DATE = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
          this.SET_REPORT_TEMP_DATE = this.$fn.formatDate(
            new Date(),
            "yyyy-MM-dd"
          );
        }
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          if (
            convertDate == "" ||
            convertDate == null ||
            convertDate == "undefined"
          ) {
            event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
            this.SET_REPORT_DATE(this.$fn.formatDate(new Date(), "yyyy-MM-dd"));
            this.SET_REPORT_TEMP_DATE(
              this.$fn.formatDate(new Date(), "yyyy-MM-dd")
            );
            return;
          }
          this.SET_REPORT_DATE(convertDate);
          this.SET_REPORT_TEMP_DATE(convertDate);
          this.getPro();
        }
      }
    },
    convertDateSTH(value) {
      const replaceVal = value.replace(/-/g, "");
      const yyyy = replaceVal.substring(0, 4);
      const mm = replaceVal.substring(4, 6);
      const dd = replaceVal.substring(6, 8);
      if (12 < mm) {
        this.SET_REPORT_DATE("");
      } else if (31 < dd) {
        this.SET_REPORT_DATE("");
      } else {
        return `${yyyy}-${mm}-${dd}`;
      }
    },
    onContext(ctx) {
      this.formatted = ctx.selectedFormatted;
      this.dateSelected = ctx.selectedYMD;
    },
    sliceExt(maxLength) {
      var result = this.MetaModalTitle.replace(/(.wav|.mp3)$/, "");
      return result.substring(0, maxLength);
    },
    modalOn() {
      this.modal = true;
    },
    modalOff() {
      this.modal = false;
    },
    modalReset() {
      this.RESET_REPORT_SELECTED();
      this.modal = false;
    },
  },
};
</script>

<style></style>
