<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="My공간" />
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
          class="has-float-label"
          :class="{ 'hasError': (hasErrorClass || $v.searchItems.start_dt.$error) }">
          <common-date-picker 
            v-model="$v.searchItems.start_dt.$model" 
            :isCurrentDate="false"
            :dayAgo="2"
          />
        </b-form-group>
      <!-- 등록일: 종료일 -->
        <b-form-group label="종료일" 
          class="has-float-label"
          :class="{ 'hasError': (hasErrorClass || $v.searchItems.end_dt.$error) }">
          <common-date-picker
            v-model="$v.searchItems.end_dt.$model"
            :isCurrentDate="true"
          />
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
          <b-button variant="outline-primary default" @click="onSearch">검색</b-button>
        </b-form-group>
      </template>
      <!-- 버튼 -->
      <template slot="form-btn-area">
        <b-input-group>
          <b-button variant="outline-primary default" size="sm" @click="onShowModalFileUpload">파일 업로드</b-button>
        </b-input-group>
        <b-input-group>
          <b-button variant="outline-secondary default" size="sm" @click="onSelectedDownload">선택 항목 다운로드</b-button>
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
          :is-actions-slot="true"
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @selectedIds="onSelectedIds"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <b-button 
              v-b-tooltip.hover.top="{ 
                title: 'tooltip!!!',
              }"
              :offsetX="5"
              class="icon-buton">
              <b-icon icon="caret-right-square" class="icon"></b-icon>
            </b-button>
            <b-button :id="`download-${props.props.rowIndex}`" class="icon-buton"
              @click.stop="onDownload(props.props.rowData.seq)">
              <b-icon icon="download" class="icon"></b-icon>
            </b-button>
            <b-tooltip
              :target="`download-${props.props.rowIndex}`"
              placement="top"
              :offsetY="30">
              <!-- {{props.props.rowData.filePath}} -->
              {{'download-' + props.props.rowIndex}}
            </b-tooltip>
            <b-button class="icon-buton" title="휴지통" @click.stop="onDeleteConfirm(props.props.rowData.seq)">
              <b-icon icon="dash-square" class="icon" variant="danger"></b-icon>
            </b-button>
            <b-button class="icon-buton" title="정보편집" @click.stop="onMetaModifyPopup(props.props.rowData)">
              <b-icon icon="exclamation-square" class="icon" variant="info"></b-icon>
            </b-button>
          </template>
        </common-data-table-scroll-paging>
      </template>
      <!-- 알림 -->
      <template slot="form-confirm-area">
        <!-- 휴지통 이동 확인창 -->
        <common-confirm
          id="modalRemove"
          title="휴지통 이동"
          message= "휴지통으로 이동하시겠습니까??"
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
    ...mapActions('file', ['open_popup', 'download']),
    getData() {
      if (this.$fn.checkGreaterStartDate(this.searchItems.start_dt, this.searchItems.end_dt)) {
        this.$fn.notify('error', { message: '시작 날짜가 종료 날짜보다 큽니다.' });
        this.hasErrorClass = true;
        return;
      }

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
    // 선택항목 다운로드
    onSelectedDownload() {
      if (this.isNoSelected()) return;
      this.onDownload();
    },
    // 단일 다운로드
    onDownload(seq) {
      let ids = this.selectedIds;

      if (typeof seq !== 'object' && seq) {
        ids = [];
        ids.push(seq);
      }

      this.download({ids: ids, type: 'private'});
    },
    // 단일 휴지통 보내기 확인창
    onDeleteConfirm(id) {
      this.singleSelectedId = id;
      this.$bvModal.show('modalRemove');
    },
    // 선택항목 휴지통 보내기 확인창 
    onMultiDeleteConfirm() {
      if (this.isNoSelected()) return;
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
      }
      
      this.$http.delete(`/api/products/workspace/private/meta/${userExtId}/${ids}`)
        .then(res => {
          if (res.status === 200 && !res.data.errorMsg) {
            this.$fn.notify('success', { message: '휴지통 이동 성공' })
            this.$bvModal.hide('modalRemove');
            this.getData();
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
    }
  }
}
</script>