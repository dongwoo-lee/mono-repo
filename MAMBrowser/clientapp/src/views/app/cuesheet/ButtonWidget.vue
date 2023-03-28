<template>
  <div>
    <div>
      <DxButton
        icon="arrowleft"
        @click="
          $router.push({ path: '/app/cuesheet/' + goBackPoint + '/list' })
        "
        hint="목록으로"
        v-if="!fav"
      />
      <DxButton
        icon="doc"
        v-b-modal.modal-clear
        hint="새큐시트"
        v-if="!fav && type != 'A'"
      />
      <DxButton
        icon="trash"
        v-b-modal.modal-favDel
        hint="카트 비우기"
        v-if="fav"
      />
      <DxButton
        icon="columnchooser"
        v-b-modal.modal-template
        hint="템플릿으로 저장"
        v-if="!fav"
      />
      <DxDropDownButton
        :items="activefolderitem"
        style="background-color: #ffffff"
        width="36"
        :drop-down-options="{ width: 160 }"
        icon="activefolder"
        :showArrowIcon="false"
        hint="가져오기"
        @item-click="onActivefolderItemClick"
        v-if="!fav && type != 'A'"
      />
      <DxDropDownButton
        :items="downloaditem"
        display-expr="name"
        key-expr="id"
        style="background-color: #ffffff"
        width="36"
        :drop-down-options="{ width: 130 }"
        icon="download"
        :showArrowIcon="false"
        hint="내려받기"
        @item-click="onDownloadItemClick"
        v-if="!fav"
      />
      <DxButton
        icon="checklist"
        style="background-color: #ffffff"
        type="default"
        styling-mode="outlined"
        hint="적용요일 변경"
        v-if="type == 'B'"
        @click="editWeekListClick"
      />
      <DxButton
        icon="save"
        type="default"
        v-b-modal.modal-save
        hint="저장"
        :text="saveText"
        v-if="type != 'A'"
      />
    </div>

    <!-- 새큐시트 -->
    <b-modal
      id="modal-clear"
      size="lg"
      centered
      title="새큐시트"
      ok-title="확인"
      cancel-title="취소"
      @ok="clearOk"
    >
      <div class="d-block">
        <b-form-group class="clear-check-title">
          <b-form-checkbox
            :value="true"
            v-model="allCheck"
            :unchecked-value="false"
            @change="clickCheckTilte"
          >
            작성된 모든 내용을 삭제합니다.
          </b-form-checkbox>
        </b-form-group>
        <b-form-group class="clear-check-item">
          <b-form-checkbox-group
            v-model="selected"
            :options="options"
            class="mb-3"
            value-field="value"
            text-field="name"
            disabled-field="notEnabled"
          ></b-form-checkbox-group>
          <b-form-checkbox-group
            v-model="cartSelected"
            :options="cartOptions"
            class="mb-3"
            value-field="value"
            text-field="name"
            disabled-field="notEnabled"
          ></b-form-checkbox-group>
        </b-form-group>
      </div>
      <template #modal-footer="{ cancel }">
        <DxButton
          :width="100"
          text="취소"
          :disabled="loadingIconVal"
          @click="cancel()"
        />
        <DxButton
          type="default"
          text="확인"
          styling-mode="outlined"
          :disabled="loadingIconVal"
          :width="100"
          @click="clearOk()"
        >
        </DxButton>
      </template>
    </b-modal>

    <!-- 즐겨찾기 비우기 -->
    <b-modal
      id="modal-favDel"
      size="lg"
      centered
      title="즐겨찾기 카트 비우기"
      ok-title="확인"
      cancel-title="취소"
      @ok="favDelOk"
    >
      <div class="d-block text-center">
        <div class="mb-3 mt-3" style="font-size: 20px">
          <div class="mb-3">작성된 모든 내용을 삭제합니다.</div>
        </div>
      </div>
      <template #modal-footer="{ cancel }">
        <DxButton
          :width="100"
          text="취소"
          :disabled="loadingIconVal"
          @click="cancel()"
        />
        <DxButton
          type="default"
          text="확인"
          styling-mode="outlined"
          :disabled="loadingIconVal"
          :width="100"
          @click="favDelOk()"
        >
        </DxButton>
      </template>
    </b-modal>

    <!-- 템플릿으로 저장 -->
    <b-modal
      id="modal-template"
      size="lg"
      centered
      title="템플릿으로 저장"
      ok-title="확인"
      cancel-title="닫기"
      @ok="addtemClick"
      @show="resetModal_tem"
      @hidden="resetModal_tem"
    >
      <div id="modelDiv" class="d-block text-center">
        <div class="mb-3 mt-3" style="font-size: 20px">
          현재 큐시트를 템플릿으로 저장합니다.
        </div>
        <b-spinner v-if="loadingIconVal" small variant="primary"></b-spinner>
        <div>
          <div
            class="dx-field-label mt-3 mb-5 pl-5 pr-0"
            style="font-size: 15px"
          >
            템플릿 명 :
          </div>
          <div class="dx-field-value mt-3 mb-5 pr-5">
            <DxTextBox
              :placeholder="templateTitle"
              width="320px"
              :maxLength="40"
              v-model="tmpTitleTextBoxValue"
            />
          </div>
        </div>
      </div>
      <template #modal-footer="{ cancel }">
        <DxButton
          :width="100"
          text="취소"
          :disabled="loadingIconVal"
          @click="cancel()"
        />
        <DxButton
          type="default"
          text="확인"
          styling-mode="outlined"
          :disabled="loadingIconVal"
          :width="100"
          @click="addtemClick()"
        >
        </DxButton>
      </template>
    </b-modal>

    <!-- 가져오기 -->
    <common-import-tem :id="id" @settings="(val) => (editOptions = val)" />
    <common-import-def
      :proid="proid"
      :type="type"
      @settings="(val) => (editOptions = val)"
    />
    <common-import-archive @settings="(val) => (editOptions = val)" />

    <!-- 큐시트 저장 -->
    <b-modal
      id="modal-save"
      size="lg"
      centered
      title="저장"
      ok-title="확인"
      cancel-title="취소"
      @show="modalSaveOpen"
      @ok="saveOk"
    >
      <div class="d-block text-center">
        <div class="mb-3 mt-3" style="font-size: 20px">
          <div class="mb-3" v-if="cueInfo.cuetype == 'D' && !fav">
            "{{ cueInfo.title }}" 큐시트를 저장합니다.
            <h5 class="pt-2" v-if="isMyDiskExist">
              ( My 공간 소재는 큐시트에 저장 되지 않습니다. )
            </h5>
          </div>
          <div class="mb-3" v-if="cueInfo.cuetype == 'B' && !fav">
            "{{ cueInfo.title }}" 기본 큐시트를 저장합니다.
            <h5 class="pt-2" v-if="isMyDiskExist">
              ( My 공간 소재는 큐시트에 저장 되지 않습니다. )
            </h5>
          </div>
          <div class="mb-3" v-if="cueInfo.cuetype == 'T' && !fav">
            "{{ cueInfo.title }}" 템플릿을 저장합니다.
          </div>
          <div class="mb-3" v-if="fav">즐겨찾기를 저장합니다.</div>
          <b-spinner v-if="loadingIconVal" small variant="primary"></b-spinner>
          <div v-if="type != 'O' && cueInfo.cuetype == 'D' && !fav">
            <!-- <b-form-checkbox-group
              class="custom-checkbox-group mt-5"
              style="font-size: 16px"
              v-model="oldCueSelected"
              :options="oldCueOptions"
              value-field="value"
              text-field="text"
            /> -->
          </div>
        </div>
      </div>
      <template #modal-footer="{ cancel }">
        <DxButton
          :width="100"
          text="취소"
          :disabled="loadingIconVal"
          @click="cancel()"
        />
        <DxButton
          type="default"
          text="확인"
          styling-mode="outlined"
          :disabled="loadingIconVal"
          :width="100"
          @click="saveOk()"
        >
        </DxButton>
      </template>
    </b-modal>

    <!-- 추가설정 -->
    <b-modal
      id="modal-setting"
      size="lg"
      centered
      title="추가설정"
      ok-title="저장"
      cancel-title="닫기"
      @show="resetModal"
      @hidden="resetModal"
    >
      <div class="settingDiv">
        <div class="dx-fieldset">
          <div class="dx-field">
            <div class="dx-field-label" style="font-size: 15px">DJ 명 :</div>
            <div class="dx-field-value">
              <DxTextBox
                width="320px"
                v-model="editOptions.djname"
                :maxLength="20"
                :disabled="cueInfo.cuetype == 'A'"
              />
            </div>
          </div>
          <div class="dx-field">
            <div class="dx-field-label" style="font-size: 15px">구성 :</div>
            <div class="dx-field-value">
              <DxTextBox
                width="320px"
                :maxLength="20"
                v-model="editOptions.membername"
                :disabled="cueInfo.cuetype == 'A'"
              />
            </div>
          </div>
          <div class="dx-field">
            <div class="dx-field-label" style="font-size: 15px">연출 :</div>
            <div class="dx-field-value">
              <DxTextBox
                width="320px"
                :maxLength="20"
                v-model="editOptions.directorname"
                :disabled="cueInfo.cuetype == 'A'"
              />
            </div>
          </div>
          <div class="dx-field">
            <div class="dx-field-label" style="font-size: 15px">머리글 :</div>
            <div class="dx-field-value">
              <DxTextArea
                :height="50"
                :maxLength="45"
                width="320px"
                v-model="editOptions.headertitle"
                :disabled="cueInfo.cuetype == 'A'"
              />
            </div>
          </div>
          <div class="dx-field">
            <div class="dx-field-label" style="font-size: 15px">바닥글 :</div>
            <div class="dx-field-value">
              <DxTextArea
                :height="100"
                :maxLength="90"
                width="320px"
                v-model="editOptions.footertitle"
                :disabled="cueInfo.cuetype == 'A'"
              />
            </div>
          </div>
        </div>
      </div>
      <template #modal-footer="{ cancel }">
        <DxButton
          :width="100"
          text="취소"
          :disabled="loadingIconVal"
          @click="cancel()"
        />
        <DxButton
          type="default"
          text="확인"
          styling-mode="outlined"
          :disabled="loadingIconVal"
          v-if="cueInfo.cuetype != 'A'"
          :width="100"
          @click="editOk()"
        >
        </DxButton>
      </template>
    </b-modal>

    <!-- ExportZip -->
    <b-modal
      ref="modal-export-zip"
      size="lg"
      centered
      title="큐시트 로컬 저장"
      ok-title="확인"
      cancel-title="취소"
      @ok="exportZip"
    >
      <div class="d-block text-center">
        <div class="mb-3 mt-3" style="font-size: 20px">
          <div class="mb-3">작성한 큐시트를 zip 파일로 묶어 저장합니다.</div>
          <b-spinner v-if="loadingIconVal" small variant="primary"></b-spinner>
        </div>
      </div>
      <template #modal-footer="{ cancel }">
        <DxButton
          :width="100"
          text="취소"
          :disabled="loadingIconVal"
          @click="cancel()"
        />
        <DxButton
          type="default"
          styling-mode="outlined"
          text="확인"
          :width="100"
          :height="30"
          :disabled="loadingIconVal"
          @click="exportZip()"
        >
        </DxButton>
      </template>
    </b-modal>

    <!-- ExportWav -->
    <b-modal
      ref="modal-export-wav"
      size="xl"
      centered
      title="Wav로 저장(A,B)"
      ok-title="확인"
      cancel-title="취소"
      @ok="OnExportWav"
      @hidden="OnWavExportCancel"
    >
      <div>
        <WavExportAbCart @isTotalDuration="isTotalDuration" />
      </div>
      <div>
        <DxProgressBar
          id="progress-bar-status"
          :min="0"
          :max="100"
          :status-format="statusFormat"
          :value.sync="seconds"
          width="100%"
        />
      </div>
      <template #modal-footer="{}">
        <div v-if="isMyDiskCheckBox" class="mydisc-check-box">
          <DxCheckBox v-model="isWavCopy" :text="labelText" />
        </div>
        <DxButton :width="100" text="취소" @click="OnWavExportCancel" />
        <DxButton
          type="default"
          styling-mode="outlined"
          text="확인"
          :width="100"
          :height="30"
          :disabled="!isWavExportDisabled"
          @click="OnExportWav()"
        >
        </DxButton>
      </template>
    </b-modal>

    <!-- 기본큐시트 요일 변경 -->
    <b-modal
      ref="modal-editWeek"
      size="lg"
      centered
      title="적용요일 변경"
      ok-title="확인"
      cancel-title="닫기"
      @hidden="resetModal_weekedit"
    >
      <div id="modelDiv" class="d-block text-center">
        <div class="modal_search">
          <b-form-group label="매체" class="has-float-label">
            <b-form-select
              style="width: 150px"
              v-model="cueInfo.media"
              :options="mediaOptions"
              disabled
            />
          </b-form-group>
          <b-form-group label="프로그램명" class="has-float-label ml-3">
            <b-form-select
              style="width: 400px"
              v-model="cueInfo.productid"
              :options="programOptions"
              disabled
            />
          </b-form-group>
        </div>
        <div class="modal_week_form">
          <b-button-group size="sm">
            <b-button
              v-for="(btn, idx) in weekButtons"
              :key="idx"
              :pressed.sync="btn.state"
              :disabled="btn.disable"
              class="m-2 p-3"
              style="font-size: 16px; border-radius: 20% !important"
              variant="outline-primary"
            >
              {{ btn.caption }}
            </b-button>
          </b-button-group>
        </div>
      </div>
      <template #modal-footer="{ cancel }">
        <DxButton
          :width="100"
          text="취소"
          :disabled="loadingIconVal"
          @click="cancel()"
        />
        <DxButton
          type="default"
          text="확인"
          styling-mode="outlined"
          :disabled="loadingIconVal"
          :width="100"
          @click="editWeekOk()"
        >
        </DxButton>
      </template>
    </b-modal>
  </div>
