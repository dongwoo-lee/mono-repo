<template>
    <vue-autosuggest
        ref="refAutosuggest"
        class="autosuggest"
        :class="classString"
        :input-props="{id:'autosuggest__input', class:'form-control', placeholder: '선택해주세요.'}"
        :suggestions="filteredOptions"
        @click="onClick"
        @selected="onSelected"
        @input="onAutoSuggestInputChange"
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
        }
    },
    methods: {
        onClick(e) {
            const interalValue = this.$refs.refAutosuggest.internalValue;
            if (interalValue !== null && this.filteredOptions.length > 0) return;
            this.getfilteredData();
        },
        onAutoSuggestInputChange(text, oldText) {
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
        // renderSuggestion(suggestion) {
        //     const character = suggestion.item;
        //     return character.name;
        // },
        // getSuggestionValue(suggestion) {
        //     console.info('getSuggestionValue')
        //     return suggestion.item.name;
        // },
        onSelected(data) {
            const interalValue = this.$refs.refAutosuggest.internalValue;
            this.$emit('selected', { id: data.item.id, name: data.item.name });
            this.getfilteredData(interalValue);
        },
        onBlur(e) {
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