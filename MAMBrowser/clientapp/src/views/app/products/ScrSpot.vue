<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="부조 SPOT" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      @changeRowPerpage="onChangeRowPerpage"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 시작일 ~ 종료일 -->
        <common-start-end-date-picker
          :startDate.sync="searchItems.start_dt"
          :startDayAgo="7"
          :maxPeriodMonth="3"
          :endDate.sync="searchItems.end_dt"
          :required="true"
          @SEDateEvent="onSearch"
          @SDateError="SDateErrorLog"
        />
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            class="width-120"
            v-model="searchItems.media"
            :options="mediaOptions"
            value-field="id"
            text-field="name"
            @change="onSearch"
          />
        </b-form-group>
        <!-- 사용처명 -->
        <b-form-group label="사용처명" class="has-float-label">
          <common-input-text
            v-model="searchItems.pgmName"
            @inputEnterEvent="onSearch"
          />
        </b-form-group>
        <!-- 제작자 -->
        <b-form-group label="제작자" class="has-float-label">
          <common-vue-select
            :suggestions="editorOptions"
            @inputEvent="onEditorSelected"
          ></common-vue-select>
        </b-form-group>
        <!-- 소재명 -->
        <b-form-group label="소재명" class="has-float-label">
          <common-input-text
            v-model="searchItems.name"
            @inputEnterEvent="onSearch"
          />
        </b-form-group>
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >검색</b-button
          >
        </b-form-group>
      </template>
      <template slot="form-btn-area">
        <b-input-group>
          <b-button
            variant="outline-primary default"
            size="sm"
            @click="showDuration"
            >방송 의뢰</b-button
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
          tableHeight="525px"
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          :totalCount="responseData.totalRowCount"
          is-actions-slot
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :downloadName="downloadName(props.props.rowData)"
              :behaviorData="behaviorList"
              :etcData="['delete', 'modify']"
              :isPossibleUpdate="authorityCheck(props.props.rowData)"
              :isPossibleDelete="authorityCheck(props.props.rowData)"
              @preview="onPreview"
              @download="onDownloadProduct"
              @mydiskCopy="onCopyToMySpacePopup"
              @modify="onMetaUpdatePopup"
              @MasteringDelete="onMetaDeletePopup"
            >
            </common-actions>
          </template>
        </common-data-table-scroll-paging>

        <CopyToMySpacePopup
          ref="refCopyToMySpacePopup"
          :show="copyToMySpacePopup"
          :MySpaceScreenName="MySpaceScreenName"
          @ok="onMyDiskCopyFromProduct"
          @close="copyToMySpacePopup = false"
        >
        </CopyToMySpacePopup>
      </template>
    </common-form>

    <!-- 마스터링 메타 데이터 수정 -->
    <transition name="slide-fade">
      <file-update
        v-if="metaUpdate"
        :rowData="rowData"
        :updateScreenName="updateScreenName"
        @updateFile="masteringUpdate"
        @UpdateModalClose="UpdateModalOff"
      ></file-update>
    </transition>

    <!-- 마스터링 파일 삭제 -->
    <transition name="slide-fade">
      <file-delete
        v-if="metaDelete"
        :rowData="rowData"
        :isScrSpot="true"
        @deleteFile="masteringDelete"
        @DeleteModalClose="DeleteModalOff"
      ></file-delete>
    </transition>

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

    <scr-spot-duration
      @setDurationSuccess="setDurationSuccess"
      @setDurationFail="setDurationFail"
      @requestValid="requestValid"
    ></scr-spot-duration>

    <add-duration></add-duration>

    <search-duration></search-duration>
  </div>
</template>

