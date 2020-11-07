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
  </app-layout>
</template>

<script>
import AppLayout from "../../layouts/AppLayout";
import FileDragUploadForm from '../../lib/file/FileDragUploadForm';
import fileUploadingToast from '../../lib/file/FileUploadingToast';
import fileUploadPopup from '../../lib/file/FileUploadPopup';
import FileMetaDataPopup from '../../lib/file/FileMetaDataPopup';
import LoginPopup from '../../lib/loginPopup/LoginPopup';
import FileUploadRefElement from '../../lib/file/FileUploadRefElement';
import LoginPopupRefElement from '../../lib/loginPopup/LoginPopupRefElement';

export default {
  components: {
    "app-layout": AppLayout,
    "FileDragUploadForm": FileDragUploadForm,
    "fileUploadingToast": fileUploadingToast,
    "fileUploadPopup": fileUploadPopup,
    "FileMetaDataPopup": FileMetaDataPopup,
    "LoginPopup": LoginPopup,
  },
  watch: {
    '$route': {
      handler(to, from) {
        const routeName = this.$route.name;
        if (routeName && (routeName === 'private' || routeName === 'public')) {
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
    });
  },
};
</script>
