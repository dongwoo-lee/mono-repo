<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="DL 3.0" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <b-row>
        <b-colxx>
            <b-card>
                <!-- 검색 -->
                <b-container fluid>
                <div class="search-form-area">
                    <b-form @submit.stop>
                    <b-row>
                        <!-- 매체 -->
                        <b-colxx sm="2">
                        <b-form-group label="매체" class="has-float-label c-zindex">
                            <b-form-select
                                v-model="searchItems.media"
                                :options="mediaOptions"
                                value-field="id"
                                text-field="name"
                                size="sm"
                            />
                        </b-form-group>
                        </b-colxx>
                        <!-- 편성일자: 시작일자 -->
                        <b-colxx sm="2">
                        <b-form-group label="편성일자" class="has-float-label c-zindex">
                            <c-input-date-picker v-model="$v.searchItems.regDtm.$model" />
                            <b-form-invalid-feedback
                            :state="$v.searchItems.regDtm.check_date"
                            >날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                        </b-form-group>
                        </b-colxx>
                        <!-- 녹음명 -->
                        <b-colxx sm="2">
                            <b-form-group label="녹음명" class="has-float-label c-zindex">
                                <c-input-text v-model="$v.searchItems.userName" />
                            </b-form-group>
                        </b-colxx>
                        <!-- 등록일 -->
                        <b-colxx sm="2">
                            <b-form-group label="등록일" class="has-float-label c-zindex">
                                <c-input-date-picker v-model="$v.searchItems.brd_dt.$model" />
                                <b-form-invalid-feedback
                                :state="$v.searchItems.brd_dt.check_date"
                                >날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                            </b-form-group>
                        </b-colxx>
                        <b-colxx sm="2">
                            <b-button class="mb-1" variant="primary default" @click="getData">검색</b-button>
                        </b-colxx>
                    </b-row>
                    </b-form>
                </div>
                <!-- 테이블 -->
                <c-data-table
                    :fields="fields"
                    :rows="responseData.data"
                />
                </b-container>
            </b-card>
        </b-colxx>
    </b-row>
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
            if (this.$v.$invalid) {
                this.$fn.notify('inputError', {});
                return;
            }

            const media = this.searchItems.media;
            const brd_dt = this.searchItems.brd_dt;

            this.$http.get(`/api/Products/dl30/${media}/${brd_dt}`, { params: this.searchItems })
                .then(res => {
                this.setResponseData(res, 'normal');
            });
        },
    }
}
</script>
