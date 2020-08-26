<template>
    <vue-autosuggest
        class="autosuggest"
        :input-props="{id:'autosuggest__input', class:'form-control', placeholder: '선택해주세요.'}"
        :suggestions="filteredOptions"
        :render-suggestion="renderSuggestion"
        :get-suggestion-value="getSuggestionValue"
        @click="onClick"
        @selected="onSelected"
        @input="onAutoSuggestInputChange"
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
        onClick() {
            if (this.filteredOptions.length > 0) return;
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
        renderSuggestion(suggestion) {
            const character = suggestion.item;
            return character.name;
        },
        getSuggestionValue(suggestion) {
            return suggestion.item.name;
        },
        onSelected(data) {
            this.$emit('selected', { id: data.item.id, name: data.item.name });
        },
    }
}
</script>