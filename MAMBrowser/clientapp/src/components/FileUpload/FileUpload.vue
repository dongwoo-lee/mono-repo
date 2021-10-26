<template>
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
        v-show="fileModal"
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
            @valueChanged="valueChanged"
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
                      :data="vtData"
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
        v-show="metaModal"
        @close="metaModalOff"
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
                        @click="datereset"
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
                    <!-- //TODO: Data Binding -->
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
import CommonFileUpload from "./CommonFileUpload";
export default {
  mixins: [CommonFileUpload]
};
</script>
<style>
@import "./FileUploadCSS.css";
</style>
<style scoped>
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
  height: 675px !important;
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
