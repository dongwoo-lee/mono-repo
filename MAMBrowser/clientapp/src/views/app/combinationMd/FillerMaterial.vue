<template>
<div>
  <b-row>
    <b-colxx xxs="12">
      <piaf-breadcrumb heading="필러(일반)"/>
      <div class="separator mb-5"></div>
    </b-colxx>
  </b-row>
  <b-row>
    <b-colxx xxs="12">
        <b-card class="mb-4">
          <b-form @submit.stop>
            <b-row>
              <!-- 분류 -->
              <b-colxx sm="2">
                <b-form-group label="분류" class="has-float-label">
                  <b-form-select 
                    v-model="searchItems.cate"
                    :options="generalOptions"
                    value-field="id"
                    text-field="name" 
                  >
                    <template v-slot:first>
                      <b-form-select-option value="">선택해주세요.</b-form-select-option>
                    </template>
                  </b-form-select>
                </b-form-group>
              </b-colxx>
              <!-- 제작자 -->
              <b-colxx sm="2">
                <b-form-group label="제작자" class="has-float-label">
                  <c-dropdown-menu-input :suggestions="editorOptions" @selected="onEditorSelected" />
                </b-form-group>
              </b-colxx>
              <!-- 소재명 -->
              <b-colxx sm="2">
                <b-form-group label="소재명" class="has-float-label">
                  <c-input-text v-model="searchItems.name"/>
                </b-form-group>
              </b-colxx>
              <b-button class="mb-1" variant="primary default" size="sm" @click="onSearch">검색</b-button>
            </b-row>
          </b-form>
        </b-card>
    </b-colxx>
  </b-row>
  </div>
</template>

<script>
import MixinFilterPage from '../../../mixin/MixinFilterPage';

export default {
  mixins: [ MixinFilterPage ],
  data() {
    return {
      searchItems: {
        media: 'A',                // 매체
        cate: '',                  // 분류
        start_dt: '20200101',      // 시작일
        end_dt: '20200706',        // 종료일
        status: '',                // 상태
        editor: '',                // 사용자
        editorName: '',            // 사용자 이름
        rowPerPage: 16,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      fields: [
        {
          name: 'rowNO',
          title: 'No',
          titleClass: 'center aligned',
          dataClass: "center aligned text-center",
          width: '4%',
        },
        {
          name: "mediaName",
          title: "매체",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '5%',
        },
      ],
    }
  },
}
</script>