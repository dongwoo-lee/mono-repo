<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb :heading="heading" />
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
            @input="onSearch"
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
            style="width:220px;"
            :suggestions="editorOptions"
            @inputEvent="onEditorSelected"
            @blurEvent="onSearch"
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
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :downloadName="downloadName(props.props.rowData)"
              :behaviorData="behaviorList"
              @preview="onPreview"
              @download="onDownloadProduct"
              @mydiskCopy="onCopyToMySpacePopup"
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
export default {
  components: { CopyToMySpacePopup, CommonVueSelect },
  mixins: [MixinFillerPage],
  props: ["heading", "screenName"],
  data() {
    return {
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
          name: "categoryName",
          title: "분류",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
          sortField: "categoryName"
        },
        {
          name: "name",
          title: "소재명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "name"
        },
        {
          name: "brdDT",
          title: "방송유효일",
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
          width: "8%",
          sortField: "duration"
        },
        {
          name: "editorName",
          title: "편집자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
          sortField: "editorName"
        },
        {
          name: "editDtm",
          title: "편집일자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%",
          sortField: "editDtm"
        },
        {
          name: "masteringDtm",
          title: "마스터링일자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          sortField: "masteringDtm",
          width: "12%",
          sortField: "masteringDtm"
        },
        {
          name: "__slot:actions",
          title: "추가작업",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "7%"
        }
      ]
    };
  },
  created() {
    this.getOptions();
  },
  methods: {
    getData() {
      if (!this.$v.searchItems.brd_dt.$invalid) {
        this.$fn.notify("inputError", {});
        return;
      }

      this.isTableLoading = this.isScrollLodaing ? false : true;
      const brd_dt = this.searchItems.brd_dt;

      this.$http
        .get(`/api/products/filler/${this.screenName}/${brd_dt}`, {
          params: this.searchItems
        })
        .then(res => {
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
    }
  }
};
</script>
