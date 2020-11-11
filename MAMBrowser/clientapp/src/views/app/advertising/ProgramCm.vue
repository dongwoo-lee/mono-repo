<template>
  <div>
     <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="프로그램CM" />
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
        <b-form-group label="방송일"
          class="has-float-label"
          :class="{ 'hasError': $v.searchItems.brd_dt.required }">
          <common-date-picker v-model="$v.searchItems.brd_dt.$model" required/>
        </b-form-group>
        <!-- 사용처 -->
        <b-form-group label="사용처" class="has-float-label">
          <common-input-text v-model="searchItems.pgmName"/>
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
              <template v-slot:cell(actions)="data">
                <!-- 다운로드 -->
                <b-button :id="`download-${data.index}`" class="icon-buton"
                  @click.stop="onDownloadProduct(data.item.id)">
                  <b-icon icon="download" class="icon"></b-icon>
                </b-button>
              </template>
              <!-- ID Tooltip -->
              <template v-slot:cell(name)="data">
                <span v-b-tooltip.hover :title=data.item.id>{{ data.item.name}}</span>
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
              <template v-slot:cell(actions)="data">
                 <!-- 미리듣기 -->
                <b-button
                  v-if="display('S01G02C002')"
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
import MixinTablePage from '../../../mixin/MixinTablePage';

export default {
  name: 'templateSBForm',
  mixins: [ MixinTablePage('cm') ],
  data() {
    return {
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
        { key: 'name', label: 'CM명', tdClass: 'text-muted' },
        { key: 'length', label: '길이', tdClass: 'text-muted' },
        { key: 'capacity', label: '분량', tdClass: 'text-muted'},
        { key: 'status', label: '상태', tdClass: 'text-muted'},
        { key: 'editorName', label: '담당자', tdClass: 'text-muted'},
        { key: 'actions', label: 'Actions', tdClass: 'text-muted'},
      ],
      fieldsContents: [
        { key: 'rowNO', label: 'No', tdClass: 'list-item-heading' },
        { key: 'advertiser', label: '광고주', tdClass: 'text-muted', thStyle: { width: '20%' } },
        { key: 'name', label: '소재명', tdClass: 'text-muted' },
        { key: 'length', label: '길이', tdClass: 'text-muted' },
        { key: 'format', label: '포맷', tdClass: 'text-muted', thStyle: { width: '10%' } },
        { key: 'codingUserID', label: '코딩인', tdClass: 'text-muted' },
        { key: 'codingDT', label: '코딩일', tdClass: 'text-muted' },
        { key: 'actions', label: 'Actions', tdClass: 'text-muted'},
      ]
    }
  },
}
</script>