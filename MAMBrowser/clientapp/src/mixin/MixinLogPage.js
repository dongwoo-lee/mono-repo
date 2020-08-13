import mixinValidate from './MixinValidate';
import CInputText from '../components/Input/CInputText';
import CDropdownMenuInput from '../components/Input/CDropdownMenuInput';
import CInputDatePicker from '../components/Input/CInputDatePicker';
import CDataTable from '../components/DataTable/CDataTable';
import CDataTableScrollPaging from '../components/DataTable/CDataTableScrollPaging';

let mixinBasicPage = {
    mixins: [ mixinValidate ],
    components: {
        CInputText,
        CDropdownMenuInput,
        CInputDatePicker,
        CDataTable,
        CDataTableScrollPaging,
    },
    data() {
        return {
            responseData: {                          // 응답 결과
                data: null,
                rowPerPage: 16,
                selectPage: 1,
                totalRowCount: 0,
            },
            numRowsToBottom: 5,
            contextMenu: [
                { name: 'edit', text: '편집' },
                { name: 'throw', text: '휴지통으로 보내기' },
                { name: 'download', text: '다운로드' },
            ]
        }
    },
    created() {
        this.$nextTick(() => {
            this.getData();
        });
    },
    methods: {
        // 검색
        onSearch() {
            this.searchItems.selectPage = 1;
            this.getData();
        },
        // 스크롤 페이징
        onScrollPerPage() {
            this.searchItems.selectPage++;
            this.getData();
        },
        // 결과값 설정
        setResponseData(res, type = '') {
            if (res.status === 200) {
                if (type === 'nomal') {
                    const { data } = res.data.resultObject;
                    this.responseData.data = row;
                } else {
                    const { data, rowPerPage, selectPage, totalRowCount } = res.data.resultObject;
                    if (selectPage > 1) {
                        data.forEach(row => {
                            this.responseData.data.push(row);
                        })
                    } else {
                        this.responseData.data = data;
                        this.responseData.rowPerPage = rowPerPage;
                        this.responseData.selectPage = selectPage;
                        this.responseData.totalRowCount = totalRowCount;
                    }
                }
            } else {
                this.$fn.notify('server-error', { message: '조회 에러' });
            }
        },
        // 우측메뉴 액션
        onContextMenuAction(v) {
            switch(v) {
                case 'edit': console.log(v); break;
                case 'throw': console.log(v); break;
                case 'download':  console.log(v); break;
                case 'storage':  console.log(v); break;
                case 'restore':  console.log(v); break;
                case 'delete':  console.log(v); break;
                default: break;
            }
        },
        // 정렬
        onSortable(sortKey) {
            this.searchItems.sortKey = sortKey;
            this.searchItems.sortValue = this.$fn.changeSortValue(this.searchItems.sortValue);
            this.getData();
        },
    }
}

export default mixinBasicPage;
