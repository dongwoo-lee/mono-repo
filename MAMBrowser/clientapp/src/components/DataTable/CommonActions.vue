<template>
    <div>
        <b-button
            v-if="display(PREVIEW_CODE)"
            class="icon-buton"
            title="미리듣기"
            @click.stop="onPreview()">
            <b-icon icon="caret-right-square" class="icon"></b-icon>
        </b-button>
        <b-button
            v-if="display(DOWNLOAD_CODE)"
            :id="`download-${rowData.rowNO}`" class="icon-buton"
            v-b-tooltip.hover.top="{ title: rowData.filePath }"
            @click.stop="onDownload()">
            <b-icon icon="download" class="icon"></b-icon>
        </b-button>           
        <b-button
            v-if="displayEtc('delete')"
            class="icon-buton"
            :title="getTitle('delete')"
            @click.stop="onDelete()">
            <b-icon icon="dash-square" class="icon" variant="danger"></b-icon>
        </b-button>
        <b-button
            v-if="displayEtc('modify')"
            class="icon-buton"
            :title="getTitle('modify')"
            @click.stop="onMetaModify()">
            <b-icon icon="exclamation-square" class="icon" variant="info"></b-icon>
        </b-button>
    </div>
</template>
<script>
import { PREVIEW_CODE, DOWNLOAD_CODE } from "@/constants/config";

export default {
    props:{
        rowData: {
            type: Object,
            default: () => {}
        },
        etcData: {
            type: Array,
            default: () => []  // ['delete', 'modify']
        },
        etcTitles: {
            type: Object,
            default: () => {}  // {'delete': '삭제', 'modify': '정보편집'}
        },
        behaviorData: {
            type: Array,
            default: () => []
        },
    },
    data() {
        return {
            PREVIEW_CODE: PREVIEW_CODE,
            DOWNLOAD_CODE: DOWNLOAD_CODE
        }
    },
    methods: {
        display(value) {
            return this.behaviorData.some(data => data.id === value && data.visible === 'Y');
        },
        displayEtc(value) {
            return this.etcData.some(data => data === value);
        },
        onPreview() {
            this.$emit('preview', this.rowData);
        },
        onDownload() {
            this.$emit('download', this.rowData);
        },
        onDelete() {
            this.$emit('delete', this.rowData);
        },
        onMetaModify() {
            this.$emit('modify', this.rowData);
        },
        getTitle(type) {
            if (this.etcTitles && this.etcTitles[type]) {
                return this.etcTitles[type];
            }
            if (type === 'delete') { return '휴지통'; }
            if (type === 'modify') { return '정보편집';}
            return '';
        }
    },
}
</script>