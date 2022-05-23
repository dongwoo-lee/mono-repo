<template>
  <div>
    <div style="margin-top: 35px">
      <b-button
        style="position: absolute; top: 117px; left: 872px"
        class="btn btn-outline-primary btn-sm default cutom-label mr-2"
        @click="modalOn"
        >방송의뢰</b-button
      >
      <div>
        <DxDataGrid
          name="mcrDxDataGrid"
          style="
            border: 1px solid silver;
            font-family: 'MBC 새로움 M';
            margin-top: 20px;
            height: 175px;
          "
          :data-source="scrRange"
          :selection="{ mode: 'single' }"
          :show-borders="true"
          :hover-state-enabled="true"
          key-expr="Pgm"
          :allow-column-resizing="true"
          :column-auto-width="true"
          no-data-text="No Data"
          @row-click="onRowClick"
        >
          <DxLoadPanel :enabled="true" />
          <DxScrolling mode="virtual" />
          <DxEditing
            :allow-deleting="true"
            :confirm-delete="false"
            :use-icons="true"
            mode="row"
          />
          <DxColumn data-field="PgmName" :width="160" caption="사용처" />
          <DxColumn data-field="SDate" :width="110" caption="시작일" />
          <DxColumn data-field="EDate" :width="110" caption="종료일" />
          <DxColumn type="buttons" :width="40" caption="삭제">
            <DxButton name="delete" />
          </DxColumn>
        </DxDataGrid>
      </div>
      <div
        style="height: 50px; margin-top: 20px; float: left; margin-right: 20px"
      >
        <b-form-group
          label="분류"
          class="has-float-label"
          style="font-size: 15px"
        >
          <b-form-select
            id="program-media"
            class="editTask"
            style="width: 200px"
            :value="scrMedia"
            :options="fileMediaOptions"
            @input="mediaChange"
          />
        </b-form-group>
      </div>
      <div style="height: 50px; margin-top: 20px">
        <b-form-group
          label="소재"
          class="has-float-label"
          style="font-size: 15px"
        >
          <b-form-input
            class="editTask"
            style="width: 200px"
            v-model="MetaData.title"
            :state="titleState"
            :maxlength="30"
            aria-describedby="input-live-help input-live-feedback"
            placeholder="소재"
            trim
          />
        </b-form-group>
        <p
          v-show="titleState"
          style="
            position: relative;
            left: 390px;
            top: -15px;
            z-index: 9999;
            width: 30px;
            margin-right: 0px;
          "
        >
          {{ MetaData.title.length }}/30
        </p>
      </div>
      <div
        style="height: 50px; margin-top: 15px; float: left; margin-right: 20px"
      >
        <b-form-group
          label="광고주"
          class="has-float-label"
          style="font-size: 15px"
        >
          <b-form-input
            class="editTask"
            style="width: 200px"
            v-model="MetaData.advertiser"
            :state="advertiserState"
            :maxLength="15"
            aria-describedby="input-live-help input-live-feedback"
            placeholder="광고주"
            trim
          />
        </b-form-group>
        <p
          v-show="advertiserState"
          style="
            position: relative;
            left: 165px;
            top: -15px;
            z-index: 9999;
            width: 30px;
            margin-right: 0px;
            margin-top: 7px;
          "
        >
          {{ MetaData.advertiser.length }}/15
        </p>
      </div>
      <div style="height: 50px; margin-top: 15px">
        <b-form-group
          label="메모"
          class="has-float-label"
          style="font-size: 15px"
        >
          <b-form-input
            class="editTask"
            style="width: 200px"
            v-model="MetaData.memo"
            :state="memoState"
            :maxLength="30"
            aria-describedby="input-live-help input-live-feedback"
            placeholder="메모"
            trim
          />
        </b-form-group>
        <p
          v-show="memoState"
          style="
            position: relative;
            left: 390px;
            top: -15px;
            z-index: 9999;
            width: 30px;
            margin-right: 0px;
          "
        >
          {{ MetaData.memo.length }}/30
        </p>
      </div>
    </div>

    <b-modal
      size="md"
      v-model="modal"
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
        <div>
          <b-form-group
            label="사용처"
            class="has-float-label"
            style="font-size: 13px"
          >
            <common-vue-select
              :class="vSelectClass"
              style="font-size: 14px; width: 450px"
              :suggestions="ProgramOptions"
              @inputEvent="pgmSelect"
              :vSelectProps="vSelectProps"
            ></common-vue-select>
          </b-form-group>

          <b-form-group
            label="시작일"
            class="has-float-label"
            style="width: 200px; font-size: 13px; margin-top: 20px; float: left"
          >
            <b-input-group style="width: 205px; float: left">
              <input
                style="height: 34px; font-size: 13px"
                id="sdateinput"
                type="text"
                class="form-control input-picker date-input"
                :value="StartDate"
                @input="onsInput"
              />
              <b-input-group-append>
                <b-form-datepicker
                  style="height: 34px; float: left"
                  :value="StartDate"
                  @input="eventSInput"
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
            label="종료일"
            class="has-float-label"
            style="
              width: 200px;
              font-size: 13px;
              margin-left: 40px;
              margin-top: 20px;
              float: left;
            "
          >
            <b-input-group class="mb-3" style="width: 205px; float: left">
              <input
                style="height: 34px; font-size: 13px"
                id="edateinput"
                type="text"
                class="form-control input-picker date-input"
                :value="EndDate"
                @input="oneInput"
              />
              <b-input-group-append>
                <b-form-datepicker
                  style="height: 34px"
                  :value="EndDate"
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
        <p
          v-show="!dateValid"
          style="
            position: absolute;
            top: 135px;
            left: 270px;
            color: red;
            font-size: 13px;
          "
        >
          시작 날짜가 종료 날짜보다 큽니다.
        </p>
      </template>
      <template v-slot:modal-footer>
        <b-button
          v-show="addValid"
          style="margin-left: 20px; height: 34px"
          variant="outline-primary"
          @click="addRange"
        >
          추가
        </b-button>
        <b-button
          v-show="!addValid"
          style="margin-left: 20px; height: 34px"
          variant="dark"
          disabled
          @click="addRange"
        >
          추가
        </b-button>
        <b-button
          variant="outline-danger default cutom-label-cancel"
          size="sm"
          class="float-right"
          @click="modalOff"
        >
          닫기</b-button
        >
      </template>
    </b-modal>
  </div>
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
import {
  DxDataGrid,
  DxColumn,
  DxSelection,
  DxScrolling,
  DxLoadPanel,
  DxButton,
  DxEditing,
} from "devextreme-vue/data-grid";
import axios from "axios";
export default {
  components: {
    CommonVueSelect,
    DxDataGrid,
    DxColumn,
    DxSelection,
    DxScrolling,
    DxLoadPanel,
    DxButton,
    DxEditing,
  },
  mixins: [CommonFileFunction, MixinBasicPage],
  data() {
    return {
      modal: false,
      vSelectProps: {},
      vSelectClass: "MasteringScrRangeMeta",
      scrMedia: "",
      scrMediaName: "",
      selectedPgm: "",
      StartDate: this.$fn.formatDate(new Date(), "yyyy-MM-dd"),
      EndDate: this.$fn.formatDate(new Date(), "yyyy-MM-dd"),
      tempSDate: "",
      tempEDate: "",
      dateValid: true,
      addValid: false,
      ProgramOptions: [],
    };
  },
  created() {
    this.reset();
    this.setTitle(this.sliceExt(30));
    this.getPgm();
    this.getEditorForPd();
    this.resetFileMediaOptions();
    axios.get("/api/categories/scr/spot").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name,
        });
      });
    });
    this.scrMedia = "ST01";
    this.scrMediaName = "우리의 소리를 찾아서";
    this.setMediaSelected(this.scrMedia);
  },
  computed: {
    ...mapState("FileIndexStore", {
      scrRange: (state) => state.scrRange,
    }),
  },
  watch: {
    StartDate() {
      this.validRange();
    },
    EndDate() {
      this.validRange();
    },
    selectedPgm() {
      this.validRange();
    },
  },
  methods: {
    ...mapMutations("FileIndexStore", ["setScrRange", "resetScrRange"]),
    validRange() {
      if (
        this.selectedPgm == "" ||
        this.selectedPgm == "undefined" ||
        this.selectedPgm == null ||
        this.selectedPgm == { id: null, name: null }
      ) {
        this.addValid = false;
      } else {
        if (!this.dateValid) {
          this.addValid = false;
          return;
        }
        this.addValid = true;
      }
    },
    onSearch() {
      this.modalOn();
    },
    modalOn() {
      this.modal = true;
    },
    modalOff() {
      this.modal = false;
    },
    addRange() {
      var data = {
        Pgm: this.selectedPgm.id,
        PgmName: this.selectedPgm.name,
        SDate: this.StartDate,
        EDate: this.EndDate,
      };
      this.setScrRange(data);
      this.selectedPgm = "";
      this.vSelectProps = { id: null, name: null };
    },
    async getPgm() {
      var res = await axios.get(`/api/categories/pgmcodes`);
      this.ProgramOptions = res.data.resultObject.data;
    },
    mediaChange(v) {
      this.setMediaSelected(v);
    },
    pgmSelect(v) {
      this.selectedPgm = v;
      this.validRange();
    },
    eventSInput(value) {
      this.StartDate = value;
      this.tempSDate = value;

      const replaceAllFileSDate = this.StartDate.replace(/-/g, "");
      const replaceAllFileEDate = this.EndDate.replace(/-/g, "");
      if (
        replaceAllFileEDate < replaceAllFileSDate &&
        replaceAllFileEDate != ""
      ) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        this.dateValid = false;
        return;
      }
      this.dateValid = true;
    },
    eventEInput(value) {
      this.EndDate = value;
      this.tempEDate = value;

      const replaceAllFileSDate = this.StartDate.replace(/-/g, "");
      const replaceAllFileEDate = this.EndDate.replace(/-/g, "");
      if (replaceAllFileEDate < replaceAllFileSDate) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        this.dateValid = false;
        return;
      }
      this.dateValid = true;
    },

    onsInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.tempSDate == null) {
          event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
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
            event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
            this.StartDate = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
            this.tempSDate = this.$fn.formatDate(new Date(), "yyyy-MM-dd");

            const replaceAllFileSDate = this.StartDate.replace(/-/g, "");
            const replaceAllFileEDate = this.EndDate.replace(/-/g, "");
            if (
              replaceAllFileEDate < replaceAllFileSDate &&
              replaceAllFileEDate != ""
            ) {
              this.$fn.notify("error", {
                message: "시작 날짜가 종료 날짜보다 큽니다.",
              });
              this.dateValid = false;
              return;
            }
            return;
          }
          this.StartDate = convertDate;
          this.tempSDate = convertDate;
          const replaceAllFileSDate = this.StartDate.replace(/-/g, "");
          const replaceAllFileEDate = this.EndDate.replace(/-/g, "");
          if (
            replaceAllFileEDate < replaceAllFileSDate &&
            replaceAllFileEDate != ""
          ) {
            this.$fn.notify("error", {
              message: "시작 날짜가 종료 날짜보다 큽니다.",
            });
            this.dateValid = false;
            return;
          }
          this.dateValid = true;
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
            this.EndDate = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
            this.tempEDate = this.$fn.formatDate(new Date(), "yyyy-MM-dd");

            const replaceAllFileSDate = this.StartDate.replace(/-/g, "");
            const replaceAllFileEDate = this.EndDate.replace(/-/g, "");
            if (replaceAllFileEDate < replaceAllFileSDate) {
              this.$fn.notify("error", {
                message: "시작 날짜가 종료 날짜보다 큽니다.",
              });
              this.dateValid = false;
              return;
            }
            return;
          }
          this.EndDate = convertDate;
          this.tempEDate = convertDate;

          const replaceAllFileSDate = this.StartDate.replace(/-/g, "");
          const replaceAllFileEDate = this.EndDate.replace(/-/g, "");
          if (replaceAllFileEDate < replaceAllFileSDate) {
            this.$fn.notify("error", {
              message: "시작 날짜가 종료 날짜보다 큽니다.",
            });
            this.dateValid = false;
            return;
          }
          this.dateValid = true;
        }
      }
    },
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
      if (!(12 < mm && 31 < dd)) {
        return `${yyyy}-${mm}-${dd}`;
      }
    },
  },
};
</script>

<style></style>
