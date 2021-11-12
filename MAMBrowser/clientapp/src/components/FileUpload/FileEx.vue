<template>
  <!-- props 로 modal 값 제어 -->
  <div id="file-container">
    <div
      id="dropzone-external"
      class="dropzone"
      v-show="dropzone"
      style="position: fixed; z-index: 9800; top: -80px; left: -80px; width: 2000px; height: 120%; background-color: rgba(0, 0, 0, 0.5); display: table;"
      :class="[
        isDropZoneActive
          ? 'dx-theme-accent-as-border-color dropzone-active'
          : 'dx-theme-border-color'
      ]"
    >
      <p
        style="margin-top:500px; margin-left:200px; text-align:center;color:white; font-size:48px;"
      >
        음원 파일 업로드
      </p>
      <div id="dropzone-text" class="flex-box" style="margin-top:80px;">
        <span
          style="position:absolute; z-index:1; color:white;  margin-top:700px;font-size:48px;"
        >
        </span>
      </div>
    </div>
    <b-button
      class="btn btn-outline-primary btn-sm default cutom-label mr-2"
      id="fileuploadbutton"
      @click="openModal()"
      style="position:absolute; top:-80px; right:580px; z-index:1030; border-color:#008ECC; color:#008ECC; background-color:white;"
      >파일 업로드
    </b-button>

    <transition name="slide-fade">
      <FileModal
        v-show="modal"
        @close="confirm()"
        style="font-family: 'Times New Roman', Times, serif; font-weight:bold;"
      >
        <h3 slot="header">
          음원 파일 업로드
          <b-button
            class="btn btn-outline-primary btn-sm default cutom-label mr-2"
            id="addFile"
            >추가</b-button
          >
          <!-- <b-button @click="exmodalon">추가</b-button> -->
          <DxFileUploader
            :chunk-size="200000"
            dialog-trigger="#addFile"
            :upload-custom-data="uploaderCustomData"
            :ref="dxfu"
            name="file"
            drop-zone=".dropzone"
            @drop-zone-enter="onDropZoneEnter"
            @drop-zone-leave="onDropZoneLeave"
            upload-url="/api/fileupload/"
            upload-mode="useButtons"
            :multiple="false"
            :visible="false"
            @valueChanged="modalon"
            @upload-started="() => (chunks = [])"
            @upload-aborted="uploadAborted"
            @upload-error="uploadError"
            @uploaded="uploadSuccess"
            @progress="onUploadProgress($event)"
          />
        </h3>
        <h4 slot="body">
          <div>
            <b-card no-body>
              <b-tabs pills justified vertical>
                <b-tab title="파일 업로드" active> </b-tab>
                <b-tab title="알림"
                  ><div>
                    <vuetable
                      :table-height="notiTable"
                      ref="vuetable-scrollable"
                      :api-mode="false"
                      :fields="notiFields"
                      :data="vtData"
                      no-data-template=""
                    >
                      <template slot="name" scope="props">
                        <div style="font-size:18px;">
                          {{ props.rowData.fileName }}
                          <!-- <button @click="rowdata(props)">확인</button> -->
                        </div>
                      </template>
                      <template slot="mastering" scope="props">
                        <div style="width:200px; height:20px;">
                          <vue-step-progress-indicator
                            :steps="[
                              '대기 중',
                              '스토리지 복사',
                              '파일 샘플링',
                              '파일 마스터링',
                              '완료'
                            ]"
                            :active-step="props.rowData.step"
                            :is-reactive="false"
                            :styles="styleData"
                            :colors="colorData"
                            style="margin-left:10px; width:670px;"
                          />
                        </div>
                      </template>
                    </vuetable>
                  </div>
                </b-tab>
                <b-tab title="로그">
                  <div style=" margin-left:20px; width:995px; height:150px;">
                    <!-- <DxDataGrid
                      :data-source="this.vtData"
                      :selection="{ mode: 'single' }"
                      :show-borders="true"
                      :hover-state-enabled="true"
                      key-expr="fileGuid"
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
                    </DxDataGrid> -->
                    <vuetable
                      :table-height="logTable"
                      ref="vuetable-scrollable"
                      :api-mode="false"
                      :fields="logFields"
                      :data="logData"
                      no-data-template=""
                    >
                      <template slot="rowNO" scope="props">
                        <div>{{ props.rowIndex + 1 }}</div>
                        <!-- <button @click="getRowData(props)">확인</button> 
                        -+ -->
                      </template>
                      <template slot="fileName" scope="props">
                        <div>
                          {{ props.rowData.fileName }}
                        </div>
                      </template>
                      <template slot="fileSize" scope="props">
                        <div>
                          {{ (props.rowData.fileSize / 1048576).toFixed(2) }} MB
                        </div>
                      </template>
                      <template slot="title" scope="props">
                        <div style="font-size:18px;">
                          {{ props.rowData.title }}
                        </div>
                      </template>
                      <template slot="memo" scope="props">
                        <div style="font-size:18px;">
                          {{ props.rowData.memo }}
                        </div>
                      </template>
                    </vuetable>
                  </div>
                </b-tab>
              </b-tabs>
            </b-card>
          </div>
        </h4>
      </FileModal>
    </transition>
    <transition name="slide-fade">
      <MetaModal
        v-show="exmodal"
        @close="exmodaloff"
        style="font-family: 'Times New Roman', Times, serif; font-weight:bold;"
      >
        <h3 slot="header">
          파일 메타 데이터 입력
        </h3>
        <h4 slot="body">
          <div :class="[isActive ? 'date-modal' : 'file-modal']">
            <div style="height:600px; overflow-y:auto; ">
              <div style="position:relative; top:20px;">
                <div style="width:550px; height:80px;">
                  <h3 style="color:#008ECC;">
                    소재 유형
                  </h3>
                  <b-form-select
                    style="width:300px;"
                    id="filetype"
                    v-model="typeSelected"
                    :options="typeOptions"
                    :state="!typeState"
                    required
                  ></b-form-select>
                </div>

                <div class="date-div" v-show="isActive">
                  <h3 style="color:#008ECC; margin-top:20px;">
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
                        @click="resetDate"
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
                        v-model="mediaSelected"
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
                    <DxDataGrid
                      :data-source="this.vtData"
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
                <h3 style="color:#008ECC; margin-top:20px;">
                  메타 데이터
                </h3>
                <div style="height:50px;  margin-top : 10px;">
                  <b-form-input
                    class="editTask"
                    v-model="title"
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
                      @click="titlereset"
                    ></b-icon>
                  </button>
                </div>

                <div style="height:50px;">
                  <b-form-input
                    class="editTask"
                    v-model="memo"
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
                      @click="memoreset"
                    ></b-icon>
                  </button>
                </div>
                <div style="height:50px;  margin-top : 10px;" v-show="isActive">
                  <b-form-input
                    class="editTask"
                    v-model="title"
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
                      @click="titlereset"
                    ></b-icon>
                  </button>
                </div>
                <div style="height:50px;  margin-top : 10px;" v-show="isActive">
                  <b-form-input
                    class="editTask"
                    v-model="title"
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
                      @click="titlereset"
                    ></b-icon>
                  </button>
                </div>
                <div style="height:40px;  margin-top : 10px;" v-show="isActive">
                  <b-form-input
                    class="editTask"
                    v-model="title"
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
                      @click="titlereset"
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
              초기화
            </b-button>

            <b-button
              variant="outline-success"
              class="modal-file-processing-button"
              @click="uploadfile()"
              v-show="processing"
            >
              <b-spinner small type="grow"></b-spinner>
              <span class="label">확인중...</span>
            </b-button>

            <b-button
              variant="outline-success"
              @click="uploadfile()"
              class="modal-file-upload-button"
              v-show="!processing"
            >
              <span class="label">업로드</span>
            </b-button>
          </div>
        </h3>
      </MetaModal>
    </transition>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import { DxDataGrid, DxColumn } from "devextreme-vue/data-grid";
