<template>
  <div class="card-body">
    <div style="margin-left: 395px">
      <h4 style="color: black">마스터링 옵션</h4>
      <div
        style="
          padding: 25px;
          padding-bottom: 10px;
          width: 840px;
          border: 1px solid silver;
        "
      >
        <span style="width: 200px; float: left; margin-right: 90px">
          <b-form-group label="Sample Rate" class="has-float-label">
            <b-form-select
              style="width: 200px"
              v-model="SAMPLE_RATE"
              :options="SampleRateOptions"
            />
          </b-form-group>
        </span>
        <span style="width: 200px; float: left; margin-right: 90px">
          <b-form-group label="Bit Depth" class="has-float-label">
            <b-form-select
              style="width: 200px"
              v-model="BIT_DEPTH"
              :options="BitDepthOptions"
            />
          </b-form-group>
        </span>
        <span style="width: 200px">
          <b-form-group label="Channels" class="has-float-label">
            <b-form-select
              style="width: 200px"
              v-model="CHANNEL"
              :options="ChannelsOptions"
            />
          </b-form-group>
        </span>

        <span style="width: 200px; float: left; margin-right: 90px">
          <b-form-group label="무음 감지 길이" class="has-float-label">
            <b-form-input v-model="SILENCE_DURATION" />
          </b-form-group>
          <p
            style="
              position: absolute;
              top: 255px;
              left: 475px;
              color: red;
              font-size: 10.5px;
            "
          >
            {{ getSILENCE_DURATION }}
          </p>
        </span>
        <span style="width: 200px; float: left; margin-right: 90px">
          <b-form-group label="무음 감지 데시벨" class="has-float-label">
            <b-form-input v-model="SILENCE_DB" />
          </b-form-group>
        </span>
        <span style="width: 200px">
          <b-form-group label="MP3 Decoder" class="has-float-label">
            <b-form-input style="width: 200px" v-model="MP3_DECODER" />
          </b-form-group>
        </span>
      </div>
      <h4 style="color: black; margin-top: 20px">스토리지 설정</h4>
      <div
        style="
          padding: 25px;
          width: 840px;
          height: 330px;
          border: 1px solid silver;
        "
      >
        <b-form-group
          label="PGM-AM"
          class="has-float-label"
          style="position: absolute; top: 355px; left: 475px"
        >
          <b-form-input v-model="PGM_AM_PATH" style="width: 375px" />
        </b-form-group>
        <b-form-group
          label="PGM-FM"
          class="has-float-label"
          style="position: absolute; top: 355px; left: 883px"
        >
          <b-form-input v-model="PGM_FM_PATH" style="width: 375px" />
        </b-form-group>
        <b-form-group
          label="PGM-DMB"
          class="has-float-label"
          style="position: absolute; top: 405px; left: 475px"
        >
          <b-form-input v-model="PGM_DMB_PATH" style="width: 375px" />
        </b-form-group>
        <b-form-group
          label="SPOT"
          class="has-float-label"
          style="position: absolute; top: 405px; left: 883px"
        >
          <b-form-input v-model="SPOT_PATH" style="width: 375px" />
        </b-form-group>
        <b-form-group
          label="취재물"
          class="has-float-label"
          style="position: absolute; top: 455px; left: 475px"
        >
          <b-form-input v-model="REPORT_PATH" style="width: 375px" />
        </b-form-group>
        <b-form-group
          label="필러"
          class="has-float-label"
          style="position: absolute; top: 455px; left: 883px"
        >
          <b-form-input v-model="FILLER_PATH" style="width: 375px" />
        </b-form-group>
        <b-form-group
          label="변동소재"
          class="has-float-label"
          style="position: absolute; top: 505px; left: 475px"
        >
          <b-form-input v-model="VAR_PATH" style="width: 375px" />
        </b-form-group>
        <b-form-group
          label="고정소재"
          class="has-float-label"
          style="position: absolute; top: 505px; left: 883px"
        >
          <b-form-input v-model="STATIC_PATH" style="width: 375px" />
        </b-form-group>
        <b-form-group
          label="Song"
          class="has-float-label"
          style="position: absolute; top: 555px; left: 475px"
        >
          <b-form-input v-model="SONG_PATH" style="width: 375px" />
        </b-form-group>
        <b-form-group
          label="임시 업로드"
          class="has-float-label"
          style="position: absolute; top: 555px; left: 883px"
        >
          <b-form-input v-model="MAM_UPLOAD_PATH" style="width: 375px" />
        </b-form-group>
        <b-form-group
          label="임시 작업"
          class="has-float-label"
          style="position: absolute; top: 605px; left: 475px"
        >
          <b-form-input v-model="MST_UPLOAD_PATH" style="width: 375px" />
        </b-form-group>
      </div>
      <div style="margin-left: 705px; margin-top: 30px; margin-bottom: -20px">
        <b-button
          variant="outline-primary"
          :disabled="!isDuration"
          @click="save"
          >저장</b-button
        >
        <b-button variant="outline-danger" @click="cancel">취소</b-button>
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
      SILENCE_DB: "",
      SILENCE_DURATION: "",
      SPOT_PATH: "",
      STATIC_PATH: "",
      VAR_PATH: "",
      MP3_DECODER: "",
      SONG_PATH: "",
      MAM_UPLOAD_PATH: "",
      MST_UPLOAD_PATH: "",
      SampleRateOptions: [
        { value: "44100", text: "44100" },
        { value: "48000", text: "48000" },
      ],
      BitDepthOptions: [
        { value: "16", text: "16" },
        { value: "24", text: "24" },
      ],
      ChannelsOptions: [{ value: "2", text: "2" }],
      sdu: false,
      sdb: false,
    };
  },
  created() {
    axios.get("/api/options/S01G06C001").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this[e.name] = e.value;
      });
    });
  },
  computed: {
    isDuration() {
      if (this.sdu) {
        return true;
      }
      return false;
    },
    getSILENCE_DURATION() {
      if (this.SILENCE_DURATION < 50 || 10000 < this.SILENCE_DURATION) {
        this.sdu = false;
        return "50에서 10000 사이의 값만 입력해야 합니다.";
      } else {
        this.sdu = true;
      }
    },
  },
  methods: {
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
          name: "MP3_DECODER",
          value: this.MP3_DECODER,
        },
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
      ];
      axios.post("/api/options/S01G06C001", list).then((res) => {
        if (res.status == 200 && res.data.errorMsg == null) {
          this.$fn.notify("primary", { title: "마스터링 옵션 저장 성공" });
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
          this.$fn.notify("primary", { title: "마스터링 옵션 변경 취소" });
        } else {
          this.$fn.notify("error", { title: res.data.errorMsg });
        }
      });
    },
  },
};
</script>

<style></style>
