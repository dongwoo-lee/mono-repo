<template>
  <div>
    <b-modal
      :id="modalId"
      size="lg"
      centered
      :title="modalTitle"
      ok-title="확인"
      cancel-title="취소"
      @show="showEditPopup"
    >
      <div>
        <div class="dx-fieldset">
          <div v-for="(item, index) in items_copy" :key="index">
            <span style="float: center">
              <b-form-group
                v-if="item.type === 'text'"
                :label="item.label"
                class="has-float-label"
                style="margin: 15px 100px 15px 100px"
              >
                <b-form-input
                  v-model="item.editedVal"
                  :state="item.state === 'notNull' ? item.isState : null"
                  :maxlength="item.maxLength > 0 ? item.maxLength : null"
                  :disabled="textDisabledList.includes(item.key)"
                  autocomplete="new-password"
                  :type="item.inputType"
                  @input="onTextInput($event, item)"
                />
              </b-form-group>
              <div
                v-if="item.type === 'codename_check'"
                style="
                  margin: 0px 100px 0px 100px;
                  display: flex;
                  align-items: center;
                "
              >
                <b-form-group
                  :label="item.label"
                  class="has-float-label"
                  style="flex-grow: 6; margin: 0px"
                >
                  <b-form-input
                    v-model="item.editedVal"
                    :state="item.state === 'notNull' ? item.isState : null"
                    :maxlength="item.maxLength > 0 ? item.maxLength : null"
                    :disabled="textDisabledList.includes(item.key)"
                    autocomplete="new-password"
                    :type="item.inputType"
                    @input="onTextInput($event, item)"
                  />
                </b-form-group>
                <b-form-checkbox
                  id="checkbox-1"
                  name="checkbox-1"
                  v-model="item.isStop"
                  :value="true"
                  :unchecked-value="false"
                  style="flex-grow: 1; text-align: center"
                >
                  폐지
                </b-form-checkbox>
              </div>
              <b-form-group
                v-if="item.type === 'select'"
                :label="item.label"
                class="has-float-label"
                style="margin: 15px 100px 15px 100px"
              >
                <b-form-select
                  v-model="item.editedVal"
                  :options="item.selectOptions"
                  :state="item.state === 'notNull' ? item.isState : null"
                  :disabled="textDisabledList.includes(item.key)"
                  @input="onTextInput($event, item)"
                >
                </b-form-select>
              </b-form-group>
              <div
                v-if="item.type === 'check'"
                style="display: flex"
                class="mt-5"
              >
                <span
                  v-for="(check_group, index) in item.checkGroups"
                  :key="index"
                  style="flex-grow: 1; margin: 0 5%"
                >
                  <b-form-group :label="check_group.groupLabel">
                    <b-form-checkbox-group
                      v-model="check_group.selected"
                      :options="check_group.options"
                      @input="onCheckInput($event, item, index)"
                      stacked
                    ></b-form-checkbox-group>
                  </b-form-group>
                </span>
              </div>
            </span>
          </div>
        </div>
      </div>

      <template #modal-footer="{ cancel }">
        <DxButton :width="100" text="취소" @click="cancel()" />
        <DxButton
          type="default"
          text="확인"
          styling-mode="outlined"
          :disabled="checkValidation()"
          :width="100"
          @click="editOk()"
        >
        </DxButton>
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
    modalTitle: {
      type: String,
    },
    modalId: {
      type: String,
    },
    textDisabledList: {
      type: Array,
      default: () => [],
    },
  },
  data() {
    return {
      items_copy: [],
      CheckBoxSelected: [],
      checkBoxOptions: [],
      invalidFeedback: "테스트중",
    };
  },
  components: { DxButton },
  methods: {
    showEditPopup() {
      this.items_copy = _.cloneDeep(this.items);
      this.clearEditVal();
    },
    setEditValue() {
      this.items_copy.forEach((ele) => {
        ele.value = ele.editedVal;
      });
    },
    clearEditVal() {
      this.items_copy.forEach((ele) => {
        if (!ele.editedVal) {
          ele.editedVal = _.cloneDeep(ele.value);
        }
        if (ele.state === "notNull" && !ele.value) ele.isState = false;
        else ele.isState = null;
      });
    },
    cancel() {
      this.clearEditVal();
    },
    checkValidation() {
      return this.items_copy.some((ele) => ele.isState == false);
    },
    editOk() {
      this.setEditValue();
      this.$emit("editOk", this.items_copy);
    },
    async onTextInput(event, item) {
      // const regex = /[ㄱ-ㅎㅏ-ㅣ가-힣]/g;
      // if (regex.test(item.editedVal)) {
      //   item.editedVal = await item.editedVal.replace(regex, "");
      // }
      this.isNotNull(event, item);
      this.isOtherValidation(event, item);
    },
    onCheckInput(event, item, index) {
      this.$emit("checkGroupClick", event, item, index);
    },
    isNotNull(text, item) {
      if (text) {
        item.isState = null;
      } else {
        item.isState = false;
      }
      this.$forceUpdate();
    },
    isOtherValidation(text, item) {
      this.$emit("otherValidation", text, item, this.items_copy);
      this.$forceUpdate();
    },
  },
};
</script>
<style>
.check-group-item {
  display: flex;
}
.validation-message {
  position: absolute;
  top: 100%;
  left: 0;
  color: #dc3545;
  font-size: 14px;
  margin-top: 5px;
}
</style>
