<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb heading="기본 큐시트" />
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
        <!-- 채널 -->
        <b-form-group label="매체" class="has-float-label">
          <b-form-select
            class="width-140"
            v-model="searchItems.channel"
            :options="[
              { value: '', text: '전체' },
              { value: 'Y', text: 'FM4U' },
              { value: 'N', text: '표준FM' },
            ]"
          />
        </b-form-group>
        <!-- 프로그램명 -->
        <b-form-group label="프로그램명" class="has-float-label">
          <b-form-select
            class="width-230"
            v-model="searchItems.programName"
            :options="[
              { value: '', text: '전체' },
              { value: 'Y', text: '김이나의 별이 빛나는 밤에' },
              { value: 'N', text: '두시의 데이트 뮤지, 안영미입니다' },
            ]"
          />
        </b-form-group>
        <!-- 수정일 -->
        <b-form-group label="수정일" class="has-float-label">
          <common-date-picker />
        </b-form-group>
        <!-- 상태 -->
        <b-form-group label="요일" class="has-float-label">
          <b-form-select
            class="width-140"
            v-model="searchItems.weekData"
            :options="[
              { value: '', text: '전체' },
              { value: 'mon', text: '월' },
              { value: 'tue', text: '화' },
              { value: 'wed', text: '수' },
              { value: 'thu', text: '목' },
              { value: 'fri', text: '금' },
              { value: 'sat', text: '토' },
              { value: 'sun', text: '일' },
            ]"
          />
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
            v-b-modal.modal-center
            >기본 큐시트 추가</b-button
          >
        </b-input-group>
        <b-input-group>
          <b-button variant="outline-secondary default" size="sm"
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
          tableHeight="525px"
          :fields="fields"
          :rows="cuesheetSchedule_d"
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
            <common-actions :rowData="props.props.rowData"> </common-actions>
          </template>
        </common-data-table-scroll-paging>
      </template>
    </common-form>

    <b-modal
      id="modal-center"
      size="lg"
      centered
      title="기본 큐시트 추가"
      ok-title="확인"
      cancel-title="취소"
    >
      <div id="modelDiv" class="d-block text-center">
        <div class="mb-3 mt-3" style="font-size: 20px">
          <div
            v-if="test == '두시의 데이트 뮤지, 안영미입니다' && test2 == '전체'"
          >
            기본 큐시트를 적용할 요일을 선택하세요.
          </div>
          <div v-else>기본 큐시트를 추가할 프로그램을 선택하세요.</div>
          <div
            v-if="test == '김이나의 별이 빛나는 밤에' && test2 == '전체'"
            style="font-size: 16px; color: red"
          >
            ※ 이미 모든 요일에 기본 큐시트가 적용되어 있습니다.
          </div>
        </div>
        <div class="dx-fieldset">
          <div class="dx-field">
            <div class="dx-field-label" style="font-size: 15px">프로그램 :</div>
            <div class="dx-field-value">
              <DxSelectBox
                width="320px"
                :items="simpleProducts"
                :value.sync="test"
                placeholder=""
              />
            </div>
          </div>
          <div class="dx-field">
            <div class="dx-field-label" style="font-size: 15px">매체 :</div>
            <div class="dx-field-value">
              <DxSelectBox
                width="320px"
                :items="simpleProducts2"
                :value.sync="test2"
                placeholder=""
              />
            </div>
          </div>
        </div>

        <div
          v-if="test == '김이나의 별이 빛나는 밤에' && test2 == '전체'"
          class="pt-0"
        >
          <template>
            <div>
              <div style="color: white">
                ※ 이미 모든 요일에 기본 큐시트가 적용되어 있습니다.
              </div>
              <DxDataGrid
                id="gridContainer"
                :data-source="cuesheetSchedule_d"
                height="200px"
                :show-borders="true"
                :showRowLines="true"
                keyExpr="updateDate"
              >
                <DxColumn caption="매체" width="80px" data-field="channel" />
                <DxColumn caption="프로그램 명" data-field="programName" />
                <DxColumn
                  caption="수정일"
                  width="150px"
                  data-field="updateDate"
                />
                <DxColumn
                  caption="적용요일"
                  width="170px"
                  data-field="updateDate"
                  cell-template="cellTemplate"
                />
                <template #cellTemplate>
                  <div>
                    <span class="week">월</span>
                    <span class="week">화</span>
                    <span class="week">수</span>
                    <span class="week">목</span>
                    <span class="week">금</span>
                    <span class="week">토</span>
                    <span class="week">일</span>
                  </div>
                </template>
              </DxDataGrid>
            </div>
          </template>
        </div>
        <div
          v-if="test == '두시의 데이트 뮤지, 안영미입니다' && test2 == '전체'"
          class="weekBtn"
        >
          <div class="testclass">
            <span class="week2">월</span>
            <span class="week2">화</span>
            <span class="week2">수</span>
            <span class="week2">목</span>
            <span class="week2">금</span>
            <span class="week2">토</span>
            <span class="week2">일</span>
          </div>
          <div class="testclass2">
            <DxCheckBox class="week3" :value="true" />
            <DxCheckBox class="week3" :value="true" />
            <DxCheckBox class="week3" :value="true" />
            <DxCheckBox class="week3" :value="true" />
            <DxCheckBox class="week3" :value="true" />
            <DxCheckBox class="week3" :value="true" />
            <DxCheckBox class="week3" :value="true" />
          </div>
        </div>
      </div>
    </b-modal>
  </div>
</template>

<script>
import { DxCheckBox } from "devextreme-vue/check-box";
import DxSelectBox from "devextreme-vue/select-box";
import { mapGetters } from "vuex";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import { DxDataGrid, DxColumn } from "devextreme-vue/data-grid";
export default {
  mixins: [MixinBasicPage],
  components: {
    DxSelectBox,
    DxDataGrid,
    DxColumn,
    DxCheckBox,
  },
  data() {
    return {
      test: null,
      test2: null,
      date: null,
      simpleProducts: [
        "김이나의 별이 빛나는 밤에",
        "두시의 데이트 뮤지, 안영미입니다",
      ],
      simpleProducts2: ["전체", "표준FM", "FM4U"],
      searchItems: {
        updateDate: "", // 수정일
        channel: "", // 채널
        programName: "", // 프로그램명
        weekData: "", // 작성상태
      },
      proOptions: [],
      fields: [
        {
          name: "channel",
          title: "매체",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%",
          sortField: "duration",
        },
        {
          name: "programName",
          title: "프로그램명",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center bold",
          sortField: "categoryName",
        },
        {
          name: "updateDate",
          title: "수정일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "18%",
        },
        {
          name: "__slot:weekrow",
          title: "적용 요일",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "18%",
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
    ...mapGetters("cuesheet", ["cuesheetSchedule_d"]),
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
.weekBtn {
  border: solid 0.5px #008ecc;
  border-radius: 2px;
  width: 700px;
  height: 150px;
  margin: auto;
  padding: auto;
}
.staca {
  border-bottom: 1px solid #d7d7d7;
}

.week3 {
  padding: 15.5px;
}
.testclass {
  margin-top: 30px;
}
.week2 {
  float: inline-start;
  padding: 15px;
  font-size: 25px;
}
.black {
  background-color: black;
  color: white;
}
.week {
  padding: 2px;
}
</style>