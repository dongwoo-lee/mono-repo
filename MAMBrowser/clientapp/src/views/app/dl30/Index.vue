<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="DL 3.0" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      :isDisplayPageSize="false"
      @changeRowPerpage="onChangeRowPerpage"
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
        <!-- 편성일자: 시작일자 -->
        <b-form-group label="방송일"
          class="has-float-label"
          :class="{ 'hasError': $v.searchItems.regDtm.required }">
          <common-date-picker v-model="$v.searchItems.regDtm.$model" required/>
          <b-form-invalid-feedback :state="!$v.searchItems.regDtm.required">날짜는 필수 입력입니다.</b-form-invalid-feedback>
        </b-form-group>
        <!-- 녹음명 -->
        <b-form-group label="녹음명" class="has-float-label c-zindex">
            <common-input-text v-model="searchItems.pgmName" />
        </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch">검색</b-button>
        </b-form-group>
      </template>
      <!-- 테이블 페이지 -->
      <template slot="form-table-page-area">
        {{ getTotalRowCount() }}
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <common-data-table
            :fields="fields"
            :rows="responseData.data"
            :isTableLoading="isTableLoading"
            is-actions-slot
        >
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :behaviorData="behaviorList"
              @preview="onPreview"
              @download="onDownloadDl30"
            >
            </common-actions>
          </template>
        </common-data-table>
      </template>
    </common-form>

    <PlayerPopup 
    :showPlayerPopup="showPlayerPopup"
    :title="soundItem.recName"
    :fileKey="soundItem.seq"
    :streamingUrl="streamingUrl"
    :waveformUrl="waveformUrl"
    :tempDownloadUrl="tempDownloadUrl"
    requestType="key"
    direct = "Y"
    @closePlayer="onClosePlayer">
    </PlayerPopup>
  </div>
</template>

<script>
import MixinBasicPage from '../../../mixin/MixinBasicPage';

export default {
    mixins: [ MixinBasicPage ],
    data() {
      return {
        streamingUrl : '/api/Products/dl30-streaming',
        waveformUrl : '/api/Products/dl30-waveform',
        tempDownloadUrl : '/api/Products/dl30-temp-download',

          searchItems: {
              media : 'A',           // 매체
              regDtm: '',            // 편성일자
              pgmName: '',             // 녹음명
              rowPerPage: 15,
              selectPage: 1,
              sortKey: '',
              sortValue: '',
          },
          isTableLoading: false,
          fields: [
              {
                  name: 'rowNO',
                  title: '순서',
                  titleClass: "center aligned text-center",
                  dataClass: "center aligned text-center",
                  width: '4%',
              },
              {
                  name: "deviceName",
                  title: "단말",
                  titleClass: "center aligned text-center",
                  dataClass: "center aligned text-center",
                  width: '10%',
                  sortField: 'deviceName',
              },
              {
                  name: "brdDate",
                  title: "송출일시",
                  titleClass: "center aligned text-center",
                  dataClass: "center aligned text-center bold",
                  width: '15%',
                  sortField: 'brdDate',
              },
              {
                  name: "recName",
                  title: "녹음소재명",
                  titleClass: "center aligned text-center",
                  dataClass: "center aligned text-center bold",
                  sortField: 'recName',
              },
              {
                  name: "sourceID",
                  title: "Source ID",
                  titleClass: "center aligned text-center",
                  dataClass: "center aligned text-center",
                  width: '7%',
                  sortField: 'sourceID',
              },
              {
                  name: "duration",
                  title: "녹음분량",
                  titleClass: "center aligned text-center",
                  dataClass: "center aligned text-center",
                  width: '10%',
                  sortField: 'duration',
              },
              {
                  name: "fileSize",
                  title: "파일사이즈(byte)",
                  titleClass: "center aligned text-center",
                  dataClass: "center aligned text-center",
                  width: '10%',
                  sortField: 'fileSize',
                   callback: (v) => {
                    return this.$fn.formatBytes(v)
                  }
              },
              {
                  name: "regDtm",
                  title: "등록일시",
                  titleClass: "center aligned text-center",
                  dataClass: "center aligned text-center",
                  width: '15%',
                  sortField: 'regDtm',
              },
              {
                name: '__slot:actions',
                title: '추가작업',
                titleClass: "center aligned text-center",
                dataClass: "center aligned text-center",
                width: "6%"
              }
          ]
      }
    },
    created() {
        this.getMediaOptions();
    },
    methods: {
        getData() {
            if (!this.$v.searchItems.regDtm.$invalid) {
                this.$fn.notify('inputError', {});
                return;
            }

            this.isTableLoading = true;
            const media = this.searchItems.media;
            const regDtm = this.searchItems.regDtm;

            this.$http.get(`/api/products/dl30/${media}/${regDtm}`, { params: this.searchItems })
                .then(res => {
                this.setResponseData(res, 'normal');
                this.isTableLoading = false;
            });
        },
    }
}
</script>
