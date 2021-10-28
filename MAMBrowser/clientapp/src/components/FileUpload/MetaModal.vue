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
            <div style="height:600px; overflow-y:auto; ">
              <div style="position:relative; top:20px;">
                <p style=" font-size:16px;">
                  파일 명 : {{ this.MetaModalTitle }}
                </p>
                <div style="width:550px; height:80px; margin-bottom:10px;">
                  <h3 style="color:#008ECC;">
                    소재 유형
                  </h3>
                  <b-form-select
                    style="width:300px;"
                    id="filetype"
                    v-model="MetaData.typeSelected"
                    :options="typeOptions"
                    :state="!typeState"
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
                      :data-source="vueTableData"
                      :selection="{ mode: 'single' }"
                      :show-borders="true"
                      :hover-state-enabled="true"
                      key-expr="id"
                      @selection-changed="onSelectionChanged"
                    >
                      <DxColumn
                        :width="70"
                        data-field="Prefix"
                        caption="Title"
                      />
                      <DxColumn data-field="FirstName" />
                      <DxColumn data-field="LastName" />
                      <DxColumn :width="180" data-field="Position" />
                      <DxColumn data-field="BirthDate" data-type="date" />
                      <DxColumn data-field="HireDate" data-type="date" />
                    </DxDataGrid>
                  </div>
                </div>
                <h3 style="color:#008ECC; margin-top:-10px;">
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
                      @click="titleReset"
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
                      @click="memoReset"
                    ></b-icon>
                  </button>
                </div>
                <div style="height:50px;  margin-top : 10px;" v-show="isActive">
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
                      @click="titleReset"
                    ></b-icon>
                  </button>
                </div>
              </div>
            </div>
          </div>
        </h4>
        <h3 slot="footer">
          <div :class="[isActive ? 'date-modal-button' : 'file-modal-button']">
            <b-button
              variant="outline-danger"
              class="modal-file-reset-button"
              @click="reset()"
            >
              취소
            </b-button>

            <b-button
              variant="outline-success"
              class="modal-file-processing-button"
              v-show="processing && !fileUploading"
            >
              <b-spinner small type="grow"></b-spinner>
              <span class="label">확인중...</span>
            </b-button>
            <b-button
              variant="outline-success"
              class="modal-file-processing-button"
              v-show="!processing && fileUploading"
            >
              <b-spinner small type="grow"></b-spinner>
              <span class="label">업로드 중...</span>
            </b-button>
            <span v-show="metaValid">
              <b-button
                variant="outline-success"
                @click="uploadfile()"
                class="modal-file-upload-button"
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
                class="modal-file-upload-button"
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
import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
export default {
  props: {
    fileState: {
      type: String,
      default: ""
    }
  },
  components: {
    CommonMetaModal
  },
  mixins: [CommonFileFunction],
  data() {
    return {};
  },
  computed: {
    ...mapState("FileIndexStore", {
      MetaModal: state => state.MetaModal,
      MetaModalTitle: state => state.MetaModalTitle,
      localFiles: state => state.localFiles,
      connectionId: state => state.connectionId,
      vueTableData: state => state.vueTableData
    })
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
      "MetaModalOff",
      "setUploaderCustomData"
    ]),
    async uploadfile() {
      if (this.metaValid) {
        //NOTE: 커스텀 데이터 파라미터
        var data = {
          user_id: sessionStorage.getItem("user_id"),
          title: this.MetaData.title,
          memo: this.MetaData.memo,
          fileSize: this.localFiles[0].size,
          connectionId: this.connectionId
        };
        this.setUploaderCustomData(data);
        this.processing = true;
        this.verifyMeta({
          type: this.MetaData.typeSelected,
          title: this.MetaData.title,
          files: this.localFiles,
          categoryCD: this.MetaData.categoryCD
        }).then(res => {
          if (res) {
            this.processing = false;
            this.fileUploading = true;
            this.$emit("upload");
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
