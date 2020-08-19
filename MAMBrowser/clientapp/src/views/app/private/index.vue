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
                  <b-button class="mb-1" variant="outline-secondary default" size="sm" @click="onDownload">선택 항목 다운로드</b-button>
                </b-input-group>
                <b-input-group class="mr-2">
                  <b-button class="mb-1" variant="outline-danger default" size="sm" @click="onDelete">선택 항목 휴지통 보내기</b-button>
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
          :contextmenu="contextMenu"
          @scrollPerPage="onScrollPerPage"
          @selectedIds="onSelectedIds"
          @contextMenuAction="onContextMenuAction"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <b-row>
              <b-colxx>
                <b-button variant="default" class="icon-buton" title="미리듣기">
                  <b-icon icon="caret-right-square" class="icon"></b-icon>
                </b-button>
                <b-button variant="default" class="icon-buton" title="다운로드" @click.stop="onDownload(props.props.rowData.seq)">
                  <b-icon icon="download" class="icon"></b-icon>
                </b-button>
                <b-button variant="default" class="icon-buton" title="휴지통" @click.stop="onDeleteConfirm(props.props.rowData.seq)">
                  <b-icon icon="dash-square" class="icon" variant="danger"></b-icon>
                </b-button>
                <b-button variant="default" class="icon-buton" title="정보편집" @click.stop="onPrivateModifyPopup(props.props.rowData)">
                  <b-icon icon="exclamation-square" class="icon" variant="info"></b-icon>
                </b-button>
              </b-colxx>
            </b-row>
          </template>
        </c-data-table-scroll-paging>
      </b-card>
    </b-row>
    <!-- 휴지통 이동 모달 -->
    <b-modal 
        id="modalRemove" 
        size="sm" 
        title="휴지통 이동"
        :hideHeaderClose="true">
        휴지통으로 이동하시겠습니까?
        <template v-slot:modal-footer>
            <b-button
              variant="primary"
              size="sm"
              class="float-right"
              @click="onDelete()"
            >
              이동
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
    <!-- My공간 메타데이터 수정 팝업 -->
    <meta-data-private-modify-popup
      ref="refMetaDataModifyPopup"
      :show="metaDataModifyPopup"
      @editSuccess="onEditSuccess"
      @close="metaDataModifyPopup = false">
    </meta-data-private-modify-popup>
  </div>
</template>

<script>
import MixinBasicPage from '../../../mixin/MixinBasicPage';
import MetaDataPrivateModifyPopup from '../../../components/popup/MetaDataPrivateModifyPopup';
import { mapActions } from 'vuex';

export default {
  mixins: [ MixinBasicPage ],
  components: { MetaDataPrivateModifyPopup },
  data() {
    return {
      searchItems: {
        cate: '',              // 분류(cate)
        title: '',             // 제목
        memo: '',              // 메모(memo)
        start_dt: '20200101',  // 등록일 시작일
        end_dt: '',            // 등록일 종료일
        rowPerPage: 15,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      metaDataModifyPopup: false,
      selectedIds: null,
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
          width: "25%"
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
          width: "8%"
        },
        {
          name: "editedDtm",
          title: "등록일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "15%"
        },
        {
          name: '__slot:actions',
          title: 'Actions',
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        }
      ],
      removeRowId: null,
    }
  },
  methods: {
    ...mapActions('file', ['open_popup', 'download']),
    getData() {
      const userExtId = sessionStorage.getItem('user_ext_id');

      this.$http.get(`/api/products/workspace/private/meta/${userExtId}`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
            setTimeout(() => {
              this.$refs.scrollPaging.addClassScroll();  
            }, 0);
      });
    },
    onShowModalFileUpload() {
      this.open_popup();
    },
    getPageInfo() {
      const dataLength = this.responseData.data ? this.responseData.data.length : 0;
      return `전체 ${this.responseData.totalRowCount}개 중 ${dataLength}개 표시`
    },
    getSelectedCount() {
      if (!this.selectedIds || this.selectedIds.length === 0) return '';
      return `${ this.selectedIds.length }개 선택되었습니다. |`;
    },
    onSelectedIds(ids) {
      this.selectedIds = ids;
    },
    onDownload(seq) {
      let ids = this.selectedIds;

      if (typeof seq !== 'object' && seq) {
        ids = [];
        ids.push(seq);
      }

      this.download(ids);
    },
    onRefresh() {
      this.getData();
    },
    onDeleteConfirm(id) {
      this.removeRowId = id;
      this.$bvModal.show('modalRemove');
    },
    onDelete() {
      const userExtId = sessionStorage.getItem('user_ext_id');
      let ids = this.selectedIds;

      if (typeof this.removeRowId) {
        ids = [];
        ids.push(this.removeRowId);
        this.removeRowId = null;
      }

      ids.forEach(id => {
        this.$http.delete(`/api/products/workspace/private/meta/${userExtId}/${id}`)
          .then(res => {
            if (res.status === 200 && !res.data.errorMsg) {
              this.$fn.notify('success', { message: '휴지통 이동 성공' })
              this.$bvModal.hide('modalRemove');
              this.getData();
            } else {
              this.$fn.notify('error', { message: '휴지통 이동 실패' })
            }
        });  
      });
    },
    onChangeRowPerpage(value) {
      this.$refs.scrollPaging.init();
      this.searchItems.selectPage = 1;
      this.searchItems.rowPerPage = value;
      this.getData();
    },
    onPrivateModifyPopup(rowData) {
      this.$refs.refMetaDataModifyPopup.setData(rowData);
      this.metaDataModifyPopup = true;
    },
    onEditSuccess() {
      this.getData();
    }
  }
}
</script>