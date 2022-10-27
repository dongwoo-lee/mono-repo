<template>
  <div class="card-body masteringOption">
    <div>
      <!--전체-->
      <div style="float: left">
        <!--좌측-->
        <h4 style="color: black">방송의뢰 옵션</h4>
        <div
          style="
            padding: 20px;
            padding-bottom: 5px;
            height: 120px;
            width: 840px;
            border: 1px solid silver;
          "
        >
          <span style="width: 250px; float: left; margin-right: 20px">
            <b-form-group label="Sample Rate" class="has-float-label">
              <b-form-select
                style="width: 250px"
                v-model="SAMPLE_RATE"
                :options="SampleRateOptions"
              />
            </b-form-group>
          </span>
          <span style="width: 250px; float: left; margin-right: 20px">
            <b-form-group label="Bit Depth" class="has-float-label">
              <b-form-select
                style="width: 250px"
                v-model="BIT_DEPTH"
                :options="BitDepthOptions"
              />
            </b-form-group>
          </span>
          <span style="width: 250px">
            <b-form-group label="Channels" class="has-float-label">
              <b-form-select
                style="width: 250px"
                v-model="CHANNEL"
                :options="ChannelsOptions"
              />
            </b-form-group>
          </span>
          <span style="width: 250px; float: left; margin-right: 20px">
            <b-form-group label="무음 감지" class="has-float-label">
              <b-form-select
                style="width: 250px"
                v-model="DETECT_SILENCE"
                :options="DSOptions"
              />
            </b-form-group>
          </span>
          <span style="width: 115px; float: left; margin-right: 20px">
            <b-form-group label="무음 감지 길이" class="has-float-label">
              <b-form-input
                style="width: 115px"
                v-show="isDetect"
                :state="this.sdu"
                @change="durationInput"
                v-model="SILENCE_DURATION"
              />
              <b-form-input
                style="width: 115px"
                disabled
                v-show="!isDetect"
                @change="durationInput"
                v-model="SILENCE_DURATION"
              />
            </b-form-group>
            <p
              style="
                position: absolute;
                top: 245px;
                left: 745px;
                color: red;
                font-size: 10.5px;
              "
            >
              {{ getSILENCE_DURATION }}
            </p>
          </span>
          <span style="width: 115px; float: left; margin-right: 20px">
            <b-form-group label="무음 감지 데시벨" class="has-float-label">
              <b-form-input
                style="width: 115px"
                v-show="isDetect"
                :state="this.sdb"
                @change="dbInput"
                v-model="SILENCE_DB"
              />
              <b-form-input
                style="width: 115px"
                v-show="!isDetect"
                disabled
                @change="dbInput"
                v-model="SILENCE_DB"
              />
            </b-form-group>
            <p
              style="
                position: absolute;
                top: 245px;
                left: 880px;
                color: red;
                font-size: 10.5px;
              "
            >
              {{ getSILENCE_DB }}
            </p>
          </span>

          <!-- <span style="width: 250px">
            <b-form-group label="MP3 Decoder" class="has-float-label">
              <b-form-input style="width: 250px" v-model="MP3_DECODER" />
            </b-form-group>
          </span> -->
        </div>
        <h4 style="color: black; margin-top: 20px">스토리지 설정</h4>
        <div
          style="
            padding: 20px;
            padding-bottom: 10px;
            width: 840px;
            height: 410px;
            border: 1px solid silver;
          "
        >
          <b-form-group
            label="PGM-AM"
            class="has-float-label"
            style="float: left; margin-right: 40px"
          >
            <b-form-input
              :state="AMState"
              v-model="PGM_AM_PATH"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group label="PGM-FM" class="has-float-label">
            <b-form-input
              :state="FMState"
              v-model="PGM_FM_PATH"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group
            label="PGM-DMB"
            class="has-float-label"
            style="float: left; margin-right: 40px"
          >
            <b-form-input
              :state="DMBState"
              v-model="PGM_DMB_PATH"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group
            label="Pro"
            class="has-float-label"
            style="float: left; margin-right: 40px"
          >
            <b-form-input
              :state="ProState"
              v-model="PRO_PATH"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group label="MyDisk" class="has-float-label">
            <b-form-input
              :state="MyDiskState"
              v-model="MYDISK_PATH"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group label="SPOT" class="has-float-label">
            <b-form-input
              :state="SpotState"
              v-model="SPOT_PATH"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group
            label="취재물"
            class="has-float-label"
            style="float: left; margin-right: 40px"
          >
            <b-form-input
              :state="ReportState"
              v-model="REPORT_PATH"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group label="필러" class="has-float-label">
            <b-form-input
              :state="FillerState"
              v-model="FILLER_PATH"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group
            label="변동소재"
            class="has-float-label"
            style="float: left; margin-right: 40px"
          >
            <b-form-input
              :state="VarState"
              v-model="VAR_PATH"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group label="고정소재" class="has-float-label">
            <b-form-input
              :state="StaticState"
              v-model="STATIC_PATH"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group
            label="Song"
            class="has-float-label"
            style="float: left; margin-right: 40px"
          >
            <b-form-input
              :state="SongState"
              v-model="SONG_PATH"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group label="임시 업로드" class="has-float-label">
            <b-form-input
              :state="MamState"
              v-model="MAM_UPLOAD_PATH"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group
            label="임시 작업"
            class="has-float-label"
            style="float: left; margin-right: 40px"
          >
            <b-form-input
              :state="MstState"
              v-model="MST_UPLOAD_PATH"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group label="삭제 파일 보관" class="has-float-label">
            <b-form-input
              :state="recycleState"
              v-model="RECYCLE_PATH"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group
            label="스토리지 ID"
            class="has-float-label"
            style="float: left; margin-right: 40px"
          >
            <b-form-input
              :state="IdState"
              v-model="STORAGE_ID"
              style="width: 375px"
            />
          </b-form-group>
          <b-form-group label="스토리지 암호" class="has-float-label">
            <b-form-input
              :state="PassState"
              v-model="STORAGE_PASS"
              style="width: 375px"
            />
          </b-form-group>
        </div>
        <div style="margin-left: 705px; margin-top: 15px; margin-bottom: -15px">
          <b-button v-show="isDuration" variant="outline-primary" @click="save"
            >저장</b-button
          >
          <b-button
            v-show="!isDuration"
            variant="outline-dark"
            disabled
            @click="save"
            >저장</b-button
          >
          <b-button variant="outline-danger" @click="cancel">취소</b-button>
        </div>
      </div>
      <div style="float: left">
        <h4 style="color: black; margin-left: 50px">기타 설정</h4>
        <div
          style="
            padding: 20px;
            padding-bottom: 10px;
            width: 650px;
            height: 120px;
            border: 1px solid silver;
            margin-left: 50px;
          "
        >
          <b-form-group
            label="웹큐시트 첨부파일 위치"
            class="has-float-label"
            style="float: left; margin-right: 40px"
          >
            <b-form-input
              :state="VarState"
              v-model="WCS_ATTACH_PATH"
              style="width: 375px"
            />
          </b-form-group>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import MixinCommon from "../../../../mixin/MixinCommon";
