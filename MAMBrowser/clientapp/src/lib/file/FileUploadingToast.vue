<template>
  <div v-show="isShow" class="notification-file-container">
    <transition name="ntf" tag="div">
        <div class="notification notification-info" @click="openUploadPopup()">
           <div class="notification-message">
               파일 업로드 중....
            </div>
        </div>
    </transition>
  </div>
</template>
<script>
import { mapGetters, mapActions } from 'vuex'
import FileUploadModel from './FileUploadModel';

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
        }
    }
}
</script>
