<template>
<!-- 미리듣기 팝업 --> 
<b-modal id="modal-player" size="xl" v-model="showPlayerPopup" no-close-on-backdrop no-close-on-esc>
    <template slot="modal-title" >
    <h5>미리듣기</h5>
    </template>
    <template slot="default" >
    <b-row>
    <b-col cols="9"> 
    <slot name="top">
    </slot>
    </b-col>
    <b-col cols="3">
        <slot name="right">
        </slot>
    </b-col>
    </b-row>
    <Player 
        :httpMethod="httpMethod" 
        :params = "params"
        :title = "title"
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
    props:{
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
        showPlayerPopup : {
            type: Boolean,
            default: () => false,
        },
        direct : {
            type: String,
            default: () => {},
        }
    },
    methods: {
        closePlayer(){
            this.$bvModal.hide('modal-player')
            this.$emit('closePlayer');
        }
    },
}
</script>