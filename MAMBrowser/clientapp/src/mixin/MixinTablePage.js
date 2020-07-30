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
        }
    },
    created() {
        this.$nextTick(() => {
            this.getData();
        });
    },
    methods: {
        onSearch() {
            this.getData();
        },
        // 결과값 설정
        setResponseData(res) {
            if (res.status === 200) {
                const { data } = res.data.resultObject;
                this.responseData.data = data;
            } else {
                this.$fn.notify('server-error', { message: '조회 에러' });
            }
        },
        setReponseContentsData(res) {
            if (res.status === 200) {
                const { data } = res.data.resultObject;
                this.reponseContentsData.data = data;
            } else {
                this.$fn.notify('server-error', { message: '조회 에러' });
            }
        },
        rowSelected(v) {
            this.getDataContents(v[0].id);
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
    }
}

export default mixinTablePage;