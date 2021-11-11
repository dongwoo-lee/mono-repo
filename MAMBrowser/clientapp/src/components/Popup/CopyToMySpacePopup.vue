<template>
  <div>
    <b-modal
      id="upload-modal-closing"
      title="메타데이터 입력"
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
            @click="submit()"
            :disabled="invalid() || status"
            >MY공간으로 복사
            <span
              v-if="status"
              class="spinner-border spinner-border-sm"
              aria-hidden="true"
            ></span>
          </b-button>
          <b-button
            variant="outline-danger default cutom-label-cancel"
            @click="close"
            >취소</b-button
          >
        </div>
      </template>
    </b-modal>
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
        title: "",
        memo: "",
        fileSize: 0,
        filePath: "copy.wav"
      },
      INPUT_MAX_LENGTH,
      rowData: {},
      status: false
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
      this.status = true;
      this.$http
        .post(`/api/products/workspace/private/verify/${userId}`, this.metaData)
        .then(res => {
          if (res.status === 200 && !res.data.errorMsg) {
            this.$emit("ok");
            this.showDialog = false;
            this.status = false;
          } else {
            // this.$fn.notify('error', { message: res.data.errorMsg })
            this.status = false;
          }
        })
        .catch(error => {
          this.status = false;
        });
    },
    setData(rowData) {
      this.metaData.title = "";
      this.metaData.memo = "";
      this.rowData = rowData;
    },
    getRowData() {
      return this.rowData;
    },
    getMetaData() {
      return this.metaData;
    },
    invalid() {
      return (
        !this.$v.metaData.title.$invalid || !this.$v.metaData.memo.$invalid
      );
    },
    reset() {
      this.metaData = {
        title: "",
        memo: "",
        fileSize: 0,
        filePath: "copy.wav"
      };
    },
    close() {
      this.showDialog = false;
    }
  }
};
</script>
