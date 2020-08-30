<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb :heading="heading" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      @changeRowPerpage="onChangeRowPerpage"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
      <!-- 방송일 -->
        <b-form-group label="방송일"
          class="has-float-label"
          :class="{ 'hasError': (hasErrorClass || $v.searchItems.brd_dt.$error) }">
          <common-date-picker v-model="$v.searchItems.brd_dt.$model" />
        </b-form-group>
        <!-- 분류 -->
        <b-form-group label="분류" class="has-float-label">
          <b-form-select
            class="width-220"
            v-model="searchItems.cate"
            :options="timetoneOptions"
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
        <!-- 제작자 -->
        <b-form-group label="제작자" class="has-float-label">
          <common-dropdown-menu-input :suggestions="editorOptions" @selected="onEditorSelected" />
        </b-form-group>
        <!-- 소재명 -->
        <b-form-group label="소재명" class="has-float-label">
          <common-input-text v-model="searchItems.name"/>
        </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch">검색</b-button>
        </b-form-group>
      </template>
      <!-- 테이블 페이지 -->
      <template slot="form-table-page-area">
        {{ getPageInfo() }}
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <common-data-table-scroll-paging
          ref="scrollPaging"
          :table-height="'500px'"
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          :is-actions-slot="true"
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
        >
        </common-data-table-scroll-paging>
      </template>
    </common-form>
  </div>
</template>

<script>
import MixinFillerPage from '../../../../mixin/MixinFillerPage';

export default {
    mixins: [ MixinFillerPage ],
    props: ['heading', 'screenName'],
    data() {
        return {
        searchItems: {
            brd_dt: '20200620',        // 방송일
            cate: '',                  // 분류
            editor: '',                // 사용자
            editorName: '',            // 사용자 이름
            name: '',                  // 소재명
            rowPerPage: 15,
            selectPage: 1,
            sortKey: '',
            sortValue: '',
        },
        isTableLoading: false,
        fields: [
            {
            name: 'rowNO',
            title: 'No',
            titleClass: "center aligned text-center",
            dataClass: "center aligned text-center",
            width: '4%',
            },
            {
            name: "categoryName",
            title: "매체",
            titleClass: "center aligned text-center",
            dataClass: "center aligned text-center",
            width: '10%',
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
                return this.$fn.dateStringTohaipun(v)
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

            this.isTableLoading = this.isScrollLodaing ? false: true;
            const brd_dt = this.searchItems.brd_dt;

            this.$http.get(`/api/Products/filler/${this.screenName}/${brd_dt}`, { params: this.searchItems })
                .then(res => {
                this.setResponseData(res, 'normal');
                this.addScrollClass();
                this.isTableLoading = false;
                this.isScrollLodaing = false;
            });
        },
        onChangeMedia(value) {
            this.getProOptions(value);
        },
        getOptions() {
            this.getMediaOptions();
            this.getEditorOptions();

            // 필러(pr)
            if (this.screenName === 'pr') this.getProOptions();
            // 필러(일반)
            if (this.screenName === 'general') this.getGeneralOptions();
            // 필러(기타)
            if (this.screenName === 'etc') this.getEtcOptions();
        }
    }
}
</script>
