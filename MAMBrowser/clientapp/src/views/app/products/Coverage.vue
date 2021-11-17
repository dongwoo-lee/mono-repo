<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="취재물" />
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
        <!-- 방송일시 -->
        <b-form-group
          label="방송일시"
          class="has-float-label"
          :class="{ hasError: $v.searchItems.brd_dt.required }"
        >
          <common-date-picker
            v-model="$v.searchItems.brd_dt.$model"
            @input="onSearch"
            required
          />
          <b-form-invalid-feedback :state="!$v.searchItems.brd_dt.required"
            >날짜는 필수 입력입니다.</b-form-invalid-feedback
          >
        </b-form-group>

        <!-- 분류 -->
        <b-form-group label="분류" class="has-float-label">
          <b-form-select
            style="border-radius:0px !impotant;"
            class="width-100"
            v-model="searchItems.cate"
            :options="rePortOptions"
            value-field="id"
            text-field="name"
            @change="onSearch"
          />
        </b-form-group>
        <!-- 사용처 -->
        <b-form-group label="사용처명" class="has-float-label w-10">
          <common-input-text
            v-model="searchItems.pgmName"
            @inputEnterEvent="onSearch"
          />
        </b-form-group>
        <!-- 취재인 -->
        <b-form-group label="취재인" class="has-float-label w-10">
          <common-input-text
            v-model="searchItems.reporterName"
            @inputEnterEvent="onSearch"
          />
        </b-form-group>
        <!-- 제작자 -->
        <b-form-group label="제작자" class="has-float-label">
          <common-vue-select
            style="width:160px;"
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
        <!-- 마스터링 완료한 소재만 보기 -->
        <b-form-checkbox
          class="custom-checkbox-group-non-align"
          v-model="searchItems.isMastering"
          value="Y"
          unchecked-value="N"
          aria-describedby="selectedSearchType1"
          aria-controls="selectedSearchType1"
          @input="onSearch"
        >
          마스터링 완료한 소재만 보기
        </b-form-checkbox>
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
          @selectedIds="onSelectedIds"
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
      id="reportRemove"
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
        cate: "RC07", // 분류
        brd_dt: "",
        pgm: "", // 사용처1
        pgmName: "", // 사용처2
        reporterName: "", // 취재인 이름
        editor: "", // 제작자
        name: "", // 소재명
        isMastering: "Y",
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
          name: "name",
          title: "소재명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "name"
        },
        {
          name: "reporter",
          title: "취재인",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%",
          sortField: "reporter"
        },
        {
          name: "pgmName",
          title: "사용처명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "pgmName"
        },
        {
          name: "brdDT",
          title: "방송일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "10%",
          sortField: "brdDT",
          callback: v => {
            return this.$fn.dateStringTohaipun(v);
          }
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
          dataClass: "center aligned text-center",
          width: "8%",
          sortField: "editorName"
        },
        {
          name: "editDtm",
          title: "최종편집일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "11%",
          sortField: "editDtm"
        },
        {
          name: "masteringDtm",
          title: "마스터링일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "11%",
          sortField: "masteringDtm"
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
    // 취재물 분류 목록 조회
    this.getReportOptions();
    // 사용자 목록 조회
    this.getEditorForReporter();

    this.$nextTick(() => {
      this.getData();
    });
  },
  methods: {
    getData() {
      if (!this.$v.searchItems.brd_dt.$invalid) {
        this.$fn.notify("inputError", {});
        return;
      }

      this.isTableLoading = this.isScrollLodaing ? false : true;

      this.$http
        .get(`/api/products/report`, { params: this.searchItems })
        .then(res => {
          this.setResponseData(res);
          this.addScrollClass();
          this.isTableLoading = false;
          this.isScrollLodaing = false;
        });
    },
    downloadName(rowData) {
      var tmpName = `${rowData.pgmName}_${rowData.brdDT}_${rowData.name}`;
      return tmpName;
    },
    onDeleteConfirm(rowData) {
      this.deleteId = rowData.id;
      var user = sessionStorage.getItem("user_id");
      var role = sessionStorage.getItem("authority");
      if (user === rowData.editorID || role == "ADMIN") {
        this.$bvModal.show("reportRemove");
      }
    },
    getRemove() {
      return "삭제하시겠습니까?";
    },
    // 휴지통 보내기
    onDelete() {
      axios.delete(`/api/Mastering/report/${this.deleteId}`).then(res => {
        console.log(res);
      });
    },
    onMetaModifyPopup(rowData) {
      this.metaUpdate = true;
      this.updateScreenName = "report";
      this.rowData = rowData;
    },
    UpdateModalOff() {
      this.metaUpdate = false;
    },
    masteringUpdate(e) {
      axios.patch("/api/Mastering/filler", e).then(res => {
        console.log(res);
      });
    }
  }
};
</script>
