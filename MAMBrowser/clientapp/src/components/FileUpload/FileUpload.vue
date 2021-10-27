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
      @click="openFileModal"
      style="position:absolute; top:-80px; right:580px; z-index:1030; border-color:#008ECC; color:#008ECC; background-color:white;"
      >파일 업로드
    </b-button>

    <transition name="slide-fade">
      <CommonFileModal
        v-show="FileModal"
        @close="closeFileModal"
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
      </CommonFileModal>
    </transition>

    <MetaModal></MetaModal>
  </div>
</template>

<script>
import CommonFileModal from "../Modal/CommonFileModal.vue";
import CommonFile from "./CommonFile";
import MetaModal from "./MetaModal";
import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
export default {
  props: {
    DragFileModalState: {
      type: Boolean,
      default: false
    }
  },
  components: {
    CommonFileModal,
    MetaModal
  },
  mixins: [CommonFile],
  data() {
    return {
      FileModal: false,
      dropzone: false
    };
  },
  watch: {
    DragFileModalState(v) {
      this.dropzone = v;
    }
  },
  methods: {
    openFileModal() {
      this.FileModal = true;
    },
    closeFileModal() {
      this.FileModal = false;
    }
  }
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
