<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="음반 기록실" />
        <div class="separator mb-3"></div>
      </b-colxx>
    </b-row>
    <common-form
      :searchItems="searchItems"
      :isDisplayBtnArea="true"
      @changeRowPerpage="onChangeRowPerpage"
    >
      <!-- 검색 -->
      <template slot="form-search-area">
        <!-- 대분류 -->
        <fieldset class="form-group">
          <div class="form-row">
            <span class="bv-no-focus-ring col-form-label">대분류: </span>

             <b-form-checkbox class="custom-checkbox-group"  style="margin-right:10px"
              v-model="allSelected"
              :indeterminate="indeterminate"
              aria-describedby="selectedSearchType1"
              aria-controls="selectedSearchType1"
              @change="toggleAll"
              >
             All
            </b-form-checkbox>


            <b-form-checkbox-group class="custom-checkbox-group" 
            v-model="selectedSearchType1" 
            :options="searchTypes1"
            value-field="code"
            text-field="label">
            </b-form-checkbox-group>  
          </div>
        </fieldset>
        <!-- 소분류 (단일 선택)-->
        <b-form-group label="소분류" class="has-float-label">
          <b-form-select
            class="width-100"
            v-model="searchItems.searchType2"
            :options="searchTypes2"
            value-field="code"
            text-field="label" />
        </b-form-group>
        <!-- 검색옵션 -->
        <fieldset class="form-group">
          <div class="form-row">
            <span class="bv-no-focus-ring col-form-label">검색옵션: </span>
            <b-form-checkbox-group class="custom-checkbox-group" 
            v-model="selectedGradeType"
            :options="gradeTypes"
            value-field="code"
            text-field="label" />
          </div>
        </fieldset>
        <!-- 검색어 -->
         <b-form-group label="검색어" class="has-float-label">
            <common-input-text v-model="searchItems.searchText"/>
          </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch">검색</b-button>
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
          :table-height="'500px'"
          :fields="fields"
          :rows="responseData.data"
          :per-page="responseData.rowPerPage"
          is-actions-slot
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @selectedIds="onSelectedIds"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <common-actions
              :rowData="props.props.rowData"
              :behaviorData="behaviorList"
              @preview="onPreview"
              @download="onDownloadMusic"
            >
            </common-actions>
          </template>
        </common-data-table-scroll-paging>
      </template>
    </common-form>
   <MusicPlayerPopup 
    :showPlayerPopup="showPlayerPopup"
    :music="soundItem"
    :streamingUrl="streamingUrl"
    :waveformUrl="waveformUrl"
    :tempDownloadUrl="tempDownloadUrl"
    requestType="token"
    @closePlayer="onClosePlayer">
    </MusicPlayerPopup>
  </div>
</template>

<script>
import MixinMusicPage from '../../../mixin/MixinMusicPage';

export default {
  mixins: [ MixinMusicPage ],
  data() {
    return {
      streamingUrl : '/api/musicsystem/streaming',
      waveformUrl : '/api/musicsystem/waveform',
      tempDownloadUrl : '/api/musicsystem/temp-download',

      allSelected: false,
      indeterminate: false,

      searchItems: {
        searchType1: 0,
        searchType2: 'song_idx',
        gradeType: 0,
        searchText: '',
        rowPerPage: 15,
        selectPage: 1,
        sortKey: '',
        sortValue: '',
      },
      selectedSearchType1: [],
      selectedGradeType: [],
      searchTypes1: [
        { label: '국내', code: 1 },
        { label: '국외', code: 2 },
        { label: '클래식', code: 4 },
      ],
      searchTypes2: [
        { label: '전체', code: 'song_idx' },
        { label: '곡명', code: 'song_name_idx'},
        { label: '곡명/아티스트', code: 'songname_artist_idx'},
        { label: '아티스트', code: 'song_artist_idx'},
        { label: '배열번호', code: 'song_disc_arr_num_idx' },
        { label: '국가명', code: 'song_country_name_idx' },
      ],
      gradeTypes: [
      { label: '히트', code: 1 },
        { label: '금지', code: 2 },
        { label: '주의', code: 4},
        { label: '청소년 유해', code: 8},
      ],
      isTableLoading: false,
      fields: [
        {
          name: 'rowNO',
          title: 'No',
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '5%',
        },
        {
          name: "name",
          title: "곡명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "artistName",
          title: "아티스트",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%"
        },
        {
          name: "duration",
          title: "재생시간",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%"
        },
        {
          name: "albumName",
          title: "음반",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '15%',
        },
        {
          name: "releaseDate",
          title: "발매년도",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "composer",
          title: "작곡가",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%"
        },
        {
          name: "writer",
          title: "작사가",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%"
        },
        {
          name: "sequenceNO",
          title: "배열번호",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%"
        },
        {
          name: "lyrics",
          title: "마스터링",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: '8%',
        },
        {
          name: '__slot:actions',
          title: 'Actions',
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "6%"
        }
      ],
    }
  },
  
  methods: {
    getData() {
      this.isTableLoading = this.isScrollLodaing ? false: true;
      var params = this.searchItems;
      params.searchType1 =0;
      params.gradeType =0;
      this.selectedSearchType1.forEach(element => {
        params.searchType1 += element;
      });

      this.selectedGradeType.forEach(element => {
        params.gradeType += element;
      });

      this.$http.get(`/api/MusicSystem/music`, { params: this.searchItems })
        .then(res => {
            this.setResponseData(res);
            this.addScrollClass();
            this.isTableLoading = false;
            this.isScrollLodaing = false;
      });
    },
    toggleAll(checked) {
        this.selectedSearchType1 = checked ? [1,2,4] : []
    },
  },
  // watch: {
  //   selectedSearchType1(newVal, oldVal) {
  //     // Handle changes in individual flavour checkboxes
  //     if (newVal.length === 0) {
  //       this.indeterminate = false
  //       this.allSelected = false
  //     } else if (newVal.length === this.searchTypes1.length) {
  //       this.indeterminate = false
  //       this.allSelected = true
  //     } else {
  //       this.indeterminate = true
  //       this.allSelected = false
  //     }
  //   }
  // }
}
</script>