<template>
    <b-modal
        id="upload-modal-closing"
        title="파일 업로드 메타데이터 입력"
        size="md"
        modal-class="my-modal-file"
        no-close-on-backdrop
        v-model="isShow">
        <b-form-group label="저장공간" label-for="input-title">
             <b-form-select v-model="$v.type.$model">
                <b-form-select-option value="private">My공간</b-form-select-option>
                <b-form-select-option value="public">공유소재</b-form-select-option>
            </b-form-select>
            <b-form-invalid-feedback :state="!$v.type.required">필수 입력입니다.</b-form-invalid-feedback>
        </b-form-group>
        <b-form-group label="제목" label-for="input-title">
            <b-form-input
                id="input-title"
                v-model="$v.title.$model">
            </b-form-input>
            <b-form-invalid-feedback :state="!$v.title.required">필수 입력입니다.</b-form-invalid-feedback>
        </b-form-group>
        <template v-if="isSharedMaterial">
            <b-form-group label="매체" label-for="input-title">
                    <b-form-select 
                    v-model="$v.mediaCD.$model"
                    :options="primaryOptions"
                    @change="onChangePrimary"
                    value-field="id"
                    text-field="name" 
                />
                <b-form-invalid-feedback :state="!$v.mediaCD.required">필수 입력입니다.</b-form-invalid-feedback>
            </b-form-group>
            <b-form-group label="분류" label-for="input-title">
                <b-form-select 
                    v-model="$v.categoryCD.$model"
                    :options="primaryCodeOptions"
                    value-field="id"
                    text-field="name" 
                >
                    <template v-slot:first>
                        <b-form-select-option v-if="primaryCodeOptions.length > 0" value="">선택해주세요.</b-form-select-option>
                        <b-form-select-option v-else value="">값이 존재하지 않습니다.</b-form-select-option>
                    </template>
                </b-form-select>
            <b-form-invalid-feedback :state="!$v.categoryCD.required">필수 입력입니다.</b-form-invalid-feedback>
            </b-form-group>
        </template>
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
            type: '',
            primaryOptions: [],                        // 공유매체 목록
            mediaCD: 'A',                               // 매체
            primaryCodeOptions: [],                       // 공유소재 분류 목록
            categoryCD: '',                                // 분류
        }
    },
    computed: {
        routeName() {
            return this.$route.name === undefined ? 'private' : this.$route.name;
        },
        isSharedMaterial() {
            if (this.type === 'public') {
                this.getPrimaryOptions();
                this.getPrimaryCodeOptions();
                return true;
            };
            return false;
        }
    },
    methods: {
        ...mapMutations('file', ['REMOVE_FILES']),
        ...mapActions('file', ['open_toast', 'upload']),
        submit() {
            console.info('this.$v', this.$v);
            if (!this.$v.title.$invalid || !this.$v.memo.$invalid) {
                this.$fn.notify('inputError', {});
                return;
            }
            let data = {};
            if (this.type === 'private') {
                data = {
                    files: this.localFiles,
                    meta: { 
                        title: this.title,
                        memo: this.memo,
                    }
                }
            } else {
                if (!this.$v.categoryCD.$invalid) {
                    this.$fn.notify('inputError', {});
                    return ;
                }

                data = {
                    files: this.localFiles,
                    meta: { 
                        title: this.title,
                        memo: this.memo,
                        mediaCD: this.mediaCD,
                        categoryCD: this.categoryCD,
                    }
                }
            }

            this.open_toast(data);
            this.upload(this.type);
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
            this.type = this.routeName;
        },
        close() {
            this.reset()
        },
        onChangePrimary() {
            this.getPrimaryCodeOptions();
        },
        // 매체목록 조회
        getPrimaryOptions() {
            this.$http.get('/api/Categories/public-codes/primary')
                .then(res => {
                    if (res.status === 200) {
                        this.primaryOptions = res.data.resultObject.data;
                    } else {
                        this.$fn.notify('server-error', { message: '조회 에러' });
                    }
            });
        },
        // 공유 소재 분류 목록 조회
        getPrimaryCodeOptions() {
            this.categoryCD = '';
            this.$http.get('/api/Categories/public-codes/primary/' + this.mediaCD)
              .then(res => {
                  if (res.status === 200) {
                      this.primaryCodeOptions = res.data.resultObject.data;
                  } else {
                      this.$fn.notify('server-error', { message: '조회 에러' });
                  }
            });
        },
    }
}
</script>