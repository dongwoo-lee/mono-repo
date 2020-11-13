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
      :isDisplayPageArea="false"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 매체 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            class="width-120"
            v-model="searchItems.media"
            :options="mediaOptions"
            value-field="id"
            text-field="name"
          />
        </b-form-group>
        <!-- 방송일 -->
        <b-form-group label="시작일"
          class="has-float-label"
          :class="{ 'hasError': $v.searchItems.brd_dt.required }">
          <common-date-picker v-model="$v.searchItems.brd_dt.$model" required/>
        </b-form-group>
        <!-- 분류 -->
        <b-form-group v-if="!screenName" label="분류" class="has-float-label">
          <b-form-select
            class="width-120"
            v-model="searchItems.cate"
            :options="cmOptions"
            value-field="id"
            text-field="name"
          />
        </b-form-group>
        <!-- 사용처 -->
        <b-form-group v-if="screenName !== 'mcr'" label="사용처" class="has-float-label">
          <common-dropdown-menu-input 
            classString="width-220"
            :isLoadingClass="isLoadingClass"
            :suggestions="pgmOptions"
            @selected="onPgmSelected"
          />
        </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch">검색</b-button>
        </b-form-group>
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <b-row>
          <b-colxx xs="12" md="6" class="no-r-p">
            <div class="table-page-info-group pb-1">
              <div class="page-info">전체 {{responseData.totalRowCount}}개</div>
            </div>
            <b-table
              class="custom-table"
              ref="custom-table"
              thead-class="custom-table-color"
              sort-by="title" sort-desc.sync="false"
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
              <template v-slot:table-busy>
                <div class="text-center text-primary my-2">
                  <b-spinner class="align-middle"></b-spinner>
                  <strong>Loading...</strong>
                </div>
              </template>
              <template v-if="screenName" v-slot:cell(actions)="data">
                <!-- 다운로드 -->
                <b-button
                  v-if="data.item.id==selectedItem.id"
                  :id="`download-${data.index}`" class="icon-buton"
                  @click.stop="onDownloadConcatenate(reponseContentsData.data)">
                  <b-icon icon="download" class="icon"></b-icon>
                </b-button>   
              </template>
            </b-table>
          </b-colxx>
          <!-- sub -->
          <b-colxx xs="12" md="6">
            <div class="table-page-info-group pb-1">
              <div class="page-info">전체 {{reponseContentsData.totalRowCount}}개</div>
            </div>
            <b-table
              class="custom-table"
              ref="custom-table"
              thead-class="custom-table-color"
              sort-by="title" sort-desc.sync="false"
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
              <template v-if="screenName" v-slot:cell(actions)="data">
                <!-- 미리듣기 -->
                <b-button
                  v-if="display(PREVIEW_CODE)"
                  class="icon-buton"
                  title="미리듣기"
                  @click.stop="onPreview(data.item)">
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
    requestType="token"
    @closePlayer="onClosePlayer">
    </PlayerPopup>
  </div>
</template>

<script>
import MixinTablePage from '../../../../mixin/MixinTablePage';
import { PREVIEW_CODE } from "@/constants/config";

export default {
  name: 'templateSBForm',
  mixins: [ MixinTablePage('sb') ],
  props: ['heading', 'screenName'],
  created() {
      // 부조SB의 경우 사용처명 표시
      if (this.screenName === 'scr') {
        this.fields.splice(4, 0, { key: 'pgmName', label: '사용처명', tdClass: 'text-muted' })
      }
  },
  data() {
    return {
      PREVIEW_CODE: PREVIEW_CODE,
      searchItems: {
        media: 'A',
        brd_dt: '20200101',
        cate: 'P',
        pgm: '',
        pgmName: '',
      },
      localType: null,
      localTypeOptions: [ 
        { value: null, text: '선택해주세요.' },
        { value: 'mcr', text: '주조SB' },
        { value: 'scr', text: '부조SB' },
      ],
      fields: [
        { key: 'rowNO', label: 'No', tdClass: 'list-item-heading' },
        { key: 'brdDT', label: '방송일', tdClass: 'text-muted' },
        { key: 'id', label: 'ID', tdClass: 'text-muted' },
        { key: 'name', label: 'SB명', tdClass: 'text-muted' },
        { key: 'length', label: '길이', tdClass: 'text-muted' },
        { key: 'capacity', label: '분량', tdClass: 'text-muted'},
        { key: 'status', label: '상태', tdClass: 'text-muted'},
        { key: 'editorName', label: '담당자', tdClass: 'text-muted'},
        { key: 'actions', label: 'Actions', tdClass: 'text-muted'},
      ],
      fieldsContents: [
        { key: 'rowNO', label: 'No', tdClass: 'list-item-heading' },
        { key: 'categoryID', label: '구분', tdClass: 'text-muted', thStyle: { width: '10%' } },
        { key: 'categoryName', label: '광고주명/분류명', tdClass: 'text-muted', thStyle: { width: '20%' } },
        { key: 'id', label: '소재ID', tdClass: 'text-muted' },
        { key: 'name', label: '소재명', tdClass: 'text-muted' },
        { key: 'length', label: '길이', tdClass: 'text-muted' },
        { key: 'format', label: '포맷', tdClass: 'text-muted', thStyle: { width: '10%' } },
        { key: 'actions', label: 'Actions', tdClass: 'text-muted'},
      ]
    }
  },
}
</script>