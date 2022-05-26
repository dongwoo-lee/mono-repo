<template>
  <div>
    <div style="margin-top: 35px">
      <b-form-group
        label="방송일"
        class="has-float-label"
        style="font-color: black; font-size: 15px; float: left"
      >
        <b-input-group
          class="mb-3"
          style="width: 220px; margin-right: 20px; float: left"
        >
          <input
            :disabled="isActive"
            id="dateinput"
            type="text"
            class="form-control input-picker"
            :value="pgmMetaData.date"
            @input="onInput"
          />
          <b-input-group-append>
            <b-form-datepicker
              :value="pgmMetaData.date"
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

      <b-form-group
        label="매체"
        class="has-float-label"
        style="margin-right: 20px; font-size: 15px; float: left"
      >
        <b-form-select
          :disabled="isActive"
          id="program-media"
          class="media-select"
          style="width: 95px; height: 37px"
          :value="this.pgmMetaData.media"
          @input="mediaChange"
          :options="pgmMediaOptions"
        />
      </b-form-group>
      <b-button
        :disabled="isActive"
        :variant="getVariant"
        style="width: 70px"
        @click="onSearch"
        >검색</b-button
      >
    </div>
    <div
      style="
        width: 550px;
        margin-top: 20px;
        font-size: 16px;
        font-family: 'MBC 새로움 M';
      "
    >
      <div style="width: 425px; float: left; margin-top: -10px">
        <b-form-group label="이벤트 명" class="has-float-label">
          <b-form-input
            class="editTask"
            style="width: 425px"
            v-model="this.pgmSelected.eventName"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 425px; float: left; margin-top: 10px">
        <b-form-group label="방송 시간" class="has-float-label">
          <b-form-input
            style="width: 425px"
            class="editTask"
            v-model="this.pgmSelected.onairTime"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 425px; float: left; margin-top: 5px">
        <b-form-group label="편성 분량" class="has-float-label">
          <b-form-input
            style="width: 425px"
            class="editTask"
            v-model="this.pgmSelected.durationSec"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
    </div>
    <div style="height: 50px; margin-top: 225px">
      <b-form-group
        label="메모"
        class="has-float-label"
        style="font-size: 15px"
      >
        <b-form-input
          class="editTask"
          v-model="pgmMetaData.memo"
          :state="pgmMemoState"
          :maxLength="200"
          placeholder="메모"
          trim
        />
      </b-form-group>
      <p style="margin-left: 392px; margin-top: -15px" v-show="pgmMemoState">
        {{ pgmMetaData.memo.length }}/30
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
          <DxDataGrid
            ref="my-proDataGrid"
            v-show="this.pgmDataOptions.eventName != ''"
            style="
              height: 280px;
              border: 1px solid silver;
              font-family: 'MBC 새로움 M';
            "
            :data-source="pgmDataOptions"
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
                  data: {
                    eventName,
                    productId,
                    onairTime,
                    durationSec,
                    audioClipID,
                  },
                },
              }"
              class="dx-row"
            >
              <tr
                v-if="
                  !userPgmList.includes(productId) &&
                  eventName != '' &&
                  role != 'ADMIN'
                "
                :class="[this.getProductId(productId)] ? 'disabledRow' : ''"
              >
                <td>{{ eventName }}</td>
                <td>{{ productId }}</td>
                <td>{{ onairTime }}</td>
                <td>{{ durationSec }}</td>
                <td
                  :class="
                    [this.getAudioClipID(audioClipID)] ? 'disabledGrayCell' : ''
                  "
                  style="text-align: center"
                >
                  {{ audioClipID == null ? "" : "O" }}
                </td>
              </tr>
              <tr
                v-if="
                  (userPgmList.includes(productId) && eventName != '') ||
                  role == 'ADMIN'
                "
              >
                <td>{{ eventName }}</td>
                <td>{{ productId }}</td>
                <td>{{ onairTime }}</td>
                <td>{{ durationSec }}</td>
                <td
                  :class="
                    [this.getAudioClipID(audioClipID)] ? 'disabledCell' : ''
                  "
                  style="text-align: center"
                >
                  {{ audioClipID == null ? "" : "O" }}
                </td>
              </tr>
            </tbody>
            <DxColumn data-field="eventName" caption="이벤트 명" />
            <DxColumn
              :width="120"
              data-field="productId"
              caption="프로그램 ID"
            />
            <DxColumn data-field="onairTime" caption="방송 시간" />
            <DxColumn
              :width="80"
              data-field="durationSec"
              caption="편성 분량"
            />
            <DxColumn :width="50" data-field="audioClipID" caption="파일" />
            <DxSelection mode="single" />
            <DxScrolling mode="virtual" />
          </DxDataGrid>
        </div>
      </template>
      <template v-slot:modal-footer>
        <b-button
          variant="outline-primary default cutom-label"
          size="sm"
          class="float-right"
          @click="modalOff"
        >
          확인</b-button
        >
        <b-button
          variant="outline-danger default cutom-label-cancel"
          size="sm"
          class="float-right"
          @click="modalReset"
        >
          취소</b-button
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
const dxdg = "my-proDataGrid";
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
      mediaName: "",
      dxdg,
    };
  },
  async created() {
    this.RESET_PGM();
    this.RESET_PGM_MEDIA_OPTIONS();

    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.SET_PGM_DATE(today);
    this.SET_PGM_TEMP_DATE(today);

    var res = await axios.get("/api/categories/media");

    this.mediaName = res.data.resultObject.data[0].id;
    this.SET_PGM_MEDIA_SELECTED(res.data.resultObject.data[0].id);

    res.data.resultObject.data.forEach((e) => {
      this.SET_PGM_MEDIA_OPTIONS({
        value: e.id,
        text: e.name,
      });
    });

    var user_id = sessionStorage.getItem("user_id");
    var res = await axios.get(
      `/api/categories/user-pgmcodes?userId=${user_id}&media=${this.pgmMetaData.media}`
    );

    this.SET_USER_PGM_LIST(res.data.resultObject.data);

    this.getPro();
  },
  computed: {
    ...mapState("FileIndexStore", {
      pgmMetaData: (state) => state.pgmMetaData,
      pgmMediaOptions: (state) => state.pgmMediaOptions,
      userPgmList: (state) => state.userPgmList,
      pgmDataOptions: (state) => state.pgmDataOptions,
      pgmSelected: (state) => state.pgmSelected,
    }),
    ...mapGetters("FileIndexStore", ["pgmMemoState"]),
    proDataGrid: function () {
      return this.$refs[dxdg].instance;
    },
  },
  methods: {
    ...mapMutations("FileIndexStore", [
      "SET_PGM_DATE",
      "SET_PGM_TEMP_DATE",
      "SET_PGM_TITLE",
      "SET_PGM_MEDIA_SELECTED",
      "SET_PGM_MEDIA_OPTIONS",
      "SET_USER_PGM_LIST",
      "SET_PGM_SELECTED",
      "RESET_PGM_DATE",
      "RESET_PGM_TEMP_DATE",
      "RESET_PGM_MEDIA_OPTIONS",
      "RESET_PGM_DATA_OPTIONS",
      "RESET_PGM_SELECTED",
      "RESET_PGM",
    ]),
    getProductId(productId) {
      if (this.userPgmList.includes(productId)) {
        return true;
      } else {
        return false;
      }
    },
    getAudioClipID(audioClipID) {
      if (audioClipID != "") {
        return true;
      } else {
        return false;
      }
    },
    mediaChange(v) {
      this.SET_PGM_MEDIA_SELECTED(v);
      var data = this.pgmMediaOptions.find((dt) => dt.value == v);
      this.mediaName = data.text;
      this.getPro();
    },
    onSearch() {
      this.modalOn();
      this.getPro();
    },
    async getPro() {
      if (this.pgmMetaData.date == "") {
        return;
      }
      this.RESET_PGM_DATA_OPTIONS();

      const replaceVal = this.pgmMetaData.date.replace(/-/g, "");
      const yyyy = replaceVal.substring(0, 4);
      const mm = replaceVal.substring(4, 6);
      const dd = replaceVal.substring(6, 8);
      var date = yyyy + "" + mm + "" + dd;

      var res = await axios.get(
        `/api/categories/pgm-sch?media=${this.pgmMetaData.media}&date=${date}`
      );

      var value = res.data.resultObject.data;
      value.forEach((e) => {
        e.durationSec = this.getDurationSec(e.durationSec);
        e.onairTime = this.getOnAirTime(e.onairTime);
      });
      this.SET_PGM_DATA_OPTIONS(res.data.resultObject.data);
      this.RESET_PGM_SELECTED();
    },
    onRowClick(v) {
      if (
        !this.userPgmList.includes(v.data.productId) &&
        this.role != "ADMIN"
      ) {
        this.proDataGrid.deselectRows(v.data.productId);
        this.RESET_PGM_SELECTED();
        return;
      }
      this.SET_PGM_SELECTED(v.data);
      this.SET_PGM_TITLE(
        `[${this.pgmMetaData.date}] [${this.mediaName}] [${this.pgmSelected.eventName}]`
      );
    },
    eventInput(event) {
      this.SET_PGM_DATE(event);
      this.SET_PGM_TEMP_DATE(event);
      this.getPro();
    },
    onInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.tempDate == null) {
          event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
        }
        event.target.value = this.tempDate;
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
            this.SET_PGM_DATE(this.$fn.formatDate(new Date(), "yyyy-MM-dd"));
            this.SET_PGM_TEMP_DATE(
              this.$fn.formatDate(new Date(), "yyyy-MM-dd")
            );
            return;
          }
          this.SET_PGM_DATE(convertDate);
          this.SET_PGM_TEMP_DATE(convertDate);
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
        this.SET_PGM_DATE("");
      } else if (31 < dd) {
        this.SET_PGM_DATE("");
      } else {
        return `${yyyy}-${mm}-${dd}`;
      }
    },
    async getPro() {
      const replaceVal = this.pgmMetaData.date.replace(/-/g, "");
      const yyyy = replaceVal.substring(0, 4);
      const mm = replaceVal.substring(4, 6);
      const dd = replaceVal.substring(6, 8);
      var date = yyyy + "" + mm + "" + dd;

      this.RESET_PGM_DATA_OPTIONS();

      var res = await axios.get(
        `/api/categories/pgm-sch?media=${this.pgmMetaData.media}&date=${date}`
      );

      var value = res.data.resultObject.data;
      value.forEach((e) => {
        e.durationSec = this.getDurationSec(e.durationSec);
        e.onairTime = this.getOnAirTime(e.onairTime);
      });

      this.SET_PGM_DATA_OPTIONS(res.data.resultObject.data);
      this.RESET_PGM_SELECTED();
    },
    onContext(ctx) {
      this.formatted = ctx.selectedFormatted;
      this.dateSelected = ctx.selectedYMD;
    },
    modalOn() {
      this.modal = true;
    },
    modalOff() {
      this.modal = false;
    },
    modalReset() {
      this.RESET_PGM_SELECTED();
      this.modal = false;
    },
  },
};
</script>

<style scoped>
.disabledRow {
  color: silver !important;
}
.disabledGrayCell {
  color: silver;
}
.disabledCell {
  color: red !important;
}
.dx-row :hover {
  background-color: #f5f5f5;
}
</style>
