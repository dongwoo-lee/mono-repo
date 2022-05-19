<template>
  <div>
    <div style="margin-top: 35px; font-size: 15px">
      <b-form-group
        label="방송일"
        class="has-float-label"
        style="width: 220px; float: left; margin-right: 20px"
      >
        <b-input-group class="mb-3" style="width: 220px; float: left">
          <input
            :disabled="isActive"
            id="dateinput"
            type="text"
            class="form-control input-picker"
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
        label="매체"
        class="has-float-label"
        style="width: 95px; float: left; margin-right: 20px"
      >
        <b-form-select
          :disabled="isActive"
          id="program-media"
          class="media-select"
          style="width: 95px; height: 36px"
          :value="mcrMedia"
          :options="fileMediaOptions"
          @input="mediaChange"
        />
      </b-form-group>
      <b-button
        :disabled="isActive"
        :variant="getVariant"
        style="width: 70px"
        @click="onSearch"
        >검색</b-button
      >
    </div>
    <div style="width: 430px; font-family: 'MBC 새로움 M'; font-size: 15px">
      <div style="width: 425px; float: left">
        <b-form-group
          label="이벤트 명"
          class="has-float-label"
          style="margin-top: -10px"
        >
          <b-form-input
            style="width: 425px"
            class="editTask"
            v-model="EventSelected.name"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 250px; float: left">
        <b-form-group
          label="이벤트 ID"
          class="has-float-label"
          style="margin-top: 10px"
        >
          <b-form-input
            style="width: 250px"
            class="editTask"
            v-model="EventSelected.id"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 155px; margin-left: 20px; float: left">
        <b-form-group
          label="편성 분량"
          class="has-float-label"
          style="margin-top: 10px"
        >
          <b-form-input
            style="width: 155px"
            class="editTask"
            v-model="EventSelected.duration"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
    </div>
    <div style="margin-top: 160px; height: 50px">
      <b-form-group
        label="메모"
        class="has-float-label"
        style="font-size: 15px"
      >
        <b-form-input
          class="editTask"
          v-model="MetaData.memo"
          :state="memoState"
          :maxLength="30"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="메모"
          trim
        />
      </b-form-group>
      <span
        v-show="memoState"
        style="
          position: relative;
          left: 390px;
          top: -15px;
          width: 30px;
          margin-right: 0px;
          font-size: 14px;
        "
      >
        {{ MetaData.memo.length }}/30
      </span>
    </div>
    <div style="font-size: 15px; margin-top: 15px">
      <b-form-group label="광고주" class="has-float-label">
        <b-form-input
          class="editTask"
          v-model="MetaData.advertiser"
          :state="advertiserState"
          :maxLength="15"
          aria-describedby="input-live-help input-live-feedback"
          placeholder="광고주"
          trim
        />
      </b-form-group>
      <p
        v-show="advertiserState"
        style="
          position: relative;
          left: 390px;
          top: -15px;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.advertiser.length }}/15
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
        <div v-show="this.MetaData.typeSelected == 'mcr-spot'">
          <DxDataGrid
            name="mcrDxDataGrid"
            v-show="this.EventData.id != ''"
            style="
              height: 280px;
              border: 1px solid silver;
              font-family: 'MBC 새로움 M';
            "
            :data-source="EventData"
            :selection="{ mode: 'single' }"
            :show-borders="false"
            :hover-state-enabled="true"
            key-expr="id"
            :allow-column-resizing="true"
            :column-auto-width="true"
            no-data-text="No Data"
            @row-click="onRowClick"
          >
            <tbody
              slot="rowTemplate"
              slot-scope="{
                data: {
                  data: { name, id, duration, audioClipID },
                },
              }"
              class="dx-row"
            >
              <tr>
                <td style="border-right: 1px solid #dddddd">{{ name }}</td>
                <td style="border-right: 1px solid #dddddd">{{ id }}</td>
                <td style="border-right: 1px solid #dddddd">{{ duration }}</td>
                <td
                  :class="
                    [this.getAudioClipID(audioClipID)] ? 'disabledCell' : ''
                  "
                  style="text-align: center"
                >
                  {{ audioClipID == null ? "" : "O" }}
                </td>
              </tr>
            </tbody>
            <!-- <DxLoadPanel :enabled="true" /> -->
            <DxSelection mode="single" />
            <DxScrolling mode="virtual" />
            <DxColumn data-field="name" caption="이벤트 명" />
            <DxColumn data-field="id" caption="이벤트 ID" />
            <DxColumn data-field="duration" caption="편성 분량" />
            <DxColumn data-field="audioClipID" :width="50" caption="파일" />
          </DxDataGrid>
        </div>
      </template>
      <template v-slot:modal-footer>
        <b-button
          variant="outline-primary default cutom-label"
          size="sm"
          class="float-right"
          @click="modalOff"
        >
          확인</b-button
        >
        <b-button
          variant="outline-danger default cutom-label-cancel"
          size="sm"
          class="float-right"
          @click="modalReset"
        >
          취소</b-button
        >
      </template>
    </b-modal>
  </div>
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import MixinFillerPage from "../../../mixin/MixinFillerPage";
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import { mapState, mapGetters, mapMutations } from "vuex";
import { DxSelection, DxScrolling } from "devextreme-vue/data-grid";
import axios from "axios";
export default {
  components: {
    CommonVueSelect,
    DxSelection,
    DxScrolling,
  },
  mixins: [CommonFileFunction, MixinBasicPage, MixinFillerPage],
  data() {
    return {
      modal: false,
      mcrMedia: "A",
      mediaName: "AM",
    };
  },
  created() {
    this.reset();
    this.getEditorForMd();
    this.resetFileMediaOptions();

    axios.get("/api/categories/media").then((res) => {
      res.data.resultObject.data.forEach((e) => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name,
        });
      });
    });
    this.mcrMedia = "A";
    this.setMediaSelected(this.mcrMedia);

    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.setDate(today);
    this.setTempDate(today);
    this.getPro();
  },
  methods: {
    getAudioClipID(audioClipID) {
      if (audioClipID != "") {
        return true;
      } else {
        return false;
      }
    },
    mediaChange(v) {
      this.setMediaSelected(v);
      var data = this.fileMediaOptions.find((dt) => dt.value == v);
      this.mediaName = data.text;
    },
    onSearch() {
      this.modalOn();
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
    getData() {},
  },
};
</script>

<style scoped>
.disabledCell {
  color: red !important;
}
.dx-row :hover {
  background-color: #f5f5f5;
}
</style>
