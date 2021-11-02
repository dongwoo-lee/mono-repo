<template>
  <div>
    <transition name="slide-fade">
      <CommonMetaModal
        v-show="this.MetaModal"
        @close="MetaModalOff"
        style="font-family: MBC 새로움 M;"
      >
        <h3 slot="header">
          파일 업로드 메타 데이터 입력
        </h3>
        <h4 slot="body">
          <div :class="[isActive ? 'date-modal' : 'file-modal']">
            <div>
              <div style="position:relative; top:20px; ">
                <span style="width:500px; margin-bottom:20px;">
                  <h6
                    style=" font-size:16px; width:500px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"
                  >
                    파일 명 : {{ this.MetaModalTitle }}
                  </h6>
                  <b-progress
                    class="w-85"
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
                </span>
                <div
                  style="width:550px; height:80px; margin-top:20px; margin-bottom:10px;"
                >
                  <h3 style="color:#008ECC;">
                    소재 유형
                  </h3>
                  <b-form-select
                    style="width:300px;"
                    id="filetype"
                    v-model="MetaData.typeSelected"
                    :options="typeOptions"
                    :state="typeState"
                    required
                  ></b-form-select>
                </div>

                <div class="date-div" v-show="isActive">
                  <h3 style="color:#008ECC; ">
                    프로그램 선택
                  </h3>
                  <div style="margin-top:15px;">
                    <b-form-group
                      label="방송일"
                      class="has-float-label"
                      style="position:absolute; z-index:9989; font-color:black;"
                    >
                      <b-input-group
                        class="mb-3"
                        style="width:300px; float:left;"
                      >
                        <input
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
                            button-variant="outline-primary"
                            right
                            aria-controls="example-input"
                            @context="onContext"
                          ></b-form-datepicker>
                        </b-input-group-append>
                      </b-input-group>
                    </b-form-group>
                    <button
                      style="position:absolute; right:210px; top:32px; z-index:9999; width:3px;  background-color:#FFFFFF; border:0; outline:0;"
                    >
                      <b-icon
                        icon="x-circle"
                        font-scale="1"
                        style="position:absolute; top:15px; right:110px; z-index:9999;"
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
                        id="program-media"
                        class="media-select"
                        style=" width:140px; height:37px;"
                        v-model="MetaData.mediaSelected"
                        :options="mediaOptions"
                      />
                    </b-form-group>
                    <b-button
                      variant="outline-primary"
                      style="position:absolute; margin-left:484px; z-index:9989; "
                      @click="getPro"
                      >검색</b-button
                    >
                  </div>
                  <div v-show="isActive" class="data-grid-div">
                    <!-- //TODO: Data Binding -->
                    <DxDataGrid
                      style="height:208px;"
                      :data-source="ProgramData"
                      :selection="{ mode: 'single' }"
                      :show-borders="true"
                      :hover-state-enabled="true"
                      key-expr="productId"
                      :allow-column-resizing="true"
                      :column-auto-width="true"
                      no-data-text="No Data"
                      @selection-changed="onSelectionChanged"
                    >
                      <DxColumn data-field="eventName" caption="이벤트 명" />
                      <DxColumn
                        :width="60"
                        data-field="eventType"
                        caption="타입"
                      />
                      <DxColumn data-field="productId" caption="프로그램 ID" />
                      <DxColumn data-field="onairTime" caption="방송 시간" />
                      <DxColumn data-field="durationSec" caption="편성 분량" />
                    </DxDataGrid>
                  </div>
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
                    style="position:relative; left:365px; top:-30px; z-index:99; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
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
                    style="position:relative; left:365px; top:-30px; z-index:99; width:3px; heigth:3px; background-color:#FFFFFF; border:0; outline:0;"
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
                <b-form-input
                  class="editTask"
                  v-model="MetaData.duration"
                  readonly
                  aria-describedby="input-live-help input-live-feedback"
                  placeholder="duration"
                  trim
                />
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
    ])
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
      }
    }
  },
  methods: {
    ...mapMutations("FileIndexStore", [
      "setUploaderCustomData",
      "resetTitle",
      "resetMemo",
      "resetType",
      "setDuration",
      "setAudioFormat"
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
              let form = new FormData();
              form.append("metaData", "0");

              axios.post("/api/fileupload/Validation", form).then(res => {
                console.log(res);
                this.setDuration(res.data.duration);
                this.setAudioFormat(res.data.audioFormat);
                this.fileUploading = true;
                this.$emit("upload");
              });
            }
          }
        });
      } else if (!this.metaValid) {
        this.$fn.notify("error", { title: "메타 데이터 확인" });
      }
    }
  }
};
</script>

<style></style>
