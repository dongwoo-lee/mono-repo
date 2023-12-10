<template>
  <div>
    <div>
      <div v-if="errorMsg">
        {{ errorMsg }}
      </div>
      <div class="text-center" v-if="spinnerFlag">
        <b-spinner
          style="width: 4rem; height: 4rem"
          variant="primary"
        ></b-spinner>
      </div>
      <div>
        <b-container class="bv-example-row" v-if="isSuccess">
          <b-row>
            <b-col>
               <span style="display: flex; align-items: center;">
                <b-button
                  style="padding: 6px; width:100px !important; margin-left:6px"
                  variant="outline-primary"
                  @click="somClick"
                  :disabled="cueInfo.cuetype == 'A'"
                >
                  시작지점 설정
                </b-button>
                <b-button
                  style="padding: 6px; width:100px !important; margin-left:6px"
                  variant="outline-primary"
                  @click="eomClick"
                  :disabled="cueInfo.cuetype == 'A'"
                >
                  종료지점 설정
                </b-button>
                <span style="border: 1px solid silver; padding: 8px; width:170px; margin-left:6px"
                  >{{ somTime }} / {{ eomTime }}</span
                >
                 <b-form-checkbox-group
                  style="display: inline-block; margin-left: 2px;"
                  class="custom-checkbox-group"
                  v-model="selected"
                  :options="fadeOptions"
                  value-field="value"
                  text-field="text"
                  :disabled="cueInfo.cuetype == 'A'"
                />  
              </span>
            </b-col>

            <b-col cols="4">
              <div style="margin-top: 8px; margin-bootm: 0">
                <div class="iconButton" style="float: right; margin-left: 10px">
                  <i
                    class="iconsminds-magnifi-glass"
                    style="font-size: 20px; line-height: initial"
                    @click="zoomInClick"
                  />
                </div>
                <div style="float: right">
                  <vue-slider
                    width="130px"
                    :min="zoomMin"
                    :max="zoomMax"
                    :interval="zoomInterval"
                    v-model="zoomSliderValue"
                    @change="changedZoom"
                    :drag-on-click="true"
                    tooltip="none"
                  />
                </div>
                <div
                  class="iconButton"
                  style="float: right; margin-right: 10px"
                >
                  <i
                    class="iconsminds-magnifi-glass--"
                    style="font-size: 20px; line-height: initial"
                    @click="zoomOutClick"
                  />
                </div>
              </div>
            </b-col>
          </b-row>
        </b-container>
      </div>
      <div id="waveform"></div>
      <div id="wave-timeline"></div>
      <br />
      <br />
      <b-container class="bv-example-row" v-if="isSuccess">
        <b-row>
          <b-col class="myCol">
            <i
              v-if="isMute"
              class="iconsminds-speaker-1"
              style="font-size: 20px; color: red"
              @click="muteToggle"
            />
            <i
              v-else
              class="iconsminds-sound"
              style="font-size: 20px"
              @click="muteToggle"
            />
          </b-col>
          <b-col class="myCol2">
            <div class="slider">
              <vue-slider
                width="130px"
                :min="min"
                :max="max"
                :interval="interval"
                v-model="sliderValue"
                @change="changedVolume"
                :drag-on-click="true"
                :tooltipFormatter="tooltipFormatter"
              />
            </div>
          </b-col>
          <b-col>
            <div align="center">
              <b-btn
                style="padding-left: 8px; padding-right: 8px"
                variant="outline-primary"
                size="sm"
                @click.prevent="SkipSec(-5)"
                title="5초전"
              >
                <b-icon
                  icon="arrow-counterclockwise"
                  aria-hidden="true"
                ></b-icon>
              </b-btn>

              <b-btn
                variant="outline-primary"
                size="sm"
                @click.prevent="Play"
                title="재생/일시정지"
                >▶/||</b-btn
              >
              <b-btn
                variant="outline-primary"
                size="sm"
                @click.prevent="Stop"
                title="중지"
                >■</b-btn
              >
              <b-btn
                style="padding-left: 8px; padding-right: 8px"
                variant="outline-primary"
                size="sm"
                @click="editplay"
                >구간 재생</b-btn
              >
              <b-btn
                style="padding-left: 8px; padding-right: 8px"
                variant="outline-primary"
                size="sm"
                @click.prevent="SkipSec(5)"
                title="5초후"
              >
                <b-icon icon="arrow-clockwise" aria-hidden="true"></b-icon>
              </b-btn>
            </div>
          </b-col>
          <b-col>
            <div>
              <p align="right" class="m-0">
                {{ CurrentTime }} / {{ TotalTime }}({{ speed }}배속)
                <b-btn
                  style="padding: 6px"
                  variant="outline-primary"
                  @click.prevent="SetFast(-0.25)"
                  title="0.25 감속"
                  ><b-icon icon="skip-backward" aria-hidden="true"></b-icon
                ></b-btn>
                <b-btn
                  style="padding: 6px"
                  variant="outline-primary"
                  @click.prevent="SetFast(0.25)"
                  title="0.25 가속"
                  ><b-icon icon="skip-forward" aria-hidden="true"></b-icon
                ></b-btn>
              </p>
            </div>
          </b-col>
        </b-row>
      </b-container>
    </div>
  </div>
