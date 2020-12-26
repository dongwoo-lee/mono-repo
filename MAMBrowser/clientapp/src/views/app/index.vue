<template>
  <app-layout>
    <router-view />
    <!-- 업로딩 토스트 -->
    <file-uploading-toast ref="refFileUploadingToast"></file-uploading-toast>
    <!-- 업로드 팝업 -->
    <file-upload-popup ref="refFileUploadPopup"></file-upload-popup>
    <!-- 파일 메타 데이터 팝업 -->
    <file-meta-data-popup ref="refFileMetaDataPopup"></file-meta-data-popup>
    <!-- 파일 드래그 업로드 폼 -->
    <file-drag-upload-form v-show="isActive"></file-drag-upload-form>
    <!-- 로그인 팝업 -->
    <login-popup ref="refLoginPopup"></login-popup>
    <!-- MY디스크 복사 팝업-->
    <my-disk-copy-looading-overlay></my-disk-copy-looading-overlay>
  </app-layout>
</template>

<script>
import AppLayout from "@/layouts/AppLayout";
import FileDragUploadForm from '@/components/File/FileDragUploadForm';
import FileUploadingToast from '@/components/File/FileUploadingToast';
import FileUploadPopup from '@/components/File/FileUploadPopup';
import FileMetaDataPopup from '@/components/File/FileMetaDataPopup';
import FileUploadRefElement from '@/components/File/FileUploadRefElement';
import LoginPopup from '@/components/Popup/Login/LoginPopup';
import LoginPopupRefElement from '@/components/Popup/Login/LoginPopupRefElement';
import MyDiskCopyLooadingOverlay from '@/components/MyDiskCopyLooadingOverlay';
import { ROUTE_NAMES } from '@/constants/config';
import { mapMutations } from 'vuex';

export default {
  components: {
    "app-layout": AppLayout,
    FileDragUploadForm,
    FileUploadingToast,
    FileUploadPopup,
    FileMetaDataPopup,
    LoginPopup,
    MyDiskCopyLooadingOverlay
  },
  watch: {
    '$route': {
      handler(to, from) {
        const routeName = this.$route.name;
        if (routeName && (routeName === ROUTE_NAMES.PRIVATE || routeName === ROUTE_NAMES.SHARED)) {
          this.isActive = true;
        } else {
          this.isActive = false;
        }
      },
      immediate: true,
    }
  },
  mounted() {
    this.$nextTick(() => {
      FileUploadRefElement.init(this);
      LoginPopupRefElement.init(this);
      this.SET_DOWNLOAD_IFRAME();
    });
  },
  methods: {
    ...mapMutations('file', ['SET_DOWNLOAD_IFRAME'])
  }
};
</script>
