<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="휴지통" />
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
        <!-- 제목 -->
        <b-form-group label="제목" class="has-float-label c-zindex">
          <common-input-text
            v-model="searchItems.title"
            @inputEnterEvent="onSearch"
          />
        </b-form-group>
        <!-- 메모 -->
        <b-form-group label="메모" class="has-float-label c-zindex">
          <common-input-text
            v-model="searchItems.memo"
            @inputEnterEvent="onSearch"
          />
        </b-form-group>
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >검색</b-button
          >
        </b-form-group>
      </template>
      <!-- 버튼 -->
      <template slot="form-btn-area">
        <b-input-group>
          <b-button
            variant="outline-primary default"
            size="sm"
            @click="onMultiDeleteConfirm"
            >선택항목 삭제</b-button
          >
        </b-input-group>
        <b-input-group>
          <b-button
            variant="outline-secondary default"
            size="sm"
            @click="onMultiRecycleConfirm"
            >선택항목 복원</b-button
          >
        </b-input-group>
        <b-input-group>
          <b-button
            variant="outline-danger default"
            size="sm"
            @click="onRecyclebinConfirm"
            >휴지통 비우기</b-button
          >
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
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          :totalCount="responseData.totalRowCount"
          :is-actions-slot="true"
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @selectedIds="onSelectedIds"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <b-colxx>
              <b-button
                class="icon-buton"
                title="복원"
                @click.stop="onRecycleConfirm(props.props.rowData)"
              >
                <b-icon icon="arrow-counterclockwise" class="icon"></b-icon>
              </b-button>
              <b-button
                class="icon-buton"
                title="영구삭제"
                @click.stop="onDeleteConfirm(props.props.rowData)"
              >
                <b-icon icon="x-octagon" class="icon" variant="danger"></b-icon>
              </b-button>
            </b-colxx>
          </template>
        </common-data-table-scroll-paging>
      </template>
      <!-- 알림 -->
      <template slot="form-confirm-area">
        <!-- 영구삭제 확인창 -->
        <common-confirm
          id="modalRemove"
          title="영구삭제"
          :message="getDeleteMsg()"
          submitBtn="영구삭제"
          :deleteState="deleteState"
          @ok="onDelete()"
        />
        <!-- 복원 확인창 -->
        <common-confirm
          id="modalRecycle"
          title="복원"
          :message="getMoveRecycleMsg()"
          submitBtn="복원"
          @ok="onRecycle()"
        />
        <!-- 휴지통 비우기 확인창 -->
        <common-confirm
          id="modalRecyclebin"
          title="휴지통 비우기"
          message="휴지통을 비우시겠습니까?<br>(파일은 영구삭제되며 되돌릴 수 없습니다.)"
          submitBtn="휴지통 비우기"
          :deleteState="deleteState"
          @ok="onRecyclebin()"
        />
      </template>
    </common-form>
  </div>
</template>

<script>
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import { USER_ID } from "@/constants/config";
import { mapActions } from "vuex";

