<template>
  <b-form-input
    ref="input"
    :class="[classString]"
    :value="value"
    :placeholder="placeholder"
    @input="onInput"
    :pattern="pattern"
    @keyup.enter="enterKeydown"
    @keydown.enter.prevent
  >
  </b-form-input>
</template>

<script>
export default {
  props: {
    value: {
      type: String,
      default: ""
    },
    type: {
      type: String,
      default: "text"
    },
    classString: {
      type: String,
      default: ""
    },
    placeholder: {
      type: String,
      default: ""
    },
    pattern: {
      type: String,
      default: ""
    }
  },
  methods: {
    enterKeydown() {
      this.$emit("keydown");
    },
    onInput(input) {
      this.$emit("input", input);
    }
  }
};
</script>
