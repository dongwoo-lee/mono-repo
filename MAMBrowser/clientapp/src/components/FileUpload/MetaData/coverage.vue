<template>
  <div>
    <div style="margin-top: 35px; font-size: 15px">
      <b-form-group
        label="방송일"
        class="has-float-label"
        style="width: 190px; float: left"
      >
        <b-input-group class="mb-3" style="width: 190px">
          <input
            :disabled="isActive"
            id="dateinput"
            type="text"
            class="form-control input-picker date-input"
            :value="date"
            @input="onInput"
          />
          <b-input-group-append>
            <b-form-datepicker
              :value="date"
              @input="eventInput"
              button-only
              :disabled="isActive"
              :button-variant="getVariant"
              left
              aria-controls="example-input"
              @context="onContext"
            ></b-form-datepicker>
          </b-input-group-append>
        </b-input-group>
      </b-form-group>

      <b-form-group
        label="분류"
        class="has-float-label"
        style="margin-left: 20px; float: left"
      >
        <b-form-select
          :disabled="isActive"
          id="program-media"
          class="media-select"
          style="width: 120px; height: 37px"
          :value="coverageType"
          :options="coverageTypeOptions"
          @input="typeChange"
        />
      </b-form-group>
      <b-button
        :disabled="isActive"
        :variant="getVariant"
        style="width: 70px; margin-left: 20px"
        @click="onSearch"
        >검색</b-button
      >
    </div>
    <div style="font-family: 'MBC 새로움 M'; font-size: 15px">
      <div style="width: 425px; float: left">
        <b-form-group
          label="이벤트 명"
          class="has-float-label"
          style="margin-top: -10px"
        >
          <b-form-input
            style="width: 425px"
            class="editTask"
            v-model="EventSelected.eventName"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 425px; float: left; margin-top: 5px">
        <b-form-group
          label="이벤트 ID"
          class="has-float-label"
          style="margin-top: 5px"
        >
          <b-form-input
            style="width: 425px"
            class="editTask"
            v-model="EventSelected.productId"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
    </div>

    <div style="font-size: 15px; margin-top: 165px; margin-bottom: 0px">
      <b-form-group label="소재명" class="has-float-label">
        <b-form-input
          class="editTask"
          v-model="MetaData.title"
          :state="titleState"
          :maxLength="30"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="소재명"
          trim
        />
      </b-form-group>
      <p
        v-show="titleState"
        style="
          position: relative;
          left: 390px;
          top: -15px;
          width: 30px;
          margin-bottom: -18px;
          margin-right: 0px;
        "
      >
        {{ MetaData.title.length }}/30
      </p>
    </div>
    <div style="font-size: 15px; margin-top: 0px; float: left">
      <b-form-group label="메모" class="has-float-label">
        <b-form-input
          style="width: 200px"
          class="editTask"
          v-model="MetaData.memo"
          :state="memoState"
          :maxLength="30"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="메모"
          trim
        />
      </b-form-group>
      <p
        v-show="memoState"
        style="
          position: relative;
          left: 165px;
          top: -15px;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.memo.length }}/30
      </p>
    </div>
    <div
      style="font-size: 15px; margin-top: 0px; float: left; margin-left: 25px"
    >
      <b-form-group label="취재인" class="has-float-label">
        <b-form-input
          style="width: 200px"
          class="editTask"
          v-model="MetaData.reporter"
          :state="reporterState"
          :maxLength="10"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="취재인"
          trim
        />
      </b-form-group>
      <p
        v-show="reporterState"
        style="
          position: relative;
          left: 165px;
          top: -15px;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.reporter.length }}/10
      </p>
    </div>
    <b-modal
      size="lg"
      v-model="modal"
      centered
      hide-header-close
      no-close-on-esc
      no-close-on-backdrop
      footer-class="scr-modal-footer"
    >
      <template slot="modal-title">
        <h5>프로그램 선택</h5>
      </template>
      <template slot="default">
        <div>
          <b-form-group
            label="매체"
            class="has-float-label"
            style="font-size: 15px"
          >
            <b-form-select
              :disabled="isActive"
              id="program-media"
              class="media-select"
              style="width: 95px; height: 37px"
              :value="this.coverageMedia"
              @input="mediaChange"
              :options="fileMediaOptions"
            />
          </b-form-group>
          <DxDataGrid
            name="mcrDxDataGrid"
            style="
              height: 295px;
              border: 1px solid silver;
              font-family: 'MBC 새로움 M';
            "
            :data-source="EventData"
            :selection="{ mode: 'single' }"
            :show-borders="true"
            :hover-state-enabled="true"
            key-expr="id"
            :allow-column-resizing="true"
            :column-auto-width="true"
            no-data-text="No Data"
            @row-click="onRowClick"
          >
            <DxLoadPanel :enabled="true" />
            <DxScrolling mode="virtual" />
            <DxColumn data-field="eventName" caption="이벤트 명" />
            <DxColumn data-field="productId" caption="이벤트 ID" />
            <DxColumn data-field="onairTime" caption="방송시작 시간" />
          </DxDataGrid>
        </div>
      </template>
      <template v-slot:modal-footer>
        <b-button
          style="margin-left: 20px; height: 34px"
          variant="outline-primary"
          @click="modalOff"
        >
          확인
        </b-button>
        <b-button
          variant="outline-danger default cutom-label-cancel"
          size="sm"
          class="float-right"
          @click="modalReset"
        >
          닫기</b-button
        >
      </template>
    </b-modal>
  </div>
  <!-- <div>
    
    
    
  </div> -->
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
import { DxScrolling, DxLoadPanel } from "devextreme-vue/data-grid";
import axios from "axios";
export default {
  components: {
    CommonVueSelect,
    DxScrolling,
    DxLoadPanel,
  },
  mixins: [CommonFileFunction, MixinBasicPage],
  data() {
    return {
      modal: false,
      coverageType: "",
      coverageMedia: "",
    };
  },
  created() {
    this.reset();
    this.setTitle(this.sliceExt(30));
    this.resetCoverageTypeOptions();
    this.resetFileMediaOptions();
    axios.get("/api/categories/report").then((res) => {
      this.setCoverageTypeSelected(res.data.resultObject.data[0].id);
      this.coverageType = res.data.resultObject.data[0].id;

      res.data.resultObject.data.forEach((e) => {
        this.setCoverageTypeOptions({
          value: e.id,
          text: e.name,
        });
      });
    });

    axios.get("/api/categories/media").then((res) => {
      this.coverageMedia = res.data.resultObject.data[0].id;
      this.setMediaSelected(this.coverageMedia);
      res.data.resultObject.data.forEach((e) => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name,
        });
      });
    });

    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.setDate(today);
    this.setTempDate(today);

    this.getPro();
  },
  methods: {
    onSearch() {
      this.modalOn();
      this.setMediaSelected(this.coverageMedia);
      this.getPro();
    },
    modalOn() {
      this.modal = true;
    },
    modalOff() {
      this.modal = false;
    },
    modalReset() {
      this.resetEventSelected();
      this.modal = false;
    },
    typeChange(v) {
      this.setCoverageTypeSelected(v);
    },
    mediaChange(v) {
      this.setMediaSelected(v);
      this.getPro();
    },
  },
};
</script>

<style></style>
