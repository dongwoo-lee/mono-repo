<template>
  <div class="card-body">
    <div style="margin-left:395px;">
      <h4 style="color:#008ECC;">
        마스터링 옵션
      </h4>
      <div
        style="margin-top: 20px; padding:25px; width:840px; border:1px solid #008ECC"
      >
        <span style="width:200px;float:left; margin-right:90px;">
          <b-form-group label="Sample Rate" class="has-float-label">
            <b-form-select
              style="width:200px;"
              v-model="SAMPLE_RATE"
              :options="SampleRateOptions"
            />
          </b-form-group>
        </span>
        <span style="width:200px;float:left; margin-right:90px;">
          <b-form-group label="Bit Depth" class="has-float-label">
            <b-form-select
              style="width:200px;"
              v-model="BIT_DEPTH"
              :options="BitDepthOptions"
            />
          </b-form-group>
        </span>
        <span style="width:200px;">
          <b-form-group label="Channels" class="has-float-label">
            <b-form-select
              style="width:200px;"
              v-model="CHANNEL"
              :options="ChannelsOptions"
            />
          </b-form-group>
        </span>

        <span style="width:200px; float:left; margin-right:90px;">
          <b-form-group label="SILENCE_DURATION" class="has-float-label">
            <b-form-input :state="isDuration" v-model="SILENCE_DURATION" />
          </b-form-group>
        </span>
        <span style="width:200px;float:left; margin-right:90px;">
          <b-form-group label="SILENCE_DB" class="has-float-label">
            <b-form-input v-model="SILENCE_DB" />
          </b-form-group>
        </span>
        <span style="width:200px;">
          <b-form-group label="MP3 Decoder" class="has-float-label">
            <b-form-input style="width:200px;" v-model="MP3_DECODER" />
          </b-form-group>
        </span>
        <p
          style="position:absolute; top:200px; left:450px; color:red; font-size:10.5px;"
        >
          {{ getSILENCE_DURATION }}
        </p>
      </div>
      <h4 style="color:#008ECC;margin-top:40px;">
        파일 경로 설정
      </h4>
      <div
        style="margin-top: 20px; padding:25px; width:840px; height:230px; border:1px solid #008ECC"
      >
        <b-form-group
          label="AM"
          class="has-float-label"
          style="position:absolute; top:415px; left:475px;"
        >
          <b-form-input v-model="PGM_AM_PATH" style="width:375px;" />
        </b-form-group>
        <b-form-group
          label="FM"
          class="has-float-label"
          style="position:absolute; top:415px; left:883px;"
        >
          <b-form-input v-model="PGM_FM_PATH" style="width:375px;" />
        </b-form-group>
        <b-form-group
          label="DMB"
          class="has-float-label"
          style="position:absolute; top:465px; left:475px;"
        >
          <b-form-input v-model="PGM_DMB_PATH" style="width:375px;" />
        </b-form-group>
        <b-form-group
          label="SPOT"
          class="has-float-label"
          style="position:absolute; top:465px; left:883px;"
        >
          <b-form-input v-model="SPOT_PATH" style="width:375px;" />
        </b-form-group>
        <b-form-group
          label="취재물"
          class="has-float-label"
          style="position:absolute; top:515px; left:475px;"
        >
          <b-form-input v-model="REPORT_PATH" style="width:375px;" />
        </b-form-group>
        <b-form-group
          label="필러"
          class="has-float-label"
          style="position:absolute; top:515px; left:883px;"
        >
          <b-form-input v-model="FILLER_PATH" style="width:375px;" />
        </b-form-group>
        <b-form-group
          label="변동소재"
          class="has-float-label"
          style="position:absolute; top:565px; left:475px;"
        >
          <b-form-input v-model="VAR_PATH" style="width:375px;" />
        </b-form-group>
        <b-form-group
          label="고정소재"
          class="has-float-label"
          style="position:absolute; top:565px; left:883px;"
        >
          <b-form-input v-model="STATIC_PATH" style="width:375px;" />
        </b-form-group>
      </div>
      <div style="margin-left:705px; margin-top:30px;  margin-bottom:-20px;">
        <b-button variant="outline-success" @click="save">저장</b-button>
        <b-button variant="outline-danger" @click="cancel">취소</b-button>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
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
      SampleRateOptions: [
        { value: "44100", text: "44100" },
        { value: "48000", text: "48000" }
      ],
      BitDepthOptions: [
        { value: "16", text: "16" },
        { value: "24", text: "24" }
      ],
      ChannelsOptions: [{ value: "2", text: "2" }],
      isDuration: false
    };
  },
  created() {
    axios.get("/api/options/S01G06C001").then(res => {
      res.data.resultObject.data.forEach(e => {
        this[e.name] = e.value;
        console.log(e);
      });
    });
  },
  computed: {
    getSILENCE_DURATION() {
      if (this.SILENCE_DURATION < 50 || 10000 < this.SILENCE_DURATION) {
        this.isDuration = false;
        return "50에서 10000 사이의 값만 입력해야 합니다.";
      } else {
        this.isDuration = true;
      }
    }
  },
  methods: {
    save() {
      let list = [
        {
          name: "BIT_DEPTH",
          value: this.BIT_DEPTH
        },
        {
          name: "SAMPLE_RATE",
          value: this.SAMPLE_RATE
        },
        {
          name: "CHANNEL",
          value: this.CHANNEL
        },
        {
          name: "SILENCE_DURATION",
          value: this.SILENCE_DURATION
        },
        {
          name: "SILENCE_DB",
          value: this.SILENCE_DB
        },
        {
          name: "PGM_AM_PATH",
          value: this.PGM_AM_PATH
        },
        {
          name: "PGM_FM_PATH",
          value: this.PGM_FM_PATH
        },
        {
          name: "PGM_DMB_PATH",
          value: this.PGM_DMB_PATH
        },
        {
          name: "SPOT_PATH",
          value: this.SPOT_PATH
        },
        {
          name: "REPORT_PATH",
          value: this.REPORT_PATH
        },
        {
          name: "FILLER_PATH",
          value: this.FILLER_PATH
        },
        {
          name: "STATIC_PATH",
          value: this.STATIC_PATH
        },
        {
          name: "VAR_PATH",
          value: this.VAR_PATH
        },
        {
          name: "MP3_DECODER",
          value: this.MP3_DECODER
        }
      ];

      axios.post("/api/options/S01G06C001", list).then(res => {
        console.log(res);
      });
    },
    cancel() {}
  }
};
</script>

<style></style>
