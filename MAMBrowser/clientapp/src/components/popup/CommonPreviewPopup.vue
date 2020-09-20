<template>
    <b-modal
        title="미리듣기"
        size="sm"
        v-model="showDialog">
          <div id='waveform'>
          </div>
          <b-btn variant="info btn-fill" size="sm" @click.prevent="Play" >재생</b-btn>
          <b-btn variant="info btn-fill ml-3"  size="sm" @click.prevent="Stop">정지</b-btn>
          <b-btn variant="info btn-fill ml-3"  size="sm" @click.prevent="LoadAudio">로드 오디오</b-btn>
    미리듣기 팝업 내용(들어오는 데이터)<br>
    {{item}}
    </b-modal>
</template>
<script>
import WaveSurfer from 'wavesurfer.js';
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
      CurrentTime : 0,
      TotalTime : 0,
      loadComplete:false,
    }
  },
  mounted() {
    
  },
  beforeDestroy(){
    this.Stop();
    wavesurfer.cancelAjax();
    wavesurfer.destroy();
  },
  methods : {
    InjectWaveSurfer() {
      wavesurfer = WaveSurfer.create({
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
        this.TotalTime = wavesurfer.getDuration().toFixed(2);
        this.CurrentTime = wavesurfer.getCurrentTime().toFixed(2);
    },
    LoadAudio(){
        this.InjectWaveSurfer();
    this.SetWaveSurfer();
      let url ='/api/products/workspace/private/files/262';
      let url2 ='/api/products/workspace/private/waveform/262';
      //     console.info('this.returnData',this.returnData);

      httpClient.get(url2,null).then(res=>
      {
        console.info('res.data', res.data);
        wavesurfer.load(url, res.data);
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