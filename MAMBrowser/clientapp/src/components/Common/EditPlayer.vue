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
            <b-col cols="3" class="p-0 mb-2">
              <p align="left">
                <b-button-group size="sm">
                  <b-button
                    @click="somClick"
                    :disabled="cueInfo.cuetype == 'A'"
                  >
                    Start Point
                  </b-button>
                  <b-button
                    @click="eomClick"
                    :disabled="cueInfo.cuetype == 'A'"
                  >
                    End Point
                  </b-button>
                </b-button-group>
              </p>
            </b-col>
            <b-col cols="4" class="pl-3">
              <p align="left" style="margin-top: 8px; margin-bootm: 0">
                <b-form-checkbox-group
                  class="custom-checkbox-group"
                  v-model="selected"
                  :options="fadeOptions"
                  value-field="value"
                  text-field="text"
                  :disabled="cueInfo.cuetype == 'A'"
                />
              </p>
            </b-col>
            <b-col cols="5">
              <div>
                <div class="iconButton" style="float: right; margin-left: 10px">
                  <!-- <b-button size="sm" variant="outline-primary"> -->
                  <b-icon
                    icon="zoom-in"
                    class=""
                    style="width: 22px; height: 22px; padding: 1px"
                    @click="zoomInClick"
                  ></b-icon>
                  <!-- </b-button> -->
                </div>
                <div style="float: right">
                  <vue-slider
                    width="150px"
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
                  <b-icon
                    icon="zoom-out"
                    class=""
                    style="width: 22px; height: 22px; padding: 1px"
                    @click="zoomOutClick"
                  ></b-icon>
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
                width="150px"
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
              <b-btn variant="outline-primary" size="sm" @click.prevent="Play"
                >▶/||</b-btn
              >
              <b-btn variant="outline-primary" size="sm" @click.prevent="Stop"
                >■</b-btn
              >
              <b-btn variant="outline-primary" size="sm" @click="editplay"
                >영역 Play</b-btn
              >
            </div>
          </b-col>
          <b-col>
            <div>
              <p align="right" class="m-0">{{ somTime }} / {{ eomTime }}</p>
              <p align="right" class="m-0">
                {{ CurrentTime }} / {{ TotalTime }}
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
          text: "Fade In",
          value: { fadeInValue: true },
        },
        { text: "Fade Out", value: { fadeOutValue: true } },
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
    if (this.fadeIn["fadeInValue"]) {
      this.selected.push(this.fadeIn);
    }
    if (this.fadeOut["fadeOutValue"]) {
      this.selected.push(this.fadeOut);
    }
    cancelToken = axios.CancelToken;
    source = cancelToken.source();
    this.LoadAudio();
  },
  computed: {
    ...mapGetters("cueList", ["cueInfo"]),
  },
  methods: {
    editplay() {
      wavesurfer.regions.list["Trim"].play();
    },
    somClick() {
      //console.log(wavesurfer.regions.list.Trim);
      this.options.start = wavesurfer.getCurrentTime();
      this.options.end = wavesurfer.regions.list.Trim.end;
      wavesurfer.regions.clear();
      wavesurfer.addRegion(this.options);

      this.somTime = new Date(
        wavesurfer.regions.list.Trim.start.toFixed(2) * 1000
      );
      this.somTime = this.somTime.toISOString().substr(11, 8);
      this.$emit("startPosition", wavesurfer.regions.list.Trim.start);
    },
    eomClick() {
      this.options.start = wavesurfer.regions.list.Trim.start;
      this.options.end = wavesurfer.getCurrentTime();
      wavesurfer.regions.clear();
      wavesurfer.addRegion(this.options);
      this.eomTime = new Date(
        wavesurfer.regions.list.Trim.end.toFixed(2) * 1000
      );
      this.eomTime = this.eomTime.toISOString().substr(11, 8);
      this.$emit("endPosition", wavesurfer.regions.list.Trim.end);
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
      wavesurfer.on("interaction", function () {
        vm.LoadAudioInfo();
      });
      wavesurfer.on("finish", function () {
        vm.LoadAudioInfo();
      });
      wavesurfer.on("seek", (e) => {
        if (wavesurfer.regions.getCurrentRegion() != null) {
          wavesurfer.play(
            wavesurfer.getCurrentTime(),
            wavesurfer.regions.getCurrentRegion().end
          );
        }
      });
      wavesurfer.on("ready", () => {
        var totalSec = wavesurfer.getDuration();
        if (totalSec >= 3000) {
          vm.zoomMax = 100;
          vm.zoomMin = 0;
          vm.zoomInterval = 10;
        } else if (totalSec >= 60) {
          vm.zoomMax = 180;
          vm.zoomInterval = 6;
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
      // if(this.direct =="Y"){
      // this.LoadDirect(waveformUrl, fileUrl);     //서버에서 막혀있음.
      // }
      // else{
      this.LoadDownloadedFile(downloadUrl, waveformUrl, fileUrl);
      // }
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
    LoadDirect(waveformUrl, fileUrl) {
      httpClient
        .get(waveformUrl, {
          cancelToken: source.token,
        })
        .then((res) => {
          wavesurfer.load(fileUrl, res.data);
          this.spinnerFlag = false;
          this.isSuccess = true;
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
  border: solid #008ecc 1px;
}
</style>