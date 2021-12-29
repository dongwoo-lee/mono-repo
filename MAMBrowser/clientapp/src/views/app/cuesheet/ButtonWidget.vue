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
        v-if="!fav && accrssCheck"
      />
      <DxDropDownButton
        :items="activefolderitem"
        :drop-down-options="{ width: 160 }"
        icon="activefolder"
        :showArrowIcon="false"
        hint="가져오기"
        @item-click="onActivefolderItemClick"
        v-if="!fav && type != 'A'"
      />
      <DxDropDownButton
        :items="downloaditem"
        :drop-down-options="{ width: 70 }"
        icon="download"
        :showArrowIcon="false"
        hint="내려받기"
        @item-click="onDownloadItemClick"
        v-if="!fav"
      />
      <DxButton
        icon="checklist"
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
        text="저장"
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

    <!-- 내보내기 -->
    <b-modal
      ref="modal-wav"
      size="lg"
      centered
      title="내보내기"
      ok-title="확인"
      cancel-title="취소"
    >
      <div class="d-block text-center">
        <div class="mb-3 mt-3" style="font-size: 20px">
          <div>
            현재 DAP (A,B) 탭에 추가된 소재를 하나의 WAV 파일로 병합합니다.
          </div>
        </div>
        <div class="wavemodalDiv">
          <div class="dx-fieldset">
            <div class="dx-field">
              <div class="dx-field-label wavemodalabel">병합될 소재 개수 :</div>
              <div class="dx-field-value">10 개</div>
            </div>
            <div class="dx-field">
              <div class="dx-field-label wavemodalabel">
                완료 후 소재 길이 :
              </div>
              <div class="dx-field-value">00:45:54</div>
            </div>
          </div>
        </div>
        <div v-if="buttonText !== '확인'">
          <DxProgressBar
            id="progress-bar-status"
            :class="{ complete: seconds == 0 }"
            :min="0"
            :max="maxValue"
            :status-format="statusFormat"
            :value="progressValue"
            width="90%"
          />
        </div>
      </div>
      <template #modal-footer="{ cancel }">
        <b-button variant="secondary" @click="cancelClick(cancel)"
          >취소
        </b-button>
        <b-button variant="primary" @click="onButtonClick">
          {{ buttonText }}
        </b-button>
      </template>
    </b-modal>

    <!-- 큐시트 저장 -->
    <b-modal
      id="modal-save"
      size="lg"
      centered
      title="저장"
      ok-title="확인"
      cancel-title="취소"
      @ok="saveOk"
    >
      <div class="d-block text-center">
        <div class="mb-3 mt-3" style="font-size: 20px">
          <div class="mb-3" v-if="cueInfo.cuetype == 'D' && !fav">
            "{{ cueInfo.title }}" 큐시트를 저장합니다.

            <div v-if="type != 'O'">
              <b-form-checkbox-group
                class="custom-checkbox-group mt-5"
                style="font-size: 16px"
                v-model="oldCueSelected"
                :options="oldCueOptions"
                value-field="value"
                text-field="text"
              />
            </div>
          </div>
          <div class="mb-3" v-if="cueInfo.cuetype == 'B' && !fav">
            "{{ cueInfo.title }}" 기본 큐시트를 저장합니다.
          </div>
          <div class="mb-3" v-if="cueInfo.cuetype == 'T' && !fav">
            "{{ cueInfo.title }}" 템플릿을 저장합니다.
          </div>
          <div class="mb-3" v-if="fav">즐겨찾기를 저장합니다.</div>
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
          <div class="dx-field">
            <div class="dx-field-label" style="font-size: 15px">메모 :</div>
            <div class="dx-field-value">
              <DxTextArea
                :height="60"
                :maxLength="25"
                width="320px"
                v-model="editOptions.memo"
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
      ref="modal-export-zip-wave"
      size="lg"
      centered
      title="내보내기"
      ok-title="확인"
      cancel-title="취소"
      @ok="exportZipWave"
    >
      <div class="d-block text-center">
        <div class="mb-3 mt-3" style="font-size: 20px">
          <div class="mb-3">작성된 내용을 ZIP 파일로 내보냅니다.</div>
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
          @click="exportZipWave()"
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
              :options="mediasOption"
              disabled
            />
          </b-form-group>
          <b-form-group label="프로그램명" class="has-float-label ml-3">
            <b-form-select
              style="width: 400px"
              v-model="cueInfo.productid"
              :options="userProOption"
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
import { mapActions, mapGetters, mapMutations } from "vuex";
import DxButton from "devextreme-vue/button";
import DxTextBox from "devextreme-vue/text-box";
import DxTextArea from "devextreme-vue/text-area";
import { DxLoadIndicator } from "devextreme-vue/load-indicator";
import CommonImportDef from "../../../components/Popup/CommonImportDef.vue";
import CommonImportTem from "../../../components/Popup/CommonImportTem.vue";
import CommonImportArchive from "../../../components/Popup/CommonImportArchive.vue";
import { USER_ID, ACCESS_GROP_ID, USER_NAME } from "@/constants/config";
import DxDropDownButton from "devextreme-vue/drop-down-button";
import { eventBus } from "@/eventBus";
import axios from "axios";

