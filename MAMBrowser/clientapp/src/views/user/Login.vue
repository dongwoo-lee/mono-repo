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

                <b-form @submit.prevent="formSubmit" class="av-tooltip tooltip-label-bottom">
                    <b-form-group label="아이디" class="has-float-label mb-4">
                        <b-form-input type="text" v-model="$v.userId.$model" :state="!$v.userId.$error"/>
                        <b-form-invalid-feedback v-if="!$v.userId.required">아이디를 입력해주세요.</b-form-invalid-feedback>
                    </b-form-group>

                    <b-form-group label="패스워드" class="has-float-label mb-4">
                        <b-form-input type="password" v-model="$v.password.$model" :state="!$v.password.$error" />
                        <b-form-invalid-feedback v-if="!$v.password.required">패스워드를 입력해주세요.</b-form-invalid-feedback>
                    </b-form-group>
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <div v-if="errorMsg" style="color: #dc3545">{{ errorMsg }}</div>
                            <!-- <b-form-checkbox
                                value="accepted"
                            >로그인 정보 기억</b-form-checkbox> -->
                        </div>
                        <b-button
                            type="submit" variant="primary" size="lg" 
                            :class="{'btn-multiple-state btn-shadow': true }">
                             <b-spinner v-show="processing" small type="grow"></b-spinner>
                             <span v-show="processing">로그인중...</span>
                            <span v-show="!processing" class="label">로그인</span>
                        </b-button>
                    </div>
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
        ...mapGetters('user', ['processing', 'roleList'])
    },
    watch: {
        password(v) {
            this.errorMsg = '';
        }
    },
    methods: {
        ...mapActions('user', ['login']),
        formSubmit() {
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
                            if (this.roleList) {
                                const firstVisibleIndex = this.roleList.findIndex(role => role.visible === 'Y' );
                                if (this.roleList[firstVisibleIndex]) {
                                    this.$router.push(this.roleList[firstVisibleIndex].to);
                                }
                            } else {
                                this.$router.push("/");
                            }
                        }
                    } else {
                        var errMsg = res.response.data.message;
                        this.$notify("error", "Login Error", errMsg, {
                            duration: 3000,
                            permanent: false
                        });
                    }
                })
                .catch(error => {
                    Promise.reject(error);
                });
            }
        }
    },
};
</script>
