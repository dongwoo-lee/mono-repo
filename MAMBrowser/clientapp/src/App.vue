<template>
  <div class="h-100">
    <router-view />
  </div>
</template>

<script>
function checkBrowser() {
  const agt = navigator.userAgent.toLowerCase();
    const possibleBrowser = ['chrome', 'safari'];
    const isPossible = possibleBrowser.some(browser => {
        return agt.indexOf(browser) != -1;
    })

    if (!isPossible) {
      alert('본 시스템은 마이크로소프트 Edge, 구글 Chrome, 애플 Safari 브라우저에 최적화 되어 있습니다.');
    }
}

window.addEventListener("unhandledrejection", function (event) {
  console.debug("WARNING: Unhandled promise rejection. Reason: " + event.reason, event);
});

checkBrowser();

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
  }
};
</script>
