<template>
  <div>
    <b-modal
      id="upload-modal-closing"
      title="메타데이터 입력"
      size="md"
      modal-class="my-modal-file"
      no-close-on-backdrop
      v-model="showDialog"
    >
      <!-- 제목 -->
      <common-input-text-max-length
        label="제목"
        labelfor="input-title"
        v-model="$v.metaData.title.$model"
        :state="!$v.metaData.title.required"
      >
      </common-input-text-max-length>
      <!-- 내용 -->
      <common-text-area-max-length
        label="내용"
        label-for="input-memo"
        v-model="$v.metaData.memo.$model"
        :state="!$v.metaData.memo.required"
      >
      </common-text-area-max-length>
      <!--  FOOTER: 액션 -->
      <template slot="modal-footer">
        <div class="flex-grow-1"></div>
        <div>
          <b-button
            variant="outline-primary default cutom-label"
            @click="submit()"
            :disabled="invalid() || status"
            >MY공간으로 복사
            <span
              v-if="status"
              class="spinner-border spinner-border-sm"
              aria-hidden="true"
            ></span>
          </b-button>
          <b-button
            variant="outline-danger default cutom-label-cancel"
            @click="close"
            >취소</b-button
          >
        </div>
      </template>
    </b-modal>
  </div>
</template>

<script>
import mixinValidate from "../../mixin/MixinValidate";
import { mapMutations, mapActions } from "vuex";
import { USER_ID, INPUT_MAX_LENGTH } from "@/constants/config";