import FileModal from "../Modal/CommonFileModal.vue";
import MetaModal from "../Modal/CommonMetaModal.vue";
import DxFileUploader from "devextreme-vue/file-uploader";
import DxValidator from "devextreme-vue/validator";
import DxTextBox from "devextreme-vue/text-box";
import VueStepProgressIndicator from "vue-step-progress-indicator";
import Vuetable from "vuetable-2/src/components/Vuetable";
const dxfu = "my-fileupload";
export default {
  props: {
    modalState: {
      type: Boolean,
      default: false
    }
  },
  components: {
    DxDataGrid,
    DxColumn,
    FileModal,
    MetaModal,
    DxFileUploader,
    DxTextBox,
    DxValidator,
    Vuetable,
    VueStepProgressIndicator
  },
  data() {
    return {
      dxfu,
      modalShow: false,
      processing: false,
      dropzone: false,
      isDropZoneActive: false,
      chunks: [],
      fileuploading: false,
      modal: false,
      exmodal: false,
      localFiles: {},
      logTable: "560px",
      notiTable: "560px",
      logData: [
        {
          fileName: "숀 (SHAUN) - Way Back Home.mp3",
          fileSize: 8858948,
          title: "건강한 아침 오프닝",
          memo: "12시까지 확인",
          step: 1
        },
        {
          fileName: "숀 (SHAUN) - Way Back Home.mp3",
          fileSize: 8858948,
          title: "건강한 아침 오프닝",
          memo: "12시까지 확인",
          step: 1
        }
      ],
      vtData: [
        {
          fileName: "숀 (SHAUN) - Way Back Home",
          fileSize: "83200",
          title: "건강한 아침 오프닝",
          memo: "12시까지 확인",
          step: 1
        },
        {
          fileName: "모모랜드 (MOMOLAND) - 뿜뿜",
          fileSize: "83200",
          title: "생생 정보통 오프닝",
          memo: "오늘 칼퇴",
          step: 2
        },
        {
          fileName: "CHRISTOPHER - Monogamy (Lyrics)",
          fileSize: "100",
          title: "ddd",
          memo: "sss",
          step: 1
        }
      ],
      title: "",
      memo: "",
      type: "",
      mediaCD: "",
      categoryCD: "",
      uploaderCustomData: {},
      step: 1,
      typeSelected: "f",
      mediaSelected: "a",
      isActive: false,
      date: "",
      formatted: "",
      dateselected: "",
      typeOptions: [
        { value: "f", text: "소재 유형" },
        { value: "a", text: "My디스크" }
      ],
      mediaOptions: [
        { value: "a", text: "AM" },
        { value: "f", text: "FM" }
      ],
      fileselect: false,
      logFields: [
        {
          name: "__slot:rowNO",
          title: "순서",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "7%"
        },
        {
          name: "__slot:fileName",
          title: "파일명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "40%"
        },
        {
          name: "__slot:fileSize",
          title: "파일 크기",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%"
        },
        {
          name: "__slot:title",
          title: "제목",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center"
        },
        {
          name: "__slot:memo",
          title: "메모",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center"
        }
      ],
      notiFields: [
        {
          name: "__slot:name",
          title: "파일명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "__slot:mastering",
          title: "마스터링",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "20%"
        }
        // {
        //   name: "size",
        //   title: "사이즈",
        //   titleClass: "center aligned text-center",
        //   dataClass: "center aligned text-center",
        //   width: "15%",
        //   callback: value => {
        //     return this.$fn.formatBytes(value);
        //   }
        // },
        // {
        //   name: "__slot:state",
        //   title: "상태",
        //   titleClass: "center aligned text-center",
        //   dataClass: "center aligned text-center",
        //   width: "10%"
        // },
        // {
        //   name: "__slot:remark",
        //   title: "비고",
        //   titleClass: "center aligned text-center",
        //   dataClass: "center aligned text-center",
        //   width: "20%"
        // },
        // {
        //   name: "__slot:actions",
        //   title: "추가작업",
        //   titleClass: "center aligned text-center",
        //   dataClass: "center aligned text-center",
        //   width: "7%"
        // }
      ],
      styleData: {
        progress__block: {
          display: "flex",
          alignItems: "center"
        },
        progress__bridge: {
          backgroundColor: "grey",
          height: "2px",
          flexGrow: "1",
          width: "20px"
        },
        progress__bubble: {
          margin: "0",
          padding: "0",
          lineHeight: "10px",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          height: "18px",
          width: "18px",
          borderRadius: "10%",
          backgroundColor: "transparent",
          border: "2px grey solid",
          textAlign: "center"
        },
        progress__label: {
          margin: "0 0.8rem",
          font: "14px;"
        }
      },
      colorData: {
        progress__bubble: {
          active: {
            color: "#fff",
            backgroundColor: "#27ae60",
            borderColor: "#27ae60"
          },
          inactive: {
            color: "#fff",
            backgroundColor: "#EF5350",
            borderColor: "#EF5350"
          },
          completed: {
            color: "#fff",
            borderColor: "#27ae60",
            backgroundColor: "#27ae60"
          }
        },
        progress__label: {
          active: {
            color: "#27ae60"
          },
          inactive: {
            color: "#EF5350"
          },
          completed: {
            color: "#27ae60"
          }
        }
      }
    };
  },
  computed: {
    ...mapGetters("menu", ["getMenuType"]),
    fileupload: function() {
      return this.$refs[dxfu].instance;
    },
    typeState() {
      return this.typeSelected == "f" ? true : false;
    },
    titleState() {
      return this.title.length >= 1 ? true : false;
    },
    memoState() {
      return this.memo.length >= 1 ? true : false;
    },
    metavalid() {
      if (
        this.title.length >= 1 &&
        this.memo.length >= 1 &&
        this.typeSelected != "f"
      )
        return true;
      else return false;
    }
  },
  watch: {
    typeSelected: function(v) {
      if (v == "f") {
        this.isActive = false;
      } else if (v == "a") {
        this.isActive = true;
      }
    },
    modalState(v) {
      this.dropzone = v;
    }
  },
  created() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("/FileHubs", {
        skipNegotiation: false
        // transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect([3000, 5000, 10000, null])
      .configureLogging(signalR.LogLevel.Information)
      .build();
  },
  mounted() {
    this.connection.start();
    // 화살표 함수
    this.connection.on("send", (res, message) => {
      console.log(res);
      console.log(message);
      if (res == 1) {
        this.logData.push(message);
        this.vtData.push(message);
      } else if (res == 2) {
        this.vtData.forEach(element => {
          if (message.fileName == element.fileName) {
            element.step = message.step;
          }
        });
      } else if (res == 3) {
        this.vtData.forEach(element => {
          if (message.fileName == element.fileName) {
            element.step = message.step;
          }
        });
      } else if (res == 4) {
        this.vtData.forEach(element => {
          if (message.fileName == element.fileName) {
            element.step = message.step;
          }
        });
      } else if (res == 5) {
        this.vtData.forEach(element => {
          if (message.fileName == element.fileName) {
            element.step = message.step;
          }
        });
        console.log("파일 업로드 성공");
      }
    });
    // window.addEventListener("beforeunload", this.unLoadEvent);
    setTimeout(() => {
      document.body.classList.add("default-transition");
    }, 100);
  },
  beforeUnmount() {
    this.connection.stop();
    // window.removeEventListener("beforeunload", this.unLoadEvent);
  },
  // beforeRouteLeave(to, from, next) {
  //   if (!this.fileuploading) {
  //     if (
  //       confirm(
  //         "이 사이트에서 나가시겠습니까?\n변경사항이 저장되지 않을 수 있습니다."
  //       )
  //     ) {
  //       next();
  //     }
  //   }
  // },
  methods: {
    onSelectionChanged() {
      console.log("selection changed");
    },
    exmodaloff() {
      this.exmodal = false;
      this.reset();
    },
    exmodalon() {
      this.exmodal = true;
    },
    ...mapActions("file", ["verifyMeta", "uploadRefresh"]),
    getRowData(props) {
      console.log(props);
    },
    getPro() {
      console.log(this.date + "_" + this.mediaSelected);
    },
    //#region 파일 업로드 모달 캘린더
    onInput(event) {
      const targetValue = event.target.value;

      const replaceAllTargetValue = targetValue.replace(/-/g, "");
      if (!isNaN(replaceAllTargetValue)) {
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          this.date = convertDate;
        }
      } else if (targetValue == "-") {
        const replaceAllTargetValue = targetValue.replace(/-/g, "");
        if (replaceAllTargetValue.length === 8) {
          const convertDate = this.convertDateSTH(replaceAllTargetValue);
          this.date = convertDate;
        }
      } else {
        this.resetDate();
        $fn.notify("error", { message: "숫자만 입력 가능 합니다." });
      }
    },
    convertDateSTH(value) {
      const replaceVal = value.replace(/-/g, "");
      const yyyy = replaceVal.substring(0, 4);
      const mm = replaceVal.substring(4, 6);
      const dd = replaceVal.substring(6, 8);
      if (12 < mm) {
        $fn.notify("error", { message: "날짜 형식 오류" });
        this.date = "";
      } else if (31 < dd) {
        $fn.notify("error", { message: "날짜 형식 오류" });
        this.date = "";
      } else {
        return `${yyyy}-${mm}-${dd}`;
      }
    },
    onContext(ctx) {
      // The date formatted in the locale, or the `label-no-date-selected` string
      this.formatted = ctx.selectedFormatted;
      // The following will be an empty string until a valid date is entered
      this.dateselected = ctx.selectedYMD;
    },
    resetDate() {
      var input = document.getElementById("dateinput");
      input.value = null;
    },
    //#endregion
    uploadError() {
      $fn.notify("error", { message: "파일 업로드 실패" });
    },
    uploadSuccess() {
      this.reset();
      this.uploadRefresh();
    },
    memoreset() {
      this.memo = "";
    },
    titlereset() {
      this.title = "";
    },
    reset() {
      this.memo = "";
      this.title = "";
      this.typeSelected = "f";
      this.resetDate();
      this.fileupload.removeFile(0);
      this.fileuploading = false;
      this.fileselect = false;
      this.localFiles = [];
      if (this.processing) {
        $fn.notify("error", { message: "파일 업로드 취소" });
      }
    },
    uploadAborted() {
      this.fileuploading = false;
      this.fileselect = false;
      $fn.notify("error", { message: "파일 업로드 취소" });
    },
    fileRemove() {
      this.fileupload.removeFile(0);
      this.localFiles = [];
    },
    async uploadfile() {
      if (this.metavalid && this.fileselect) {
        if (this.typeSelected == "a") {
          this.type = "private";
        }

        //NOTE: 커스텀 데이터 파라미터

        var data = {
          user_id: sessionStorage.getItem("user_id"),
          title: this.title,
          memo: this.memo,
          fileSize: this.localFiles[0].size,
          connectionId: this.connection.connectionId
        };

        this.uploaderCustomData = data;
        this.processing = true;
        this.verifyMeta({
          type: this.type,
          title: this.title,
          files: this.localFiles,
          categoryCD: this.categoryCD
        }).then(res => {
          this.processing = false;
          if (res) {
            // 파일 업로드
            this.fileupload.upload(0);
            // this.connection.on("send", (res, message) => {
            //   if (res) {
            //     console.log(message);
            //   }
            // });
            // this.reset();

            // if (this.isDragDropState) {
            //   this.SET_DRAG_DROP_STATE(false);
            // }
          }
        });
      } else if (!this.metavalid) {
        alert("메타데이터");
      } else if (!this.fileselect) {
        alert("파일 확인");
      }
    },
    modalon(event) {
      this.localFiles = [];
      this.localFiles.push(event.value[0]);
      if (event.value.length != 0) {
        if (
          event.value[0].type == "audio/mpeg" ||
          event.value[0].type == "image/jpeg"
        ) {
          this.modal = true;
          this.exmodalon();
          this.fileselect = true;
          this.fileuploading = true;
        } else {
          alert("업로드 할 수 없는 파일 형식입니다.");
          this.fileupload.removeFile(0);
          this.fileselect = false;
        }
      } else if (event.value.length == 0) {
        this.fileselect = false;
      }
    },
    onUploadProgress(e) {
      this.chunks.push({
        segmentSize: e.segmentSize,
        bytesLoaded: e.bytesLoaded,
        bytesTotal: e.bytesTotal
      });
    },
    makeToast(variant = null) {
      this.$bvToast.toast("FileName Background Task Start", {
        title: "File Upload Complete",
        variant: variant,
        solid: true,
        noAutoHide: false,
        autoHideDelay: 5000
      });
    },
    openModal() {
      this.modal = true;
    },
    closeModal() {
      this.modal = false;
    },
    confirm() {
      if (this.localFiles.length != null) {
        if (this.localFiles.length == 1) {
          if (confirm("현재 진행 중인 작업이 있습니다. 창을 닫으시겠습니까?")) {
            this.modal = false;
            this.localFiles = [];
            this.reset();
          } else {
            return;
          }
        } else {
          this.modal = false;
        }
      } else {
        this.modal = false;
      }
    },
    onDropZoneEnter(e) {
      if (e.dropZoneElement.id === "dropzone-external") {
        this.isDropZoneActive = true;
      }
    },
    onDropZoneLeave(e) {
      if (e.dropZoneElement.id === "dropzone-external") {
        this.isDropZoneActive = false;
        this.dropzone = false;
        this.$emit("dropZoneLeave");
      }
    }
  }
};
</script>
<style>
.form-control:focus {
  z-index: 0 !important;
}
.data-grid-div {
  position: absolute;
  top: 90px;
  width: 550px;
  margin-top: 10px;
  height: 250px;
  border: 1px solid #008ecc;
}
.date-div {
  position: relative;
  left: 0px;
  width: 550px;
  height: 380px;
}
/* .modal-file-processing-button {
  position: absolute;
  top: 465px;
  left: 330px;
}
.modal-file-upload-button {
  position: absolute;
  top: 465px;
  left: 360px;
}
.modal-file-reset-button {
  position: absolute;
  top: 465px;
  left: 40px;
} */
.file-modal-button {
  padding-top: 25px;
  margin-left: 400px;
}
.date-modal-button {
  padding-top: 25px;
  margin-left: 400px;
}
.file-modal {
  padding-left: 20px;
  height: 480px;
}
.date-modal {
  padding-left: 20px;
  height: 600px;
}
#__BVID__25___BV_modal_outer_ {
  z-index: 2000 !important;
}
.myModal-body {
  margin: 0px !important;
}

