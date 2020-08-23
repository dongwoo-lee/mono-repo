import mixinValidate from './MixinValidate';
import CInputText from '../components/Input/CInputText';
import CDropdownMenuInput from '../components/Input/CDropdownMenuInput';
import CDataTable from '../components/DataTable/CDataTable';
import CDataTableScrollPaging from '../components/DataTable/CDataTableScrollPaging';

let mixinBasicPage = {
    mixins: [ mixinValidate ],
    components: {
        CInputText,
        CDropdownMenuInput,
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
            selectedItems: [],                       // 선택된 로우 데이터
            selectedIds: null,
            mediaOptions: [],                        // 매체 목록
            editorOptions: [],                       // 사용자(제작자) 목록
            pgmOptions: [],                          // 사용처 목록 조회
            publicOptions: [],                       // 공유소재 분류 목록
            rePortOptions: [],                       // 취재물 분류 목록
            mediaPrimaryOptions: [],                      // (구)프로소재, 공유소재 매체목록 
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
            if (res.status === 200 && !res.data.errorMsg) {
                if (!res.data.resultObject) return;
                if (type === 'nomal') {
                    const { data } = data.resultObject;
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
                this.$fn.notify('server-error', { message: '조회 에러: ' + res.data.errorMsg });
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
        onRefresh() {
            this.getData();
        },
        getPageInfo() {
            const dataLength = this.responseData.data ? this.responseData.data.length : 0;
            return `${dataLength}개 / 전체 ${this.responseData.totalRowCount}개`
        },
        // getSelectedCount() {
        //     if (!this.selectedIds || this.selectedIds.length === 0) return '';
        //     return `${ this.selectedIds.length }개 선택되었습니다. |`;
        // },
        onSelectedIds(ids) {
            this.selectedIds = ids;
        },
        onChangeRowPerpage(value) {
            this.$refs.scrollPaging.init();
            this.searchItems.selectPage = 1;
            this.searchItems.rowPerPage = value;
            this.getData();
        },
        // 다운로드
        downLoad() {
            this.$http.get('/api/products/workspace/private/files/{fileid}')
                .then(res => {
                    const { status, data } = res;
                    if (status === 200 && !data.errorMsg) {
                        // this.$fn.notify('success', { message: '다운로드 요청을 하였습니다.' });
                    } else {
                        this.$fn.notify('server-error', { message: '조회 에러:' + data.errorMsg });
                    }
                })
        },
        // 정렬
        onSortable(sortKey) {
            this.searchItems.sortKey = sortKey;
            this.searchItems.sortValue = this.$fn.changeSortValue(this.searchItems.sortValue);
            this.getData();
        },
        // 카테고리 API 요청
        requestCall(url, attr) {
            this.$http.get(url)
                .then(res => {
                    const { status, data } = res;
                    if (status === 200 && !data.errorMsg) {
                        this[attr] = data.resultObject.data;
                    } else {
                        this.$fn.notify('server-error', { message: '조회 에러: ' + data.errorMsg });
                    }
            });
        },
        // 매체목록 조회
        getMediaOptions() {
            this.requestCall('/api/Categories/media', 'mediaOptions');
        },
        // (구)프로소재, 공유소재 매체 목록 조회
        getMediaPrimaryOptions() {
            this.requestCall('/api/Categories/public-codes/primary', 'mediaPrimaryOptions');
        },
        // (구)프로 목록 조회
        getProOptions() {
            this.requestCall('/api/Categories/pro', 'proOptions');
        },
        // 제작자(사용자) 목록 조회
        getEditorOptions() {
            this.requestCall('/api/Categories/users', 'editorOptions');
        },
        // 취재물 분류 목록 조회
        getReportOptions() {
            this.requestCall('/api/Categories/report', 'rePortOptions');
        },
        // 사용처 목록 조회
        getPgmOptions(brd_dt) {
            if (!brd_dt) return;
            this.requestCall('/api/Categories/pgmcodes/' + brd_dt, 'pgmOptions');
        },
        // 공유 소재 분류 목록 조회
        getPublicOptions(primaryCode = '') {
            this.$http.get(`/api/Categories/public-codes/primary/${primaryCode}`)
              .then(res => {
                  const { status, data } = res;
                  if (status === 200 && !data.errorMsg) {
                      this.publicOptions = data.resultObject.data;
                  } else {
                    this.$fn.notify('server-error', { message: '조회 에러: ' + data.errorMsg });
                  }
            });
        },
        // 사용처 분류 선택
        onPgmSelected(data) {
            const { id, name } = data;
            this.searchItems.pgm = id;
            this.searchItems.pgmName = name;
        },
        // 제작자 선택
        onEditorSelected(data) {
            const { id, name } = data;
            this.searchItems.editor = id;
            this.searchItems.editorName = name;
        },
        // 공유 소재 분류 선택
        onPublicSelected(data) {
            const { id, name } = data;
            this.searchItems.cate = id;
            this.searchItems.cateName = name;
        }
    }
}

export default mixinBasicPage;