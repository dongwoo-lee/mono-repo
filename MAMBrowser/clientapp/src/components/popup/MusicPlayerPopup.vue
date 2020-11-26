<template>
<!-- 미리듣기 팝업 --> 
<b-modal id="music-player" size="xl" v-model="show" no-close-on-backdrop >
    <template slot="modal-title" >
    <h5>{{this.music.name}}</h5>
    </template>
    <template slot="default" >
    <b-row>
    <b-col cols="9"> 
     개선중
    </b-col>
    <b-col cols="3">
        개선중
    </b-col>
    </b-row>
    <Player
        ref="play"
        :requestType="requestType" 
        :fileKey = "music.fileToken"
        :streamingUrl = "streamingUrl"
        :waveformUrl = "waveformUrl"
        :tempDownloadUrl="tempDownloadUrl"
        :direct = "direct"
    />
    </template>
    <template v-slot:modal-footer>
    <b-button
        variant="outline-danger default cutom-label-cancel"
        size="sm"
        class="float-right"
        @click="show=false"
    >
    닫기</b-button>
    </template>
</b-modal>
</template>
<script>
export default {
    data () {
        return {
            tempImageDownloadUrl : '/api/musicsystem/temp-image-download',
            lyricsUrl : 'lyrics',
            imagePathTokenList : [],
            ui:{
                imageList:[],
                lyrics:''
            },
        }
    },
    props:{
        requestType : {
            type: String,
            default: () => {},
        },
        music:{
            type: Object,
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
        showPlayerPopup : {
            type: Boolean,
            default: () => false,
        },
        direct : {
            type: String,
            default: () => {},
        },
    },
    computed: {
        show: {
            get() {
                return this.showPlayerPopup;
            },
            set(v) {
                if (!v) {
                    this.closePlayer();
                } else {
                    console.info('showPlayerPopup open'); 
                    this.GetAlumbImageAndLyrics();
                }
            }
        },
    },
    methods: {
        GetAlumbImageAndLyrics(){
            //작업중. 
            this.imageList = [];
            let params ={
                token : this.music.fileToken,
                albumToken : this.music.albumToken
            }
            // this.$http.get(`/api/musicsystem/${this.imageListUrl}`, { params: params, })
            // .then(res => {

            //     res.data.forEach(imagePathToken => {
            //         this.$http.get(`/api/musicsystem/${this.imageUrl}?albumToken=${this.music.albumToken}`, { 
            //             responseType: 'arraybuffer'
            //         })
            //         .then(res => {
            //             this.ui.imageList.push(Buffer.from(response.data, 'binary'));
            //         });        
            //     });

            // });

            // this.$http.get(`/api/musicsystem/${this.lyricsUrl}?seq=${this.music.seq}`, null)
            //     .then(res => {
            //     this.ui.lyrics = res.data;
            // });   
            console.info('loading music player');
        },
        closePlayer(){
            this.$refs.play.close();
            this.$emit('closePlayer');
        }
    },
}
</script>