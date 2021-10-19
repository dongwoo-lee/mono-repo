import mixinValidate from './MixinValidate';
import { mapGetters, mapActions } from 'vuex';
import { eventBus } from '../eventBus';

let mixinCommon = {
    mixins: [mixinValidate],
    data() {
        return {
            streamingUrl: '/api/products/streaming',
            waveformUrl: '/api/products/waveform',
            tempDownloadUrl: '/api/products/temp-download',

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
            soundItem: {},
            showPlayerPopup: false,
            isLoadingClass: false,
            hasErrorClass: false,
            isTableLoading: false,
            isScrollLodaing: false,
            copyToMySpacePopup: false,
        }
    },
    computed: {
        ...mapGetters('user', ['behaviorList'])
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
        // 토큰 만료시 재로그인할때, 로직 태움
        eventBus.$on('onLoadData', (viewName) => {
            this.getData();
        });
    },
    methods: {
        ...mapActions('file', ['downloadWorkspace', 'downloadProduct'
            , 'downloadMusic', 'downloadDl30', 'downloadConcatenate']),
        ...mapActions('user', ['getSummaryUser']),
        // 검색
        onSearch() {
            this.searchItems.selectPage = 1;
            // scroll paging table인 경우
            if (this.$refs.scrollPaging) {
                this.$refs.scrollPaging.init();
            }
            this.initSelectedIds();
            this.getData();
        },
        // 스크롤 페이징
        onScrollPerPage() {
            this.isScrollLodaing = true;
            this.searchItems.selectPage++;
            this.getData();
        },
        // 결과값 설정
        setResponseData(res, type = '') {
            if (res.status === 200 && !res.data.errorMsg) {
                if (!res.data.resultObject) return;
                if (type === 'normal') {
                    const { data, totalRowCount } = res.data.resultObject;
                    this.responseData.data = data;
                    this.responseData.totalRowCount = totalRowCount;
                } else {
                    this.$refs.scrollPaging.displayLastPage(false);
                    const { data, rowPerPage, selectPage, totalRowCount } = res.data.resultObject;
                    if (selectPage > 1) {
                        const resData = [];
                        data.forEach(row => {
                            resData.push(row);
                        })
                        const totalArr = [...this.responseData.data, ...resData];
                        this.responseData.data = totalArr;
                        this.$refs.scrollPaging.loading();
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
        getTotalRowCount() {
            return `전체 ${this.responseData.totalRowCount}개`;
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
            console.debug('sorkKey', this.searchItems.sortKey, this.searchItems.sortValue);
            this.getData();
        },
        // 카테고리 API 요청
        requestCall(url, attr) {
            this.isLoadingClass = true;
            return this.$http.get(url)
                .then(res => {
                    if (res.status === 200) {
                        this[attr] = res.data.resultObject.data;
                    } else {
                        this.$fn.notify('server-error', { message: '조회 에러' });
                    }
                    this.isLoadingClass = false;
                    return res.data;
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
        // 사용처 목록 조회
        getPgmOptions(brd_dt, media) {
            if (!brd_dt) return;
            if (!media) {
                media = '';
            }
            this.requestCall(`/api/Categories/pgmcodes?brd_dt=${brd_dt}&media=${media}`, 'pgmOptions');
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
        addScrollClass() {
            setTimeout(() => {
                if (this.$refs.scrollPaging) {
                    this.$refs.scrollPaging.addClassScroll();
                }
            }, 0);
        },
        initSelectedIds() {
            if (!this.$refs.scrollPaging) return;
            this.$refs.scrollPaging.initSelectedIds();
        },
        onPreview(item) {
            console.log(item);
            this.soundItem = item;
            this.showPlayerPopup = true;
        },
        onClosePlayer() {
            this.soundItem = {};
            this.showPlayerPopup = false;
        },
        onDownloadProduct(item, downloadName) {
            this.downloadProduct({ item: item, downloadName: downloadName });
        },
        onDownloadMusic(item, downloadName) {
            this.downloadMusic({ item: item, downloadName: downloadName });
        },
        onDownloadDl30(item) {
            this.downloadDl30(item);
        },
        onDownloadConcatenate(item) {
            this.downloadConcatenate(item);
        },
        onMyDiskCopyFromPublic() {
            var item = this.$refs.refCopyToMySpacePopup.getRowData();
            var metaData = this.$refs.refCopyToMySpacePopup.getMetaData();
            this.onMyDisCopy(`/api/products/workspace/public/public-to-myspace/${item.seq}`, item.title, metaData);
        },
        onMyDiskCopyFromMusic(item, metaData) {
            var item = this.$refs.refCopyToMySpacePopup.getRowData();
            var metaData = this.$refs.refCopyToMySpacePopup.getMetaData();
            this.onMyDisCopy(`/api/musicsystem/music-to-myspace?token=${item.fileToken}`, item.name, metaData);
        },
        onMyDiskCopyFromProduct(item, metaData) {
            var item = this.$refs.refCopyToMySpacePopup.getRowData();
            var metaData = this.$refs.refCopyToMySpacePopup.getMetaData();
            this.onMyDisCopy(`/api/products/product-to-myspace?token=${item.fileToken}`, item.name, metaData);
        },
        onMyDiskCopyFromDl30(item, metaData) {
            var item = this.$refs.refCopyToMySpacePopup.getRowData();
            var metaData = this.$refs.refCopyToMySpacePopup.getMetaData();
            this.onMyDisCopy(`/api/products/dl30-to-myspace/${item.seq}`, item.recName, metaData);
        },
        onMyDisCopy(url, oldName, metaData) {
            // eventBus.$emit('common-loading-overlay-show');
            this.$fn.notify('primary', { message: `'${metaData.title}' MY디스크에 복사가 요청되었습니다. 용량에 따라 많은 시간이 소요 될 수 있습니다.` });
            this.$http.post(url, metaData, { timeout: 3600000 }).then(res => {
                if (res.data && res.data.resultCode === 0) {
                    this.$fn.notify('primary', { message: `MY공간으로 '${metaData.title}' 가 등록 되었습니다.` });
                    this.getSummaryUser();
                }
            })
                .finally(() => {
                    // eventBus.$emit('common-loading-overlay-hide');
                })
        },
        onCopyToMySpacePopup(rowData) {
            this.$refs.refCopyToMySpacePopup.setData(rowData);
            this.copyToMySpacePopup = true;
        },

    }
}

export default mixinCommon;