<template>
  <span>
    <h1 v-if="heading && heading.length > 0" :title="tooltip">{{ heading }}</h1>
    <b-nav
      class="pt-0 breadcrumb-container d-none d-sm-block d-lg-inline-block"
    >
      <b-breadcrumb v-if="!noNav" :items="items" />
    </b-nav>
  </span>
</template>

<script>
export default {
  props: ["heading", "noNav", "tooltip"],
  data() {
    return {
      items: [],
    };
  },
  methods: {
    getUrl(path, sub, index) {
      var pathToGo = "/" + path.split(sub)[0] + sub;
      if (pathToGo === "/app") {
        pathToGo = "/";
      }
      return pathToGo;
    },
  },
  mounted() {
    let path = this.$route.path.substr(1);
    let rawPaths = path.split("/");

    for (var pName in this.$route.params) {
      if (rawPaths.includes(this.$route.params[pName])) {
        rawPaths = rawPaths.filter((x) => x !== this.$route.params[pName]);
      }
    }
    rawPaths.map((sub, index) => {
      if (index === 0) {
        // home
        this.items.push({
          text: this.$t("menu." + sub),
          to: this.$fn.getFirstAccessiblePage(),
        });
      } else if (index === 1) {
        // 1dept
        this.items.push({
          text: this.$t("menu." + sub),
        });
      } else {
        this.items.push({
          // 2dept
          text: this.$t("menu." + sub),
          to: this.getUrl(path, sub, index),
        });
      }
    });
  },
};
</script>
