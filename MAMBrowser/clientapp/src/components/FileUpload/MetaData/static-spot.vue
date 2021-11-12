<template>
  <div>
    <transition name="fade">
      <div>
        <b-form-group
          label="제작자"
          class="has-float-label"
          style="position:absolute; top:380px; left:-400px; z-index:9999; font-size:16px;"
        >
          <common-vue-select
            style="font-size:14px; width:200px; border: 1px solid #008ecc;"
            :suggestions="editorOptions"
            @inputEvent="inputEditor"
          ></common-vue-select>
        </b-form-group>
      </div>
    </transition>
    <div style="position:absolute; top:40px;">
      <div>
        <b-form-group
          label="시작일"
          class="has-float-label"
          style="width:200px; float:left; margin-right:20px;"
        >
          <b-input-group class="mb-3" style="width:200px; float:left;">
            <input
              style="height:33px; font-size:13px;"
              id="dateinput"
              type="text"
              class="form-control input-picker date-input"
              :value="fileSDate"
              @input="onsInput"
            />
            <b-input-group-append>
              <b-form-datepicker
                style="height:33px;"
                :value="fileSDate"
                @input="eventSInput"
                button-only
                button-variant="outline-primary"
                right
                aria-controls="example-input"
                @context="onContext"
              ></b-form-datepicker>
            </b-input-group-append>
          </b-input-group>
        </b-form-group>
        <b-form-group
          label="종료일"
          class="has-float-label"
          style="width:200px;"
        >
          <b-input-group class="mb-3" style="width:200px; float:left;">
            <input
              style="height:33px; font-size:13px;"
              id="dateinput"
              type="text"
              class="form-control input-picker date-input"
              :value="fileEDate"
              @input="oneInput"
            />
            <b-input-group-append>
              <b-form-datepicker
                style="height:33px;"
                :value="fileEDate"
                @input="eventEInput"
                button-only
                button-variant="outline-primary"
                right
                aria-controls="example-input"
                @context="onContext"
              ></b-form-datepicker>
            </b-input-group-append>
          </b-input-group>
        </b-form-group>
      </div>
    </div>
  </div>
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
import axios from "axios";
export default {
  components: {
    CommonVueSelect
  },
  mixins: [CommonFileFunction, MixinBasicPage],
  data() {
    return {
      staticMedia: "",
      sdate: "",
      edate: ""
    };
  },
  created() {
    this.reset();
    this.getEditorForPd();
    this.resetFileMediaOptions();
    axios.get("/api/categories/media").then(res => {
      res.data.resultObject.data.forEach(e => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name
        });
      });
    });
    this.staticMedia = "A";
    this.setMediaSelected(this.staticaMedia);

    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");

    console.log(today);
    this.edate = today;
    this.setFileEDate(today);

    var newDate = new Date();
    var dayOfMonth = newDate.getDate();
    newDate.setDate(dayOfMonth - 14);
    newDate = this.$fn.formatDate(newDate, "yyyy-MM-dd");

    this.sdate = newDate;
    this.setFileSDate(newDate);
  },
  watch: {
    sdate() {
      console.log(this.sdate);
      const replaceAllFileSDate = this.sdate.replace(/-/g, "");
      const replaceAllFileEDate = this.edate.replace(/-/g, "");
      if (
        replaceAllFileEDate < replaceAllFileSDate &&
        replaceAllFileEDate != ""
      ) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다."
        });
      }
    },
    edate() {
      console.log(this.edate);
      const replaceAllFileSDate = this.sdate.replace(/-/g, "");
      const replaceAllFileEDate = this.edate.replace(/-/g, "");
      if (replaceAllFileEDate < replaceAllFileSDate) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다."
        });
      }
    }
  },
  methods: {
    ...mapMutations("FileIndexStore", ["setEditor"]),
    inputEditor(v) {
      this.setEditor(v.id);
    },
    changeMedia(v) {
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
      if (event.target.value != null) {
        const targetValue = event.target.value;
        if (targetValue == "") {
          this.sdate = "";
          this.resetFileSDate();
          return;
        }
        const replaceAllTargetValue = targetValue.replace(/-/g, "");
        if (!isNaN(replaceAllTargetValue)) {
          if (replaceAllTargetValue.length === 8) {
            const convertDate = this.convertDateSTH(replaceAllTargetValue);
            this.sdate = convertDate;
            this.setFileSDate(convertDate);
          }
        } else if (targetValue == "-") {
          const replaceAllTargetValue = targetValue.replace(/-/g, "");
          if (replaceAllTargetValue.length === 8) {
            const convertDate = this.convertDateSTH(replaceAllTargetValue);
            this.sdate = convertDate;
            this.setFileSDate(convertDate);
          }
        } else {
          this.sdate = "";
          this.resetFileSDate();
          this.$fn.notify("error", { message: "숫자만 입력 가능 합니다." });
        }
      }
    },
    oneInput(event) {
      if (event.target.value != null) {
        const targetValue = event.target.value;
        if (targetValue == "") {
          this.edate = "";
          this.resetFileEDate();
          return;
        }
        const replaceAllTargetValue = targetValue.replace(/-/g, "");
        if (!isNaN(replaceAllTargetValue)) {
          if (replaceAllTargetValue.length === 8) {
            const convertDate = this.convertDateSTH(replaceAllTargetValue);
            this.edate = convertDate;
            this.setFileEDate(convertDate);
          }
        } else if (targetValue == "-") {
          const replaceAllTargetValue = targetValue.replace(/-/g, "");
          if (replaceAllTargetValue.length === 8) {
            const convertDate = this.convertDateSTH(replaceAllTargetValue);
            this.edate = convertDate;
            this.setFileEDate(convertDate);
          }
        } else {
          this.edate = "";
          this.resetFileEDate();
          this.$fn.notify("error", { message: "숫자만 입력 가능 합니다." });
        }
      }
    }
  }
};
</script>

<style></style>
