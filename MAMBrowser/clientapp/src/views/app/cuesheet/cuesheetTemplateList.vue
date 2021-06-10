<template>
  <div>
    <b-row v-if="!modalData">
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="템플릿" />
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
        <!-- 템플릿 이름 -->
        <b-form-group label="템플릿 이름" class="has-float-label">
          <common-input-text />
        </b-form-group>
        <!-- 생성일 -->
        <b-form-group label="생성일" class="has-float-label">
          <common-date-picker />
        </b-form-group>
        <!-- 수정일 -->
        <b-form-group label="수정일" class="has-float-label">
          <common-date-picker />
        </b-form-group>
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >검색</b-button
          >
        </b-form-group>
      </template>

      <!-- 버튼 -->
      <template slot="form-btn-area" v-if="!modalData">
        <b-input-group>
          <b-button
            variant="outline-primary default"
            size="sm"
            v-b-modal.modal-center
            >템플릿 추가</b-button
          >
        </b-input-group>
        <b-input-group>
          <b-button variant="outline-secondary default" size="sm"
            >선택 항목 삭제</b-button
          >
        </b-input-group>
      </template>

      <!-- 테이블 페이지 -->
      <template slot="form-table-page-area" v-if="!modalData">
        {{ getPageInfo() }}
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <common-data-table-scroll-paging
          ref="scrollPaging"
          :tableHeight="tablewidth"
          :fields="fields"
          :rows="templateList"
          :per-page="responseData.rowPerPage"
          :totalCount="responseData.totalRowCount"
          is-actions-slot
          isWeekSlot="true"
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <common-actions :rowData="props.props.rowData" :temData="temData">
            </common-actions>
          </template>
        </common-data-table-scroll-paging>
      </template>
    </common-form>

    <b-modal
      id="modal-center"
      size="lg"
      centered
      title="템플릿 추가"
      ok-title="확인"
      cancel-title="취소"
    >
      <div id="modelDiv" class="d-block text-center">
        <div class="mb-3 mt-3" style="font-size: 20px">
          추가할 템플릿 이름을 입력해 주세요.
        </div>
        <div>
          <div
            class="dx-field-label mt-3 mb-5 pl-5 pr-0"
            style="font-size: 15px"
          >
            템플릿 명 :
          </div>
          <div class="dx-field-value mt-3 mb-5 pr-5">
            <DxTextBox placeholder="이름없는 템플릿" width="320px" />
          </div>
        </div>
      </div>
    </b-modal>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import { DxDataGrid, DxColumn } from "devextreme-vue/data-grid";
import DxTextBox from "devextreme-vue/text-box";
export default {
  props: {
    modalData: false,
    temData: true,
    tablewidth: "",
  },
  mixins: [MixinBasicPage],
  components: {
    DxDataGrid,
    DxColumn,
    DxTextBox,
  },
  data() {
    return {
      tablewidth: "525px",
      test: null,
      date: null,
      simpleProducts: [
        "김이나의 별이 빛나는 밤에",
        "두시의 데이트 뮤지, 안영미입니다",
      ],
      searchItems: {
        updateDate: "", // 수정일
        channel: "", // 채널
        programName: "", // 프로그램명
        weekData: "", // 작성상태
      },
      proOptions: [],
      fields: [
        {
          name: "title",
          title: "템플릿 이름",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
        },
        {
          name: "addDate",
          title: "생성일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "20%",
        },

        {
          name: "updateDate",
          title: "수정일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "20%",
        },
        {
          name: "__slot:actions",
          title: "",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "10%",
        },
      ],
    };
  },

  computed: {
    ...mapGetters("cuesheet", ["templateList"]),
  },

  created() {
    // (구)프로소재, 공유소재 매체 목록 조회
    this.getMediaPrimaryOptions();
    // 사용자 목록 조회
    this.getEditorOptions();
    // (구)프로 목록 조회
    this.getProOptions();
  },
  methods: {
    weekclassbind1(props) {
      console.log(props.rowIndex);
      var s = this.rows[props.rowIndex].weekData;
      if (s.indexOf("mon") >= 0) {
        return true;
      }
      return false;
    },
    weekclassbind2(props) {
      console.log(props.rowIndex);
      var s = this.rows[props.rowIndex].weekData;
      if (s.indexOf("tue") >= 0) {
        return true;
      }
      return false;
    },
    weekclassbind3(props) {
      console.log(props.rowIndex);
      var s = this.rows[props.rowIndex].weekData;
      if (s.indexOf("wed") >= 0) {
        return true;
      }
      return false;
    },
    weekclassbind4(props) {
      console.log(props.rowIndex);
      var s = this.rows[props.rowIndex].weekData;
      if (s.indexOf("thu") >= 0) {
        return true;
      }
      return false;
    },
    weekclassbind5(props) {
      console.log(props.rowIndex);
      var s = this.rows[props.rowIndex].weekData;
      if (s.indexOf("fri") >= 0) {
        return true;
      }
      return false;
    },
    weekclassbind6(props) {
      console.log(props.rowIndex);
      var s = this.rows[props.rowIndex].weekData;
      if (s.indexOf("sat") >= 0) {
        return true;
      }
      return false;
    },
    weekclassbind7(props) {
      console.log(props.rowIndex);
      var s = this.rows[props.rowIndex].weekData;
      if (s.indexOf("sun") >= 0) {
        return true;
      }
      return false;
    },
    getData() {
      if (
        this.$fn.checkGreaterStartDate(
          this.searchItems.start_dt,
          this.searchItems.end_dt
        )
      ) {
        this.$fn.notify("error", {
          message: "시작 날짜가 종료 날짜보다 큽니다.",
        });
        this.hasErrorClass = true;
        return;
      }
      this.isTableLoading = this.isScrollLodaing ? false : true;
      this.$http
        .get(`/api/products/old_pro`, { params: this.searchItems })
        .then((res) => {
          this.setResponseData(res);
          this.addScrollClass();
          this.isTableLoading = false;
          this.isScrollLodaing = false;
        });
    },
  },
};
</script>
<style scope>
.staca {
  border-bottom: 1px solid #d7d7d7;
}
.black {
  background-color: black;
  color: white;
}
.week {
  padding: 2px;
}
</style>