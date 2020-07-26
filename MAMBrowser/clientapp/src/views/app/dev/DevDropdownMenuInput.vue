<template>
     <b-card class="mb-4" title="드롭다운메뉴+입력박스+순간검색">
         <c-dropdown-menu-input
            :suggestions="suggestions"
            @selected="onSelected"
            @enter="onEnter"
        >
        </c-dropdown-menu-input>
        <div>Selected: {{ localDropdownSelectedVal }}</div>
     </b-card>
</template>

<script>
import CDropdownMenuInput from '../../../components/Input/CDropdownMenuInput';
export default {
    components: { CDropdownMenuInput },
    data() {
        return {
            localDropdownSelectedVal: '',
            localDropdownEnteredVal: '',
            suggestions: [
                {
                    id: 1,
                    name: "Marble Cake"
                },
                {
                    id: 2,
                    name: "Fruitcake"
                },
                {
                    id: 3,
                    name: "Chocolate Cake"
                },
                {
                    id: 4,
                    name: "Fat Rascal"
                },
                {
                    id: 5,
                    name: "Financier"
                },
                {
                    id: 6,
                    name: "Genoise"
                },
                {
                    id: 7,
                    name: "Gingerbread"
                },
                {
                    id: 8,
                    name: "Goose Breast"
                },
                {
                    id: 9,
                    name: "Parkin"
                },
                {
                    id: 10,
                    name: "Petit Gâteau"
                },
                {
                    id: 11,
                    name: "Salzburger Nockerl"
                },
                {
                    id: 12,
                    name: "Soufflé"
                },
                {
                    id: 13,
                    name: "Streuselkuchen"
                },
                {
                    id: 14,
                    name: "Tea Loaf"
                },
                {
                    id: 15,
                    name: "Napoleonshat"
                },
                {
                    id: 16,
                    name: "Merveilleux"
                },
                {
                    id: 17,
                    name: "Magdalena"
                },
                {
                    id: 18,
                    name: "Cremeschnitte"
                },
                {
                    id: 19,
                    name: "Cheesecake"
                },
                {
                    id: 20,
                    name: "Bebinca"
                }
            ],
        }
    },
    methods: {
        onSelected(data) {
            this.localDropdownSelectedVal = data;
        },
        onEnter(data) {
            this.localDropdownEnteredVal = data;
        }
    }
}
</script>
