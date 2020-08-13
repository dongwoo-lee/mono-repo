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
                                v-model="searchItems.logLevel"
                                :options="logLevelOptions"
                                value-field="id"
                                text-field="name"
                                size="sm"
                            />
                        </b-form-group>
                        </b-colxx>
                        <!-- 편성일자: 시작일 -->
                        <b-colxx sm="2">
                        <b-form-group label="편성-시작일" class="has-float-label c-zindex">
                            <c-input-date-picker v-model="searchItems.start_dt" />
                            <!-- <b-form-invalid-feedback
                            :state="$v.searchItems.start_dt.check_date"
                            >날짜 형식이 맞지 않습니다.</b-form-invalid-feedback> -->
                        </b-form-group>
                        </b-colxx>
                        <!-- 편성일자: 종료일 -->
                        <b-colxx sm="2">
                        <b-form-group label="편성-종료일" class="has-float-label c-zindex">
                            <c-input-date-picker v-model="searchItems.end_dt" />
                            <!-- <b-form-invalid-feedback
                            :state="$v.searchItems.end_dt.check_date"
                            >날짜 형식이 맞지 않습니다.</b-form-invalid-feedback> -->
                        </b-form-group>
                        </b-colxx>
                        <!-- 녹음명 -->
                        <b-colxx sm="2">
                        <b-form-group label="녹음명" class="has-float-label c-zindex">
                            <c-input-text v-model="searchItems.userName" />
                        </b-form-group>
                        </b-colxx>
                        <b-colxx sm="2">
                        <b-button class="mb-1" variant="primary default" @click="getData">검색</b-button>
                        </b-colxx>
                    </b-row>
                    </b-form>
                </div>
                <!-- 테이블 -->

                </b-container>
            </b-card>
        </b-colxx>
    </b-row>
  </div>
</template>
<script>
import MixinLogPage from '../../../mixin/MixinLogPage';

export default {
    mixins: [ MixinLogPage ],
    data() {
        return {
            searchItems: {
                logLevel: '',          // 로그 유형
                userName: '',          // 작업자
                start_dt: '20200101',  // 등록일 시작일
                end_dt: '20200730',    // 등록일 종료일
                rowPerPage: 16,
                selectPage: 1,
                sortKey: '',
                sortValue: '',
            },
            logLevelOptions: [
                { id: 'DEBUG', name: "DEBUG" },
                { id: 'INFO', name: "INFO" },
                { id: 'WARN', name: "WARN" },
                { id: 'ERROR', name: "ERROR" },
            ],
        }
    },
    methods: {
        getData() {

        },
    }
}
</script>
