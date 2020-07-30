<template>
<div>
    <b-row>
        <b-colxx xxs="12">
            <piaf-breadcrumb heading="개발 API 연결 컴포넌트 모음" />
            <div class="separator mb-3"></div>
        </b-colxx>
    </b-row>
    <b-row>
        <!-- dropdown menu + input + 순간 검색 -->
        <b-colxx xxs="6">
            <b-card class="mb-4" title="dropdownmenu-input - /api/Categories/media(매체목록)">
                <c-dropdown-menu-input
                    :suggestions="suggestions"
                    @selected="onDropdownInputSelected"
                />
                <div>Selected: {{ localDropdownSelectedVal }}</div>
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
                                <c-input-date-picker-group
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
            <div>Selected: {{ logProductSpotSrc }}</div>
            <b-card class="mb-4" title="스크롤 페이징 - /api/Products/spot/scr?start_dt=20200101&end_dt=20200701&rowPerPage=16&selectPage=1(부조 SPOT 소재 조회)">
                <b-form class="mb-3" inline>
                    <b-input-group class="mr-2">
                        <b-button class="mb-1" variant="primary default" size="sm">파일 업로드</b-button>
                    </b-input-group>
                    <b-input-group class="mr-2">
                        <b-button class="mb-1" variant="danger default" size="sm">휴지통비우기</b-button>
                    </b-input-group>
                    <b-input-group class="mr-2">
                        <b-button class="mb-1" variant="secondary default" size="sm">복원</b-button>
                    </b-input-group>
                </b-form>
                <c-data-table-scroll-paging
                    ref="scrollPaging"
                    :table-height="'500px'"
                    :fields="localDataTableFields"
                    :rows="localDataTableData"
                    :per-page="localDataTableRowPerPage"
                    :is-actions-slot="true"
                    :num-rows-to-bottom="3"
                    :contextmenu="localContextMenu"
                    @scrollPerPage="onScrollPerPage"
                    @contextMenuAction="onContextMenuAction"
                    @sortableclick="onSortable"
                >
                    <template slot="actions" scope="props">
                        <b-button class="mb-1" variant="primary default" @click="handlerPreview(props, props.rowIndex)">미리듣기</b-button>
                    </template>
                </c-data-table-scroll-paging>
            </b-card>
        </b-colxx>
    </b-row>
</div>
</template>

<script>
import CDropdownMenuInput from '../../../components/Input/CDropdownMenuInput';
import CDataTableScrollPaging from '../../../components/DataTable/CDataTableScrollPaging';
import CInputDatePickerGroup from '../../../components/Input/CInputDatePickerGroup';
import CInput from '../../../components/Input/CInputText';

