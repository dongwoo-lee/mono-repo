<template>
  <div>
    <b-modal
      id="setScrRange"
      size="lg"
      v-model="Duration"
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
          <DxScrolling mode="virtual" />
          <DxColumn data-field="spotName" caption="SPOT명" />
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
          @click="showAdd"
        >
          추가</b-button
        ><b-button
          variant="outline-danger default cutom-label-cancel"
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
import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
import {
  DxDataGrid,
  DxScrolling,
  DxLoadPanel,
  DxColumn,
  DxSelection,
} from "devextreme-vue/data-grid";
import axios from "axios";
export default {
  components: {
    DxDataGrid,
    DxScrolling,
    DxLoadPanel,
    DxColumn,
    DxSelection,
  },
  computed: {
    ...mapState("ScrSpotDuration", {
      Duration: (state) => state.Duration,
      requestScr: (state) => state.requestScr,
    }),
    ...mapGetters("ScrSpotDuration", ["requestValid"]),
  },
  data() {
    return {
      key: ["ProductID", "spot"],
    };
  },
  methods: {
    ...mapMutations("ScrSpotDuration", [
      "hideDuration",
      "showAdd",
      "setSelectedScr",
      "deleteRequest",
    ]),
    requestSelect(v) {
      this.setSelectedScr(v.rowIndex);
    },
    request() {
      if (this.requestScr.length != 0) {
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
      } else {
        this.$emit("requestValid");
      }
    },
  },
};
</script>

<style></style>
