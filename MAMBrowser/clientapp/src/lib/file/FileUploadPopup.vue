<template>
    <div>
        <b-modal 
            id="upload-modal-closing"
            title="파일 업로드"
            size="lg"
            modal-class="my-modal-file"
            no-close-on-backdrop
            v-model="isShow"
            @close.prevent
            hide-footer
        >
            <!-- HEADER-CLOSE -->
            <template slot="modal-header-close">
                <div>
                    <b-button variant="outline-primary" size="sm" @click.stop="hide(false)">
                        <i class="iconsminds-minimize"></i>최소화
                    </b-button>
                    <b-button variant="outline-danger" class="icon-button" @click.stop="onClose">
                        <i class="simple-icon-close"></i>
                    </b-button>
                </div>
            </template>
            <!-- BODY: 파일업로드 -->
            <file-upload></file-upload>
        </b-modal>
        <!-- 휴지통 이동 확인창 -->
        <common-confirm
          id="closeFileUploadModal"
          title="파일 업로드창 닫기"
          message= " 파일 업로드 창을 닫으시겠습니까?"
          submitBtn="확인"
          @ok="onCloseFileUploadModal()"
        />
    </div>
</template>

<script>
import FileUpload from './FileUpload';
import { mapGetters, mapActions, mapMutations } from 'vuex';
export default {
    components: { FileUpload },
    data() {
        return {
            isShow: false,
        }
    },
    computed: {
        ...mapGetters('file', ['getFileData']),
    },
    methods: {
        ...mapActions('file', ['open_toast']),
        ...mapMutations('file', ['REMOVE_FILES_ALL']),
        show() {
            this.isShow = true;
        },
        hide(isOpenToast) {
            this.isShow = false;
            if (!isOpenToast) {
                this.open_toast();
            }
        }, 
        onClose() {
            if (this.getFileData.length > 0) {
                this.$bvModal.show('closeFileUploadModal');
                return;
            }
            this.isShow = false;
        },
        onCloseFileUploadModal() {
            // TODO: 파일 삭제 로직
            this.REMOVE_FILES_ALL();
            this.$bvModal.hide('closeFileUploadModal');
            this.isShow = false;
        },
    }
}
</script>
