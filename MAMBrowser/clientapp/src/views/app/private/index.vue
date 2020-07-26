<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb :heading="$t('menu.private')"/>
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <b-row>
      <b-colxx xxs="12">
        <!-- 검색 -->
        <b-card class="mb-4">
          <b-form @submit.stop>
            <b-row>
              <!-- 분류 -->
              <!-- <b-colxx sm="1">
                  <b-form-group label="분류" class="inline-block">
                      <c-input v-model="searchItems.cete"></c-input>
                  </b-form-group>
              </b-colxx> -->
              <!-- 파일명 -->
              <b-colxx sm="2">
                <b-form-group label="파일명" class="inline-block">
                  <c-input-text v-model="searchItems.filename" />
                </b-form-group>
              </b-colxx>
              <!-- 제목 -->
              <b-colxx sm="2">
                <b-form-group label="제목" class="inline-block">
                  <c-input-text v-model="searchItems.title" />
                </b-form-group>
              </b-colxx>
              <!-- 메모 -->
              <b-colxx sm="3">
                <b-form-group label="메모" class="inline-block">
                  <c-input-text v-model="searchItems.memo" />
                </b-form-group>
              </b-colxx>
              <b-button class="mb-1" variant="primary default" size="sm" @click="getData">검색</b-button>
            </b-row>
          </b-form>
        </b-card>
        <!-- 테이블 -->
        <b-card class="mb-4">
            <b-form class="mb-3" inline>
                <b-input-group class="mr-2">
                    <b-button class="mb-1" variant="primary default" size="sm">파일 업로드</b-button>
                </b-input-group>
                <b-input-group class="mr-2">
                    <b-button class="mb-1" variant="secondary default" size="sm">다운로드</b-button>
                </b-input-group>
                <b-input-group class="mr-2">
                    <b-button class="mb-1" variant="danger default" size="sm">삭제</b-button>
                </b-input-group>
                <b-input-group class="mr-2">
                    <b-button class="mb-1" variant="info default" size="sm">정보편집</b-button>
                </b-input-group>
            </b-form>
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
                @contextMenuAction="onContextMenuAction"
                @sortableclick="onSortableClick"
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
import CInputText from '../../../components/Input/CInputText';
import CDataTableScrollPaging from '../../../components/DataTable/CDataTableScrollPaging';

export default {
  components: { CInputText, CDataTableScrollPaging },
  data() {
    return {
      searchItems: {
        cate: '',
        filename: '',
        title: '',
        memo: '',
        rowPerPage: 16,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      responseData: {
          data: null,
          rowPerPage: 16,
          selectPage: 1,
          totalRowCount: 0,
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
          title: "파일명",
          titleClass: "",
          dataClass: "list-item-heading",
          width: "15%"
        },
        {
          name: "sales",
          title: "메모",
          titleClass: "",
          dataClass: "list-item-heading",
        },
        {
          name: "stock",
          title: "파일형식",
          titleClass: "",
          dataClass: "list-item-heading",
          width: "20%"
        },
        {
          name: "category",
          title: "상세정보",
          titleClass: "",
          dataClass: "list-item-heading",
          width: "25%"
        },
        {
          name: "writeDate",
          title: "등록일시",
          titleClass: "",
          dataClass: "list-item-heading",
          width: "10%"
        },
      ],
      contextMenu: [
          { name: 'edit', text: '편집' },
          { name: 'throw', text: '휴지통으로 보내기' },
          { name: 'download', text: '다운로드' },
      ]
    }
  },
  created() {
    this.$nextTick(() => {
      this.getData();
    });
  },
  methods: {
    getData() {
      const params = {
          // 분류(cate)
          // 파일명(filename)
          // 제목(title)
          // 메모(memo)
          // 페이지당 행 개수(rowPerPage)
          // 선택된 페이지(selectPage)
          // sortKey(정렬 키, 필드명)
          // sortValue(정렬값, ASC/DESC)
          cate: this.searchItems.cate,
          filename: this.searchItems.filename,
          title: this.searchItems.title,
          memo: this.searchItems.memo,
          sortKey: this.searchItems.sortKey,
          sortValue: this.searchItems.sortValue,
          rowPerPage: this.responseData.rowPerPage,
          selectPage: this.responseData.selectPage,
      }

      const editor = sessionStorage.getItem('user_name');

      this.$http.get(`/api/products/workspace/private/files/${editor}`, { params: params })
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
      });
    },
    onScrollPerPage() {
      this.responseData.selectPage++;
      this.getDate();
    },
    onContextMenuAction(v) {
        switch(v) {
            case 'edit': console.log(v); break;
            case 'throw': console.log(v); break;
            case 'download':  console.log(v); break;
            default: break;
        }
    },
    onSortableClick(sortKey) {
      this.searchItems.sortKey = sortKey;
      this.searchItems.sortValue = this.$fn.changeSortValue(this.searchItems.sortValue);
      this.getDate();
    },
  }
}
</script>