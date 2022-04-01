<template>
  <div>
    <div>
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
            <b-col cols="5">
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
import SectionsPlugin from "../WaveSurferPlugin/SectionsPlugin";
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

      groupData: [],
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
          SectionsPlugin.create({
            sections: [],
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

      this.LoadDownloadedFile();
    },
    LoadDownloadedFile() {
      let fileRequestUrl = "/api/products/concatenate-files-request";
      let waveformUrl = "/api/products/concatenate-files-waveform";
      let streamingUrl = "/api/products/concatenate-files-streaming";
      let fileRequestparam = {
        grpType: this.grpType,
        brd_Dt: this.brd_Dt,
        grpId: this.grpId,
      };
      let fileName = "";
      const userId = sessionStorage.getItem(USER_ID);

      httpClient
        .get(fileRequestUrl, {
          cancelToken: source.token,
          headers: {
            "X-Csrf-Token": sessionStorage.getItem(ACCESS_TOKEN),
          },
          params: fileRequestparam,
        })
        .then((res) => {
          if (res.status == 200) {
            //  this.groupData = res.data.resultObject.data.groupData;

            // res.data.resultObject.data.groupData.forEach(dt => {
            // this.groupData.push({startTime:dt.startTime, endTime:dt.endTime, label:dt.title});
            // console.info('wavesurfer', wavesurfer);

            // wavesurfer.sections.sections.push({startTime:dt.startTime, endTime:dt.endTime, label:dt.title});
            // console.info('sectionsPlugin',this.sectionsPlugin);
            // });

            fileName = res.data.resultObject.data.fileName;
            httpClient
              .get(waveformUrl + `?fileName=${fileName}`, {
                cancelToken: source.token,
                headers: {
                  "X-Csrf-Token": sessionStorage.getItem(ACCESS_TOKEN),
                },
              })
              .then((resWaveForm) => {
                this.spinnerFlag = false;
                this.isSuccess = true;
                wavesurfer.load(
                  streamingUrl + `?userid=${userId}&fileName=${fileName}`,
                  resWaveForm.data
                );
                res.data.resultObject.data.groupData.forEach((dt) => {
                  wavesurfer.addSection({
                    startTime: dt.startTime,
                    endTime: dt.endTime,
                    label: dt.title,
                  });
                });
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
  props: ["title", "grpId", "grpType", "brd_Dt"],
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

.wavesurfer-section > div {
  background-color: #ceeace !important;
  font-size: 12px;
  padding-left: 10px;
  padding-right: 10px;
  line-height: 25px;
  border-top-style: none !important;
  border-bottom-style: none !important;
}
.iconButton :hover {
  cursor: pointer;
  background: #008ecc;
  color: white;
}
</style>