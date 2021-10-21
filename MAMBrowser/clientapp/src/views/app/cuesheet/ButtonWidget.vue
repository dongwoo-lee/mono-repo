<template>
  <div>
    <div>
      <DxButton
        icon="arrowleft"
        @click="$router.push({ path: '/app/cuesheet/day/list' })"
        hint="목록으로"
      />
      <DxButton icon="doc" v-b-modal.modal-clear hint="새큐시트" />
      <DxButton
        icon="columnchooser"
        v-b-modal.modal-template
        hint="템플릿으로 저장"
      />
      <DxDropDownButton
        :items="activefolderitem"
        :drop-down-options="{ width: 160 }"
        icon="activefolder"
        :showArrowIcon="false"
        hint="가져오기"
        @item-click="onActivefolderItemClick"
      />

      <!-- <DxDropDownButton :drop-down-options="{ width: 70 }" icon="activefolder" v-b-modal.commonCueimport hint="가져오기" /> -->
      <DxDropDownButton
        :items="downloaditem"
        :drop-down-options="{ width: 70 }"
        icon="download"
        :showArrowIcon="false"
        hint="내려받기"
        @item-click="onDownloadItemClick"
      />
      <DxButton
        icon="save"
        type="default"
        v-b-modal.modal-save
        hint="저장"
        text="저장"
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
    </b-modal>

    <!-- 템플릿으로 저장 -->
    <b-modal
      id="modal-template"
      size="lg"
      centered
      title="템플릿으로 저장"
      ok-title="확인"
      cancel-title="취소"
      @ok="addTemplate"
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
              v-model="temtitle"
            />
          </div>
        </div>
      </div>
    </b-modal>

    <!-- 가져오기 -->
    <common-import-tem :id="id" />
    <common-import-def :proid="proid" :cuesheetData="cuesheetData" />

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

    <!-- 추가정보 -->
    <b-modal
      id="modal-setting"
      size="lg"
      centered
      title="추가정보"
      ok-title="저장"
      cancel-title="취소"
    >
      <div class="settingDiv">
        <div class="dx-fieldset">
          <div class="dx-field">
            <div class="dx-field-label" style="font-size: 15px">
              이게되는거야? :
            </div>
            <div class="dx-field-value">
              <DxTextBox placeholder="김이나" width="320px" />
            </div>
          </div>
          <div class="dx-field">
            <div class="dx-field-label" style="font-size: 15px">구성 :</div>
            <div class="dx-field-value">
              <DxTextBox placeholder="홍재정, 이희상" width="320px" />
            </div>
          </div>
          <div class="dx-field">
            <div class="dx-field-label" style="font-size: 15px">연출 :</div>
            <div class="dx-field-value">
              <DxTextBox placeholder="홍희주" width="320px" />
            </div>
          </div>
          <div class="dx-field">
            <div class="dx-field-label" style="font-size: 15px">기타 :</div>
            <div class="dx-field-value">
              <DxTextArea
                :height="100"
                value="이 문서는 MBC의 동의 없이 수정, 변경 및 복사 할 수 없습니다."
              />
            </div>
          </div>
        </div>
      </div>
    </b-modal>

    <!-- 큐시트 저장 -->
    <b-modal
      id="modal-save"
      size="lg"
      centered
      title="큐시트 저장"
      ok-title="확인"
      cancel-title="취소"
      @ok="saveOk"
    >
      <div class="d-block text-center">
        <div class="mb-3 mt-3" style="font-size: 20px">
          <div class="mb-3">큐시트를 저장합니다.</div>
        </div>
      </div>
    </b-modal>
  </div>
</template>

<script>
import { mapActions, mapGetters, mapMutations } from "vuex";
import DxButton from "devextreme-vue/button";
import DxTextBox from "devextreme-vue/text-box";
import DxDropDownButton from "devextreme-vue/drop-down-button";
import axios from "axios";
import { eventBus } from "@/eventBus";
import CommonImportDef from "../../../components/Popup/CommonImportDef.vue";
import CommonImportTem from "../../../components/Popup/CommonImportTem.vue";

