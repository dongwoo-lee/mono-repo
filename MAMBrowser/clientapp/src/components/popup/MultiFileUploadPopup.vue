<template>
    <div>
        <b-modal 
            id="upload-modal-closing"
            title="파일 업로드" 
            modal-class="my-modal-file"
            no-close-on-backdrop
            v-model="showDialog"
        >
            <!-- HEADER-CLOSE -->
            <template slot="modal-header-close">
                <div>
                    <button type="button" class="close float-left" @click.stop="onHide">_</button>
                    <button type="button" class="close" @click="showDialog=false">×</button>
                </div>
            </template>
            <!-- BODY -->
            <b-container class="my-container">
                <div class="drop-active" v-show="dropActive">
                    <h3>파일을 올려주세요.</h3>
                    <label class="btn btn-primary default" @click="onFileSelect">파일 선택</label> -->
                    <b-form-file ref="refFileSelect" v-model="files" @input="onFileUpload" class="mt-3" plain multiple v-show="false"></b-form-file>

                    <!-- <file-upload
                        class="btn btn-primary default dropdown-toggle"
                        :post-action="postAction"
                        :put-action="putAction"
                        :extensions="extensions"
                        :accept="accept"
                        :multiple="multiple"
                        :directory="directory"
                        :size="size || 0"
                        :thread="thread < 1 ? 1 : (thread > 5 ? 5 : thread)"
                        :headers="headers"
                        :data="data"
                        :drop="drop"
                        :drop-directory="dropDirectory"
                        :add-index="addIndex"
                        v-model="files"
                        @input-filter="inputFilter"
                        @input-file="inputFile"
                        ref="upload">
                        <span>Select</span>
                    </file-upload> -->
                </div>
                <div v-show="!dropActive">
                    <vuetable
                        table-height="200px"
                        ref="vuetable-scrollable"
                        :api-mode="false"
                        :fields="fields"
                        no-data-template=""
                    >
                    </vuetable>
                </div>
            </b-container>
            <template slot="modal-footer">
                <b-button variant="success default" @click="onSubmit">전송</b-button>
                <b-button variant="danger default" @click="showDialog=false">닫기</b-button>
            </template>
        </b-modal>
    </div>
</template>

<script>
import Vuetable from "vuetable-2/src/components/Vuetable";

export default {
    components: { Vuetable },
    props: ['show'],
    computed: {
        showDialog: {
            get() {
                return this.show;
            },
            set(v) {
                if (!v) this.$emit('close')
            }
        },
    },
    data() {
        return {
            files: [],
            dropActive: true,
            fields: [
                {
                    name: 'no',
                    title: 'No',
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                },
                {
                    name: 'fileName',
                    title: '파일명',
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                },
                {
                    name: 'size',
                    title: '사이즈',
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                },
                {
                    name: 'status',
                    title: '상태',
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                }
            ]
        }
    },
    methods: {
       onHide() {
           console.info('hide');
       },
       onSubmit() {
           console.info('submit');
       },
       onFileSelect() {
           this.$refs.refFileSelect.$el.click();
       },
       onFileUpload() {
           console.info('files', this.files);
       }
    }
}
</script>