<template>
  <div>
    <b-modal
      id="setScrRange"
      size="lg"
      v-model="show"
      centered
      hide-header-close
      no-close-on-esc
      no-close-on-backdrop
      footer-class="scr-modal-footer"
    >
      <template slot="modal-title">
        <h5>부조SPOT 방송의뢰</h5>
      </template>
      <template slot="default">
        <DxDataGrid
          :data-source="requestScr"
          :show-borders="true"
          style="height: 200px"
          :hover-state-enabled="true"
          :key-expr="key"
          :allow-column-resizing="true"
          :column-auto-width="true"
          no-data-text="No Data"
          @row-click="requestSelect"
        >
          <DxSelection mode="single" />
          <DxPager :visible="false" />
          <DxScrolling mode="standard" />
          <DxColumn data-field="spot" caption="SPOT명" />
          <DxColumn data-field="ProductID" caption="사용처" />
          <DxColumn data-field="StartDate" caption="시작일" />
          <DxColumn data-field="EndDate" caption="종료일" />
        </DxDataGrid>
      </template>
      <template v-slot:modal-footer>
        <b-button
          variant="outline-primary default cutom-label"
          size="sm"
          class="float-left"
          style="margin-right: 5px"
          @click="showAddDuration"
        >
          추가</b-button
        ><b-button
          variant="outline-primary default cutom-label-cancel"
          size="sm"
          class="float-left"
          style="margin-right: 480px"
          @click="deleteRequest"
        >
          삭제</b-button
        >
        <b-button
          variant="outline-primary default cutom-label"
          size="sm"
          class="float-right"
          @click="request"
        >
          방송의뢰</b-button
        >
        <b-button
          variant="outline-danger default cutom-label-cancel"
          size="sm"
          class="float-right"
          @click="hideDuration"
        >
          취소</b-button
        >
        <!-- 여기에다가 편집 저장 버튼 추가해야함 그리고 거기에 Click이벤트로 SOM, EOM 찍히는지 확인하기 -->
      </template>
    </b-modal>
  </div>
</template>

<script>
import {
  DxDataGrid,
  DxScrolling,
  DxLoadPanel,
  DxColumn,
  DxPager,
  DxSelection,
} from "devextreme-vue/data-grid";
import axios from "axios";
export default {
  components: {
    DxDataGrid,
    DxScrolling,
    DxLoadPanel,
    DxColumn,
    DxPager,
    DxSelection,
  },
  props: {
    show: {
      type: Boolean,
      default: false,
    },
    Scr: {
      type: Array,
      default: [],
    },
  },
  data() {
    return {
      key: ["event", "spot"],
      requestScr: this.Scr,
      selectedScr: "",
    };
  },
  methods: {
    hideDuration() {
      this.$emit("hideDuration");
    },
    showAddDuration() {
      this.$emit("showAddDuration");
    },
    requestSelect(v) {
      this.selectedScr = v.data;
    },
    request() {
      console.log(this.requestScr);
      axios
        .post(`/api/Mastering/scr-spot/duration`, this.requestScr)
        .then((res) => {
          console.log(res);
          if (res.status == 200 && res.data.errorMsg == null) {
            this.$emit("setDurationSuccess");
          } else {
            this.$emit("setDurationFail", res.data.errorMsg);
          }
        });
    },
    deleteRequest() {
      this.$emit("deleteRequest", this.selectedScr);
    },
  },
};
</script>

<style></style>
