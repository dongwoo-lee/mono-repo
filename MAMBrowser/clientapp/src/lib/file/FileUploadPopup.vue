<template>
    <div>
        <b-modal 
            id="upload-modal-closing"
            title="파일 업로드 NEW"
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
        <b-modal 
            id="closeFileUploadModal" 
            size="sm" 
            title="파일 업로드창 닫기"
            :hideHeaderClose="true"
            @ok="onCloseFileUploadModal">
            파일 업로드 창을 닫으시겠습니까?
        </b-modal>
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
            this.$bvModal.show('closeFileUploadModal');
        },
        onCloseFileUploadModal() {
            // TODO: 파일 삭제 로직
            this.REMOVE_FILES_ALL();
            this.$bvModal.hide('modal-prevent-closing');
            this.isShow = false;
        },
    }
}
</script>
