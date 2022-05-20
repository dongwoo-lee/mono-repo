<template>
  <div>
    <transition name="slide-fade">
      <CommonMetaModal
        v-show="this.MetaModal"
        @close="MetaModalOff"
        style="font-family: MBC 새로움 M"
        :isActive="this.isActive"
      >
        <h3 slot="header">파일 업로드</h3>
        <h4 slot="body">
          <div :class="[isActive ? 'new' : 'old']">
            <div :class="[isActive ? 'fold' : 'expand']">
              <h3 style="color: black">파일 정보</h3>
              <div style="padding-top: 10px">
                <b-form-group
                  label="파일명"
                  class="has-float-label"
                  style="font-size: 15px; margin-top: 10px"
                >
                  <b-form-input
                    :title="this.MetaModalTitle"
                    style="width: 430px; font-size: 14px"
                    class="editTask title-ellipsis"
                    v-model="this.MetaModalTitle"
                    disabled
                    aria-describedby="input-live-help input-live-feedback"
                    placeholder="Title"
                    trim
                  />
                </b-form-group>
                <div style="height: 50px; margin-top: 20px">
                  <b-form-group
                    label="파일 분량"
                    class="has-float-label"
                    style="font-size: 15px"
                  >
                    <b-form-input
                      v-if="this.durationState"
                      style="width: 430px"
                      class="editTask"
                      v-model="MetaData.duration"
                      disabled
                      aria-describedby="input-live-help input-live-feedback"
                      placeholder="duration"
                      trim
                    >
                    </b-form-input>
                    <b-form-input
                      v-if="!this.durationState"
                      style="width: 430px; background-color: #ffb600"
                      class="editTask"
                      v-model="MetaData.duration"
                      disabled
                      aria-describedby="input-live-help input-live-feedback"
                      placeholder="duration"
                      trim
                    >
                    </b-form-input>
                    <!-- <b-icon-alarm
                      v-if="!this.durationState"
                      style="position: relative; top: -28px; left: 300px"
                    ></b-icon-alarm> -->
                  </b-form-group>
                </div>
                <div style="height: 50px; margin-top: 20px">
                  <b-form-group
                    label="오디오 포맷"
                    class="has-float-label"
                    style="font-size: 15px"
                  >
                    <b-form-input
                      :title="MetaData.audioFormat"
                      style="width: 430px"
                      class="editTask"
                      v-model="MetaData.audioFormat"
                      disabled
                      aria-describedby="input-live-help input-live-feedback"
                      placeholder="audioFormat"
                      trim
                    />
                  </b-form-group>
                </div>
                <div
                  style="height: 50px; margin-top: 20px"
                  v-if="!this.isActive"
                >
                  <b-form-group
                    label="제작자"
                    class="has-float-label"
                    style="font-size: 15px"
                  >
                    <b-form-input
                      title="제작자"
                      style="width: 430px; font-size: 14px"
                      class="editTask"
                      :value="userID"
                      disabled
                      aria-describedby="input-live-help input-live-feedback"
                      placeholder="제작자"
                      trim
                    />
                  </b-form-group>
                </div>
                <div style="width: 300px; margin-top: 25px">
                  <b-form-group
                    label="소재 유형"
                    class="has-float-label"
                    style="font-size: 15px"
                  >
                    <b-form-select
                      style="width: 430px"
                      id="filetype"
                      v-model="MetaData.typeSelected"
                      :options="typeOptions"
                      @change="resetMemo"
                    ></b-form-select>
                  </b-form-group>
                </div>
              </div>
            </div>
            <div :class="[isActive ? 'fold2' : 'expand2']">
              <h3>메타 데이터</h3>
              <div>
                <my-disk
                  v-if="this.MetaData.typeSelected == 'my-disk'"
                ></my-disk>
                <pro v-if="this.MetaData.typeSelected == 'pro'"></pro>
              </div>

              <transition name="slide-fade">
                <div>
                  <div v-show="!isActive" class="date-div">
                    <program
                      v-if="this.MetaData.typeSelected == 'program'"
                    ></program>
                    <mcr-spot
                      v-if="this.MetaData.typeSelected == 'mcr-spot'"
                    ></mcr-spot>
                    <scr-spot
                      v-if="this.MetaData.typeSelected == 'scr-spot'"
                    ></scr-spot>
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
                </div>
              </transition>
            </div>
          </div>
        </h4>

        <h3 slot="footer">
          <div :class="[isActive ? 'file-progress' : 'date-progress']">
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
          </div>
          <div :class="[isActive ? 'file-modal-button' : 'date-modal-button']">
            <!-- <b-button
              variant="outline-success"
              @click="log"
              style="margin-left: -80px"
            >
              <span class="label">확인</span>
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
                style="margin-left: 25px"
                v-show="!processing && !fileUploading"
              >
                <span class="label">업로드</span>
              </b-button>
            </span>
            <span v-show="!metaValid">
              <b-button
                variant="primary"
                disabled
                style="margin-left: 25px"
                v-show="!processing && !fileUploading"
              >
                <span class="label">업로드</span>
              </b-button>
            </span>
            <b-button
              variant="outline-danger"
              style="margin-left: 20px"
              @click="MetaModalOff"
            >
              취소
            </b-button>
          </div>
        </h3>
      </CommonMetaModal>
    </transition>
    <common-confirm
      id="durationOver"
      title="편성 분량 확인"
      :message="getDurationOverMsg()"
      submitBtn="업로드"
      :customClose="true"
      @ok="DurationOK()"
      @close="DurationCancel()"
    />
    <common-confirm
      id="audioClipIDOver"
      title="확인"
      :message="getAudioClipIDOverMsg()"
      submitBtn="업로드"
      :customClose="true"
      @ok="audioClipOK()"
      @close="audioClipCancel()"
    />
  </div>
