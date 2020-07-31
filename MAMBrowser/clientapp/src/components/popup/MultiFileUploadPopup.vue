<template>
    <div>
        <b-modal 
            id="upload-modal-closing"
            title="파일 업로드" 
            size="lg"
            modal-class="my-modal-file"
            no-close-on-backdrop
            v-model="showDialog"
            hide-footer
        >
        <!-- footer-class="c-modal-upload-footer" -->
            <!-- HEADER-CLOSE -->
            <template slot="modal-header-close">
                <div>
                    <b-button variant="outline-primary" size="sm" @click.stop="onHide">
                        <i class="iconsminds-minimize"></i>최소화
                    </b-button>
                    <b-button variant="outline-danger" class="icon-button" @click="showDialog=false">
                        <i class="simple-icon-close"></i>
                    </b-button>
                </div>
            </template>
            <!-- BODY: 파일업로드 -->
            <c-file-upload></c-file-upload>
       
            <!--  FOOTER: 액션 -->
            <!-- <template slot="modal-footer">
                <div class="flex-grow-1">
                </div>
                <div>
                    <b-button variant="outline-success default" @click="onSubmit">
                        <b-icon icon="upload"></b-icon>업로드
                    </b-button>
                </div>
            </template> -->
        </b-modal>
    </div>
</template>

<script>
import CFileUpload from '../file/CFileUpload';
export default {
    components: { CFileUpload },
    props: ['show'],
    computed: {
        showDialog: {
            get() {
                return this.show;
            },
            set(v) {
                if (!v) { 
                    this.$emit('close');
                }
            }
        },
    },
    methods: {
       onHide() {
           console.info('hide');
       },
       onSubmit() {
           console.info('submit');
       },
    }
}
</script>