import axios from "axios";
export default {
  mixin: [MixinCommon],
  data() {
    return {
      BIT_DEPTH: "",
      CHANNEL: "",
      FILLER_PATH: "",
      PGM_AM_PATH: "",
      PGM_DMB_PATH: "",
      PGM_FM_PATH: "",
      REPORT_PATH: "",
      SAMPLE_RATE: "",
      DETECT_SILENCE: "",
      SILENCE_DB: "",
      SILENCE_DURATION: "",
      MYDISK_PATH: "",
      PRO_PATH: "",
      SPOT_PATH: "",
      STATIC_PATH: "",
      VAR_PATH: "",
      WCS_ATTACH_PATH: "",
      // MP3_DECODER: "",
      SONG_PATH: "",
      MAM_UPLOAD_PATH: "",
      MST_UPLOAD_PATH: "",
      RECYCLE_PATH: "",
      STORAGE_ID: "",
      STORAGE_PASS: "",
      SampleRateOptions: [
        { value: "44100", text: "44100" },
        { value: "48000", text: "48000" },
      ],
      BitDepthOptions: [
        { value: "16", text: "16" },
        { value: "24", text: "24" },
      ],
      DSOptions: [
        { value: "Y", text: "사용함" },
        { value: "N", text: "사용안함" },
      ],
      ChannelsOptions: [{ value: "2", text: "2" }],
      sdu: false,
      sdb: false,
      isDetect: false,
    };
  },
  created() {
    axios.get("/api/options/S01G06C001").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this[e.name] = e.value;
      });
    });
  },
  watch: {
    DETECT_SILENCE(v) {
      if (v == "Y") {
        this.isDetect = true;
      } else if (v == "N") {
        this.isDetect = false;
        this.SILENCE_DURATION = 4000;
        this.SILENCE_DB = -42;
      }
    },
  },
  computed: {
    AMState() {
      return this.PGM_AM_PATH == "" ? false : true;
    },
    FMState() {
      return this.PGM_FM_PATH == "" ? false : true;
    },
    DMBState() {
      return this.PGM_DMB_PATH == "" ? false : true;
    },
    MyDiskState() {
      return this.MYDISK_PATH == "" ? false : true;
    },
    ProState() {
      return this.PRO_PATH == "" ? false : true;
    },
    SpotState() {
      return this.SPOT_PATH == "" ? false : true;
    },
    ReportState() {
      return this.REPORT_PATH == "" ? false : true;
    },
    FillerState() {
      return this.FILLER_PATH == "" ? false : true;
    },
    VarState() {
      return this.VAR_PATH == "" ? false : true;
    },
    StaticState() {
      return this.STATIC_PATH == "" ? false : true;
    },
    SongState() {
      return this.SONG_PATH == "" ? false : true;
    },
    MamState() {
      return this.MAM_UPLOAD_PATH == "" ? false : true;
    },
    MstState() {
      return this.MST_UPLOAD_PATH == "" ? false : true;
    },
    recycleState() {
      return this.RECYCLE_PATH == "" ? false : true;
    },
    IdState() {
      return this.STORAGE_ID == "" ? false : true;
    },
    PassState() {
      return this.STORAGE_PASS == "" ? false : true;
    },
    isDuration() {
      if (
        this.sdu &&
        this.sdb &&
        this.AMState &&
        this.FMState &&
        this.DMBState &&
        this.MyDiskState &&
        this.ProState &&
        this.SpotState &&
        this.ReportState &&
        this.FillerState &&
        this.VarState &&
        this.StaticState &&
        this.SongState &&
        this.MamState &&
        this.MstState
      ) {
        return true;
      }
      return false;
    },
    getSILENCE_DB() {
      if (this.SILENCE_DB < -100 || -20 < this.SILENCE_DB) {
        this.sdb = false;
        return "-20 ~ -100 입력 가능";
      } else if (this.SILENCE_DB == "") {
        this.sdb = false;
        return "-20 ~ -100 입력 가능";
      } else {
        this.sdb = true;
      }
    },
    getSILENCE_DURATION() {
      if (this.SILENCE_DURATION < 2000 || 10000 < this.SILENCE_DURATION) {
        this.sdu = false;
        return "2000 ~ 10000 입력 가능";
      } else if (this.SILENCE_DURATION == "") {
        this.sdu = false;
      } else {
        this.sdu = true;
      }
    },
  },
  methods: {
    dbInput(event) {
      if (isNaN(event) || event == "") {
        this.SILENCE_DB = -42;
        return;
      }
    },
    durationInput(v) {
      if (isNaN(v) || v == "") {
        this.SILENCE_DURATION = 4000;
        return;
      }
    },
    save() {
      let list = [
        {
          name: "BIT_DEPTH",
          value: this.BIT_DEPTH,
        },
        {
          name: "SAMPLE_RATE",
          value: this.SAMPLE_RATE,
        },
        {
          name: "CHANNEL",
          value: this.CHANNEL,
        },
        {
          name: "DETECT_SILENCE",
          value: this.DETECT_SILENCE,
        },
        {
          name: "SILENCE_DURATION",
          value: this.SILENCE_DURATION,
        },
        {
          name: "SILENCE_DB",
          value: this.SILENCE_DB,
        },
        {
          name: "PGM_AM_PATH",
          value: this.PGM_AM_PATH,
        },
        {
          name: "PGM_FM_PATH",
          value: this.PGM_FM_PATH,
        },
        {
          name: "PGM_DMB_PATH",
          value: this.PGM_DMB_PATH,
        },
        {
          name: "MYDISK_PATH",
          value: this.MYDISK_PATH,
        },
        {
          name: "PRO_PATH",
          value: this.PRO_PATH,
        },
        {
          name: "SPOT_PATH",
          value: this.SPOT_PATH,
        },
        {
          name: "REPORT_PATH",
          value: this.REPORT_PATH,
        },
        {
          name: "FILLER_PATH",
          value: this.FILLER_PATH,
        },
        {
          name: "STATIC_PATH",
          value: this.STATIC_PATH,
        },
        {
          name: "VAR_PATH",
          value: this.VAR_PATH,
        },
        {
          name: "WCS_ATTACH_PATH",
          value: this.WCS_ATTACH_PATH,
        },
        // {
        //   name: "MP3_DECODER",
        //   value: this.MP3_DECODER,
        // },
        {
          name: "MST_UPLOAD_PATH",
          value: this.MST_UPLOAD_PATH,
        },
        {
          name: "MAM_UPLOAD_PATH",
          value: this.MAM_UPLOAD_PATH,
        },
        {
          name: "SONG_PATH",
          value: this.SONG_PATH,
        },
        {
          name: "RECYCLE_PATH",
          value: this.RECYCLE_PATH,
        },
        {
          name: "STORAGE_ID",
          value: this.STORAGE_ID,
        },
        {
          name: "STORAGE_PASS",
          value: this.STORAGE_PASS,
        },
      ];
      axios.post("/api/options/S01G06C001", list).then((res) => {
        if (res.status == 200 && res.data.errorMsg == null) {
          this.$fn.notify("primary", { title: "방송의뢰 옵션 저장 성공" });
        } else {
          this.$fn.notify("error", { title: res.data.errorMsg });
        }
      });
    },
    cancel() {
      axios.get("/api/options/S01G06C001").then((res) => {
        if (res.status == 200 && res.data.errorMsg == null) {
          res.data.resultObject.data.forEach((e) => {
            this[e.name] = e.value;
          });
          this.$fn.notify("primary", { title: "방송의뢰 옵션 변경 취소" });
        } else {
          this.$fn.notify("error", { title: res.data.errorMsg });
        }
      });
    },
  },
};
</script>

<style>
.masteringOption {
  padding-top: 0px !important;
  padding-bottom: 0px !important;
}
</style>
