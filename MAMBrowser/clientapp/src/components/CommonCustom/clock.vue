<template>
    <div :class="className">{{clock}}</div>
</template>

<script>
export default {
    props: ['className'],
    data() {
        return {
            clock: '',
        }
    },
    mounted() {
        this.initClock();
        setInterval(this.initClock, 1000);
    },
    methods: {
        initClock() {
            var date = new Date();
            const year = date.getFullYear();
            var month = date.getMonth();
            var clockDate = date.getDate();
            var day = date.getDay();
            var week = ['일', '월', '화', '수', '목', '금', '토'];
            var hours = date.getHours();
            var minutes = date.getMinutes();
            var seconds = date.getSeconds();
            this.clock = `${year}-${month+1}-${clockDate}(${week[day]})` +
            ` ${hours < 10 ? `0${hours}` : hours}:${minutes < 10 ? `0${minutes }` : minutes }:${seconds < 10 ? `0${seconds }`  : seconds }`;
        }
    }
}
</script>