<template>
  <b-modal 
    id="loginPopup" 
    title="로그인"
    size="sm"
    no-close-on-backdrop
    modal-class="common-confirm"
    v-model="isShow"
    @close.prevent
    :hideHeaderClose="true"
    :hide-footer="true">
      <!-- contents -->
      <div class="form-side">
        <b-form @submit.prevent="formSubmit" class="av-tooltip tooltip-label-bottom">
            <b-form-group label="아이디" class="has-float-label mb-4">
            <b-form-input type="text" v-model="userId" disabled/>
          </b-form-group>

          <b-form-group label="패스워드" class="has-float-label mb-4">
            <b-form-input type="password" v-model="$v.password.$model" :state="!$v.password.$error" />
            <div class="custom-invalid-feedback">
              <span v-if="$v.password.$error & !$v.password.required">패스워드를 입력해주세요.</span>
              <span v-if="errorMsg" style="color: #dc3545">{{ errorMsg }}</span>
            </div>
          </b-form-group>

          <div>
            <b-form-group style="float:right">
              <!-- 로그인 버튼 -->
              <b-button type="submit" variant="outline-primary default cutom-label">
                  <b-spinner v-show="processing" small type="grow"></b-spinner>
                  <span v-show="processing">로그인중...</span>
                <span v-show="!processing" class="label">로그인</span>
              </b-button>
              <!-- 닫기 버튼 -->
              <b-button variant="outline-danger default cutom-label-cancel" @click="close()">
                닫기
              </b-button>
            </b-form-group>
          </div>
        </b-form>
      </div>
    </b-modal>
</template>
<script>
import { mapGetters, mapActions, mapMutations } from "vuex";
import { validationMixin } from "vuelidate";
const { required } = require("vuelidate/lib/validators");
import { eventBus } from '@/eventBus';

export default {
  mixins: [validationMixin],
  validations: {
    password: {
      required,
    },
    userId: {
      required,
    }
  },
  data() {
    return {
      isShow: false,
      password: '',
      errorMsg: '',
    }
  },
  watch: {
    password(v) {
      this.errorMsg = '';
    }
  },
  computed: {
    ...mapGetters('user', ['processing', 'userId'])
  },
  beforeDestroy() {
    this.isShow = false;
  },
  methods: {
    ...mapActions('user', ['login', 'signOut']),
    ...mapMutations('user', ['SET_LOGOUT', 'SET_REMOVE_TOKEN']),
    show() {
      this.SET_REMOVE_TOKEN();
      this.isShow = true;
    },
    hide() {
        this.isShow = false;
    }, 
    formSubmit() {
      if (!this.userId) { return; }

      this.$v.$touch();
      if (!this.$v.$anyError) {
        this.login({
          userId: this.userId,
          pass: this.password
        }).then(res => {
          if (res.status === 200) {
              if (res.data.resultCode !== 0) {
                this.errorMsg = res.data.errorMsg;
              } else {
                this.$v.$reset();
                this.$bvModal.hide('loginPopup');
                eventBus.$emit('onResetTimer');
                eventBus.$emit('onLoadData', this.$router.currentRoute.name);
              }
          }

          this.password = '';
        })
      }
    },
    close() {
      this.SET_LOGOUT();
      this.$router.push({path: '/user/login' });
      this.hide();
    }
  }
}
</script>