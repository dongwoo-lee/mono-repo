<template>
  <b-modal
    id="setSpot"
    v-model="Search"
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
            @keyup.enter="getScrSpot"
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
            @keyup.enter="getScrSpot"
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
        <DxScrolling mode="virtual" />
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
        @click="setSpot"
      >
        선택</b-button
      >
      <b-button
        variant="outline-danger default cutom-label-cancel"
        size="sm"
        class="float-right"
        @click="cancel"
      >
        취소</b-button
      >
    </template>
  </b-modal>
</template>

<script>
import {
  DxDataGrid,
  DxScrolling,
  DxLoadPanel,
  DxColumn,
  DxSelection,
} from "devextreme-vue/data-grid";
import axios from "axios";
import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
export default {
  components: {
    DxScrolling,
    DxLoadPanel,
    DxDataGrid,
    DxColumn,
    DxSelection,
  },
  data() {
    return {
      scrMediaOptions: [], // 분류 옵션 - created 에서 데이터 push
      // 검색 파라미터
      searchSpot: {
        title: "",
        advertiser: "",
        media: "",
        sDate: "",
        eDate: this.$fn.formatDate(new Date(), "yyyy-MM-dd"),
      },
      rowSelect: {
        spotID: "",
        spotName: "",
        codeID: "",
        codeName: "",
        CMOwner: "",
      },
      //부조SPOT검색 모달 그리드 데이터
      spotData: [
        {
          spotID: "",
          spotName: "",
          codeID: "",
          codeName: "",
          CMOwner: "",
        },
      ],
      tempSDate2: "",
      tempEDate2: "",
    };
  },
  created() {
    var newDate = new Date();
    var dayOfMonth = newDate.getDate();
    newDate.setDate(dayOfMonth - 14);
    newDate = this.$fn.formatDate(newDate, "yyyy-MM-dd");

    this.searchSpot.sDate = newDate;

    axios.get("/api/categories/scr/spot").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this.scrMediaOptions.push({ value: e.id, text: e.name });
      });
    });
    this.searchSpot.media = "ST01";
    this.getScrSpot();
  },
  computed: {
    ...mapState("ScrSpotDuration", {
      Search: (state) => state.Search,
      selectedSpot: (state) => state.selectedSpot,
    }),
    selectRowValid() {
      if (this.rowSelect.spotID != "") {
        return true;
      }
      return false;
    },
  },
  methods: {
    ...mapMutations("ScrSpotDuration", ["hideSearch", "setSelectedSpot"]),
    cancel() {
      this.hideSearch();
      this.searchSpot = {
        title: "",
        advertiser: "",
        media: "",
        sDate: "",
        eDate: this.$fn.formatDate(new Date(), "yyyy-MM-dd"),
      };

      var newDate = new Date();
      var dayOfMonth = newDate.getDate();
      newDate.setDate(dayOfMonth - 14);
      newDate = this.$fn.formatDate(newDate, "yyyy-MM-dd");

      this.searchSpot.sDate = newDate;

      this.searchSpot.media = "ST01";

      this.getScrSpot();
    },
    setSpot() {
      this.setSelectedSpot(this.rowSelect);
      this.hideSearch();
    },
    mediaChange(v) {
      this.searchSpot.media = v;
      this.getScrSpot();
    },
    spotSelect(v) {
      this.rowSelect = v.data;
    },
    resetSpotData() {
      this.spotData = [
        {
          spotID: "",
          spotName: "",
          codeID: "",
          codeName: "",
          CMOwner: "",
        },
      ];
    },
    async getScrSpot() {
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
      } else {
        this.resetSpotData();
        await axios
          .get(
            `/api/categories/scr-spot?spotName=${
              this.searchSpot.title
            }&codeId=${this.searchSpot.media}&cmOwner=${
              this.searchSpot.advertiser
            }
          &startDate=${this.searchSpot.sDate.replace(
            /-/g,
            ""
          )}&endDate=${this.searchSpot.eDate.replace(/-/g, "")}`
          )
          .then((res) => {
            this.spotData = res.data.resultObject.data;
          });
      }
    },
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
      } else {
        this.getScrSpot();
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
      } else {
        this.getScrSpot();
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
          } else {
            this.getScrSpot();
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
          } else {
            this.getScrSpot();
          }
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

<style></style>
