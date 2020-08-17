<template>
  <div v-show="isShow" class="notification-file-container">
    <transition name="ntf" tag="div">
        <div class="notification notification-info" @click="openUploadPopup()">
           <div class="notification-message">
                {{ getDetail() }}
            </div>
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
            if (successCnt < total) {
                return `(${successCnt}/${total}) 업로드 중......`
            }

            if (successCnt === total) {
                return `(${successCnt}/${total}) 업로드 완료`
            }
        }
    }
}
</script>
