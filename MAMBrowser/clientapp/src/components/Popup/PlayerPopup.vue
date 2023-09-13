<template>
  <!-- 미리듣기 팝업 -->
  <b-modal id="modal-player" size="lg2" v-model="show" no-close-on-backdrop>
    <template slot="modal-title">
      <h5 class="player-overflow" :title="title">
        {{ title }}
      </h5>
    </template>
    <template slot="default">
      <Player
        ref="play"
        :requestType="requestType"
        :fileKey="fileKey"
        :tempDownloadUrl="tempDownloadUrl"
        :waveformUrl="waveformUrl"
        :streamingUrl="streamingUrl"
        :direct="direct"
      />
    </template>
    <template v-slot:modal-footer>
      <b-button
        variant="outline-danger default cutom-label-cancel"
        size="sm"
        class="float-right"
        @click="show = false"
      >
        닫기</b-button
      >
    </template>
  </b-modal>
</template>
<script>
export default {
  props: {
    requestType: {
      type: String,
      default: () => {},
    },
    fileKey: {
      type: [String, Number],
      default: () => {},
    },
    title: {
      type: String,
      default: () => {},
    },
    tempDownloadUrl: {
      type: String,
      default: () => {},
    },
    streamingUrl: {
      type: String,
      default: () => {},
    },
    waveformUrl: {
      type: String,
      default: () => {},
    },
    showPlayerPopup: {
      type: Boolean,
      default: () => false,
    },
    direct: {
      type: String,
      default: () => "N",
    },
  },
  computed: {
    show: {
      get() {
        return this.showPlayerPopup;
      },
      set(v) {
        if (!v) {
          this.closePlayer();
        }
      },
    },
  },
  methods: {
    closePlayer() {
      this.$refs.play.close();
      this.$emit("closePlayer");
    },
    changedVolume2(num) {
      this.$refs.play.changedVolume2(num);
    },
    SkipSec(num) {
      this.$refs.play.SkipSec(num);
    },
  },
};
</script>
<style scoped>
.player-overflow {
  width: 700px;
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
}
</style>
