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
        <b-form-group label="시작일" class="has-float-label">
          <common-date-picker v-model="$v.searchItems.brd_dt.$model" />
          <b-form-invalid-feedback
            :state="$v.searchItems.brd_dt.check_date"
          >날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
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
        <b-form-group label="사용처" class="has-float-label">
          <common-dropdown-menu-input classString="width-220" :suggestions="pgmOptions" @selected="onPgmSelected" />
        </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch">검색</b-button>
        </b-form-group>
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <b-row>
          <b-colxx xs="12" md="6">
            <b-table
              ref="custom-table"
              class="vuetable"
              sort-by="title" sort-desc.sync="false"
              selectable
              select-mode="single"
              selectedVariant="primary"
              :fields="fields"
              :items="responseData.data"
              @row-selected="rowSelected"
            />
          </b-colxx>
          <!-- sub -->
          <b-colxx xs="12" md="6">
            <b-table
              ref="custom-table"
              class="vuetable"
              sort-by="title" sort-desc.sync="false"
              selectable
              select-mode="single"
              selectedVariant="primary"
              :fields="fieldsContents"
              :items="reponseContentsData.data"
            />
          </b-colxx>
        </b-row>
      </template>
    </common-form>
  </div>
</template>

<script>
import MixinTablePage from '../../../../mixin/MixinTablePage';

export default {
  name: 'templateSBForm',
  mixins: [ MixinTablePage ],
  props: ['heading', 'screenName'],
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
        { key: 'rowNO', label: 'No', sortable: false, sortDirection: 'desc', tdClass: 'list-item-heading' },
        { key: 'brdDT', label: '방송일', sortable: false, tdClass: 'text-muted' },
        { key: 'id', label: 'ID', sortable: false, tdClass: 'text-muted' },
        { key: 'name', label: 'SB명', sortable: false, tdClass: 'text-muted' },
        { key: 'length', label: '길이', sortable: false, tdClass: 'text-muted' },
        { key: 'capacity', label: '분량', sortable: false, tdClass: 'text-muted', thStyle: { width: '10%' } },
        { key: 'status', label: '상태', sortable: false, tdClass: 'text-muted', thStyle: { width: '9%' } },
        { key: 'pgmName', label: '사용처', sortable: false, tdClass: 'text-muted', thStyle: { width: '15%' } },
        { key: 'editorName', label: '담당자', sortable: false, tdClass: 'text-muted', thStyle: { width: '14%' } },
      ],
      fieldsContents: [
        { key: 'rowNO', label: 'No', sortable: false, sortDirection: 'desc', tdClass: 'list-item-heading' },
        { key: 'categoryID', label: '구분', sortable: false, tdClass: 'text-muted', thStyle: { width: '10%' } },
        { key: 'categoryName', label: '광고주명/분류명', sortable: false, tdClass: 'text-muted', thStyle: { width: '20%' } },
        { key: 'id', label: '소재ID', sortable: false, tdClass: 'text-muted' },
        { key: 'name', label: '소재명', sortable: false, tdClass: 'text-muted' },
        { key: 'length', label: '길이', sortable: false, tdClass: 'text-muted' },
        { key: 'format', label: '포맷', sortable: false, tdClass: 'text-muted', thStyle: { width: '10%' } },
      ]
    }
  },
}
</script>