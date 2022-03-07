<template>
  <div>
    <div style="font-size: 16px; margin-top: 20px">
      <b-button
        style="position: absolute; top: 117px; left: 872px"
        class="btn btn-outline-primary btn-sm default cutom-label mr-2"
        @click="modalOn"
        >방송의뢰</b-button
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
              v-model="EventSelected.name"
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
            style="margin-top: 20px"
          >
            <b-form-input
              style="width: 425px"
              class="editTask"
              v-model="fileSDate"
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
            style="margin-top: 20px"
          >
            <b-form-input
              style="width: 425px"
              class="editTask"
              v-model="EventSelected.duration"
              disabled
              aria-describedby="input-live-help input-live-feedback"
              trim
            />
          </b-form-group>
        </div>
      </div>
      <div style="height: 50px">
        <b-form-group
          label="메모"
          class="has-float-label"
          style="float: left; margin-top: 20px"
        >
          <b-form-input
            class="editTask"
            v-model="MetaData.memo"
            :state="memoState"
            :maxLength="30"
            aria-describedby="input-live-help input-live-feedback"
            placeholder="메모"
            trim
        /></b-form-group>
        <p
          v-show="memoState"
          style="
            position: relative;
            left: 390px;
            width: 30px;
            margin-right: 0px;
          "
        >
          {{ MetaData.memo.length }}/30
        </p>
      </div>
    </div>
    <div style="height: 50px">
      <b-form-group
        label="광고주"
        class="has-float-label"
        style="float: left; margin-top: 20px; font-size: 15px"
      >
        <b-form-input
          class="editTask"
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
        style="position: relative; left: 390px; width: 30px; margin-right: 0px"
      >
        {{ MetaData.advertiser.length }}/15
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
        <h5>부조SPOT 방송의뢰</h5>
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

        <DxDataGrid
          name="mcrDxDataGrid"
          v-show="this.EventData.id != ''"
          style="
            margin-top: 30px;
            height: 295px;
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
          <DxLoadPanel :enabled="true" />
          <DxScrolling mode="virtual" />
          <DxColumn data-field="name" caption="이벤트 명" />
          <DxColumn data-field="id" caption="이벤트 ID" />
          <DxColumn data-field="duration" caption="편성분량" />
        </DxDataGrid>
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

  <!-- <div>
   
   
   
    
    프로그램
    
  </div> -->
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import mixinFillerPage from "../../../mixin/MixinFillerPage";
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
  mixins: [CommonFileFunction, MixinBasicPage, mixinFillerPage],
  data() {
    return {
      modal: false,
      mediaName: "AM",
      staticMedia: "A",
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
    this.sdate = today;
    this.setFileSDate(today);
    this.setTempFileSDate(today);

    // var newDate = new Date();
    // var dayOfMonth = newDate.getDate();
    // newDate.setDate(dayOfMonth + 7);
    // newDate = this.$fn.formatDate(newDate, "yyyy-MM-dd");

    this.edate = today;
    this.setFileEDate(today);
    this.setTempFileEDate(today);

    this.getPro();
  },
  methods: {
    onSearch() {
      this.modalOn();
      this.getPro();
    },
    modalOn() {
      this.modal = true;
    },
    modalOff() {
      this.modal = false;
    },
    modalReset() {
      this.resetEventSelected();
      this.modal = false;
    },
    mediaChange(v) {
      this.setMediaSelected(v);
      var data = this.fileMediaOptions.find((dt) => dt.value == v);
      this.mediaName = data.text;
      this.getPro();
    },
    eventSInput(value) {
      this.sdate = value;
      this.setFileSDate(value);
      this.setTempFileSDate(value);

      const replaceAllFileSDate = this.sdate.replace(/-/g, "");
      const replaceAllFileEDate = this.edate.replace(/-/g, "");
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
      this.edate = value;
      this.setFileEDate(value);
      this.setTempFileEDate(value);

      const replaceAllFileSDate = this.sdate.replace(/-/g, "");
      const replaceAllFileEDate = this.edate.replace(/-/g, "");
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
        if (this.tempFileSDate == null) {
          event.target.value = this.get7daysago();
          return;
        }
        event.target.value = this.tempFileSDate;
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
            this.sdate = this.get7daysago();
            this.setFileSDate(this.get7daysago());
            this.setTempFileSDate(this.get7daysago());

            const replaceAllFileSDate = this.sdate.replace(/-/g, "");
            const replaceAllFileEDate = this.edate.replace(/-/g, "");
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
          this.sdate = convertDate;
          this.setFileSDate(convertDate);
          this.setTempFileSDate(convertDate);

          const replaceAllFileSDate = this.sdate.replace(/-/g, "");
          const replaceAllFileEDate = this.edate.replace(/-/g, "");
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
        if (this.tempFileEDate == null) {
          event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
        }
        event.target.value = this.tempFileEDate;
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
            this.edate = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
            this.setFileEDate(this.$fn.formatDate(new Date(), "yyyy-MM-dd"));
            this.setTempFileEDate(
              this.$fn.formatDate(new Date(), "yyyy-MM-dd")
            );

            const replaceAllFileSDate = this.sdate.replace(/-/g, "");
            const replaceAllFileEDate = this.edate.replace(/-/g, "");
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
          this.edate = convertDate;
          this.setFileEDate(convertDate);
          this.setTempFileEDate(convertDate);
          const replaceAllFileSDate = this.sdate.replace(/-/g, "");
          const replaceAllFileEDate = this.edate.replace(/-/g, "");
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

<style>
.date-input:focus {
  border: 1px solid #4475c4 !important;
}
.date-input {
  border: 1px solid silver !important;
}
</style>
