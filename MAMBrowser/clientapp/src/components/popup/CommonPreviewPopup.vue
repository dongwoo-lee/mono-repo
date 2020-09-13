<template>
    <b-modal
        title="미리듣기"
        size="sm"
        v-model="showDialog"
    >
    미리듣기 팝업 내용(들어오는 데이터)<br>
    {{item}}
    </b-modal>
</template>
<script>
export default {
    props: {
        show: {
            type: Boolean,
            default: false,
        },
        item: {
            type: Object,
            default: () => {},
        }
    },
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
}
</script>