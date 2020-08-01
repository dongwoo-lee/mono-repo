<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb :heading="heading"/>
      <div class="separator mb-5"></div>
    </b-colxx>
  </b-row>
  <b-row>
    <b-colxx xxs="12">
        <b-card class="mb-4">
            <b-form @submit.stop>
              <b-row>
                <!-- 매체 -->
                <b-colxx sm="2">
                  <b-form-group label="매체" class="has-float-label">
                    <b-form-select 
                      v-model="searchItems.media"
                      :options="mediaOptions"
                      value-field="id"
                      text-field="name"
                    />
                  </b-form-group>
                </b-colxx>
                <!-- 방송일 -->
                <b-colxx sm="2">
                  <b-form-group label="방송일" class="has-float-label">
                    <c-input-date-picker v-model="$v.searchItems.brd_dt.$model" />
                    <b-form-invalid-feedback :state="$v.searchItems.brd_dt.check_date">날짜 형식이 맞지 않습니다.</b-form-invalid-feedback>
                  </b-form-group>
                </b-colxx>
                <!-- 분류 -->
                <b-colxx sm="2" v-if="!type">
                  <b-form-group label="분류" class="has-float-label">
                    <b-form-select 
                      v-model="searchItems.cate"
                      :options="cmOptions"
                      value-field="id"
                      text-field="name"
                    />
                  </b-form-group>
                </b-colxx>
                <!-- 사용처 -->
                <b-colxx sm="2">
                  <b-form-group label="사용처" class="has-float-label">
                    <c-dropdown-menu-input :suggestions="pgmOptions" @selected="onPgmSelected" />
                  </b-form-group>
                </b-colxx>
                <b-button class="mb-1" variant="primary default" size="sm" @click="onSearch">검색</b-button>
              </b-row>
            </b-form>
        </b-card>
        <!-- 테이블, main -->
        <b-row>
          <b-colxx xs="12" md="6">
            <b-card>
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
            </b-card>
          </b-colxx>
          <!-- sub -->
          <b-colxx xs="12" md="6">
            <b-card>
              <b-table
                ref="custom-table"
                class="vuetable"
                sort-by="title" sort-desc.sync="false"
                selectable
                select-mode="single"
                selectedVariant="primary"
                :fields="fieldsContents"
                :items="reponseContentsData.data"
                @row-selected="rowSelected"
              />
            </b-card>
          </b-colxx>
        </b-row>
    </b-colxx>
  </b-row>
  </div>
</template>

<script>
import MixinTablePage from '../../../../mixin/MixinTablePage';

export default {
  name: 'templateSBForm',
  mixins: [ MixinTablePage ],
  props: ['heading', 'type'],
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
        { key: 'status', label: '상태', sortable: false, tdClass: 'text-muted', thStyle: { width: '14%' } },
        { key: 'pgmName', label: '사용처명', sortable: false, tdClass: 'text-muted', thStyle: { width: '14%' } },
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