<template>
    <b-colxx>
        <b-card>
            <b-table
                ref="refUserListTable"
                sort-by="title" sort-desc.sync="false"
                selectable
                select-mode="single"
                selectedVariant="primary"
                :fields="fields"
                :items="reponseContentsData.data"
            >
                <template v-slot:cell(diskAllocation)="{ item, rowSelected, index }">
                    <div v-show="!rowSelected">{{item.diskAllocation.name}}</div>
                    <b-form-select
                        v-show="rowSelected"
                        :value="item.diskAllocation.id"
                        :options="diskOptions"
                        value-field="id"
                        text-field="name"
                        size="sm"
                        @change="onChangeDiskAllocation($event, item, index)"
                    >
                    </b-form-select>
                </template>
                <template v-slot:cell(menuType)="{ item, rowSelected, index }">
                    <div v-show="!rowSelected">{{item.menuType.name}}</div>
                    <b-form-select
                        v-show="rowSelected"
                        :value="item.menuType.id"
                        :options="menuTypeOptions"
                        value-field="id"
                        text-field="name"
                        size="sm"
                        @change="onChangeMenuType($event, item, index)"
                    >
                    </b-form-select>
                </template>
                <template v-slot:cell(actions)="{ item, index }">
                    <div v-if="equalOringData(item)">
                        <b-button
                        class="mb-1"
                        variant="primary default"
                        @click="onUpdate(item, index)"
                        >저장하기</b-button>
                    </div>
                </template>
            </b-table>
        </b-card>
    </b-colxx>
</template>
<script>
import mockData from '../../../../data/userList';

export default {
    data() {
        return {
            fields: [
                { key: 'rowNO', label: 'No', sortable: false, sortDirection: 'desc', tdClass: 'list-item-heading' },
                { key: 'name', label: '사용자', sortable: false, tdClass: 'text-muted' },
                { key: 'diskAllocation', label: '디스크 할당', sortable: false, tdClass: 'text-muted' },
                { key: 'menuType', label: '메뉴 유형', sortable: false, tdClass: 'text-muted' },
                { key: 'actions', label: '', thStyle: { width: '200px' } }
            ],
            diskOptions: [
                { id: '10', name: "10gb" },
                { id: '20', name: "20gb" },
                { id: '30', name: "30gb" },
                { id: '40', name: "40gb" },
                { id: '50', name: "50gb" },
                { id: '60', name: "60gb" },
                { id: '70', name: "70gb" },
            ],
            menuTypeOptions: [
                { id: 'ALL', name: '전체 메뉴' },
                { id: 'menu1', name: '메뉴 유형1' },
                { id: 'menu2', name: '메뉴 유형2' },
            ],
            reponseContentsData: {
                data: mockData.data,
            },
        }
    },
    methods: {
        onChangeDiskAllocation(value, item, index) {
            const findOptionItem = this.diskOptions.filter(item => item.id === value);
            const selectData = this.reponseContentsData.data[index];
            
            // 변경 횟수 증가 및 원데이터 저장
            if (selectData.originDiskAllocation === undefined) {
                selectData.originDiskAllocation = item.diskAllocation;
            }

            // 변경된 데이터와 원데이터 비교
            if (value === selectData.originDiskAllocation.id) {
                selectData.isDisAllocationChange = false;
            } else {
                selectData.isDisAllocationChange = true;
            }

            this.reponseContentsData.data[index].diskAllocation = findOptionItem[0];
        },
        onChangeMenuType(value, item, index) {
            const findOptionItem = this.menuTypeOptions.filter(item => item.id === value);
            const selectData = this.reponseContentsData.data[index];

            // 변경 횟수 증가 및 원데이터 저장
            if (selectData.originMenuType === undefined) {
                selectData.originMenuType = item.menuType;
            }

            // 변경된 데이터와 원데이터 비교
            if (value === selectData.originMenuType.id) {
                selectData.isMenuTypeChange = false;
            } else {
                selectData.isMenuTypeChange = true;
            }

            this.reponseContentsData.data[index].menuType = findOptionItem[0];
        },
        addActionState(index) {
            this.reponseContentsData.data[index].state = true;
        },
        equalOringData(item) {
            return  item.isDisAllocationChange || item.isMenuTypeChange;
        }, 
        onUpdate(item, index) {
            item.isDisAllocationChange = false;
            item.isMenuTypeChange = false;

            // click trigger
            const rowElem = this.$refs.refUserListTable.$el.querySelectorAll('tbody tr')[index];
            rowElem.click();
        }
    }
}
</script>
