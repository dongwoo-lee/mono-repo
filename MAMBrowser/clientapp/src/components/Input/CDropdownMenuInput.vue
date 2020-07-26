<template>
    <vue-autosuggest
        class="autosuggest"
        :input-props="{id:'autosuggest__input', class:'form-control', placeholder: '선택해주세요.'}"
        :suggestions="filteredOptions"
        :render-suggestion="renderSuggestion"
        :get-suggestion-value="getSuggestionValue"
        :limit="6"
        @selected="onSelected"
        @input="onAutoSuggestInputChange"
        @blur="handlerEvent"
        @keyup.enter="handlerEvent"
    ></vue-autosuggest>
</template>
<script>
import { VueAutosuggest } from "vue-autosuggest";
export default {
    components: { VueAutosuggest },
    props: {
        // value: {
        //     type: Object,
        //     default: () => {
        //         return {
        //             editor: '',
        //             editorName: '',
        //         }
        //     },
        // },
        suggestions: Array,   // list data
        default: () => [],
    },
    data() {
        return {
            filteredOptions: [], // filtered
            oldData: {
                editor: '',
                editorName: '',
            },
        }
    },
    methods: {
        onAutoSuggestInputChange(text, oldText) {
            if (text == null) return;
            
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
        onSelected(item, originalInput) {
            console.info('item', item, originalInput);
            if (item) {
                this.emit({ editor: item.item.id, editorName: '' });
            }
        },
        handlerEvent(event) {
            const value = event.target.value;
            if (!this.isEmpty(value) || !this.isEmpty(this.oldData.editor) || !this.isEmpty(this.oldData.editorName)) {
                this.emit({editor: '', editorName: event.target.value })
            }
        },
        emit(data) {
            console.info('emit', data);
            this.oldData = data;
            this.$emit('selected', data);
        },
        isEmpty(data) {
            return data.length === 0 || !data.trim();
        }
    }
}
</script>