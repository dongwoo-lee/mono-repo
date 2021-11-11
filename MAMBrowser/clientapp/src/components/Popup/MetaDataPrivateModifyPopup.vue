<template>
  <div>
    <b-modal
      id="upload-modal-closing"
      title="메타데이터 수정"
      size="md"
      modal-class="my-modal-file"
      no-close-on-backdrop
      v-model="showDialog"
    >
      <!-- 제목 -->
      <common-input-text-max-length
        label="제목"
        labelfor="input-title"
        v-model="$v.metaData.title.$model"
        :state="!$v.metaData.title.required"
      >
      </common-input-text-max-length>
      <!-- 내용 -->
      <common-text-area-max-length
        label="내용"
        label-for="input-memo"
        v-model="$v.metaData.memo.$model"
        :state="!$v.metaData.memo.required"
      >
      </common-text-area-max-length>
      <!--  FOOTER: 액션 -->
      <template slot="modal-footer">
        <div class="flex-grow-1"></div>
        <div>
          <b-button
            variant="outline-primary default cutom-label"
            @click="submitConfirm"
            :disabled="invalid()"
            >수정</b-button
          >
          <b-button
            variant="outline-danger default cutom-label-cancel"
            @click="close"
            >취소</b-button
          >
        </div>
      </template>
    </b-modal>
    <!-- 수정 확인 창 -->
    <common-confirm
      id="modalModify"
      title="메타 데이터 수정 확인"
      message="메타 데이터를 수정하시겠습니까?"
      submitBtn="수정"
      @ok="submit()"
    />
  </div>
</template>

<script>
import mixinValidate from "../../mixin/MixinValidate";
import { mapMutations, mapActions } from "vuex";
import { USER_ID, INPUT_MAX_LENGTH } from "@/constants/config";

export default {
  mixins: [mixinValidate],
  props: {
    show: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      metaData: {
        seq: 0,
        title: "",
        memo: "",
        USER_ID
      },
      INPUT_MAX_LENGTH
    };
  },
  computed: {
    showDialog: {
      get() {
        return this.show;
      },
      set(v) {
        if (!v) {
          this.typeReset();
          this.$emit("close");
        }
      }
    }
  },
  methods: {
    submitConfirm() {
      if (this.invalid()) {
        this.$fn.notify("inputError", {});
        return;
      }

      this.$bvModal.show("modalModify");
    },
    submit() {
      this.$bvModal.hide("modalModify");
      const userId = sessionStorage.getItem(USER_ID);
      const formData = new FormData();

      this.$http
        .put(`/api/products/workspace/private/meta/${userId}`, this.metaData)
        .then(res => {
          if (res.status === 200 && !res.data.errorMsg) {
            this.$fn.notify("primary", {
              message: "메타 데이터가 수정되었습니다."
            });
            this.$emit("editSuccess");
            this.showDialog = false;
          } else {
            this.$fn.notify("error", {
              message: "메타 데이터가 수정 실패: " + res.data.errorMsg
            });
          }
        });
    },
    setData({ seq, title, memo }) {
      this.metaData.seq = seq;
      this.metaData.title = title;
      this.metaData.memo = memo;
    },
    invalid() {
      return (
        !this.$v.metaData.title.$invalid || !this.$v.metaData.memo.$invalid
      );
    },
    reset() {
      this.metaData = {
        title: "",
        memo: ""
      };
    },
    close() {
      this.showDialog = false;
    }
  }
};
</script>
