<template>
    <b-form-input
        ref="input"
        :class="[classString]"
        :value="value"
        :placeholder="placeholder"
        @input="onInput"
        :pattern="pattern"
        @keydown.enter.prevent
    >
    </b-form-input>
</template>

<script>
export default {
    props: {
        value: {
            type: String,
            default: '',
        },
        type: {
            type: String,
            default: 'text',
        },
        classString: {
            type: String,
            default: '',
        },
        placeholder: {
            type: String,
            default: '',
        },
        pattern: {
            type: String,
            default: '',
        },
    },
    methods: {
        onInput(input) {
            this.$emit('input', input);
        },
    }
}
</script>