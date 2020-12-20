<template>
    <b-container class="my-container">
        <!-- 파일이 없을 경우, 버튼 및 드롭 -->
        <div class="drop-active" v-show="localFiles.length === 0">
            <h4>드래그 또는 클릭으로 파일을 업로드하세요.</h4>
            <label for="file" class="btn btn-outline-primary default cutom-label w-50">
                파일 선택
            </label>
        </div>
        <!-- 파일이 존재할 경우, 테이블 -->
        <div v-show="localFiles.length > 0">
            <div class="c-upload-top d-flex flex-row">
                <div class="flex-grow-1">
                    총 {{localFiles.length}}개 파일 중 {{ getSuccessUploadFileLength() }}개 업로드 완료
                </div>
                <!-- <label v-show="uploadAddState()" for="file" class="btn btn-outline-primary btn-sm default cutom-label mr-2">
                    <i class="iconsminds-add-file"></i>추가
                </label> -->
                <label for="file" class="btn btn-outline-primary btn-sm default cutom-label mr-2">
                    <i class="iconsminds-add-file"></i>추가
                </label>
                <!-- <label v-show="uploadStopState()" class="btn btn-outline-warning btn-sm default cutom-label"
                    :class="{ 'mr-2': uploadStopState() }"
                    @click="onStopUpload">
                    <i class="iconsminds-add-file"></i>업로드 정지
                </label>
                <label v-show="uploadStartState()" class="btn btn-outline-primary btn-sm default cutom-label"
                    @click="onStartUpload">
                    <i class="iconsminds-add-file"></i>업로드 시작
                </label> -->
            </div>
            <vuetable
                table-height="340px"
                ref="vuetable-scrollable"
                :api-mode="false"
                :fields="fields"
                :data="localFiles"
                no-data-template=""
            >
                <!-- 파일명 -->
                <template slot="name" scope="props">
                    <div class="mb-2">{{ props.rowData.name }}</div>
                    <!-- 프로그래스바 -->
                    <div class="file-progress">
                        <div 
                            :class="{'progress-bar': true, 
                            'progress-bar-striped': true, 
                            'bg-danger': props.rowData.error, 
                            'progress-bar-animated': props.rowData.active}" 
                            role="progressbar" 
                            :style="{width: props.rowData.progress + '%'}">
                            {{props.rowData.progress}}%
                        </div>
                    </div>
                    <div v-show="props.rowData.uploadState === 'save'">※용량에 따라 저장시간이 오래 걸릴수 있습니다.</div>
                </template>
                <!-- 상태 -->
                <template slot="state" scope="props">
                    <div>{{ getState(props.rowData.uploadState) }}</div>
                </template>
                <!-- 비고 -->
                <template slot="remark" scope="props">
                    <div>{{ getRemark(props.rowData.metaData) }}</div>
                </template>
                <!-- 액션 -->
                <template slot="actions" scope="props">
                    <template v-if="isSave(props.rowData.uploadState)">
                        <label>저장중</label>
                    </template>
                    <template v-else>
                        <b-button variant="outline-danger default" size="sm" 
                            @click="confirmDelete(props.rowData)">
                        {{getDeleteState(props.rowData.uploadState)}}
                    </b-button>
                    </template>
                </template>
            </vuetable>
        </div>

        <!-- 진행 중 삭제 -->
        <common-confirm
            id="modalStartingDelete"
            title="파일 업로드 취소"
            message="파일 업로드가 진행중입니다. 업로드를 취소하시겠습니까?"
            submitBtn="업로드 취소"
            :customClose="true"
            @ok="onRemoveFileAndCancelToken()"
            @close="onCloseRemoveFile()"
        />
    </b-container>
</template>

<script>
import Vuetable from "vuetable-2/src/components/Vuetable";
import { mapGetters, mapActions } from 'vuex';

export default {
    components: { Vuetable },
    data() {
        return {
            dropActive: true,
            confirmDeleteData: '',
            fields: [
                {
                    name: '__sequence',
                    title: '순서',
                    titleClass: 'center aligned text-center',
                    dataClass: 'center aligned text-center',
                    width: '7%',
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
                    name: '__slot:state',
                    title: '상태',
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: '10%',
                },
                {
                    name: '__slot:remark',
                    title: '비고',
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: '20%',
                },
                {
                    name: '__slot:actions',
                    title: '추가작업',
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: '12%',
                }
            ]
        }
    },
    computed: {
        ...mapGetters('file', ['getFileData', 'getUploadTransmitState', 'getBeingUploaded']),
        localFiles() {
            const tmpFiles = [];
            this.getFileData.forEach(data => {
                this.$set(data.file, 'uploadState', data.uploadState);
                this.$set(data.file, 'metaData', data.metaData);
                tmpFiles.push(data.file);
            })

            console.debug('localFiles', tmpFiles);
            return tmpFiles;
        },
    },
    methods: {
        ...mapActions('file', ['open_toast', 'remove_file', 'removeFileAndCancelToken', 'upload']),
        addFiles() {
            this.open_toast();
        },
        onStartUpload() {
            this.upload();
        },
        onRemoveFile(rowData) {
            this.remove_file(rowData.id);
            this.$bvModal.hide('modalStartingDelete');
        },
        onRemoveFileAndCancelToken() {
            console.debug('confirmDeleteData', this.confirmDeleteData);
            this.removeFileAndCancelToken(
                {
                    id: this.confirmDeleteData.id,
                    fileId: this.confirmDeleteData.id
                }
            );
            this.confirmDeleteData = '';
            this.$bvModal.hide('modalStartingDelete');
        },
        getState(state) {
            if (state === 'wait') return '대기중';
            if (state === 'stop') return '정지';
            if (state === 'start') return '전송중';
            if (state === 'success') return '전송완료';
            if (state === 'save') return '저장중';
            return '';
        },
        getDeleteState(state) {
            if (state === 'start' || state === 'stop') return '취소';
            if (state === 'success') return '목록제거';
            if (state === 'save') return '저장중';
            if (state === 'wait') return '삭제';
        },
        getRemark(metaData) {
            const metaDataToJson = JSON.parse(metaData);
            const {title, mediaCD } = metaDataToJson;
            return `${title}`;
        },
        getSuccessUploadFileLength() {
            const filterSuccess = this.localFiles.filter(file => file.success)
            return filterSuccess.length;
        },
        uploadAddState() {
            return !this.getUploadTransmitState || !this.getBeingUploaded;
        },
        uploadStartState() {
            return !this.getUploadTransmitState && this.localFiles.length > this.getSuccessUploadFileLength();
        },
        uploadStopState() {
            return this.getUploadTransmitState && this.getBeingUploaded;
        },
        isSave(state) {
            return state === 'save';
        },
        confirmDelete(rowData) {
            const confirmTarget = ['start', 'stop']
            if (confirmTarget.includes(rowData.uploadState)) {
                this.confirmDeleteData = rowData;
                this.$bvModal.show('modalStartingDelete');
            } else {
                this.onRemoveFile(rowData);
            }
        },
        onCloseRemoveFile() {
            this.confirmDeleteData = '';
            this.$bvModal.hide('modalStartingDelete');
        },
    }
}
</script>
