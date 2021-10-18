<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="프로그램CM" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form :searchItems="searchItems" :isDisplayPageArea="false">
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 방송일 -->
        <b-form-group
          label="방송일"
          class="has-float-label"
          :class="{ hasError: $v.searchItems.brd_dt.required }"
        >
          <common-date-picker
            v-model="$v.searchItems.brd_dt.$model"
            @input="onSearch"
            required
          />
        </b-form-group>
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
        <!-- 사용처 -->
        <b-form-group label="사용처" class="has-float-label">
          <common-input-text
            v-model="searchItems.pgmName"
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
      <template slot="form-table-area">
        <!-- 테이블 -->
        <b-row>
          <b-colxx xs="12" md="6" class="no-r-p">
            <div class="table-page-info-group pb-1">
              <div class="title">{{ getSelectDate() }}</div>
              <div class="page-info">
                전체 {{ responseData.totalRowCount }}개
              </div>
            </div>
            <b-table
              style="height:580px"
              class="custom-table"
              ref="custom-table"
              thead-class="custom-table-color"
              sort-by="title"
              sort-desc.sync="false"
              selectable
              sticky-header
              responsive
              show-empty
              empty-text="데이터가 없습니다."
              :busy="isTableLoading"
              select-mode="single"
              selectedVariant="primary"
              :fields="fields"
              :items="responseData.data"
              @row-selected="rowSelected"
            >
              <template v-slot:cell(index)="data">
                {{ data.index + 1 }}
              </template>
              <template v-slot:table-busy>
                <div class="text-center text-primary my-2">
                  <b-spinner class="align-middle"></b-spinner>
                  <strong>Loading...</strong>
                </div>
              </template>
              <template v-slot:cell(actions)="data">
                <!-- 다운로드 -->
                <b-button
                  v-if="
                    data.item.id == selectedItem.id &&
                      data.item.length !== '00:00'
                  "
                  :id="`download-${data.index}`"
                  class="icon-buton"
                  v-b-tooltip.hover.top="{ title: '통합다운로드' }"
                  @click.stop="
                    onDownloadConcatenate({
                      grpType: 'cm',
                      brd_Dt: searchItems.brd_dt,
                      grpId: data.item.id,
                      downloadName: `${data.item.name}_${data.item.brdDT}_${data.item.mediaName}_${data.item.id}`
                    })
                  "
                >
                  <b-icon icon="download" class="icon"></b-icon>
                </b-button>
              </template>
              <!-- ID Tooltip -->
              <template v-slot:cell(name)="data">
                <span v-b-tooltip.hover :title="data.item.id">{{
                  data.item.name
                }}</span>
              </template>
            </b-table>
          </b-colxx>
          <!-- sub -->
          <b-colxx xs="12" md="6">
            <div class="table-page-info-group pb-1">
              <div class="title">{{ getSelectName() }}</div>
              <div class="page-info">
                전체 {{ reponseContentsData.totalRowCount }}개
              </div>
            </div>
            <b-table
              class="custom-table"
              ref="custom-table"
              thead-class="custom-table-color"
              sort-by="title"
              sort-desc.sync="false"
              selectable
              sticky-header
              responsive
              show-empty
              empty-text="데이터가 없습니다."
              :busy="isSubTableLoading"
              select-mode="single"
              selectedVariant="primary"
              :fields="fieldsContents"
              :items="reponseContentsData.data"
            >
              <template v-slot:table-busy>
                <div class="text-center text-primary my-2">
                  <b-spinner class="align-middle"></b-spinner>
                  <strong>Loading...</strong>
                </div>
              </template>
              <template v-slot:cell(actions)="data">
                <!-- 미리듣기 -->
                <b-button
                  v-if="display(PREVIEW_CODE)"
                  class="icon-buton"
                  v-b-tooltip.hover.top="{
                    title: IS_ADMIN ? data.item.filePath : '미리듣기',
                    customClass: rowCustomClass(data)
                  }"
                  @click.stop="onPreview(data.item)"
                >
                  <b-icon icon="caret-right-square" class="icon"></b-icon>
                </b-button>
              </template>
            </b-table>
          </b-colxx>
        </b-row>
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
import MixinTablePage from "../../../mixin/MixinTablePage";
import { PREVIEW_CODE } from "@/constants/config";

export default {
  name: "templateSBForm",
  mixins: [MixinTablePage("cm")],
  data() {
    return {
      PREVIEW_CODE: PREVIEW_CODE,
      searchItems: {
        media: "A",
        brd_dt: "",
        cate: "P",
        pgm: "",
        pgmName: ""
      },
      localType: null,
      localTypeOptions: [
        { value: null, text: "선택해주세요." },
        { value: "mcr", text: "주조SB" },
        { value: "scr", text: "부조SB" }
      ],
      fields: [
        { key: "index", label: "순서", tdClass: "list-item-heading" },
        {
          key: "name",
          label: "CM명",
          sortable: true,
          tdClass: "text-muted bold"
        },
        {
          key: "length",
          label: "길이(초)",
          sortable: true,
          tdClass: "text-muted bold"
        },
        {
          key: "capacity",
          label: "용량(초)",
          sortable: true,
          tdClass: "text-muted bold"
        },
        { key: "status", label: "상태", sortable: true, tdClass: "text-muted" },
        {
          key: "editorName",
          label: "담당자",
          sortable: true,
          tdClass: "text-muted"
        },
        { key: "actions", label: "추가작업", tdClass: "text-muted" }
      ],
      fieldsContents: [
        { key: "rowNO", label: "순서", tdClass: "list-item-heading" },
        {
          key: "advertiser",
          label: "광고주",
          tdClass: "text-muted",
          thStyle: { width: "20%" }
        },
        { key: "name", label: "소재명", tdClass: "text-muted" },
        { key: "length", label: "길이(초)", tdClass: "text-muted" },
        { key: "codingUserName", label: "제작자", tdClass: "text-muted" },
        { key: "codingDT", label: "제작일", tdClass: "text-muted" },
        { key: "actions", label: "추가작업", tdClass: "text-muted" }
      ]
    };
  },
  methods: {
    rowCustomClass(data) {
      if (this.reponseContentsData.data.length === 2 && data.index === 0) {
        return "two";
      }
      if (this.reponseContentsData.data.length === 1) {
        return "single";
      }
      return "";
    },
    getSelectDate() {
      if (this.selectBrdDate) {
        return `${this.$fn.formatDate(
          this.selectBrdDate,
          "yyyy-MM-dd"
        )} 프로그램CM 리스트`;
      }
      return `프로그램CM 리스트`;
    },
    getSelectName() {
      if (this.selectName) {
        return `${this.selectName} 상세 내역`;
      }
      return "상세 내역";
    }
  }
};
</script>
