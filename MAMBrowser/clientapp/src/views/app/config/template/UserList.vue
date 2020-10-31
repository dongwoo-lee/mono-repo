<template>
    <b-table
        ref="refUserListTable"
        sort-by="title" sort-desc.sync="false"
        selectable
        show-empty
        empty-text="데이터가 없습니다."
        select-mode="single"
        selectedVariant="primary"
        :busy="isLoading"
        :fields="fields"
        :items="userList"
    >
        <template v-slot:cell(no)="{ index }">
            <div class="text-nowrap">{{ index + 1 }}</div>
        </template>
        <template v-slot:cell(diskUsed)="{ item }">
            <div class="text-nowrap">{{ $fn.formatBytes(item.diskUsed, 1) }}</div>
        </template>
        <template v-slot:cell(diskMax)="{ item, rowSelected, index }">
            <template v-if="isDisplayMyDiskMenu">
                <span v-show="!rowSelected">{{item.diskMax}}GB</span>
                <b-form-select
                    v-show="rowSelected"
                    :value="item.diskMax"
                    :options="diskScopeOptions"
                    value-field="value"
                    text-field="name"
                    size="sm"
                    @change="onChangeDiskMax($event, item, index)"
                >
                </b-form-select>
            </template>
             <template v-else>
                <span>디스크할당없음</span>
            </template>
        </template>
        <template v-slot:cell(menuGrpID)="{ item, rowSelected, index }">
            <div v-show="!rowSelected">{{item.menuGrpName}}</div>
            <b-form-select 
                :value="item.menuGrpID"
                v-show="rowSelected"
                @change="onChangeMenuGrp($event, item, index)">
                <option
                    v-for="(option, idx) in menuGrpOptions"
                    :key="idx"
                    :value="option.code"
                    :title="option.description">
                    {{option.name}}
                </option>
            </b-form-select>
        </template>
        <template v-slot:cell(actions)="{ item, index }">
            <div v-if="equalOringData(item)">
                <b-button variant="primary default" class="common-btn" @click="onUpdate(item, index)">저장하기</b-button>
            </div>
        </template>
        <template v-slot:table-busy>
            <div class="text-center text-primary my-2">
                <b-spinner class="align-middle"></b-spinner>
                <strong>Loading...</strong>
            </div>
        </template>
    </b-table>
</template>
<script>
import { mapGetters } from 'vuex';
export default {
    data() {
        return {
            userList: [],
            isLoading: false,
            fields: [
                { key: 'no', label: 'No', sortable: false, sortDirection: 'desc', tdClass: 'list-item-heading' },
                { key: 'name', label: '사용자', sortable: false, tdClass: 'text-center' },
                { key: 'diskUsed', label: '사용 용량', sortable: false, tdClass: 'text-muted' },
                { key: 'diskMax', label: '디스크 할당', sortable: false, tdClass: 'text-muted' },
                { key: 'menuGrpID', label: '메뉴 유형', sortable: false, tdClass: 'text-muted' },
                { key: 'actions', label: '', thStyle: { width: '200px' } }
            ],
            menuGrpOptions: [],
            diskScopeOptions: [],
        }
    },
    computed: {
        ...mapGetters('user', ['isDisplayMyDiskMenu']),
    },
    created() {
        this.getUserData();
        this.getMenuGrpOptions();
        this.getDiskScopeOptions();
    },
    methods: {
        getUserData() {
            this.isLoading = true;
            this.$http.get('/api/users')
                .then(res => {
                    if (res.status === 200) {
                        this.userList = res.data.resultObject.data;
                    } else {
                        this.$fn.notify('server-error', { message: '조회 에러' });
                    }
                    this.isLoading = false;
            });
        },
        getMenuGrpOptions() {
            this.$http.get('/api/menugrp')
              .then(res => {
                  if (res.status === 200) {
                      this.menuGrpOptions = res.data.resultObject.data;
                        this.menuGrpOptions.forEach(menu => {
                            this.setMenuGrpDescription(menu);
                        })
                  } else {
                      this.$fn.notify('server-error', { message: '조회 에러' });
                  }
            });
        },
        setMenuGrpDescription(menu) {
            this.$http.get('/api/menugrp/' + menu.code)
              .then(res => {
                  if (res.status === 200) {
                      const list = res.data.resultObject.data;
                      const values = [];
                      list.forEach(data => {
                          values.push(data.name);
                      });
                      menu.description = values.join('\n')
                  }
            });
        },
        getDiskScopeOptions() {
            this.$http.get('/api/disk-scope')
              .then(res => {
                  if (res.status === 200) {
                      this.diskScopeOptions = res.data.resultObject.data;
                  } else {
                      this.$fn.notify('server-error', { message: '조회 에러' });
                  }
            });
        },
        onChangeDiskMax(value, item, index) {
            const findOptionItem = this.diskMaxOptions.filter(option => option.value === value);
            const selectData = this.userList[index];
            
            // 변경 횟수 증가 및 원데이터 저장
            if (selectData.originDiskMax === undefined) {
                selectData.originDiskMax = item.diskMax;
            }

            // 변경된 데이터와 원데이터 비교
            if (value === selectData.originDiskMax) {
                selectData.isChangeDiskMax = false;
            } else {
                selectData.isChangeDiskMax = true;
            }

            selectData.diskMax = findOptionItem[0].value;
        },
        onChangeMenuGrp(value, item, index) {
            const findOptionItem = this.menuGrpOptions.filter(option => option.code === value);
            const selectData = this.userList[index];

            // 변경 횟수 증가 및 원데이터 저장
            if (selectData.originMenuGrpID === undefined) {
                selectData.originMenuGrpID = item.menuGrpID;
            }

            // 변경된 데이터와 원데이터 비교
            if (value === selectData.originMenuGrpID) {
                selectData.isChangeMenuGrpID = false;
            } else {
                selectData.isChangeMenuGrpID = true;
            }

            selectData.menuGrpID = findOptionItem[0].code;
            selectData.menuGrpName = findOptionItem[0].name;
        },
        equalOringData(item) {
            return  item.isChangeDiskMax || item.isChangeMenuGrpID;
        }, 
        onUpdate(item, index) {
            item.isChangeDiskMax = false;
            item.isChangeMenuGrpID = false;

            const params = [];
            params.push(item);

            this.$http.put('/api/users', params)
              .then(res => {
                  const { status, data } = res;
                  if (status === 200 && !data.errorMsg) {
                      this.$fn.notify('success', { message: `${item.name} 사용자 정보 업데이트 성공` });
                  } else {
                      this.$fn.notify('server-error', { message: '조회 에러:' + data.errorMsg });
                  }
            });
            console.info('this..equalOringData', this.equalOringData(item))
    
            // click trigger
            const rowElem = this.$refs.refUserListTable.$el.querySelectorAll('tbody tr')[index];
            rowElem.click();
        }
    }
}
</script>
