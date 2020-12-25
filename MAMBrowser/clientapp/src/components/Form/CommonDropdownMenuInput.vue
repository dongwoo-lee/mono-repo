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
        @keyup.enter="onEnter"
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
        onInput(text, oldText) {
            if (!text) {
                 this.resetInputData();
            }
            this.getfilteredData(text, oldText);
        },
        getfilteredData(text = '', oldText = '') {
            const filteredData = this.suggestions.filter(option => {
                return option.name.toLowerCase().indexOf(text.toLowerCase()) > -1;
            });

            if (filteredData.length === 1) {
                this.$emit('selected', { id: filteredData[0].id, name: filteredData[0].name });
            }

            this.filteredOptions = [
                {
                    data: filteredData
                }
            ];
        },
        onSelected(item) {
            const interalValue = this.$refs.refAutosuggest.internalValue;
            if (item) {
                // 리스트 클릭 & 방향키 아래로 엔터시 여기로 진입
                this.$emit('selected', { id: item.item.id, name: item.item.name });
                this.getfilteredData(interalValue);
                return;
            } else {
                this.$refs.refAutosuggest.$el.children[0].blur();
            }
        },
        onBlur(e) {
            const targetValue = e.target.value;
            this.validData(targetValue);
        },
        onEnter(e) {
            e.target.blur();
        },
        validData(inputValue) {
            const suggestions = this.$refs.refAutosuggest.suggestions;
            const { data } = suggestions[0];

            if (data.length > 0) {
                const index = data.findIndex(d => d.name === inputValue);
                if (index !== -1) {
                    this.$emit('selected', { id: data[index].id, name: data[index].name });
                    this.getfilteredData(inputValue);
                } else {
                    this.resetInputData();    
                }
            } else {
                this.resetInputData();
            }
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