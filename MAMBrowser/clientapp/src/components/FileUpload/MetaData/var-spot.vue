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
    <!-- 시작/종료일 -->
    <div style="margin-top:20px;">
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
    <!-- select box -->
    <div>
      <b-form-group label="매체" class="has-float-label">
        <b-form-select
          id="program-media"
          class="media-select"
          style=" width:200px; height:37px;"
          :value="staticMedia"
          :options="fileMediaOptions"
          @input="mediaChange"
        />
      </b-form-group>
      <b-form-group label="분류" class="has-float-label">
        <b-form-select
          id="program-media"
          class="media-select"
          style=" width:200px; height:37px;"
          :value="MetaData.timeToneSelected"
          :options="timetoneOptions"
          value-field="id"
          text-field="name"
          @input="mediaChange"
        />
      </b-form-group>
      <b-form-group label="상태" class="has-float-label">
        <b-form-select
          id="program-media"
          class="media-select"
          style=" width:200px; height:37px;"
          :value="MetaData.reqStatusSelected"
          :options="reqStatusOptions"
          value-field="id"
          text-field="name"
          @input="mediaChange"
        />
      </b-form-group>
    </div>
    <div style="height:50px;">
      <b-form-input
        style="width:550px;"
        class="editTask"
        v-model="MetaData.advertiser"
        :state="advertiserState"
        aria-describedby="input-live-help input-live-feedback"
        placeholder="광고주 명"
        trim
      />
      <button
        v-show="advertiserState"
        style="position:relative; left:170px; top:-27px; z-index:9999; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
      >
        <b-icon
          icon="x-circle"
          font-scale="1"
          style="position:relative; top:0px; right:0px; z-index:9999;"
          variant="secondary"
          @click="resetAdvertiser"
        ></b-icon>
      </button>
    </div>
  </div>
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import mixinFillerPage from "../../../mixin/MixinFillerPage";
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
import axios from "axios";
export default {
  components: {
    CommonVueSelect
  },
  mixins: [CommonFileFunction, MixinBasicPage, mixinFillerPage],
  data() {
    return {
      staticMedia: "",
      sdate: "",
      edate: ""
    };
  },
  created() {
    this.reset();
    this.getEditorForMd(); //제작자
    this.resetFileMediaOptions(); //매체 초기화
    //매체 생성
    axios.get("/api/categories/media").then(res => {
      res.data.resultObject.data.forEach(e => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name
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
          message: "시작 날짜가 종료 날짜보다 큽니다."
        });
      }
    },
    edate() {
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
