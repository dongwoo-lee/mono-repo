<template>
  <div>
    <b-modal
      :id="modalId"
      size="lg"
      centered
      :title="modalTitle"
      ok-title="확인"
      cancel-title="취소"
      @show="showModal"
    >
      <div id="popup_file_upload_content">
        <div style="text-align: center" class="m-4">
          <p>
            대표이미지로 설정할 이미지 파일 ({{
              allowedFileExtensions.toString()
            }})을 선택하세요.
          </p>
        </div>
        <div class="file_upload_flex_box">
          <div v-if="textVisible && !imageSource">
            <span>{{ imageText }}</span>
          </div>
          <img class="responsive_image" :src="imageSource" v-else alt="" />
        </div>
        <div class="mt-4" style="text-align: center">
          <input
            type="file"
            id="fileInput"
            style="display: none"
            @change="handleFileSelect"
            accept=".jpg, .jpeg, .png"
            ref="fileInputRef"
          />
          <DxButton
            type="normal"
            style="padding: 5px 15px 5px 15px"
            text="업로드 파일 선택"
            styling-mode="outlined"
            @click="openFilePicker()"
          ></DxButton>
        </div>
      </div>

      <template #modal-footer="{ cancel }">
        <DxButton
          type="default"
          styling-mode="outlined"
          :width="130"
          @click="uploadOk()"
          ><span class="dx-button-text"
            >저장
            <b-spinner
              v-if="isSaveLoading"
              small
              variant="primary"
              label="Spinning"
            ></b-spinner>
          </span>
        </DxButton>
        <DxButton :width="100" text="취소" @click="cancel()" />
      </template>
    </b-modal>
  </div>
</template>
<script>
import DxButton from "devextreme-vue/button";
import { DxFileUploader } from "devextreme-vue/file-uploader";

export default {
  props: {
    modalId: {
      type: String,
      defalut: "",
    },
    modalTitle: {
      type: String,
      defalut: "파일 업로드",
    },
    isSaveLoading: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      allowedFileExtensions: [".jpg", ".jpeg", ".png"],
      imageSource: "",
      imageText: "No Image",
      file: null,
      textVisible: true,
      selectedFileName: null,
    };
  },
  mounted() {},
  components: { DxButton, DxFileUploader },
  methods: {
    showModal() {
      this.file = null;
      this.imageSource = "";
      this.imageText = "No Image";
    },
    cancel() {},
    uploadOk() {
      this.$emit("uploadOk", this.file);
      this.file = null;
    },
    openFilePicker() {
      this.$refs.fileInputRef.click();
    },
    handleFileSelect(event) {
      const selectFile = event.target.files[0];
      const allowedExtensions = ["jpg", "jpeg", "png"];
      const fileExtension = selectFile.name.split(".").pop().toLowerCase();
      if (allowedExtensions.includes(fileExtension)) {
        this.file = selectFile;
        const fileReader = new FileReader();
        fileReader.onload = () => {
          this.imageSource = fileReader.result;
        };
        fileReader.readAsDataURL(this.file);
      } else {
        this.imageText = "업로드 할 수 없는 확장자 파일입니다.";
      }
    },
  },
};
</script>
<style>
#popup_file_upload_content .file_upload_flex_box {
  position: relative;
  width: 584px;
  height: 328px;
  text-align: center;
  font-size: large;
  line-height: 320px;
  background-color: rgba(183, 183, 183, 0.1);
  border-width: 2px;
  border-color: darkgray;
  border-style: dashed;
  color: darkgray;
  box-sizing: border-box;
  margin: auto;
}
#popup_file_upload_content img {
  width: 100%;
  height: 100%;
}
#popup_file_upload_content .dx-fileuploader-input-wrapper .dx-button {
  float: unset;
}
#popup_file_upload_content .dx-fileuploader-input-wrapper {
  text-align: center;
}
.dx-fileuploader-files-container {
  display: none;
}
span {
  font-family: "MBC 새로움 M" !important;
}
.responsive_image {
  object-fit: cover;
}
</style>
