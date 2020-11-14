<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="MY 디스크" />
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
        <!-- 등록일: 시작일 -->
        <b-form-group label="시작일"
          class="has-float-label">
          <common-date-picker v-model="searchItems.start_dt" :isCurrentDate="false"/>
        </b-form-group>
      <!-- 등록일: 종료일 -->
        <b-form-group label="종료일" 
          class="has-float-label">
          <common-date-picker v-model="searchItems.end_dt" :isCurrentDate="false"/>
        </b-form-group>
      <!-- 제목 -->
        <b-form-group label="제목" class="has-float-label">
          <common-input-text v-model="searchItems.title" />
        </b-form-group>
      <!-- 메모 -->
        <b-form-group label="메모" class="has-float-label">
          <common-input-text class-string="memo" v-model="searchItems.memo" />
        </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="btn-outline-primary default" @click="onSearch">검색</b-button>
        </b-form-group>
      </template>
      <!-- 버튼 -->
      <template slot="form-btn-area">
        <b-input-group>
          <b-button variant="outline-primary default" size="sm" @click="onShowModalFileUpload">파일 업로드</b-button>
        </b-input-group>
        <b-input-group>
          <b-button variant="outline-secondary default" size="sm" @click="onDownloadMultiple">선택 항목 다운로드</b-button>
        </b-input-group>
        <b-input-group>
          <b-button variant="outline-danger default" size="sm" @click="onMultiDeleteConfirm">선택 항목 휴지통 보내기</b-button>
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
          is-actions-slot
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @selectedIds="onSelectedIds"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
              <common-actions
                :rowData="props.props.rowData"
                :behaviorData="behaviorList"
                :etcData="['delete', 'modify']"
                @preview="onPreview"
                @download="onDownloadSingle"
                @delete="onDeleteConfirm"
                @modify="onMetaModifyPopup"
              >
              </common-actions>
          </template>
        </common-data-table-scroll-paging>
      </template>
      <!-- 알림 -->
      <template slot="form-confirm-area">
        <!-- 휴지통 이동 확인창 -->
        <!-- :message= "휴지통으로 이동하시겠습니까??" -->
        <common-confirm
          id="modalRemove"
          title="휴지통 이동"
          :message="getMoveRecyclebinMsg()"
          submitBtn="이동"
          @ok="onDelete()"
        />
        <!-- My공간 메타데이터 수정 팝업 -->
        <meta-data-private-modify-popup
          ref="refMetaDataModifyPopup"
          :show="metaDataModifyPopup"
          @editSuccess="onEditSuccess"
          @close="metaDataModifyPopup = false">
        </meta-data-private-modify-popup>
      </template>
    </common-form>

    <PlayerPopup 
    :showPlayerPopup="showPlayerPopup"
    :title="soundItem.title"
    :fileKey="soundItem.seq"
    :streamingUrl="streamingUrl"
    :waveformUrl="waveformUrl"
    requestType="key"
    direct ="Y"
    @closePlayer="onClosePlayer">
    </PlayerPopup>

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
      streamingUrl : '/api/products/workspace/private/streaming',
      waveformUrl : '/api/products/workspace/private/waveform',
      searchItems: {
        cate: '',              // 분류(cate)
        title: '',             // 제목
        memo: '',              // 메모(memo)
        start_dt: '',          // 등록일 시작일
        end_dt: '',            // 등록일 종료일
        rowPerPage: 15,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      metaDataModifyPopup: false,
      singleSelectedId: null,
      isTableLoading: false,
      innerHtmlSelectedFileNames: '',
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
        {
          name: "fileExt",
          title: "파일형식",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%"
        },
         {
          name: "fileSize",
          title: "파일사이즈",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
          callback: (v) => {
            return this.$fn.formatBytes(v)
          }
        },
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
          sortField: 'editedDtm',
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
    }
  },
  methods: {
    ...mapActions('file', ['open_popup']),
    getData() {
      if (this.$fn.checkGreaterStartDate(this.searchItems.start_dt, this.searchItems.end_dt)) {
        this.$fn.notify('error', { message: '시작 날짜가 종료 날짜보다 큽니다.' });
        this.hasErrorClass = true;
        return;
      }

      this.selectedIds = [];
      this.isTableLoading = this.isScrollLodaing ? false: true;
      const userExtId = sessionStorage.getItem('user_ext_id');

      this.$http.get(`/api/products/workspace/private/meta/${userExtId}`, { params: this.searchItems })
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
    onDownloadSingle(item) {
      let ids = [];
      ids.push(item.seq);
      this.downloadWorkspace({ids: ids, type: 'private'});
    },
    onDownloadMultiple() {
      let ids = this.selectedIds;
      this.downloadWorkspace({ids: ids, type: 'private'});
    },
    // 단일 휴지통 보내기 확인창
    onDeleteConfirm(rowData) {
      this.singleSelectedId = rowData.seq;
      this.innerHtmlSelectedFileNames = this.getInnerHtmlSelectdFileNames(rowData.title);
      this.$bvModal.show('modalRemove');
    },
    // 선택항목 휴지통 보내기 확인창 
    onMultiDeleteConfirm() {
      if (this.isNoSelected()) return;
      this.innerHtmlSelectedFileNames = this.getInnerHtmlSelectdFileNamesFromMulti(this.selectedIds, this.responseData.data);
      this.$bvModal.show('modalRemove');
    },
    // 휴지통 보내기
    onDelete() {
      const userExtId = sessionStorage.getItem('user_ext_id');
      let ids = this.selectedIds;

      if (this.singleSelectedId !== null) {
        ids = [];
        ids.push(this.singleSelectedId);
        this.singleSelectedId = null;
        this.selectedIds = [];
      }
      
      this.$http.delete(`/api/products/workspace/private/meta/${userExtId}/${ids}`)
        .then(res => {
          if (res.status === 200 && !res.data.errorMsg) {
            this.$fn.notify('success', { message: '휴지통 이동하는데 짧은 시간이 소요됩니다. 새로고침 및 재검색을 해주세요.' })
            this.$bvModal.hide('modalRemove');
            setTimeout(() => {
              this.initSelectedIds();
              this.getData();
            }, 0);
          } else {
            this.$fn.notify('error', { message: '휴지통 이동 실패: ' + res.data.errorMsg })
          }
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
    },
    getMoveRecyclebinMsg() {
      return this.innerHtmlSelectedFileNames + "휴지통으로 이동하시겠습니까?";
    },
  },
}
</script>