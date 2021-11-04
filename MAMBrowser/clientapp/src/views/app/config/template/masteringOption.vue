<template>
  <div class="card-body">
    <div style="margin-left:395px">
      <span style="width:200px;float:left; margin-right:90px;">
        <b-form-group label="Sample Rate" class="has-float-label">
          <b-form-select
            style="width:200px;"
            v-model="optionData.SampleRate"
            :options="SampleRateOptions"
          />
        </b-form-group>
      </span>
      <span style="width:200px;float:left; margin-right:90px;">
        <b-form-group label="Bit Depth" class="has-float-label">
          <b-form-select
            style="width:200px;"
            v-model="optionData.BitDepth"
            :options="BitDepthOptions"
          />
        </b-form-group>
      </span>
      <span style="width:200px;">
        <b-form-group label="Channels" class="has-float-label">
          <b-form-select
            style="width:200px;"
            v-model="optionData.Channels"
            :options="ChannelsOptions"
          />
        </b-form-group>
      </span>
    </div>
    <div style="margin-left:395px;">
      <span style="width:200px; float:left; margin-right:90px;">
        <b-form-group label="SILENCE_DURATION" class="has-float-label">
          <b-form-input
            :state="isActive"
            v-model="optionData.SILENCE_DURATION"
          />
        </b-form-group>
      </span>
      <span style="width:200px;float:left; margin-right:90px;">
        <b-form-group label="SILENCE_DB" class="has-float-label">
          <b-form-input v-model="optionData.SILENCE_DB" />
        </b-form-group>
      </span>
      <span style="width:200px;">
        <b-form-group label="MP3 Decoder" class="has-float-label">
          <b-form-input style="width:200px;" v-model="optionData.MP3Decoder" />
        </b-form-group>
      </span>
      <p
        style="position:absolute; top:200px; left:450px; color:red; font-size:10.5px;"
      >
        {{ getSILENCE_DURATION }}
      </p>
    </div>

    <div style="margin-top:40px; margin-left:395px; width:780px;">
      <b-form-group label="AM" class="has-float-label">
        <b-form-input v-model="optionData.AM" />
      </b-form-group>
      <b-form-group label="FM" class="has-float-label">
        <b-form-input v-model="optionData.FM" />
      </b-form-group>
      <b-form-group label="DMB" class="has-float-label">
        <b-form-input v-model="optionData.DMB" />
      </b-form-group>
      <b-form-group label="SPOT" class="has-float-label">
        <b-form-input v-model="optionData.SPOT" />
      </b-form-group>
      <b-form-group label="취재물" class="has-float-label">
        <b-form-input v-model="optionData.REPORT" />
      </b-form-group>
      <b-form-group label="필러" class="has-float-label">
        <b-form-input v-model="optionData.FILLER" />
      </b-form-group>
      <b-form-group label="변동소재" class="has-float-label">
        <b-form-input v-model="optionData.VAR" />
      </b-form-group>
      <b-form-group label="고정소재" class="has-float-label">
        <b-form-input v-model="optionData.STATIC" />
      </b-form-group>
    </div>
    <div style="margin-left:1040px;">
      <b-button variant="outline-success" @click="save">저장</b-button>
      <b-button variant="outline-danger" @click="cancel">취소</b-button>
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  data() {
    return {
      optionData: {
        SampleRate: "44100",
        BitDepth: "16",
        Channels: "2",
        SILENCE_DURATION: "4000",
        SILENCE_DB: "-40",
        MP3Decoder: "LAME",
        AM: "\\\\TEST_SVR\\MBCDATA\\AM\\",
        FM: "\\\\TEST_SVR\\MBCDATA\\FM\\",
        DMB: "\\\\TEST_SVR\\MBCDATA\\DMB\\",
        SPOT: "\\\\TEST_SVR\\MBCDATA\\SPOT\\",
        REPORT: "\\\\TEST_SVR\\MBCDATA\\REPORT\\",
        FILLER: "\\\\TEST_SVR\\MBCDATA\\FILLER\\",
        VAR: "\\\\TEST_SVR\\MBCDATA\\VAR\\",
        STATIC: "\\\\TEST_SVR\\MBCDATA\\STATIC\\"
      },
      SampleRateOptions: [
        { value: "44100", text: "44100" },
        { value: "48000", text: "48000" }
      ],
      BitDepthOptions: [
        { value: "16", text: "16" },
        { value: "24", text: "24" }
      ],
      ChannelsOptions: [{ value: "2", text: "2" }],
      isActivce: false
    };
  },
  created() {
    axios.get("/api/options/S01G06C001").then(res => {
      console.log(res);
    });
  },
  computed: {
    getSILENCE_DURATION() {
      if (
        this.optionData.SILENCE_DURATION < 50 ||
        10000 < this.optionData.SILENCE_DURATION
      ) {
        this.isActivce = false;
        return "50에서 10000 사이의 값만 입력해야 합니다.";
      } else {
        this.isActivce = true;
      }
    }
  },
  methods: {
    save() {
      let list = [
        {
          name: "BIT_DEPTH",
          value: this.optionData.BitDepth
        },
        {
          name: "SAMPLE_RATE",
          value: this.optionData.SampleRate
        },
        {
          name: "CHANNEL",
          value: this.optionData.Channels
        },
        {
          name: "SILENCE_DURATION",
          value: this.optionData.SILENCE_DURATION
        },
        {
          name: "SILENCE_DB",
          value: this.optionData.SILENCE_DB
        },
        {
          name: "PGM_AM_PATH",
          value: this.optionData.AM
        },
        {
          name: "PGM_FM_PATH",
          value: this.optionData.FM
        },
        {
          name: "PGM_DMB_PATH",
          value: this.optionData.DMB
        },
        {
          name: "SPOT_PATH",
          value: this.optionData.SPOT
        },
        {
          name: "REPORT_PATH",
          value: this.optionData.REPORT
        },
        {
          name: "FILLER_PATH",
          value: this.optionData.FILLER
        },
        {
          name: "STATIC_PATH",
          value: this.optionData.STATIC
        },
        {
          name: "VAR_PATH",
          value: this.optionData.VAR
        }
      ];

      axios.post("/api/options/S01G06C001", list);
    },
    cancel() {}
  }
};
</script>

<style></style>
