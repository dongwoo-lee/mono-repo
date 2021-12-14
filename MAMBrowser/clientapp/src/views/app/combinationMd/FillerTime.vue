<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="FILLER(시간)" />
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
        <!-- 분류 -->
        <b-form-group label="분류" class="has-float-label">
          <b-form-select
            class="width-220"
            v-model="searchItems.cate"
            :options="timetoneOptions"
            :disabled="timetoneOptions.length === 0"
            value-field="id"
            text-field="name"
            @change="onSearch"
          >
            <template v-slot:first>
              <b-form-select-option v-if="timetoneOptions.length > 0" value=""
                >선택해주세요.</b-form-select-option
              >
              <b-form-select-option v-else value=""
                >값이 존재하지 않습니다.</b-form-select-option
              >
            </template>
          </b-form-select>
        </b-form-group>
        <!-- 상태 -->
        <b-form-group label="상태" class="has-float-label">
          <b-form-select
            class="width-140"
            v-model="searchItems.status"
            :options="reqStatusOptions"
            value-field="id"
            text-field="name"
            @change="onSearch"
          >
            <template v-slot:first>
              <b-form-select-option value=""
                >선택해주세요.</b-form-select-option
              >
            </template>
          </b-form-select>
        </b-form-group>
        <!-- 제작자 -->
        <b-form-group label="제작자" class="has-float-label">
          <common-vue-select
            style="width: 220px"
            :suggestions="editorOptions"
            @inputEvent="onEditorSelected"
          ></common-vue-select>
        </b-form-group>
        <!-- 방송 종료일이 남은 소재만 보기 -->
        <!-- <b-form-checkbox class="custom-checkbox-group-non-align"
          v-model="searchItems.isAvailable"
          value="Y"
          unchecked-value="N"
          aria-describedby="selectedSearchType1"
          aria-controls="selectedSearchType1">
          방송 종료일이 남은 소재만 보기
          </b-form-checkbox> -->
        <!-- 검색 버튼 -->
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
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @sortableclick="onSortable"
        >
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :downloadName="downloadName(props.props.rowData)"
              :behaviorData="behaviorList"
              :etcData="['delete']"
              :isPossibleDelete="authorityCheck(props.props.rowData)"
              @preview="onPreview"
              @download="onDownloadProduct"
              @mydiskCopy="onCopyToMySpacePopup"
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
import FileDelete from "../../../components/FileUpload/FileUpdate/FileDelete.vue";
import axios from "axios";
export default {
  components: { CopyToMySpacePopup, CommonVueSelect, FileDelete },
  mixins: [MixinFillerPage],
  data() {
    return {
      deleteId: "",
      MySpaceScreenName: "[Filler 시간]",
      metaDelete: false,
      searchItems: {
        media: "A", // 매체
        start_dt: "", // 시작일
        end_dt: "", // 종료일
        cate: "", // 분류
        status: "", // 상태
        editor: "", // 사용자
        editorName: "", // 사용자 이름
        // isAvailable: 'Y',
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
          width: "6%",
          sortField: "name",
        },
        {
          name: "brdDate",
          title: "방송개시일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "8%",
          sortField: "brdDate",
          callback: (v) => {
            return this.$fn.dateStringTohaipun(v);
          },
        },
        {
          name: "endDT",
          title: "방송종료일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "8%",
          sortField: "endDT",
          callback: (v) => {
            return this.$fn.dateStringTohaipun(v);
          },
        },
        {
          name: "status",
          title: "상태",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
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
          title: "편집자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "7%",
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
          name: "fileName",
          title: "파일명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
          sortField: "fileName",
        },

        {
          name: "masteringDtm",
          title: "마스터링일자",
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
    // 매체목록 조회
    this.getMediaOptions();
    // 필러(시간) 분류 목록 조회
    this.getTimetoneOptions();
    // 방송의뢰 상태 목록 조회
    this.getReqStatusOptions();
    // 제작자(Md) 목록 조회
    this.getEditorForMd();
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
        .get(`/api/products/filler/time/${media}`, { params: this.searchItems })
        .then((res) => {
          this.setResponseData(res);
          this.addScrollClass();
          this.isTableLoading = false;
          this.isScrollLodaing = false;
        });
    },
    downloadName(rowData) {
      var tmpName = `${rowData.name}_${rowData.brdDate}_${rowData.endDT}_${rowData.id}`;
      return tmpName;
    },
    DeleteModalOff() {
      this.metaDelete = false;
    },
    onMetaDeletePopup(rowData) {
      this.metaDelete = true;
      this.rowData = rowData;
    },
    masteringDelete(e) {
      axios.delete(`/api/Mastering/scr-spot/${e.deleteId}`).then((res) => {
        console.log(res);
      });
    },
  },
};
</script>
