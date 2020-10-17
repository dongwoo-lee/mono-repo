<template>
<div>
    <div class="drop-active" v-show="$refs.refFileUpload && $refs.refFileUpload.dropActive">
        <h4>드래그 파일 업로드</h4>
        <file-upload 
            ref="refFileUpload" 
            v-model="localFiles"
            :multiple="false"
            :drop="true"
            :drop-directory="true"
            @input-filter="inputFilter"
            @input-file="inputFile">
        </file-upload>
    </div>
</div>
</template>

<script>
import { mapMutations, mapActions } from 'vuex';

export default {
    data() {
        return {
            localFiles: [],
        }
    },
    watch: {
        localFiles(files, oldFiles) {
            
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
        }
    },
    methods: {
        ...mapActions('file', ['open_meta_data_popup', 'close_meta_data_popup']),
        ...mapMutations('file', ['SET_DRAG_DROP_STATE']),
        inputFilter(data) { 
        },
        inputFile(data) {
        },
        closePopup() {
            this.SET_DRAG_DROP_STATE(false);
            // 메타 데이터 입력창 닫기
            this.close_meta_data_popup();
            this.localFiles = [];
        }
    }
}
</script>
