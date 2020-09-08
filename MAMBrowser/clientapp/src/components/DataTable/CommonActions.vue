<template>
    <div>
        <b-button
            v-if="display('preview')"
            class="icon-buton"
            @click.stop="onPreview()">
            <b-icon icon="caret-right-square" class="icon"></b-icon>
        </b-button>
        <b-button
            v-if="display('download')"
            :id="`download-${rowData.rowNO}`" class="icon-buton"
            v-b-tooltip.hover.top="{ title: rowData.filePath }"
            @click.stop="onDownload()">
            <b-icon icon="download" class="icon"></b-icon>
        </b-button>           
        <b-button
            v-if="display('delete')"
            class="icon-buton" title="휴지통"
            @click.stop="onDelete()">
            <b-icon icon="dash-square" class="icon" variant="danger"></b-icon>
        </b-button>
        <b-button
            v-if="display('modify')"
            class="icon-buton" title="정보편집"
            @click.stop="onMetaModify()">
            <b-icon icon="exclamation-square" class="icon" variant="info"></b-icon>
        </b-button>
    </div>
</template>
<script>
export default {
    props:{
        rowData: {
            type: Object,
            default: () => {}
        },
        arrData: {
            type: Array,
            default: () => []
        }
    },
    methods: {
        display(value) {
            return this.arrData.some(data => data === value);
        },
        onPreview() {
            this.$emit('preview', this.rowData);
        },
        onDownload() {
            this.$emit('download', this.rowData.seq);
        },
        onDelete() {
            this.$emit('delete', this.rowData.seq);
        },
        onMetaModify() {
            this.$emit('modify', this.rowData);
        },
    },
}
</script>