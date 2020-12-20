<template>
    <b-form-group>
        <div style="display:flex; justify-content:space-between;">
            <div>
                <label :for="labelfor" class="d-block">{{label}}</label>
            </div>
            <div :style="{ color: wordLength === max ? 'red': ''}">({{wordLength}}/{{max}})</div>
        </div>
        <b-form-input
            ref="input"
            :id="labelfor"
            :label="label"
            :class="[classString]"
            :value="value"
            :placeholder="placeholder"
            :maxlength="maxLength"
            @input="onInput"
            @keydown.enter.prevent
        >
        </b-form-input>
        <b-form-invalid-feedback :state="state">필수 입력입니다.</b-form-invalid-feedback>
    </b-form-group>
</template>

<script>
import { INPUT_MAX_LENGTH } from '@/constants/config';
export default {
    props: {
        value: {
            type: String,
            default: '',
        },
        label: {
            type: String,
            default: '',
        },
        labelfor: {
            type: String,
            default: 'input-label',
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
        maxLength: {
            type: Number,
            default: INPUT_MAX_LENGTH,
        },
        state: {
            type: Boolean,
            default: false,
        }
    },
    data() {
        return {
            wordLength: 0,
            max: 99999999,
            INPUT_MAX_LENGTH
        }
    },
    watch: {
        value: {
            handler(v) {
                this.wordLength = v.length;
            },
            immediate:true,
        }
    },
    mounted() {
        this.max = this.maxLength;
    },
    methods: {
        onInput(input) {
            if (input.length <= this.max) {
                this.$emit('input', input);
            }
        },
    }
}
</script>