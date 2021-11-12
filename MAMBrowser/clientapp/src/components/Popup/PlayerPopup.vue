<template>
  <!-- 미리듣기 팝업 -->
  <b-modal id="modal-player" size="lg" v-model="show" no-close-on-backdrop>
    <template slot="modal-title">
      <h5>{{ title }}</h5>
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
        :startPoint="startPoint"
        :endPoint="endPoint"
        @startPosition="(val) => (startPosition = val)"
        @endPosition="(val) => (endPosition = val)"
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
      <b-button
        variant="outline-success default cutom-label"
        size="sm"
        class="float-right"
        @click="editOK()"
      >
        편집 저장</b-button
      >
      <!-- 여기에다가 편집 저장 버튼 추가해야함 그리고 거기에 Click이벤트로 SOM, EOM 찍히는지 확인하기 -->
    </template>
  </b-modal>
</template>
<script>
import { mapActions, mapGetters, mapMutations } from "vuex";
import { eventBus } from "@/eventBus";
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
    rowNum: Number,
    type: String,
    startPoint: Number,
    endPoint: Number,
  },
  data() {
    return {
      startPosition: 0,
      endPosition: 0,
    };
  },
  computed: {
    ...mapGetters("cueList", ["abCartArr"]),
    ...mapGetters("cueList", ["cChannelData"]),
    ...mapGetters("cueList", ["cueFavorites"]),

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
    ...mapMutations("cueList", ["SET_ABCARTARR"]),
    ...mapMutations("cueList", ["SET_CCHANNELDATA"]),
    ...mapMutations("cueList", ["SET_CUEFAVORITES"]),
    closePlayer() {
      this.$refs.play.close();
      this.$emit("closePlayer");
    },
    editOK() {
      if (this.type == "A") {
        var rowData = [...this.abCartArr];
        rowData = this.setTime(rowData);
        this.SET_ABCARTARR(rowData);
        console.log(rowData);
      } else if (this.type == "channel_my") {
        var rowData = [...this.cueFavorites];
        this.setTime(rowData);
        this.SET_CUEFAVORITES(rowData);
        eventBus.$emit("clearFav");
      }
      this.show = false;
      //이거 나중에 C 도 추가해야하는데 C는 따로 해야함
    },
    setTime(rowData) {
      var startTime = 0;
      var endTime = 0;
      rowData.forEach((ele) => {
        if (ele.rowNum == this.rowNum) {
          startTime = Math.floor(this.startPosition * 1000);
          if (this.endPosition * 1000 > ele.duration) {
            endTime = ele.duration;
          } else {
            endTime = Math.floor(this.endPosition * 1000);
          }
          ele.startposition = startTime;

          if (endTime != 0) {
            ele.endposition = endTime;
          }
        }
      });
      return rowData;
    },
  },
};
</script>