<template>
  <div>
    <transition name="slide-fade">
      <CommonMetaModal
        v-show="this.MetaModal"
        @close="MetaModalOff"
        style="font-family: MBC 새로움 M"
        :isActive="this.isActive"
      >
        <h3 slot="header">메타 데이터 입력</h3>
        <h4 slot="body">
          <div style="width: 1000px; height: 500px; float: left">
            <div
              style="
                width: 350px;
                height: 70px;
                position: relative;
                top: 15px;
                left: 20px;
                margin-bottom: 20px;
              "
            >
              <h3 style="color: #008ecc">파일 정보</h3>
              <div style="padding: 10px; border: 1px solid #008ecc">
                <b-form-group
                  label="파일명"
                  class="has-float-label"
                  style="font-size: 15px; margin-top: 10px"
                >
                  <b-form-input
                    title="오디오 포맷"
                    style="width: 330px; font-size: 14px"
                    class="editTask title-ellipsis"
                    v-model="this.MetaModalTitle"
                    disabled
                    aria-describedby="input-live-help input-live-feedback"
                    placeholder="Title"
                    trim
                  />
                </b-form-group>
                <div style="height: 50px; margin-top: 10px">
                  <b-form-group
                    label="파일 분량"
                    class="has-float-label"
                    style="font-size: 15px"
                  >
                    <b-form-input
                      style="width: 330px"
                      class="editTask"
                      v-model="MetaData.duration"
                      disabled
                      aria-describedby="input-live-help input-live-feedback"
                      placeholder="duration"
                      trim
                    >
                    </b-form-input>
                    <b-icon-alarm
                      v-if="!this.durationState"
                      variant="warning"
                      style="position: relative; top: -28px; left: 300px"
                    ></b-icon-alarm>
                  </b-form-group>
                </div>
                <div style="height: 50px; margin-top: 10px">
                  <b-form-group
                    label="오디오 포맷"
                    class="has-float-label"
                    style="font-size: 15px"
                  >
                    <b-form-input
                      title="오디오 포맷"
                      style="width: 330px"
                      class="editTask"
                      v-model="MetaData.audioFormat"
                      disabled
                      aria-describedby="input-live-help input-live-feedback"
                      placeholder="audioFormat"
                      trim
                    />
                  </b-form-group>
                </div>
              </div>
              <div style="width: 300px; margin-top: 15px">
                <h3 style="color: #008ecc">소재 유형</h3>
                <b-form-select
                  style="width: 350px; border-color: #008eca"
                  id="filetype"
                  v-model="MetaData.typeSelected"
                  :options="typeOptions"
                  @change="resetMemo"
                ></b-form-select>
              </div>
            </div>

            <div :class="[isActive ? 'date-modal' : 'file-modal']">
              <div style="width: 350px">
                <h3 style="color: #008ecc">메타 데이터</h3>
                <my-disk
                  v-if="this.MetaData.typeSelected == 'my-disk'"
                ></my-disk>
                <scr-spot
                  v-if="this.MetaData.typeSelected == 'scr-spot'"
                ></scr-spot>
              </div>
            </div>
            <transition name="slide-fade">
              <div>
                <div v-show="!isActive" class="date-div">
                  <h3 style="color: #008ecc">프로그램 선택</h3>
                  <program
                    v-if="this.MetaData.typeSelected == 'program'"
                  ></program>
                  <mcr-spot
                    v-if="this.MetaData.typeSelected == 'mcr-spot'"
                  ></mcr-spot>
                  <static-spot
                    v-if="this.MetaData.typeSelected == 'static-spot'"
                  ></static-spot>
                  <var-spot v-if="this.MetaData.typeSelected == 'var-spot'">
                  </var-spot>
                  <report v-if="this.MetaData.typeSelected == 'report'">
                  </report>
                  <filler
                    v-if="this.MetaData.typeSelected == 'filler'"
                  ></filler>
                </div>
                <div
                  style="
                    position: absolute;
                    top: 620px;
                    left: 20px;
                    width: 950px;
                  "
                >
                  <b-progress
                    v-if="!isActive"
                    class="w-100"
                    variant="success"
                    :max="100"
                    height="16px"
                  >
                    <b-progress-bar
                      :max="100"
                      :value="percent"
                      :label="`${percent} %`"
                      show-progress
                    ></b-progress-bar
                  ></b-progress>
                </div>
              </div>
            </transition>
          </div>
        </h4>

        <h3 slot="footer">
          <div style="margin-left: 20px; width: 350px">
            <b-progress
              v-if="isActive"
              class="w-100"
              variant="success"
              :max="100"
              height="16px"
            >
              <b-progress-bar
                :max="100"
                :value="percent"
                :label="`${percent} %`"
                show-progress
              ></b-progress-bar
            ></b-progress>
          </div>
          <div :class="[isActive ? 'date-modal-button' : 'file-modal-button']">
            <!-- 로그 버튼 -->
            <!-- <b-button
              variant="outline-success"
              @click="log"
              style="margin-left: -80px"
            >
              <span class="label">확인</span>
            </b-button> -->
            <!-- <b-button
              class="defaultButton"
              variant="outline-danger"
              @click="resetEvent"
            >
              초기화
            </b-button> -->

            <!-- <b-button
              class="defaultButton"
              variant="outline-success"
              v-show="processing && !fileUploading"
            >
              <b-spinner small type="grow"></b-spinner>
              <span class="label">확인중...</span>
            </b-button> -->
            <b-button
              variant="outline-primary"
              v-show="!processing && fileUploading"
            >
              <b-spinner small type="grow"></b-spinner>
              <span class="label">업로드 중...</span>
            </b-button>
            <span v-show="metaValid">
              <b-button
                variant="outline-primary"
                @click="uploadfile()"
                style="margin-left: 45px"
                v-show="!processing && !fileUploading"
              >
                <span class="label">업로드</span>
              </b-button>
            </span>
            <span v-show="!metaValid">
              <b-button
                variant="primary"
                disabled
                style="margin-left: 45px"
                v-show="!processing && !fileUploading"
              >
                <span class="label">업로드</span>
              </b-button>
            </span>
          </div>
        </h3>
      </CommonMetaModal>
    </transition>
  </div>
