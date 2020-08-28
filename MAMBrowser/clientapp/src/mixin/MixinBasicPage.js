import MixinCommon from './MixinCommon';

let mixinBasicPage = {
    mixins: [ MixinCommon ],
    data() {
        return {
            selectedItems: [],                       // 선택된 로우 데이터
            selectedIds: null,
            pgmOptions: [],                          // 사용처 목록 조회
            publicOptions: [],                       // 공유소재 분류 목록
            rePortOptions: [],                       // 취재물 분류 목록
            mediaPrimaryOptions: [],                      // (구)프로소재, 공유소재 매체목록 
            contextMenu: [
                { name: 'edit', text: '편집' },
                { name: 'throw', text: '휴지통으로 보내기' },
                { name: 'download', text: '다운로드' },
            ]
        }
    },
    methods: {
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
        onSelectedIds(ids) {
            this.selectedIds = ids;
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
        // (구)프로소재, 공유소재 매체 목록 조회
        getMediaPrimaryOptions() {
            this.requestCall('/api/Categories/public-codes/primary', 'mediaPrimaryOptions');
        },
        // (구)프로 목록 조회
        getProOptions() {
            this.requestCall('/api/Categories/pro', 'proOptions');
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
        // 공유 소재 분류 선택
        onPublicSelected(data) {
            const { id, name } = data;
            this.searchItems.cate = id;
            this.searchItems.cateName = name;
        }
    }
}

export default mixinBasicPage;