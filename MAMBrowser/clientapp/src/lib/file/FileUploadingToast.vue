<template>
  <div v-show="isShow" class="notification-file-container">
    <transition name="ntf" tag="div">
        <div class="notification notification-info" @click="openUploadPopup()">
           <div class="notification-message" v-html="getDetail()"></div>
        </div>
    </transition>
  </div>
</template>
<script>
import { mapGetters, mapActions } from 'vuex'
import FileUploadRefElement from './FileUploadRefElement';

export default {
    data() {
        return {
            toast: false,
        }
    },
    computed: {
        ...mapGetters('file', ['getFileData']),
        isShow() {
            return this.toast && this.getFileData.length > 0;
        }
    },
    methods: {
        ...mapActions('file', ['open_popup']),
        openUploadPopup() {
            this.open_popup();
        },
        show() {
            this.toast = true;
        },
        close() {
            this.toast = false;
        },
        getDetail() {
            const total = this.getFileData.length;
            const successCnt = this.getFileData.filter(file => file.file.success).length;
            const isSaving = this.getFileData.some(file => file.uploadState === 'save');
            if (successCnt < total) {
                return `(${successCnt}/${total}) 업로드 중......`;
            }

            if (isSaving) {
                return `(${successCnt}/${total}) 파일 저장중입니다.<br/> 잠시만 기다려주세요.`;
            }

            if (successCnt === total) {
                return `(${successCnt}/${total}) 업로드 완료`;
            }
        }
    }
}
</script>