export default {
  props: {
    cuesheetData: Object,
  },
  data() {
    return {
      temtitle: "이름없는 템플릿",
      proid: "",
      id: "",
      cuetype: "",
      allCheck: true,
      selected: ["print", "ab", "fav"],
      cartSelected: ["c1", "c2", "c3", "c4"],
      options: [
        { name: "출력용", value: "print", notEnabled: true },
        { name: "DAP(A, B)", value: "ab", notEnabled: true },
        { name: "즐겨찾기", value: "fav", notEnabled: true },
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
      downloaditem: [".zip", ".wav", ".pdf", ".docx", ".excel"],
    };
  },
  mounted() {},
  components: {
    DxButton,
    DxDropDownButton,
    DxTextBox,
    CommonImportDef,
    CommonImportTem,
  },
  computed: {
    ...mapGetters("cuesheet", ["abChannelData"]),
    ...mapGetters("cuesheet", ["cChannelData"]),
    ...mapGetters("cuesheet", ["cuePrint"]),
  },
  methods: {
    ...mapActions("cuesheet", ["saveDayCue"]),
    ...mapMutations("cuesheet", ["SET_SELEDAYCUE"]),
    ...mapMutations("cuesheet", ["SET_CUEPRINT"]),
    ...mapMutations("cuesheet", ["SET_ABCHANNELDATA"]),
    ...mapMutations("cuesheet", ["SET_CCHANNELDATA"]),

    //템플릿으로 저장(추가정보들도 추가해야함 footer 등등)
    async addTemplate() {
      var pram = this.cueDataSet();
      pram.temParam = {
        personid: this.cuesheetData.personid,
        tmptitle: this.temtitle,
        directorname: this.cuesheetData.directorname,
        djname: this.cuesheetData.djname,
        footertitle: this.cuesheetData.footertitle,
        headertitle: this.cuesheetData.headertitle,
        membername: this.cuesheetData.membername,
        memo: this.cuesheetData.memo,
      };
      await axios.post(`/api/TempCueSheet/SaveTempCue`, pram).then((res) => {
        this.temtitle = "이름없는 템플릿";
      });
    },
    //새큐시트 전체 및 부분삭제 (즐겨찾기 추가해야함)
    clearOk() {
      if (this.selected.length > 0) {
        if (this.selected.includes("print")) {
          var printData = [];
          this.SET_CUEPRINT([]);
          eventBus.$emit("printDataSet", printData);
        }
        if (this.selected.includes("ab")) {
          var abData = [];
          this.SET_ABCHANNELDATA([]);
          eventBus.$emit("abDataSet", abData);
        }
      }
      if (this.cartSelected.length > 0) {
        var cData = [];
        for (var i = 0; i < 16; i++) {
          cData.push({});
        }
        if (this.cartSelected.includes("c1")) {
          this.SET_CCHANNELDATA({ type: "channel_1", value: cData });
          eventBus.$emit("channel_1", cData);
        }
        if (this.cartSelected.includes("c2")) {
          this.SET_CCHANNELDATA({ type: "channel_2", value: cData });
          eventBus.$emit("channel_2", cData);
        }
        if (this.cartSelected.includes("c3")) {
          this.SET_CCHANNELDATA({ type: "channel_3", value: cData });
          eventBus.$emit("channel_3", cData);
        }
        if (this.cartSelected.includes("c4")) {
          this.SET_CCHANNELDATA({ type: "channel_4", value: cData });
          eventBus.$emit("channel_4", cData);
        }
      }
    },
    clickCheckTilte() {
      if (this.allCheck) {
        //아이템 전체 비활성화
        this.selected = ["print", "ab", "fav"];
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
      if (e.itemData === ".wav") {
        this.$refs["modal-wav"].show();
      }
      if (
        e.itemData === ".pdf" ||
        e.itemData === ".docx" ||
        e.itemData === ".excel"
      ) {
        this.exportWord();
      }
    },
    onActivefolderItemClick(e) {
      switch (e.itemData) {
        case "템플릿 가져오기":
          this.id = this.cuesheetData.personid;
          this.$bvModal.show("commonImportTem");
          break;
        case "기본 큐시트 가져오기":
          this.proid = this.cuesheetData.productid;
          this.$bvModal.show("commonImportDef");
          break;
        case "이전 큐시트 가져오기":
          // this.$bvModal.show("commonCueimport");
          break;
        default:
          break;
      }
    },
    //큐시트 저장
    saveOk() {
      var pram = this.cueDataSet();
      this.saveCueApi(pram);
    },
    cueDataSet() {
      //출력용
      var printData = [];
      this.cuePrint.forEach((ele, index) => {
        printData[index] = Object.assign({}, ele);
        printData[index].seqnum = index + 1;
        printData[index].starttime = ele.duration;
        delete printData[index].rowNum;
        if (ele.code == "") {
          printData[index].code = "CSGP10";
        }
      });
      //AB채널
      var abData = [];
      this.abChannelData.forEach((ele, index) => {
        abData[index] = Object.assign({}, ele);
        abData[index].channeltype = "N";
        abData[index].seqnum = index + 1;
      });
      //C채널
      var cData = [];
      var seqnum = 1;
      for (let i = 0; i <= 3; i++) {
        this.cChannelData[Object.keys(this.cChannelData)[i]].forEach((ele) => {
          cData.push(Object.assign({}, ele));
        });
      }
      var cDataResult = [];
      cData.forEach((ele, index) => {
        if (Object.keys(ele).length !== 0) {
          ele.channeltype = "I";
          ele.seqnum = index + 1;
          cDataResult.push(ele);
        }
        seqnum = seqnum + 1;
      });
      var conParams = abData.concat(cDataResult);
      var pram = {
        conParams: conParams,
        printParams: printData,
      };
      return pram;
    },
    async saveCueApi(pram) {
      var cuetype = "";
      var dayData = {};
      var defParams = [];
      var temParam = {};
      switch (this.cuesheetData.cuetype) {
        case "D":
          dayData = {
            brddate: this.cuesheetData.detail[0].brddate,
            brdtime: this.cuesheetData.detail[0].brdtime,
          };
          this.cuesheetData.cueid = this.cuesheetData.detail[0].cueid;
          cuetype = "day";
          break;
        case "B":
          //기존에 선택한 요일에서 삭제할경우 해야함
          this.cuesheetData.detail.forEach((ele) => {
            defParams.push(ele.week);
          });
          cuetype = "def";
          break;
        case "T":
          temParam = this.cuesheetData;
          temParam.cueid = temParam.detail[0].cueid;
          temParam.tmptitle = temParam.detail[0].tmptitle;
          cuetype = "temp";
          break;
        default:
          //날짜정보
          dayData = {
            brddate: this.cuesheetData.brddate,
            brdtime: this.cuesheetData.brdtime,
          };
          cuetype = "day";
          break;
      }
      pram.cueParam = this.cuesheetData;
      pram.temParam = temParam;
      pram.dayParam = dayData;
      pram.defParams = defParams;

      //일일큐시트저장
      await axios
        .post(`/api/${cuetype}CueSheet/Save${cuetype}Cue`, pram)
        .then((res) => {
          if (cuetype == "day") {
            this.cuesheetData.cueid = res.data.cueID;
            this.SET_SELEDAYCUE(this.cuesheetData);
          }
        });
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
</style>