#dropzone-external.dropzone-active {
  position: absolute;
  z-index: 9989;
  top: 10px;
  left: 10px;
  width: 1920px;
  height: 934px;
  border: 3px solid #1f1f1f !important;
  background-color: rgba(0, 0, 0, 0.7);
}
.is-valid {
  border-color: #28a745 !important;
  height: 36px !important;
}
.is-invalid {
  border-color: #dc3545 !important;
  height: 36px !important;
}
.editTask {
  margin-top: 10px;
  border-radius: 3px;
  width: 400px;
  height: 36px;
  /* font-family: "Courier New"; */
  font-weight: 400;
  font-size: 18;
}

.title {
  padding: 20px 0 20px 0;
  font-size: 18px;
  font-weight: 500;
}

.editors {
  margin-right: 320px;
}

.editors .left,
.editors .right {
  display: inline-block;
  width: 49%;
  padding-right: 20px;
  box-sizing: border-box;
}

.editors .left {
  margin-right: 4px;
}

.editors .left > *,
.editors .right > *,
.editors .center > * {
  margin-bottom: 20px;
}

.editors .center {
  padding: 20px 27px 0 0;
}

.validate {
  float: right;
}

.options {
  padding: 20px;
  position: absolute;
  bottom: 0;
  right: 0;
  width: 260px;
  top: 0;
  background-color: rgba(191, 191, 191, 0.15);
}

