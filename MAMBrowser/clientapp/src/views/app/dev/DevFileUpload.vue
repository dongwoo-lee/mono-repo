<template>
<div>
    <b-card class="mb-4" title="파일 업로드">
        <b-button class="mb-1" variant="primary default" size="sm" @click="show = true">파일 업로드</b-button>
        <multi-file-upload-popup :show="show" @close="show=false"></multi-file-upload-popup>
    </b-card>
</div>
</template>
<script>
export default {
    data() {
        return {
            show: false,
        }
    },
}
</script>