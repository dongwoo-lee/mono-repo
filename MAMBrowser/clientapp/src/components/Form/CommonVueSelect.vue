<template>
  <v-select
    class="v-select"
    :options="suggestions"
    label="name"
    @input="inputEvent"
    @search:blur="blurEvent"
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
    }
  },
  data() {
    return {
      selected: false,
      deselected: false
    };
  },
  methods: {
    blurEvent() {
      if (this.selected || this.deselected) {
        if (this.deselected) {
          this.deselected = false;
        }
        this.$emit("blurEvent");
      } else {
        return;
      }
    },
    inputEvent(e) {
      if (e == null) {
        if (this.selected) {
          this.$emit("inputEvent", { id: null, name: null });
          this.selected = false;
          this.deselected = true;
        } else return;
      } else {
        this.$emit("inputEvent", { id: e.id, name: e.name });
        this.selected = true;
      }
    }
  }
};
</script>
