import CInputText from '../components/Input/CInputText';
import CDropdownMenuInput from '../components/Input/CDropdownMenuInput';
import CInputDatePicker from '../components/Input/CInputDatePicker';
import CDataTable from '../components/DataTable/CDataTable';
import CDataTableScrollPaging from '../components/DataTable/CDataTableScrollPaging';
import mixinValidate from './MixinValidate';

let mixinBasicPage = {
    mixins: [ mixinValidate ],
    components: {
        CInputText,
        CDropdownMenuInput,
        CInputDatePicker,
        CDataTable,
        CDataTableScrollPaging
    },
    data() {
        return {
            responseData: {
                data: null,
                rowPerPage: 16,
                selectPage: 1,
                totalRowCount: 0,
            },
            mediaOptions: [],                        // 매체 목록
            editorOptions: [],                       // 사용자(제작자) 목록
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
                default: break;
            }
        },
        // 정렬
        onSortable(sortKey) {
            this.searchItems.sortKey = sortKey;
            this.searchItems.sortValue = this.$fn.changeSortValue(this.searchItems.sortValue);
            this.getData();
        },
        // 매체목록 조회
        getMediaOptions() {
            this.$http.get('/api/Categories/media')
              .then(res => {
                  if (res.status === 200) {
                      this.mediaOptions = res.data.resultObject.data;
                  } else {
                      this.$fn.notify('server-error', { message: '조회 에러' });
                  }
            });
        },
        // (구)프로 목록 조회
        getProOptions() {
            this.$http.get('/api/Categories/pro')
              .then(res => {
                  if (res.status === 200) {
                      this.proOptions = res.data.resultObject.data;
                  } else {
                      this.$fn.notify('server-error', { message: '조회 에러' });
                  }
            });
        },
        // 제작자(사용자) 목록 조회
        getEditorOptions() {
            this.$http.get('/api/Categories/users')
              .then(res => {
                  if (res.status === 200) {
                      this.editorOptions = res.data.resultObject.data;
                  } else {
                      this.$fn.notify('server-error', { message: '조회 에러' });
                  }
            });
        },
        // 제작자 선택
        onEditorSelected(data) {
            const { id, name } = data;
            this.searchItems.editor = id;
            this.searchItems.editorName = name;
        },
        // 취재물 분류 목록 조회
        getReportOptions() {
            this.$http.get('/api/Categories/report')
              .then(res => {
                  if (res.status === 200) {
                      this.rePortOptions = res.data.resultObject.data;
                  } else {
                      this.$fn.notify('server-error', { message: '조회 에러' });
                  }
            });
        },
    }
}

export default mixinBasicPage;