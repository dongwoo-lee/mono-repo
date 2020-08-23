<template>
  <div class="common-form">
    <b-row style="marin-top:-10px;">
      <b-card>
        <!-- 검색 영역 -->
        <b-container fluid>
          <div class="search-form-area">
            <b-form class="search-form" @submit.stop>
              <b-row>
                  <slot name="form-search-area"></slot>
              </b-row>
            </b-form>
          </div>
        </b-container>

        <!-- 버튼 영역 -->
        <b-container fluid class="text-center mb-1">
          <!-- 버튼 모음 -->
          <b-row align-v="center">
            <b-col cols="auto" class="mr-auto pt-3 form-btn">
                <b-form class="mb-1" inline :isDisplayBtnArea="isDisplayBtnArea">
                    <slot name="form-btn-area"></slot>
                </b-form>
            </b-col>
            <b-col cols="auto" class="pt-3">
              <div class="page-info-group">
                <div class="page-info">
                    <slot name="form-table-page-area"></slot>
                </div>
                <div class="page-size">
                  <b-form-select :value="searchItems.rowPerPage" @change="onChagne">
                    <b-form-select-option value="15">15개</b-form-select-option>
                    <b-form-select-option value="30">30개</b-form-select-option>
                    <b-form-select-option value="50">50개</b-form-select-option>
                    <b-form-select-option value="100">100개</b-form-select-option>
                  </b-form-select>
                </div>
              </div>
            </b-col>
          </b-row>
        </b-container>
        <!-- 테이블 영역 -->
        <slot name="form-table-area"></slot>
      </b-card>
    </b-row>
    <!-- 확인창 영역 -->
    <slot name="form-confirm-area"></slot>
  </div>
</template>
<script>
export default {
    props: ['searchItems', 'isDisplayBtnArea'],
    props: {
      searchItems: {
        type: Object,
        default: () => ({
          rowPerPage: 0
        })
      },
      isDisplayBtnArea: {
        type: Boolean,
        default: false,
      }
    },
    methods: {
        onChagne(rowPerpage) {
            this.$emit('changeRowPerpage', rowPerpage);
        }
    }
}
</script>