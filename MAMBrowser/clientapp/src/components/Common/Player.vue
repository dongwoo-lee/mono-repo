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
            <b-col> </b-col>
            <b-col>
              <div align="center"></div>
            </b-col>
            <b-col>
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
            </div>
          </b-col>
          <b-col>
            <p align="right">{{ CurrentTime }} / {{ TotalTime }}</p>
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
    };
  },
  mounted() {
    cancelToken = axios.CancelToken;
    source = cancelToken.source();
    this.LoadAudio();
  },
  computed: {},
  methods: {
    InjectWaveSurfer() {
      wavesurfer = WaveSurfer.create({
        plugins: [
          TimelinePlugin.create({
            container: "#wave-timeline",
          }),
        ],
        container: "#waveform",
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
      wavesurfer.on("ready", function () {
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
    LoadAudio() {
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
          headers: {
            "X-Csrf-Token": sessionStorage.getItem(ACCESS_TOKEN),
          },
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
.iconButton :hover {
  cursor: pointer;
  background: #008ecc;
  color: white;
  border: solid #008ecc 1px;
}
</style>