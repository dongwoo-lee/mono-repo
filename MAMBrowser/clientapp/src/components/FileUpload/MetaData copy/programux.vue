<template>
  <div>
    <div style="margin-top: 35px">
      <b-form-group
        label="방송일"
        class="has-float-label"
        style="font-color: black; font-size: 15px; float: left"
      >
        <b-input-group
          class="mb-3"
          style="width: 220px; margin-right: 20px; float: left"
        >
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
              right
              aria-controls="example-input"
              @context="onContext"
            ></b-form-datepicker>
          </b-input-group-append>
        </b-input-group>
      </b-form-group>

      <b-form-group
        label="매체"
        class="has-float-label"
        style="margin-right: 20px; font-size: 15px; float: left"
      >
        <b-form-select
          :disabled="isActive"
          id="program-media"
          class="media-select"
          style="width: 95px; height: 37px"
          :value="this.programMedia"
          @input="mediaChange"
          :options="fileMediaOptions"
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
    <div
      style="
        width: 550px;
        margin-top: 20px;
        font-size: 16px;
        font-family: 'MBC 새로움 M';
      "
    >
      <div style="width: 425px; float: left; margin-top: -10px">
        <b-form-group label="이벤트 명" class="has-float-label">
          <b-form-input
            class="editTask"
            style="width: 425px"
            v-model="this.ProgramSelected.eventName"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 425px; float: left; margin-top: 10px">
        <b-form-group label="방송 시간" class="has-float-label">
          <b-form-input
            style="width: 425px"
            class="editTask"
            v-model="this.ProgramSelected.onairTime"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
      <div style="width: 425px; float: left; margin-top: 5px">
        <b-form-group label="편성 분량" class="has-float-label">
          <b-form-input
            style="width: 425px"
            class="editTask"
            v-model="this.ProgramSelected.durationSec"
            disabled
            aria-describedby="input-live-help input-live-feedback"
            trim
          />
        </b-form-group>
      </div>
    </div>
    <div style="height: 50px; margin-top: 225px">
      <b-form-group
        label="메모"
        class="has-float-label"
        style="font-size: 15px"
      >
        <b-form-input
          class="editTask"
          v-model="MetaData.memo"
          :state="memoState"
          :maxLength="200"
          placeholder="메모"
          trim
        />
      </b-form-group>
      <p style="margin-left: 392px; margin-top: -15px" v-show="memoState">
        {{ MetaData.memo.length }}/30
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
        <div v-show="this.MetaData.typeSelected == 'program'">
          <DxDataGrid
            ref="my-proDataGrid"
            v-show="this.ProgramData.eventName != ''"
            style="
              height: 280px;
              border: 1px solid silver;
              font-family: 'MBC 새로움 M';
            "
            :data-source="ProgramData"
            :selection="{ mode: 'single' }"
            :show-borders="false"
            :hover-state-enabled="true"
            key-expr="productId"
            :allow-column-resizing="true"
            :column-auto-width="true"
            no-data-text="No Data"
            @row-click="onRowClick"
          >
            <tbody
              slot="rowTemplate"
              slot-scope="{
                data: {
                  data: { eventName, productId, onairTime, durationSec },
                },
              }"
              class="dx-row"
            >
              <tr
                v-if="
                  !userProgramList.includes(productId) &&
                  eventName != '' &&
                  role != 'ADMIN'
                "
                :class="[getProductId(productId)] ? 'disabledRow' : ''"
              >
                <td>{{ eventName }}</td>
                <!-- <td>{{ eventType }}</td> -->
                <td>{{ productId }}</td>
                <td>{{ onairTime }}</td>
                <td>{{ durationSec }}</td>
              </tr>
              <tr
                v-if="
                  (userProgramList.includes(productId) && eventName != '') ||
                  role == 'ADMIN'
                "
              >
                <!-- <td><b-icon-alarm></b-icon-alarm> 아이콘 추가</td> -->
                <td>{{ eventName }}</td>
                <!-- <td>{{ eventType }}</td> -->
                <td>{{ productId }}</td>
                <td>{{ onairTime }}</td>
                <td>{{ durationSec }}</td>
              </tr>
            </tbody>
            <DxColumn data-field="eventName" caption="이벤트 명" />
            <!-- <DxColumn :width="50" data-field="eventType" caption="타입" /> -->
            <DxColumn
              :width="95"
              data-field="productId"
              caption="프로그램 ID"
            />
            <DxColumn data-field="onairTime" caption="방송 시간" />
            <DxColumn
              :width="80"
              data-field="durationSec"
              caption="편성 분량"
            />
            <DxSelection mode="single" />
            <DxScrolling mode="virtual" />
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
import CommonVueSelect from "../../Form/CommonVueSelect.vue";
import MixinBasicPage from "../../../mixin/MixinBasicPage";
import { mapState, mapGetters, mapMutations } from "vuex";
import { DxScrolling, DxLoadPanel } from "devextreme-vue/data-grid";
import axios from "axios";
const dxdg = "my-proDataGrid";
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
      programMedia: "A",
      mediaName: "AM",
      dxdg,
    };
  },
  created() {
    this.reset();
    this.getEditorForPd();
    this.resetFileMediaOptions();

    axios.get("/api/categories/media").then((res) => {
      this.programMedia = res.data.resultObject.data[0].id;
      this.setMediaSelected(this.programMedia);
      res.data.resultObject.data.forEach((e) => {
        this.setFileMediaOptions({
          value: e.id,
          text: e.name,
        });
      });
    });

    var user_id = sessionStorage.getItem("user_id");
    axios
      .get(
        `/api/categories/user-pgmcodes?userId=${user_id}&media=${this.MetaData.mediaSelected}`
      )
      .then((res) => {
        this.setUserProgramList(res.data.resultObject.data);
      });

    const today = this.$fn.formatDate(new Date(), "yyyy-MM-dd");
    this.setDate(today);
    this.setTempDate(today);

    this.getPro();
  },
  computed: {
    getTitle() {
      return `[${this.date}] [${this.mediaName}] [${this.ProgramSelected.eventName}]`;
    },
    getProgramId(productId) {
      if (this.userProgramList.includes(productId)) {
        return true;
      } else {
        return false;
      }
    },
    proDataGrid: function () {
      return this.$refs[dxdg].instance;
    },
  },
  methods: {
    mediaChange(v) {
      this.setMediaSelected(v);
      var data = this.fileMediaOptions.find((dt) => dt.value == v);
      this.mediaName = data.text;
      this.getPro();
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
      this.resetProgramSelected();
      this.modal = false;
    },
  },
};
</script>

<style scoped>
.disabledRow {
  color: silver !important;
}
.dx-row :hover {
  background-color: #f5f5f5;
}
</style>
