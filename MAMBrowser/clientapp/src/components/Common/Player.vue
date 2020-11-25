<template>
<div>
    <div>
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
var httpClient;
var cancelToken;
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
    cancelToken = axios.CancelToken;
    source = cancelToken.source();
    this.LoadAudio();
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
        xhr:{
          requestHeaders: [{
            key: "X-Csrf-Token",
            value: sessionStorage.getItem('access_token')
          }]
        }
      })
      httpClient = axios.create({
        baseURL: process.env.VUE_APP_API_BASEURL,
        withCredentials: false,
        headers:{
            'Content-Type': 'application/json',
            'X-Csrf-Token': sessionStorage.getItem('access_token')
            },
            timeout:80000
      });
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
      if (wavesurfer.getDuration()) {
        this.TotalTime = new Date(wavesurfer.getDuration().toFixed(2) * 1000).toISOString().substr(11, 8);  
        this.CurrentTime = new Date(wavesurfer.getCurrentTime().toFixed(2) * 1000).toISOString().substr(11, 8);
      } else {
        this.TotalTime = 0;
        this.CurrentTime = 0;
      }
    },
    LoadAudio(){
      this.InjectWaveSurfer();
      this.SetWaveSurfer();
      let fileUrl ='';
      let waveformUrl ='';
      let downloadUrl ='';
      let userId =  sessionStorage.getItem('user_id');

      if(this.requestType === "key")
      {
        fileUrl =`${this.streamingUrl}/${this.fileKey}?userid=${userId}&direct=${this.direct}`;
        waveformUrl =`${this.waveformUrl}/${this.fileKey}?userid=${userId}`;
        downloadUrl =`${this.tempDownloadUrl}/${this.fileKey}`; //인증토큰에 user id가 있어서 전달필요 없음.
      }
      else
      { 
        fileUrl =`${this.streamingUrl}?token=${this.fileKey}&userid=${userId}&direct=${this.direct}`;
        waveformUrl =`${this.waveformUrl}?token=${this.fileKey}&userid=${userId}`;
        downloadUrl =`${this.tempDownloadUrl}?token=${this.fileKey}`; //인증토큰에 user id가 있어서 전달필요 없음.
      }
      // if(this.direct =="Y"){
      //   this.LoadDirect(waveformUrl, fileUrl);     //일단... 다이렉트는 FTP프로토콜(x), MP2(x)
      // }
      // else{
        this.LoadDownloadedFile(downloadUrl, waveformUrl, fileUrl);
      // }
     
    },
    LoadDownloadedFile(downloadUrl,waveformUrl,fileUrl){
      
      httpClient.get(downloadUrl, {
        cancelToken: source.token,
        headers:{
          'X-Csrf-Token': sessionStorage.getItem('access_token'),
        }
      }).then(res=>{
        console.info('tempdownload status', res.status);
        if(res.status == 200){
          httpClient.get(waveformUrl, {
          cancelToken: source.token,
          }).then(res=>{
            wavesurfer.load(fileUrl, res.data);
            this.spinnerFlag = false;
            this.isSuccess = true;
          }).catch(error=>{
              console.debug('httpClient', error)
              if (error.response){
                this.$notify("error", `${error.response.status} : ${error.response.statusText}` , error.response.data, {
                    duration: 8000,
                    permanent: false
                });
              } else {
                console.debug('httpClient.get url:', waveformUrl, error);
              }
          });;
        }
      });
    },
    LoadDirect(waveformUrl, fileUrl){
      httpClient.get(waveformUrl, {
        cancelToken: source.token,
      }).then(res=>{
          wavesurfer.load(fileUrl, res.data);
          this.spinnerFlag = false;
          this.isSuccess = true;
        }).catch(error=>{
          console.debug('httpClient', error)
            if (error.response){
              this.$notify("error", `${error.response.status} : ${error.response.statusText}` , error.response.data, {
                  duration: 8000,
                  permanent: false
              });
            } else {
              console.debug('httpClient.get url:', waveformUrl, error);
            }
        });;
    },
    Play(){
      wavesurfer.play();
    },
    Stop(){
      source.cancel('Operation canceled by the user.');
      this.TotalTime = wavesurfer.getDuration().toFixed(2);
      this.CurrentTime = (0).toFixed(2);
      wavesurfer.stop();
    },
    close() {
      source.cancel();
      wavesurfer.cancelAjax();
      wavesurfer.destroy();
    }
  },
  props: {
      requestType : {
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
      tempDownloadUrl :{
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