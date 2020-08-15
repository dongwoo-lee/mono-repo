<template>
  <div>
    <!-- nav -->
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="My공간" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <b-row style="marin-top:-10px;">
      <b-card>
        <!-- 검색 영역 -->
        <b-container fluid>
          <div class="search-form-area">
            <b-form class="search-form" @submit.stop>
              <b-row>
                <!-- 등록일: 시작일 -->
                <b-colxx sm="2">
                  <b-form-group label="시작일" class="has-float-label c-zindex">
                    <c-input-date-picker v-model="$v.searchItems.start_dt.$model" />
                    <b-form-invalid-feedback
                      :state="$v.searchItems.start_dt.check_date"
                    >날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                  </b-form-group>
                </b-colxx>
                <!-- 등록일: 종료일 -->
                <b-colxx sm="2">
                  <b-form-group label="종료일" class="has-float-label c-zindex">
                    <c-input-date-picker v-model="$v.searchItems.end_dt.$model" />
                    <b-form-invalid-feedback
                      :state="$v.searchItems.end_dt.check_date"
                    >날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                  </b-form-group>
                </b-colxx>
                <!-- 제목 -->
                <b-colxx sm="2">
                  <b-form-group label="제목" class="has-float-label c-zindex">
                    <c-input-text v-model="searchItems.title" />
                  </b-form-group>
                </b-colxx>
                <!-- 메모 -->
                <b-colxx sm="2">
                  <b-form-group label="메모" class="has-float-label c-zindex">
                    <c-input-text v-model="searchItems.memo" />
                  </b-form-group>
                </b-colxx>
                <b-colxx sm="2">
                  <b-button class="mb-1" variant="outline-primary default" size="sm" @click="getData">검색</b-button>
                </b-colxx>
              </b-row>
            </b-form>
          </div>
        </b-container>

        <!-- 버튼 영역 -->
        <b-container fluid class="text-center">
          <!-- 버튼 모음 -->
          <b-row align-v="center">
            <b-col cols="auto" class="mr-auto pt-3">
              <b-form class="mb-1" inline>
                <b-input-group class="mr-2">
                  <b-button class="mb-1" variant="outline-primary default" size="sm" @click="onShowModalFileUpload">파일 업로드</b-button>
                </b-input-group>
                <b-input-group class="mr-2">
                  <b-button class="mb-1" variant="outline-secondary default" size="sm">선택 항목 다운로드</b-button>
                </b-input-group>
                <b-input-group class="mr-2">
                  <b-button class="mb-1" variant="outline-danger default" size="sm">선택 항목 삭제</b-button>
                </b-input-group>
               </b-form>
            </b-col>
            <b-col cols="auto" class="pt-3">
              전체 {{ responseData.totalRowCount }}개 중 {{ responseData.data.length }}개표시
              <b-form-select class="page-size" v-model="searchItems.rowPerPage">
                <b-form-select-option value="8">8개</b-form-select-option>
                <b-form-select-option value="16">16개</b-form-select-option>
                <b-form-select-option value="24">24개</b-form-select-option>
              </b-form-select>
            </b-col>
          </b-row>
        </b-container>
        <!-- 테이블 영역 -->
        <c-data-table-scroll-paging
          ref="scrollPaging"
          :table-height="'500px'"
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          :is-actions-slot="true"
          :num-rows-to-bottom="5"
          :contextmenu="contextMenu"
          @scrollPerPage="onScrollPerPage"
          @selectedItems="onSelectedItems"
          @contextMenuAction="onContextMenuAction"
          @sortableclick="onSortable"
        >
          <template slot="actions" scope="props">
            <b-button class="mb-1" variant="primary default" @click="handlerPreview(props, props.rowIndex)">미리듣기</b-button>
          </template>
        </c-data-table-scroll-paging>
      </b-card>
    </b-row>
  </div>
</template>

<script>
import MixinBasicPage from '../../../mixin/MixinBasicPage';
import { mapActions } from 'vuex';

export default {
  mixins: [ MixinBasicPage ],
  data() {
    return {
      searchItems: {
        cate: '',              // 분류(cate)
        title: '',             // 제목
        memo: '',              // 메모(memo)
        start_dt: '20200101',  // 등록일 시작일
        end_dt: '',            // 등록일 종료일
        rowPerPage: 16,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      fields: [
        {
          name: "__checkbox",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "5%"
        },
        {
          name: "title",
          title: "제목",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%"
        },
        {
          name: "memo",
          title: "메모",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        // {
        //   name: "",
        //   title: "파일형식",
        //   titleClass: "center aligned text-center",
        //   dataClass: "center aligned text-center",
        //   width: "20%"
        // },
        {
          name: "audioFormat",
          title: "오디오포맷",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "25%"
        },
        {
          name: "editedDtm",
          title: "등록일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
      ],
    }
  },
  methods: {
    ...mapActions('file', ['open_popup']),
    getData() {
      const userExtId = sessionStorage.getItem('user_ext_id');

      this.$http.get(`/api/products/workspace/private/meta/${userExtId}`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
      });
    },
    onShowModalFileUpload() {
      this.open_popup();
    }
  }
}
</script>