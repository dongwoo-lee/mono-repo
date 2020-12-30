<template>
    <div class="mydisk-copy-loading-overlay-box" v-if="isShow">
        <span class="content" style="word-break:break-all;">
            <div class="circles">
                <i></i>
                <i></i>
                <i></i>
            </div>
            MY디스크에 복사중입니다. <br>용량에 따라 다소 시간이 소요 될 수 있습니다.
        </span>
    </div>
</template>
<script>
import { eventBus } from '@/eventBus';
export default {
    data() {
        return {
            isShow: false,
        }
    },
    created(){
        eventBus.$on('common-loading-overlay-show', ()=>{
            this.show();
        });
        eventBus.$on('common-loading-overlay-hide', ()=>{
            this.hide();
        });
    },
    methods: {
        show() {
            this.isShow = true;
        },
        hide() {
            this.isShow = false;
        }
    }
}
</script>