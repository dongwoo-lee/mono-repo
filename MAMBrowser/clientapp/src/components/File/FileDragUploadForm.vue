<template>
<div>
    <div class="drop-active" v-show="$refs.refFileUpload && $refs.refFileUpload.dropActive">
        <h4>음원 파일 업로드</h4>
        <file-upload 
            ref="refFileUpload" 
            v-model="localFiles"
            :multiple="false"
            :drop="true"
            :drop-directory="true"
            :accept="FILE_UPLOAD_ACCEPT"
            @input-filter="inputFilter"
            @input-file="inputFile">
        </file-upload>
    </div>
    <common-confirm
        id="modalOverSize"
        title="파일 용량 초과"
        message="파일은 최대 2GB까지 업로드할 수 있습니다."
    />
    <common-confirm
        id="modalFileNotExtension"
        title="유효하지 않은 파일입니다."
        :message="`이 파일은 유효한 파일이 아닙니다. <br>확장자가 ${FILE_UPLOAD_EXTENSIONS}인 파일을 업로드해주세요.`"
    />
    <common-confirm
        id="modalNotDiskAvailable"
        title="디스크 공간이 부족합니다."
        :message="`디스크 용량이 부족합니다. <br>업로드 가능 사이즈는 ${$fn.formatBytes(diskAvailable)}입니다.`"
    />
</div>
</template>

<script>
import { mapMutations, mapActions, mapGetters } from 'vuex';
import {FILE_UPLOAD_ACCEPT, FILE_UPLOAD_EXTENSIONS, MAXIMUM_FILE_SIZE, ROUTE_NAMES} from '@/constants/config';

export default {
    data() {
        return {
            FILE_UPLOAD_ACCEPT: FILE_UPLOAD_ACCEPT,
            FILE_UPLOAD_EXTENSIONS: FILE_UPLOAD_EXTENSIONS,
            localFiles: [],
            maxSize: MAXIMUM_FILE_SIZE,
        }
    },
    computed: {
        ...mapGetters('user', ['diskAvailable']),
    },
    watch: {
        localFiles(files, oldFiles) {
            // size 체크
            const overSize = files.some(file => file.size >= this.maxSize);
            if (overSize) {
                this.$bvModal.show('modalOverSize');
                return;
            }

            // 확장자 체크
            if (this.invalidExtension(files))  {
                this.$bvModal.show('modalFileNotExtension');
                return;
            }

            // 업로드 가능 용량 체크
            if (this.notDiskAvailable(files)) {
                this.$bvModal.show('modalNotDiskAvailable');
                return;
            }

            if (files.length > 0) {
                let uniqIds = [];
                oldFiles.forEach(data => {
                    uniqIds.push(data.id);
                })

                const filterFileData = files.filter(data => {
                    return !uniqIds.includes(data.id);
                })

                const newFileData = [];
                filterFileData.forEach(data => {
                    newFileData.push(data);
                })

                if (filterFileData.length === 0) return;
                // 메타 데이터 입력창 오픈
                this.open_meta_data_popup(newFileData);
                this.SET_DRAG_DROP_STATE(true);
            }
        },
        '$route': {
            handler(to, from) {
                // drag 기능 초기화
                const dropActive = this.$refs.refFileUpload.dropActive;
                if (dropActive) {
                    this.$refs.refFileUpload.dropActive = false;
                }
            },
        }
    },
    methods: {
        ...mapActions('file', ['open_meta_data_popup', 'close_meta_data_popup']),
        ...mapMutations('file', ['SET_DRAG_DROP_STATE']),
        inputFilter(file) {},
        inputFile(data) {},
        invalidExtension(files) {
            if (files.length > 0) {
                return files.some(file => {
                    const extension = file.name.substr(file.name.lastIndexOf('.') + 1).toLowerCase();
                    return FILE_UPLOAD_EXTENSIONS.indexOf(extension) === -1;
                })
            }
            return true;
        },
        notDiskAvailable(files) {
            if (files.length > 0) {
                const result = this.diskAvailable - files[0].size;
                return result < 0;
            }
            return true;
        },
        closePopup() {
            this.SET_DRAG_DROP_STATE(false);
            // 메타 데이터 입력창 닫기
            this.close_meta_data_popup();
            this.localFiles = [];
        },
    }
}
</script>
