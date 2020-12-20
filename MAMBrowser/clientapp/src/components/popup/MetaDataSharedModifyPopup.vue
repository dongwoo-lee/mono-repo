<template>
<div>
    <b-modal
        id="upload-modal-closing"
        title="메타데이터 수정"
        size="md"
        modal-class="my-modal-file"
        no-close-on-backdrop
        v-model="showDialog">
        <!-- 제목 -->
        <common-input-text-max-length 
            label="제목" 
            labelfor="input-title" 
            v-model="$v.metaData.title.$model" 
            :state="!$v.metaData.title.required">
        </common-input-text-max-length>
        <b-form-group label="매체" label-for="input-title">
                <b-form-select 
                v-model="$v.metaData.mediaCD.$model"
                :options="primaryOptions"
                @change="onChangePrimary"
                value-field="id"
                text-field="name" 
            />
            <b-form-invalid-feedback :state="!$v.metaData.mediaCD.required">필수 입력입니다.</b-form-invalid-feedback>
        </b-form-group>
        <b-form-group label="분류" label-for="input-title">
            <b-form-select 
                v-model="$v.metaData.categoryCD.$model"
                :options="primaryCodeOptions"
                value-field="id"
                text-field="name" 
            />
            <b-form-invalid-feedback :state="!$v.metaData.categoryCD.required">필수 입력입니다.</b-form-invalid-feedback>
        </b-form-group>
        <!-- 내용 -->
        <common-text-area-max-length
            label="내용"
            label-for="input-memo"
            v-model="$v.metaData.memo.$model"
            :state="!$v.metaData.memo.required">
        </common-text-area-max-length>
         <!--  FOOTER: 액션 -->
        <template slot="modal-footer">
            <div class="flex-grow-1">
            </div>
            <div>
                <b-button variant="outline-primary default cutom-label" @click="submitConfirm">수정</b-button>
                <b-button variant="outline-danger default cutom-label-cancel" @click="close">취소</b-button>
            </div>
        </template>
    </b-modal>
    <!-- 수정 확인 창 -->
    <common-confirm
        id="modalModify"
        title="메타 데이터 수정 확인"
        message="메타 데이터를 수정하시겠습니까?"
        submitBtn="수정"
        @ok="submit()"
    />
</div>
</template>

<script>
import mixinValidate from '../../mixin/MixinValidate';
import { mapMutations, mapActions } from 'vuex';
import { USER_ID, INPUT_MAX_LENGTH } from '@/constants/config';

export default {
    mixins: [ mixinValidate ],
    props: {
        show: {
            type: Boolean,
            default: false,
            USER_ID,
        }
    },
    data() {
        return {
            metaData: {
                seq: 0,
                title: '',
                memo: '',
                mediaCD: 'A',                     // 매체
                categoryCD: '',                         // 분류
            },
            primaryOptions: [],                   // 공유매체 목록
            primaryCodeOptions: [],               // 공유소재 분류 목록
            INPUT_MAX_LENGTH
        }
    },
    computed: {
        showDialog: {
            get() {
                return this.show;
            },
            set(v) {
                if (!v) {
                    this.reset();
                    this.$emit('close');
                }
            }
        },
    },
    methods: {
        submitConfirm() {
            if (!this.$v.metaData.title.$invalid 
                || !this.$v.metaData.memo.$invalid
                || !this.$v.metaData.mediaCD.$invalid
                || !this.$v.metaData.categoryCD.$invalid
            ) {
                this.$fn.notify('inputError', {});
                return;
            }

            this.$bvModal.show('modalModify');
        },
        submit() {
            this.$bvModal.hide('modalModify')
            const userId = sessionStorage.getItem(USER_ID);
           
            this.$http.put(`/api/products/workspace/public/meta/${userId}`, this.metaData,
                {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                    }
                })
                .then(res => {
                    if (res.status === 200 && !res.data.errorMsg) {
                        this.$fn.notify('success', { message: '메타 데이터가 수정되었습니다.' })
                        this.$emit('editSuccess');
                        this.showDialog = false;
                    } else {
                        this.$fn.notify('error', { message: '메타 데이터가 수정 실패: ' + res.data.errorMsg })
                    }
                })
        },
        setData({ seq, title, memo, mediaCD, categoryCD }) {
            this.metaData.seq = seq;
            this.metaData.title = title;
            this.metaData.memo = memo;
            this.metaData.mediaCD = mediaCD;
            this.metaData.categoryCD = categoryCD;
            this.getPrimaryOptions();
            this.getPrimaryCodeOptions();
        },
        reset() {
            this.metaData = {
                title: '',
                memo: '',
                mediaCD: 'A',
                categoryCD: '',    
            };
        },
        close() {
            this.showDialog = false;
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
            this.$http.get('/api/Categories/public-codes/primary/' + this.metaData.mediaCD)
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