<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="사용자 로그보기" :noNav="true" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      :isDisplayBtnArea="true"
      @changeRowPerpage="onChangeRowPerpage"
    >
      <!-- 검색 -->
    <template slot="form-search-area">
        <!-- 로그레벨 -->
        <b-form-group label="로그레벨" class="has-float-label c-zindex">
            <b-form-select
                class="width-140"
                v-model="searchItems.logLevel"
                :options="logLevelOptions"
                value-field="id"
                text-field="name"
                size="sm"
            >
            <template v-slot:first>
                <b-form-select-option value="">선택해주세요.</b-form-select-option>
            </template>
            </b-form-select>
        </b-form-group>
        <!-- 작업자 -->
        <b-form-group label="작업자" class="has-float-label c-zindex">
            <common-input-text v-model="searchItems.userName" />
        </b-form-group>
        <!-- 시작일 -->
        <b-form-group label="시작일"
          class="has-float-label"
          :class="{ 'hasError': hasErrorClass }">
          <common-date-picker v-model="searchItems.start_dt" :dayAgo="7" required/>
        </b-form-group>
        <!-- 종료일 -->
        <b-form-group label="종료일"
          class="has-float-label"
          :class="{ 'hasError': hasErrorClass }">
          <common-date-picker v-model="searchItems.end_dt" required/>
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
import MixinCommon from '../../../mixin/MixinCommon';

export default {
    mixins: [ MixinCommon ],
    data() {
        return {
            searchItems: {
                logLevel: '',          // 로그 유형
                userName: '',          // 작업자
                start_dt: '',  // 등록일 시작일
                end_dt: '',    // 등록일 종료일
                rowPerPage: 15,
                selectPage: 1,
                sortKey: '',
                sortValue: '',
            },
            isTableLoading: false,
            fields: [
                  {
                    name: '__sequence',
                    title: 'No',
                    titleClass: 'center aligned text-center',
                    dataClass: 'center aligned text-center',
                    width: '5%',
                },
                {
                    name: "logLevel",
                    title: "로그레벨",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: '7%',
                },
                {
                    name: "regDtm",
                    title: "일시",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: '10%',
                },
                {
                    name: "userName",
                    title: "작업자",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: '10%',
                },
                {
                    name: "category",
                    title: "접속단말",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: '10%',
                },
                {
                    name: "description",
                    title: "내용",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-left word-break-all",
                },
                {
                    name: "note",
                    title: "비고",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: '20%',
                },
            ],
            logLevelOptions: [
                { id: 'DEBUG', name: "DEBUG" },
                { id: 'INFO', name: "INFO" },
                { id: 'WARN', name: "WARN" },
                { id: 'ERROR', name: "ERROR" },
            ],
            responseData: {
                data: null,
                rowPerPage: 16,
                selectPage: 1,
                totalRowCount: 0,
            }
        }
    },
    created() {
        this.getData();
    },
    methods: {
        getData() {
          if (this.$fn.checkGreaterStartDate(this.searchItems.start_dt, this.searchItems.end_dt)) {
            this.$fn.notify('error', { message: '시작 날짜가 종료 날짜보다 큽니다.' });
            this.hasErrorClass = true;
          }

          this.isTableLoading = this.isScrollLodaing ? false: true;
          this.$http.get('/api/Logs', { params: this.searchItems })
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
                this.isTableLoading = false;
                this.isScrollLodaing = false;
        });
      },
    }
}
</script>