</template>

<script>
import CommonMetaModal from "../Modal/CommonMetaModal";
import MixinBasicPage from "../../mixin/MixinBasicPage";
import CommonVueSelect from "../Form/CommonVueSelect.vue";
import myDisk from "./MetaData/my-disk.vue";
import program from "./MetaData/program.vue";
import pro from "./MetaData/pro.vue";
import mcrSpot from "./MetaData/mcr-spot.vue";
import scrSpot from "./MetaData/scr-spot.vue";
import staticSpot from "./MetaData/static-spot.vue";
import varSpot from "./MetaData/var-spot.vue";
import report from "./MetaData/coverage.vue";
import filler from "./MetaData/filler.vue";
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
    pro,
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
      userID: sessionStorage.getItem("user_name"),
    };
  },
  computed: {
    ...mapGetters("user", ["diskAvailable"]),
    ...mapState("FileIndexStore", {
      MetaModalTitle: (state) => state.MetaModalTitle,
      date: (state) => state.date,
      button: (state) => state.button,
      fileSDate: (state) => state.fileSDate,
      fileEDate: (state) => state.fileEDate,
      localFiles: (state) => state.localFiles,
      scrRange: (state) => state.scrRange,
      MetaData: (state) => state.MetaData,
      masteringListData: (state) => state.masteringListData,
      ProgramData: (state) => state.ProgramData,
      ProgramSelected: (state) => state.ProgramSelected,
      EventSelected: (state) => state.EventSelected,
      isActive: (state) => state.isActive,
      processing: (state) => state.processing,
      fileUploading: (state) => state.fileUploading,
      typeOptions: (state) => state.typeOptions,
      uploaderCustomData: (state) => state.uploaderCustomData,
    }),
    ...mapGetters("FileIndexStore", [
      "typeState",
      "titleState",
      "memoState",
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
        this.resetTypeOptions();
        this.typeOptionsByRole(this.getMenuGrpName);
      }
    },
  },
  methods: {
    ...mapMutations("FileIndexStore", [
      "setTitle",
      "setUploaderCustomData",
      "setMasteringListData",
      "setProcessing",
      "setFileUploading",
      "resetTitle",
      "resetMemo",
      "resetType",
      "resetTypeOptions",
      "resetUploaderCustomData",
    ]),
    resetEvent() {
      this.$emit("reset");
    },
    MetaModalOff() {
      if (this.processing || this.fileUploading) {
        this.cancel = true;
        this.$emit("cancel");
      }
      this.$emit("close");
    },
    log() {
      //NOTE: 커스텀 데이터 파라미터
      if (this.MetaData.typeSelected == "my-disk") {
        var data = {
          editor: sessionStorage.getItem("user_id"),
          title: this.MetaData.title,
          memo: this.MetaData.memo,
        };
      } else if (this.MetaData.typeSelected == "program") {
        var data = {
          title: this.MetaData.title,
          memo: this.MetaData.memo,
          media: this.MetaData.mediaSelected,
          productId: this.ProgramSelected.productId,
          brdDTM: this.ProgramSelected.onairTime,
          SchDate: this.date,
          editor: sessionStorage.getItem("user_id"),
        };
      } else if (this.MetaData.typeSelected == "pro") {
        var data = {
          editor: sessionStorage.getItem("user_id"),
          category: this.MetaData.mediaSelected,
          type: this.MetaData.proType,
          typeName: this.MetaData.proTypeName,
          title: this.MetaData.title,
          memo: this.MetaData.memo,
        };
      } else if (this.MetaData.typeSelected == "mcr-spot") {
        var data = {
          title: this.MetaData.title,
          memo: this.MetaData.memo,
          media: this.MetaData.mediaSelected,
          productId: this.EventSelected.id,
          brdDT: this.date,
          advertiser: this.MetaData.advertiser,
          editor: sessionStorage.getItem("user_id"),
        };
      } else if (this.MetaData.typeSelected == "scr-spot") {
        var data = {
          title: this.MetaData.title,
          memo: this.MetaData.memo,
          advertiser: this.MetaData.advertiser,
          editor: sessionStorage.getItem("user_id"),
          category: this.MetaData.mediaSelected,
          scrRange: JSON.stringify(this.scrRange),
        };
      } else if (this.MetaData.typeSelected == "static-spot") {
        var data = {
          title: this.MetaData.title,
          media: this.MetaData.mediaSelected,
          productId: this.EventSelected.id,
          SDate: this.fileSDate,
          EDate: this.fileEDate,
          editor: sessionStorage.getItem("user_id"),
          memo: this.MetaData.memo,
          advertiser: this.MetaData.advertiser,
        };
      } else if (this.MetaData.typeSelected == "var-spot") {
        var data = {
          title: this.MetaData.title,
          media: this.MetaData.mediaSelected,
          productId: this.EventSelected.id,
          SDate: this.fileSDate,
          EDate: this.fileEDate,
          editor: sessionStorage.getItem("user_id"),
          memo: this.MetaData.memo,
          advertiser: this.MetaData.advertiser,
        };
      } else if (this.MetaData.typeSelected == "report") {
        var data = {
          title: this.MetaData.title,
          category: this.MetaData.coverageTypeSelected,
          ProductId: this.EventSelected.productId,
          brdDT: this.date,
          editor: sessionStorage.getItem("user_id"),
          memo: this.MetaData.memo,
          reporter: this.MetaData.reporter,
        };
      } else if (this.MetaData.typeSelected == "filler") {
        var data = {
          category: this.MetaData.fillerTypeSelected,
          title: this.MetaData.title,
          memo: this.MetaData.memo,
          editor: sessionStorage.getItem("user_id"),
          brdDT: this.date,
        };
      }
      console.log(data);
    },
    uploadfile() {
      if (this.metaValid) {
        //NOTE: 커스텀 데이터 파라미터
        if (this.MetaData.typeSelected == "my-disk") {
          var data = {
            editor: sessionStorage.getItem("user_id"),
            title: this.MetaData.title,
            memo: this.MetaData.memo,
          };
        } else if (this.MetaData.typeSelected == "program") {
          var data = {
            title: this.MetaData.title,
            memo: this.MetaData.memo,
            media: this.MetaData.mediaSelected,
            productId: this.ProgramSelected.productId,
            brdDTM: this.ProgramSelected.onairTime,
            SchDate: this.date,
            editor: sessionStorage.getItem("user_id"),
          };
        } else if (this.MetaData.typeSelected == "pro") {
          var data = {
            editor: sessionStorage.getItem("user_id"),
            category: this.MetaData.mediaSelected,
            type: this.MetaData.proType,
            typeName: this.MetaData.proTypeName,
            title: this.MetaData.title,
            memo: this.MetaData.memo,
          };
        } else if (this.MetaData.typeSelected == "mcr-spot") {
          var data = {
            title: this.MetaData.title,
            memo: this.MetaData.memo,
            media: this.MetaData.mediaSelected,
            productId: this.EventSelected.id,
            brdDT: this.date,
            advertiser: this.MetaData.advertiser,
            editor: sessionStorage.getItem("user_id"),
          };
        } else if (this.MetaData.typeSelected == "scr-spot") {
          var data = {
            title: this.MetaData.title,
            memo: this.MetaData.memo,
            advertiser: this.MetaData.advertiser,
            editor: sessionStorage.getItem("user_id"),
            category: this.MetaData.mediaSelected,
            scrRange: JSON.stringify(this.scrRange),
          };
        } else if (this.MetaData.typeSelected == "static-spot") {
          var data = {
            title: this.MetaData.title,
            media: this.MetaData.mediaSelected,
            productId: this.EventSelected.id,
            SDate: this.fileSDate,
            EDate: this.fileEDate,
            editor: sessionStorage.getItem("user_id"),
            memo: this.MetaData.memo,
            advertiser: this.MetaData.advertiser,
          };
        } else if (this.MetaData.typeSelected == "var-spot") {
          var data = {
            title: this.MetaData.title,
            media: this.MetaData.mediaSelected,
            productId: this.EventSelected.id,
            SDate: this.fileSDate,
            EDate: this.fileEDate,
            editor: sessionStorage.getItem("user_id"),
            memo: this.MetaData.memo,
            advertiser: this.MetaData.advertiser,
          };
        } else if (this.MetaData.typeSelected == "report") {
          var data = {
            title: this.MetaData.title,
            category: this.MetaData.coverageTypeSelected,
            productId: this.EventSelected.productId,
            brdDT: this.date,
            editor: sessionStorage.getItem("user_id"),
            memo: this.MetaData.memo,
            reporter: this.MetaData.reporter,
          };
        } else if (this.MetaData.typeSelected == "filler") {
          var data = {
            category: this.MetaData.fillerTypeSelected,
            title: this.MetaData.title,
            memo: this.MetaData.memo,
            editor: sessionStorage.getItem("user_id"),
            brdDT: this.date,
          };
        }
        this.resetUploaderCustomData();
        this.setUploaderCustomData(data);
        if (!this.durationState) {
          this.$bvModal.show("durationOver");
          return;
        }

        if (!this.audioClipIdState) {
          if (
            this.MetaData.typeSelected == "program" ||
            this.MetaData.typeSelected == "mcr-spot"
          ) {
            this.$bvModal.show("audioClipIDOver");
            return;
          }
        }

        if (this.MetaData.typeSelected == "my-disk") {
          if (!this.notDiskAvailable(this.localFiles[0].size)) {
            this.$fn.notify("error", { title: "용량 부족" });
            return;
          }

          //title 유효성 검사
          var formData = new FormData();

          formData.append("userId", sessionStorage.getItem("user_id"));
          formData.append("title", this.MetaData.title);
          formData.append("fileSize", this.localFiles[0].size);
          formData.append("fileName", this.localFiles[0].name);
          axios.post(`/api/mastering/TitleValidation`, formData).then((res) => {
            if (res.status == 200 && res.data.resultCode == 0) {
              this.setFileUploading(true);
              this.$emit("upload");
            } else {
              this.$fn.notify("error", { title: res.data.errorMsg });
              return;
            }
          });
        } else {
          this.setFileUploading(true);
          this.$emit("upload");
        }
      } else if (!this.metaValid) {
        this.$fn.notify("error", { title: "메타 데이터 확인" });
      }
    },
    notDiskAvailable(files) {
      const result = this.diskAvailable - files;
      if (result < 0) {
        return false;
      } else {
        return true;
      }
    },
    DurationOK() {
      this.$bvModal.hide("durationOver");

      if (this.MetaData.typeSelected == "program") {
        if (this.ProgramSelected.audioClipID == null) {
          this.setFileUploading(true);
          this.$emit("upload");
        } else {
          this.$bvModal.show("audioClipIDOver");
        }
      } else if (this.MetaData.typeSelected == "mcr-spot") {
        if (this.EventSelected.audioClipID == null) {
          this.setFileUploading(true);
          this.$emit("upload");
        } else {
          this.$bvModal.show("audioClipIDOver");
        }
      } else {
        this.setFileUploading(true);
        this.$emit("upload");
      }
    },
    DurationCancel() {
      this.$bvModal.hide("durationOver");
    },
    getDurationOverMsg() {
      return `<text style="color:red;">편성 분량을 확인하세요.</text><br><br><text style="color:red;">정말 업로드 하시겠습니까?</text>`;
    },
    audioClipOK() {
      if (this.MetaData.typeSelected == "program") {
        axios
          .delete(
            `/api/mastering/program/${this.ProgramSelected.audioClipID}`,
            {
              headers: {
                Authorization: sessionStorage.getItem("access_token"),
              },
            }
          )
          .then((res) => {
            if (res.status == 200 && res.statusText == "OK") {
              this.$bvModal.hide("audioClipIDOver");
              this.setFileUploading(true);
              this.$emit("upload");
            } else {
              this.$fn.notify("error", { title: res.data.errorMsg });
            }
          })
          .catch((err) => {
            this.$bvModal.hide("audioClipIDOver");
            this.setFileUploading(false);
            this.$fn.notify("error", { title: err.message });
          });
      } else if (this.MetaData.typeSelected == "mcr-spot") {
        axios
          .delete(`/api/mastering/mcr-spot/${this.EventSelected.audioClipID}`, {
            headers: { Authorization: sessionStorage.getItem("access_token") },
          })
          .then((res) => {
            if (res.status == 200 && res.statusText == "OK") {
              this.$bvModal.hide("audioClipIDOver");
              this.setFileUploading(true);
              this.$emit("upload");
            } else {
              this.$fn.notify("error", { title: res.data.errorMsg });
            }
          })
          .catch((err) => {
            this.$bvModal.hide("audioClipIDOver");
            this.setFileUploading(false);
            this.$fn.notify("error", { title: err.message });
          });
      }
    },
    audioClipCancel() {
      this.$bvModal.hide("audioClipIDOver");
    },
    getAudioClipIDOverMsg() {
      return `<text style="color:red;">이미 등록되어 있습니다.</text><br><br><text style="color:red;">덮어 쓰시겠습니까?</text>`;
    },
    typeOptionsByRole(role) {
      if (this.button == "nav") {
        if (role == "관리자") {
          this.typeOptions.push({
            value: "null",
            text: "소재유형을 선택해주세요",
          });
          this.typeOptions.push({ value: "my-disk", text: "My디스크" });
          this.typeOptions.push({ value: "program", text: "프로그램" });
          this.typeOptions.push({ value: "pro", text: "프로소재" });
          this.typeOptions.push({ value: "mcr-spot", text: "주조SPOT" });
          this.typeOptions.push({ value: "scr-spot", text: "부조SPOT" });
          this.typeOptions.push({
            value: "static-spot",
            text: "고정소재(필러/시간)",
          });
          this.typeOptions.push({
            value: "var-spot",
            text: "변동소재(필러/시간)",
          });
          this.typeOptions.push({ value: "report", text: "취재물" });
          this.typeOptions.push({
            value: "filler",
            text: "필러(PR, 소재, 기타)",
          });
        } else if (role == "편성PD") {
          this.typeOptions.push({
            value: "null",
            text: "소재유형을 선택해주세요",
          });
          this.typeOptions.push({ value: "my-disk", text: "My디스크" });
          this.typeOptions.push({ value: "program", text: "프로그램" });
          this.typeOptions.push({ value: "pro", text: "프로소재" });
          this.typeOptions.push({ value: "mcr-spot", text: "주조SPOT" });
          this.typeOptions.push({ value: "scr-spot", text: "부조SPOT" });
          this.typeOptions.push({
            value: "static-spot",
            text: "고정소재(필러/시간)",
          });
          this.typeOptions.push({
            value: "var-spot",
            text: "변동소재(필러/시간)",
          });
          this.typeOptions.push({ value: "report", text: "취재물" });
          this.typeOptions.push({
            value: "filler",
            text: "필러(PR, 소재, 기타)",
          });
        } else if (role == "제작PD") {
          this.typeOptions.push({
            value: "null",
            text: "소재유형을 선택해주세요",
          });
          this.typeOptions.push({ value: "my-disk", text: "My디스크" });
          this.typeOptions.push({ value: "program", text: "프로그램" });
          this.typeOptions.push({ value: "pro", text: "프로소재" });
          this.typeOptions.push({
            value: "filler",
            text: "필러(PR, 소재, 기타)",
          });
        } else if (role == "리포터") {
          this.typeOptions.push({
            value: "null",
            text: "소재유형을 선택해주세요",
          });
          this.typeOptions.push({ value: "my-disk", text: "My디스크" });
          this.typeOptions.push({ value: "pro", text: "프로소재" });
          this.typeOptions.push({ value: "report", text: "취재물" });
        } else if (role == "라디오뉴스") {
          this.typeOptions.push({
            value: "null",
            text: "소재유형을 선택해주세요",
          });
          this.typeOptions.push({ value: "my-disk", text: "My디스크" });
          this.typeOptions.push({ value: "report", text: "취재물" });
        } else if (role == "제작 Staff") {
          this.typeOptions.push({
            value: "null",
            text: "소재유형을 선택해주세요",
          });
          this.typeOptions.push({ value: "my-disk", text: "My디스크" });
          this.typeOptions.push({ value: "pro", text: "프로소재" });
        } else if (role == "TD") {
          this.typeOptions.push({
            value: "null",
            text: "소재유형을 선택해주세요",
          });
          this.typeOptions.push({ value: "my-disk", text: "My디스크" });
          this.typeOptions.push({ value: "pro", text: "프로소재" });
        }
      } else if (this.button == "private") {
        this.typeOptions.push({ value: "my-disk", text: "My디스크" });
      } else {
        this.typeOptions.push({ value: "my-disk", text: "My디스크" });
      }
    },
  },
};
</script>