export default {
    components: { 
        CDropdownMenuInput,
        CDataTableScrollPaging,
        CInputDatePickerGroup,
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
                sortKey: '',
                sortValue: 'DESC',
            },
            responseData: {
                data: null,
                rowPerPage: 16,
                selectPage: 1,
                totalRowCount: 0,
            },
            suggestions: [],                 // 드롭다운 제안 리스트
            localDropdownSelectedVal : '',   // 드롭다운 선택 값
            localDropdownEnteredVal: '',     // 드롭다운 엔터키 값
            localDataTableData: [],          // 테이블 데이터
            localDataTableRowPerPage: 16,    // 테이블 로우 개수
            localDataTableSelectPage: 1,     // 테이블 현재 페이지
            localDataTableTotalRowCount: 0,
            localDataTableFields: [          // 테이블 헤더
                {
                    name: "__checkbox",
                    titleClass: "center aligned text-center",
                    dataClass: "center aligned text-center",
                    width: "5%"
                },
                {
                    name: "rowNO",
                    title: "",
                    titleClass: "center aligned",
                    dataClass: "center aligned text-center",
                    width: "4%"
                },
                {
                    name: "name",
                    title: "소재명",
                    titleClass: 'center aligned',
                    dataClass: "center aligned",
                },
                {
                    name: "categoryName",
                    title: "분류명",
                    titleClass: 'center aligned',
                    dataClass: "center aligned text-center",
                },
                {
                    name: "duration",
                    title: "길이",
                    titleClass: 'center aligned',
                    dataClass: "center aligned text-center",
                    width: '6%',
                    callback: (v) => {
                        return this.$fn.splitFirst(v);
                    }
                },
                {
                    name: "track",
                    title: "트랙",
                    titleClass: 'center aligned',
                    dataClass: "center aligned text-center",
                    width: "4%"
                },
                {
                    name: "brdDT",
                    sortField: 'brdDT',
                    title: "방송일",
                    titleClass: 'center aligned',
                    dataClass: "center aligned text-center",
                    callback: (v) => {
                        return this.$fn.formatDate(v, 'yyyy-MM-dd');
                    }
                },
                {
                    name: "editorID",
                    title: "제작자ID",
                    titleClass: 'center aligned',
                    dataClass: "center aligned text-center",
                },
                {
                    name: "pgmName",
                    title: "사용처명",
                    titleClass: 'center aligned',
                    dataClass: "center aligned text-center",
                },
                {
                    name: "masteringDtm",
                    title: "마스터링 일시",
                    titleClass: 'center aligned',
                    dataClass: "center aligned text-center",
                    width: '9%',
                },
                {
                    name: "filePath",
                    title: "파일경로",
                    titleClass: 'center aligned',
                    dataClass: "center aligned text-center word-break",
                    width: "10%"
                },
                {
                    name: "editorName",
                    title: "제작자",
                    titleClass: 'center aligned',
                    dataClass: "center aligned text-center",
                },
                {
                    name: "editDtm",
                    title: "편집일시",
                    titleClass: 'center aligned',
                    dataClass: "center aligned text-center",
                },
                {
                    name: '__slot:actions',
                    title: '미리듣기',
                    dataClass: "center aligned text-center",
                    width: "10%"
                }   
            ],
            localContextMenu: [
                { name: 'edit', text: '편집' },
                { name: 'throw', text: '휴지통으로 보내기' },
                { name: 'download', text: '다운로드' },
            ]
        }
    },
    created() {
        this.$nextTick(() => {
            this.getMediaData();
            this.getProductSpotSrcData();
        });
    },
    methods: {
        getMediaData() {
            this.$http.get('/api/Categories/media').then(
                res => {
                    if (res.status === 200) {
                        this.suggestions = res.data.resultObject.data;
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
                sortKey: this.localSearchItems.sortKey,
                sortValue: this.localSearchItems.sortValue,
                rowPerPage: this.localDataTableRowPerPage,
                selectPage: this.localDataTableSelectPage,
            }

            this.logProductSpotSrc = params;
            this.$http.get('/api/Products/spot/scr', {params : params}).then(
                res => {
                    if (res.status === 200) {
                        const { data, rowPerPage, selectPage, totalRowCount } = res.data.resultObject;
                        if (selectPage > 1) {
                            data.forEach(row => {
                                this.localDataTableData.push(row);
                            })
                        } else {
                            this.localDataTableData = data;
                            this.localDataTableRowPerPage = rowPerPage;
                            this.localDataTableSelectPage = selectPage;
                            this.localDataTableTotalRowCount = totalRowCount;
                        }
                    } else {
                        this.$fn.notify('server-error', { message: '스크롤 페이징 테이블 조회 에러' });
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
        onContextMenuAction(v) {
            switch(v) {
                case 'edit': console.log(v); break;
                case 'throw': console.log(v); break;
                case 'download':  console.log(v); break;
                default: break;
            }
        },
        handlerPreview(props) {
            console.info('handlerPreview', props);
        },
        onSortable(d) {
            this.localSearchItems.sortKey = d;
            this.localSearchItems.sortValue = this.$fn.changeSortValue(this.localSearchItems.sortValue);
            this.getProductSpotSrcData();
        }
    }
}
</script>
