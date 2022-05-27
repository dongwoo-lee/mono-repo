<template>
  <div>
    <div style="margin-top: 35px; font-size: 15px">
      <b-form-group
        label="방송일"
        class="has-float-label"
        style="width: 220px; float: left; margin-right: 20px"
      >
        <b-input-group class="mb-3" style="width: 220px; float: left">
          <input
            :disabled="isActive"
            id="dateinput"
            type="text"
            class="form-control input-picker"
            :value="mcrMetaData.date"
            @input="onInput"
          />
          <b-input-group-append>
            <b-form-datepicker
              :value="mcrMetaData.date"
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
        label="매체"
        class="has-float-label"
        style="width: 95px; float: left; margin-right: 20px"
      >
        <b-form-select
          :disabled="isActive"
          id="program-media"
          class="media-select"
          style="width: 95px; height: 36px"
          :value="mcrMetaData.media"
          :options="mcrMediaOptions"
          @input="mediaChange"
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
    <div style="width: 430px; font-family: 'MBC 새로움 M'; font-size: 15px">
      <div style="width: 425px; float: left">
        <b-form-group
          label="이벤트 명"
          class="has-float-label"
          style="margin-top: -10px"
        >
          <b-form-input
            style="width: 425px"
            class="editTask"
            v-model="mcrSelected.name"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 250px; float: left">
        <b-form-group
          label="이벤트 ID"
          class="has-float-label"
          style="margin-top: 10px"
        >
          <b-form-input
            style="width: 250px"
            class="editTask"
            v-model="mcrSelected.id"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 155px; margin-left: 20px; float: left">
        <b-form-group
          label="편성 분량"
          class="has-float-label"
          style="margin-top: 10px"
        >
          <b-form-input
            style="width: 155px"
            class="editTask"
            v-model="mcrSelected.duration"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
    </div>
    <div style="margin-top: 160px; height: 50px">
      <b-form-group
        label="메모"
        class="has-float-label"
        style="font-size: 15px"
      >
        <b-form-input
          class="editTask"
          v-model="mcrMetaData.memo"
          :state="mcrMemoState"
          :maxLength="30"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="메모"
          trim
        />
      </b-form-group>
      <span
        v-show="mcrMemoState"
        style="
          position: relative;
          left: 390px;
          top: -15px;
          width: 30px;
          margin-right: 0px;
          font-size: 14px;
        "
      >
        {{ mcrMetaData.memo.length }}/30
      </span>
    </div>
    <div style="font-size: 15px; margin-top: 15px">
      <b-form-group label="광고주" class="has-float-label">
        <b-form-input
          class="editTask"
          v-model="mcrMetaData.advertiser"
          :state="mcrAdvertiserState"
          :maxLength="15"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="광고주"
          trim
        />
      </b-form-group>
      <p
        v-show="mcrAdvertiserState"
        style="
          position: relative;
          left: 390px;
          top: -15px;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ mcrMetaData.advertiser.length }}/15
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
            name="mcrDxDataGrid"
            v-show="this.mcrDataOptions.id != ''"
            style="
              height: 280px;
              border: 1px solid silver;
              font-family: 'MBC 새로움 M';
            "
            :data-source="mcrDataOptions"
            :selection="{ mode: 'single' }"
            :show-borders="false"
            :hover-state-enabled="true"
            key-expr="id"
            :allow-column-resizing="true"
            :column-auto-width="true"
            no-data-text="No Data"
            @row-click="onRowClick"
          >
            <tbody
              slot="rowTemplate"
              slot-scope="{
                data: {
                  data: { name, id, duration, audioClipID },
                },
              }"
              class="dx-row"
            >
              <tr>
                <td style="border-right: 1px solid #dddddd">{{ name }}</td>
                <td style="border-right: 1px solid #dddddd">{{ id }}</td>
                <td style="border-right: 1px solid #dddddd">{{ duration }}</td>
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
            <!-- <DxLoadPanel :enabled="true" /> -->
            <DxSelection mode="single" />
            <DxScrolling mode="virtual" />
            <DxColumn data-field="name" caption="이벤트 명" />
            <DxColumn data-field="id" caption="이벤트 ID" />
            <DxColumn data-field="duration" caption="편성 분량" />
            <DxColumn data-field="audioClipID" :width="50" caption="파일" />
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
import { DxSelection, DxScrolling } from "devextreme-vue/data-grid";
import axios from "axios";
export default {
  components: {
    CommonVueSelect,
    DxSelection,
    DxScrolling,
  },
  mixins: [CommonFileFunction],
  data() {
    return {
      modal: false,
      mcrMedia: "A",
      mediaName: "AM",
    };
  },
  async created() {
    this.RESET_MCR();
    this.RESET_MCR_MEDIA_OPTIONS();

    var res = await axios.get("/api/categories/media");

    this.SET_MCR_MEDIA(res.data.resultObject.data[0].id);
    this.mediaName = res.data.resultObject.data[0].name;

    res.data.resultObject.data.forEach((e) => {
      this.SET_MCR_MEDIA_OPTIONS({
        value: e.id,
        text: e.name,
      });
    });

    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.SET_MCR_DATE(today);
    this.SET_MCR_TEMP_DATE(today);
    this.getPro();
  },
  computed: {
    ...mapState("FileIndexStore", {
      mcrMetaData: (state) => state.mcrMetaData,
      mcrMediaOptions: (state) => state.mcrMediaOptions,
      mcrDataOptions: (state) => state.mcrDataOptions,
      mcrSelected: (state) => state.mcrSelected,
    }),
    ...mapGetters("FileIndexStore", [
      "mcrTitleState",
      "mcrMemoState",
      "mcrAdvertiserState",
    ]),
  },
  methods: {
    ...mapMutations("FileIndexStore", [
      "SET_MCR_TITLE",
      "SET_MCR_MEDIA",
      "SET_MCR_DATE",
      "SET_MCR_TEMP_DATE",
      "SET_MCR_MEDIA_OPTIONS",
      "SET_MCR_DATA_OPTIONS",
      "SET_MCR_SELECTED",
      "RESET_MCR_MEDIA_OPTIONS",
      "RESET_MCR_DATA_OPTIONS",
      "RESET_MCR_SELECTED",
      "RESET_MCR",
    ]),
    getAudioClipID(audioClipID) {
      if (audioClipID != "") {
        return true;
      } else {
        return false;
      }
    },
    mediaChange(v) {
      this.SET_MCR_MEDIA(v);
      var data = this.mcrMediaOptions.find((dt) => dt.value == v);
      this.mediaName = data.text;
    },
    onSearch() {
      this.modalOn();
      this.getPro();
    },
    onRowClick(v) {
      this.SET_MCR_SELECTED(v.data);
      this.SET_MCR_TITLE(
        `[${this.mcrMetaData.date}] [${this.mediaName}] [${this.mcrSelected.name}]`
      );
    },
    eventInput(event) {
      this.SET_MCR_DATE(event);
      this.SET_MCR_TEMP_DATE(event);
      this.getPro();
    },
    onInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.mcrMetaData.tempDate == null) {
          event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
        }
        event.target.value = this.mcrMetaData.tempDate;
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
            this.SET_MCR_DATE(this.$fn.formatDate(new Date(), "yyyy-MM-dd"));
            this.SET_MCR_TEMP_DATE(
              this.$fn.formatDate(new Date(), "yyyy-MM-dd")
            );
            return;
          }
          this.SET_MCR_DATE(convertDate);
          this.SET_MCR_TEMP_DATE(convertDate);
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
        this.SET_MCR_DATE("");
      } else if (31 < dd) {
        this.SET_MCR_DATE("");
      } else {
        return `${yyyy}-${mm}-${dd}`;
      }
    },
    onContext(ctx) {
      this.formatted = ctx.selectedFormatted;
      this.dateSelected = ctx.selectedYMD;
    },
    async getPro() {
      const replaceVal = this.mcrMetaData.date.replace(/-/g, "");
      const yyyy = replaceVal.substring(0, 4);
      const mm = replaceVal.substring(4, 6);
      const dd = replaceVal.substring(6, 8);
      var date = yyyy + "" + mm + "" + dd;
      this.RESET_MCR_DATA_OPTIONS();

      var res = await axios.get(
        `/api/categories/spot-sch?media=${this.mcrMetaData.media}&date=${date}&spotType=MS`
      );

      var value = res.data.resultObject.data;
      value.forEach((e) => {
        e.duration = this.getDurationSec(e.duration);
      });

      this.SET_MCR_DATA_OPTIONS(res.data.resultObject.data);
      this.RESET_MCR_SELECTED();
    },
    modalOn() {
      this.modal = true;
    },
    modalOff() {
      this.modal = false;
    },
    modalReset() {
      this.RESET_MCR_SELECTED();
      this.modal = false;
    },
    getData() {},
  },
};
</script>

<style scoped>
.disabledCell {
  color: red !important;
}
.dx-row :hover {
  background-color: #f5f5f5;
}
</style>
