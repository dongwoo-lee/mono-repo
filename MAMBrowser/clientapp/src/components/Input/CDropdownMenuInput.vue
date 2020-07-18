<template>
    <vue-autosuggest
        class="autosuggest"
        :input-props="{id:'autosuggest__input', class:'form-control', placeholder: '선택해주세요.'}"
        :suggestions="filteredOptions"
        :render-suggestion="renderSuggestion"
        :get-suggestion-value="getSuggestionValue"
        :limit="6"
        @selected="onAutosuggestSelected"
        @input="onAutoSuggestInputChange"
        @keyup.enter="handlerEnter"
    ></vue-autosuggest>
</template>
<script>
import { VueAutosuggest } from "vue-autosuggest";
export default {
    components: { VueAutosuggest },
    props: {
        suggestions: Array,   // list data
        default: () => [],
    },
    data() {
        return {
            filteredOptions: [], // filtered
        }
    },
    methods: {
        onAutoSuggestInputChange(text, oldText) {
            if (text === null) {
                return;
            }
            const filteredData = this.suggestions.filter(option => {
                return option.name.toLowerCase().indexOf(text.toLowerCase()) > -1;
            });

            this.filteredOptions = [
                {
                    data: filteredData
                }
            ];
        },
        onAutosuggestSelected(item) {
            this.$emit('selected', item);
        },
        renderSuggestion(suggestion) {
            const character = suggestion.item;
            return character.name; /* 기본 템플릿 슬롯보다 우선 */
        },
        getSuggestionValue(suggestion) {
            return suggestion.item.name;
        },
        handlerEnter(event) {
            this.$emit('enter', event.target.value);
        }
    }
}
</script>