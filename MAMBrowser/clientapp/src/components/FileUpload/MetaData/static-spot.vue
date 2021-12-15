<template>
  <div>
    <transition name="fade">
      <div
        style="
          position: absolute;
          top: 350px;
          left: -400px;
          z-index: 9999;
          font-size: 16px;
        "
      >
        <b-form-group label="메모" class="has-float-label">
          <b-form-input
            class="editTask"
            v-model="MetaData.memo"
            :state="memoState"
            :maxLength="30"
            aria-describedby="input-live-help input-live-feedback"
            placeholder="메모"
            trim
        /></b-form-group>
        <button
          v-show="memoState"
          style="
            position: relative;
            left: 315px;
            top: -40px;
            z-index: 9999;
            width: 3px;
            heigth: 3px;
            background-color: #ffffff;
            border: 0;
            outline: 0;
          "
        >
          <b-icon
            icon="x-circle"
            font-scale="1"
            style="position: relative; top: 0px; right: 0px; z-index: 999"
            variant="secondary"
            @click="resetMemo"
          ></b-icon>
        </button>
        <p
          v-show="memoState"
          style="
            position: relative;
            left: 315px;
            top: -35px;
            z-index: 9999;
            width: 30px;
            margin-right: 0px;
          "
        >
          {{ MetaData.memo.length }}/30
        </p>
      </div>
    </transition>
    <transition name="fade">
      <div
        style="
          position: absolute;
          top: 415px;
          left: -400px;
          font-size: 16px;
          z-index: 9999;
        "
      >
        <b-form-group label="광고주" class="has-float-label">
          <b-form-input
            class="editTask"
            v-model="MetaData.advertiser"
            :state="advertiserState"
            :maxLength="50"
            aria-describedby="input-live-help input-live-feedback"
            placeholder="광고주"
            trim
          />
        </b-form-group>
        <button
          v-show="advertiserState"
          style="
            position: relative;
            left: 315px;
            top: -40px;
            z-index: 9999;
            width: 3px;
            heigth: 3px;
            background-color: #ffffff;
            border: 0;
            outline: 0;
          "
        >
          <b-icon
            icon="x-circle"
            font-scale="1"
            style="position: relative; top: 0px; right: 0px; z-index: 9999"
            variant="secondary"
            @click="resetAdvertiser"
          ></b-icon>
        </button>
        <p
          v-show="advertiserState"
          style="
            position: relative;
            left: 315px;
            top: -35px;
            z-index: 9999;
            width: 30px;
            margin-right: 0px;
          "
        >
          {{ MetaData.advertiser.length }}/50
        </p>
      </div>
    </transition>
    <transition name="fade">
      <div>
        <b-form-group
          label="제작자"
          class="has-float-label"
          style="
            position: absolute;
            top: 485px;
            left: -400px;
            z-index: 9999;
            font-size: 16px;
          "
        >
          <b-form-input
            title="제작자"
            style="width: 350px; font-size: 14px"
            class="editTask"
            :value="userID"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            placeholder="제작자"
            trim
          />
        </b-form-group>
      </div>
    </transition>
    <!-- 시작/종료일 -->
    <div style="position: absolute; top: 40px; width: 550px">
      <div>
        <b-form-group
          label="시작일"
          class="has-float-label"
          style="width: 180px; float: left; margin-right: 10px"
        >
          <b-input-group class="mb-3" style="width: 180px; float: left">
            <input
              style="height: 33px; font-size: 13px"
              id="sdateinput"
              type="text"
              class="form-control input-picker"
              :value="fileSDate"
              @input="onsInput"
            />
            <b-input-group-append>
              <b-form-datepicker
                style="height: 33px"
                :value="fileSDate"
                @input="eventSInput"
                button-only
                button-variant="outline-dark"
                left
                aria-controls="example-input"
                @context="onContext"
              ></b-form-datepicker>
            </b-input-group-append>
          </b-input-group>
          <button
            v-show="!isActive"
            style="
              position: absolute;
              right: 72px;
              top: 7px;
              z-index: 9999;
              width: 3px;
              background-color: #ffffff;
              border: 0;
              outline: 0;
            "
          >
            <b-icon
              icon="x-circle"
              font-scale="1"
              style="position: absolute; z-index: 9999"
              variant="secondary"
              @click="resetFileSDate"
            ></b-icon>
          </button>
        </b-form-group>
        <b-form-group
          label="종료일"
          class="has-float-label"
          style="width: 180px; float: left; margin-right: 10px"
        >
          <b-input-group class="mb-3" style="width: 180px; float: left">
            <input
              style="height: 33px; font-size: 13px"
              id="edateinput"
              type="text"
              class="form-control input-picker"
              :value="fileEDate"
              @input="oneInput"
            />
            <b-input-group-append>
              <b-form-datepicker
                style="height: 33px"
                :value="fileEDate"
                @input="eventEInput"
                button-only
                button-variant="outline-dark"
                right
                aria-controls="example-input"
                @context="onContext"
              ></b-form-datepicker>
            </b-input-group-append>
          </b-input-group>
          <button
            v-show="!isActive"
            style="
              position: absolute;
              right: 72px;
              top: 7px;
              z-index: 9999;
              width: 3px;
              background-color: #ffffff;
              border: 0;
              outline: 0;
            "
          >
            <b-icon
              icon="x-circle"
              font-scale="1"
              style="position: absolute; z-index: 9999"
              variant="secondary"
              @click="resetFileEDate"
            ></b-icon>
          </button>
        </b-form-group>
      </div>
      <div>
        <b-form-group
          label="매체"
          class="has-float-label"
          style="float: left; margin-right: 10px"
        >
          <b-form-select
            id="program-media"
            class="media-select"
            style="width: 80px; height: 33px"
            :value="staticMedia"
            :options="fileMediaOptions"
            @input="mediaChange"
          />
        </b-form-group>
      </div>
      <b-button
        :disabled="isActive"
        :variant="getVariant"
        @click="getPro"
        style="height: 33px"
        >검색</b-button
      >
    </div>
    <div style="position: absolute; top: 100px">
      <DxDataGrid
        name="mcrDxDataGrid"
        v-show="this.EventData.id != ''"
        style="
          height: 300px;
          border: 1px solid silver;
          font-family: 'MBC 새로움 M';
        "
        :data-source="EventData"
        :selection="{ mode: 'single' }"
        :show-borders="true"
        :hover-state-enabled="true"
        key-expr="id"
        :allow-column-resizing="true"
        :column-auto-width="true"
        no-data-text="No Data"
        @row-click="onRowClick"
      >
        <DxPager :visible="true" />
        <DxColumn data-field="name" caption="이벤트 명" />
        <DxColumn data-field="id" caption="이벤트 ID" />
        <DxColumn data-field="startDate" caption="시작일" />
        <DxColumn data-field="duration" caption="편성분량" />
      </DxDataGrid>
    </div>
    <!-- 프로그램 -->
    <div
      v-show="!isActive && EventSelected.id != ''"
      style="
        position: absolute;
        top: 420px;
        width: 550px;
        height: 110px;
        padding-top: 10px;
        padding-left: 10px;
        padding-right: 10px;
        float: left;
        border: 1px solid silver;
        font-family: 'MBC 새로움 M';
      "
    >
      <div style="width: 180px; float: left">
        <b-form-group
          label="이벤트 명"
          class="has-float-label"
          style="margin-top: 20px"
        >
          <b-form-input
            style="width: 180px"
            class="editTask"
            v-model="EventSelected.name"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 180px; margin-left: 20px; float: left">
        <b-form-group
          label="방송 시작일"
          class="has-float-label"
          style="margin-top: 20px"
        >
          <b-form-input
            style="width: 180px"
            class="editTask"
            v-model="EventSelected.startDate"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 120px; margin-left: 20px; float: left">
        <b-form-group
          label="편성 분량"
          class="has-float-label"
          style="margin-top: 20px"
        >
          <b-form-input
            style="width: 120px"
            class="editTask"
            v-model="EventSelected.duration"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
    </div>
  </div>
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import mixinFillerPage from "../../../mixin/MixinFillerPage";
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
import { DxPager } from "devextreme-vue/data-grid";
import axios from "axios";
export default {
  components: {
    CommonVueSelect,
    DxPager,
  },
  mixins: [CommonFileFunction, MixinBasicPage, mixinFillerPage],
  data() {
    return {
      staticMedia: "",
      sdate: "",
      edate: "",
    };
  },
  created() {
    this.reset();
    this.getEditorForMd(); //제작자
    this.resetFileMediaOptions(); //매체 초기화
    //매체 생성
    axios.get("/api/categories/media").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name,
        });
      });
    });
    this.staticMedia = "A"; //매체 초기 값 설정
    this.setMediaSelected(this.staticMedia); //매체 초기값 store 설정

    //분류
    this.getTimetoneOptions();
    //상태
    this.getReqStatusOptions();

    // 시작/종료일 초기값 설정
    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.edate = today;
    this.setFileEDate(today);

    var newDate = new Date();
    var dayOfMonth = newDate.getDate();
    newDate.setDate(dayOfMonth - 7);
    newDate = this.$fn.formatDate(newDate, "yyyy-MM-dd");

    this.sdate = newDate;
    this.setFileSDate(newDate);

    this.getPro();
  },
  watch: {
    sdate() {
      const replaceAllFileSDate = this.sdate.replace(/-/g, "");
      const replaceAllFileEDate = this.edate.replace(/-/g, "");
      if (
        replaceAllFileEDate < replaceAllFileSDate &&
        replaceAllFileEDate != ""
      ) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
      }
    },
    edate() {
      const replaceAllFileSDate = this.sdate.replace(/-/g, "");
      const replaceAllFileEDate = this.edate.replace(/-/g, "");
      if (replaceAllFileEDate < replaceAllFileSDate) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
      }
    },
  },
  methods: {
    mediaChange(v) {
      this.setMediaSelected(v);
    },
    eventSInput(value) {
      this.sdate = value;
      this.setFileSDate(value);
    },
    eventEInput(value) {
      this.edate = value;
      this.setFileEDate(value);
    },
    onsInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        event.target.value = targetValue.slice(0, -1);
        this.$fn.notify("error", { message: "날짜 형식 오류입니다." });
        return;
      }

      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          this.sdate = convertDate;
          this.setFileSDate(convertDate);
        }
      }
    },
    oneInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        event.target.value = targetValue.slice(0, -1);
        this.$fn.notify("error", { message: "날짜 형식 오류입니다." });
        return;
      }

      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          this.edate = convertDate;
          this.setFileEDate(convertDate);
        }
      }
    },
    getData() {},
  },
};
</script>

<style>
.date-input:focus {
  border: 1px solid #4475c4 !important;
}
.date-input {
  border: 1px solid silver !important;
}
</style>
