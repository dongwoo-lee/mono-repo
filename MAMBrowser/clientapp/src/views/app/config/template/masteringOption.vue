<template>
  <div class="card-body">
    <b-form-group label="Sample Rate" class="has-float-label">
      <b-form-select
        style="width:200px;"
        v-model="SampleRate"
        :options="SampleRateOptions"
      />
    </b-form-group>
    <b-form-group label="Bit Depth" class="has-float-label">
      <b-form-select
        style="width:200px;"
        v-model="BitDepth"
        :options="BitDepthOptions"
      />
    </b-form-group>
    <b-form-group label="Channels" class="has-float-label">
      <b-form-select
        style="width:200px;"
        v-model="Channels"
        :options="ChannelsOptions"
      />
    </b-form-group>
    <b-form-group label="MSec" class="has-float-label">
      <b-form-input v-model="Msec" />
    </b-form-group>
    {{ getMSec }}
    <br />
    <br />

    <b-form-group label="DB" class="has-float-label">
      <b-form-input v-model="DB" />
    </b-form-group>

    <b-form-group label="MP3 Decoder" class="has-float-label">
      <b-form-input v-model="MP3Decoder" />
    </b-form-group>
    <b-form-group label="MP3 Decoder" class="has-float-label">
      <b-form-input v-model="MP3Decoder" />
    </b-form-group>
    <b-form-group label="MP3 Decoder" class="has-float-label">
      <b-form-input v-model="MP3Decoder" />
    </b-form-group>
    <b-form-group label="MP3 Decoder" class="has-float-label">
      <b-form-input v-model="MP3Decoder" />
    </b-form-group>
    <b-form-group label="MP3 Decoder" class="has-float-label">
      <b-form-input v-model="MP3Decoder" />
    </b-form-group>
  </div>
</template>

<script>
export default {
  data() {
    return {
      SampleRate: "44100",
      SampleRateOptions: [
        { value: "44100", text: "44100" },
        { value: "48000", text: "48000" }
      ],
      BitDepth: "16",
      BitDepthOptions: [
        { value: "16", text: "16" },
        { value: "24", text: "24" }
      ],
      Channels: "2",
      ChannelsOptions: [{ value: "2", text: "2" }],
      Msec: "5",
      DB: "-40",
      MP3Decoder: "LAME"
    };
  },
  computed: {
    getMSec() {
      if (this.Msec < 50 || 10000 < this.Msec) {
        return "50에서 10000 사이의 값만 입력해야 합니다.";
      }
    }
  }
};
</script>

<style></style>
