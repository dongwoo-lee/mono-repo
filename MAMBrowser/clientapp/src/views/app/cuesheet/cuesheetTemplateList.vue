<template>
  <div>
    <b-row>
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
        <!-- 검색 버튼 -->
        <b-form-group>
          <b-button variant="outline-primary default" @click="onSearch"
            >검색</b-button
          >
        </b-form-group>
      </template>

      <!-- 버튼 -->
      <template slot="form-btn-area">
        <b-input-group>
          <b-button
            variant="outline-primary default"
            size="sm"
            v-b-modal.modal-add
            >템플릿 추가</b-button
          >
        </b-input-group>
        <b-input-group>
          <b-button
            variant="outline-secondary default"
            size="sm"
            v-b-modal.modal-del
            >선택 항목 삭제</b-button
          >
        </b-input-group>
      </template>

      <!-- 테이블 페이지 -->
      <template slot="form-table-page-area">
        {{ getPageInfo() }}
      </template>
      <template slot="form-table-area">
        <!-- 테이블 -->
        <common-data-table-scroll-paging
          ref="scrollPaging"
          :fields="fields"
          :rows="templateList"
          :per-page="responseData.rowPerPage"
          :totalCount="responseData.totalRowCount"
          is-actions-slot
          isWeekSlot="true"
          :num-rows-to-bottom="5"
          :isTableLoading="isTableLoading"
          @scrollPerPage="onScrollPerPage"
          @selectedIds="onSelectedIds"
          @sortableclick="onSortable"
          @refresh="onRefresh"
        >
          <template slot="actions" scope="props">
            <common-actions :rowData="props.props.rowData" :widgetIndex="16">
            </common-actions>
          </template>
        </common-data-table-scroll-paging>
      </template>
    </common-form>

    <!-- 템플릿 추가 modal -->
    <b-modal
      id="modal-add"
      size="lg"
      centered
      title="템플릿 추가"
      ok-title="확인"
      cancel-title="취소"
      @ok="addTemCue"
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
            <DxTextBox
              placeholder="이름없는 템플릿"
              width="320px"
              v-model="temData.tmptitle"
            />
          </div>
        </div>
      </div>
    </b-modal>
    <!-- 템플릿 삭제 modal -->
    <b-modal
      id="modal-del"
      size="lg"
      centered
      title="템플릿 삭제"
      ok-title="확인"
      cancel-title="취소"
      @ok="delTemCue"
    >
      <div id="modelDiv" class="d-block text-center">
        <div class="m-3" style="font-size: 20px">
          <div>선택된 템플릿을 삭제하시겠습니까?</div>
        </div>
      </div>
    </b-modal>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import { USER_ID } from "@/constants/config";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import { DxDataGrid, DxColumn } from "devextreme-vue/data-grid";
import DxTextBox from "devextreme-vue/text-box";
import axios from "axios";
import "moment/locale/ko";
const moment = require("moment");
const userId = sessionStorage.getItem(USER_ID);
const qs = require("qs");

export default {
  mixins: [MixinBasicPage],
  components: {
    DxDataGrid,
    DxColumn,
    DxTextBox,
  },
  data() {
    return {
      templateList: [],
      searchItems: {
        rowPerPage: 30,
        selectPage: 1,
      },
      temData: {
        personid: "",
        tmptitle: "이름없는 템플릿",
      },
      fields: [
        {
          name: "__checkbox",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "3%",
        },
        {
          name: "createtime",
          title: "생성일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "20%",
          callback: (value) => {
            return value === null
              ? ""
              : moment(value, "YYYYMMDDHH:mm:ss").format(
                  "YYYY-MM-DD : HH시 mm분"
                );
          },
        },

        {
          name: "edittime",
          title: "수정일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          width: "20%",
          callback: (value) => {
            return value === null
              ? ""
              : moment(value, "YYYYMMDDHH:mm:ss").format(
                  "YYYY-MM-DD : HH시 mm분"
                );
          },
        },
        {
          name: "tmptitle",
          title: "템플릿 이름",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
        },
        {
          name: "__slot:actions",
          title: "",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "13%",
        },
      ],
    };
  },

  computed: {
    // ...mapGetters("cuesheet", ["templateList"]),
  },

  mounted() {
    this.selectedIds = [];
  },
  methods: {
    async getData() {
      this.isTableLoading = this.isScrollLodaing ? false : true;
      var temList = await this.getTemList();
      if (temList) {
        var seqnum = 0;
        this.setResponseData(temList);
        temList.data.forEach((ele) => {
          ele.seq = seqnum;
          seqnum = seqnum + 1;
        });
        this.addScrollClass();
        this.isTableLoading = false;
        this.isScrollLodaing = false;
        this.templateList = temList.data;
      }
    },
    //템플릿 목록 가져오기
    getTemList() {
      return axios.get(`/api/TempCueSheet/GetTempList?personid=${userId}`);
    },
    // 템플릿 추가 (modal)
    async addTemCue() {
      this.temData.personid = userId;
      var pram = {
        temParam: this.temData,
      };
      await axios.post(`/api/TempCueSheet/SaveTempCue`, pram).then((res) => {});
      this.getData();
    },
    //템플릿 삭제
    async delTemCue() {
      if (this.selectedIds.length > 0) {
        var delcueidList = [];
        this.selectedIds.forEach((ids) => {
          delcueidList.push(this.templateList[ids].cueid);
        });
        await axios.delete(`/api/TempCueSheet/DelTempCue`, {
          params: {
            tempids: delcueidList,
          },
          paramsSerializer: (params) => {
            return qs.stringify(params);
          },
        });
        this.getData();
        this.initSelectedIds();
      }
    },
  },
};
</script>
<style scope>
</style>