<style>
.expand {
  width: 470px;
  height: 410px;
  margin-top: 15px;
  padding-top: 20px;
  margin-left: 20px;
  padding-left: 20px;
  margin-bottom: 20px;
  border: 1px solid silver;
  float: left;
}
.fold {
  width: 470px;
  height: 340px;
  margin-top: 15px;
  padding-top: 20px;
  margin-left: 20px;
  padding-left: 20px;
  margin-bottom: 20px;
  border: 1px solid silver;
  float: left;
}
.expand2 {
  border: 1px solid silver;
  width: 470px;
  height: 410px;
  margin-top: 15px;
  padding-top: 20px;
  margin-left: 510px;
  padding-left: 20px;
}
.fold2 {
  border: 1px solid silver;
  width: 470px;
  height: 340px;
  margin-top: 15px;
  padding-top: 20px;
  margin-left: 510px;
  padding-left: 20px;
}

.new {
  width: 1000px;
  height: 300px;
  float: left;
}
.old {
  width: 1000px;
  height: 300px;
  /* height: 470px; */
  float: left;
}
.file-progress {
  margin-top: 0px;
  margin-bottom: 0px;
  margin-left: 20px;
  width: 960px;
}
.date-progress {
  margin-top: 80px;
  margin-bottom: -80px;
  margin-left: 20px;
  width: 960px;
}
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
