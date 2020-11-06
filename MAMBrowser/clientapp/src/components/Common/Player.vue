<template>
<div>
    <div>
      <h1> {{title}} </h1>
      <div class="text-center" v-if="spinnerFlag" >
        <b-spinner style="width: 4rem; height: 4rem;" variant="primary"></b-spinner>
      </div>
      <div id="waveform" >
      </div>
        <div id='wave-timeline'>
      </div>
      <br>
      <br>
      <b-container class="bv-example-row" v-if="isSuccess">
        <b-row>
          <b-col>
          </b-col>
          <b-col>
            <div align="center" >
             <b-btn variant="outline-primary" size="sm" @click.prevent="Play" >▶</b-btn>
            <b-btn variant="outline-primary"  size="sm" @click.prevent="Stop">■</b-btn>
            </div>
          </b-col>
          <b-col >
            <p align="right" >{{CurrentTime}} / {{TotalTime}}</p>
          </b-col>
        </b-row>
      </b-container>
    </div>
  </div>
</template>

<script>
import WaveSurfer from 'wavesurfer.js';
import TimelinePlugin from 'wavesurfer.js/dist/plugin/wavesurfer.timeline.min.js';

import axios from 'axios';
var wavesurfer;
const httpClient = axios.create({
        baseURL: process.env.VUE_APP_API_BASEURL,
        withCredentials: false,
        haaders:{
            'Content-Type': 'application/json',
            },
            timeout:80000
        });

var CancelToken;
var source;


export default {
    data () {
    return {
      wavesurfer: null,
      CurrentTime : '00:00:00',
      TotalTime : '00:00:00',
      loadComplete:false,
      isSuccess : false,
      spinnerFlag : true
    }
  },
  mounted() {
    CancelToken = axios.CancelToken;
    source = CancelToken.source();
    this.LoadAudio();
  },
  beforeDestroy(){
    this.Stop();
    wavesurfer.cancelAjax();
    wavesurfer.destroy();
  },
  methods : {
    InjectWaveSurfer() {
      wavesurfer = WaveSurfer.create({
                plugins: [
                 TimelinePlugin.create({
                      container:"#wave-timeline"
                  }),
                ],
                container: '#waveform',
                waveColor: 'gray',
                progressColor: 'skyblue',
                backend: 'MediaElementWebAudio',
                // splitChannels: true,
                xhr:{requestHeaders: [{
                key: "X-Csrf-Token",
                value: sessionStorage.getItem('access_token')
                }]}
                })
    },
    SetWaveSurfer() {
      let vm = this;
      wavesurfer.on('audioprocess', function() {
        if(wavesurfer.isPlaying()) { 
            vm.LoadAudioInfo();
          }
        });
      wavesurfer.on('interaction', function() {
        vm.LoadAudioInfo();
      });
      wavesurfer.on('finish', function() {
          vm.LoadAudioInfo();
      });
      wavesurfer.on('ready', function() {
          vm.LoadAudioInfo();
          vm.Play();
      });
      wavesurfer.on('pause', function() {
          vm.LoadAudioInfo();
      });
    },
    LoadAudioInfo(){
      this.TotalTime = new Date(wavesurfer.getDuration().toFixed(2) * 1000).toISOString().substr(11, 8);
      this.CurrentTime = new Date(wavesurfer.getCurrentTime().toFixed(2) * 1000).toISOString().substr(11, 8);
    },
    LoadAudio(){
      this.InjectWaveSurfer();
      this.SetWaveSurfer();
      if(this.httpMethod === "post")
      {
        console.info('post');
        httpClient.post(this.waveformUrl, { params : this.params}).then(res=>
        {
          wavesurfer.load(this.streamingUrl, res.data);
          this.spinnerFlag = false;
          this.isSuccess = true;
        });
      }
      else
      {
        let url =`${this.streamingUrl}/${this.params}?direct=${this.direct}`;
        let url2 =`${this.waveformUrl}/${this.params}`;
        httpClient.get(url2, null).then(res=>
        {
          wavesurfer.load(url, res.data);
          this.spinnerFlag = false;
          this.isSuccess = true;
        });
      }
    },
    Play(){
      wavesurfer.play();
    },
    Stop(){
      wavesurfer.stop();
       this.TotalTime = wavesurfer.getDuration().toFixed(2);
      this.CurrentTime = (0).toFixed(2);
    },
  },
  props: {
      httpMethod : {
          type: String,
          default: () => {},
      },
      params: {
          type: Object,
      },
      title: {
          type: String,
          default: () => {},
      },
      streamingUrl :{
          type: String,
          default: () => {},
      },
      waveformUrl :{
          type: String,
          default: () => {},
      },
      direct : {
        type: String,
        default: () => {},
      }
  },
}
</script>