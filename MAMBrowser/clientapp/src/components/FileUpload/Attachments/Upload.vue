<template>
  <div id="webcuesheet_fileupload">
    <div v-if="cueInfo.cuetype != 'A'">
      <DxFileUploader
        :ref="fileUploadRef"
        :chunk-size="chunkSize"
        :labelText="title_name"
        :uploadAbortedMessage="uploadAbortedMessage"
        :max-file-size="104857600"
        select-button-text="파일 업로드"
        name="file"
        :accept="accept"
        :upload-url="getUrl"
        :upload-custom-data="uploaderCustomData"
        @uploaded="onUploaded"
        @upload-started="OnUploadStarted"
      >
      </DxFileUploader>
    </div>
    <div v-else class="uploader_title">{{ title_name }}</div>
    <div class="chunk-panel">
      <div class="chunk-item" v-for="(file, index) in attachments" :key="index">
        <div v-if="!file.delstate">
          <span class="segment-size dx-theme-accent-as-text-color mr-2"
            >{{ index + 1 }}.</span
          >
          <span class="segment-size dx-theme-accent-as-text-color mr-2">{{
            file.filename
          }}</span>
          <span id="file_manager_btn">
            <DxButton
              icon="download"
              hint="다운로드"
              @click="onFileDownload(file)"
            />
            <DxButton
              v-if="cueInfo.cuetype != 'A'"
              icon="remove"
              hint="삭제"
              @click="onFileDelete(file)"
            />
          </span>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import DxFileUploader from "devextreme-vue/file-uploader";
import DxButton from "devextreme-vue/button";
import { mapGetters, mapMutations } from "vuex";
import { USER_ID } from "@/constants/config";
import { eventBus } from "@/eventBus";
const fileUploadRef = "fileUploader";
const qs = require("qs");

function get_date_str(date) {
  var sYear = date.getFullYear();
  var sMonth = date.getMonth() + 1;
  var sDate = date.getDate();
  var hours = date.getHours();
  var minutes = date.getMinutes();
  var seconds = date.getSeconds();
  var milliseconds = date.getMilliseconds();

  sMonth = sMonth > 9 ? sMonth : "0" + sMonth;
  sDate = sDate > 9 ? sDate : "0" + sDate;
  hours = hours > 9 ? hours : "0" + hours;
  minutes = minutes > 9 ? minutes : "0" + minutes;
  seconds = seconds > 9 ? seconds : "0" + seconds;

  var result =
    sYear +
    "" +
    sMonth +
    "" +
    sDate +
    "" +
    hours +
    "" +
    minutes +
    "" +
    seconds +
    "" +
    milliseconds;
  return result;
}

export default {
  data() {
    return {
      fileUploadRef,
      title_name: "첨부파일",
      files: [],
      uploaderCustomData: {
        folder_date: "",
      }, //이후에 props로 바꾸기
      chunkSize: 1000000,
      maxFileLength: 10,
      uploadAbortedMessage: "파일을 추가할 수 없습니다.",
      accept:
        "application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,text/plain,.pdf,.docx,.hwp",
      getUrl: "/api/CueAttachments/chunkFileTempUpload",
    };
  },
  components: {
    DxFileUploader,
    DxButton,
  },
  created() {
    const date = new Date();
    this.uploaderCustomData.folder_date = get_date_str(date);
    eventBus.$on("attachments-delete", () => {
      this.attachments.forEach((file) => {
        this.onFileDelete(file);
      });
      this.SET_ATTACHMENTS([]);
    });
  },
  computed: {
    ...mapGetters("cueList", ["attachments"]),
    ...mapGetters("cueList", ["cueInfo"]),
    fileUploader: function () {
      return this.$refs[fileUploadRef].instance;
    },
  },
  methods: {
    ...mapMutations("cueList", ["SET_ATTACHMENTS"]),
    async onFileDelete(file) {
      if (!file.fileid) {
        await this.$http
          .delete(`/api/CueAttachments/attachmentsDelete`, {
            params: file,
            paramsSerializer: (params) => {
              return qs.stringify(params);
            },
          })
          .then((res) => {
            var resultArray = this.attachments.filter((item) => {
              return item.filepath != file.filepath;
            });
            this.SET_ATTACHMENTS(resultArray);
            window.$notify("info", `삭제완료.`, "", {
              duration: 10000,
              permanent: false,
            });
          })
          .catch((err) => {
            window.$notify("error", `삭제실패.`, "", {
              duration: 10000,
              permanent: false,
            });
          });
      } else {
        file.delstate = true;
      }
    },
    onFileDownload(file) {
      const link = document.createElement("a");
      link.href = `/api/CueAttachments/exportFileDownload?guid=${
        file.filepath
      }&userid=${sessionStorage.getItem(USER_ID)}&downloadname=${
        file.filename
      }`;
      document.body.appendChild(link);
      link.click();
    },
    onUploaded(e) {
      const arrData = _.cloneDeep(this.attachments);
      var file = {};
      file.filepath = JSON.parse(e.request.response).resultObject;
      file.filename = e.file.name;
      file.filesize = e.file.size;
      file.delstate = false;
      arrData.push(file);
      this.SET_ATTACHMENTS(arrData);
    },
    OnUploadStarted(e) {
      const regexImoji =
        /(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])|[~!@#$%^&*()+|<>?:{}]/gi;
      const regex = /^[ㄱ-ㅎ|가-힣|a-z|A-Z|0-9|]+$/;
      if (this.attachments.length >= this.maxFileLength) {
        e.component.abortUpload();
      }
      if (regexImoji.test(e.file.name) || regex.test(e.file.name)) {
        e.component.abortUpload();
      }
    },
  },
};
</script>
<style>
#webcuesheet_fileupload .dx-fileuploader-wrapper {
  padding: 15px 10px 0px 10px;
}
#webcuesheet_fileupload .dx-fileuploader-input-wrapper {
  position: relative;
  border: 1px solid #d7d7d7;
  border-radius: 2px 2px 0px 0px;
}
#webcuesheet_fileupload .chunk-panel {
  overflow: auto;
  height: 140px;
  padding: 15px 15px 15px 5px;
  margin: 0px 10px 10px 10px;
  border-bottom: 1px solid #d7d7d7;
  border-right: 1px solid #d7d7d7;
  border-left: 1px solid #d7d7d7;
  border-radius: 0px 0px 2px 2px;
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
#webcuesheet_fileupload .uploader_title {
  margin: 10px 10px 0px 10px;
  padding: 10px;
  border: 1px solid #d7d7d7;
}
#webcuesheet_fileupload .dx-fileuploader-file {
  line-height: 18px;
}
#webcuesheet_fileupload .chunk-item {
  padding: 5px;
}
#file_manager_btn .dx-button-content {
  width: 20px;
  height: 20px;
  padding: 0;
  /* background: #ddd;
    border-radius: 2px; */
}
</style>
