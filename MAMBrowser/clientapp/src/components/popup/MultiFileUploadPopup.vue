<template>
    <div>
        <b-modal 
            id="upload-modal-closing"
            title="파일 업로드" 
            size="lg"
            modal-class="my-modal-file"
            no-close-on-backdrop
            v-model="showDialog"
        >
            <!-- HEADER-CLOSE -->
            <template slot="modal-header-close">
                <div>
                    <button type="button" class="close float-left" @click.stop="onHide">_</button>
                    <button type="button" class="close" @click="showDialog=false">×</button>
                </div>
            </template>
            <!-- BODY -->
            <b-container class="my-container">
                <!-- 업로드: 버튼 -->
                <div class="drop-active" v-show="files.length === 0">
                    <h3>파일을 올려주세요.</h3>
                    <label :for="'file'" class="btn btn-primary default">
                        파일 선택
                    </label>
                    <file-upload 
                        ref="refFileSelect" 
                        v-model="files" 
                        :multiple="true"
                        @input-file="inputFile">
                    </file-upload>
                </div>
                <!-- 업로드: 테이블 -->
                <div v-show="files.length > 0">
                    <vuetable
                        table-height="340px"
                        ref="vuetable-scrollable"
                        :api-mode="false"
                        :fields="fields"
                        :data="files"
                        no-data-template=""
                    >
                        <template slot="name" scope="props">
                            <!-- 파일명 -->
                            <div class="mb-2">{{ props.rowData.name }}</div>
                            <!-- 프로그래스바 -->
                            <div class="file-progress">
                                <div 
                                    :class="{'progress-bar': true, 
                                    'progress-bar-striped': true, 
                                    'bg-danger': props.rowData.file.error, 
                                    'progress-bar-animated': props.rowData.file.active}" 
                                    role="progressbar" 
                                    :style="{width: props.rowData.file.progress + '%'}">
                                    {{props.rowData.file.progress}}%
                                </div>
                            </div>
                        </template>
                        <template slot="actions" scope="props">
                            <b-button variant="success default" size="sm">전송</b-button>
                            <b-button variant="danger defMultiFileUploadPopupault" size="sm" @click="onRemoveFile(props.rowData)">삭제</b-button>
                        </template>
                    </vuetable>
                </div>
            </b-container>
            <!--  FOOTER: 액션 -->
            <template slot="modal-footer">
                <b-button variant="success default" @click="onSubmit">전송</b-button>
                <b-button variant="danger default" @click="showDialog=false">닫기</b-button>
            </template>
        </b-modal>
    </div>
</template>

<script>
import Vuetable from "vuetable-2/src/components/Vuetable";

export default {
    components: { Vuetable },
    props: ['show'],
    computed: {
        showDialog: {
            get() {
                return this.show;
            },
            set(v) {
                if (!v) { 
                    this.files = [];
                    this.$emit('close');
                }
            }
        },
    },
    data() {
        return {
            files: [],
            dropActive: true,
            fields: [
                {
                    name: '__sequence',
                    title: 'No',
                    titleClass: 'center aligned',
                    dataClass: 'center aligned text-center',
                    width: '10%',
                },
                {
                    name: '__slot:name',
                    title: '파일명',
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                },
                {
                    name: 'size',
                    title: '사이즈',
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: '15%',
                    callback: (value) => {
                        return this.$fn.formatBytes(value);
                    }
                },
                {
                    name: '__slot:actions',
                    title: '상태',
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: '30%',
                }
            ]
        }
    },
    methods: {
       onHide() {
           console.info('hide');
       },
       onSubmit() {
           console.info('submit');
       },
       inputFile(newFile, oldFile) {
           this.dropActive = false;
           console.info('inputFile', newFile, oldFile);
       },
       onFileSelect() {
           console.info('this.$refs.refFileSelect', this.$refs.refFileSelect);
           this.$refs.refFileSelect.click();
       },
       onFileUpload() {
           console.info('files', this.files);
       },
       onItemAction(data, index) {
           console.info('onItemAction', data, index);
       },
       onProgress() {

       },
       onRemoveFile(rowData) {
           const findIndex = this.files.findIndex(file => file.id === rowData.id);
           console.info("this.files-remove", findIndex, this.files, rowData);
           this.files.splice(findIndex, 1);
       }
    }
}
</script>
<style scoped>
.file-progress {
    display: -ms-flexbox;
    display: flex;
    overflow: hidden;
    font-size: .75rem;
    line-height: 1rem;
    text-align: center;
    background-color: #e9ecef;
    border-radius: .25rem;
}
.file-progress .progress-bar-striped {
    background-image: linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent);
    background-size: 1rem 1rem;
}
.file-progress .progress-bar {
    height: 1rem;
    line-height: 1rem;
    color: #fff;
    background-color: #007bff;
    transition: width .6s ease;
}
.file-progress .bg-danger {
    background-color: #dc3545!important;
}
</style>
