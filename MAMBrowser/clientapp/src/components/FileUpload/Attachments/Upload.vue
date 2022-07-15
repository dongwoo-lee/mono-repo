<template>
  <div id="webcuesheet_fileupload">
    <div>
      <DxFileUploader
        :chunk-size="chunkSize"
        labelText="첨부파일"
        select-button-text="파일 업로드"
        name="file"
        :accept="accept"
        :upload-url="getUrl"
        :upload-custom-data="uploaderCustomData"
        @upload-started="onUploadedStarted"
        @value-changed="onValueChanged"
        @option-changed="onOptionChanged"
        @uploaded="onUploaded"
      />
    </div>
    <div class="chunk-panel">
      <div v-for="(file, index) in files" :key="index">
        <span class="segment-size dx-theme-accent-as-text-color">{{
          file
        }}</span>
        <span id="file_manager_btn">
          <DxButton
            :width="120"
            icon="download"
            styling-mode="outlined"
            text="다운로드"
          />
          <DxButton
            :width="100"
            icon="remove"
            styling-mode="outlined"
            text="삭제"
            @click="onFileDelete(file)"
          />
        </span>
      </div>
    </div>
  </div>
</template>

<script>
import DxFileUploader from "devextreme-vue/file-uploader";
import DxButton from "devextreme-vue/button";
export default {
  data() {
    return {
      files: [],
      uploaderCustomData: {
        serviceName: "테스트 프로그램",
        brd_date: "20220707",
      }, //이후에 props로 바꾸기
      chunkSize: 1,
      accept:
        "application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,text/plain,.pdf,.docx,.hwp",
      getUrl: "/api/CueAttachments/chunkFileUpload",
    };
  },
  components: {
    DxFileUploader,
    DxButton,
  },
  mounted() {},
  methods: {
    onFileDelete(file) {},
    onUploadedStarted(e) {},
    onOptionChanged(e) {},
    onValueChanged(e) {},
    onUploaded(e) {
      var file = {};
      file.token = JSON.parse(e.request.response).token;
      //JSON.parse(e.request.response).token //token 가져오면 넣어주기
      this.files.push(file);
    },
  },
};
</script>

<style>
#webcuesheet_fileupload .dx-fileuploader-wrapper {
  padding: 10px 10px 0px 10px;
}
#webcuesheet_fileupload .dx-fileuploader-input-wrapper {
  border: 1px solid #d7d7d7;
}
#webcuesheet_fileupload .chunk-panel {
  overflow: auto;
  height: 140px;
  padding: 10px;
  margin: 0px 10px 10px 10px;
  border-bottom: 1px solid #d7d7d7;
  border-right: 1px solid #d7d7d7;
  border-left: 1px solid #d7d7d7;
}
#webcuesheet_fileupload .dx-fileuploader-files-container {
  padding: 0px;
  border-right: 1px solid #d7d7d7;
  border-left: 1px solid #d7d7d7;
}
#webcuesheet_fileupload .dx-fileuploader-file-container {
  padding: 10px 10px 0px 10px;
}
#webcuesheet_fileupload .dx-fileuploader-input-wrapper .dx-button {
  float: right;
}
#webcuesheet_fileupload .dx-fileuploader-button {
  margin: 3px 10px 0px 0px;
}
#webcuesheet_fileupload .dx-fileuploader-input-label {
  padding: 8px 15px;
}
</style>