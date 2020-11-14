<template>
<!-- 미리듣기 팝업 --> 
<b-modal id="music-player" size="xl" v-model="showPlayerPopup" no-close-on-backdrop >
    <template slot="modal-title" >
    <h5>{{this.music.name}}</h5>
    </template>
    <template slot="default" >
    <b-row>
    <b-col cols="9"> 
     image
    </b-col>
    <b-col cols="3">
        lyrics
    </b-col>
    </b-row>
    <Player 
        :requestType="requestType" 
        :fileKey = "music.fileToken"
        :streamingUrl = "streamingUrl"
        :waveformUrl = "waveformUrl"
        :direct = "direct"
    />
    </template>
    <template v-slot:modal-footer>
    <b-button
        variant="outline-danger default cutom-label-cancel"
        size="sm"
        class="float-right"
        @click="closePlayer()"
    >
    닫기</b-button>
    </template>
</b-modal>
</template>
<script>
export default {
    data () {
        return {
            imageListUrl : 'albums/images-path',
            imageUrl : 'albums/images/files',
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
    watch: {
        showPlayerPopup(isShow) {
            if (isShow) {
                // open
                console.info('showPlayerPopup open');
                this.GetAlumbImageAndLyrics();
            } else {
                // close
                console.info('showPlayerPopup close');
            }
        }
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
            this.$emit('closePlayer');
        }
    },
}
</script>