<template>
  <div>
    <transition name="slide-fade">
      <CommonMetaModal
        v-show="this.MetaModal"
        @close="MetaModalOff"
        style="font-family: MBC 새로움 M;"
        :isActive="this.isActive"
      >
        <h3 slot="header">
          파일 업로드 메타 데이터 입력
        </h3>
        <h4 slot="body">
          <div style="width:1000px; height:500px; float:left;">
            <div
              style="width:350px; height:70px; position:relative; top:15px; left:20px; margin-bottom:20px; "
            >
              <p
                style=" font-size:16px; width:350px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"
              >
                파일 명 : {{ this.MetaModalTitle }}
              </p>
              <b-progress
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

              <div style="height:50px;  margin-top : 20px;">
                <b-form-input
                  class="editTask"
                  v-model="MetaData.duration"
                  readonly
                  aria-describedby="input-live-help input-live-feedback"
                  placeholder="duration"
                  trim
                />
              </div>
              <div style="height:50px;  margin-top : 10px;">
                <b-form-input
                  class="editTask"
                  v-model="MetaData.audioFormat"
                  readonly
                  aria-describedby="input-live-help input-live-feedback"
                  placeholder="audioFormat"
                  trim
                />
              </div>
            </div>
            <div :class="[isActive ? 'date-modal' : 'file-modal']">
              <div>
                <div style="position:relative; ">
                  <div style="width:300px; height:80px;  margin-bottom:10px;">
                    <h3 style="color:#008ECC;">
                      소재 유형
                    </h3>
                    <b-form-select
                      style="width:350px;"
                      id="filetype"
                      v-model="MetaData.typeSelected"
                      :options="typeOptions"
                      :state="typeState"
                      required
                    ></b-form-select>
                  </div>

                  <h3 style="color:#008ECC; ">
                    메타 데이터
                  </h3>

                  <div v-show="this.MetaData.typeSelected == 'my-disk'">
                    <div style="height:50px;  margin-top : 10px;">
                      <b-form-input
                        class="editTask"
                        v-model="MetaData.title"
                        :state="titleState"
                        placeholder="제목"
                        trim
                      />

                      <button
                        v-show="titleState"
                        style="position:relative; left:315px; top:-27px; z-index:99; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
                      >
                        <b-icon
                          icon="x-circle"
                          font-scale="1"
                          style="position:relative; top:0px; right:0px; z-index:999;"
                          variant="secondary"
                          @click="resetTitle"
                        ></b-icon>
                      </button>
                    </div>
                  </div>

                  <!-- program -->
                  <transition name="fade">
                    <div v-show="this.MetaData.typeSelected == 'program'">
                      <b-form-group
                        label="제작자"
                        class="has-float-label"
                        style="position:fixed; top:550px; left:490px; z-index:9999; font-size:16px;"
                      >
                        <common-vue-select
                          style="font-size:14px; width:200px; border: 1px solid #008ecc;"
                          :suggestions="editorOptions"
                          @inputEvent="inputEditor"
                        ></common-vue-select>
                      </b-form-group>
                    </div>
                  </transition>

                  <!-- mcr-spot -->

                  <!-- scr-spot -->
                  <scr-spot
                    v-if="this.MetaData.typeSelected == 'scr-spot'"
                    :proMediaOptions="this.mediaOptions"
                  ></scr-spot>

                  <div style="height:50px;">
                    <b-form-input
                      class="editTask"
                      v-model="MetaData.memo"
                      :state="memoState"
                      aria-describedby="input-live-help input-live-feedback"
                      placeholder="설명"
                      trim
                    />

                    <button
                      v-show="memoState"
                      style="position:relative; left:315px; top:-27px; z-index:99; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
                    >
                      <b-icon
                        icon="x-circle"
                        font-scale="1"
                        style="position:relative; top:0px; right:0px; z-index:999;"
                        variant="secondary"
                        @click="resetMemo"
                      ></b-icon>
                    </button>
                  </div>
                </div>
              </div>
            </div>
            <transition name="slide-fade">
              <div v-show="!isActive" class="date-div">
                <h3 style="color:#008ECC; ">
                  프로그램 선택
                </h3>
                <!-- program -->
                <program
                  v-if="this.MetaData.typeSelected == 'program'"
                  :proMediaOptions="this.mediaOptions"
                  @proData="proData"
                ></program>
                <!-- mcr-spot -->
                <mcr-spot
                  v-if="this.MetaData.typeSelected == 'mcr-spot'"
                  :mcrMediaOptions="this.mediaOptions"
                  @mcrData="mcrData"
                  @mcrDate="mcrDate"
                ></mcr-spot>
              </div>
            </transition>
          </div>
        </h4>

        <h3 slot="footer">
          <!-- <b-button
            variant="outline-success"
            @click="log"
            style="margin-left:0px;"
          >
            <span class="label">확인</span>
          </b-button> -->
          <div :class="[isActive ? 'date-modal-button' : 'file-modal-button']">
            <b-button variant="outline-danger" @click="resetEvent">
              초기화
            </b-button>

            <b-button
              variant="outline-success"
              v-show="processing && !fileUploading"
            >
              <b-spinner small type="grow"></b-spinner>
              <span class="label">확인중...</span>
            </b-button>
            <b-button
              variant="outline-success"
              style="margin-right:10px;"
              v-show="!processing && fileUploading"
            >
              <b-spinner small type="grow"></b-spinner>
              <span class="label">업로드 중...</span>
            </b-button>
            <span v-show="metaValid">
              <b-button
                variant="outline-success"
                @click="uploadfile()"
                style="margin-left:28px;"
                v-show="!processing && !fileUploading"
              >
                <span class="label">업로드</span>
              </b-button>
            </span>
            <span v-show="!metaValid">
              <b-button
                variant="success"
                disabled
                @click="uploadfile()"
                style="margin-left:28px;"
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
// import CommonFileFunction from "./CommonFileFunction";
import program from "./MetaData/program.vue";
import mcrSpot from "./MetaData/mcr-spot.vue";
import scrSpot from "./MetaData/scr-spot.vue";
import CommonVueSelect from "../../components/Form/CommonVueSelect.vue";
import MixinBasicPage from "../../mixin/MixinBasicPage";
import axios from "axios";
import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
export default {
  props: {
    fileState: {
      type: String,
      default: ""
    },
    percent: {
      type: Number,
      default: 0
    },
    MetaModal: {
      type: Boolean,
      default: false
    }
  },
  components: {
    CommonMetaModal,
    program,
    mcrSpot,
    scrSpot,
    CommonVueSelect
  },
  mixins: [MixinBasicPage],
  data() {
    return {
      cancel: false
    };
  },
  created() {
    axios.get("/api/Mastering/mastering-status").then(res => {
      res.data.resultObject.data.forEach(e => {
        var vueTableData = {
          title: e.title,
          type: this.getCategory(e.category),
          user_id: e.regUserId,
          date: e.regDtm,
          step: e.workStatus
        };
        this.setVueTableData(vueTableData);
      });
    });
  },
  computed: {
    ...mapState("FileIndexStore", {
      MetaModalTitle: state => state.MetaModalTitle,
      localFiles: state => state.localFiles,
      MetaData: state => state.MetaData,
      connectionId: state => state.connectionId,
      vueTableData: state => state.vueTableData,
      ProgramData: state => state.ProgramData,
      isActive: state => state.isActive,
      processing: state => state.processing,
      fileUploading: state => state.fileUploading,
      typeOptions: state => state.typeOptions
    }),
    ...mapGetters("FileIndexStore", [
      "typeState",
      "titleState",
      "memoState",
      "editorState",
      "metaValid"
    ]),
    ...mapGetters("user", ["getMenuGrpName"]),
    getVariant() {
      return this.isActive ? "outline-dark" : "outline-primary";
    }
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
    }
  },
  methods: {
    log() {
      if (this.MetaData.typeSelected == "my-disk") {
        var data = {
          UserId: sessionStorage.getItem("user_id"),
          title: this.MetaData.title,
          memo: this.MetaData.memo
        };
      } else if (this.MetaData.typeSelected == "program") {
        var data = {
          memo: this.MetaData.memo,
          UserId: sessionStorage.getItem("user_id"),
          media: this.MetaData.proMediaSelected,
          productId: this.ProgramGrid.productId,
          onairTime: this.ProgramGrid.onairTime,
          editor: this.MetaData.editor
        };
      } else if (this.MetaData.typeSelected == "mcr-spot") {
        var data = {
          memo: this.MetaData.memo,
          UserId: sessionStorage.getItem("user_id"),
          media: this.MetaData.mcrMediaSelected,
          productId: this.EventSelected,
          onairTime: this.date,
          editor: this.MetaData.editor
        };
      }
      console.log(data);
    },
    ...mapMutations("FileIndexStore", [
      "setUploaderCustomData",
      "setEditor",
      "setVueTableData",
      "setProcessing",
      "setFileUploading",
      "resetTitle",
      "resetMemo",
      "resetEditor",
      "resetType"
    ]),
    resetEvent() {
      this.$emit("reset");
    },
    getCategory(v) {
      if (v == 0) {
        return "My 디스크";
      } else if (v == 1) {
        return "프로소재";
      } else if (v == 2) {
        return "프로그램";
      } else if (v == 3) {
        return "주조SPOT";
      } else if (v == 4) {
        return "부조SPOT";
      } else if (v == 5) {
        return "FILLER";
      } else if (v == 6) {
        return "취재물";
      } else if (v == 7) {
        return "고정소재";
      } else if (v == 8) {
        return "변동소재";
      }
    },
    inputEditor(v) {
      this.setEditor(v.id);
    },
    MetaModalOff() {
      if (this.processing || this.fileUploading) {
        console.log(this.processing);
        console.log(this.fileUploading);
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
            memo: this.MetaData.memo
          };
        } else if (this.MetaData.typeSelected == "program") {
          var data = {
            memo: this.MetaData.memo,
            UserId: sessionStorage.getItem("user_id"),
            media: this.MetaData.proMediaSelected,
            productId: this.ProgramGrid.productId,
            onairTime: this.ProgramGrid.onairTime,
            editor: this.MetaData.editor
          };
        } else if (this.MetaData.typeSelected == "mcr-spot") {
          var data = {
            memo: this.MetaData.memo,
            UserId: sessionStorage.getItem("user_id"),
            media: this.MetaData.mcrMediaSelected,
            productId: this.EventSelected,
            onairTime: this.date,
            editor: this.MetaData.editor
          };
        }
        this.setUploaderCustomData(data);
        this.setProcessing(true);
        // this.verifyMeta({
        //   type: this.MetaData.typeSelected,
        //   title: this.MetaData.title,
        //   files: this.localFiles,
        //   categoryCD: this.MetaData.categoryCD
        // }).then(res => {
        this.setProcessing(false);
        if (this.cancel) {
          this.cancel = false;
          return;
        } else {
          this.setFileUploading(true);
          this.$emit("upload");
        }
        // });
      } else if (!this.metaValid) {
        this.$fn.notify("error", { title: "메타 데이터 확인" });
      }
    },
    proData(v) {
      this.ProgramGrid = v;
    },
    mcrData(v) {
      this.EventSelected = v;
    },
    mcrDate(v) {
      this.date = v;
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
    }
  }
};
</script>

<style>
.defaultButton {
  background-color: #fff !important;
  border-color: #bbbbbb !important;
  color: #3a3a3a !important;
}
</style>
