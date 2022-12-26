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
        <!-- 시작일 ~ 종료일 -->
        <common-start-end-date-picker
          :startDate.sync="searchItems.start_dt"
          :endDate.sync="searchItems.end_dt"
          :required="false"
          :isCurrentDate="false"
          @SEDateEvent="onSearch"
        />
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            class="width-100"
            v-model="searchItems.media"
            :options="mediaPrimaryOptions"
            value-field="id"
            text-field="name"
            @change="onSearch"
          />
        </b-form-group>
        <!-- 분류 -->
        <b-form-group label="분류" class="has-float-label">
          <b-form-select
            class="width-120"
            v-model="searchItems.cate"
            :options="publicCodesOptions"
            value-field="id"
            text-field="name"
            :disabled="publicCodesOptions.length === 0"
            @change="onSearch"
          />
        </b-form-group>
        <!-- 제작자 -->
        <b-form-group label="제작자" class="has-float-label">
          <common-vue-select
            :suggestions="editorOptions"
            @inputEvent="onEditorSelected"
            @blurEvent="onSearch"
          ></common-vue-select>
        </b-form-group>
        <!-- 제목 -->
        <b-form-group label="제목" class="has-float-label">
          <common-input-text
            v-model="searchItems.title"
            @inputEnterEvent="onSearch"
          />
        </b-form-group>
        <!-- 메모 -->
        <b-form-group label="메모" class="has-float-label">
          <common-input-text
            v-model="searchItems.memo"
            @inputEnterEvent="onSearch"
          />
        </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >검색</b-button
          >
        </b-form-group>
      </template>
      <!-- 버튼 -->
      <!-- <template slot="form-btn-area">
        <b-input-group>
          <b-button
            variant="outline-primary default"
            size="sm"
            @click="onShowModalFileUpload"
            >파일 업로드</b-button
          >
        </b-input-group>
        <b-input-group>
          <b-button
            variant="outline-secondary default"
            size="sm"
            @click="onDownloadMultiple"
            >선택 항목 다운로드</b-button
          >
        </b-input-group>
        <b-input-group>
          <b-button
            variant="outline-danger default"
            size="sm"
            @click="onMultiDeleteConfirm"
            >선택 항목 삭제</b-button
          >
        </b-input-group>
      </template> -->
      <!-- 테이블 페이지 -->
      <template slot="form-table-page-area">
        {{ getPageInfo() }}
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <common-data-table-scroll-paging
          ref="scrollPaging"
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          :totalCount="responseData.totalRowCount"
          :is-actions-slot="true"
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @sortableclick="onSortable"
          @selectedIds="onSelectedIds"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :behaviorData="behaviorList"
              :etcData="['delete', 'modify']"
              :etcTitles="{ delete: '삭제' }"
              :isPossibleDelete="isPossibleDelete(props.props.rowData.userId)"
              @preview="onPreview"
              @download="onDownloadSingle"
              @delete="onDeleteConfirm"
              @modify="onMetaModifyPopup"
              @mydiskCopy="onCopyToMySpacePopup"
            >
            </common-actions>
          </template>
        </common-data-table-scroll-paging>
      </template>
      <!-- 알림 -->
      <template slot="form-confirm-area">
        <!-- 삭제 모달 -->
        <common-confirm
          id="modalRemove"
          title="영구삭제"
          :message="getDeleteMsg()"
          submitBtn="영구삭제"
          @ok="onDelete()"
        />
        <!-- 공유소재 메타데이터 수정 팝업 -->
        <meta-data-shared-modify-popup
          ref="refMetaDataModifyPopup"
          :show="metaDataModifyPopup"
          @editSuccess="onEditSuccess"
          @close="metaDataModifyPopup = false"
        >
        </meta-data-shared-modify-popup>

        <CopyToMySpacePopup
          ref="refCopyToMySpacePopup"
          :show="copyToMySpacePopup"
          @ok="onMyDiskCopyFromPublic"
          @close="copyToMySpacePopup = false"
        >
        </CopyToMySpacePopup>
      </template>
    </common-form>

    <PlayerPopup
      :showPlayerPopup="showPlayerPopup"
      :title="soundItem.title"
      :fileKey="soundItem.seq"
      :streamingUrl="streamingUrl"
      :waveformUrl="waveformUrl"
      :tempDownloadUrl="tempDownloadUrl"
      requestType="key"
      direct="Y"
      @closePlayer="onClosePlayer"
    >
    </PlayerPopup>
  </div>
</template>

<script>
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import MetaDataSharedModifyPopup from "../../../components/Popup/MetaDataSharedModifyPopup";
import { mapActions } from "vuex";
import { USER_ID } from "@/constants/config";
import CopyToMySpacePopup from "../../../components/Popup/CopyToMySpacePopup";
import CommonVueSelect from "../../../components/Form/CommonVueSelect.vue";

