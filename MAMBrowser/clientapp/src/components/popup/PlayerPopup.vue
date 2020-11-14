<template>
<!-- 미리듣기 팝업 --> 
<b-modal id="modal-player" size="lg" 
    v-model="show" 
    no-close-on-backdrop>
    <template slot="modal-title" >
    <h5>{{title}}</h5>
    </template>
    <template slot="default" >
    <Player 
        ref="play"
        :requestType="requestType" 
        :fileKey = "fileKey"
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
        @click="show=false"
    >
    닫기</b-button>
    </template>
</b-modal>
</template>
<script>
export default {
    props:{
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
            default: () => 'N',
        }
    },
    computed: {
        show: {
            get() {
                return this.showPlayerPopup;
            },
            set(v) {
                if (!v) {
                    this.closePlayer();
                }
            }
        },
    },
    methods: {
        closePlayer(){
            this.$refs.play.close();
            this.$emit('closePlayer');
        }
    },
}
</script>