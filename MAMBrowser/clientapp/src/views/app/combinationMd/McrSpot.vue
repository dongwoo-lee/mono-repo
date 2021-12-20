<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="주조SPOT" />
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
            :options="mcrSpotMediaOptions"
            value-field="id"
            text-field="name"
            @change="mediaReset"
          />
        </b-form-group>
        <!-- 사용처 -->
        <b-form-group label="사용처" class="has-float-label">
          <common-vue-select
            style="width: 220px"
            :suggestions="spotOptions"
            :vSelectProps="vSelectProps"
            :vChangedProps="vChangedProps"
            @inputEvent="onSpotSelected"
            @propsChanged="propsChanged"
          ></common-vue-select>
        </b-form-group>
        <!-- 상태 -->
        <!-- <b-form-group label="상태" class="has-float-label">
          <b-form-select
            class="width-140"
            v-model="searchItems.status"
            :options="reqStatusOptions"
            value-field="id"
            text-field="name"
          >
            <template v-slot:first>
              <b-form-select-option value="">선택해주세요.</b-form-select-option>
            </template>
          </b-form-select>
        </b-form-group> -->
        <!-- 제작자 -->
        <b-form-group label="제작자" class="has-float-label">
          <common-vue-select
            style="width: 180px"
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
          is-actions-slot
          :per-page="responseData.rowPerPage"
          :totalCount="responseData.totalRowCount"
          :num-rows-to-bottom="numRowsToBottom"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @sortableclick="onSortable"
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
  </div>
</template>

<script>
import MixinFillerPage from "../../../mixin/MixinFillerPage";
import CopyToMySpacePopup from "../../../components/Popup/CopyToMySpacePopup";
import CommonVueSelect from "../../../components/Form/CommonVueSelect.vue";
import FileUpdate from "../../../components/FileUpload/FileUpdate/FileUpdate.vue";
import FileDelete from "../../../components/FileUpload/FileUpdate/FileDelete.vue";
import axios from "axios";
export default {
  components: { CopyToMySpacePopup, CommonVueSelect, FileUpdate, FileDelete },
  mixins: [MixinFillerPage],
  data() {
    return {
      deleteId: "",
      metaUpdate: false,
      updateScreenName: "",
      rowData: "",
      MySpaceScreenName: "[주조SPOT]",
      metaDelete: false,
      vSelectProps: {},
      vChangedProps: false,
      searchItems: {
        media: "A", // 매체
        cate: "", // 분류
        start_dt: "", // 시작일
        end_dt: "", // 종료일
        status: "", // 상태
        editor: "", // 사용자
        editorName: "", // 사용자 이름
        spotId: "", // spotId
        rowPerPage: 30,
        selectPage: 1,
        sortKey: "",
        sortValue: "",
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
          name: "status",
          title: "상태",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "6%",
          sortField: "status",
        },
        {
          name: "duration",
          title: "길이(초)",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
          sortField: "duration",
        },
        {
          name: "editorName",
          title: "제작자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
          sortField: "editorName",
        },
        {
          name: "editDtm",
          title: "최종편집일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%",
          sortField: "editDtm",
        },
        {
          name: "reqCompleteDtm",
          title: "방송의뢰일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "14%",
          sortField: "reqCompleteDtm",
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
    // 제작자(Md) 목록 조회
    this.getEditorForMd();
    // 주조 spot 분류 목록 조회
    this.getSpotOptions(this.searchItems.media);
    // 주조SPOT 매체 목록 조회
    this.getmcrSpotMediaOptions();
  },
  methods: {
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
    SDateErrorLog() {
      this.$fn.notify("error", {
        message: "시작 날짜가 종료 날짜보다 큽니다.",
      });
      this.hasErrorClass = true;
    },
    propsChanged() {
      this.vChangedProps = false;
    },
    mediaReset() {
      this.getSpotOptions(this.searchItems.media);
      this.searchItems.spotId = null;
      this.searchItems.spotName = null;
      this.vSelectProps = { id: null, name: null };
      this.vChangedProps = true;
      this.onSearch();
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
      }

      this.isTableLoading = this.isScrollLodaing ? false : true;
      const media = this.searchItems.media;

      this.$http
        .get(`/api/products/spot/mcr/${media}`, { params: this.searchItems })
        .then((res) => {
          this.setResponseData(res);
          this.addScrollClass();
          this.isTableLoading = false;
          this.isScrollLodaing = false;
        });
    },
    downloadName(rowData) {
      var tmpName = `${rowData.name}_${rowData.brdDT}_${rowData.mediaName}_${rowData.id}`;
      return tmpName;
    },
    onMetaUpdatePopup(rowData) {
      this.metaUpdate = true;
      this.updateScreenName = "mcr-spot";
      this.rowData = rowData;
    },

    UpdateModalOff() {
      this.metaUpdate = false;
    },
    masteringUpdate(e) {
      axios.patch("/api/Mastering/mcr-spot", e).then((res) => {
        if (res && res.status === 200 && !res.data.errorMsg) {
          this.UpdateModalOff();
          this.$fn.notify("primary", {
            title: "메타 데이터 수정 성공",
          });
          this.getData();
        } else {
          this.UpdateModalOff();
          $fn.notify("error", {
            message: "파일 업로드 실패: " + res.data.errorMsg,
          });
          this.getData();
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
      axios.delete(`/api/Mastering/mcr-spot/${e.deleteId}?filetoken=${e.fileToken}`).then((res) => {
        if (res && res.status === 200 && !res.data.errorMsg) {
          this.DeleteModalOff();
          this.$fn.notify("primary", {
            title: "파일 삭제 성공",
          });
          this.getData();
        } else {
          this.UpdateModalOff();
          $fn.notify("error", {
            message: "파일 삭제 실패: " + res.data.errorMsg,
          });
          this.getData();
        }
      });
    },
  },
};
</script>