export default {
  mixins: [MixinBasicPage],
  data() {
    return {
      deleteState: false,
      searchItems: {
        // cate: '',           // 분류
        title: "", // 제목
        memo: "", // 메모
        rowPerPage: 30,
        selectPage: 1,
        sortKey: "deletedDtm",
        sortValue: "desc",
      },
      singleSelectedId: null,
      recycleId: null,
      isTableLoading: false,
      innerHtmlSelectedFileNames: "",
      fields: [
        {
          name: "__checkbox",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "5%",
        },
        {
          name: "rowNO",
          title: "순서",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "4%",
        },
        {
          name: "title",
          title: "제목",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
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
        },
        {
          name: "audioFormat",
          title: "오디오포맷",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "9%",
          sortField: "audioFormat",
        },
        {
          name: "deletedDtm",
          title: "삭제일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          sortField: "deletedDtm",
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
  created() {
    this.$nextTick(() => {
      this.getData();
    });
  },
  methods: {
    ...mapActions("user", ["getSummaryUser"]),
    getData() {
      //this.selectedIds = [];
      this.isTableLoading = this.isScrollLodaing ? false : true;
      const userId = sessionStorage.getItem(USER_ID);
      this.$http
        .get(`/api/products/workspace/private/recyclebin/${userId}`, {
          params: this.searchItems,
        })
        .then((res) => {
          this.setResponseData(res);
          this.addScrollClass();
          this.isTableLoading = false;
          this.isScrollLodaing = false;
        });
    },
    // 단일 영구 삭제 확인창
    onDeleteConfirm(rowData) {
      console.info("rowData", rowData);
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
      this.deleteState = true;
      const userId = sessionStorage.getItem(USER_ID);
      let ids = this.selectedIds;

      if (this.singleSelectedId) {
        ids = [];
        ids.push(this.singleSelectedId);
        this.singleSelectedId = null;
      }

      this.$http
        .delete(`/api/products/workspace/private/recyclebin/${userId}/${ids}`)
        .then((res) => {
          if (res.status === 200 && !res.data.errorMsg) {
            this.$fn.notify("primary", { message: "파일이 삭제 되었습니다." });
            this.$bvModal.hide("modalRemove");
            this.deleteState = false;
            this.initSelectedIds();
            this.getSummaryUser();
            this.onSearch();
          } else {
            this.$fn.notify("error", {
              message: "삭제 실패: " + res.data.errorMsg,
            });
          }
        });
    },
    // 단일 복원 확인창
    onRecycleConfirm(rowData) {
      this.recycleId = rowData.seq;
      this.innerHtmlSelectedFileNames = this.getInnerHtmlSelectdFileNames(
        rowData.title
      );
      this.$bvModal.show("modalRecycle");
    },
    // 선택항목 복원 확인창
    onMultiRecycleConfirm() {
      if (this.isNoSelected()) return;
      this.innerHtmlSelectedFileNames =
        this.getInnerHtmlSelectdFileNamesFromMulti(
          this.selectedIds,
          this.responseData.data
        );
      this.$bvModal.show("modalRecycle");
    },
    // 복원하기
    onRecycle() {
      const userId = sessionStorage.getItem(USER_ID);
      let ids = this.selectedIds;

      if (this.recycleId) {
        ids = [];
        ids.push(this.recycleId);
        this.recycleId = null;
      }

      this.$http
        .put(
          `/api/products/workspace/private/recyclebin/${userId}/recycle/${ids}`
        )
        .then((res) => {
          if (res.status === 200 && !res.data.errorMsg) {
            this.$fn.notify("primary", { message: "복원 성공" });
            this.$bvModal.hide("modalRecycle");
            this.initSelectedIds();
            this.onSearch();
          } else {
            this.$fn.notify("error", {
              message: "복원 실패: " + res.data.errorMsg,
            });
          }
          this.recycleId = null;
        });
    },
    // 휴지통 비우기(물리적인파일 포함 전체 영구삭제) 확인창
    onRecyclebinConfirm() {
      this.$bvModal.show("modalRecyclebin");
    },
    // 휴지통 비우기(물리적인파일 포함 전체 영구삭제)
    onRecyclebin() {
      this.deleteState = true;
      const userId = sessionStorage.getItem(USER_ID);

      this.$http
        .delete(`/api/products/workspace/private/recyclebin/${userId}`)
        .then((res) => {
          if (res.status === 200 && !res.data.errorMsg) {
            // this.$fn.notify('primary', { message: '휴지통 비우기가 요청되었습니다.' });
            this.$bvModal.hide("modalRecyclebin");
            this.deleteState = false;
            this.getSummaryUser();
            this.initSelectedIds();
            this.onSearch();
          } else {
            this.$fn.notify("error", {
              message: "휴지통 비우기 실패: " + res.data.errorMsg,
            });
          }
        });
    },
    isNoSelected() {
      return !this.selectedIds || this.selectedIds.length === 0;
    },
    getMoveRecycleMsg() {
      return this.innerHtmlSelectedFileNames + "복원하시겠습니까?";
    },
    getDeleteMsg() {
      return (
        this.innerHtmlSelectedFileNames +
        "파일을 삭제하시겠습니까?<br>(파일은 영구삭제되며 되돌릴 수 없습니다.)"
      );
    },
  },
};
</script>
