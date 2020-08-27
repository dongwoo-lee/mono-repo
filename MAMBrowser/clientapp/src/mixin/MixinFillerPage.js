import mixinValidate from './MixinValidate';
import CDataTableScrollPaging from '../components/DataTable/CDataTableScrollPaging';

let mixinFillerPage = {
    mixins: [ mixinValidate ],
    components: {
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
            mediaOptions: [],                        // 매체 목록
            editorOptions: [],                       // 사용자(제작자) 목록
            spotOptions: [],                         // 주조 spot 분류 목록
            categoryOptions: [],                     // 필러 화면별 카테고리 분류
            // proOptions: [],                          // 필러(pr) 분류 목록
            // generalOptions: [],                      // 필러(일반) 분류 목록
            timetoneOptions: [],                     // 필러(시간) 분류 목록
            // etcOptions: [],                          // 필러(기타) 분류 목록
            reqStatusOptions: [],                    // 방송의뢰 상태 목록
            numRowsToBottom: 5,
        }
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
        // 정렬
        onSortable(sortKey) {
            this.searchItems.sortKey = sortKey;
            this.searchItems.sortValue = this.$fn.changeSortValue(this.searchItems.sortValue);
            this.getData();
        },
        getPageInfo() {
            const dataLength = this.responseData.data ? this.responseData.data.length : 0;
            return `${dataLength}개 / 전체 ${this.responseData.totalRowCount}개`
        },
        onChangeRowPerpage(value) {
            this.$refs.scrollPaging.init();
            this.searchItems.selectPage = 1;
            this.searchItems.rowPerPage = value;
            this.getData();
        },
        // 카테고리 API 요청
        requestCall(url, attr) {
            this.$http.get(url)
                .then(res => {
                    if (res.status === 200) {
                        this[attr] = res.data.resultObject.data;
                    } else {
                        this.$fn.notify('server-error', { message: '조회 에러' });
                    }
          });
        },
        // 매체목록 조회
        getMediaOptions() {
            this.requestCall('/api/Categories/media', 'mediaOptions');
        },
        // 제작자(사용자) 목록 조회
        getEditorOptions() {
            this.requestCall('/api/Categories/users', 'editorOptions');
        },
        // 주조 spot 분류 목록 조회
        getSpotOptions(value) {
          this.requestCall('/api/Categories/mcr/spot' + '?media=' + value, 'spotOptions');
        },
        // 방송의뢰 상태 목록 조회
        getReqStatusOptions() {
            this.requestCall('/api/Categories/req-status', 'reqStatusOptions');
        },
        // 필러(pr) 분류 목록 조회
        getProOptions() {
            this.requestCall('/api/Categories/filler/pro', 'categoryOptions');
        },
        // 필러(일반) 분류 목록 조회
        getGeneralOptions() {
            this.requestCall('/api/Categories/filler/general', 'categoryOptions');
        },
        // 필러(시간) 분류 목록 조회
        getTimetoneOptions() {
            this.requestCall('/api/Categories/filler/timetone', 'timetoneOptions');
        },
        // 필러(기타) 분류 목록 조회
        getEtcOptions() {
            this.requestCall('/api/Categories/filler/etc', 'categoryOptions');
        },
        // 제작자 선택
        onEditorSelected(data) {
            const { id, name } = data;
            this.searchItems.editor = id;
            this.searchItems.editorName = name;
        },
    }
}

export default mixinFillerPage;