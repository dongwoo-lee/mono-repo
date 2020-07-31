<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb heading="주조SPOT"/>
      <div class="separator mb-5"></div>
    </b-colxx>
  </b-row>
  <b-row>
    <b-colxx xxs="12">
      <b-card class="mb-4">
        <b-form @submit.stop>
          <b-row>
            <!-- 매체 -->
            <b-colxx sm="2">
              <b-form-group label="매체" class="has-float-label">
                <b-form-select 
                  v-model="searchItems.media"
                  :options="mediaOptions"
                  value-field="id"
                  text-field="name"
                  @change="onChangeMedia"
                />
              </b-form-group>
            </b-colxx>
            <!-- 분류 -->
            <b-colxx sm="3">
              <b-form-group label="분류" class="has-float-label">
                <b-form-select 
                  v-model="searchItems.cate"
                  :options="spotOptions"
                  :disabled="spotOptions.length === 0"
                  value-field="id"
                  text-field="name" 
                >
                  <template v-slot:first>
                    <b-form-select-option v-if="spotOptions.length > 0" value="">선택해주세요.</b-form-select-option>
                    <b-form-select-option v-else value="">값이 존재하지 않습니다.</b-form-select-option>
                  </template>
                </b-form-select>
              </b-form-group>
            </b-colxx>
            <!-- 시작일 -->
            <b-colxx sm="2">
              <b-form-group label="시작일" class="has-float-label c-zindex">
                  <c-input-date-picker v-model="$v.searchItems.start_dt.$model"/>
                  <b-form-invalid-feedback :state="$v.searchItems.start_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
              </b-form-group>
            </b-colxx>
            <!-- 종료일 -->
            <b-colxx sm="2">
              <b-form-group label="종료일" class="has-float-label c-zindex">
                  <c-input-date-picker v-model="$v.searchItems.end_dt.$model"/>
                  <b-form-invalid-feedback :state="$v.searchItems.end_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
              </b-form-group>
            </b-colxx>
            <!-- 상태 -->
            <b-colxx sm="2">
              <b-form-group label="매체" class="has-float-label">
                <b-form-select 
                  v-model="searchItems.status"
                  :options="filterStatusOptions"
                />
              </b-form-group>
            </b-colxx>
            <!-- 제작자 -->
            <b-colxx sm="2">
              <b-form-group label="제작자" class="has-float-label">
                <c-dropdown-menu-input :suggestions="editorOptions" @selected="onEditorSelected" />
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
import MixinFilterPage from '../../../mixin/MixinFilterPage';

export default {
  mixins: [ MixinFilterPage ],
  data() {
    return {
      filterStatusOptions: [       // 상태
        { value: '', text: '선택하세요.' },
        { value: 'pr', text: 'PR' },
        { value: 'program', text: '프로그램' }
      ],
      searchItems: {
        media: 'A',                // 매체
        cate: '',                  // 분류
        start_dt: '20200101',      // 시작일
        end_dt: '20200706',        // 종료일
        status: '',                // 상태
        editor: '',                // 사용자
        editorName: '',            // 사용자 이름
        rowPerPage: 16,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      fields: [
        {
          name: 'rowNO',
          title: 'No',
          titleClass: 'center aligned',
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
          titleClass: 'center aligned',
          dataClass: "center aligned",
        },
        {
          name: "brdDT",
          title: "방송일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
          callback: (v) => {
            return this.$fn.formatDate(v, 'yyyy-mm-dd')
          }
        },
         {
          name: "status",
          title: "상태",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '6%',
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
          title: "제작자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%'
        },
        {
          name: "editDtm",
          title: "편집일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '12%'
        },
        {
          name: "reqCompleteDtm",
          title: "방송의뢰일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '14%',
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
    // 매체목록 조회
    this.getMediaOptions();
    // 사용자 목록 조회
    this.getEditorOptions();
    // 주조 spot 분류 목록 조회
    this.getSpotOptions(this.searchItems.media);
    this.getData();
  },
  methods: {
    getData() {
      if (this.$v.$invalid) {
        this.$fn.notify('inputError', {});
        return;
      }

      const media = this.searchItems.media;

      this.$http.get(`/api/Products/spot/mcr/${media}`, { params: this.searchItems })
        .then(res => {
           this.setResponseData(res, 'normal');
      });
    },
    onChangeMedia(value) {
        this.getSpotOptions(value);
    }
  },
}
</script>