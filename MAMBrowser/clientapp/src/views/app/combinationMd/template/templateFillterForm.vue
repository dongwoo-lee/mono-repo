<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb :heading="heading" :tooltip="tooltip" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      @changeRowPerpage="onChangeRowPerpage"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 방송일 -->
        <b-form-group
          label="방송일"
          class="has-float-label"
          :class="{ hasError: hasErrorClass || $v.searchItems.brd_dt.required }"
        >
          <common-date-picker
            v-model="$v.searchItems.brd_dt.$model"
            @input="onSearch"
            required
          />
        </b-form-group>
        <!-- 분류 -->
        <b-form-group label="분류" class="has-float-label">
          <b-form-select
            class="width-220"
            v-model="searchItems.cate"
            :options="categoryOptions"
            :disabled="categoryOptions.length === 0"
            value-field="id"
            text-field="name"
            @change="onSearch"
          >
            <template v-slot:first>
              <b-form-select-option v-if="categoryOptions.length > 0" value=""
                >선택해주세요.</b-form-select-option
              >
              <b-form-select-option v-else value=""
                >값이 존재하지 않습니다.</b-form-select-option
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
        <!-- 소재명 -->
        <b-form-group label="소재명" class="has-float-label">
          <common-input-text
            v-model="searchItems.name"
            @inputEnterEvent="onSearch"
          />
        </b-form-group>
        <!-- 방송 유효일이 남은 소재만 보기 -->
        <!-- <b-form-checkbox v-if="screenName === 'pr'"
          class="custom-checkbox-group-non-align"
          v-model="searchItems.isAvailable"
          value="Y"
          unchecked-value="N"
          aria-describedby="selectedSearchType1"
          aria-controls="selectedSearchType1">
          방송 유효일이 남은 소재만 보기
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
          <template slot="rowNO" scope="props">
            <div>
              <span
                v-b-tooltip.hover
                :title="getMachineName(props.props.rowData.masteringMachine)"
                >{{ props.props.rowData.rowNO }}</span
              >
            </div>
          </template>

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
              @modify="onMetaModifyPopup"
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
    <transition name="slide-fade">
      <file-update
        v-if="metaUpdate"
        :rowData="rowData"
        :updateScreenName="updateScreenName"
        :fillerType="fillerType"
        @updateFile="masteringUpdate"
        @UpdateModalClose="UpdateModalOff"
      ></file-update>
    </transition>
    <!-- 방송의뢰 파일 삭제 -->
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
import MixinFillerPage from "../../../../mixin/MixinFillerPage";
import CopyToMySpacePopup from "../../../../components/Popup/CopyToMySpacePopup";
import CommonVueSelect from "../../../../components/Form/CommonVueSelect.vue";
import FileUpdate from "../../../../components/FileUpload/FileUpdate/FileUpdate.vue";
import FileDelete from "../../../../components/FileUpload/FileUpdate/FileDelete.vue";
import axios from "axios";
export default {
  components: { CopyToMySpacePopup, CommonVueSelect, FileUpdate, FileDelete },
  mixins: [MixinFillerPage],
  props: ["heading", "screenName", "tooltip"],
  data() {
    return {
      deleteId: "",
      metaUpdate: false,
      metaDelete: false,
      fillerType: "",
      rowData: "",
      updateScreenName: "",
      searchItems: {
        brd_dt: "", // 방송일
        cate: "", // 분류
        editor: "", // 사용자
        editorName: "", // 사용자 이름
        name: "", // 소재명
        // isAvailable: this.screenName === 'pr' ? 'Y': 'N',
        rowPerPage: 30,
        selectPage: 1,
        sortKey: "",
        sortValue: "",
      },
      isTableLoading: false,
      fields: [
        {
          name: "__slot:rowNO",
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
          width: "10%",
          sortField: "categoryName",
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
          title: "방송유효일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "10%",
          sortField: "brdDT",
          callback: (v) => {
            return this.$fn.dateStringTohaipun(v);
          },
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
          width: "8%",
          sortField: "editorName",
        },
        {
          name: "editDtm",
          title: "편집일자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%",
          sortField: "editDtm",
        },
        {
          name: "masteringDtm",
          title: "방송의뢰일자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          sortField: "masteringDtm",
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
    this.getOptions();
    this.getScreenName();
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
    getData() {
      if (!this.$v.searchItems.brd_dt.$invalid) {
        this.$fn.notify("inputError", {});
        return;
      }

      this.isTableLoading = this.isScrollLodaing ? false : true;
      const brd_dt = this.searchItems.brd_dt;

      this.$http
        .get(`/api/products/filler/${this.screenName}/${brd_dt}`, {
          params: this.searchItems,
        })
        .then((res) => {
          this.setResponseData(res);
          this.addScrollClass();
          this.isTableLoading = false;
          this.isScrollLodaing = false;
        });
    },
    onChangeMedia(value) {
      this.getProOptions(value);
    },
    getOptions() {
      this.getMediaOptions();
      this.getEditorForMd();

      // 필러(pr)
      if (this.screenName === "pr") this.getProOptions();
      // 필러(일반)
      if (this.screenName === "general") this.getGeneralOptions();
      // 필러(기타)
      if (this.screenName === "etc") this.getEtcOptions();
    },
    downloadName(rowData) {
      var tmpName = `${rowData.name}_${rowData.brdDT}_${rowData.categoryName}_${rowData.id}`;
      return tmpName;
    },
    getScreenName() {
      if (this.screenName == "pr") {
        this.MySpaceScreenName = "[Filler PR]";
      } else if (this.screenName == "general") {
        this.MySpaceScreenName = "[Filler 소재]";
      } else if (this.screenName == "etc") {
        this.MySpaceScreenName = "[Filler 기타]";
      }
    },
    onMetaModifyPopup(rowData) {
      this.metaUpdate = true;
      this.updateScreenName = "filler";
      if (this.screenName == "pr") {
        this.fillerType = "pro";
      } else if (this.screenName == "general") {
        this.fillerType = "general";
      } else if (this.screenName == "etc") {
        this.fillerType = "etc";
      }
      this.rowData = rowData;
    },
    UpdateModalOff() {
      this.metaUpdate = false;
    },
    masteringUpdate(e) {
      axios.patch("/api/Mastering/filler", e).then((res) => {
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
          `/api/Mastering/filler/${e.deleteId}?fileToken=${e.fileToken}`,
          {
            headers: {
              "Content-Type": "application/json",
              "X-Csrf-Token": sessionStorage.getItem("access_token"),
            },
          }
        )
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