<script>
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CopyToMySpacePopup from "../../../components/Popup/CopyToMySpacePopup";
import CommonVueSelect from "../../../components/Form/CommonVueSelect.vue";
import FileUpdate from "../../../components/FileUpload/FileUpdate/FileUpdate.vue";
import FileDelete from "../../../components/FileUpload/FileUpdate/FileDelete.vue";
import axios from "axios";
import ScrSpotDuration from "./ScrSpotDuration/ScrSpotDuration.vue";
import AddDuration from "./ScrSpotDuration/AddDuration";
import SearchDuration from "./ScrSpotDuration/SearchScrSpot.vue";
import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
export default {
  components: {
    CopyToMySpacePopup,
    CommonVueSelect,
    FileUpdate,
    FileDelete,
    ScrSpotDuration,
    AddDuration,
    SearchDuration,
  },
  mixins: [MixinBasicPage],
  data() {
    return {
      deleteId: "",
      metaUpdate: false,
      updateScreenName: "",
      rowData: "",
      MySpaceScreenName: "[부조SPOT]",
      metaDelete: false,
      searchItems: {
        start_dt: "", // 시작일
        end_dt: "", // 종료일
        editor: "", // 제작자ID
        editorName: "", // 제작자이름
        name: "", // 소재명
        media: "A", // 매체
        pgmName: "", // 사용처명
        rowPerPage: 30,
        selectPage: 1,
        sortKey: "",
        sortValue: "DESC",
      },
      isTableLoading: false,
      fields: [
        {
          name: "rowNO",
          title: "순서",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "4%",
        },
        {
          name: "name",
          title: "소재명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "name",
        },
        {
          name: "categoryName",
          title: "분류",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "13%",
          sortField: "categoryName",
        },
        {
          name: "duration",
          title: "길이(초)",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%",
          sortField: "duration",
          callback: (v) => {
            return this.$fn.splitFirst(v);
          },
        },
        {
          name: "brdDT",
          title: "방송일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "10%",
          sortField: "brdDT",
          callback: (v) => {
            return this.$fn.dateStringTohaipun(v);
          },
        },
        {
          name: "editorName",
          title: "제작자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "5%",
          sortField: "editorName",
        },
        {
          name: "pgmName",
          title: "사용처명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "pgmName",
        },
        {
          name: "masteringDtm",
          title: "마스터링 일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%",
          sortField: "masteringDtm",
        },
        {
          name: "__slot:actions",
          title: "추가작업",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
        },
      ],
    };
  },
  created() {
    // 매체 목록 조회
    this.getMediaOptions();
    // 사용자 목록 조회
    this.getEditorOptions();

    this.$nextTick(() => {
      this.getData();
    });
  },
  methods: {
    ...mapMutations("ScrSpotDuration", ["showDuration", "hideDuration"]),
    requestValid() {
      this.$fn.notify("info", { title: "기간 설정 데이터를 입력하세요." });
    },
    setDurationSuccess() {
      this.hideDuration();
      this.$fn.notify("primary", {
        title: "부조SPOT 기간설정 성공",
      });
      this.onSearch();
    },
    setDurationFail(v) {
      this.$fn.notify("error", {
        title: v,
      });
    },
    SDateErrorLog() {
      this.$fn.notify("error", {
        message: "시작 날짜가 종료 날짜보다 큽니다.",
      });
      this.hasErrorClass = true;
    },
    authorityCheck(e) {
      if (
        e.editorID == sessionStorage.getItem("user_id") ||
        sessionStorage.getItem("authority") == "ADMIN"
      ) {
        return true;
      } else {
        return false;
      }
    },
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

      this.isTableLoading = this.isScrollLodaing ? false : true;
      this.$http
        .get(`/api/products/spot/scr/${this.searchItems.media}`, {
          params: this.searchItems,
        })
        .then((res) => {
          console.log("res");
          console.log(res);
          this.setResponseData(res);
          this.addScrollClass();
          this.isTableLoading = false;
          this.isScrollLodaing = false;
        });
    },
    downloadName(rowData) {
      var tmpName = `${rowData.name}_${rowData.brdDT}`;
      return tmpName;
    },

    onMetaUpdatePopup(rowData) {
      this.metaUpdate = true;
      this.updateScreenName = "scr-spot";
      this.rowData = rowData;
    },

    UpdateModalOff() {
      this.metaUpdate = false;
    },
    masteringUpdate(e) {
      axios.patch("/api/Mastering/scr-spot", e).then((res) => {
        if (res && res.status === 200 && !res.data.errorMsg) {
          this.UpdateModalOff();
          this.$fn.notify("primary", {
            title: "메타 데이터 수정 성공",
          });
          this.onSearch();
        } else {
          this.UpdateModalOff();
          $fn.notify("error", {
            message: "파일 업로드 실패: " + res.data.errorMsg,
          });
          this.onSearch();
        }
      });
    },
    DeleteModalOff() {
      this.metaDelete = false;
    },
    onMetaDeletePopup(rowData) {
      this.metaDelete = true;
      this.rowData = rowData;
    },
    masteringDelete(e) {
      axios
        .delete(
          `/api/Mastering/scr-spot?spotID=${e.spotID}&productID=${e.productID}&brdDT=${e.brdDT}&filetoken=${e.fileToken}`, {
          headers : { 
            'Content-Type': 'application/json',
            'X-Csrf-Token': sessionStorage.getItem('access_token'),
          }
        })
        .then((res) => {
          if (res && res.status === 200 && !res.data.errorMsg) {
            this.DeleteModalOff();
            this.$fn.notify("primary", {
              title: "파일 삭제 성공",
            });
            this.onSearch();
          } else {
            this.UpdateModalOff();
            $fn.notify("error", {
              message: "파일 삭제 실패: " + res.data.errorMsg,
            });
            this.onSearch();
          }
        });
    },
  },
};
</script>
<style>
.scr-modal-footer {
  padding-left: 1.5rem !important;
}
</style>
