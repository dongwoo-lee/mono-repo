<template>
  <b-row class="h-100">
    <b-colxx xxs="12" md="10" class="mx-auto my-auto">
      <b-card class="auth-card" no-body>
        <div class="position-relative image-side"></div>
        <div class="form-side">
          <router-link tag="a" to="/">
            <p class="title">MIROS 소재 검색 시스템</p>
            <p class="h5">(MIROS MAM Browser)</p>
            <br />
            <br />
          </router-link>

          <h6 class="mb-4">Login</h6>

          <b-form
            @submit.prevent="formSubmit"
            class="av-tooltip tooltip-label-bottom"
          >
            <b-form-group label="아이디" class="has-float-label mb-4 h-45">
              <b-form-input
                type="text"
                v-model="$v.userId.$model"
                :state="!$v.userId.$error"
              />
              <b-form-invalid-feedback v-if="!$v.userId.required"
                >아이디를 입력해주세요.</b-form-invalid-feedback
              >
            </b-form-group>

            <b-form-group label="패스워드" class="has-float-label mb-4 h-45">
              <b-form-input
                type="password"
                v-model="$v.password.$model"
                :state="!$v.password.$error"
              />
              <b-form-invalid-feedback v-if="!$v.password.required"
                >패스워드를 입력해주세요.</b-form-invalid-feedback
              >
            </b-form-group>
            <div class="d-flex justify-content-between align-items-center">
              <div style="width: 240px">
                <div v-if="errorMsg" style="color: #dc3545">{{ errorMsg }}</div>
                <!-- <b-form-checkbox
                                value="accepted"
                            >로그인 정보 기억</b-form-checkbox> -->
              </div>
              <b-button
                type="submit"
                variant="primary"
                size="lg"
                :class="{ 'btn-multiple-state btn-shadow': true }"
              >
                <b-spinner v-show="processing" small type="grow"></b-spinner>
                <span v-show="processing">로그인중...</span>
                <span v-show="!processing" class="label">로그인</span>
              </b-button>
            </div>
          </b-form>
          <p class="mb-0" style="position: absolute; right: 75px; bottom: 25px">
            로그인 문제시 라디오기술부(2973)으로 문의 바랍니다.
          </p>
          <p class="mb-0" style="position: absolute; right: 75px; bottom: 5px">
            (MIROS ID로 로그인)
          </p>
          <a id="alink" class="btn-link" @click="showModal"
            >관련 매뉴얼/프로그램 다운로드</a
          >
        </div>
      </b-card>
    </b-colxx>
    <b-modal ref="manualModal" size="lg" ok-only scrollable>
      <div>
        <b-table
          class="custom-table"
          thead-class="custom-table-color"
          show-empty
          empty-text="데이터가 없습니다."
          striped
          hover
          :items="manualData"
        >
          <template #cell(downloadLink)="row">
            <span
              v-if="row.item.downloadLink"
              class="btn-link alinkclass"
              @click="linkClick(row.item.downloadLink)"
              >다운로드</span
            >
          </template>
        </b-table>
      </div>
    </b-modal>
  </b-row>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import { validationMixin } from "vuelidate";
import axios from "axios";
const { required } = require("vuelidate/lib/validators");

export default {
  data() {
    return {
      userId: "",
      password: "",
      errorMsg: "",
      manualData: [],
    };
  },
  mixins: [validationMixin],
  validations: {
    password: {
      required,
    },
    userId: {
      required,
    },
  },
  computed: {
    ...mapGetters("user", ["processing", "roleList"]),
  },
  watch: {
    password(v) {
      this.errorMsg = "";
    },
  },
  methods: {
    ...mapActions("user", ["login"]),
    formSubmit() {
      this.$v.$touch();
      if (!this.$v.$anyError) {
        this.login({
          userId: this.userId,
          pass: this.password,
        })
          .then((res) => {
            if (res.status === 200) {
              if (res.data.resultCode !== 0) {
                this.errorMsg = res.data.errorMsg;
              } else {
                if (this.roleList) {
                  const firstVisibleIndex = this.roleList.findIndex(
                    (role) => role.visible === "Y"
                  );
                  if (this.roleList[firstVisibleIndex]) {
                    this.$router.push(this.roleList[firstVisibleIndex].to);
                  }
                } else {
                  this.$router.push("/");
                }
              }
            }
          })
          .catch((error) => {
            Promise.reject(error);
          });
      }
    },
    showModal() {
      axios.get("/manual.json").then((rs) => {
        this.manualData = rs.data;
        this.$refs["manualModal"].show();
      });
    },
    hideModal() {
      this.$refs["manualModal"].hide();
    },
    linkClick(linkUrl) {
      const link = document.createElement("a");
      link.target = "_blank";
      link.href = linkUrl;
      link.click();
    },
  },
};
</script>
<style scoped>
#alink {
  position: absolute;
  top: 450px;
  left: 10px;
  z-index: 2000;
  color: blue;
  cursor: pointer;
  text-decoration: underline;
}
.alinkclass {
  color: blue;
  cursor: pointer;
  text-decoration: underline;
}
</style>
