<template>
  <div>
    <div style="font-size: 16px">
      <b-button
        style="position: absolute; top: 117px; left: 900px"
        class="btn btn-outline-primary btn-sm default cutom-label mr-2"
        @click="modalOn"
        >검색</b-button
      >

      <div v-show="!isActive" style="font-family: 'MBC 새로움 M'">
        <div style="width: 425px; float: left">
          <b-form-group
            label="이벤트 명"
            class="has-float-label"
            style="margin-top: 20px"
          >
            <b-form-input
              style="width: 425px"
              class="editTask"
              v-model="varSelected.name"
              disabled
              aria-describedby="input-live-help input-live-feedback"
              trim
            />
          </b-form-group>
        </div>
        <div style="width: 425px; float: left">
          <b-form-group
            label="방송 시작일"
            class="has-float-label"
            style="margin-top: 5px"
          >
            <b-form-input
              style="width: 425px"
              class="editTask"
              v-model="varMetaData.sDate"
              disabled
              aria-describedby="input-live-help input-live-feedback"
              trim
            />
          </b-form-group>
        </div>
        <div style="width: 425px; float: left">
          <b-form-group
            label="편성 분량"
            class="has-float-label"
            style="margin-top: 5px"
          >
            <b-form-input
              style="width: 425px"
              class="editTask"
              v-model="varSelected.duration"
              disabled
              aria-describedby="input-live-help input-live-feedback"
              trim
            />
          </b-form-group>
        </div>
      </div>
    </div>
    <div style="height: 50px">
      <b-form-group
        label="메모"
        class="has-float-label"
        style="float: left; margin-top: 10px; font-size: 15px"
      >
        <b-form-input
          class="editTask"
          v-model="varMetaData.memo"
          :maxLength="30"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="메모"
          trim
      /></b-form-group>
      <p
        v-show="varMemoState"
        style="
          position: relative;
          left: 390px;
          top: -15px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ varMetaData.memo.length }}/30
      </p>
    </div>
    <div style="height: 50px">
      <b-form-group
        label="광고주"
        class="has-float-label"
        style="float: left; margin-top: 5px; font-size: 15px"
      >
        <b-form-input
          class="editTask"
          v-model="varMetaData.advertiser"
          :maxLength="15"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="광고주"
          trim
        />
      </b-form-group>
      <p
        v-show="varAdvertiserState"
        style="
          position: relative;
          left: 390px;
          top: -15px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ varMetaData.advertiser.length }}/15
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
            label="시작일"
            class="has-float-label"
            style="width: 250px; float: left; margin-right: 20px"
          >
            <b-input-group class="mb-3" style="width: 250px; float: left">
              <input
                style="height: 33px; font-size: 13px"
                id="sdateinput"
                type="text"
                class="form-control input-picker"
                :value="varMetaData.sDate"
                @input="onsInput"
              />
              <b-input-group-append>
                <b-form-datepicker
                  style="height: 33px"
                  :value="varMetaData.sDate"
                  @input="eventSInput"
                  button-only
                  button-variant="outline-dark"
                  left
                  aria-controls="example-input"
                  @context="onContext"
                ></b-form-datepicker>
              </b-input-group-append>
            </b-input-group>
          </b-form-group>
          <b-form-group
            label="종료일"
            class="has-float-label"
            style="width: 250px; float: left; margin-right: 20px"
          >
            <b-input-group class="mb-3" style="width: 250px; float: left">
              <input
                style="height: 33px; font-size: 13px"
                id="edateinput"
                type="text"
                class="form-control input-picker"
                :value="varMetaData.eDate"
                @input="oneInput"
              />
              <b-input-group-append>
                <b-form-datepicker
                  style="height: 33px"
                  :value="varMetaData.eDate"
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
        <div>
          <b-form-group
            label="매체"
            class="has-float-label"
            style="float: left; margin-right: 20px"
          >
            <b-form-select
              id="program-media"
              class="media-select"
              style="width: 115px; height: 33px"
              :value="varMetaData.media"
              :options="varMediaOptions"
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

        <div style="margin-top: 40px">
          <DxDataGrid
            name="mcrDxDataGrid"
            style="
              height: 295px;
              border: 1px solid silver;
              font-family: 'MBC 새로움 M';
            "
            :data-source="varDataOptions"
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
            <DxColumn data-field="name" caption="이벤트 명" />
            <DxColumn data-field="id" caption="이벤트 ID" />
            <DxColumn data-field="duration" caption="편성분량" />
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
      mediaName: "AM",
    };
  },
  async created() {
    this.RESET_VAR();
    this.RESET_VAR_MEDIA_OPTIONS(); //매체 초기화
    //매체 생성
    var res = await axios.get("/api/categories/media");

    this.SET_VAR_MEDIA(res.data.resultObject.data[0].id);

    res.data.resultObject.data.forEach((e) => {
      this.SET_VAR_MEDIA_OPTIONS({
        value: e.id,
        text: e.name,
      });
    });
  },
  computed: {
    ...mapState("FileIndexStore", {
      varMetaData: (state) => state.varMetaData,
      varMediaOptions: (state) => state.varMediaOptions,
      varDataOptions: (state) => state.varDataOptions,
      varSelected: (state) => state.varSelected,
    }),
    ...mapGetters("FileIndexStore", ["varMemoState", "varAdvertiserState"]),
  },
  methods: {
    ...mapMutations("FileIndexStore", [
      "SET_VAR_TITLE",
      "SET_VAR_MEDIA",
      "SET_VAR_S_DATE",
      "SET_VAR_S_TEMP_DATE",
      "SET_VAR_E_DATE",
      "SET_VAR_E_TEMP_DATE",
      "SET_VAR_MEDIA_OPTIONS",
      "SET_VAR_DATA_OPTIONS",
      "SET_VAR_SELECTED",
      "RESET_VAR_MEDIA_OPTIONS",
      "RESET_VAR_DATA_OPTIONS",
      "RESET_VAR_SELECTED",
      "RESET_VAR",
    ]),
    modalOn() {
      this.modal = true;

      this.getPro();
    },
    modalOff() {
      if (this.varSelected.id == "") {
        this.SET_VAR_S_DATE("");
        this.SET_VAR_S_TEMP_DATE("");
      }
      this.modal = false;
    },
    modalReset() {
      this.RESET_VAR_SELECTED();

      this.SET_VAR_S_DATE("");
      this.SET_VAR_S_TEMP_DATE("");

      this.SET_VAR_E_DATE("");
      this.SET_VAR_E_TEMP_DATE("");
      this.modal = false;
    },
    mediaChange(v) {
      this.SET_VAR_MEDIA(v);
      var data = this.fileMediaOptions.find((dt) => dt.value == v);
      if (data) {
        this.mediaName = data.text;
      }
      this.getPro();
    },
    onRowClick(v) {
      this.SET_VAR_SELECTED(v.data);
      this.SET_VAR_TITLE(
        `[${this.varMetaData.sDate} ~ ${this.varMetaData.eDate}] [${this.mediaName}] [${this.varSelected.name}]`
      );
    },
    async getPro() {
      if (this.varMetaData.sDate == "") {
        setTimeout(() => {
          const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
          this.SET_VAR_S_DATE(today);
          this.SET_VAR_S_TEMP_DATE(today);

          this.SET_VAR_E_DATE(today);
          this.SET_VAR_E_TEMP_DATE(today);
        }, 200);
      }
      const replaceVal = this.varMetaData.sDate.replace(/-/g, "");
      const yyyy = replaceVal.substring(0, 4);
      const mm = replaceVal.substring(4, 6);
      const dd = replaceVal.substring(6, 8);
      var date = yyyy + "" + mm + "" + dd;
      this.RESET_VAR_DATA_OPTIONS();

      var res = await axios.get(
        `/api/categories/spot-sch?media=${this.varMetaData.media}&date=${date}&spotType=TS`
      );

      var value = res.data.resultObject.data;
      value.forEach((e) => {
        e.duration = this.getDurationSec(e.duration);
        e.startDate = this.getStartDate(e.startDate);
      });

      this.SET_VAR_DATA_OPTIONS(res.data.resultObject.data);
      this.RESET_VAR_SELECTED();
    },
    eventSInput(value) {
      this.SET_VAR_S_DATE(value);
      this.SET_VAR_S_TEMP_DATE(value);

      const replaceAllFileSDate = this.varMetaData.sDate.replace(/-/g, "");
      const replaceAllFileEDate = this.varMetaData.eDate.replace(/-/g, "");
      if (
        replaceAllFileEDate < replaceAllFileSDate &&
        replaceAllFileEDate != ""
      ) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        return;
      } else {
        this.getPro();
      }
    },
    eventEInput(value) {
      this.SET_VAR_E_DATE(value);
      this.SET_VAR_E_TEMP_DATE(value);

      const replaceAllFileSDate = this.varMetaData.sDate.replace(/-/g, "");
      const replaceAllFileEDate = this.varMetaData.eDate.replace(/-/g, "");
      if (replaceAllFileEDate < replaceAllFileSDate) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        return;
      } else {
        this.getPro();
      }
    },
    get7daysago() {
      var newDate = new Date();
      var dayOfMonth = newDate.getDate();
      newDate.setDate(dayOfMonth - 7);
      newDate = this.$fn.formatDate(newDate, "yyyy-MM-dd");
      return newDate;
    },
    onsInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.varMetaData.sTempDate == null) {
          event.target.value = this.get7daysago();
          return;
        }
        event.target.value = this.varMetaData.sTempDate;
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
            this.SET_VAR_S_DATE(this.get7daysago());
            this.SET_VAR_S_TEMP_DATE(this.get7daysago());

            const replaceAllFileSDate = this.varMetaData.sDate.replace(
              /-/g,
              ""
            );
            const replaceAllFileEDate = this.varMetaData.eDate.replace(
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
            } else {
              this.getPro();
            }
            return;
          }
          this.SET_VAR_S_DATE(convertDate);
          this.SET_VAR_S_TEMP_DATE(convertDate);

          const replaceAllFileSDate = this.varMetaData.sDate.replace(/-/g, "");
          const replaceAllFileEDate = this.varMetaData.eDate.replace(/-/g, "");
          if (
            replaceAllFileEDate < replaceAllFileSDate &&
            replaceAllFileEDate != ""
          ) {
            this.$fn.notify("error", {
              message: "시작 날짜가 종료 날짜보다 큽니다.",
            });
            return;
          } else {
            this.getPro();
          }
        }
      }
    },
    oneInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.varMetaData.eTempDate == null) {
          event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
        }
        event.target.value = this.varMetaData.eTempDate;
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
            this.SET_VAR_E_DATE(this.$fn.formatDate(new Date(), "yyyy-MM-dd"));
            this.SET_VAR_E_TEMP_DATE(
              this.$fn.formatDate(new Date(), "yyyy-MM-dd")
            );

            const replaceAllFileSDate = this.varMetaData.sDate.replace(
              /-/g,
              ""
            );
            const replaceAllFileEDate = this.varMetaData.eDate.replace(
              /-/g,
              ""
            );
            if (replaceAllFileEDate < replaceAllFileSDate) {
              this.$fn.notify("error", {
                message: "시작 날짜가 종료 날짜보다 큽니다.",
              });
              return;
            } else {
              this.getPro();
            }
            return;
          }
          this.SET_VAR_E_DATE(convertDate);
          this.SET_VAR_E_TEMP_DATE(convertDate);
          const replaceAllFileSDate = this.varMetaData.sDate.replace(/-/g, "");
          const replaceAllFileEDate = this.varMetaData.eDate.replace(/-/g, "");
          if (replaceAllFileEDate < replaceAllFileSDate) {
            this.$fn.notify("error", {
              message: "시작 날짜가 종료 날짜보다 큽니다.",
            });
            return;
          } else {
            this.getPro();
          }
        }
      }
    },
    getData() {},
  },
};
</script>

<style></style>