export default {
  mixins: [mixinValidate],
  props: {
    show: {
      type: Boolean,
      default: false,
    },
    MySpaceScreenName: {
      type: String,
      default: false,
    },
  },
  data() {
    return {
      metaData: {
        title: "",
        memo: "",
        fileSize: 0,
        filePath: "copy.wav",
      },
      INPUT_MAX_LENGTH,
      rowData: {},
      status: false,
    };
  },
  computed: {
    showDialog: {
      get() {
        return this.show;
      },
      set(v) {
        if (!v) {
          this.reset();
          this.$emit("close");
        }
      },
    },
  },
  methods: {
    submitConfirm() {
      if (this.invalid()) {
        this.$fn.notify("inputError", {});
        return;
      }

      this.$bvModal.show("modalModify");
    },
    submit() {
      this.$bvModal.hide("modalModify");
      const userId = sessionStorage.getItem(USER_ID);
      this.status = true;
      this.$http
        .post(`/api/products/workspace/private/verify/${userId}`, this.metaData)
        .then((res) => {
          if (res.status === 200 && !res.data.errorMsg) {
            this.$emit("ok");
            this.showDialog = false;
            this.status = false;
          } else {
            // this.$fn.notify('error', { message: res.data.errorMsg })
            this.status = false;
          }
        })
        .catch((error) => {
          this.status = false;
        });
    },
    setData(rowData) {
      console.log(rowData);
      this.metaData.title = "";
      this.metaData.memo = "";
      this.rowData = rowData;
      if (this.MySpaceScreenName == "[DL3]") {
        this.getDL3Meta(rowData);
      } else if (this.MySpaceScreenName == "[프로그램]") {
        this.getProgramMeta(rowData);
      } else if (this.MySpaceScreenName == "[부조SPOT]") {
        this.getScrMeta(rowData);
      } else if (this.MySpaceScreenName == "[취재물]") {
        this.getCoverageMeta(rowData);
      } else if (this.MySpaceScreenName == "[(구)프로]") {
        this.getProMeta(rowData);
      } else if (this.MySpaceScreenName == "[음원]") {
        this.getSongMeta(rowData);
      } else if (this.MySpaceScreenName == "[효과음]") {
        this.getEffectMeta(rowData);
      } else if (this.MySpaceScreenName == "[Filler PR]") {
        this.getFillerPrMeta(rowData);
      } else if (this.MySpaceScreenName == "[Filler 소재]") {
        this.getFillerGeneralMeta(rowData);
      } else if (this.MySpaceScreenName == "[Filler 기타]") {
        this.getFillerEtcMeta(rowData);
      } else if (this.MySpaceScreenName == "[주조SPOT]") {
        this.getMcrSpotMeta(rowData);
      } else if (this.MySpaceScreenName == "[Filler 시간]") {
        this.getFillerTimeMeta(rowData);
      }
    },
    getRowData() {
      return this.rowData;
    },
    getMetaData() {
      return this.metaData;
    },
    invalid() {
      return (
        !this.$v.metaData.title.$invalid || !this.$v.metaData.memo.$invalid
      );
    },
    reset() {
      this.metaData = {
        title: "",
        memo: "",
        fileSize: 0,
        filePath: "copy.wav",
      };
    },
    close() {
      this.showDialog = false;
    },
    getDL3Meta(rowData) {
      this.metaData.title =
        this.MySpaceScreenName +
        rowData.mediaName +
        "_" +
        rowData.name +
        "_" +
        rowData.brdDT;
      this.metaData.memo =
        "녹음 분량 : " +
        rowData.duration +
        "\n" +
        "단말 : " +
        rowData.mediaName;
    },
    getProgramMeta(rowData) {
      this.metaData.title =
        this.MySpaceScreenName +
        rowData.mediaName +
        "_" +
        rowData.name +
        "_" +
        rowData.brdDT +
        "_" +
        rowData.brdTime;
      this.metaData.memo =
        "길이 : " +
        rowData.duration +
        "\n" +
        "제작자 : " +
        rowData.editorName +
        "\n" +
        "최종 편집 일시 : " +
        rowData.reqCompleteDtm;
    },
    getScrMeta(rowData) {
      this.metaData.title =
        this.MySpaceScreenName +
        rowData.mediaName +
        "_" +
        rowData.name +
        "_" +
        rowData.categoryName +
        "_" +
        rowData.pgmName +
        "_" +
        rowData.brdDT;
      this.metaData.memo =
        "길이 : " + rowData.duration + "\n" + "제작자 : " + rowData.editorName;
    },
    getCoverageMeta(rowData) {
      this.metaData.title =
        this.MySpaceScreenName +
        rowData.categoryName +
        "_" +
        rowData.name +
        "_" +
        rowData.pgmName +
        "_" +
        rowData.brdDT;
      this.metaData.memo =
        "길이 : " +
        rowData.duration +
        "\n" +
        "최종 편집 일시 : " +
        rowData.editDtm +
        "\n" +
        "취재인 : " +
        rowData.reporter;
    },
    getProMeta(rowData) {
      this.metaData.title =
        this.MySpaceScreenName +
        rowData.name +
        "_" +
        rowData.categoryName +
        "_" +
        rowData.editorName;
      this.metaData.memo =
        "길이 : " +
        rowData.duration +
        "\n" +
        "최종 편집 일시 : " +
        rowData.editDtm +
        "\n" +
        "타입 : " +
        rowData.proType;
    },
    getSongMeta(rowData) {
      this.metaData.title =
        this.MySpaceScreenName +
        rowData.name +
        "_" +
        rowData.artistName +
        "_" +
        rowData.albumName;
      this.metaData.memo =
        "재생 시간 : " +
        rowData.duration +
        "\n" +
        "작곡가 : " +
        rowData.composer +
        "\n" +
        "작사가 : " +
        rowData.writer +
        "\n" +
        "발매년도 : " +
        rowData.releaseDate +
        "\n" +
        "배열번호 : " +
        rowData.sequenceNO;
    },
    getEffectMeta(rowData) {
      this.metaData.title =
        this.MySpaceScreenName + rowData.name + "_" + rowData.description;
      this.metaData.memo =
        "길이 : " + rowData.duration + "\n" + "포맷 : " + rowData.audioFormat;
    },
    getFillerPrMeta(rowData) {
      this.metaData.title =
        this.MySpaceScreenName +
        rowData.categoryName +
        "_" +
        rowData.name +
        "_" +
        rowData.brdDT;
      this.metaData.memo =
        "길이 : " + rowData.duration + "\n" + "편집자 : " + rowData.editorName;
    },
    getFillerGeneralMeta(rowData) {
      this.metaData.title =
        this.MySpaceScreenName +
        rowData.categoryName +
        "_" +
        rowData.name +
        "_" +
        rowData.brdDT;
      this.metaData.memo =
        "길이 : " + rowData.duration + "\n" + "편집자 : " + rowData.editorName;
    },
    getFillerEtcMeta(rowData) {
      this.metaData.title =
        this.MySpaceScreenName +
        rowData.categoryName +
        "_" +
        rowData.name +
        "_" +
        rowData.brdDT;
      this.metaData.memo =
        "길이 : " + rowData.duration + "\n" + "편집자 : " + rowData.editorName;
    },
    getMcrSpotMeta(rowData) {
      this.metaData.title =
        this.MySpaceScreenName +
        rowData.mediaName +
        "_" +
        rowData.name +
        "_" +
        rowData.brdDT;
      this.metaData.memo =
        "길이 : " + rowData.duration + "\n" + "편집자 : " + rowData.editorName;
    },
    getFillerTimeMeta(rowData) {
      this.metaData.title =
        this.MySpaceScreenName + rowData.mediaName + "_" + rowData.name;
      this.metaData.memo =
        "방송 개시일 : " +
        rowData.startDT +
        "\n" +
        "방송 종료일 : " +
        rowData.endDT +
        "\n" +
        "길이 : " +
        rowData.duration +
        "\n" +
        "편집자 : " +
        rowData.editorName;
    },
  },
};
</script>
