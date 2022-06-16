<template>
  <div
    id="app-container"
    :class="getMenuType"
    @dragstart="FileDragStart"
    @dragenter="dropzoneon"
    @dragend="FileDragEnd"
    @drop="dropzoneoff"
  >
    <topnav />
    <sidebar />
    <main>
      <div class="container-fluid">
        <slot></slot>
      </div>
    </main>
  </div>
</template>

<script>
import Sidebar from "../containers/navs/Sidebar";
import Topnav from "../containers/navs/Topnav";
import { mapGetters } from "vuex";

export default {
  components: {
    topnav: Topnav,
    sidebar: Sidebar,
  },
  data() {
    return {
      show: false,
    };
  },
  computed: {
    ...mapGetters("menu", ["getMenuType"]),
  },
  mounted() {
    setTimeout(() => {
      document.body.classList.add("default-transition");
    }, 100);
  },
  methods: {
    FileDragStart() {
      this.$emit("FileDragStart");
    },
    FileDragEnd() {
      this.$emit("FileDragEnd");
    },
    dropzoneoff() {
      this.$emit("FileDrop");
    },
    dropzoneon() {
      this.$emit("FileDragEnter");
    },
  },
};
</script>
