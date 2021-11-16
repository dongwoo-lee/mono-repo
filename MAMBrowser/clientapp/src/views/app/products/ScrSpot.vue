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
              @delete="onDeleteConfirm"
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
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CopyToMySpacePopup from "../../../components/Popup/CopyToMySpacePopup";
import CommonVueSelect from "../../../components/Form/CommonVueSelect.vue";
import axios from "axios";
export default {
  components: { CopyToMySpacePopup, CommonVueSelect },
  mixins: [MixinBasicPage],
  data() {
    return {
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
        sortValue: "DESC"
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
          name: "categoryName",
          title: "분류",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "13%",
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
          name: "editorName",
          title: "제작자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "5%",
          sortField: "editorName"
        },
        {
          name: "pgmName",
          title: "사용처명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "pgmName"
        },
        {
          name: "masteringDtm",
          title: "마스터링 일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%",
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
    // 매체 목록 조회
    this.getMediaOptions();
    // 사용자 목록 조회
    this.getEditorOptions();

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
        .get(`/api/products/spot/scr/${this.searchItems.media}`, {
          params: this.searchItems
        })
        .then(res => {
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
    onDeleteConfirm(rowData) {
      console.log(rowData);
    },
    onMetaModifyPopup(rowData) {
      var body = {
        AudioFileID: rowData.id,
        Title: rowData.name
      };
      axios.patch("/api/Mastering/scr-spot", body).then(res => {
        console.log(res);
      });
    }
  }
};
</script>