export default {
  props: {
    // cuesheetData: Object,
    type: String,
    fav: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      goBackPoint: "",
      accrssCheck: true,
      loadingIconVal: false,
      tmpTitleTextBoxValue: "이름없는 템플릿",
      proid: "",
      id: "",
      cuetype: "",
      allCheck: true,
      selected: ["print", "ab"],
      cartSelected: ["c1", "c2", "c3", "c4"],
      oldCueOptions: [
        {
          text: "(구)DAP에 현재 큐시트 저장",
          value: true,
        },
      ],
      oldCueSelected: [true],
      options: [
        { name: "출력용", value: "print", notEnabled: true },
        { name: "DAP(A, B)", value: "ab", notEnabled: true },
      ],
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
      downloaditem: [".zip", ".pdf", ".docx", ".excel"],
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
    const gropId = sessionStorage.getItem(ACCESS_GROP_ID);
    if (
      gropId == "S01G04C004" ||
      gropId == "S01G04C006" ||
      gropId == "S01G04C001"
    ) {
      this.accrssCheck = true;
    } else {
      this.accrssCheck = false;
    }
    this.editOptions = { ...this.cueInfo };
  },
  components: {
    CommonImportDef,
    CommonImportTem,
    CommonImportArchive,
    DxButton,
    DxDropDownButton,
    DxTextBox,
    DxTextArea,
    DxLoadIndicator,
  },
  computed: {
    ...mapGetters("cueList", ["abCartArr"]),
    ...mapGetters("cueList", ["cChannelData"]),
    ...mapGetters("cueList", ["printArr"]),
    ...mapGetters("cueList", ["cueFavorites"]),
    ...mapGetters("cueList", ["cueInfo"]),

    ...mapGetters("cueList", ["mediasOption"]),
    ...mapGetters("cueList", ["userProOption"]),

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
  },
  methods: {
    ...mapMutations("cueList", ["SET_PRINTARR"]),
    ...mapMutations("cueList", ["SET_ABCARTARR"]),
    ...mapMutations("cueList", ["SET_CCHANNELDATA"]),
    ...mapMutations("cueList", ["SET_CUEFAVORITES"]),
    ...mapMutations("cueList", ["SET_CUEINFO"]),
    ...mapActions("cueList", ["saveDayCue"]),
    ...mapActions("cueList", ["saveDefCue"]),
    ...mapActions("cueList", ["saveTempCue"]),
    ...mapActions("cueList", ["saveOldCue"]),

    ...mapActions("cueList", ["addTemplate"]),
    ...mapActions("cueList", ["setCueConFav_save"]),
    ...mapActions("cueList", ["setclearFav"]),
    ...mapActions("cueList", ["getuserProOption"]),

    //테스트중
    ...mapActions("cueList", ["exportZip"]),

    //템플릿으로 저장
    async addtemClick() {
      this.loadingIconVal = true;
      const userId = sessionStorage.getItem(USER_ID);
      var cueCon = await this.setCueConFav_save(false);
      // var temParam = { personid: userId, tmptitle: this.tmpTitleTextBoxValue };
      // cueCon.temParam = temParam;
      var tempItem = {
        personid: userId,
        detail: [{ cueid: -1 }],
        title: this.tmpTitleTextBoxValue,
        djname: this.cueInfo.djname,
        directorname: this.cueInfo.directorname,
        membername: this.cueInfo.membername,
        headertitle: this.cueInfo.headertitle,
        footertitle: this.cueInfo.footertitle,
        memo: this.cueInfo.memo,
      };
      this.tmpTitleTextBoxValue = "이름없는 템플릿";
      // var pram = {
      //   CueSheetDTO: tempItem,
      // };
      cueCon.CueSheetDTO = tempItem;

      await this.addTemplate(cueCon);
      this.loadingIconVal = false;
      this.$bvModal.hide("modal-template");
    },
    resetModal_tem() {
      this.tmpTitleTextBoxValue = "이름없는 템플릿";
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
        this.selected = ["print", "ab"];
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
      if (e.itemData == ".zip" || e.itemData == ".wav") {
        this.$refs["modal-export-zip-wave"].show();
      } else {
        eventBus.$emit("exportGo", e.itemData);
      }
      // eventBus.$emit("exportGo", e.itemData);
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
        case "O":
          this.saveOldCue();
          break;
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
          await axios
            .post(
              `/api/Favorite/SetFavorites?personid=${userId}`,
              this.cueFavorites
            )
            .then((res) => {
              window.$notify("info", `즐겨찾기 저장완료.`, "", {
                duration: 10000,
                permanent: false,
              });
            })
            .catch((err) => {
              window.$notify("error", `즐겨찾기 저장실패.`, "", {
                duration: 10000,
                permanent: false,
              });
            });
          break;
        default:
          break;
      }
      this.loadingIconVal = false;
      this.$bvModal.hide("modal-save");
    },
    // zip 파일 다운로드
    async exportZipWave() {
      this.loadingIconVal = true;
      var cuesheetData = await this.setCueConFav_save(false);
      var pramList = [];
      for (var i = 1; i < 5; i++) {
        cuesheetData.InstanceCon["channel_" + i].forEach((ele) => {
          if (ele.cartcode != null && ele.cartcode != "") {
            pramList.push(ele);
          }
        });
      }
      cuesheetData.NormalCon.forEach((ele) => {
        if (ele.cartcode != null && ele.cartcode != "") {
          pramList.push(ele);
        }
      });
      if (pramList.length == 0) {
        window.$notify("error", `내려받을 소재가 없습니다.`, "", {
          duration: 10000,
          permanent: false,
        });
      } else {
        await axios
          .post(
            `/api/CueAttachments/exportZipFile?title=${this.cueInfo.title}`,
            pramList
          )
          .then((response) => {
            const link = document.createElement("a");
            link.href =
              "/api/CueAttachments/exportZipFileDownload?fileName=" +
              response.data;
            document.body.appendChild(link);
            link.click();
          })
          .catch((error) => {
            console.log(error);
          });
      }
      this.loadingIconVal = false;
      this.$refs["modal-export-zip-wave"].hide();
    },
    async editWeekListClick() {
      const userName = sessionStorage.getItem(USER_NAME);
      const gropId = sessionStorage.getItem(ACCESS_GROP_ID);
      var pram = {
        person: userName,
        gropId: gropId,
        media: this.cueInfo.media,
      };
      var proOption = await this.getuserProOption(pram);
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
        window.$notify("error", `적용요일을 선택하세요.`, "", {
          duration: 10000,
          permanent: false,
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
  margin: 0px 150px 0px 150px;
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
</style>
