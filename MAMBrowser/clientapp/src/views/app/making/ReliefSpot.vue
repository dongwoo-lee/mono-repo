<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb heading="부조 SPOT"/>
      <div class="separator mb-5"></div>
    </b-colxx>
  </b-row>
  <b-row>
    <b-colxx xxs="12">
      <!-- 검색 -->
      <b-card class="mb-4">
        <b-form @submit.stop>
          <b-row>
            <!-- 매체 -->
            <b-colxx sm="2">
              <b-form-group label="매체">
                <b-form-select size="sm" v-model="searchItems.media" :options="mediaOptions"/>
              </b-form-group>
            </b-colxx>
            <!-- 시작일-종료일 -->
            <b-colxx sm="3">
              <b-form-group label="시작일-종료일">
                <div class="d-flex">
                  <c-input-date-picker class="w-50 mr-2" v-model="searchItems.start_dt"/>
                  <c-input-date-picker class="w-50 mr-2" v-model="searchItems.end_dt"/>
                </div>
              </b-form-group>
            </b-colxx>
            <!-- 제작자 -->
            <b-colxx sm="2">
              <b-form-group label="제작자">
                <c-dropdown-menu-input
                  :suggestions="editorOptions"
                  @selected="onDropdownInputSelected"
                />
                제작자값: {{ searchItems.editor }}
              </b-form-group>
            </b-colxx>
            <!-- 소재명 -->
            <b-colxx sm="2">
              <b-form-group label="소재명">
                <c-input-text v-model="searchItems.name"/>
              </b-form-group>
            </b-colxx>
            <b-button class="mb-1" variant="primary default" size="sm" @click="getData">검색</b-button>
          </b-row>
        </b-form>
      </b-card>

      <!-- 테이블 -->
      <b-card class="mb-4">
        <c-data-table-scroll-paging
          ref="scrollPaging"
          :table-height="'500px'"
          :fields="fields"
          :rows="responseData.data"
          :per-page="searchItems.rowPerPage"
          :num-rows-to-bottom="numRowsToBottom"
          :contextmenu="contextMenu"
          @scrollPerPage="onScrollPerPage"
          @contextMenuAction="onContextMenuAction"
          @sortableclick="onSortableClick"
        />
      </b-card>
    </b-colxx>
  </b-row>
  </div>
</template>

<script>
import MixinBasicPage from '../../mixin/MixinBasicPage';
import CInputText from '../../../components/Input/CInputText';
import CDropdownMenuInput from '../../../components/Input/CDropdownMenuInput';
import CInputDatePicker from '../../../components/Input/CInputDatePicker';
import CDataTableScrollPaging from '../../../components/DataTable/CDataTableScrollPaging';

export default {
  mixins: [ MixinBasicPage ],
  components: { 
    CInputText,
    CDropdownMenuInput,
    CInputDatePicker,
    CDataTableScrollPaging
  },
  data() {
    return {
      users: [],            // 사용자 목록
      searchItems: {
        start_dt: '',       // 시작일
        end_dt: '',         // 종료일
        editor: '',         // 제작자
        name: '',           // 소재명
        media: '',
        rowPerPage: 16,
        selectPage: 1,
        sortKey: '',
        sortValue: 'DESC',
      },
      mediaOptions: [
        { value: '', text: '선택하세요.' },
        { value: 'A', text: 'AM' },
        { value: 'F', text: 'FM' },
        { value: 'D', text: 'DMB' },
        { value: 'C', text: '공통' },
      ],
      editorOptions: [],
      fields: [
        {
          name: 'rowNO',
          title: 'No',
          titleClass: 'center aligned',
          dataClass: 'right aligned',
          width: '4%',
        },
        {
          name: "name",
          title: "소재명",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "categoryName",
          title: "분류",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "duration",
          title: "길이",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "track",
          title: "트랙",
          titleClass: "center aligned",
          dataClass: "center aligned",
          width: '4%',
        },
        {
          name: "brdDT",
          title: "방송일",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "pgmName",
          title: "사용처명",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "masteringDtm",
          title: "마스터링 일시",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "filePath",
          title: "파일경로",
          titleClass: "",
          dataClass: "list-item-heading",
        },
      ]
    }
  },
  created() {
    this.getEditor();
  },
  methods: {
    getData() {
      this.searchItems.selectPage = 1;
      this.$http.get(`/api/Products/spot/scr`, { params: this.searchItems })
        .then(res => {
            if (res.status === 200) {
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
            } else {
                this.$fn.notify('server-error', { message: '조회 에러' });
            }
      });
    },
    getEditor() {
      this.$http.get('/api/users')
        .then(res => {
            if (res.status === 200) {
                this.editorOptions = res.data.resultObject.data;
            } else {
                this.$fn.notify('server-error', { message: '조회 에러' });
            }
      });
    },
    onDropdownInputSelected(data) {
      const { editor, editorName } = data;
      this.searchItems.editor = editor ? editor : editorName;
    },
  }
}
</script>