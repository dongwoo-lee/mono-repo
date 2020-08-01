import mixinValidate from './MixinValidate';
import CInputDatePicker from '../components/Input/CInputDatePicker';
import CDropdownMenuInput from '../components/Input/CDropdownMenuInput';

let mixinTablePage = {
    mixins: [ mixinValidate ],
    components: { CInputDatePicker, CDropdownMenuInput },
    data() {
        return {
            responseData: {
                data: null,
                rowPerPage: 16,
                selectPage: 1,
                totalRowCount: 0,
            },
            reponseContentsData: {
                data: null,
                rowPerPage: 16,
                selectPage: 1,
                totalRowCount: 0,
            },
            mediaOptions: [],                        // 매체 목록
            editorOptions: [],                       // 사용자(제작자) 목록
            pgmOptions: [],                          // 사용처 목록 목록
            cmOptions: [],                           // 광고 분류 목록
        }
    },
    watch: {
        ['searchItems.brd_dt'](v) {
            if (v) {
                this.getPgmOptions(v);
            }
        }
    },
    created() {
        // 매체목록 조회
        this.getMediaOptions();
        // 사용처 조회
        this.getPgmOptions(this.searchItems.brd_dt);
    },
    mounted() {
        this.$nextTick(() => {
          if (!this.type) {
              // 광고 목록 조회
            this.getCmOptions();
          }
          this.getData();
        });
      },
    methods: {
        // 검색
        onSearch() {
            this.getData();
        },
        // 메인 데이터 조회
        getData() {
            if (this.$v.$invalid) {
              this.$fn.notify('inputError', {});
              return;
            }
      
            const media = this.searchItems.media;
            const brd_dt = this.searchItems.brd_dt;

            let subUrl = `sb/${this.type}/${media}/${brd_dt}`;
            if (!this.type) {
                subUrl = `cm/${media}/${brd_dt}`;
            }
            ///api/Products/cm/{media}/{brd_dt}
            this.$http.get(`/api/Products/${subUrl}`, { params: this.searchItems })
              .then(res => {
                 this.setResponseData(res);
            });
          },
        // 서브 데이터 조회
        getDataContents(sbID) {
            if (sbID === undefined) return;
            const brd_dt = this.searchItems.brd_dt;
            this.$http.get(`/api/Products/sb/contents/${brd_dt}/${sbID}`)
              .then(res => {
                 this.setReponseContentsData(res, 'normal');
            });
        },
        // 메인 결과값 설정
        setResponseData(res) {
            if (res.status === 200) {
                const { data } = res.data.resultObject;
                this.responseData.data = data;
            } else {
                this.$fn.notify('server-error', { message: '조회 에러' });
            }
        },
        // 서브 결과값 설정
        setReponseContentsData(res) {
            if (res.status === 200) {
                const { data } = res.data.resultObject;
                this.reponseContentsData.data = data;
            } else {
                this.$fn.notify('server-error', { message: '조회 에러' });
            }
        },
        // 레코드 선택 이벤트
        rowSelected(v) {
            this.getDataContents(v[0].id);
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
        // 사용처 목록 조회
        getPgmOptions(brd_dt) {
            if (!brd_dt) return;
            this.requestCall('/api/Categories/pgmcodes/' + brd_dt, 'pgmOptions');
        },
        // 광고 분류 조회
        getCmOptions() {
            this.requestCall('/api/Categories/cm', 'cmOptions');
        },
        // 제작자 선택
        onEditorSelected(data) {
            const { id, name } = data;
            this.searchItems.editor = id;
            this.searchItems.editorName = name;
        },
        // 사용처 분류 선택
        onPgmSelected(data) {
        const { id, name } = data;
        this.searchItems.pgm = id;
        this.searchItems.pgmName = name;
        }
    }
}

export default mixinTablePage;