</template>

<script>
import { DxCheckBox } from "devextreme-vue/check-box";
import { mapActions, mapGetters, mapMutations } from "vuex";
import DxButton from "devextreme-vue/button";
import DxTextBox from "devextreme-vue/text-box";
import DxTextArea from "devextreme-vue/text-area";
import { DxLoadIndicator } from "devextreme-vue/load-indicator";
import { DxProgressBar } from "devextreme-vue/progress-bar";
import CommonImportDef from "../../../components/Popup/CommonImportDef.vue";
import CommonImportTem from "../../../components/Popup/CommonImportTem.vue";
import CommonImportArchive from "../../../components/Popup/CommonImportArchive.vue";
import WavExportAbCart from "../../../components/Popup/WavExportABCart.vue";
import {
  AUTHORITY,
  AUTHORITY_ADMIN,
  MY_DISK_PAGE_ID,
  USER_ID,
  PREVIEW_CODE,
} from "@/constants/config";
import DxDropDownButton from "devextreme-vue/drop-down-button";
import { DxLoadPanel } from "devextreme-vue/load-panel";
import { eventBus } from "@/eventBus";
import axios from "axios";

import "moment/locale/ko";
const moment = require("moment");
function statusFormat(value) {
  return `${parseInt(value * 100)}%`;
}
const CancelToken = axios.CancelToken;
const signalR = require("@microsoft/signalr");
const connection = new signalR.HubConnectionBuilder()
  .withUrl("/api/ProgressHub")
  .build();
