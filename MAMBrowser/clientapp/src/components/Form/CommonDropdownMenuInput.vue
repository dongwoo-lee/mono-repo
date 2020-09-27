<template>
    <vue-autosuggest
        ref="refAutosuggest"
        class="autosuggest"
        :class="classString"
        :input-props="{id:'autosuggest__input', class:'form-control', placeholder: '선택해주세요.'}"
        :suggestions="filteredOptions"
        @click="onClick"
        @selected="onSelected"
        @input="onInput"
        @blur="onBlur"
    >
        <template slot-scope="{suggestion}">
            <span class="my-suggestion-item">{{suggestion.item.name}}</span>
        </template>
        <template slot="after-input">
            <div class="loding" :class="{ 'isLoading' : isLoadingClass}">
                <b-spinner small></b-spinner>
            </div>
        </template>
    </vue-autosuggest>
</template>
<script>
import { VueAutosuggest } from "vue-autosuggest";

export default {
    components: { VueAutosuggest },
    props: {
        suggestions: {
            type: Array,   // list data
            default: () => [],
        },
        classString: {
            type: String,
            default: '',
        },
        isLoadingClass: {
            type: Boolean,
            default: false,
        }
    },
    watch: {
        isLoadingClass(v) {
            if (v) {
                this.resetInputData();
            }
        }
    },
    data() {
        return {
            filteredOptions: [], // filtered
            isSelected: false,
        }
    },
    methods: {
        onClick(e) {
            const interalValue = this.$refs.refAutosuggest.internalValue;
            if (interalValue !== null && this.filteredOptions.length > 0) return;
            this.getfilteredData();
        },
        onInput(text, oldText) {
            if (text == null) return;
            this.getfilteredData(text, oldText);
        },
        getfilteredData(text = '', oldText = '') {
            const filteredData = this.suggestions.filter(option => {
                return option.name.toLowerCase().indexOf(text.toLowerCase()) > -1;
            });

            this.filteredOptions = [
                {
                    data: filteredData
                }
            ];
        },
        onSelected(item) {
            const interalValue = this.$refs.refAutosuggest.internalValue;
            if (item) {
                this.$emit('selected', { id: item.item.id, name: item.item.name });
                this.getfilteredData(interalValue);
                return;
            }

            const suggestions = this.$refs.refAutosuggest.suggestions;
            const { data } = suggestions[0];
            if (data.length === 1) {
                const obj = data[0];
                if (obj.name === interalValue) {
                    this.isSelected = true;
                    this.$emit('selected', { id: obj.id, name: obj.name });
                    this.getfilteredData(interalValue);
                }
            }
        },
        onBlur(e) {
            if (this.isSelected) {
                this.isSelected = false;
                return;
            }

            if (e.relatedTarget && e.relatedTarget.localName === 'div') { return; }
            this.resetInputData();
        },
        resetInputData() {
            this.$refs.refAutosuggest.internalValue = null;
            this.$refs.refAutosuggest.searchInputOriginal = null;
            this.$emit('selected', { id: '', name: '' });
        }
    }
}
</script>
<style lang="scss" scoped>
.autosuggest {
    .loding{
        display:none;
    }
    .isLoading {
        display: flex;
        justify-content: center;
        position: absolute;
        width:100%;
        bottom: 7px;
    }
}
</style>