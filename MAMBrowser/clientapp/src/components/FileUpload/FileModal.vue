<template>
  <div>
    <transition name="slide-fade">
      <CommonFileModal
        v-show="this.fileModal"
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
  </div>
</template>
<script>
import CommonFileModal from "../Modal/CommonFileModal.vue";
import CommonFile from "./CommonFile";
import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
export default {
  components: {
    CommonFileModal
  },
  mixins: [CommonFile],
  computed: {
    ...mapState("FileStore", {
      fileModal: state => state.fileModal
    })
  }
};
</script>

<style></style>