export default {
  mixins: [MixinBasicPage],
  components: {
    MetaDataSharedModifyPopup,
    CopyToMySpacePopup,
    CommonVueSelect,
  },
  data() {
    return {
      streamingUrl: "/api/products/workspace/public/streaming",
      waveformUrl: "/api/products/workspace/public/waveform",
      tempDownloadUrl: "/api/products/workspace/public/temp-download",

      searchItems: {
        media: "A", // 매체
        cate: "S01G05C001", // 분류
        cateName: "", // 분류명
        start_dt: "", // 시작일
        end_dt: "", // 종료일
        type: "", // 구분
        editor: "", // 제작자
        title: "", // 제목
        memo: "", // 메모
        rowPerPage: 30,
        selectPage: 1,
        sortKey: "",
        sortValue: "",
      },
      metaDataModifyPopup: false,
      singleSelectedId: null,
      isTableLoading: false,
      innerHtmlSelectedFileNames: "",
      fields: [
        {
          name: "__checkbox",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "3%",
        },
        {
          name: "rowNO",
          title: "순서",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "4%",
        },
        {
          name: "categoryName",
          title: "분류",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%",
          // sortField: 'categoryName'
        },
        {
          name: "title",
          title: "제목",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "title",
        },
        {
          name: "memo",
          title: "메모",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          sortField: "memo",
        },
        {
          name: "fileExt",
          title: "파일형식",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "5%",
        },
        {
          name: "fileSize",
          title: "파일사이즈",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          sortField: "fileSize",
          width: "8%",
          callback: (v) => {
            return this.$fn.formatBytes(v);
          },
        },
        {
          name: "audioFormat",
          title: "오디오포맷",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%",
          sortField: "audioFormat",
        },
        {
          name: "userName",
          title: "제작자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
          sortField: "userName",
        },
        {
          name: "editedDtm",
          title: "등록일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "14%",
          sortField: "editedDtm",
        },
        {
          name: "__slot:actions",
          title: "추가작업",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
        },
      ],
      USER_ID,
    };
  },
  watch: {
    ["searchItems.media"](v) {
      this.getPublicCodesOptions(v);
    },
  },
  created() {
    // 프로소재, 공유소재 매체 목록 조회
    this.getMediaPrimaryOptions();
    // 사용자 목록 조회
    this.getEditorOptions();
    // 공유 소재 분류 목록 조회
    this.getPublicCodesOptions(this.searchItems.media);
  },
  methods: {
    ...mapActions("file", ["open_popup", "downloadWorkspace"]),
    getData() {
      if (
        this.$fn.checkGreaterStartDate(
          this.searchItems.start_dt,
          this.searchItems.end_dt
        )
      ) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        this.hasErrorClass = true;
        return;
      }

      this.selectedIds = [];
      this.isTableLoading = this.isScrollLodaing ? false : true;
      const { media, cate } = this.searchItems;
      this.$http
        .get(`/api/products/workspace/public/meta/${media}/${cate}`, {
          params: this.searchItems,
        })
        .then((res) => {
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
      this.downloadWorkspace({ ids: ids, type: "public" });
    },
    onDownloadMultiple() {
      let ids = this.selectedIds;
      this.downloadWorkspace({ ids: ids, type: "public" });
    },
    // 단일 영구 삭제 확인창
    onDeleteConfirm(rowData) {
      this.singleSelectedId = rowData.seq;
      this.innerHtmlSelectedFileNames = this.getInnerHtmlSelectdFileNames(
        rowData.title
      );
      this.$bvModal.show("modalRemove");
    },
    // 선택항목 영구 삭제 확인창
    onMultiDeleteConfirm() {
      if (this.isNoSelected()) return;
      this.innerHtmlSelectedFileNames =
        this.getInnerHtmlSelectdFileNamesFromMulti(
          this.selectedIds,
          this.responseData.data
        );
      this.$bvModal.show("modalRemove");
    },
    // 영구 삭제
    onDelete() {
      let ids = this.selectedIds;

      if (this.singleSelectedId !== null) {
        ids = [];
        ids.push(this.singleSelectedId);
        this.singleSelectedId = null;
      }

      ids.forEach((seq) => {
        this.$http
          .delete(`/api/products/workspace/public/meta/${seq}`)
          .then((res) => {
            if (res.status === 200 && !res.data.errorMsg) {
              this.$fn.notify("primary", {
                message: "파일을 삭제 하였습니다.",
              });
              this.$bvModal.hide("modalRemove");
              setTimeout(() => {
                this.initSelectedIds();
                this.getData();
              }, 0);
            } else {
              this.$fn.notify("error", { message: "삭제가 실패 하였습니다." });
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
    },
    getDeleteMsg() {
      return (
        this.innerHtmlSelectedFileNames +
        "다시 복원할 수 없습니다. 정말 삭제하시겠습니까?"
      );
    },
    isPossibleDelete(userId) {
      return sessionStorage.getItem(USER_ID) === userId;
    },
  },
};
</script>
