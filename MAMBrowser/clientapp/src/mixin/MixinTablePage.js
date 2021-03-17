import MixinCommon from './MixinCommon';
import { mapActions } from 'vuex';
import { PREVIEW_CODE, AUTHORITY, AUTHORITY_ADMIN } from "@/constants/config";

let mixinTablePage = apiType => ({
    mixins: [ MixinCommon ],
    data() {
        return {
            PREVIEW_CODE: PREVIEW_CODE,
            IS_ADMIN: sessionStorage.getItem(AUTHORITY) === AUTHORITY_ADMIN,
            selectedItem : {},      //CM, SB에서 선택된 아이템만 다운로드 보이게끔하기 위해.
            selectBrdDate: '',
            selectName: '',
            responseData: {
                data: [],
                totalRowCount: 0,
            },
            reponseContentsData: {
                data: [],
                totalRowCount: 0,
            },
            mediaOptions: [],                        // 매체 목록
            editorOptions: [],                       // 사용자(제작자) 목록
            pgmOptions: [],                          // 사용처 목록 목록
            cmOptions: [],                           // 광고 분류 목록
            isTableLoading: false,
            isSubTableLoading: false,
        }
    },
    watch: {
        ['searchItems.brd_dt'](v) {
            if (v) {
                this.getPgmOptions(v, this.searchItems.media);
            }
        },
        ['searchItems.media'](v) {
            if (v) {
                this.getPgmOptions(this.searchItems.brd_dt, v);
            }
        }
    },
    created() {
        this.$nextTick(() => {
            // 매체목록 조회
            this.getMediaOptions();
            // 사용처 조회
            this.getPgmOptions(this.searchItems.brd_dt, this.searchItems.media);
            // 목록 조회
            this.getData();
        });
    },
    mounted() {
        this.$nextTick(() => {
          if (!this.screenName) {
              // 광고 목록 조회
            this.getCmOptions();
          }
          this.getData();
        });
      },
    methods: {
        ...mapActions('file', ['download']),
        // 검색
        onSearch() {
            this.reponseContentsData.data = [];
            this.selectBrdDate = this.searchItems.brd_dt;
            this.selectName = '';
            this.getData();
        },
        // 메인 데이터 조회
        getData() {
            if (!this.$v.searchItems.brd_dt.$invalid) {
              this.$fn.notify('inputError', {});
              return;
            }
            
            this.isTableLoading = true;
            const media = this.searchItems.media;
            const brd_dt = this.searchItems.brd_dt;

            let subUrl = `sb/${this.screenName}/${media}/${brd_dt}`;
            if (!this.screenName) {
                subUrl = `cm/${media}/${brd_dt}`;
            }
            ///api/products/cm/{media}/{brd_dt}
            this.$http.get(`/api/products/${subUrl}`, { params: this.searchItems })
              .then(res => {
                 this.setResponseData(res);
                 this.isTableLoading = false;
            });
          },
        // 서브 데이터 조회
        getDataContents(sbID, brdDT) {
            if (sbID === undefined) return;
            this.isSubTableLoading = true;
            this.$http.get(`/api/products/${apiType}/contents/${brdDT}/${sbID}`)
              .then(res => {
                 this.setReponseContentsData(res, 'normal');
                 this.isSubTableLoading = false;
            });
        },
        // 메인 결과값 설정
        setResponseData(res) {
            if (res.status === 200) {
                const { data, totalRowCount } = res.data.resultObject;
                this.responseData.data = data;
                this.responseData.totalRowCount = totalRowCount;
            } else {
                this.$fn.notify('server-error', { message: '조회 에러' });
            }
        },
        // 서브 결과값 설정
        setReponseContentsData(res) {
            if (res.status === 200) {
                const { data, totalRowCount } = res.data.resultObject;
                this.reponseContentsData.data = data;
                this.reponseContentsData.totalRowCount = totalRowCount;
            } else {
                this.$fn.notify('server-error', { message: '조회 에러' });
            }
        },
        // 레코드 선택 이벤트
        rowSelected(items) {
            if (items.length === 0) return;
            this.selectedItem = items[0];
            const { id, name, brdDT } = items[0];
            this.selectName = name;
            this.getDataContents(id, brdDT);
        },
        // 광고 분류 조회
        getCmOptions() {
            this.requestCall('/api/Categories/cm', 'cmOptions');
        },
        display(value) {
            return this.behaviorList.some(data => data.id === value && data.visible === 'Y');
        },
    }
})

export default mixinTablePage;