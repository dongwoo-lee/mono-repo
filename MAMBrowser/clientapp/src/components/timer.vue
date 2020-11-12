<template>
    <div class="timer">
        <div class="time">
            {{title}}
        </div>
        <div>  
            <!-- <button class="btn-s btn-outline-primary" @click="startTimer">
                시작
            </button> -->
            <button class="btn-s" @click="resetTimer">
                로그인 연장
            </button>
        </div>
    </div>
</template>

<script>
import LoginPopupRefElement from '../lib/loginPopup/LoginPopupRefElement';
import { eventBus } from '../eventBus';
import jwt_decode from 'jwt-decode';

export default {
    props: {
        expires: {
            type: Number,
            default: 0
        },
    },
    data() {
        return {
            timer: null,
            totalTime: 0,
        }
    },
    created() {
        eventBus.$on('onResetTimer', () => {
            this.resetTimer();
        })
        this.startTimer();
    },
    methods: {
        getExpireTime() {
            return this.expires;
        },
		startTimer() {
            this.totalTime = this.getExpireTime();
            this.timer = setInterval(() => this.countdown(), 1000);
		},
		resetTimer() {
			clearInterval(this.timer);
            this.timer = null;
            this.startTimer();
        },
        clearTimer() {
            clearInterval(this.timer);
            this.timer = null;
        },
		countdown() {
            // const { state, commit, dispatch } = context;
            // const { accessToken } = state;
            // const { exp } = jwt_decode(accessToken);
            // const now = Date.now() / 1000;
            // let timeUntilRefresh = exp - now;
            // timeUntilRefresh -= (15 * 60);

			if(this.totalTime >= 1) {
				this.totalTime--;
			} else {
				this.totalTime = 0;
                this.clearTimer();
                LoginPopupRefElement.loginPopup.show();
			}
		},
		toHHmmss(time) {
			const myNum = parseInt(time, 10);
            let hours   = Math.floor(myNum / 3600);
			let minutes = Math.floor((myNum - (hours * 3600)) / 60);
			let seconds = myNum - (hours * 3600) - (minutes * 60);

            if (hours   < 10) {hours = "0" + hours;}
            if (minutes < 10) {minutes = "0" + minutes;}
            if (seconds < 10) {seconds = "0" + seconds;}

            return hours + ':' + minutes + ':' + seconds;
		}
	},
	computed: {
        title() {
            return this.toHHmmss(this.totalTime);
        }
	}
}
</script>