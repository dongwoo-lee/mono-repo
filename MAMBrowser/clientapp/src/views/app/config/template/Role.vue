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
        :items="rolesList"
    >
        <template v-slot:cell(no)="{ index }">
            <div class="text-nowrap">{{ index + 1 }}</div>
        </template>
        <template v-slot:cell(authorCode)="{ item, rowSelected, index }">
            <div v-show="!rowSelected">{{getAuthorName(item.authorCode)}}</div>
            <b-form-select
                class=""
                v-show="rowSelected"
                :value="item.authorCode"
                :options="authorityOptions"
                value-field="code"
                text-field="name"
                size="sm"
                @change="onChangeAuthority($event, item, index)"
            >
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
export default {
    data() {
        return {
            rolesList: [],
            isLoading: false,
            fields: [
                { key: 'no', label: 'No', sortable: false, sortDirection: 'desc', tdClass: 'list-item-heading' },
                { key: 'id', label: 'Id', sortable: false, tdClass: 'text-muted' },
                { key: 'name', label: '역할', sortable: false, tdClass: 'text-muted' },
                { key: 'authorCode', label: '권한', sortable: false, tdClass: 'text-muted' },
                { key: 'actions', label: '', thStyle: { width: '200px' } }
            ],
            authorityOptions: [],       // 권환 목록
        }
    },
    created() {
        this.getRolseData();
        this.getAuthorityOptions();
    },
    methods: {
        getRolseData() {
            this.isLoading = true;
            this.$http.get('/api/roles')
                .then(res => {
                    if (res.status === 200) {
                        this.rolesList = res.data.resultObject.data;
                    } else {
                        this.$fn.notify('server-error', { message: '조회 에러' });
                    }
                    this.isLoading = false;
            });
        },
        getAuthorityOptions() {
            this.$http.get('/api/authority-list')
              .then(res => {
                  if (res.status === 200) {
                      this.authorityOptions = res.data.resultObject.data;
                  } else {
                      this.$fn.notify('server-error', { message: '조회 에러' });
                  }
            });
        },
        onChangeAuthority(value, item, index) {
            const findOptionItem = this.authorityOptions.filter(option => option.code === value);
            const selectData = this.rolesList[index];

            // 변경 횟수 증가 및 원데이터 저장
            if (selectData.originAuthorCode === undefined) {
                selectData.originAuthorCode = item.authorCode;
            }

            // 변경된 데이터와 원데이터 비교
            if (value === selectData.originAuthorCode) {
                selectData.isChangeAuthorCode = false;
            } else {
                selectData.isChangeAuthorCode = true;
            }

            selectData.authorCode = findOptionItem[0].code;
        },
        equalOringData(item) {
            return  item.isChangeAuthorCode;
        },
        onUpdate(item, index) {
            item.isChangeAuthorCode = false;

            const params = [
                {
                    ID: item.id,
                    AuthorCode: item.authorCode,
                }
            ]

            this.$http.put('/api/roles', params)
              .then(res => {
                  if (res.status === 200) {
                      this.$fn.notify('success', { message: `${item.name} 역할 정보 업데이트 성공` });
                  } else {
                      this.$fn.notify('server-error', { message: '조회 에러' });
                  }
            });

            // click trigger
            const rowElem = this.$refs.refUserListTable.$el.querySelectorAll('tbody tr')[index];
            rowElem.click();
        },
        getAuthorName(code) {
            const findAuthOption = this.authorityOptions.filter(option => option.code === code);
            if (findAuthOption.length > 0) {
                return findAuthOption[0].name;
            }
            
            return code;
        }
    }
}
</script>
