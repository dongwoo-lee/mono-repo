<template>
    <div>
        <b-modal 
            id="upload-modal-closing"
            title="음원 파일 업로드"
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
                    <b-icon icon="dash" class="icon" variant="info" v-show="getBeingUploaded" @click.stop="hide(false)"></b-icon>
                    <b-icon icon="x" class="icon" variant="danger" @click.stop="onClose"></b-icon>
                </div>
            </template>
            <!-- BODY: 파일업로드 -->
            <file-upload></file-upload>
        </b-modal>
        <!-- 휴지통 이동 확인창 -->
        <common-confirm
          id="closeFileUploadModal"
          title="파일 업로드창 닫기"
          message= "업로드가 중단됩니다. 파일 업로드 창을 닫으시겠습니까?"
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
        ...mapGetters('file', ['getBeingUploaded']),
    },
    methods: {
        ...mapActions('file', ['open_toast', 'remove_file_all']),
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
            // 업로드 진행 중
            if (this.getBeingUploaded) {
                this.$bvModal.show('closeFileUploadModal');
                return;
            } 

            this.remove_file_all();
            this.isShow = false;
        },
        onCloseFileUploadModal() {
            this.remove_file_all();
            this.$bvModal.hide('closeFileUploadModal');
            this.isShow = false;
        },
    }
}
</script>
