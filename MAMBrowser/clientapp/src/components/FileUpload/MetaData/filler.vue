<template>
  <div>
    <div style="margin-top: 37px; font-size: 15px">
      <b-form-group label="방송일" class="has-float-label">
        <b-input-group class="mb-3" style="width: 425px; float: left">
          <input
            :disabled="isActive"
            id="dateinput"
            type="text"
            class="form-control input-picker date-input"
            :value="fillerMetaData.date"
            @input="onInput"
          />
          <b-input-group-append>
            <b-form-datepicker
              :value="fillerMetaData.date"
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
        label="1차 분류"
        class="has-float-label"
        style="margin-top: 13px"
      >
        <b-form-select
          :disabled="isActive"
          id="program-media"
          class="media-select"
          style="width: 425px; height: 37px"
          :value="this.firstCategory"
          :options="firstCategoryOptions"
          @input="getFillerCategory"
        />
      </b-form-group>
      <b-form-group
        label="2차 분류"
        class="has-float-label"
        style="margin-top: 32px"
      >
        <b-form-select
          id="program-media"
          class="media-select"
          style="width: 425px; height: 37px"
          :value="fillerMetaData.category"
          :options="fillerCategoryOptions"
          @input="fillerCategoryChanged"
        />
      </b-form-group>
    </div>
    <div style="font-size: 16px; margin-top: 20px; height: 50px">
      <b-form-group label="소재명" class="has-float-label">
        <b-form-input
          class="editTask"
          v-model="fillerMetaData.title"
          :state="fillerTitleState"
          :maxLength="30"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="소재명"
          trim
        />
      </b-form-group>
      <p
        v-show="fillerTitleState"
        style="
          position: relative;
          left: 390px;
          top: -15px;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ fillerMetaData.title.length }}/30
      </p>
    </div>

    <div style="font-size: 16px; margin-top: 15px; height: 50px">
      <b-form-group label="메모" class="has-float-label">
        <b-form-input
          style="width: 425px"
          class="editTask"
          v-model="fillerMetaData.memo"
          :maxLength="30"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="메모"
          trim
        />
      </b-form-group>
      <p
        v-show="fillerMemoState"
        style="
          position: relative;
          left: 390px;
          top: -15px;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ fillerMetaData.memo.length }}/30
      </p>
    </div>
  </div>
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
import axios from "axios";
export default {
  components: {
    CommonVueSelect,
  },
  mixins: [CommonFileFunction],
  data() {
    return {
      firstCategoryOptions: [
        { value: "pr", text: "Filler(PR)" },
        { value: "general", text: "Filler(소재)" },
        { value: "etc", text: "Filler(기타)" },
      ],
      firstCategory: "pr",
    };
  },
  async created() {
    this.RESET_FILLER();
    this.SET_FILLER_TITLE(this.sliceExt(30));
    this.RESET_FILLER_CATEGORY_OPTIONS();

    var res = await axios.get("/api/categories/filler/pro");

    this.SET_FILLER_CATEGORY(res.data.resultObject.data[0].id);

    res.data.resultObject.data.forEach((e) => {
      this.SET_FILLER_CATEGORY_OPTIONS({
        value: e.id,
        text: e.name,
      });
    });

    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.SET_FILLER_DATE(today);
    this.SET_FILLER_TEMP_DATE(today);
  },
  computed: {
    ...mapState("FileIndexStore", {
      fillerMetaData: (state) => state.fillerMetaData,
      fillerCategoryOptions: (state) => state.fillerCategoryOptions,
    }),
    ...mapGetters("FileIndexStore", ["fillerTitleState", "fillerMemoState"]),
  },
  methods: {
    ...mapMutations("FileIndexStore", [
      "SET_FILLER_TITLE",
      "SET_FILLER_DATE",
      "SET_FILLER_TEMP_DATE",
      "SET_FILLER_CATEGORY",
      "SET_FILLER_CATEGORY_OPTIONS",
      "RESET_FILLER_DATE",
      "RESET_FILLER_TEMP_DATE",
      "RESET_FILLER_CATEGORY_OPTIONS",
      "RESET_FILLER",
    ]),
    fillerCategoryChanged(v) {
      this.SET_FILLER_CATEGORY(v);
    },
    async getFillerCategory(v) {
      this.RESET_FILLER_CATEGORY_OPTIONS();
      if (this.findValue(0, v)) {
        var res = await axios.get("/api/categories/filler/pro");

        this.SET_FILLER_CATEGORY(res.data.resultObject.data[0].id);

        res.data.resultObject.data.forEach((e) => {
          this.SET_FILLER_CATEGORY_OPTIONS({
            value: e.id,
            text: e.name,
          });
        });
      } else if (this.findValue(1, v)) {
        var res = await axios.get("/api/categories/filler/general");

        this.SET_FILLER_CATEGORY(res.data.resultObject.data[0].id);

        res.data.resultObject.data.forEach((e) => {
          this.SET_FILLER_CATEGORY_OPTIONS({
            value: e.id,
            text: e.name,
          });
        });
      } else if (this.findValue(2, v)) {
        var res = await axios.get("/api/categories/filler/etc");

        this.SET_FILLER_CATEGORY(res.data.resultObject.data[0].id);

        res.data.resultObject.data.forEach((e) => {
          this.SET_FILLER_CATEGORY_OPTIONS({
            value: e.id,
            text: e.name,
          });
        });
      }
    },
    findValue(num, v) {
      if (this.firstCategoryOptions[num].value == v) {
        return true;
      } else {
        return false;
      }
    },
    eventInput(event) {
      this.SET_FILLER_DATE(event);
      this.SET_FILLER_TEMP_DATE(event);
      this.getPro();
    },
    onInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");

      if (this.validDateType(targetValue)) {
        if (this.fillerMetaData.tempDate == null) {
          event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
        }
        event.target.value = this.fillerMetaData.tempDate;
        return;
      }

      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 0) {
          event.target.value = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
          this.SET_FILLER_DATE = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
          this.SET_FILLER_TEMP_DATE = this.$fn.formatDate(
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
            this.SET_FILLER_DATE(this.$fn.formatDate(new Date(), "yyyy-MM-dd"));
            this.SET_FILLER_TEMP_DATE(
              this.$fn.formatDate(new Date(), "yyyy-MM-dd")
            );
            return;
          }
          this.SET_FILLER_DATE(convertDate);
          this.SET_FILLER_TEMP_DATE(convertDate);
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
        this.SET_FILLER_DATE("");
      } else if (31 < dd) {
        this.SET_FILLER_DATE("");
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
  },
};
</script>

<style></style>
