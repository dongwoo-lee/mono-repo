<template>
    <b-modal 
        :id="id" 
        size="sm" 
        :title="title"
        :hideHeaderClose="true">
        {{ message }}
        <template v-slot:modal-footer>
            <b-button
              variant="primary"
              size="sm"
              class="float-right"
              @click="ok()"
            >
              {{submitBtn}}
            </b-button>
            <b-button
              variant="danger"
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
            default: '확인'
        },
        closeBtn: {
            type: String,
            default: '닫기',
        }
    },
    // data() {
    //     return {
    //         title: '',
    //         message: '',
    //         submitString: '확인',
    //         closeString: '닫기',
    //     }
    // },
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