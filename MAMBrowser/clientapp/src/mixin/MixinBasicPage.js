import MixinCommon from './MixinCommon';

let mixinBasicPage = {
    mixins: [ MixinCommon ],
    data() {
        return {
            selectedItems: [],                       // 선택된 로우 데이터
            selectedIds: null,
            pgmOptions: [],                          // 사용처 목록 조회
            publicCodesOptions: [],                       // 공유소재 분류 목록
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
        getPublicCodesOptions(primaryCode = '') {
            this.requestCall(`/api/Categories/public-codes/primary/${primaryCode}`, 'publicCodesOptions')
            .then(data => {
                if(data.resultObject && data.resultCode === 0) {
                    if (data.resultObject.data.length > 0) {
                        this.searchItems.cate =  data.resultObject.data[0].id;
                    }
                }
            });
        },
        // 사용처 분류 선택
        onPgmSelected(data) {
            const { id, name } = data;
            this.searchItems.pgm = id;
            this.searchItems.pgmName = name;
        },
        getInnerHtmlSelectdFileNames(title) {
            return `파일명:<text style="color:red;">${title}</text><br><br>`;
        },
        getInnerHtmlSelectdFileNamesFromMulti(selectedIds, rows) {
            const selectedNames = [];
            selectedIds.forEach(id => {
                rows.some(data => {
                  if (data.seq === id) {
                    selectedNames.push(data.title);
                    return true;
                  }
                  return false;
                });
            });
      
            return `파일명:<text style="color:red;">${selectedNames.join(',')}</text><br>파일 개수:${selectedNames.length}개<br><br>`;
        }
    }
}

export default mixinBasicPage;