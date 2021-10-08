<template>
  <v-select
    class="v-select"
    :options="suggestions"
    label="name"
    @input="inputEvent"
    @search:focus="focusEvent"
    @search:blur="blurEvent"
    :value="vSelectValue"
  ></v-select>
</template>

<script>
export default {
  props: {
    suggestions: {
      type: Array,
      default() {
        return [];
      }
    },
    vSelectProps: {
      type: Object
    },
    vChangedProps: {
      type: Boolean
    }
  },
  data() {
    return {
      selected: false,
      deselected: false,
      vSelectValue: { id: null, name: null }
    };
  },
  watch: {
    vSelectProps(value) {
      this.vSelectValue = value;
    }
  },
  computed: {
    getChangedProps() {
      if (this.vChangedProps) {
        return true;
      } else {
        return false;
      }
    }
  },
  methods: {
    blurEvent() {
      if (this.selected || this.deselected) {
        if (this.deselected) {
          this.deselected = false;
        }
        if (this.selected) {
          this.selected = false;
        }
        if (this.getChangedProps) {
          this.$emit("propsChanged");
          return;
        }
        this.$emit("blurEvent");
      } else {
        return;
      }
    },
    focusEvent() {
      this.$emit("propsChanged");
    },
    inputEvent(e) {
      this.vSelectValue = e;
      if (e == null) {
        this.$emit("inputEvent", { id: null, name: null });
        this.selected = false;
        this.deselected = true;
      } else {
        this.$emit("inputEvent", { id: e.id, name: e.name });
        this.selected = true;
      }
    }
  }
};
</script>
