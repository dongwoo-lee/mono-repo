<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="공유소재" />
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
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
          class="width-100"
          v-model="searchItems.media"
          :options="mediaPrimaryOptions"
          value-field="id"
          text-field="name" 
        />
        </b-form-group>
        <!-- 분류 -->
        <b-form-group label="분류" class="has-float-label">
          <common-dropdown-menu-input 
            classString="width-120" 
            :isLoadingClass="isLoadingClass"
            :suggestions="publicCodesOptions" 
            @selected="onPublicCodesSelected"
          />
        </b-form-group>
        <!-- 시작일 -->
        <b-form-group label="시작일" class="has-float-label">
          <common-date-picker v-model="$v.searchItems.start_dt.$model" />
          <b-form-invalid-feedback
            :state="$v.searchItems.start_dt.check_date"
          >날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
        </b-form-group>
        <!-- 종료일 -->
        <b-form-group label="종료일" class="has-float-label">
          <common-date-picker v-model="$v.searchItems.end_dt.$model" />
          <b-form-invalid-feedback
            :state="$v.searchItems.end_dt.check_date"
          >날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
        </b-form-group>
        <!-- 제작자 -->
        <b-form-group label="제작자" class="has-float-label">
          <common-dropdown-menu-input :suggestions="editorOptions" @selected="onEditorSelected" />
        </b-form-group>
        <!-- 제목 -->
        <b-form-group label="제목" class="has-float-label">
          <common-input-text v-model="searchItems.title"/>
        </b-form-group>
        <!-- 메모 -->
        <b-form-group label="메모" class="has-float-label">
          <common-input-text v-model="searchItems.memo"/>
        </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch">검색</b-button>
        </b-form-group>
      </template>
      <!-- 버튼 -->
      <template slot="form-btn-area">
        <b-input-group>
          <b-button variant="outline-primary default" size="sm" @click="onShowModalFileUpload">파일 업로드</b-button>
        </b-input-group>
        <b-input-group>
          <b-button variant="outline-secondary default" size="sm" @click="onDownload">선택 항목 다운로드</b-button>
        </b-input-group>
        <b-input-group>
          <b-button variant="outline-danger default" size="sm" @click="onMultiDeleteConfirm">선택 항목 삭제</b-button>
        </b-input-group>
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
          @selectedIds="onSelectedIds"
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
                <b-button variant="default" class="icon-buton" title="삭제" @click.stop="onDeleteConfirm(props.props.rowData.seq)">
                  <b-icon icon="dash-square" class="icon" variant="danger"></b-icon>
                </b-button>
                <b-button variant="default" class="icon-buton" title="정보편집" @click.stop="onMetaModifyPopup(props.props.rowData)">
                  <b-icon icon="exclamation-square" class="icon" variant="info"></b-icon>
                </b-button>
              </b-colxx>
            </b-row>
          </template>
        </common-data-table-scroll-paging>
      </template>
      <!-- 알림 -->
      <template slot="form-confirm-area">
        <!-- 삭제 모달 -->
        <common-confirm
          id="modalRemove"
          title="영구삭제"
          message= "다시 복원할 수 없습니다. 정말 삭제하시겠습니까?"
          submitBtn="영구삭제"
          @ok="onDelete()"
        />
        <!-- 공유소재 메타데이터 수정 팝업 -->
        <meta-data-shared-modify-popup
          ref="refMetaDataModifyPopup"
          :show="metaDataModifyPopup"
          @editSuccess="onEditSuccess"
          @close="metaDataModifyPopup = false">
        </meta-data-shared-modify-popup>
      </template>
    </common-form>
  </div>
</template>

<script>
import MixinBasicPage from '../../../mixin/MixinBasicPage';
import MetaDataSharedModifyPopup from '../../../components/popup/MetaDataSharedModifyPopup';
import { mapActions } from 'vuex';

export default {
  mixins: [ MixinBasicPage ],
  components: { MetaDataSharedModifyPopup },
  data() {
    return {
      searchItems: {
        media: 'A',                // 매체
        cate: '',                  // 분류
        cateName: '',              // 분류명
        start_dt: '20200101',      // 시작일
        end_dt: '',                // 종료일
        type: '',                  // 구분
        editor: '',                // 제작자
        title: '',                 // 제목
        memo: '',                  // 메모
        rowPerPage: 15,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      metaDataModifyPopup: false,
      singleSelectedId: null,
      isTableLoading: false,
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
          width: '5%',
        },
        {
          name: "mediaName",
          title: "매체",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "5%"
        },
        {
          name: "categoryName",
          title: "분류",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
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
          width: "8%"
        },
        {
          name: "audioFormat",
          title: "오디오포맷",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%"
        },
        {
          name: "userName",
          title: "제작자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "editedDtm",
          title: "등록일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%"
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
  watch: {
    ['searchItems.media'](v) {
      this.getPublicCodesOptions(v);
    }
  },
  created() {
    // (구)프로소재, 공유소재 매체 목록 조회
    this.getMediaPrimaryOptions();
    // 사용자 목록 조회
    this.getEditorOptions();
    // 공유 소재 분류 목록 조회
    this.getPublicCodesOptions(this.searchItems.media);
  },
  methods: {
    ...mapActions('file', ['open_popup', 'download']),
    getData() {
      this.isTableLoading = this.isScrollLodaing ? false: true;
      this.$http.get(`/api/products/workspace/public/meta`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
            this.addScrollClass();
            this.isTableLoading = false;
            this.isScrollLodaing = false;
      });
    },
    onShowModalFileUpload() {
      this.open_popup();
    },
    onDownload(seq) {
      let ids = this.selectedIds;

      if (typeof seq !== 'object' && seq) {
        ids = [];
        ids.push(seq);
      }

      this.download({ids: ids, type: 'public'});
    },
    // 단일 영구 삭제 확인창 
    onDeleteConfirm(id) {
      this.singleSelectedId = id;
      this.$bvModal.show('modalRemove');
    },
    // 선택항목 영구 삭제 확인창 
    onMultiDeleteConfirm() {
      if (this.isNoSelected()) return;
      this.$bvModal.show('modalRemove');
    },
    // 영구 삭제
    onDelete() {
      let ids = this.selectedIds;

      if (this.singleSelectedId !== null) {
        ids = [];
        ids.push(this.singleSelectedId);
        this.singleSelectedId = null;
      }

      ids.forEach(seq => {
        this.$http.delete(`/api/products/workspace/public/meta/${seq}`)
          .then(res => {
            if (res.status === 200 && !res.data.errorMsg) {
              this.$fn.notify('success', { message: '삭제하였습니다.' })
              this.$bvModal.hide('modalRemove');
              this.getData();
            } else {
              this.$fn.notify('error', { message: '삭제 실패하였습니다.' })
            }
        });  
      });
    },
    onMetaModifyPopup(rowData) {
      this.$refs.refMetaDataModifyPopup.setData(rowData);
      this.metaDataModifyPopup = true;
    },
    onEditSuccess() {
      this.getData();
    },
    isNoSelected() {
      return !this.selectedIds || this.selectedIds.length === 0;
    }
  }
}
</script>