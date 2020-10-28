<template>
<!-- 미리듣기 팝업 --> 
<b-modal id="modal-player" size="lg" v-model="showPreviewPopup" no-close-on-backdrop no-close-on-esc>
    <template slot="modal-title" >
    <h5>미리듣기</h5>
    </template>
    <template slot="default" >
    <Player 
        :item="previewItem" 
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
        previewItem : {
            type: Object,
            default: () => {}
        },
        showPreviewPopup :{
            type: Boolean,
            default: () =>false
        }
    },
    methods: {
        display(value) {
            return this.behaviorData.some(data => data.id === value && data.visible === 'Y');
        },
        displayEtc(value) {
            return this.etcData.some(data => data === value);
        },
        onPreview() {
            this.$emit('preview', this.rowData);
        },
        onDownload() {
            this.$emit('download', this.rowData);
        },
        onDelete() {
            this.$emit('delete', this.rowData);
        },
        onMetaModify() {
            this.$emit('modify', this.rowData);
        },
        closePlayer(){
            this.$emit('closePreview');
            // this.$bvModal.hide('modal-player')
        }
    },
}
</script>