<template>
<b-row class="h-100">
    <b-colxx xxs="12" md="10" class="mx-auto my-auto">
        <b-card class="auth-card" no-body>
            <div class="position-relative image-side">
                <p class="text-white h2">MAMBrowser</p>
                <!-- <p class="white mb-0">
                    Please use your credentials to login.
                    <br />If you are not a member, please
                    <router-link tag="a" to="/user/register" class="white">register</router-link>.
                </p> -->
            </div>
            <div class="form-side">
                <router-link tag="a" to="/">
                    <span class="logo-single" />
                </router-link>
                <h6 class="mb-4">Login</h6>

                <b-form @submit.prevent class="av-tooltip tooltip-label-bottom">
                    <b-form-group label="아이디" class="has-float-label mb-4">
                        <b-form-input type="text" v-model="$v.userId.$model" :state="!$v.userId.$error"/>
                        <b-form-invalid-feedback v-if="!$v.userId.required">아이디를 입력해주세요.</b-form-invalid-feedback>
                    </b-form-group>

                    <b-form-group label="패스워드" class="has-float-label mb-4">
                        <b-form-input type="password" v-model="$v.password.$model" :state="!$v.password.$error" />
                        <b-form-invalid-feedback v-if="!$v.password.required">패스워드를 입력해주세요.</b-form-invalid-feedback>
                    </b-form-group>
                    <div class="d-flex justify-content-between align-items-center">
                        <b-form-checkbox
                            value="accepted"
                        >로그인 정보 기억</b-form-checkbox>
                        <b-button
                            @click="formSubmit"
                            type="button" variant="primary" size="lg" :disabled="processing"
                            :class="{'btn-multiple-state btn-shadow': true }">
                            <span class="spinner d-inline-block">
                                <span class="bounce1"></span>
                                <span class="bounce2"></span>
                                <span class="bounce3"></span>
                            </span>
                            <span class="icon success">
                                <i class="simple-icon-check"></i>
                            </span>
                            <span class="icon fail">
                                <i class="simple-icon-exclamation"></i>
                            </span>
                            <span class="label">로그인</span>
                        </b-button>
                    </div>
                    <div v-if="errorMsg" style="color: #dc3545">로그인에 실패하였습니다. 아이디 및 패스워드를 확인해주세요.</div>
                </b-form>
            </div>
        </b-card>
    </b-colxx>
</b-row>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import {
    validationMixin
} from "vuelidate";
const { required } = require("vuelidate/lib/validators");

export default {
    data() {
        return {
            userId: '',
            password: '',
            errorMsg: '',
        };
    },
    mixins: [validationMixin],
    validations: {
        password: {
            required,
        },
        userId: {
            required,
        }
    },
    computed: {
        ...mapGetters('user', ['currentUser', 'processing'])
    },
    watch: {
        password(v) {
            this.errorMsg = '';
        }
    },
    methods: {
        ...mapActions('user', ['login']),
        formSubmit() {
            console.info('.$v', this.$v);
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
                            this.$router.push("/");
                        }
                    } else {
                        var errMsg = res.response.data.message;
                        this.$notify("error", "Login Error", errMsg, {
                            duration: 3000,
                            permanent: false
                        });
                    }
                });
            }
        }
    },
};
</script>
