<template>
  <div>
    <!-- nav -->
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="공유소재" />
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
                <!-- 매체 -->
                <b-colxx sm="2">
                  <b-form-group label="매체" class="has-float-label">
                    <b-form-select 
                    v-model="searchItems.media"
                    :options="mediaPrimaryOptions"
                    value-field="id"
                    text-field="name" 
                  />
                  </b-form-group>
                </b-colxx>
                <!-- 분류 -->
                <b-colxx sm="2">
                  <b-form-group label="분류" class="has-float-label">
                    <c-dropdown-menu-input :suggestions="publicOptions" @selected="onPublicSelected" />
                  </b-form-group>
                </b-colxx>
                <!-- 시작일 -->
                <b-colxx sm="2">
                  <b-form-group label="시작일" class="has-float-label c-zindex">
                      <common-date-picker v-model="$v.searchItems.start_dt.$model"/>
                      <b-form-invalid-feedback :state="$v.searchItems.start_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                  </b-form-group>
                </b-colxx>
                <!-- 종료일 -->
                <b-colxx sm="2">
                  <b-form-group label="종료일" class="has-float-label c-zindex">
                      <common-date-picker v-model="$v.searchItems.end_dt.$model"/>
                      <b-form-invalid-feedback :state="$v.searchItems.end_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                  </b-form-group>
                </b-colxx>
                <!-- 제작자 -->
                <b-colxx sm="2">
                  <b-form-group label="제작자" class="has-float-label">
                    <c-dropdown-menu-input :suggestions="editorOptions" @selected="onEditorSelected" />
                  </b-form-group>
                </b-colxx>
                <!-- 제목 -->
                <b-colxx sm="2">
                  <b-form-group label="제목" class="has-float-label">
                    <c-input-text v-model="searchItems.title"/>
                  </b-form-group>
                </b-colxx>
                <!-- 메모 -->
                <b-colxx sm="2">
                  <b-form-group label="메모" class="has-float-label">
                    <c-input-text v-model="searchItems.memo"/>
                  </b-form-group>
                </b-colxx>
                <b-colxx sm="2">
                  <b-button class="mb-1" variant="outline-primary default" size="sm" @click="getData">검색</b-button>
                </b-colxx>
              </b-row>
            </b-form>
          </div>
        </b-container>

        <!-- 버튼 | 페이지 정보 영역 -->
        <b-container fluid class="text-center mb-1">
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
                  <b-button class="mb-1" variant="outline-danger default" size="sm">선택 항목 삭제</b-button>
                </b-input-group>
               </b-form>
            </b-col>
            <b-col cols="auto" class="pt-3">
              <div class="page-info-group">
                <div class="page-info">
                  {{ getPageInfo() }}
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
        </c-data-table-scroll-paging>
      </b-card>
    </b-row>
    <!-- 삭제 모달 -->
    <common-confirm
      id="modalRemove"
      title="영구삭제"
      message= "영구적으로 삭제하시겠습니까?"
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
          name: "catetoryName",
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
  created() {
    // (구)프로소재, 공유소재 매체 목록 조회
    this.getMediaPrimaryOptions();
    // 사용자 목록 조회
    this.getEditorOptions();
    // 공유 소재 분류 목록 조회
    this.getPublicOptions(this.media);
  },
  methods: {
    ...mapActions('file', ['open_popup', 'download']),
    getData() {
      this.$http.get(`/api/products/workspace/public/meta`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
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
    onDeleteConfirm(id) {
      this.removeRowId = id;
      this.$bvModal.show('modalRemove');
    },
    onDelete() {
      let ids = this.selectedIds;

      if (typeof this.removeRowId) {
        ids = [];
        ids.push(this.removeRowId);
        this.removeRowId = null;
      }

      ids.forEach(seq => {
        this.$http.delete(`/api/products/workspace/public/meta/${seq}`)
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
    onMetaModifyPopup(rowData) {
      this.$refs.refMetaDataModifyPopup.setData(rowData);
      this.metaDataModifyPopup = true;
    },
    onEditSuccess() {
      this.getData();
    }
  }
}
</script>