import mixinValidate from './MixinValidate';

let mixinCommon = {
    mixins: [ mixinValidate ],
    data() {
        return {
            responseData: {                          // 응답 결과
                data: null,
                rowPerPage: 16,
                selectPage: 1,
                totalRowCount: 0,
            },
            numRowsToBottom: 5,
            mediaOptions: [],                        // 매체 목록
            editorOptions: [],                       // 사용자(제작자) 목록
            sortItems: {},
            isLoadingClass: false,
            hasErrorClass: false,
            isTableLoading: false,
            isScrollLodaing: false,
        }
    },
    watch: {
        ['searchItems.start_dt'](v) {
          if (!this.$fn.checkGreaterStartDate(v, this.searchItems.end_dt)) {
            this.hasErrorClass = false;
          }
        },
        ['searchItems.end_dt'](v) {
          if (!this.$fn.checkGreaterStartDate(this.searchItems.start_dt, v)) {
            this.hasErrorClass = false;
          }
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
            console.info('onScrollPerPageonScrollPerPageonScrollPerPage');
            this.isScrollLodaing = true;
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
        // 페이지 정보
        getPageInfo() {
            const dataLength = this.responseData.data ? this.responseData.data.length : 0;
            return `${dataLength}개 / 전체 ${this.responseData.totalRowCount}개`
        },
        // 페이지당 로우수 변경
        onChangeRowPerpage(value) {
            this.$refs.scrollPaging.init();
            this.searchItems.selectPage = 1;
            this.searchItems.rowPerPage = value;
            this.getData();
        },
        // 정렬
        onSortable(sortKey) {
            if (!this.sortItems[sortKey]) {
                this.sortItems[sortKey] = 'ASC';
            } else {
                this.sortItems[sortKey] = this.$fn.changeSortValue(this.sortItems, sortKey);
            }

            this.$refs.scrollPaging.init();
            this.searchItems.selectPage = 1;
            this.searchItems.sortKey = sortKey;
            this.searchItems.sortValue = this.sortItems[sortKey];
            console.info('sorkKey', this.searchItems.sortKey, this.searchItems.sortValue);
            this.getData();
        },
        // 카테고리 API 요청
        requestCall(url, attr) {
            this.isLoadingClass = true;
            this.$http.get(url)
                .then(res => {
                    if (res.status === 200) {
                        this[attr] = res.data.resultObject.data;
                    } else {
                        this.$fn.notify('server-error', { message: '조회 에러' });
                    }
                    this.isLoadingClass = false;
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
        // 제작자 선택
        onEditorSelected(data) {
            const { id, name } = data;
            this.searchItems.editor = id;
            this.searchItems.editorName = name;
        },
        addScrollClass() {
            setTimeout(() => {
                this.$refs.scrollPaging.addClassScroll();  
            }, 0);
        }
    }
}

export default mixinCommon;