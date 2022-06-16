<template>
  <div class="h-100" style="font-family: 'MBC 새로움 M'">
    <router-view />
  </div>
</template>

<script>
window.addEventListener("unhandledrejection", function (event) {
  console.debug(
    "WARNING: Unhandled promise rejection. Reason: " + event.reason,
    event
  );
});

import { getDirection } from "./utils";
export default {
  beforeMount() {
    const direction = getDirection();
    if (direction.isRtl) {
      document.body.classList.add("rtl");
      document.dir = "rtl";
      document.body.classList.remove("ltr");
    } else {
      document.body.classList.add("ltr");
      document.dir = "ltr";
      document.body.classList.remove("rtl");
    }
  },
};
</script>
