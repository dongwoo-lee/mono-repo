<template>
    <b-form-input
        ref="input"
        :class="[classString]"
        :value="value"
        :placeholder="placeholder"
        @input="onInput"
        :pattern="pattern"
        @keydown.enter.prevent
        :maxlength="maxLength"
    >
    </b-form-input>
</template>

<script>
import { INPUT_MAX_LENGTH } from '@/constants/config';
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
        maxLength: {
            type: Number,
            default: INPUT_MAX_LENGTH,
        }
    },
    data() {
        return {
            INPUT_MAX_LENGTH
        }
    },
    methods: {
        onInput(input) {
            if (input.length >= this.maxLength) {
                var msg = '최대 입력 글자 수는 ' + this.maxLength + '입니다.'
                this.$notify('info', msg);
            } else {
                this.$emit('input', input);
            }
        },
    }
}
</script>