</template>

<script>
import CommonMetaModal from "../Modal/CommonMetaModal";
import MixinBasicPage from "../../mixin/MixinBasicPage";
import CommonVueSelect from "../../components/Form/CommonVueSelect.vue";
import myDisk from "./MetaData/my-disk.vue";
import program from "./MetaData/program.vue";
import mcrSpot from "./MetaData/mcr-spot.vue";
import scrSpot from "./MetaData/scr-spot.vue";
import staticSpot from "./MetaData/static-spot.vue";
import varSpot from "./MetaData/var-spot.vue";
import report from "./MetaData/coverage.vue";
import filler from "./MetaData/filler";
import axios from "axios";
import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
export default {
  props: {
    fileState: {
      type: String,
      default: "",
    },
    percent: {
      type: Number,
      default: 0,
    },
    MetaModal: {
      type: Boolean,
      default: false,
    },
  },
  components: {
    CommonMetaModal,
    myDisk,
    program,
    mcrSpot,
    scrSpot,
    staticSpot,
    CommonVueSelect,
    varSpot,
    report,
    filler,
  },
  mixins: [MixinBasicPage],
  data() {
    return {
      cancel: false,
    };
  },
  computed: {
    ...mapState("FileIndexStore", {
      MetaModalTitle: (state) => state.MetaModalTitle,
      date: (state) => state.date,
      fileSDate: (state) => state.fileSDate,
      fileEDate: (state) => state.fileEDate,
      localFiles: (state) => state.localFiles,
      MetaData: (state) => state.MetaData,
      masteringListData: (state) => state.masteringListData,
      ProgramData: (state) => state.ProgramData,
      ProgramSelected: (state) => state.ProgramSelected,
      EventSelected: (state) => state.EventSelected,
      isActive: (state) => state.isActive,
      processing: (state) => state.processing,
      fileUploading: (state) => state.fileUploading,
      typeOptions: (state) => state.typeOptions,
    }),
    ...mapGetters("FileIndexStore", [
      "typeState",
      "titleState",
      "memoState",
      "editorState",
      "durationState",
      "metaValid",
    ]),
    ...mapGetters("user", ["getMenuGrpName"]),
    getVariant() {
      return this.isActive ? "outline-dark" : "outline-primary";
    },
  },
  watch: {
    fileState(v) {
      if (v == "업로드 성공") {
        this.setFileUploading(false);
        this.MetaModalOff();
        this.resetEvent();
      } else if (v == "리셋") {
        this.MetaModalOff();
        this.resetEvent();
      }
    },
    MetaModal(v) {
      if (!v) {
        this.resetEvent();
      } else {
        if (this.typeOptions.length == 1) {
          this.typeOptionsByRole(this.getMenuGrpName);
        }
      }
    },
  },
  methods: {
    calc() {
      var dh = this.MetaData.duration.slice(0, 2);
      var dm = this.MetaData.duration.slice(3, 5);
      var ds = this.MetaData.duration.slice(6, 8);
      var calcD = dh * 60 * 60 + dm * 60 + ds * 1;

      var ph = this.ProgramSelected.durationSec.slice(0, 2);
      var pm = this.ProgramSelected.durationSec.slice(3, 5);
      var ps = this.ProgramSelected.durationSec.slice(6, 8);
      var calcP = ph * 60 * 60 + pm * 60 + ps * 1;
      console.log(calcD - calcP);
    },
    log() {
      if (this.MetaData.typeSelected == "my-disk") {
        var data = {
          UserId: sessionStorage.getItem("user_id"),
          title: this.MetaData.title,
          memo: this.MetaData.memo,
        };
      } else if (this.MetaData.typeSelected == "program") {
        var data = {
          UserId: sessionStorage.getItem("user_id"),
          media: this.MetaData.mediaSelected,
          productId: this.ProgramSelected.productId,
          onairTime: this.ProgramSelected.onairTime,
          editor: this.MetaData.editor,
          memo: this.MetaData.memo,
        };
      } else if (this.MetaData.typeSelected == "mcr-spot") {
        var data = {
          UserId: sessionStorage.getItem("user_id"),
          media: this.MetaData.mediaSelected,
          productId: this.EventSelected.id,
          onairTime: this.date,
          editor: this.MetaData.editor,
          memo: this.MetaData.memo,
          advertiser: this.MetaData.advertiser,
        };
      } else if (this.MetaData.typeSelected == "scr-spot") {
        var data = {
          UserId: sessionStorage.getItem("user_id"),
          title: this.MetaData.title,
          memo: this.MetaData.memo,
          advertiser: this.MetaData.advertiser,
          editor: this.MetaData.editor,
          media: this.MetaData.mediaSelected,
        };
      } else if (this.MetaData.typeSelected == "static-spot") {
        var data = {
          UserId: sessionStorage.getItem("user_id"),
          media: this.MetaData.mediaSelected,
          productId: this.EventSelected.id,
          SDate: this.fileSDate,
          EDate: this.fileEDate,
          editor: this.MetaData.editor,
          memo: this.MetaData.memo,
          advertiser: this.MetaData.advertiser,
        };
      } else if (this.MetaData.typeSelected == "var-spot") {
        var data = {
          UserId: sessionStorage.getItem("user_id"),
          media: this.MetaData.mediaSelected,
          productId: this.EventSelected.id,
          SDate: this.fileSDate,
          EDate: this.fileEDate,
          editor: this.MetaData.editor,
          memo: this.MetaData.memo,
          advertiser: this.MetaData.advertiser,
        };
      } else if (this.MetaData.typeSelected == "report") {
        var data = {
          UserId: sessionStorage.getItem("user_id"),
          meida: this.MetaData.mediaSelected,
          ProductId: this.EventSelected.id,
          date: this.date,
          editor: this.MetaData.editor,
          memo: this.MetaData.memo,
          reporter: this.MetaData.reporter,
        };
      } else if (this.MetaData.typeSelected == "filler") {
        var data = {
          UserId: sessionStorage.getItem("user_id"),
          media: this.MetaData.mediaSelected,
          title: this.MetaData.title,
          memo: this.MetaData.memo,
          editor: this.MetaData.editor,
          onairTime: this.date,
        };
      }
    },
    ...mapMutations("FileIndexStore", [
      "setUploaderCustomData",
      "setEditor",
      "setMasteringListData",
      "setProcessing",
      "setFileUploading",
      "resetTitle",
      "resetMemo",
      "resetEditor",
      "resetType",
    ]),
    resetEvent() {
      this.$emit("reset");
    },
    inputEditor(v) {
      this.setEditor(v.id);
    },
    MetaModalOff() {
      if (this.processing || this.fileUploading) {
        this.cancel = true;
        this.$emit("cancel");
      }
      this.$emit("close");
    },
    async uploadfile() {
      if (this.metaValid) {
        //NOTE: 커스텀 데이터 파라미터
        if (this.MetaData.typeSelected == "my-disk") {
          var data = {
            UserId: sessionStorage.getItem("user_id"),
            title: this.MetaData.title,
            memo: this.MetaData.memo,
          };
        } else if (this.MetaData.typeSelected == "program") {
          var data = {
            memo: this.MetaData.memo,
            UserId: sessionStorage.getItem("user_id"),
            media: this.MetaData.mediaSelected,
            productId: this.ProgramSelected.productId,
            onairTime: this.ProgramSelected.onairTime,
            editor: this.MetaData.editor,
          };
        } else if (this.MetaData.typeSelected == "mcr-spot") {
          var data = {
            memo: this.MetaData.memo,
            UserId: sessionStorage.getItem("user_id"),
            media: this.MetaData.mediaSelected,
            productId: this.EventSelected,
            onairTime: this.date,
            editor: this.MetaData.editor,
          };
        } else if (this.MetaData.typeSelected == "scr-spot") {
          var data = {
            UserId: sessionStorage.getItem("user_id"),
            title: this.MetaData.title,
            memo: this.MetaData.memo,
            advertiser: this.MetaData.advertiser,
            editor: this.MetaData.editor,
            media: this.MetaData.mediaSelected,
          };
        } else if (this.MetaData.typeSelected == "static-spot") {
          var data = {
            UserId: sessionStorage.getItem("user_id"),
            media: this.MetaData.mediaSelected,
            productId: this.EventSelected.id,
            SDate: this.fileSDate,
            EDate: this.fileEDate,
            editor: this.MetaData.editor,
            memo: this.MetaData.memo,
            advertiser: this.MetaData.advertiser,
          };
        } else if (this.MetaData.typeSelected == "var-spot") {
          var data = {
            UserId: sessionStorage.getItem("user_id"),
            media: this.MetaData.mediaSelected,
            productId: this.EventSelected.id,
            SDate: this.fileSDate,
            EDate: this.fileEDate,
            editor: this.MetaData.editor,
            memo: this.MetaData.memo,
            advertiser: this.MetaData.advertiser,
          };
        } else if (this.MetaData.typeSelected == "report") {
          var data = {
            UserId: sessionStorage.getItem("user_id"),
            meida: this.MetaData.mediaSelected,
            ProductId: this.EventSelected.id,
            date: this.date,
            editor: this.MetaData.editor,
            memo: this.MetaData.memo,
            reporter: this.MetaData.reporter,
          };
        } else if (this.MetaData.typeSelected == "filler") {
          var data = {
            UserId: sessionStorage.getItem("user_id"),
            media: this.MetaData.mediaSelected,
            title: this.MetaData.title,
            memo: this.MetaData.memo,
            editor: this.MetaData.editor,
            onairTime: this.date,
          };
        }
        this.setUploaderCustomData(data);
        if (this.cancel) {
          this.cancel = false;
          return;
        } else {
          this.setFileUploading(true);
          this.$emit("upload");
        }
      } else if (!this.metaValid) {
        this.$fn.notify("error", { title: "메타 데이터 확인" });
      }
    },
    typeOptionsByRole(role) {
      if (role == "관리자") {
        this.typeOptions.push({ value: "my-disk", text: "My디스크" });
        this.typeOptions.push({ value: "program", text: "프로그램" });
        this.typeOptions.push({ value: "mcr-spot", text: "주조SPOT" });
        this.typeOptions.push({ value: "scr-spot", text: "부조SPOT" });
        this.typeOptions.push({ value: "static-spot", text: "고정소재" });
        this.typeOptions.push({ value: "var-spot", text: "변동소재" });
        this.typeOptions.push({ value: "report", text: "취재물" });
        this.typeOptions.push({ value: "filler", text: "필러" });
      } else if (role == "편성PD") {
        this.typeOptions.push({ value: "my-disk", text: "My디스크" });
        this.typeOptions.push({ value: "program", text: "프로그램" });
        this.typeOptions.push({ value: "scr-spot", text: "부조SPOT" });
        this.typeOptions.push({ value: "report", text: "취재물" });
        this.typeOptions.push({ value: "mcr-spot", text: "주조SPOT" });
        this.typeOptions.push({ value: "filler", text: "필러" });
        this.typeOptions.push({ value: "static-spot", text: "고정소재" });
        this.typeOptions.push({ value: "var-spot", text: "변동소재" });
      } else if (role == "제작PD") {
        this.typeOptions.push({ value: "my-disk", text: "My디스크" });
        this.typeOptions.push({ value: "program", text: "프로그램" });
        this.typeOptions.push({ value: "filler", text: "필러" });
      } else if (role == "리포터") {
        this.typeOptions.push({ value: "my-disk", text: "My디스크" });
        this.typeOptions.push({ value: "report", text: "취재물" });
      } else if (role == "라디오뉴스") {
        this.typeOptions.push({ value: "my-disk", text: "My디스크" });
        this.typeOptions.push({ value: "report", text: "취재물" });
      } else if (role == "제작 Staff") {
        this.typeOptions.push({ value: "my-disk", text: "My디스크" });
      } else if (role == "TD") {
        this.typeOptions.push({ value: "my-disk", text: "My디스크" });
      }
    },
  },
};
</script>

<style>
.defaultButton {
  background-color: #fff !important;
  border-color: #bbbbbb !important;
  color: #3a3a3a !important;
}
.title-ellipsis {
  font-size: 16px;
  width: 340px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

/* .title-ellipsis:hover {
  text-overflow: clip;
  white-space: normal;
  word-break: break-word;
} */
</style>
