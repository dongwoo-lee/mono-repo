<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="(구)프로소재" />
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
          startDateLabel="시작일(마스터링)"
          endDateLabel="종료일(마스터링)"
          :startDate.sync="searchItems.start_dt"
          :endDate.sync="searchItems.end_dt"
          :startMonthAgo="3"
          :required="false"
          :isCurrentDate="true"
          @SEDateNullEvent="onSearch"
          @SEDateEvent="onSearch"
          @SDateError="SDateErrorLog"
        />
        <!-- 구분 -->
        <b-form-group label="구분" class="has-float-label">
          <b-form-select
            class="width-140"
            v-model="searchItems.type"
            :options="[
              { value: '', text: '선택해주세요.' },
              { value: 'Y', text: '방송중' },
              { value: 'N', text: '폐지' }
            ]"
            @change="onSearch"
          />
        </b-form-group>
        <!-- 분류 -->
        <b-form-group label="분류" class="has-float-label">
          <common-vue-select
            style="width:220px;"
            :suggestions="proOptions"
            @inputEvent="onProSelected"
          ></common-vue-select>
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
              @preview="onPreview"
              @download="onDownloadProduct"
              @mydiskCopy="onCopyToMySpacePopup"
              @modify="onMetaModifyPopup"
              @MasteringDelete="onDeleteConfirm"
            >
            </common-actions>
          </template>
        </common-data-table-scroll-paging>

        <CopyToMySpacePopup
          ref="refCopyToMySpacePopup"
          :show="copyToMySpacePopup"
          @ok="onMyDiskCopyFromProduct"
          @close="copyToMySpacePopup = false"
        >
        </CopyToMySpacePopup>
      </template>
    </common-form>
    <transition name="slide-fade">
      <file-update
        v-if="metaUpdate"
        :rowData="rowData"
        :updateScreenName="updateScreenName"
        @updateFile="masteringUpdate"
        @UpdateModalClose="UpdateModalOff"
      ></file-update>
    </transition>
    <!-- 삭제 -->
    <common-confirm
      id="proRemove"
      title="삭제?"
      :message="getRemove()"
      submitBtn="이동"
      @ok="onDelete()"
    />
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
import FileUpdate from "../../../components/FileUpload/FileUpdate/FileUpdate.vue";
import axios from "axios";
export default {
  components: { CopyToMySpacePopup, CommonVueSelect, FileUpdate },
  mixins: [MixinBasicPage],
  data() {
    return {
      deleteId: "",
      metaUpdate: false,
      rowData: "",
      updateScreenName: "",
      searchItems: {
        media: "A", // 매체
        cate: "", // 분류
        type: "Y", // 타입
        editor: "", // 제작자
        name: "", // 소재명
        start_dt: "", // 시작일
        end_dt: "", // 종료일
        rowPerPage: 30,
        selectPage: 1,
        sortKey: "",
        sortValue: ""
      },
      proOptions: [],
      fields: [
        {
          name: "rowNO",
          title: "순서",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "4%"
        },
        {
          name: "name",
          title: "소재명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "name"
        },
        {
          name: "categoryName",
          title: "분류",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          sortField: "categoryName"
        },
        {
          name: "duration",
          title: "길이(초)",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%",
          sortField: "duration",
          callback: v => {
            return this.$fn.splitFirst(v);
          }
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
          name: "masteringDtm",
          title: "마스터링 일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "12%",
          sortField: "masteringDtm"
        },
        {
          name: "proType",
          title: "타입",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
          sortField: "proType"
        },
        {
          name: "__slot:actions",
          title: "추가작업",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        }
      ]
    };
  },
  created() {
    // (구)프로소재, 공유소재 매체 목록 조회
    this.getMediaPrimaryOptions();
    // 사용자 목록 조회
    this.getEditorOptions();
    // (구)프로 목록 조회
    this.getProOptions();

    this.$nextTick(() => {
      this.getData();
    });
  },
  methods: {
    SDateErrorLog() {
      this.$fn.notify("error", {
        message: "시작 날짜가 종료 날짜보다 큽니다."
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
          message: "시작 날짜가 종료 날짜보다 큽니다."
        });
        this.hasErrorClass = true;
        return;
      }

      this.isTableLoading = this.isScrollLodaing ? false : true;
      this.$http
        .get(`/api/products/old_pro`, { params: this.searchItems })
        .then(res => {
          this.setResponseData(res);
          this.addScrollClass();
          this.isTableLoading = false;
          this.isScrollLodaing = false;
        });
    },
    onProSelected(data) {
      this.searchItems.cate = data.id;
    },
    downloadName(rowData) {
      var tmpName = `${rowData.name}_${rowData.categoryName}`;
      return tmpName;
    },
    onDeleteConfirm(rowData) {
      this.deleteId = rowData.id;
      var user = sessionStorage.getItem("user_id");
      var role = sessionStorage.getItem("authority");
      if (user === rowData.editorID || role == "ADMIN") {
        this.$bvModal.show("proRemove");
      }
    },
    getRemove() {
      return "삭제하시겠습니까?";
    },
    // 휴지통 보내기
    onDelete() {
      axios.delete(`/api/Mastering/pro/${this.deleteId}`).then(res => {
        console.log(res);
      });
    },
    onMetaModifyPopup(rowData) {
      this.metaUpdate = true;
      this.updateScreenName = "pro";
      this.rowData = rowData;
    },
    UpdateModalOff() {
      this.metaUpdate = false;
    },
    masteringUpdate(e) {
      axios.patch("/api/Mastering/pro", e).then(res => {
        console.log(res);
      });
    }
  }
};
</script>
