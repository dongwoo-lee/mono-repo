<template>
    <b-container class="my-container">
        <!-- 파일이 없을 경우, 버튼 및 드롭 -->
        <div class="drop-active" v-show="files.length === 0">
            <h4>드래그 또는 클릭으로 파일을 업로드하세요.</h4>
            <label for="file" class="btn btn-primary default w-50">
                파일 선택
            </label>
            <file-upload 
                ref="refFileSelect" 
                :multiple="true"
                :drop="true"
                :drop-directory="true"
                v-model="files"
                @input-filter="inputFilter"
                @input-file="inputFile">
            </file-upload>
        </div>
        <!-- 파일이 존재할 경우, 테이블 -->
        <div v-show="files.length > 0">
            <div class="c-upload-top d-flex flex-row">
                <div class="flex-grow-1">
                    총 {{files.length}}개 파일 중 0개 업로드 완료
                </div>
                <label for="file" class="btn btn-outline-primary btn-sm default">
                    <i class="iconsminds-add-file"></i>추가
                </label>
            </div>
            <vuetable
                table-height="340px"
                ref="vuetable-scrollable"
                :api-mode="false"
                :fields="fields"
                :data="files"
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
                            'bg-danger': props.rowData.file.error, 
                            'progress-bar-animated': props.rowData.file.active}" 
                            role="progressbar" 
                            :style="{width: props.rowData.file.progress + '%'}">
                            {{props.rowData.file.progress}}%
                        </div>
                    </div>
                </template>
                <!-- 상태 -->
                <template slot="state" scope="props">
                    <div>{{ getState(props.rowData) }}</div>
                </template>
                <!-- 액션 -->
                <template slot="actions" scope="props">
                    <b-button variant="outline-danger default" size="sm" @click="onRemoveFile(props.rowData)">
                        <i class="iconsminds-remove-file"></i>삭제
                    </b-button>
                </template>
            </vuetable>
        </div>
    </b-container>
</template>

<script>
import Vuetable from "vuetable-2/src/components/Vuetable";

export default {
    components: { Vuetable },
    data() {
        return {
            files: [],
            dropActive: true,
            fields: [
                {
                    name: '__sequence',
                    title: 'No',
                    titleClass: 'center aligned text-center',
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
                    name: '__slot:state',
                    title: '상태',
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: '15%',
                },
                {
                    name: '__slot:actions',
                    title: '',
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: '15%',
                }
            ]
        }
    },
    beforeDestroy() {
        this.files = [];
    },
    methods: {
        inputFilter(newFile, oldFile) { 
            console.info('inputFilter', newFile, oldFile);
        },
        inputFile(newFile) {
            console.info('inputFile', newFile);
           this.dropActive = false;
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
            this.files.splice(findIndex, 1);
        },
        getState(rowData) {
            if (!rowData.success) return '대기중';
            if (rowData.success) return '전송완료';
        },
    }
}
</script>
