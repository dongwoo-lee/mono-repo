<template>
  <!-- 미리듣기 팝업 -->
  <b-modal id="modal-player" size="lg2" v-model="show" no-close-on-backdrop>
    <template slot="modal-title">
      <div
        style="display: inline-flex"
        class="editPlayer-overflow"
        :title="title"
      >
        <h6 v-if="parentName != ''">
          <b-badge class="mr-2" variant="dark">{{ parentName }}</b-badge>
        </h6>
        <h5 style="line-height: 1.4" class="editPlayer-overflow">
          {{ title }}
        </h5>
      </div>
    </template>
    <template slot="default">
      <EditPlayer
        ref="play"
        :requestType="requestType"
        :fileKey="fileKey"
        :tempDownloadUrl="tempDownloadUrl"
        :waveformUrl="waveformUrl"
        :streamingUrl="streamingUrl"
        :direct="direct"
        :startPoint="startPoint"
        :endPoint="endPoint"
        :fadeIn="{ fadeInValue: fadeIn }"
        :fadeOut="{ fadeOutValue: fadeOut }"
        :exceptflag="{ exceptFlagValue: exceptflag}"
        @startPosition="(val) => (startPosition = val)"
        @endPosition="(val) => (endPosition = val)"
        @fadeValue="(val) => (selected = val)"
        @isSuccess="(val) => (isSuccess = val)"
        :parentName ="parentName"
      />
    </template>
    <template v-slot:modal-footer>
      <div>
        <b-button
          variant="outline-warning default cutom-label"
          size="sm"
          v-if="cueInfo.cuetype != 'A' && isSuccess"
          @click="editClear()"
        >
          편집 초기화</b-button
        >
        <b-button
          size="sm"
          v-if="cueInfo.cuetype != 'A' && isSuccess"
          @click="editOK()"
        >
          편집 저장</b-button
        >
      </div>
      <div style="margin-left: auto">
        <b-button
          variant="outline-danger default cutom-label-cancel"
          size="sm"
          @click="show = false"
        >
          닫기</b-button
        >
      </div>
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
    parentName: {
      type: String,
      default: "",
    },
    rowNum: Number,
    type: String,
    startPoint: Number,
    endPoint: Number,
    fadeIn: Boolean,
    fadeOut: Boolean,
    exceptflag : Boolean,
  },
  data() {
    return {
      selected: [],
      startPosition: null,
      endPosition: null,
      isSuccess: false,
    };
  },
  computed: {
    ...mapGetters("cueList", ["abCartArr"]),
    ...mapGetters("cueList", ["cChannelData"]),
    ...mapGetters("cueList", ["cueFavorites"]),
    ...mapGetters("cueList", ["cueInfo"]),

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
      this.startPosition = null;
      this.endPosition = null;
      this.selected = [];
      this.isSuccess = false;
      this.$emit("closePlayer");
    },
    editOK() {    
      switch (this.type) {
        case "A":
          var rowData = [...this.abCartArr];
          rowData = this.setTime(rowData);
          this.SET_ABCARTARR(rowData);
          break;
        case "channel_my":
          var rowData = [...this.cueFavorites];
          this.setTime(rowData);
          this.SET_CUEFAVORITES(rowData);
          break;
        case "channel_1":
          var rowData = { ...this.cChannelData };
          this.setTime(rowData[this.type]);
          this.SET_CCHANNELDATA(rowData);
          break;
        case "channel_2":
          var rowData = { ...this.cChannelData };
          this.setTime(rowData[this.type]);
          this.SET_CCHANNELDATA(rowData);
          break;
        case "channel_3":
          var rowData = { ...this.cChannelData };
          this.setTime(rowData[this.type]);
          this.SET_CCHANNELDATA(rowData);
          break;
        case "channel_4":
          var rowData = { ...this.cChannelData };
          this.setTime(rowData[this.type]);
          this.SET_CCHANNELDATA(rowData);
          break;
        default:
          break;
      }
      this.startPosition = null;
      this.endPosition = null;
      this.show = false;
    },
    editClear() {
      var endVal = this.$refs.play.clearEdit();
      this.startPosition = 0;
      this.endPosition = endVal;
    },
    setTime(rowData) {
      console.info('rowData', rowData);

      var startTime = 0;
      var endTime = 0;
      rowData.forEach((ele) => {
        if (ele.rownum == this.rowNum) {
          if (this.startPosition != null) {
            startTime = Math.floor(this.startPosition * 1000);
            ele.startposition = startTime;
          }
          if (this.endPosition * 1000 + 1 > ele.duration) {
            endTime = ele.duration;
          } else {
            endTime = Math.floor(this.endPosition * 1000);
          }

          if (endTime != 0) {
            ele.endposition = endTime;
          }
          var fadeinvalue = this.selected.filter((ele) => {
            return Object.keys(ele).includes("fadeInValue");
          });
          if (fadeinvalue.length != 0) {
            ele.fadeintime = true;
          } else {
            ele.fadeintime = false;
          }
          var fadeoutvalue = this.selected.filter((ele) => {
            return Object.keys(ele).includes("fadeOutValue");
          });
          if (fadeoutvalue.length != 0) {
            ele.fadeouttime = true;
          } else {
            ele.fadeouttime = false;
          }

           var exceptFlagValue = this.selected.filter((ele) => {
            return Object.keys(ele).includes("exceptFlagValue");
          });
          if (exceptFlagValue.length != 0) {
            ele.exceptflag = exceptFlagValue ? 'Y' : 'N';
          } else {
            ele.exceptflag = 'N';
          }
        }
      });
      this.selected = [];
      return rowData;
    },
  },
};
</script>
<style scoped>
.editPlayer-overflow {
  width: 700px;
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
}
</style>
