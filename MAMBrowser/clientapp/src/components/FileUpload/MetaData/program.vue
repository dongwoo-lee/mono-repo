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
          />
        </b-form-group>
        <button
          v-show="memoState"
          style="
            position: relative;
            left: 315px;
            top: -40px;
            z-index: 99;
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
            left: 310px;
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
          z-index: 9999;
          font-size: 16px;
        "
      >
        <b-form-group label="제작자" class="has-float-label">
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
    <div style="position: absolute; top: 40px">
      <b-form-group
        label="방송일"
        class="has-float-label"
        style="position: absolute; z-index: 9989; font-color: black"
      >
        <b-input-group class="mb-3" style="width: 300px; float: left">
          <input
            :disabled="isActive"
            id="dateinput"
            type="text"
            class="form-control input-picker"
            :value="date"
            @input="onInput"
          />
          <b-input-group-append>
            <b-form-datepicker
              :value="date"
              @input="eventInput"
              button-only
              :disabled="isActive"
              :button-variant="getVariant"
              right
              aria-controls="example-input"
              @context="onContext"
            ></b-form-datepicker>
          </b-input-group-append>
        </b-input-group>
      </b-form-group>
      <button
        v-show="!isActive"
        style="
          position: absolute;
          right: -220px;
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
          @click="resetDate"
        ></b-icon>
      </button>

      <b-form-group
        label="매체"
        class="has-float-label"
        style="position: absolute; margin-left: 320px; z-index: 9999"
      >
        <b-form-select
          :disabled="isActive"
          id="program-media"
          class="media-select"
          style="width: 140px; height: 37px"
          :value="this.programMedia"
          @input="mediaChange"
          :options="fileMediaOptions"
        />
      </b-form-group>
      <b-button
        :disabled="isActive"
        :variant="getVariant"
        style="position: absolute; width: 70px; right: -550px; z-index: 9989"
        @click="getPro"
        >검색</b-button
      >
    </div>
    <div
      v-show="this.MetaData.typeSelected == 'program'"
      style="position: absolute; width: 550px; top: 90px; height: 210px"
    >
      <DxDataGrid
        ref="my-proDataGrid"
        v-show="this.ProgramData.eventName != ''"
        style="
          height: 295px;
          border: 1px solid silver;
          font-family: 'MBC 새로움 M';
        "
        :data-source="ProgramData"
        :selection="{ mode: 'single' }"
        :show-borders="false"
        :hover-state-enabled="true"
        key-expr="productId"
        :allow-column-resizing="true"
        :column-auto-width="true"
        no-data-text="No Data"
        @row-click="onRowClick"
      >
        <tbody
          slot="rowTemplate"
          slot-scope="{
            data: {
              data: { eventName, eventType, productId, onairTime, durationSec },
            },
          }"
          class="dx-row"
        >
          <tr
            v-if="!userProgramList.includes(productId) && eventName != ''"
            :class="[getProductId(productId)] ? 'disabledRow' : ''"
          >
            <td>{{ eventName }}</td>
            <td>{{ eventType }}</td>
            <td>{{ productId }}</td>
            <td>{{ onairTime }}</td>
            <td>{{ durationSec }}</td>
          </tr>
          <tr v-if="userProgramList.includes(productId) && eventName != ''">
            <!-- <td><b-icon-alarm></b-icon-alarm> 아이콘 추가</td> -->
            <td>{{ eventName }}</td>
            <td>{{ eventType }}</td>
            <td>{{ productId }}</td>
            <td>{{ onairTime }}</td>
            <td>{{ durationSec }}</td>
          </tr>
        </tbody>
        <DxColumn data-field="eventName" caption="이벤트 명" />
        <DxColumn :width="50" data-field="eventType" caption="타입" />
        <DxColumn :width="95" data-field="productId" caption="프로그램 ID" />
        <DxColumn data-field="onairTime" caption="방송 시간" />
        <DxColumn :width="80" data-field="durationSec" caption="편성 분량" />
        <DxSelection mode="single" />
        <DxPager :visible="false" />
        <DxScrolling mode="standard" />
      </DxDataGrid>
    </div>
    <!-- 프로그램 -->
    <div
      v-show="!isActive && this.ProgramSelected.eventName != ''"
      style="
        width: 550px;
        height: 82px;
        margin-top: 350px;
        padding-top: 20px;
        padding-left: 10px;
        padding-right: 10px;
        float: left;
        border: 1px solid silver;
        font-size: 16px;
        font-family: 'MBC 새로움 M';
      "
    >
      <!-- <div style="width: 550px">
        <b-form-group label="제목" class="has-float-label">
          <b-form-input
            class="editTask"
            style="width: 530px"
            v-model="getTitle"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div> -->
      <div style="width: 200px; float: left">
        <b-form-group label="이벤트 명" class="has-float-label">
          <b-form-input
            class="editTask"
            style="width: 200px"
            v-model="this.ProgramSelected.eventName"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 170px; float: left; margin-left: 20px">
        <b-form-group label="방송 시간" class="has-float-label">
          <b-form-input
            style="width: 170px"
            class="editTask"
            v-model="this.ProgramSelected.onairTime"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 100px; margin-left: 20px; float: left">
        <b-form-group label="편성 분량" class="has-float-label">
          <b-form-input
            style="width: 100px"
            class="editTask"
            v-model="this.ProgramSelected.durationSec"
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
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import { mapState, mapGetters, mapMutations } from "vuex";
import { DxScrolling, DxLoadPanel, DxPager } from "devextreme-vue/data-grid";
import axios from "axios";
const dxdg = "my-proDataGrid";
export default {
  components: {
    CommonVueSelect,
    DxScrolling,
    DxLoadPanel,
    DxPager,
  },
  mixins: [CommonFileFunction, MixinBasicPage],
  data() {
    return {
      programMedia: "A",
      mediaName: "AM",
      dxdg,
    };
  },
  created() {
    this.reset();
    this.getEditorForPd();
    this.resetFileMediaOptions();

    axios.get("/api/categories/media").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name,
        });
      });
    });

    this.programMedia = "A";
    this.setMediaSelected(this.programMedia);

    var user_id = sessionStorage.getItem("user_id");
    axios
      .get(
        `/api/categories/user-pgmcodes?userId=${user_id}&media=${this.MetaData.mediaSelected}`
      )
      .then((res) => {
        this.setUserProgramList(res.data.resultObject.data);
      });

    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.setDate(today);
    this.setTempDate(today);

    this.getPro();
  },
  computed: {
    getTitle() {
      return `[${this.date}] [${this.mediaName}] [${this.ProgramSelected.eventName}]`;
    },
    getProgramId(productId) {
      if (this.userProgramList.includes(productId)) {
        return true;
      } else {
        return false;
      }
    },
    proDataGrid: function () {
      return this.$refs[dxdg].instance;
    },
  },
  methods: {
    mediaChange(v) {
      this.setMediaSelected(v);
      var data = this.fileMediaOptions.find((dt) => dt.value == v);
      this.mediaName = data.text;
      this.getPro();
    },
  },
};
</script>

<style scoped>
.disabledRow {
  color: silver !important;
}
.dx-row :hover {
  background-color: #f5f5f5;
}
</style>