.caption {
  font-size: 18px;
  font-weight: 500;
}

.option {
  margin-top: 20px;
}
.slide-fade-enter-active {
  transition: all 0.5s ease;
}
.slide-fade-leave-active {
  transition: all 0.5s cubic-bezier(1, 0.5, 0.8, 1);
}
.slide-fade-enter, .slide-fade-leave-to
/* .slide-fade-leave-active below version 2.1.8 */ {
  transform: translateY(-30px);
  opacity: 0;
}
.chunk-panel {
  width: 505px;
  height: 165px;
  overflow-y: auto;
  padding: 18px;
  margin-top: 40px;
  background-color: rgba(191, 191, 191, 0.15);
}
.segment-size,
.loaded-size {
  margin-left: 3px;
}
.note {
  display: block;
  font-size: 10pt;
  color: #484848;
  margin-left: 9px;
}
.note > span {
  font-weight: 700;
}
#dropzone-external {
  position: absolute;
  top: -100px;
  z-index: 800;

  width: 1920px;
  height: 900px;
  background-color: rgba(183, 183, 183, 0.1);
  border-width: 2px;
  border-style: dashed;
  padding: 10px;
}
#dropzone-external > * {
  pointer-events: none;
}
#dropzone-external.dropzone-active {
  border-style: solid;
}
.widget-container > span {
  font-size: 22px;
  font-weight: bold;
  margin-bottom: 16px;
}
#dropzone-image {
  max-width: 100%;
  max-height: 100%;
}
#dropzone-text > span {
  font-weight: 100;
  opacity: 0.5;
}
#upload-progress {
  display: flex;
  margin-top: 10px;
}
.flex-box {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}
.dx-fileuploader-files-container {
  height: 50px;
  width: 400px;
  overflow-y: auto;
}
.dx-fileuploader-wrapper {
  width: 400px;
  height: 130px;
  background-color: #ececec;
}
.dx-fileuploader-files-container {
  width: 400px !important;
  background-color: #ececec;
  padding: 0px 3px 0px !important;
}
.dx-fileuploader-file-name {
  color: black;
}
.dx-fileuploader-file-size {
  color: black;
}
/* .dx-progressbar {
} */
.dx-fileuploader-file-status-message {
  color: black;
}
/* 버튼 없는 버전 */
/* .dx-fileuploader-file{
  position:absolute;
  right:45px;
  top:110px;
  width:380px;
  background-color:#ECECEC;
} */
/* 버튼 있는 버전 */
.dx-fileuploader-file {
  position: absolute;
  padding-left: 10px;
  left: 40px;
  top: 120px;
  width: 352px;
  background-color: #ececec;
}
.dx-fileuploader-files-container::-webkit-scrollbar {
  width: 10px;
}
/* Track */
.dx-fileuploader-files-container::-webkit-scrollbar-track {
  background: #faafaf;
  border-radius: 5px;
}
/* Handle */
.dx-fileuploader-files-container::-webkit-scrollbar-thumb {
  background: #0f0f0f;
  border-radius: 5px;
}
/* Handle on hover */
.dx-fileuploader-files-container::-webkit-scrollbar-thumb:hover {
  background: #999;
}
.dx-fileuploader-upload-button.dx-button-has-text {
  display: none !important;
}
/* .dx-fileuploader-button-container{
  display: none !important;
} */
.dx-fileuploader-upload-button.dx-button-has-text {
  display: none !important;
}
.dx-fileuploader-input-label {
  display: none !important;
}
.nav-pills .nav-link {
  border-radius: 0px !important;
  border-top: 1px solid white;
  border-bottom: 1px solid white;
}
.nav-pills .nav-link.active {
  color: #008ecc;
  background-color: white !important;
  border-right: 5px solid #008ecc !important;
}
.nav-item {
  border: 1px solid rgb(211, 211, 211);
}
.dropdown-toggle {
  border-radius: 0.1rem !important;
}
.btn {
  border-radius: 0.1rem !important;
}
</style>
<style scoped>
/* ::-webkit-scrollbar {
  display: none;
} */
.progress__wrapper {
  margin: 0px !important;
}
.vuetable .thead .tr {
  height: 33px !important;
}
.vuetable-head-wrapper {
  height: 33px !important;
}
.card {
  height: 670px !important;
}
.date-input:focus {
  border: 1px solid #4475c4 !important;
}
.date-input {
  border: 1px solid #008ecc !important;
}
.media-select {
  border: 1px solid #008ecc !important;
}
.media-select:focus {
  border: 1px solid #4475c4 !important;
}
</style>
