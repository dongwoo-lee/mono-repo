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
          <div style="width:1000px; height:540px; float:left;">
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

                  <div style="height:50px;  margin-top : 10px;">
                    <b-form-input
                      class="editTask"
                      v-model="MetaData.title"
                      :state="titleState"
                      placeholder="제목"
                      trim
                    />

                    <button
                      style="position:relative; left:315px; top:-30px; z-index:99; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
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
                      style="position:relative; left:315px; top:-30px; z-index:99; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
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
                      style="position:relative; left:315px; top:-30px; z-index:99; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
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
                      style="position:relative; left:315px; top:-30px; z-index:99; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
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
            <div v-if="!isActive" class="date-div">
              <h3 style="color:#008ECC; ">
                프로그램 선택
              </h3>
              <div style="margin-top:15px;">
                <b-form-group
                  label="방송일"
                  class="has-float-label"
                  style="position:absolute; z-index:9989; font-color:black;"
                >
                  <b-input-group class="mb-3" style="width:300px; float:left;">
                    <input
                      :disabled="isActive"
                      id="dateinput"
                      type="text"
                      class="form-control input-picker date-input"
                      :value="date"
                      @input="onInput"
                    />
                    <b-input-group-append>
                      <b-form-datepicker
                        v-model="date"
                        button-only
                        :disabled="isActive"
                        :button-variant="getVariant"
                        right
                        aria-controls="example-input"
                        @context="onContext"
                      ></b-form-datepicker>
                    </b-input-group-append>
                  </b-input-group>
                </b-form-group>
                <button
                  v-show="!isActive"
                  style="position:absolute; right:340px; top:45px;  z-index:9999; width:3px;  background-color:#FFFFFF; border:0; outline:0;"
                >
                  <b-icon
                    icon="x-circle"
                    font-scale="1"
                    style="position:absolute; z-index:9999;"
                    variant="secondary"
                    @click="dateReset"
                  ></b-icon>
                </button>

                <b-form-group
                  label="매체"
                  class="has-float-label"
                  style="position:absolute; margin-left:320px; z-index:9999;"
                >
                  <b-form-select
                    :disabled="isActive"
                    id="program-media"
                    class="media-select"
                    style=" width:140px; height:37px;"
                    v-model="MetaData.mediaSelected"
                    :options="mediaOptions"
                  />
                </b-form-group>
                <b-button
                  :disabled="isActive"
                  :variant="getVariant"
                  style="position:absolute; margin-left:484px; z-index:9989; "
                  @click="getPro"
                  >검색</b-button
                >
              </div>
              <div v-show="!isActive" class="data-grid-div">
                <!-- //TODO: Data Binding -->
                <DxDataGrid
                  v-show="this.ProgramData.eventName != ''"
                  style="height:208px;"
                  :data-source="ProgramData"
                  :selection="{ mode: 'single' }"
                  :show-borders="true"
                  :hover-state-enabled="true"
                  key-expr="productId"
                  :allow-column-resizing="true"
                  :column-auto-width="true"
                  no-data-text="No Data"
                  @row-click="onRowClick"
                >
                  <DxColumn data-field="eventName" caption="이벤트 명" />
                  <DxColumn :width="60" data-field="eventType" caption="타입" />
                  <DxColumn data-field="productId" caption="프로그램 ID" />
                  <DxColumn data-field="onairTime" caption="방송 시간" />
                  <DxColumn data-field="durationSec" caption="편성 분량" />
                </DxDataGrid>
              </div>
              <div
                v-show="!isActive && ProgramGrid.eventName != ''"
                style="width: 550px; height:140px; margin-top:10px; padding-left:10px; padding-right:10px; float:left; border:1px solid #008ecc;"
              >
                <div style="width:180px; float:left;">
                  <b-form-group
                    label="이벤트 명"
                    class="has-float-label"
                    style="margin-top:20px;"
                  >
                    <b-form-input
                      style="width:180px;"
                      class="editTask"
                      v-model="ProgramGrid.eventName"
                      readonly
                      aria-describedby="input-live-help input-live-feedback"
                      trim
                    />
                  </b-form-group>
                </div>
                <div style="width:170px; margin-left:20px; float:left;">
                  <b-form-group
                    label="프로그램 ID"
                    class="has-float-label"
                    style="margin-top:20px;"
                  >
                    <b-form-input
                      style="width:170px;"
                      class="editTask"
                      v-model="ProgramGrid.productId"
                      readonly
                      aria-describedby="input-live-help input-live-feedback"
                      trim
                    />
                  </b-form-group>
                </div>
                <div style="width:120px; margin-left:400px;">
                  <b-form-group
                    label="이벤트 타입"
                    class="has-float-label"
                    style="margin-top:20px;"
                  >
                    <b-form-input
                      style="width:120px;"
                      class="editTask"
                      v-model="ProgramGrid.eventType"
                      readonly
                      aria-describedby="input-live-help input-live-feedback"
                      trim
                    />
                  </b-form-group>
                </div>

                <div style="width:200px; float:left;">
                  <b-form-group label="방송 시간" class="has-float-label">
                    <b-form-input
                      style="width:200px;"
                      class="editTask"
                      v-model="ProgramGrid.onairTime"
                      readonly
                      aria-describedby="input-live-help input-live-feedback"
                      trim
                    />
                  </b-form-group>
                </div>
                <div style="width:200px; margin-left:20px; float:left;">
                  <b-form-group label="편성 분량" class="has-float-label">
                    <b-form-input
                      style="width:120px;"
                      class="editTask"
                      v-model="ProgramGrid.durationSec"
                      readonly
                      aria-describedby="input-live-help input-live-feedback"
                      trim
                    />
                  </b-form-group>
                </div>
              </div>
            </div>
          </div>
        </h4>

        <h3 slot="footer">
          <div :class="[isActive ? 'date-modal-button' : 'file-modal-button']">
            <b-button variant="outline-danger" @click="reset()">
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
import CommonFileFunction from "./CommonFileFunction";
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
    CommonMetaModal
  },
  mixins: [CommonFileFunction],
  data() {
    return {
      cancel: false
    };
  },
  computed: {
    ...mapState("FileIndexStore", {
      MetaModalTitle: state => state.MetaModalTitle,
      localFiles: state => state.localFiles,
      MetaData: state => state.MetaData,
      connectionId: state => state.connectionId,
      vueTableData: state => state.vueTableData,
      ProgramData: state => state.ProgramData
    }),
    ...mapGetters("FileIndexStore", [
      "typeState",
      "titleState",
      "memoState",
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
        this.fileUploading = false;
      } else if (v == "리셋") {
        this.reset();
      }
    },
    MetaModal(v) {
      if (!v) {
        this.reset();
      } else {
        if (this.typeOptions.length == 1) {
          this.typeOptionsByRole(this.getMenuGrpName);
        }
      }

      //  { value: "private", text: "My디스크" },
      //   { value: "program", text: "프로그램" },
      //   { value: "mcrspot", text: "주조SPOT" },
      //   { value: "scrspot", text: "부조SPOT" },
      //   { value: "static", text: "고정소재" },
      //   { value: "var", text: "변동소재" },
      //   { value: "report", text: "취재물" },
      //   { value: "filler", text: "필러" }
    }
  },
  methods: {
    ...mapMutations("FileIndexStore", [
      "setUploaderCustomData",
      "resetTitle",
      "resetMemo",
      "resetType"
    ]),
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

        var data = {
          user_id: sessionStorage.getItem("user_id"),
          title: this.MetaData.title,
          memo: this.MetaData.memo,
          type: this.MetaData.typeSelected,
          fileSize: this.localFiles[0].size,
          connectionId: this.connectionId,
          ProgramSelected: this.ProgramSelected
        };
        this.setUploaderCustomData(data);
        this.processing = true;
        this.verifyMeta({
          type: this.MetaData.typeSelected,
          title: this.MetaData.title,
          files: this.localFiles,
          categoryCD: this.MetaData.categoryCD
        }).then(res => {
          this.processing = false;
          if (this.cancel) {
            return;
          } else {
            if (res) {
              this.$emit("upload");
            }
          }
        });
      } else if (!this.metaValid) {
        this.$fn.notify("error", { title: "메타 데이터 확인" });
      }
    },
    typeOptionsByRole(role) {
      if (role == "관리자") {
        this.typeOptions.push({ value: "private", text: "My디스크" });
        this.typeOptions.push({ value: "program", text: "프로그램" });
        this.typeOptions.push({ value: "mcrspot", text: "주조SPOT" });
        this.typeOptions.push({ value: "scrspot", text: "부조SPOT" });
        this.typeOptions.push({ value: "static", text: "고정소재" });
        this.typeOptions.push({ value: "var", text: "변동소재" });
        this.typeOptions.push({ value: "report", text: "취재물" });
        this.typeOptions.push({ value: "filler", text: "필러" });
      } else if (role == "편성PD") {
        this.typeOptions.push({ value: "private", text: "My디스크" });
        this.typeOptions.push({ value: "program", text: "프로그램" });
        this.typeOptions.push({ value: "scrspot", text: "부조SPOT" });
        this.typeOptions.push({ value: "report", text: "취재물" });
        this.typeOptions.push({ value: "mcrspot", text: "주조SPOT" });
        this.typeOptions.push({ value: "filler", text: "필러" });
        this.typeOptions.push({ value: "static", text: "고정소재" });
        this.typeOptions.push({ value: "var", text: "변동소재" });
      } else if (role == "제작PD") {
        this.typeOptions.push({ value: "private", text: "My디스크" });
        this.typeOptions.push({ value: "program", text: "프로그램" });
        this.typeOptions.push({ value: "filler", text: "필러" });
      } else if (role == "리포터") {
        this.typeOptions.push({ value: "private", text: "My디스크" });
        this.typeOptions.push({ value: "report", text: "취재물" });
      } else if (role == "라디오뉴스") {
        this.typeOptions.push({ value: "private", text: "My디스크" });
        this.typeOptions.push({ value: "report", text: "취재물" });
      } else if (role == "제작 Staff") {
        this.typeOptions.push({ value: "private", text: "My디스크" });
      } else if (role == "TD") {
        this.typeOptions.push({ value: "private", text: "My디스크" });
      }
    }
  }
};
</script>

<style scoped></style>
