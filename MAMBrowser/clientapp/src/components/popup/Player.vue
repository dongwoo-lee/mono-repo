<template>
<div>
    <div>
      <h1>{{item.title}} : {{item.memo}}</h1>
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
                key: "Authorization",
                value: "Bearer " + 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2Nlc3NJbmZvIjp7IklEIjoicmFkaW9lbmciLCJSb2xlSUQiOiJSX1NZU19TVVBFUiIsIkF1dGhvckNEIjoiUzAxRzA0QzAwMyIsIkF1dGhvck5hbWUiOiLsi5zsiqTthZwg6rSA66as7J6QIiwiTWVudUdycElEIjoiUzAxRzA1QzAwMSJ9LCJuYmYiOjE1OTY0NDA3MTAsImV4cCI6MTU5NjQ0NDMxMCwiaWF0IjoxNTk2NDQwNzEwLCJpc3MiOiJNQU0ifQ.21U1T0q5iz2X9Cr_n6SVy_dLaM9EIm-ycPD5f_DiNkg'
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
        let url =`/api/products/workspace/private/streaming/${this.item.seq}`;
        let url2 =`/api/products/workspace/private/waveform/${this.item.seq}`;
        //     console.info('this.returnData',this.returnData);

        httpClient.get(url2,null).then(res=>
        {
          wavesurfer.load(url, res.data);
          this.spinnerFlag = false;
          this.isSuccess = true;
        });
        // });
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
        show: {
            type: Boolean,
            default: false,
        },
        item: {
            type: Object,
            default: () => {},
        }
    },
    computed: {
        showDialog: {
            get() {
                return this.show;
            },
            set(v) {
                if (!v) {
                    this.$emit('close');
                }
            }
        },
    },
}
</script>