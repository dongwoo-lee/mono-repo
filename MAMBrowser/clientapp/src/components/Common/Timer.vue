<template>
  <div class="timer">
    <div class="time">
      {{ title }}
    </div>
    <div>
      <!-- <button class="btn-s btn-outline-primary" @click="startTimer">
                시작
            </button> -->
      <button class="btn-s" @click="resetTimer">로그인 연장</button>
    </div>
  </div>
</template>

<script>
import LoginPopupRefElement from "@/components/Popup/Login/LoginPopupRefElement";
import { mapActions, mapGetters, mapMutations } from "vuex";
import { eventBus } from "@/eventBus";
import jwt_decode from "jwt-decode";

export default {
  props: {
    expires: {
      type: Number,
      default: 0,
    },
    timerProccessing: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      timer: null,
      totalTime: 0,
    };
  },
  computed: {
    title() {
      return this.toHHmmss(this.totalTime);
    },
  },
  watch: {
    timerProccessing: {
      handler(value) {
        if (value) {
          this.startTimer();
        }
      },
      immediate: true,
    },
  },
  created() {
    eventBus.$on("onResetTimer", () => {
      this.resetTimer();
    });
    //로그아웃까지 남은 시간 가져오기
    eventBus.$on("getTimer", () => {
      this.SET_TIMER(this.totalTime);
    });
  },
  methods: {
    ...mapMutations("user", ["SET_TIMER"]),
    getExpireTime() {
      return this.expires;
    },
    startTimer() {
      this.clearTimer();
      this.totalTime = this.getExpireTime();
      //this.totalTime = 30;
      this.timer = setInterval(() => this.countdown(), 1000);
    },
    resetTimer() {
      this.$emit("resetTimer");
      clearInterval(this.timer);
      this.timer = null;
    },
    clearTimer() {
      clearInterval(this.timer);
      this.timer = null;
    },
    countdown() {
      if (this.totalTime >= 1) {
        this.totalTime--;
      } else {
        this.totalTime = 0;
        this.clearTimer();
        LoginPopupRefElement.loginPopup.show();
      }
    },
    toHHmmss(time) {
      const myNum = parseInt(time, 10);
      let hours = Math.floor(myNum / 3600);
      let minutes = Math.floor((myNum - hours * 3600) / 60);
      let seconds = myNum - hours * 3600 - minutes * 60;

      if (hours < 10) {
        hours = "0" + hours;
      }
      if (minutes < 10) {
        minutes = "0" + minutes;
      }
      if (seconds < 10) {
        seconds = "0" + seconds;
      }

      return hours + ":" + minutes + ":" + seconds;
    },
  },
};
</script>