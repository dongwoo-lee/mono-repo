<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb :heading="heading"/>
      <div class="separator mb-5"></div>
    </b-colxx>
  </b-row>
  <b-row>
    <b-colxx xxs="12">
        <b-card class="mb-4">
          <b-form @submit.stop>
            <b-row>
              <!-- 방송일 -->
              <b-colxx sm="2">
                <b-form-group label="방송일" class="has-float-label c-zindex">
                    <c-input-date-picker v-model="$v.searchItems.brd_dt.$model"/>
                    <b-form-invalid-feedback :state="$v.searchItems.brd_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                </b-form-group>
              </b-colxx>
              <!-- 분류 -->
              <b-colxx sm="3">
                <b-form-group label="분류" class="has-float-label">
                  <b-form-select 
                    v-model="searchItems.cate"
                    :options="categoryOptions"
                    :disabled="categoryOptions.length === 0"
                    value-field="id"
                    text-field="name"
                  >
                    <template v-slot:first>
                      <b-form-select-option v-if="categoryOptions.length > 0" value="">선택해주세요.</b-form-select-option>
                      <b-form-select-option v-else value="">값이 존재하지 않습니다.</b-form-select-option>
                    </template>
                  </b-form-select>
                </b-form-group>
              </b-colxx>
              <!-- 제작자 -->
              <b-colxx sm="2">
                <b-form-group label="제작자" class="has-float-label">
                  <c-dropdown-menu-input :suggestions="editorOptions" @selected="onEditorSelected" />
                </b-form-group>
              </b-colxx>
              <!-- 소재명 -->
              <b-colxx sm="2">
                <b-form-group label="소재명" class="has-float-label">
                  <c-input-text v-model="searchItems.name"/>
                </b-form-group>
              </b-colxx>
              <b-button class="mb-1" variant="primary default" size="sm" @click="onSearch">검색</b-button>
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
            :per-page="responseData.rowPerPage"
            :num-rows-to-bottom="numRowsToBottom"
            @scrollPerPage="onScrollPerPage"
            @sortableclick="onSortable"
          />
        </b-card>
    </b-colxx>
  </b-row>
</div>
</template>

<script>
import MixinFillerPage from '../../../../mixin/MixinFillerPage';

export default {
    mixins: [ MixinFillerPage ],
    props: ['heading', 'type'],
    data() {
        return {
        searchItems: {
            brd_dt: '20200620',        // 방송일
            cate: '',                  // 분류
            editor: '',                // 사용자
            editorName: '',            // 사용자 이름
            name: '',                  // 소재명
            rowPerPage: 16,
            selectPage: 1,
            sortKey: '',
            sortValue: '',
        },
        fields: [
            {
            name: 'rowNO',
            title: 'No',
            titleClass: "center aligned text-center",
            dataClass: "center aligned text-center",
            width: '4%',
            },
            {
            name: "mediaName",
            title: "매체",
            titleClass: "center aligned text-center",
            dataClass: "center aligned text-center",
            width: '5%',
            },
            {
            name: "name",
            title: "소재명",
            titleClass: "center aligned text-center",
            dataClass: "center aligned text-center",
            },
            {
            name: "brdDT",
            title: "방송일유효일",
            titleClass: "center aligned text-center",
            dataClass: "center aligned text-center",
            width: '8%',
            callback: (v) => {
                return this.$fn.formatDate(v, 'yyyy-mm-dd')
            }
            },
            {
            name: "duration",
            title: "길이",
            titleClass: "center aligned text-center",
            dataClass: "center aligned text-center",
            width: '8%',
            },
            {
            name: "track",
            title: "트랙",
            titleClass: "center aligned text-center",
            dataClass: "center aligned text-center",
            width: '4%',
            },
            {
            name: "editorName",
            title: "편집자",
            titleClass: "center aligned text-center",
            dataClass: "center aligned text-center",
            width: '8%'
            },
            {
            name: "editDtm",
            title: "편집일자",
            titleClass: "center aligned text-center",
            dataClass: "center aligned text-center",
            width: '12%'
            },
            {
            name: "masteringDtm",
            title: "미스터링일자",
            titleClass: "center aligned text-center",
            dataClass: "center aligned text-center",
            width: '12%'
            },
            {
            name: "filePath",
            title: "파일경로",
            titleClass: "center aligned text-center",
            dataClass: "center aligned text-center word-break",
            width: '17%',
            callback: (v) => {
                return v.substring(1, v.length).replace(/\\/g, '/');
            }
            },
        ],
        }
    },
    created() {
        this.getOptions();
        this.getData();
    },
    methods: {
        getData() {
            if (this.$v.$invalid) {
                this.$fn.notify('inputError', {});
                return;
            }

            const brd_dt = this.searchItems.brd_dt;

            this.$http.get(`/api/Products/filler/${this.type}/${brd_dt}`, { params: this.searchItems })
                .then(res => {
                this.setResponseData(res, 'normal');
            });
        },
        onChangeMedia(value) {
            this.getProOptions(value);
        },
        getOptions() {
            this.getMediaOptions();
            this.getEditorOptions();

            // 필러(pr)
            if (this.type === 'pr') this.getProOptions();
            // 필러(일반)
            if (this.type === 'general') this.getGeneralOptions();
            // 필러(기타)
            if (this.type === 'etc') this.getEtcOptions();
        }
    }
}
</script>
