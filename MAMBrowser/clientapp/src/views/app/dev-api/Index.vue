<template>
<div>
    <b-row>
        <b-colxx xxs="12">
            <piaf-breadcrumb heading="개발 API 연결 컴포넌트 모음" />
            <div class="separator mb-5"></div>
        </b-colxx>
    </b-row>
    <b-row>
        <!-- dropdown menu + input + 순간 검색 -->
        <b-colxx xxs="4">
            <b-card class="mb-4" title="dropdownmenu-input - /api/Categories/media(매체목록)">
                <c-dropdown-menu-input
                    :suggestions="suggestions"
                    @selected="onDropdownInputSelected"
                    @enter="onDropdownInputEnter"
                />
                <div>Selected: {{ localDropdownSelectedVal }}</div>
                <div>Enteed: {{ localDropdownEnteredVal }}</div>
            </b-card>
        </b-colxx>
        <!-- scroll paging table -->
        <b-colxx xxs="12">
             <!-- 검색 -->
            <b-card class="mb-4">
            <b-form @submit.stop>
                <b-row>
                    <b-colxx sm="3">
                        <!-- 데이터 피커 -->
                        <b-form-group label="날짜(시작-종료)" class="inline-block">
                            <c-input-date-picker
                                @startDate="onDatePickerStartDate"
                                @endDate="onDatePickerEndDate"
                            />
                        </b-form-group>
                    </b-colxx>
                    <b-colxx sm="1">
                        <!-- 인풋 텍스트: 사용자 -->
                        <b-form-group label="사용자" class="inline-block">
                            <c-input v-model="localSearchItems.editor"></c-input>
                        </b-form-group>
                    </b-colxx>
                    <b-colxx sm="1">
                        <!-- 인풋 텍스트: 사용자 -->
                        <b-form-group label="이름" class="inline-block">
                            <c-input v-model="localSearchItems.name"></c-input>
                        </b-form-group>
                    </b-colxx>
                    <b-button class="mb-1" variant="primary default" size="sm" @click="getProductSpotSrcData">검색</b-button>
                </b-row>
            </b-form>
            </b-card>
            <b-card class="mb-4" title="스크롤 페이징 - /api/Products/spot/scr?start_dt=20200101&end_dt=20200701&rowPerPage=16&selectPage=1(부조 SPOT 소재 조회)">
                <div>Selected: {{ logProductSpotSrc }}
                    <span style="color:red">※ Todo: 날짜 입력 안할 경우 처리는 어떻게 할것인가?</span>
                </div>
                <data-table-scroll-paging
                    ref="scrollPaging"
                    :table-height="'500px'"
                    :fields="localDataTableFields"
                    :rows="localDataTableData"
                    :per-page="localDataTableRowPerPage"
                    :is-actions-slot="true"
                    :num-rows-to-bottom="3"
                    @scrollPerPage="onScrollPerPage"
                >
                    <template slot="actions">
                        <b-button class="mb-1" variant="primary default">미리듣기</b-button>
                    </template>
                </data-table-scroll-paging>
            </b-card>
        </b-colxx>
    </b-row>
</div>
</template>

<script>
import CDropdownMenuInput from '../../../components/Input/CDropdownMenuInput';
import DataTableScrollPaging from '../../../components/DataTable/DataTableScrollPaging';
import CInputDatePicker from '../../../components/Input/CInputDatePicker';
import CInput from '../../../components/Input/CInputText';

export default {
    components: { 
        CDropdownMenuInput,
        DataTableScrollPaging,
        CInputDatePicker,
        CInput,
    },
    data() {
        return {
            logProductSpotSrc: '',
            localSearchItems: {              // 검색
                start_dt: '',
                end_dt: '',
                editor: '',
                name: '',
            },
            suggestions: [],                 // 드롭다운 제안 리스트
            localDropdownSelectedVal : '',   // 드롭다운 선택 값
            localDropdownEnteredVal: '',     // 드롭다운 엔터키 값
            localDataTableData: [],          // 테이블 데이터
            localDataTableRowPerPage: 16,    // 테이블 로우 개수
            localDataTableSelectPage: 1,     // 테이블 현재 페이지
            localDataTableFields: [          // 테이블 헤더
                {
                    name: "__checkbox",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: "5%"
                },
                {
                    name: "name",
                    title: "소재명",
                    titleClass: "",
                    dataClass: "list-item-heading",
                },
                {
                    name: "category",
                    title: "분류명",
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
                    titleClass: "",
                    dataClass: "list-item-heading",
                },
                {
                    name: "brdDT",
                    title: "방송일",
                    titleClass: "",
                    dataClass: "list-item-heading",
                },
                {
                    name: "editorID",
                    title: "제작자ID",
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
                {
                    name: "editorName",
                    title: "제작자",
                    titleClass: "",
                    dataClass: "list-item-heading",
                },
                {
                    name: "editDtm",
                    title: "편집일시",
                    titleClass: "",
                    dataClass: "list-item-heading",
                },
                {
                    name: "count",
                    title: "개수",
                    titleClass: "",
                    dataClass: "list-item-heading",
                },
            ]
        }
    },
    created() {
        this.getMediaData();
        this.getProductSpotSrcData();
    },
    methods: {
        getMediaData() {
            this.$http.post('/api/Categories/media').then(
                res => {
                    if (res.status === 200) {
                        this.suggestions = res.data.resultObject.dataList;
                    }
                }
            )
        },
        getProductSpotSrcData() {
            const params = {
                start_dt: this.localSearchItems.start_dt,
                end_dt: this.localSearchItems.end_dt,
                editor: this.localSearchItems.editor,
                name: this.localSearchItems.name,
                rowPerPage: this.localDataTableRowPerPage,
                selectPage: this.localDataTableSelectPage,
            }

            this.logProductSpotSrc = params;
            this.$http.get('/api/Products/spot/scr', {params : params}).then(
                res => {
                    if (res.status === 200) {
                        const { dataList, rowPerPage, selectPage } = res.data.resultObject;
                        if (selectPage > 1) {
                            dataList.forEach(row => {
                                this.localDataTableData.push(row);
                            })
                        } else {
                            this.localDataTableData = dataList;
                            this.localDataTableRowPerPage = rowPerPage;
                            this.localDataTableSelectPage = selectPage;
                        }
                    }
                }
            )
        },
        onDropdownInputSelected(data) {
            this.localDropdownSelectedVal = data;
        },
        onDropdownInputEnter(data) {
            this.localDropdownEnteredVal = data;
        },
        onScrollPerPage() {
            this.localDataTableSelectPage++;
            this.getProductSpotSrcData();
        },
        onDatePickerStartDate(v){
            this.localSearchItems.start_dt = v;
        },
        onDatePickerEndDate(v) {
            this.localSearchItems.end_dt = v;
        },
    }
}
</script>