</template>

<script>
import WaveSurfer from "wavesurfer.js";
import TimelinePlugin from "wavesurfer.js/dist/plugin/wavesurfer.timeline.min.js";
import { USER_ID, ACCESS_TOKEN } from "@/constants/config";
import VueSlider from "vue-slider-component";
// import Markers from "wavesurfer.js/dist/plugin/wavesurfer.markers.min.js";
import Regions from "wavesurfer.js/dist/plugin/wavesurfer.regions.min.js";
import { mapActions, mapGetters } from "vuex";

import "vue-slider-component/theme/default.css";
import axios from "axios";
var wavesurfer;
var httpClient;
var cancelToken;
var source;

export default {
  components: {
    VueSlider,
  },
  data() {
    return {
      speed: 1,
      selected: [],
      isMute: false,
      wavesurfer: null,
      CurrentTime: "00:00:00",
      TotalTime: "00:00:00",
      loadComplete: false,
      isSuccess: false,
      spinnerFlag: true,
      ACCESS_TOKEN,
      USER_ID,
      min: 0,
      max: 1,
      interval: 0.01,
      sliderValue: 1,

      zoomMin: 0,
      zoomMax: 80,
      zoomInterval: 0.1,
      zoomSliderValue: 0,
      errorMsg: "",

      options: {
        id: "Trim",
        start: 0,
        end: 0,
        loop: false,
        drag: false,
        color: "hsla(200, 50%, 70%, 0.4)",
      },
      som: 0,
      eom: 0,
      somTime: "00:00:00",
      eomTime: "00:00:00",
      fadeOptions: [
        {
          text: "페이드 인",
          value: { fadeInValue: true },
        },
        { text: "페이드 아웃", value: { fadeOutValue: true } },
        { text: "선곡제외", value: { exceptFlagValue: true } },
      ],
      buttonItem: [
        {
          icon: "alignleft",
          hint: "현재 재생지점을 Start Point로 변경",
          style: "bold",
          text: "Start Point",
        },
        {
          icon: "alignleft",
          hint: "현재 재생지점을 End Point로 변경",
          style: "bold",
          text: "End Point",
        },
      ],
    };
  },
  watch: {
    selected(val) {
      this.$emit("fadeValue", this.selected);
    },
  },
  mounted() {
    console.info('this.exceptflag', this.exceptflag);
    console.info('this.fadeIn', this.fadeIn);
    console.info('this.fadeOut', this.fadeOut);
    if (this.fadeIn["fadeInValue"]) {
      this.selected.push(this.fadeIn);
    }
    if (this.fadeOut["fadeOutValue"]) {
      this.selected.push(this.fadeOut);
    }
    
     if (this.exceptflag["exceptFlagValue"]) {
      this.selected.push(this.exceptflag);
    }

    cancelToken = axios.CancelToken;
    source = cancelToken.source();
    this.LoadAudio();
  },
  computed: {
    ...mapGetters("cueList", ["cueInfo"]),
  },
  methods: {
    SetFast(num) {
      this.speed = this.speed + num;
      if (this.speed < 0.25) this.speed = 0.25;
      else if (this.speed >= 4) this.speed = 4;

      wavesurfer.setPlaybackRate(this.speed, true);
    },
    SkipSec(num) {
      wavesurfer.skipForward(num);
    },
    editplay() {
      wavesurfer.regions.list["Trim"].play();
    },
    somClick() {
      var startVal = wavesurfer.getCurrentTime();
      var endVal = wavesurfer.regions.list.Trim.end;
      if (endVal > startVal) {
        this.options.start = startVal;
        this.options.end = endVal;
        wavesurfer.regions.clear();
        wavesurfer.addRegion(this.options);
        this.somTime = new Date(
          wavesurfer.regions.list.Trim.start.toFixed(2) * 1000
        );
        this.somTime = this.somTime.toISOString().substr(11, 8);
        this.$emit("startPosition", wavesurfer.regions.list.Trim.start);
      } else {
        this.options.start = startVal;
        this.options.end = this.eom;

        wavesurfer.regions.clear();
        wavesurfer.addRegion(this.options);

        this.somTime = new Date(
          wavesurfer.regions.list.Trim.start.toFixed(2) * 1000
        );
        this.eomTime = new Date(this.options.end.toFixed(2) * 1000);

        this.somTime = this.somTime.toISOString().substr(11, 8);
        this.eomTime = this.eomTime.toISOString().substr(11, 8);
        this.$emit("endPosition", this.options.end);
        this.$emit("startPosition", wavesurfer.regions.list.Trim.start);
      }
    },
    eomClick() {
      var startVal = wavesurfer.regions.list.Trim.start;
      var endVal = wavesurfer.getCurrentTime();
      if (endVal > startVal) {
        this.options.start = startVal;
        this.options.end = endVal;
        wavesurfer.regions.clear();
        wavesurfer.addRegion(this.options);
        this.eomTime = new Date(
          wavesurfer.regions.list.Trim.end.toFixed(2) * 1000
        );
        this.eomTime = this.eomTime.toISOString().substr(11, 8);
        this.$emit("endPosition", wavesurfer.regions.list.Trim.end);
      } else {
        this.options.start = 0;
        this.options.end = endVal;

        wavesurfer.regions.clear();
        wavesurfer.addRegion(this.options);

        this.eomTime = new Date(
          wavesurfer.regions.list.Trim.end.toFixed(2) * 1000
        );
        this.somTime = new Date(this.options.start.toFixed(2) * 1000);

        this.eomTime = this.eomTime.toISOString().substr(11, 8);
        this.somTime = this.somTime.toISOString().substr(11, 8);
        this.$emit("endPosition", wavesurfer.regions.list.Trim.end);
        this.$emit("startPosition", this.options.start);
      }
    },
    InjectWaveSurfer() {
      wavesurfer = WaveSurfer.create({
        plugins: [
          TimelinePlugin.create({
            container: "#wave-timeline",
          }),
          Regions.create(),
        ],
        container: "#waveform",
        //forceDecode: "true",
        waveColor: "gray",
        progressColor: "skyblue",
        backend: "MediaElementWebAudio",
        // splitChannels: true,
        xhr: {
          requestHeaders: [
            {
              key: "X-Csrf-Token",
              value: sessionStorage.getItem(ACCESS_TOKEN),
            },
          ],
        },
      });
      httpClient = axios.create({
        baseURL: process.env.VUE_APP_API_BASEURL,
        withCredentials: false,
        headers: {
          "Content-Type": "application/json",
          "X-Csrf-Token": sessionStorage.getItem(ACCESS_TOKEN),
        },
        timeout: 90000,
      });
    },
    SetWaveSurfer() {
      let vm = this;
      wavesurfer.on("audioprocess", function () {
        if (wavesurfer.isPlaying()) {
          vm.LoadAudioInfo();
        }
      });
      wavesurfer.on("finish", function () {
        vm.LoadAudioInfo();
      });
      wavesurfer.on("seek", (e) => {
        vm.LoadAudioInfo();
      });
      //영역 중간 click 시 정지 여부 > 정지 시 오류 발생됨
      // wavesurfer.on("seek", (e) => {
      //   console.log(wavesurfer.regions.getCurrentRegion());
      //   if (wavesurfer.regions.getCurrentRegion() != null) {
      //     wavesurfer.play(
      //       wavesurfer.getCurrentTime(),
      //       wavesurfer.regions.getCurrentRegion().end
      //     );
      //   }
      // });
      wavesurfer.on("ready", () => {
        var totalSec = wavesurfer.getDuration();
        if (totalSec >= 3000) {
          vm.zoomMax = 100;
          vm.zoomMin = 0;
          vm.zoomInterval = 10;
        } else if (totalSec >= 60) {
          vm.zoomMax = 180;
          vm.zoomInterval = 9;
        } else if (totalSec >= 20) {
          vm.zoomMax = 180;
          vm.zoomInterval = 45;
        } else {
          vm.zoomMax = 180;
          vm.zoomInterval = 90;
        }

        vm.LoadAudioInfo();
        vm.Play();
        if (this.startPoint > 0) {
          vm.options.start = this.startPoint / 1000;
        }
        //vm.options.end = wavesurfer.getDuration();
        vm.options.end = this.endPoint / 1000;

        wavesurfer.addRegion(vm.options);

        vm.eom = wavesurfer.getDuration();
        vm.eomTime = new Date(vm.options.end.toFixed(2) * 1000);
        vm.eomTime = vm.eomTime.toISOString().substr(11, 8);
        vm.somTime = new Date(vm.options.start.toFixed(2) * 1000);
        vm.somTime = vm.somTime.toISOString().substr(11, 8);
      });
      wavesurfer.on("region-updated", (obj) => {
        vm.eom = obj.end;
        vm.som = obj.start;
        vm.eomTime = new Date(obj.end.toFixed(2) * 1000)
          .toISOString()
          .substr(11, 8);
        vm.somTime = new Date(obj.start.toFixed(2) * 1000)
          .toISOString()
          .substr(11, 8);
        this.$emit("startPosition", obj.start);
        this.$emit("endPosition", obj.end);
      });
      wavesurfer.on("pause", function () {
        vm.LoadAudioInfo();
      });
    },
    LoadAudioInfo() {
      if (wavesurfer.getDuration()) {
        this.TotalTime = new Date(wavesurfer.getDuration().toFixed(2) * 1000)
          .toISOString()
          .substr(11, 8);
        this.CurrentTime = new Date(
          wavesurfer.getCurrentTime().toFixed(2) * 1000
        )
          .toISOString()
          .substr(11, 8);
      } else {
        this.TotalTime = 0;
        this.CurrentTime = 0;
      }
    },
    async LoadAudio() {
      this.InjectWaveSurfer();
      this.SetWaveSurfer();
      let fileUrl = "";
      let waveformUrl = "";
      let downloadUrl = "";
      let userId = sessionStorage.getItem(USER_ID);

      if (this.requestType === "key") {
        fileUrl = `${this.streamingUrl}/${this.fileKey}?userid=${userId}&direct=${this.direct}`;
        waveformUrl = `${this.waveformUrl}/${this.fileKey}?userid=${userId}`;
        downloadUrl = `${this.tempDownloadUrl}/${this.fileKey}`; //인증토큰에 user id가 있어서 전달필요 없음.
      } else {
        fileUrl = `${this.streamingUrl}?token=${this.fileKey}&userid=${userId}&direct=${this.direct}`;
        waveformUrl = `${this.waveformUrl}?token=${this.fileKey}&userid=${userId}`;
        downloadUrl = `${this.tempDownloadUrl}?token=${this.fileKey}`; //인증토큰에 user id가 있어서 전달필요 없음.
      }
      this.LoadDownloadedFile(downloadUrl, waveformUrl, fileUrl);
    },
    LoadDownloadedFile(downloadUrl, waveformUrl, fileUrl) {
      httpClient
        .get(downloadUrl, {
          cancelToken: source.token,
        })
        .then((res) => {
          if (res.status == 200) {
            httpClient
              .get(waveformUrl, {
                cancelToken: source.token,
              })
              .then((res) => {
                if (res.status == 200) {
                  wavesurfer.load(fileUrl, res.data);
                  this.spinnerFlag = false;
                  this.isSuccess = true;
                  this.$emit("isSuccess", this.isSuccess);
                } else {
                  this.$notify(
                    "error",
                    `${res.status} : ${res.statusText}`,
                    res.data,
                    {
                      duration: 10000,
                      permanent: false,
                    }
                  );
                  this.spinnerFlag = false;
                  this.isSuccess = false;
                  this.errorMsg = res.data;
                }
              })
              .catch((error) => {
                console.debug("httpClient", error);
                if (error.response) {
                  this.$notify(
                    "error",
                    `${error.response.status} : ${error.response.statusText}`,
                    error.response.data,
                    {
                      duration: 10000,
                      permanent: false,
                    }
                  );
                } else {
                  console.debug("httpClient.get url:", waveformUrl, error);
                }
                this.spinnerFlag = false;
                this.isSuccess = false;
                this.errorMsg = error.response.data;
              });
          } else {
            this.$notify(
              "error",
              `${res.status} : ${res.statusText}`,
              res.data,
              {
                duration: 10000,
                permanent: false,
              }
            );
            this.spinnerFlag = false;
            this.isSuccess = false;
            this.errorMsg = res.data;
          }
        })
        .catch((error) => {
          console.debug("httpClient", error);
          if (error.response) {
            this.$notify(
              "error",
              `${error.response.status} : ${error.response.statusText}`,
              error.response.data,
              {
                duration: 10000,
                permanent: false,
              }
            );
          } else {
            console.debug("httpClient.get url:", waveformUrl, error);
          }
          this.spinnerFlag = false;
          this.isSuccess = false;
          this.errorMsg = error.response.data;
        });
    },
    Play() {
      if (wavesurfer.isPlaying()) {
        wavesurfer.pause();
      } else {
        wavesurfer.play();
      }
    },
    Stop() {
      source.cancel("Operation canceled by the user.");
      this.TotalTime = wavesurfer.getDuration().toFixed(2);
      this.CurrentTime = (0).toFixed(2);
      wavesurfer.stop();
    },
    close() {
      source.cancel();
      wavesurfer.cancelAjax();
      wavesurfer.destroy();
    },
    changedVolume(v) {
      wavesurfer.setVolume(v);
    },
    changedZoom(v) {
      var zoomLevel = Number(v);
      console.info("zoomLevel", zoomLevel);
      wavesurfer.zoom(zoomLevel);
    },
    zoomInClick() {
      if (this.zoomSliderValue >= this.zoomMax) return;

      this.zoomSliderValue = this.zoomSliderValue + this.zoomInterval;
      wavesurfer.zoom(this.zoomSliderValue);
    },
    zoomOutClick() {
      if (this.zoomSliderValue <= this.zoomMin) return;

      if (this.zoomSliderValue - this.zoomInterval < 0)
        this.zoomSliderValue = 0;
      else this.zoomSliderValue = this.zoomSliderValue - this.zoomInterval;

      wavesurfer.zoom(this.zoomSliderValue);
    },
    tooltipFormatter(v) {
      return (v * 100).toFixed(0);
    },
    zoomTooltipFormatter(v) {
      return "";
    },
    muteToggle() {
      this.isMute = !this.isMute;
      wavesurfer.setMute(this.isMute);
    },
    clearEdit() {
      this.selected = [];
      var endVal = wavesurfer.getDuration();
      wavesurfer.regions.clear();
      this.options.start = 0;
      this.options.end = endVal;
      this.eomTime = new Date(this.options.end.toFixed(2) * 1000);
      this.eomTime = this.eomTime.toISOString().substr(11, 8);
      this.somTime = new Date(this.options.start.toFixed(2) * 1000);
      this.somTime = this.somTime.toISOString().substr(11, 8);
      wavesurfer.addRegion(this.options);
      return endVal;
    },
  },
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
    direct: {
      type: String,
      default: () => {},
    },
    startPoint: Number,
    endPoint: Number,
    fadeIn: Object,
    fadeOut: Object,
    exceptflag : Object,
  },
};
</script>
<style>
.myCol {
  max-width: 4.33333%;
  padding-left: 0px;
  padding-right: 0px;
}
.myCol2 {
  padding-left: 0px;
}
.slider {
  padding-top: 6px;
}
.wavesurfer-region {
  z-index: 3 !important;
}
#editform {
  margin-top: 10px;
  margin-bottom: 50px;
  padding: 15px;
  border: solid 1px #d7d7d7;
  /* background-color: #d7d7d7; */
  text-align: center;
}
.wavesurfer-region {
  background-color: red !important ;
  opacity: 0.2 !important;
}
.iconButton :hover {
  cursor: pointer;
  background: #008ecc;
  color: white;
}
</style>