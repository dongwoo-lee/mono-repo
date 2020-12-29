<template>
    <b-modal
        dialog-class="copyright-modal"
        v-model="showDialog"
        no-close-on-backdrop
        hide-header
        hide-footer>
        <div style="padding-bottom:20px;text-align: center;">
            <h4>음원 다운로드 저작권 확인</h4>
        </div>
        <div style="font-size:0.9rem;">
            해당 음원은 업무 사용으로 한정하며, 저작권 보호, 음원 외부 유출 금지에 동의하십니까?
            (동의하지 않는 경우 음원 파일을 다운로드 받을 수 없습니다.)
        </div>
        <div class="btn-copyright-wrap">
             <b-button
              variant="outline-primary default cutom-label"
              size="sm"
              class="float-right"
              @click="agree()"
            >
              동의하고 다운로드
            </b-button>
            <b-button
              variant="outline-danger default cutom-label-cancel"
              size="sm"
              class="float-right"
              @click="close()"
            >
              비동의
            </b-button>
        </div>
    </b-modal>
</template>
<script>
export default {
    props: {
        show: {
            type: Boolean,
            default: false,
        }
    },
    computed: {
        showDialog: {
            get() {
                console.info('soundCopyrightPopup' , this.show);
                return this.show;
            },
            set(v) {
                if (!v) {
                    this.$emit('close');
                }
            }
        },
    },
    beforeDestroy() {
        this.close();
    },
    methods: {
        agree() {
            this.$emit('agree');
        },
        close() {
            this.showDialog = false;
        }
    }
}
</script>
<style>
@media (min-width: 576px)
 {
    .copyright-modal {
        top: 10%;
        max-width: 530px;
        margin: 1.75rem auto;
    }
 }

.btn-copyright-wrap {
    display: flex;
    justify-content: space-evenly;
    padding-top: 25px;
}
</style>