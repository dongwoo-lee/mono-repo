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
    <file-drag-upload-form></file-drag-upload-form>
  </app-layout>
</template>

<script>
import AppLayout from "../../layouts/AppLayout";
import FileDragUploadForm from '../../lib/file/FileDragUploadForm';
import fileUploadingToast from '../../lib/file/FileUploadingToast';
import fileUploadPopup from '../../lib/file/FileUploadPopup';
import FileUploadRefElement from '../../lib/file/FileUploadRefElement';
import FileMetaDataPopup from '../../lib/file/FileMetaDataPopup';

export default {
  components: {
    "app-layout": AppLayout,
    "FileDragUploadForm": FileDragUploadForm,
    "fileUploadingToast": fileUploadingToast,
    "fileUploadPopup": fileUploadPopup,
    "FileMetaDataPopup": FileMetaDataPopup,
  },
  mounted() {
    this.$nextTick(() => {
      FileUploadRefElement.init(this);
    });
  },
};
</script>
