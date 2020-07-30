<template>
<div>
    <b-card class="mb-4" title="파일 업로드">
        <b-button class="mb-1" variant="primary default" size="sm" @click="onPopupFileUpload">파일 업로드</b-button>
        <multi-file-upload-popup :show="show" @close="show=false"></multi-file-upload-popup>
    </b-card>
</div>
</template>
<script>
import MultiFileUploadPopup from '../../../components/popup/MultiFileUploadPopup';
export default {
    components: { MultiFileUploadPopup },
    data() {
        return {
            show: false,
        }
    },
    methods: {
        onPopupFileUpload(event) {
            const bodyElem = window.document.querySelector('body');
            // bodyElem.classList.add('')
            console.info('this.body', event, bodyElem);
            this.show = true;
        }
    }
}
</script>