<template>
<div>
    <b-overlay
        :show="show"
        :opacity="options.opacity"
        :spinner-small="options['spinner-small']"
        :class="options.class"
        no-wrap>
    </b-overlay>
</div>
</template>

<script>
export default {
    data() {
        return {
            show: false,
             options: {
                rounded: false,
                opacity: '0.0',
                'spinner-small': false,
                class: '',
            },
        }
    },
    methods: {
        enable() {
            // let itemOptions = Object.assign({}, this.options, options);
            this.show = true;
            // this.options = itemOptions;
        },
        disable() {
            this.show = false;
        }
    }
}
</script>