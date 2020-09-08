<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="DL 3.0" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      @changeRowPerpage="onChangeRowPerpage"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            class="width-120"
            v-model="searchItems.media"
            :options="mediaOptions"
            value-field="id"
            text-field="name"
          />
        </b-form-group>
        <!-- 편성일자: 시작일자 -->
        <b-form-group label="편성일자*"
          class="has-float-label"
          :class="{ 'hasError': $v.searchItems.regDtm.required }">
          <common-date-picker v-model="$v.searchItems.regDtm.$model"/>
          <b-form-invalid-feedback :state="!$v.searchItems.regDtm.required">날짜는 필수 입력입니다.</b-form-invalid-feedback>
        </b-form-group>
        <!-- 녹음명 -->
        <b-form-group label="녹음명" class="has-float-label c-zindex">
            <common-input-text v-model="$v.searchItems.userName" />
        </b-form-group>
        <!-- 등록일 -->
        <b-form-group label="등록일" class="has-float-label">
          <common-date-picker 
            v-model="searchItems.brd_dt" isCurrentDate />
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
        <common-data-table
            :fields="fields"
            :rows="responseData.data"
            :isTableLoading="isTableLoading"
        />
      </template>
    </common-form>
  </div>
</template>

<script>
import MixinBasicPage from '../../../mixin/MixinBasicPage';

export default {
    mixins: [ MixinBasicPage ],
    data() {
        return {
            searchItems: {
                media : 'A',           // 매체
                regDtm: '',            // 편성일자
                pgmID: '',             // 녹음명
                brd_dt: '',            // 등록일
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
                    name: "mediaName",
                    title: "단말",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: '5%',
                },
                {
                    name: "mediaName",
                    title: "매체",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: '5%',
                },
                {
                    name: "mediaName",
                    title: "녹음명",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                },
                {
                    name: "mediaName",
                    title: "편성일자",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                },
                {
                    name: "mediaName",
                    title: "녹음시작일시",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                },
                {
                    name: "mediaName",
                    title: "녹음종료일시",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                },
                {
                    name: "mediaName",
                    title: "방송분량",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                },
                {
                    name: "mediaName",
                    title: "파일크기(MB)",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                },
                {
                    name: "mediaName",
                    title: "등록일시",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                },
            ]
        }
    },
    created() {
        this.getMediaOptions();
    },
    methods: {
        getData() {
            if (!this.$v.searchItems.regDtm.$invalid) {
                this.$fn.notify('inputError', {});
                return;
            }

            this.isTableLoading = true;
            const media = this.searchItems.media;
            const regDtm = this.searchItems.regDtm;

            this.$http.get(`/api/Products/dl30/${media}/${regDtm}`, { params: this.searchItems })
                .then(res => {
                this.setResponseData(res, 'normal');
                this.isTableLoading = false;
            });
        },
    }
}
</script>
