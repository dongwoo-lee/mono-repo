<template>
    <vue-autosuggest
        class="autosuggest"
        :input-props="{id:'autosuggest__input', class:'form-control', placeholder: '선택해주세요.'}"
        :suggestions="filteredOptions"
        :render-suggestion="renderSuggestion"
        :get-suggestion-value="getSuggestionValue"
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
        onSelected(item) {
            // 클릭 이벤트 외에 filtered값이 존재하는 경우 enter keydown 이벤트도 들어옵니다.
            if (event.type === 'keydown') { return };
            this.emit(event, item);
        },
        handlerEvent(event) {
            // 마우스 오버, 엔터
            this.emit(event, event.target.value)
        },
        emit(event, data) {
            let emitData = data;
            const { type } = event;
            if (type === 'mouseup') {
                // 마우스 클릭
                emitData = { editor: data.item.id, editorName: '' };
            } else {
                // 마우스 오버, 엔터
                const targetValue = event.target.value;
                const filteredData = this.suggestions.filter(option => {
                    return option.name.toLowerCase() === targetValue.toLowerCase();
                });

                if (filteredData.length > 0) {
                    emitData = { editor: filteredData[0].id, editorName: '' };    
                } else {
                    emitData = { editor: '', editorName: targetValue };
                }
            }

            this.$emit('selected', emitData);
        },
    }
}
</script>
<style scoped>

</style>