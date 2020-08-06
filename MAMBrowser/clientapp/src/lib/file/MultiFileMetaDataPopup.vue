<template>
    <b-modal
        id="upload-modal-closing"
        title="멀티 파일 업로드 메타데이터 입력"
        size="md"
        modal-class="my-modal-file"
        no-close-on-backdrop
        v-model="isShow">
        <b-form-group label="제목" label-for="input-title">
            <b-form-input
                id="input-title"
                v-model="$v.title.$model">
            </b-form-input>
            <b-form-invalid-feedback :state="!$v.title.required">필수 입력입니다.</b-form-invalid-feedback>
        </b-form-group>
        <b-form-group label="내용" label-for="input-memo">
            <b-form-textarea
                id="input-memo"
                v-model="$v.memo.$model"
                size="sm">
            </b-form-textarea>
            <b-form-invalid-feedback :state="!$v.memo.required">필수 입력입니다.</b-form-invalid-feedback>
        </b-form-group>
         <!--  FOOTER: 액션 -->
        <template slot="modal-footer">
            <div class="flex-grow-1">
            </div>
            <div>
                <b-button variant="outline-success default" @click="submit">업로드</b-button>
                <b-button variant="outline-danger default" @click="close">취소</b-button>
            </div>
        </template>
    </b-modal>
</template>

<script>
import mixinValidate from '../../mixin/MixinValidate';
import { mapMutations, mapActions } from 'vuex';

export default {
    mixins: [ mixinValidate ],
    data() {
        return {
            localFiles: [],
            isShow: false,
            title: '',
            memo: '',
        }
    },
    methods: {
        ...mapMutations('file', ['REMOVE_FILES']),
        ...mapActions('file', ['open_toast', 'add_files']),
        submit() {
            if (!this.$v.title.$invalid || !this.$v.memo.$invalid) {
                this.$fn.notify('inputError', {});
                return;
            }

            const data = {
                files: this.localFiles,
                meta: { title: this.title, memo: this.memo }
            }

            this.open_toast(data);
            this.reset();
        },
        reset() {
            this.isShow = false;
            this.title = '';
            this.memo = '';
            this.localFiles = [];
        },
        show(files) {
            this.localFiles = files;
            this.isShow = true;
        },
        close() {
            this.reset()
        },
    }
}
</script>