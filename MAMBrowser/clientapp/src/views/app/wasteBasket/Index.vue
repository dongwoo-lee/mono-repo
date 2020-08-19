<template>
  <div>
    <!-- nav -->
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="휴지통" />
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
                  <b-button class="mb-1" variant="outline-primary default" size="sm">선택항목 삭제</b-button>
                </b-input-group>
                <b-input-group class="mr-2">
                  <b-button class="mb-1" variant="outline-secondary default" size="sm">선택항목 복원</b-button>
                </b-input-group>
                <b-input-group class="mr-2">
                  <b-button class="mb-1" variant="outline-danger default" size="sm">휴지통 비우기</b-button>
                </b-input-group>
               </b-form>
            </b-col>
            <b-col cols="auto" class="pt-3">
              <div class="page-info-group">
                <div class="page-info">
                  {{ getSelectedCount() }} {{ getPageInfo() }}
                </div>
                <div class="page-size">
                  <b-form-select v-model="searchItems.rowPerPage" @change="onChangeRowPerpage">
                    <b-form-select-option value="15">15개</b-form-select-option>
                    <b-form-select-option value="30">30개</b-form-select-option>
                    <b-form-select-option value="50">50개</b-form-select-option>
                    <b-form-select-option value="100">100개</b-form-select-option>
                  </b-form-select>
                </div>
              </div>
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
          @scrollPerPage="onScrollPerPage"
          @selectedIds="onSelectedIds"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <b-row>
              <b-colxx>
                <b-button variant="default" class="icon-buton" title="복원">
                  <b-icon icon="shift-fill" class="icon"></b-icon>
                </b-button>
                <b-button variant="default" class="icon-buton" title="영구삭제" @click.stop="onDeleteConfirm(props.props.rowData.seq)">
                  <b-icon icon="dash-square" class="icon" variant="danger"></b-icon>
                </b-button>
              </b-colxx>
            </b-row>
          </template>
        </c-data-table-scroll-paging>
      </b-card>
    </b-row>
    <!-- 영구삭제 모달 -->
    <b-modal 
        id="modalRemove" 
        size="sm" 
        title="영구삭제"
        :hideHeaderClose="true">
        영구적으로 삭제하시겠습니까?
        <template v-slot:modal-footer>
            <b-button
              variant="primary"
              size="sm"
              class="float-right"
              @click="onDelete()"
            >
              영구삭제
            </b-button>
            <b-button
              variant="danger"
              size="sm"
              class="float-right"
              @click="$bvModal.hide('modalRemove')"
            >
              취소
            </b-button>
      </template>
    </b-modal>
  </div>
</template>

<script>
import MixinBasicPage from '../../../mixin/MixinBasicPage';

export default {
  mixins: [ MixinBasicPage ],
  data() {
    return {
      searchItems: {
        // cate: '',           // 분류
        title: '',             // 제목
        memo: '',              // 메모
        // start_dt: '',       // 삭제기간-시작일
        // end_dt: '',         // 삭제기간-종료일
        rowPerPage: 15,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      removeRowId: null,
      fields: [
        {
          name: "__checkbox",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "5%"
        },
        {
          name: 'rowNO',
          title: 'No',
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '4%',
        },
        {
          name: "title",
          title: "제목",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "memo",
          title: "메모",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "fileExt",
          title: "파일형식",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "audioFormat",
          title: "오디오포맷",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: "editedDtm",
          title: "삭제일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
        },
        {
          name: '__slot:actions',
          title: 'Actions',
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        }
      ]
    }
  },
  methods: {
    getData() {
      const userExtId = sessionStorage.getItem('user_ext_id');
      this.$http.get(`/api/products/workspace/private/recyclebin/${userExtId}`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
      });
    },
    onDeleteConfirm(id) {
      this.removeRowId = id;
      this.$bvModal.show('modalRemove');
    },
    onDelete() {

    }
  }
}
</script>