<template>
    <div>
        <b-form-input
            ref="input"
            :value="value"
            :placeholder="placeholder"
            @input="onInput"
            :pattern="pattern"
        >
        </b-form-input>
    </div>
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
        }
    }
}
</script>