export default {
  props: {
    type: String,
    saveText: String,
    cueClearItems: {
      type: Array,
      default: () => ["print", "ab", "tags", "memo"],
    },
    cueClearOptions: {
      type: Array,
      default: () => [
        { name: "출력용", value: "print", notEnabled: true },
        { name: "SLAP (A, B)", value: "ab", notEnabled: true },
        { name: "태그", value: "tags", notEnabled: true },
        { name: "메모", value: "memo", notEnabled: true },
      ],
    },
    fav: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      date: new Date(),
      pramObj: { person: null, brd_dt: null, media: null },
      pgmList: [],
      mediaOptions: [],
      programOptions: [],
      source: null,
      statusFormat,
      seconds: 0,
      PREVIEW_CODE,
      MY_DISK_PAGE_ID,
      currentPageName: "",
      IS_ADMIN: sessionStorage.getItem(AUTHORITY) === AUTHORITY_ADMIN,
      isMyDiskCheckBox: false,
      labelText: "병합 후 My 디스크에 해당 소재 추가",
      isWavCopy: false,
      isWavExportDisabled: true,
      goBackPoint: "",
      loadingIconVal: false,
      tmpTitleTextBoxValue: "",
      proid: "",
      id: "",
      cuetype: "",
      allCheck: true,
      templateTitle: "",
      selected: [],
      isMyDiskExist: false,
      cartSelected: ["c1", "c2", "c3", "c4"],
      oldCueOptions: [
        {
          text: "(구)DAP에 현재 큐시트 저장",
          value: true,
        },
      ],
      oldCueSelected: [],
      options: [],
      cartOptions: [
        { name: "C1", value: "c1", notEnabled: true },
        { name: "C2", value: "c2", notEnabled: true },
        { name: "C3", value: "c3", notEnabled: true },
        { name: "C4", value: "c4", notEnabled: true },
      ],
      valueForEditableTextArea: "메모를 삽입하세요.",
      buttonText: "확인",
      activefolderitem: [
        "템플릿 가져오기",
        "기본 큐시트 가져오기",
        "이전 큐시트 가져오기",
      ],
      downloaditem: [
        { id: "zip", name: "큐시트 로컬 저장" },
        { id: "wav", name: "Wav로 저장(AB)" },
        { id: "pdf", name: "PDF로 저장" },
        { id: "docx", name: "Word로 저장" },
        { id: "excel", name: "엑셀로 저장" },
      ],
      weekButtons: [
        { caption: "월", value: "MON", state: false, disable: false },
        { caption: "화", value: "TUE", state: false, disable: false },
        { caption: "수", value: "WED", state: false, disable: false },
        { caption: "목", value: "THU", state: false, disable: false },
        { caption: "금", value: "FRI", state: false, disable: false },
        { caption: "토", value: "SAT", state: false, disable: false },
        { caption: "일", value: "SUN", state: false, disable: false },
      ],
      editOptions: {
        djname: "",
        membername: "",
        directorname: "",
        headertitle: "",
        footertitle: "",
        memo: "",
      },
    };
  },
  mounted() {
    switch (this.type) {
      case "O":
        this.goBackPoint = "old";
        break;
      case "D":
        this.goBackPoint = "day";
        break;
      case "B":
        this.goBackPoint = "week";
        break;
      case "T":
        this.goBackPoint = "template";
        break;
      case "A":
        this.goBackPoint = "previous";
        break;

      default:
        break;
    }
    this.editOptions = { ...this.cueInfo };
    this.templateTitle = this.cueInfo.title + "_(복사)";
    if (this.cueInfo.cuetype != "T") {
      var mediaName = "";
      switch (this.cueInfo.media) {
        case "A":
          mediaName = "[AM]";
          break;
        case "F":
          mediaName = "[FM]";
          break;
        case "D":
          mediaName = "[DMB]";
          break;
        case "C":
          mediaName = "[공통]";
          break;
        case "Z":
          mediaName = "[기타]";
          break;
        default:
          break;
      }
      if (this.cueInfo.cuetype == "B") {
        this.templateTitle = mediaName + this.cueInfo.title + "_(기본)";
      } else {
        this.templateTitle =
          mediaName +
          this.cueInfo.title +
          "_" +
          moment(this.cueInfo.brdtime).format("YYYY-MM-DD");
      }
    }
    this.isMyDiskCheckBox = this.roleList.some(
      (data) => data.id === this.MY_DISK_PAGE_ID && data.visible === "Y"
    );
  },
  components: {
    DxCheckBox,
    CommonImportDef,
    CommonImportTem,
    CommonImportArchive,
    DxButton,
    DxDropDownButton,
    DxTextBox,
    DxTextArea,
    DxLoadIndicator,
    DxLoadPanel,
    WavExportAbCart,
    DxProgressBar,
  },
  computed: {
    ...mapGetters("cueList", ["abCartArr"]),
    ...mapGetters("cueList", ["cChannelData"]),
    ...mapGetters("cueList", ["attachments"]),
    ...mapGetters("cueList", ["printArr"]),
    ...mapGetters("cueList", ["cueFavorites"]),
    ...mapGetters("cueList", ["cueInfo"]),
    ...mapGetters("user", ["roleList"]),

    btnWeekStates() {
      var result = this.weekButtons.map((btn) => {
        if (btn.state == true) {
          return btn.value;
        }
      });
      return result.filter((ele) => {
        return ele !== undefined;
      });
    },
  },
  created() {
    if (this.type == "B") {
      this.weekButtons.forEach((week) => {
        if (this.cueInfo.productWeekList[0].weekList.includes(week.value)) {
          if (this.cueInfo.activeWeekList.includes(week.value)) {
            week.state = true;
          } else {
            week.disable = true;
          }
        } else {
          week.disable = false;
        }
      });
    }
    this.selected = this.cueClearItems.concat();
    this.options = this.cueClearOptions.concat();
  },
  methods: {
    ...mapMutations("cueList", ["SET_PRINTARR"]),
    ...mapMutations("cueList", ["SET_ABCARTARR"]),
    ...mapMutations("cueList", ["SET_ATTACHMENTS"]),
    ...mapMutations("cueList", ["SET_TAGS"]),
    ...mapMutations("cueList", ["SET_CCHANNELDATA"]),
    ...mapMutations("cueList", ["SET_CUEFAVORITES"]),
    ...mapMutations("cueList", ["SET_CUEINFO"]),
    ...mapMutations("user", ["SET_INIT_CALL_LOGIN_AUTH_TRY_CNT"]),
    ...mapActions("cueList", ["saveDayCue"]),
    ...mapActions("cueList", ["saveDefCue"]),
    ...mapActions("cueList", ["saveTempCue"]),
    ...mapActions("cueList", ["addByTemplate"]),
    ...mapActions("cueList", ["setCueConFav_save"]),
    ...mapActions("cueList", ["setclearFav"]),
    ...mapActions("cueList", ["getDateStr"]),
    ...mapActions("cueList", ["SetMediaOption"]),
    ...mapActions("user", ["renewal"]),
    ...mapActions("cueList", ["enableNotification"]),

    //템플릿으로 저장
    async addtemClick() {
      this.loadingIconVal = true;
      const userId = sessionStorage.getItem(USER_ID);
      var cueCon = await this.setCueConFav_save(false);

      var tempItem = {
        personid: userId,
        detail: [{ cueid: -1 }],
        djname: this.cueInfo.djname,
        directorname: this.cueInfo.directorname,
        membername: this.cueInfo.membername,
        headertitle: this.cueInfo.headertitle,
        footertitle: this.cueInfo.footertitle,
        memo: this.cueInfo.memo,
      };

      this.tmpTitleTextBoxValue
        ? (tempItem.title = this.tmpTitleTextBoxValue)
        : (tempItem.title = this.templateTitle);
      cueCon.CueSheetDTO = tempItem;

      await this.addByTemplate(cueCon);
      this.loadingIconVal = false;
      this.$bvModal.hide("modal-template");
    },
    resetModal_tem() {
      this.tmpTitleTextBoxValue = "";
    },
    clearOk() {
      this.loadingIconVal = true;
      if (this.selected.length > 0) {
        if (this.selected.includes("print")) {
          this.SET_PRINTARR([]);
        }
        if (this.selected.includes("ab")) {
          this.SET_ABCARTARR([]);
        }
        //첨부파일
        this.selected.includes("attachments") &&
          eventBus.$emit("attachments-delete");
        //태그
        this.selected.includes("tags") && this.SET_TAGS([]);
        //메모
        if (this.selected.includes("memo")) {
          this.cueInfo.memo = "";
          this.SET_CUEINFO(this.cueInfo);
        }
      }
      if (this.cartSelected.length > 0) {
        eventBus.$emit("clearCData", this.cartSelected);
      }
      this.loadingIconVal = false;
      this.$bvModal.hide("modal-clear");
    },
    //즐겨찾기 비우기
    favDelOk() {
      this.setclearFav();
      eventBus.$emit("clearFav");
      this.$bvModal.hide("modal-favDel");
    },
    clickCheckTilte() {
      if (this.allCheck) {
        //아이템 전체 비활성화
        this.selected = this.cueClearItems.concat();
        this.cartSelected = ["c1", "c2", "c3", "c4"];
        this.options.forEach((ele) => {
          ele.notEnabled = true;
        });
        this.cartOptions.forEach((ele) => {
          ele.notEnabled = true;
        });
      } else {
        //아이템 전체 활성화
        this.options.forEach((ele) => {
          ele.notEnabled = false;
        });
        this.cartOptions.forEach((ele) => {
          ele.notEnabled = false;
        });
      }
    },
    onDownloadItemClick(e) {
      //wav도 추가해야함
      if (e.itemData.id == "zip") {
        this.$refs["modal-export-zip"].show();
      }
      if (e.itemData.id == "wav") {
        // this.maxValue
        if (!connection.connectionStarted) {
          connection.start().catch(function (err) {
            console.log("err", err);
          });
          connection.on("sendProgress", (progress) => {
            this.seconds = progress;
          });
        }
        this.$refs["modal-export-wav"].show();
      } else {
        eventBus.$emit("exportGo", e.itemData.id);
      }
    },
    onActivefolderItemClick(e) {
      switch (e.itemData) {
        case "템플릿 가져오기":
          this.id = this.cueInfo.personid;
          this.$bvModal.show("commonImportTem");
          break;
        case "기본 큐시트 가져오기":
          this.proid = this.cueInfo.productid;
          this.$bvModal.show("commonImportDef");
          break;
        case "이전 큐시트 가져오기":
          this.$bvModal.show("CommonImportArchive");
          break;
        default:
          break;
      }
    },
    //큐시트 저장
    async saveOk() {
      this.loadingIconVal = true;
      switch (this.type) {
        case "D":
          if (this.oldCueSelected.length != 0) {
            await this.saveDayCue(true);
          } else {
            await this.saveDayCue();
          }
          break;

        case "B":
          await this.saveDefCue();
          break;

        case "T":
          await this.saveTempCue();
          break;

        case "F":
          const userId = sessionStorage.getItem(USER_ID);
          await this.$http
            .post(
              `/api/Favorite/SetFavorites?personid=${userId}`,
              this.cueFavorites
            )
            .then((res) => {
              this.enableNotification({
                type: "info",
                message: `즐겨찾기 저장완료.`,
              });
            })
            .catch((err) => {
              this.enableNotification({
                type: "error",
                message: `즐겨찾기 저장실패.`,
              });
            });
          break;
        default:
          break;
      }
      this.SET_INIT_CALL_LOGIN_AUTH_TRY_CNT();
      this.renewal();
      this.loadingIconVal = false;
      this.$bvModal.hide("modal-save");
    },
    modalSaveOpen() {
      this.checkMyDisk();
    },
    checkMyDisk() {
      let totalItems = this.abCartArr.concat(
        this.cChannelData.channel_1,
        this.cChannelData.channel_2,
        this.cChannelData.channel_3,
        this.cChannelData.channel_4
      );
      return (this.isMyDiskExist = totalItems.some(
        (item) => item.cartcode === "S01G01C007"
      ));
    },
    // zip, wave 파일 다운로드
    // wave는 AB만 들어가야 함 나중에 method 분리하기
    async exportZip() {
      this.loadingIconVal = true;
      var cuesheetData = await this.setCueConFav_save(false);
      var pramList = [];
      for (var i = 1; i < 5; i++) {
        cuesheetData.InstanceCon["channel_" + i].forEach((ele) => {
          if (ele.cartcode != null && ele.cartcode != "") {
            let cObj = { ...ele };
            cObj.channeltype = "I";
            cObj.rownum = ele.rownum + (i - 1) * 16;
            pramList.push(cObj);
          }
        });
      }
      cuesheetData.NormalCon.forEach((ele) => {
        ele.channeltype = "N";
        pramList.push(ele);
      });
      if (pramList.length == 0) {
        this.enableNotification({
          type: "error",
          message: `내려받을 소재가 없습니다.`,
        });
      } else {
        var downloadName = "";
        switch (this.cueInfo.cuetype) {
          case "D":
            downloadName = `${this.cueInfo.day}_${this.cueInfo.title}`;
            break;
          case "B":
            downloadName = `[기본]_${this.cueInfo.title}`;
            break;
          case "T":
            downloadName = `${this.cueInfo.title}`;
            break;
          case "A":
            downloadName = `${this.cueInfo.brddate}_${this.cueInfo.title}`;
            break;
        }
        await axios
          .post(
            `/api/CueAttachments/exportZipFile?userid=${sessionStorage.getItem(
              USER_ID
            )}`,
            pramList
          )
          .then((response) => {
            const link = document.createElement("a");
            link.href = `/api/CueAttachments/exportFileDownload?guid=${
              response.data
            }&userid=${sessionStorage.getItem(
              USER_ID
            )}&downloadname=${downloadName}.zip`;
            document.body.appendChild(link);
            link.click();
          })
          .catch((error) => {
            console.log(error);
          });
      }
      this.loadingIconVal = false;

      this.$refs["modal-export-zip"].hide();
    },
    async OnExportWav() {
      this.loadingIconVal = true;
      this.isWavExportDisabled = false;
      const date = new Date();
      this.source = CancelToken.source();
      const dateStr = await this.getDateStr(date);
      var cuesheetData = await this.setCueConFav_save(false);
      var pramList = [];
      cuesheetData.NormalCon.forEach((ele) => {
        if (ele.cartcode != "") pramList.push(ele);
      });
      if (pramList.length == 0) {
        this.enableNotification({
          type: "error",
          message: `내려받을 소재가 없습니다.`,
        });
      } else {
        let downloadName = "";
        switch (this.cueInfo.cuetype) {
          case "D":
            downloadName = `${this.cueInfo.title}_${dateStr}`;
            break;
          case "B":
            downloadName = `[기본]_${this.cueInfo.title}_${dateStr}`;
            break;
          case "T":
            downloadName = `${this.cueInfo.title}_${dateStr}`;
            break;
          case "A":
            downloadName = `${this.cueInfo.title}_${dateStr}`;
            break;
        }
        await this.$http
          .post(
            `/api/CueAttachments/exportWavFile?connectionId=${connection.connectionId}`,
            pramList,
            { cancelToken: this.source.token }
          )
          .then((response) => {
            if (response.data.resultCode === 0) {
              const downPath = response.data.resultObject.value;
              this.isWavCopy && this.copyToMySpace(downPath, downloadName);
              this.downloadFile(downPath, downloadName);
            }
          })
          .catch((err) => {
            if (err.code === "ECONNABORTED") {
              this.enableNotification({
                type: "error",
                message: `요청시간이 만료되었습니다.`,
              });
              this.seconds = 0;
            }
          });
      }
      this.loadingIconVal = false;
      this.isWavExportDisabled = true;
      this.$refs["modal-export-wav"].hide();
      this.seconds = 0;
    },
    OnWavExportCancel() {
      this.isWavExportDisabled = true;
      if (this.source) {
        this.source.cancel();
      }
      this.$refs["modal-export-wav"].hide();
    },
    downloadFile(fileData, fileName) {
      const link = document.createElement("a");
      link.href = `/api/CueAttachments/exportFileDownload?guid=${fileData}&userid=${sessionStorage.getItem(
        USER_ID
      )}&downloadname=${fileName}.wav`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    },
    copyToMySpace(filePath, fileName) {
      this.$http
        .post(
          `/api/CueAttachments/WavCopyToMyspace?title=${fileName}`,
          filePath
        )
        .then((res) => {
          if (res.data.resultCode === 0) {
            this.enableNotification({
              type: "info",
              message: `소재가 My디스크에 추가되었습니다.`,
            });
          }
        });
    },
    async editWeekListClick() {
      const { productid, title: eventName, media } = { ...this.cueInfo };
      this.programOptions.push({ value: productid, text: eventName });

      this.mediaOptions = await this.SetMediaOption([{ media: media }]);
      this.$refs["modal-editWeek"].show();
    },
    //적용요일 변경
    editWeekOk() {
      var cueData = { ...this.cueInfo };
      var weekList = [];
      var activeWeekList = [];
      var pushList = [];
      var delId = [];
      var stateList = this.weekButtons.filter((ele) => {
        return ele.state == true;
      });
      if (stateList.length == 0) {
        this.enableNotification({
          type: "error",
          message: `적용요일을 선택하세요.`,
        });
      } else {
        this.weekButtons.forEach((week, index) => {
          if (week.state) {
            weekList.push({ week: week.value });
            activeWeekList.push(week.value);
          }
        });
        cueData.detail.forEach((v) => {
          if (!activeWeekList.includes(v.week)) {
            delId.push(v.cueid);
          }
        });
        cueData.productWeekList[0].weekList.forEach((week) => {
          if (!cueData.activeWeekList.includes(week)) {
            pushList.push(week);
          }
        });
        cueData.productWeekList[0].weekList = pushList.concat(activeWeekList);

        cueData.newdetail = weekList;
        cueData.activeWeekList = activeWeekList;
        cueData.delId = delId;
        this.SET_CUEINFO(cueData);
        this.$refs["modal-editWeek"].hide();
      }
    },
    resetModal_weekedit() {
      if (this.type == "B") {
        this.weekButtons.forEach((week) => {
          if (this.cueInfo.productWeekList[0].weekList.includes(week.value)) {
            if (this.cueInfo.activeWeekList.includes(week.value)) {
              week.state = true;
            } else {
              week.disable = true;
            }
          } else {
            week.state = false;
            week.disable = false;
          }
        });
      }
    },
    editOk() {
      this.SET_CUEINFO(this.editOptions);
      this.$bvModal.hide("modal-setting");
    },
    resetModal() {
      this.editOptions = { ...this.cueInfo };
    },
    isTotalDuration(value) {
      this.isWavExportDisabled = value;
    },
  },
};
</script>

<style lang="scss" scoped>
.clear-check-title {
  font-size: 20px;
  margin-left: 30%;
}
.clear-check-item {
  text-align: center;
  border-radius: 20px;
  border: solid 1px #cecece;
  font-size: 14px;
  padding-top: 20px;
  padding-bottom: 10px;
  margin: 0px 130px 0px 130px;
}
/* 모달 CSS */
#modal-setting .dx-field-label {
  font-family: "MBC 새로움 M" !important;
  text-align: end;
  width: 28%;
}
#modal-setting
  .dx-field-value:not(.dx-switch):not(.dx-checkbox):not(.dx-button) {
  font-family: "MBC 새로움 M" !important;
  width: 65%;
}
.mydisc-check-box {
  position: absolute;
  left: 40px;
}
</style>
