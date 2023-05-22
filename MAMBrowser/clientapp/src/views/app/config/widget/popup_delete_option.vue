<template>
  <div>
    <b-modal
      id="modal-delete-option"
      size="lg"
      centered
      :title="modalTitle"
      ok-title="확인"
      cancel-title="취소"
      @show="showPopup"
    >
      <div>
        <div class="dx-fieldset">
          <div v-for="(item, index) in items_copy" :key="index">
            <span style="float: center">
              <b-form-group
                :label="item.label"
                class="has-float-label"
                style="margin: 15px 100px 15px 100px"
              >
                <b-form-select
                  v-model="item.editedVal"
                  @input="onTextInput($event, item)"
                >
                  <b-form-select-option
                    :value="option.value"
                    v-for="(option, index) in item.selectOptions"
                    :key="index"
                  >
                    {{ option.text }}
                  </b-form-select-option>
                </b-form-select>
              </b-form-group>
            </span>
          </div>
        </div>
      </div>

      <template #modal-footer="{ cancel }">
        <div>
          <b-input-group>
            <b-input-group-prepend v-if="cycleTime_copy">
              <span class="input-group-text">{{ cycleTime_copy.label }}</span>
            </b-input-group-prepend>
            <b-form-timepicker
              locale="en"
              v-model="cycleTime_copy.editedVal"
              class="delete-option-timepicker"
            ></b-form-timepicker>
          </b-input-group>
        </div>
        <div class="delete-option-btn">
          <DxButton :width="100" text="취소" @click="cancel()" />
          <DxButton
            type="default"
            text="확인"
            styling-mode="outlined"
            :width="100"
            @click="editOk()"
          >
          </DxButton>
        </div>
      </template>
    </b-modal>
  </div>
</template>
<script>
import DxButton from "devextreme-vue/button";
export default {
  props: {
    items: {
      type: Array,
      defalut: () => [],
    },
    cycleTime: {
      type: Object,
      default: () => {},
    },
    modalTitle: {
      type: String,
    },
  },
  data() {
    return {
      items_copy: [],
      copycycleTime_copy: {},
      selectedTime: null,
    };
  },
  components: { DxButton },
  methods: {
    showPopup() {
      this.items_copy = _.cloneDeep(this.items);
      this.cycleTime_copy = _.cloneDeep(this.cycleTime);
      this.clearEditVal();
    },
    setEditValue() {
      this.items_copy.forEach((ele) => {
        ele.value = ele.editedVal;
      });
      this.cycleTime_copy.value = this.cycleTime_copy.editedVal;
    },
    clearEditVal() {
      this.items_copy.forEach((ele) => {
        if (!ele.editedVal) {
          ele.editedVal = _.cloneDeep(ele.value);
        }
      });
      if (!this.cycleTime_copy.editedVal) {
        this.cycleTime_copy.editedVal = this.cycleTime_copy.value;
      }
    },
    cancel() {
      this.clearEditVal();
    },
    editOk() {
      this.setEditValue();
      this.$emit("editOk", this.items_copy, this.cycleTime_copy);
    },
    onTextInput(event, item) {},
  },
};
</script>
<style>
.check-group-item {
  display: flex;
}
.delete-option-timepicker output {
  margin: auto !important;
  padding: auto !important;
}
.delete-option-timepicker label {
  margin-top: 0.2rem !important;
  margin-right: 1rem !important;
}
#modal-delete-option___BV_modal_footer_ {
  justify-content: space-between;
}
</style>
