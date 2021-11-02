<template>
  <div class="card-body">
    <div>
      <span style="width:400px;float:left; margin-right:180px;">
        <b-form-group label="Sample Rate" class="has-float-label">
          <b-form-select
            style="width:400px;"
            v-model="optionData.SampleRate"
            :options="SampleRateOptions"
          />
        </b-form-group>
      </span>
      <span style="width:400px;float:left; margin-right:180px;">
        <b-form-group label="Bit Depth" class="has-float-label">
          <b-form-select
            style="width:400px;"
            v-model="optionData.BitDepth"
            :options="BitDepthOptions"
          />
        </b-form-group>
      </span>
      <span style="width:400px;">
        <b-form-group label="Channels" class="has-float-label">
          <b-form-select
            style="width:400px;"
            v-model="optionData.Channels"
            :options="ChannelsOptions"
          />
        </b-form-group>
      </span>
    </div>
    <div>
      <span style="width:200px; float:left; margin-right:20px;">
        <b-form-group label="MSec" class="has-float-label">
          <b-form-input v-model="optionData.Msec" />
        </b-form-group>
      </span>
      <p style="vertical-align:text-top; float:left; margin-right:100px;">
        {{ getMSec }}
      </p>
      <span style="width:400px;float:left; margin-right:180px;">
        <b-form-group label="DB" class="has-float-label">
          <b-form-input v-model="optionData.DB" />
        </b-form-group>
      </span>
      <span style="width:400px;">
        <b-form-group label="MP3 Decoder" class="has-float-label">
          <b-form-input style="width:400px;" v-model="optionData.MP3Decoder" />
        </b-form-group>
      </span>
    </div>

    <div style="margin-top:20px;">
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
    <div style="margin-left:1432px;">
      <b-button variant="outline-success" @click="save">저장</b-button>
      <b-button variant="outline-danger" @click="cancel">취소</b-button>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      optionData: {
        SampleRate: "44100",
        BitDepth: "16",
        Channels: "2",
        Msec: "4",
        DB: "-40",
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
      ChannelsOptions: [{ value: "2", text: "2" }]
    };
  },
  computed: {
    getMSec() {
      if (this.Msec < 50 || 10000 < this.Msec) {
        return "50에서 10000 사이의 값만 입력해야 합니다.";
      }
    }
  },
  methods: {
    save() {
      console.log(this.optionData);
    },
    cancel() {}
  }
};
</script>

<style></style>
