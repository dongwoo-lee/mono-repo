<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="프로그램" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      :isDisplayPageSize="true"
      @changeRowPerpage="onChangeRowPerpage"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 방송일 -->
        <b-form-group label="방송일" class="has-float-label">
          <common-date-picker @input="onSearch" v-model="searchItems.brd_dt" />
        </b-form-group>
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            class="width-120"
            v-model="searchItems.media"
            :options="mediaOptions"
            value-field="id"
            text-field="name"
            @change="mediaReset"
          />
        </b-form-group>
        <!-- 프로그램 -->
        <b-form-group label="프로그램" class="has-float-label">
          <common-vue-select
            style="width:220px;"
            :suggestions="pgmOptions"
            :vSelectProps="vSelectProps"
            @inputEvent="onPgmSelected"
          ></common-vue-select>
        </b-form-group>
        <!-- 제작자 -->
        <b-form-group label="제작자" class="has-float-label">
          <common-vue-select
            :suggestions="editorOptions"
            @inputEvent="onEditorSelected"
          ></common-vue-select>
        </b-form-group>
        <!-- 검색버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >검색</b-button
          >
        </b-form-group>
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
          is-actions-slot
          :isTableLoading="isTableLoading"
          @contextMenuAction="onContextMenuAction"
          @scrollPerPage="onScrollPerPage"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
          <CopyToMySpacePopup
            ref="refCopyToMySpacePopup"
            :show="copyToMySpacePopup"
            @ok="onMyDiskCopyFromProduct"
            @close="copyToMySpacePopup = false"
          >
          </CopyToMySpacePopup>
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :downloadName="downloadName(props.props.rowData)"
              :behaviorData="behaviorList"
              :etcData="['delete', 'modify']"
              @preview="onPreview"
              @download="onDownloadProduct"
              @mydiskCopy="onCopyToMySpacePopup"
              @modify="onMetaModifyPopup"
              @delete="onDeleteConfirm"
            >
            </common-actions>
          </template>
        </common-data-table-scroll-paging>
      </template>
    </common-form>

    <PlayerPopup
      :showPlayerPopup="showPlayerPopup"
      :title="soundItem.name"
      :fileKey="soundItem.fileToken"
      :streamingUrl="streamingUrl"
      :waveformUrl="waveformUrl"
      :tempDownloadUrl="tempDownloadUrl"
      requestType="token"
      @closePlayer="onClosePlayer"
    >
    </PlayerPopup>
  </div>
</template>

<script>
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CopyToMySpacePopup from "../../../components/Popup/CopyToMySpacePopup";
import CommonVueSelect from "../../../components/Form/CommonVueSelect.vue";
import axios from "axios";
export default {
  components: { CopyToMySpacePopup, CommonVueSelect },
  mixins: [MixinBasicPage],
  data() {
    return {
      vSelectProps: {},
      searchItems: {
        media: "A",
        brd_dt: "",
        pgm: "",
        rowPerPage: 30,
        selectPage: 1,
        sortKey: "",
        sortValue: ""
      },
      isTableLoading: false,
      fields: [
        {
          name: "rowNO",
          title: "순서",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "4%"
        },
        {
          name: "mediaName",
          title: "매체",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "5%",
          sortField: "mediaName"
        },
        {
          name: "name",
          title: "프로그램명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "name"
        },
        {
          name: "brdDT",
          title: "방송일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
          sortField: "brdDT",
          callback: v => {
            return this.$fn.dateStringTohaipun(v);
          }
        },
        {
          name: "brdTime",
          title: "방송시간",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "7%",
          sortField: "brdTime"
        },
        {
          name: "status",
          title: "상태",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
          sortField: "status",
          callback: v => {
            if (v === "제작중") {
              return `<span style="color:red; font-weight:600;">${v}</span>`;
            }
            return v;
          }
        },
        {
          name: "duration",
          title: "길이(초)",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
          sortField: "duration"
        },
        {
          name: "editorName",
          title: "제작자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "8%",
          sortField: "editorName"
        },
        {
          name: "editDtm",
          title: "최종편집일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%",
          sortField: "editDtm"
        },
        {
          name: "reqCompleteDtm",
          title: "방송의뢰일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%",
          sortField: "reqCompleteDtm"
        },
        {
          name: "__slot:actions",
          title: "추가작업",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        }
      ],
      contextMenu: [
        { name: "storage", text: "내 저장공간으로 복사" },
        { name: "download", text: "다운로드" }
      ]
    };
  },
  watch: {
    ["searchItems.brd_dt"](v) {
      if (v) {
        // 사용처 조회
        this.getPgmOptions(this.searchItems.brd_dt, this.searchItems.media);
        //NOTE: 프로그램 두번 검색하는 쿼리 삭제
        // setTimeout(() => {
        //   this.onSearch();
        // }, 500);
      }
    },
    ["searchItems.media"](v) {
      if (v) {
        // 사용처 조회
        this.getPgmOptions(this.searchItems.brd_dt, this.searchItems.media);
        //NOTE: 프로그램 두번 검색하는 쿼리 삭제
        // setTimeout(() => {
        //   this.onSearch();
        // }, 500);
      }
    }
  },
  created() {
    // 매체목록 조회
    this.getMediaOptions();
    // 제작자(PD용) 조회
    this.getEditorForPd();
    this.$nextTick(() => {
      this.getData();
    });
  },
  methods: {
    mediaReset() {
      this.searchItems.pgm = null;
      this.searchItems.pgmName = null;
      this.vSelectProps = { id: null, name: null };
      this.onSearch();
    },
    getData() {
      this.isTableLoading = this.isScrollLodaing ? false : true;
      const media = this.searchItems.media;

      this.$http
        .get(`/api/products/pgm/${media}`, { params: this.searchItems })
        .then(res => {
          this.setResponseData(res);
          this.isTableLoading = false;
          this.isScrollLodaing = false;
        });
    },
    downloadName(rowData) {
      var tmpName = `${rowData.name}_${rowData.brdDT}`;
      return tmpName;
    },
    onDeleteConfirm(rowData) {
      console.log(rowData);
    },

    // 휴지통 보내기
    onDelete() {
      const userId = sessionStorage.getItem(USER_ID);
      let ids = this.selectedIds;

      if (this.singleSelectedId !== null) {
        ids = [];
        ids.push(this.singleSelectedId);
        this.singleSelectedId = null;
        this.selectedIds = [];
      }

      this.$http
        .delete(`/api/products/workspace/private/meta/${userId}/${ids}`)
        .then(res => {
          if (res.status === 200 && !res.data.errorMsg) {
            this.$fn.notify("primary", {
              message: "휴지통으로 이동되었습니다."
            });
            this.$bvModal.hide("modalRemove");
            setTimeout(() => {
              this.initSelectedIds();
              this.getData();
            }, 0);
          } else {
            this.$fn.notify("error", {
              message: "휴지통 이동 실패: " + res.data.errorMsg
            });
          }
        });
    },
    onMetaModifyPopup(rowData) {
      var body = {
        AudioFileID: rowData.id,
        Title: rowData.name
      };
      axios.patch("/api/Mastering/program", body).then(res => {
        console.log(res);
      });
    },
    onEditSuccess() {
      this.getData();
    }
  }
};
</script>
