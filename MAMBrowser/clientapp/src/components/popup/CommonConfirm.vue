<template>
    <b-modal 
        :id="id" 
        size="sm" 
        :title="title"
        modal-class="common-confirm"
        :hideHeaderClose="true">
        <span v-html="message"></span>
        <!-- {{ message }} -->
        <template v-slot:modal-footer>
            <b-button
              v-if="submitBtn"
              variant="outline-primary default cutom-label"
              size="sm"
              class="float-right"
              @click="ok()"
            >
              {{submitBtn}}
            </b-button>
            <b-button
              variant="outline-danger default cutom-label-cancel"
              size="sm"
              class="float-right"
              @click="close()"
            >
              {{closeBtn}}
            </b-button>
      </template>
    </b-modal>
</template>
<script>
export default {
    props: {
        id: {
            type: String,
            default: 'commonModal',
        },
        title: {
            type: String,
            default: '',
        },
        message: {
            type: String,
            default: '',
        },
        submitBtn: {
            type: String,
        },
        closeBtn: {
            type: String,
            default: '닫기',
        }
    },
    methods: {
        ok() {
            this.$emit('ok');
        },
        close() {
            this.$bvModal.hide(this.id)
        }
    }
}
</script>