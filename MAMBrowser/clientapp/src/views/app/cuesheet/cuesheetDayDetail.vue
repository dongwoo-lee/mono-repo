<template>
  <div class="overView">
    <div id="cuesheet_view">
      <div class="toolDiv">
        <div v-if="tttt">
          <div id="subtitleDiv">
            <span class="subTitle">큐시트</span>
            <span class="subTitle">|</span>
            <span class="subTitle">일일 큐시트 작성</span>
          </div>
          <div>
            <h1>김이나의 별이 빛나는 밤에</h1>
          </div>
          <div class="separator mb-3 mt-0"></div>
          <div class="iconDiv">
            <DxButton icon="arrowleft" v-b-modal.modal-new />
            <DxButton icon="doc" v-b-modal.modal-new />
            <DxButton icon="columnchooser" v-b-modal.modal-template />
            <DxDropDownButton
              :items="activefolderitem"
              :drop-down-options="{ width: 160 }"
              icon="activefolder"
              :showArrowIcon="false"
              @item-click="onActivefolderItemClick"
            />
            <DxDropDownButton
              :items="downloaditem"
              :drop-down-options="{ width: 70 }"
              icon="download"
              :showArrowIcon="false"
              @item-click="onDownloadItemClick"
            />

            <DxButton icon="save" type="default" v-b-modal.modal-new />
          </div>
          <b-modal
            id="modal-new"
            size="lg"
            centered
            title="새큐시트"
            ok-title="확인"
            cancel-title="취소"
          >
            <div id="modelDiv" class="d-block text-center">
              <div class="mb-3 mt-3" style="font-size: 20px">
                <div class="mb-3">빈 새큐시트를 불러옵니다.</div>
                입력한 모든 내용은 삭제됩니다. 계속하시겠습니까?
              </div>
            </div>
          </b-modal>
          <b-modal
            id="modal-template"
            size="lg"
            centered
            title="템플릿으로 저장"
            ok-title="확인"
            cancel-title="취소"
          >
            <div id="modelDiv" class="d-block text-center">
              <div class="mb-3 mt-3" style="font-size: 20px">
                <div>현재 큐시트를 템플릿으로 저장합니다.</div>
                템플릿 이름을 입력해 주세요.
              </div>
              <div>
                <div
                  class="dx-field-label mt-3 mb-5 pl-5 pr-0"
                  style="font-size: 15px"
                >
                  템플릿 명 :
                </div>
                <div class="dx-field-value mt-3 mb-5 pr-5">
                  <DxTextBox placeholder="이름없는 템플릿" width="320px" />
                </div>
              </div>
            </div>
          </b-modal>
          <b-modal
            ref="modal-templateLoad"
            size="xl"
            centered
            title="템플릿 불러오기"
            ok-title="취소"
            ok-only
          >
            <div id="modelDiv" class="d-block text-center">
              <div>
                <CuesheetTemplateList
                  :modalData="modalData"
                  :temData="temData"
                  :tablewidth="tablewidth"
                />
              </div>
            </div>
          </b-modal>
          <b-modal
            ref="modal-wav"
            size="lg"
            centered
            title="내보내기"
            ok-title="확인"
            cancel-title="취소"
          >
            <div id="modelDiv" class="d-block text-center">
              <div class="mb-3 mt-3" style="font-size: 20px">
                <div>
                  현재 DAP (A,B) 탭에 추가된 소재를 하나의 WAV 파일로
                  병합합니다.
                </div>
              </div>
              <div class="wavemodalDiv">
                <div class="dx-fieldset">
                  <div class="dx-field">
                    <div class="dx-field-label wavemodalabel">
                      병합될 소재 개수 :
                    </div>
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
          <b-modal
            id="modal-memo"
            size="lg"
            centered
            title="메모편집"
            ok-title="저장"
            cancel-title="취소"
          >
            <div id="modelDiv" class="d-block text-center">
              <div class="mb-3 mt-3" style="font-size: 20px">
                <DxTextArea
                  :height="300"
                  :value.sync="valueForEditableTextArea"
                />
              </div>
            </div>
          </b-modal>
        </div>
        <!-- <button
          id="searchBtn"
          type="button"
          class="btn btn-primary btn-lg"
          @click="itemfindOn"
        >
          <b-icon-search></b-icon-search>
        </button> -->
        <div id="tabsDiv">
          <div>
            <DxTabPanel
              id="tabPanel"
              :defer-rendering="false"
              :onSelectionChanged="SelectionChanged"
            >
              <DxItem title="출력용">
                <template #default>
                  <div class="TabDiv">
                    <div>
                      <DxDataGrid
                        id="channelAB_id"
                        :data-source="viewtable"
                        :row-alternation-enabled="true"
                        :ref="dataGridRef"
                        height="625px"
                        :focusedRowEnabled="true"
                        :showColumnLines="true"
                        :show-borders="true"
                        :showRowLines="true"
                        @cell-prepared="onCellPrepared"
                        @selection-changed="onSelectionChanged"
                        @toolbar-preparing="onToolbarPreparing2($event)"
                        @focused-row-changed="onFocusedRowChanged2"
                        keyExpr="rowNO"
                        :show-column-headers="false"
                      >
                        <DxEditing
                          :allow-adding="true"
                          :allow-updating="true"
                          mode="cell"
                        />
                        <DxSelection mode="multiple" />
                        <DxRowDragging
                          dropFeedbackMode="indicate"
                          :allow-reordering="true"
                          :on-reorder="onReorder2"
                          :show-drag-icons="false"
                        />
                        <DxColumn
                          :width="100"
                          alignment="center"
                          data-field="categoryID"
                          cell-template="cellTemplate3"
                        />
                        <DxColumn caption="내용" data-field="name" />
                        <DxColumn
                          css-class="durationTime"
                          data-field="duration"
                          caption="사용시간"
                          alignment="center"
                          :width="100"
                        />
                        <DxColumn
                          css-class="durationTime"
                          caption="시작시간"
                          data-field="duration"
                          alignment="center"
                          :width="100"
                        />
                        <DxColumn
                          caption="비고"
                          :width="150"
                          alignment="center"
                        />
                        <DxScrolling mode="virtual" />
                        <template #diff-cell-template6="{ data }">
                          <div
                            v-if="data.data.name"
                            v-bind:class="{ graycolor: !data.data.categoryID }"
                          >
                            {{ data.data.duration }}
                          </div>
                        </template>
                        <template #diff-cell-template5="{ data }">
                          <div v-if="data.data.categoryID">
                            {{ data.data.duration }}
                          </div>
                        </template>
                        <template #diff-cell-template="{ data }">
                          <div>
                            <div class="dataname">{{ data.data.name }}</div>
                            <div class="categoryname">
                              {{ data.data.categoryName }}
                            </div>
                          </div>
                        </template>
                        <template #cellTemplate3="{ data }">
                          <div class="rowbtn" v-if="data.data.duration">CM</div>
                        </template>
                        <template #totalGroupCount3>
                          <div>
                            <DxButton
                              id="gridDeleteSelected"
                              :height="34"
                              :disabled="!selectedItemKeys.length"
                              icon="trash"
                              @click="selectionDel2"
                            />
                          </div>
                        </template>
                        <template #totalGroupCount2>
                          <div>
                            <DxButton
                              id="gridDeleteSelected"
                              :height="34"
                              icon="print"
                              @click="exportGrid()"
                            />
                          </div>
                        </template>
                      </DxDataGrid>
                    </div>
                  </div>
                </template>
              </DxItem>

              <DxItem title="C1">
                <template #default>
                  <div class="cuesheet-cart-table">
                    <div class="column">
                      <SortableWidget
                        :widgetIndex="widgetIndex"
                        :fileData="channelC_1"
                        :channelC_1_Class="(channelC_Class = 1)"
                      />
                    </div>
                  </div>
                </template>
              </DxItem>
              <DxItem title="C2">
                <template #default>
                  <div class="cuesheet-cart-table">
                    <div class="column">
                      <SortableWidget
                        :widgetIndex="widgetIndex"
                        :fileData="channelC_2"
                        :channelC_1_Class="(channelC_Class = 2)"
                      />
                    </div>
                  </div>
                </template>
              </DxItem>
              <DxItem title="C3">
                <template #default>
                  <div class="cuesheet-cart-table">
                    <div class="column">
                      <SortableWidget
                        :widgetIndex="widgetIndex"
                        :fileData="channelC_3"
                        :channelC_1_Class="(channelC_Class = 3)"
                      />
                    </div>
                  </div>
                </template>
              </DxItem>
              <DxItem title="C4">
                <template #default>
                  <div class="cuesheet-cart-table">
                    <div class="column">
                      <SortableWidget
                        :widgetIndex="widgetIndex"
                        :fileData="channelC_4"
                        :channelC_1_Class="(channelC_Class = 4)"
                      />
                    </div>
                  </div>
                </template>
              </DxItem>
              <DxItem title="즐겨찾기">
                <template #default>
                  <div class="cuesheet-cart-table">
                    <div class="column">
                      <SortableWidget
                        :widgetIndex="widgetIndex"
                        :fileData="channelC_my"
                        :channelC_1_Class="(channelC_Class = 0)"
                      />
                    </div>
                  </div>
                </template>
              </DxItem>
              <DxItem title="추가">
                <template #default>
                  <div>
                    <div>
                      <div id="fileuploader" class="fileuploader-container">
                        <DxFileUploader
                          select-button-text="파일 첨부"
                          accept="image/*"
                          :multiple="true"
                          label-text="큐시트에 필요한 대본 등을 첨부합니다. (※ 파일을 드래그하여 추가가 가능합니다.)"
                          upload-mode="instantly"
                        />
                      </div>
                      <div id="modelDiv" class="d-block text-center">
                        <div class="mb-3 mt-3" style="font-size: 20px">
                          <DxTextArea
                            :height="200"
                            :value.sync="valueForEditableTextArea"
                          />
                        </div>
                      </div>
                      <div class="tagDiv">
                        <DxTagBox
                          :accept-custom-value="true"
                          @customItemCreating="onCustomItemCreating"
                          placeholder="태그를 등록하세요 (※ 태그등록 시 큐시트 조회에서 태그 검색이 가능합니다.) "
                        />
                      </div>
                    </div>
                  </div>
                </template>
              </DxItem>
            </DxTabPanel>
          </div>
        </div>
        <div class="detailText">
          <span class="front">
            <span class="test">.</span>
            방송 예정일 :
            <span class="back">2021.04.21</span>
          </span>
          <span class="front">
            <span class="test">.</span>
            매체 :
            <span class="back">FM4U</span>
          </span>
          <span class="front">
            <span class="test">.</span>
            담당자 :
            <span class="back">김은비</span>
          </span>
          <span class="front">
            <span class="test">.</span>
            수정일 :
            <span class="back">2021.04.21 16:09</span>
          </span>
          <span class="autosavesapn">
            <b-form-checkbox v-model="checked" name="autosavebtn" switch>
              자동저장
            </b-form-checkbox>
          </span>
        </div>
      </div>
      <div>
        <div class="abchannel">
          <div class="tabletoptextgrid">DAP (A,B)</div>
          <DxDataGrid
            id="channelAB_id2"
            :focused-row-enabled="true"
            :data-source="channelAB"
            :row-alternation-enabled="true"
            height="450px"
            :focusedRowEnabled="true"
            :showColumnLines="false"
            :show-borders="true"
            :showRowLines="true"
            keyExpr="rowNO"
            @selection-changed="onSelectionChanged"
            @toolbar-preparing="onToolbarPreparing($event)"
            @focused-row-changed="onFocusedRowChanged"
            :show-column-headers="false"
          >
            <DxRowDragging
              dropFeedbackMode="indicate"
              :allow-reordering="true"
              :on-add="onAddChannelAB"
              :on-reorder="onReorder"
              :show-drag-icons="false"
              group="tasksGroup"
            />
            <DxEditing
              :allow-adding="true"
              :allow-updating="true"
              mode="cell"
            />
            <DxSelection mode="multiple" />
            <DxColumn
              cell-template="rowIndexTemplate"
              caption="순서"
              :width="100"
              alignment="center"
            />
            <DxColumn caption="" :width="80" cell-template="cellTemplate2" />
            <DxColumn
              caption="소재명"
              cell-template="cellTemplate3"
              data-field="name"
            />
            <DxColumn
              data-field="duration"
              caption="시간"
              alignment="center"
              :width="170"
            />
            <DxColumn
              caption="편집정보"
              alignment="center"
              :width="80"
              cell-template="cellTemplate1"
            />
            <DxScrolling mode="virtual" />
            <template #rowIndexTemplate="{ data }">
              <div>
                <div>{{ data.rowIndex + 1 }}</div>
              </div>
            </template>
            <template #cellTemplate1="{ data }">
              <div class="rowbtn" v-if="data.data.categoryName">
                <b-icon icon="bar-chart-fill"></b-icon>
              </div>
            </template>
            <template #cellTemplate2="{ data }">
              <div class="rowbtn" v-if="data.data.categoryName">
                <b-icon icon="folder-fill"></b-icon>
                <b-icon icon="suit-diamond-fill"></b-icon>
              </div>
            </template>
            <template #cellTemplate3="{ data }">
              <div>
                <div>{{ data.data.name }}</div>
                <div class="categoryname" v-if="data.data.categoryName">
                  {{ data.data.categoryName }}
                </div>
              </div>
            </template>
            <template #totalGroupCount>
              <div>
                <DxButton
                  id="gridDeleteSelected"
                  :height="34"
                  :disabled="!selectedItemKeys.length"
                  icon="trash"
                  @click="selectionDel"
                />
              </div>
            </template>
          </DxDataGrid>
          <DxSpeedDialAction
            icon="search"
            label="소재검색"
            @click="itemfindOn"
          />
        </div>
      </div>
    </div>
    <div class="schdiv">
      <div class="schlist mt-3"></div>
      <div class="schtable mt-3">
        <DxDataGrid
          :data-source="dataSource"
          :ref="dataGridRef"
          :row-alternation-enabled="true"
          height="340"
          :focusedRowEnabled="true"
          :showColumnLines="true"
          :show-borders="true"
          :showRowLines="true"
          keyExpr="rowNO"
          :element-attr="dataGridAttri"
        >
          <DxRowDragging :show-drag-icons="false" group="tasksGroup" />
          <DxColumn
            caption="순서"
            :width="50"
            alignment="center"
            data-field="rowNO"
          />
          <DxColumn data-field="name" caption="소재명" />
          <DxColumn data-field="categoryName" caption="분류" :width="130" />
          <DxColumn data-field="editorName" caption="제작자" :width="80" />
          <DxColumn data-field="duration" caption="사용시간" :width="110" />
          <DxColumn
            data-field="masteringDtm"
            caption="마스터링 일시"
            :width="180"
          />
          <DxColumn data-field="proType" caption="타입" :width="70" />
          <DxScrolling mode="virtual" />
          <template #cellTemplate3="{ data }">
            <div class="rowbtn" v-if="data === data">CM</div>
          </template>
        </DxDataGrid>
      </div>
      <!-- <div>
            <button
              id="searchBtnClose"
              type="button"
              class="btn btn-lg"
              @click="itemfindOff"
              style="background-color: transparent"
            >
              <b-icon-x></b-icon-x>
            </button>
          </div> -->
    </div>
  </div>
</template>

<script>
import { DxProgressBar } from "devextreme-vue/progress-bar";
import CuesheetTemplateList from "./cuesheetTemplateList";
import DxSpeedDialAction from "devextreme-vue/speed-dial-action";
import Player from "../../../components/Common/Player";
import DxTextArea from "devextreme-vue/text-area";
import DxTabPanel, { DxItem } from "devextreme-vue/tab-panel";
import DxDateBox from "devextreme-vue/date-box";
import { mapGetters } from "vuex";
import DxSelectBox from "devextreme-vue/select-box";
import { DxCheckBox } from "devextreme-vue/check-box";
import DxMenu from "devextreme-vue/menu";
import SortableWidget from "./SortableWidget";
import {
  DxDataGrid,
  DxColumn,
  DxEditing,
  DxSelection,
  DxScrolling,
  DxRowDragging,
} from "devextreme-vue/data-grid";
import DxDropDownButton from "devextreme-vue/drop-down-button";
import { DxFileUploader } from "devextreme-vue/file-uploader";
import DxTagBox from "devextreme-vue/tag-box";
import DxButton from "devextreme-vue/button";
import DxList from "devextreme-vue/list";
import DxTextBox from "devextreme-vue/text-box";
import font from "../../../data/font";
import axios from "axios";
import { jsPDF } from "jspdf";
import "jspdf-autotable";
import { exportDataGrid as exportDataGridToPdf } from "devextreme/pdf_exporter";
import {
  Document,
  Packer,
  Paragraph,
  Table,
  TableCell,
  TableRow,
  WidthType,
  Footer,
  Header,
} from "docx";
import { saveAs } from "file-saver";
const dataGridRef = "dataGrid";
const widgetIndex = 16;
const maxValue = 10;
function statusFormat(value) {
  return `Loading: ${value * 100}%`;
}
export default {
  components: {
    DxDataGrid,
    DxButton,
    DxColumn,
    DxScrolling,
    DxFileUploader,
    DxTagBox,
    DxSelection,
    DxList,
    DxTextBox,
    SortableWidget,
    DxRowDragging,
    DxMenu,
    DxCheckBox,
    DxSelectBox,
    DxTabPanel,
    DxItem,
    DxEditing,
    DxDateBox,
    DxTextArea,
    Player,
    DxSpeedDialAction,
    DxDropDownButton,
    CuesheetTemplateList,
    DxProgressBar,
  },
  data() {
    return {
      tttt: true,
      viewtitletext: "김이나의 별이 빛나는 밤에",
      testrowNO: 99,
      focusRowIndex: -1,
      focusRowIndex2: -1,
      valueForEditableTextArea: "메모를 삽입하세요.",
      maxValue,
      seconds: maxValue,
      buttonText: "확인",
      inProgress: false,
      statusFormat,
      tablewidth: "300px",
      modalData: true,
      temData: false,
      activefolderitem: [
        "템플릿 불러오기",
        "기본 큐시트 불러오기",
        "이전 큐시트 불러오기",
      ],
      downloaditem: [".zip", ".wav", ".pdf", ".docx", ".excel"],
      activetab: null,
      som: false,
      eom: false,
      code: "CM",
      requestType: "key",
      fileKey: 5000902,
      tempDownloadUrl: "/api/products/workspace/private/temp-download",
      waveformUrl: "/api/products/workspace/private/waveform",
      streamingUrl: "/api/products/workspace/private/streaming",
      direct: "Y",
      now: new Date(),
      menuitem_id: null,
      channelC_Class: 0,
      subjectList_sb: ["AM", "FM", "DMB"],
      subjectList_s: [
        "전체",
        "곡명",
        "곡명/아티스트",
        "아티스트",
        "배열번호",
        "국가명",
      ],
      subjectList_b: ["전체", "국내", "국외", "클래식"],
      subjectList_o: ["전체", "히트", "금지", "주의", "청소년 유해"],
      products: [
        { id: "1", name: "음반기록실" },
        { id: "2", name: "공유소재" },
        { id: "3", name: "(구)프로소재" },
        {
          id: "4",
          name: "광고협찬",
          items: [
            {
              id: "4_1",
              name: "부조SB",
            },
            {
              id: "4_2",
              name: "부조SPOT",
            },
            {
              id: "4_3",
              name: "프로그램CM",
            },
            {
              id: "4_4",
              name: "CM",
            },
          ],
        },
        {
          id: "5",
          name: "제작",
          items: [
            {
              id: "5_1",
              name: "취재물",
            },
            {
              id: "5_2",
              name: "효과음",
            },
            {
              id: "5_3",
              name: "Filler전부",
            },
          ],
        },
        {
          id: "6",
          name: "기타",
          items: [
            {
              id: "6_1",
              name: "MY공간",
            },
            {
              id: "6_2",
              name: "프로그램",
            },
            {
              id: "6_3",
              name: "DL3",
            },
            {
              id: "6_4",
              name: "주조SB",
            },
            {
              id: "6_5",
              name: "주조SPOT",
            },
          ],
        },
      ],
      viewtable: [],
      channelAB: [],
      channelC_1: [],
      channelC_2: [],
      channelC_3: [],
      channelC_4: [],
      channelC_my: [],
      selectedItemKeys: [],
      schdivVal: true,
      programSchedule: [],
      widgetIndex,
      dataSource2: ["My 공간", "제작", "음원", "광고", "편성MD"],
      dataSource3: [
        "프로그램",
        "부조 SPOT",
        "공유소재",
        "취재물",
        "(구)프로소재",
      ],
      onCustomItemCreating: (args) => {
        const newValue = args.text;
        args.customItem = newValue;
        this.editableProducts.unshift(newValue);
      },
      checked: true,
      dataGridRef,
      dataSource: null,
      searchItems: {
        cate: "",
        editor: "",
        end_dt: "20200329",
        media: "A",
        name: "",
        rowPerPage: 30,
        selectPage: 1,
        sortKey: "",
        sortValue: "",
        start_dt: "20191229",
        type: "Y",
      },
      dataGridAttri: {
        class: "schdatagrid",
      },
    };
  },
  created() {
    axios(`/api/products/old_pro`, {
      params: this.searchItems,
    }).then((res) => {
      this.dataSource = res.data.resultObject.data;
      console.log("axios -> dataSource Data");
      console.log(this.dataSource);
    });
  },
  computed: {
    ...mapGetters("cuesheet", ["cuesheetSchedule"]),
    select_color: function (data) {
      return {
        graycolor: data.data.categoryID,
      };
    },
    dataGrid: function () {
      return this.$refs[dataGridRef].instance;
    },
    progressValue() {
      return maxValue - this.seconds;
    },
  },
  methods: {
    onFocusedRowChanged(e) {
      this.focusRowIndex = e.rowIndex;
    },
    onFocusedRowChanged2(e) {
      this.focusRowIndex2 = e.rowIndex;
    },
    cancelClick(e) {
      this.buttonText = "확인";
      e();
    },
    time(value) {
      return `00:00:${`0${value}`.slice(-2)}`;
    },
    onButtonClick() {
      if (this.buttonText == "다운로드") {
        this.exportWord();
        return;
      }
      if (this.inProgress) {
        this.buttonText = "다시시작";
        clearInterval(this.intervalId);
      } else {
        this.buttonText = "중지";

        if (this.seconds === 0) {
          this.seconds = maxValue;
        }

        this.intervalId = setInterval(() => this.timer(), 1000);
      }

      this.inProgress = !this.inProgress;
    },

    timer() {
      this.seconds = this.seconds - 1;

      if (this.seconds == 0) {
        this.buttonText = "다운로드";
        //clearInterval(this.intervalId);
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
      if (e.itemData === "템플릿 불러오기") {
        this.$refs["modal-templateLoad"].show();
      }
    },
    alldelete_c() {
      console.log(this.activetab);
    },
    SelectionChanged(e) {
      this.activetab = e.addedItems[0].title;
    },
    c_eom() {
      this.eom = !this.eom;
    },
    c_som() {
      this.som = !this.som;
      console.log(this.som);
    },
    onValueChanged(value, cellInfo) {
      cellInfo.setValue(value);
      cellInfo.component.updateDimensions();
    },
    itemclick(e) {
      console.log(e.itemData.id);
      this.menuitem_id = e.itemData.id;
    },
    onToolbarPreparing(e) {
      let toolbarItems = e.toolbarOptions.items;
      toolbarItems.forEach((item) => {
        if (item.name === "addRowButton") {
          item.location = "after";
        }
        if (item.name === "addRowButton") {
          item.options = {
            icon: "add",
            onClick: () => {
              if (this.focusRowIndex != -1) {
                this.channelAB = [...this.channelAB];
                this.channelAB.splice(this.focusRowIndex + 1, 0, {
                  rowNO: this.testrowNO,
                  name: "",
                });
                this.testrowNO = this.testrowNO - 1;
              } else {
                this.channelAB = [...this.channelAB];
                this.channelAB.splice(0, 0, {
                  rowNO: this.testrowNO,
                  name: "",
                });
                this.testrowNO = this.testrowNO - 1;
              }
            },
          };
        }
      });
      toolbarItems.push({
        location: "after",
        template: "totalGroupCount",
      });
    },
    onToolbarPreparing2(e) {
      let toolbarItems = e.toolbarOptions.items;
      toolbarItems.forEach((item) => {
        if (item.name === "addRowButton") {
          item.location = "after";
        }
        if (item.name === "addRowButton") {
          item.options = {
            icon: "add",
            onClick: () => {
              if (this.focusRowIndex2 != -1) {
                this.viewtable = [...this.viewtable];
                this.viewtable.splice(this.focusRowIndex2 + 1, 0, {
                  rowNO: this.testrowNO,
                  name: "",
                });
                this.testrowNO = this.testrowNO - 1;
              } else {
                this.viewtable = [...this.viewtable];
                this.viewtable.splice(0, 0, {
                  rowNO: this.testrowNO,
                  name: "",
                });
                this.testrowNO = this.testrowNO - 1;
              }
            },
          };
        }
      });
      toolbarItems.push({
        location: "after",
        template: "totalGroupCount3",
      });
      toolbarItems.push({
        location: "after",
        template: "totalGroupCount2",
      });
    },
    selectionDel() {
      let a = this.channelAB;
      let b = this.selectedItemKeys;
      for (let i = 0; i < b.length; i++) {
        for (let j = 0; j < a.length; j++) {
          if (b[i].rowNO == a[j].rowNO) {
            a.splice(j, 1);
            break;
          }
        }
        this.channelAB = a;
      }
    },
    selectionDel2() {
      let a = this.viewtable;
      let b = this.selectedItemKeys;
      for (let i = 0; i < b.length; i++) {
        for (let j = 0; j < a.length; j++) {
          if (b[i].rowNO == a[j].rowNO) {
            a.splice(j, 1);
            break;
          }
        }
        this.viewtable = a;
      }
    },
    onSelectionChanged({ selectedRowsData }) {
      this.selectedItemKeys = selectedRowsData;
    },
    onAddChannelAB(e) {
      console.log(e.fromData);
      this.channelAB = [...this.channelAB];
      if (e.fromData !== undefined) {
        this.channelAB.splice(e.toIndex, 0, e.fromData);
      } else {
        this.channelAB.splice(e.toIndex, 0, e.itemData);
      }
    },
    onReorder(e) {
      this.channelAB = [...this.channelAB];
      this.channelAB.splice(e.fromIndex, 1);
      this.channelAB.splice(e.toIndex, 0, e.itemData);
    },
    onReorder2(e) {
      this.viewtable = [...this.viewtable];
      this.viewtable.splice(e.fromIndex, 1);
      this.viewtable.splice(e.toIndex, 0, e.itemData);
    },
    itemfindOff() {
      document.querySelector(".schdiv").classList.remove("schdivon");
      this.schdivVal = true;
    },
    itemfindOn() {
      this.tttt = !this.tttt;
      document.querySelector(".schdiv").classList.add("schdivon");
      this.schdivVal = false;
    },
    onCellPrepared(e) {
      if (e.row.data.categoryID) {
        e.cellElement.style.backgroundColor = "skyblue";
      }
    },
    exportWord() {
      const rows = [];
      this.dataSource.forEach((i) => {
        rows.push(
          new TableRow({
            children: [
              new TableCell({
                width: {
                  size: 7010,
                  type: WidthType.DXA,
                },
                children: [new Paragraph(i.name)],
              }),
              new TableCell({
                width: {
                  size: 2000,
                  type: WidthType.DXA,
                },
                children: [new Paragraph(i.editorName)],
              }),
            ],
          })
        );
      });
      const table = new Table({
        columnWidths: [3505, 5505],
        rows: rows,
      });

      const doc = new Document({
        sections: [
          {
            headers: {
              default: new Header({
                children: [new Paragraph("김이나의 별이 빛나는 밤에")],
              }),
            },
            footers: {
              default: new Footer({
                children: [
                  new Paragraph(
                    "참여방법 : #8001번 단문 50원, 장문&포토문자 100원 / 미니 무료 / (03925)서울시 마포구 성암로 267 별밤 (앞)"
                  ),
                ],
              }),
            },
            children: [table],
          },
        ],
      });

      Packer.toBlob(doc).then((blob) => {
        saveAs(blob, "example.docx");
      });
    },
    exportGrid() {
      const doc = new jsPDF();
      this.dataGrid.columnOption("소재명", "visible", false);
      doc.addFileToVFS("MBC NEW M-normal.ttf", font);
      doc.addFont("MBC NEW M-normal.ttf", "MBC NEW M", "normal");
      exportDataGridToPdf({
        jsPDFDocument: doc,
        component: this.dataGrid,
        customizeCell: function (options) {
          const { gridCell, pdfCell } = options;
          pdfCell.styles = {
            font: "MBC NEW M",
            fontSize: 10,
          };
        },
      }).then(() => {
        this.dataGrid.columnOption("소재명", "visible", true);
        const pageSize = doc.internal.pageSize;
        const pageWidth = pageSize.width ? pageSize.width : pageSize.getWidth();
        const pageHeight = pageSize.height
          ? pageSize.height
          : pageSize.getHeight();

        const header = "김이나의 별이 빛나는 밤에";
        const footer =
          "참여방법 : #8001번 단문 50원, 장문&포토문자 100원 / 미니 무료 / (03925)서울시 마포구 성암로 267 별밤 (앞)";
        doc.setFontSize(18);
        doc.setFont("MBC NEW M");
        doc.text(header, 25, 3, { baseline: "top" });
        doc.setFontSize(9);
        doc.text(
          footer,
          pageWidth / 2 - doc.getTextWidth(footer) / 2,
          pageHeight - 5,
          { baseline: "bottom" }
        );
        doc.autoPrint();
        // doc.save("Customers.pdf");
        //doc.output("dataurlnewwindow");

        const hiddFrame = document.createElement("iframe");
        hiddFrame.style.position = "fixed";
        hiddFrame.style.width = "1px";
        hiddFrame.style.height = "1px";
        hiddFrame.style.opacity = "0.01";
        const isSafari = /^((?!chrome|android).)*safari/i.test(
          window.navigator.userAgent
        );
        if (isSafari) {
          hiddFrame.onload = () => {
            try {
              hiddFrame.contentWindow.document.execCommand(
                "print",
                false,
                null
              );
            } catch (e) {
              hiddFrame.contentWindow.print();
            }
          };
        }
        hiddFrame.src = doc.output("bloburl");
        document.body.appendChild(hiddFrame);
      });
    },
  },
};
</script>

<style scope>
.tabletoptextgrid {
  font-size: 15px;
  padding: 7px;
  position: absolute;
  z-index: 8;
}
.abchannel {
  padding: 10px;
  background-color: white;
  border: solid 0.5px #ddd;
}
#progress-bar-status {
  display: inline-block;
}
.complete .dx-progressbar-range {
  background-color: green;
}
.wavemodalabel {
  padding: 0;
}
.wavemodalDiv {
  margin: auto;

  width: 520px;
}
.graycolor {
  color: #cccccc;
}
.dx-header-row {
  font-size: 12px;
}
.durationTime {
  font-size: 12px;
}
.dataname {
  color: #cccccc;
  font-size: 13px;
}
.categoryname {
  color: #cccccc;
  font-size: 12px;
}
#channelAB_id td,
#channelAB_id2 td {
  vertical-align: middle;
}
/* 파일첨부문구 */
.dx-fileuploader-input-container {
  width: 800px;
}
/* 파형 크기설정 */
.playerout {
  position: absolute;
  width: 99%;
  padding: 5px 10px 10px 10px;
}
.playerDiv {
  width: 100%;
  height: 100%;
}
/* tab내부높이 */
.dx-multiview-wrapper {
  height: 430px;
  padding: 10px;
}
/* 드래그 안고장나게해주는 CSS */
#app-container {
  position: fixed;
}

/* C채널 즐겨찾기 */
.column {
  background-color: white;
}
.cuesheet-cart-table {
  height: 100%;
  width: 100%;
  display: grid;
}

/* 출력뷰 */
.dx-widget {
  background-color: white;
}

#datagridtitleText {
  position: absolute;
  margin: 10px 0px 0px 350px;
  font-size: 17px;
}

.datagridsubText {
  z-index: 4;
  letter-spacing: 0.2px;
  font-size: 14px;
  position: absolute;
  top: 40px;
  right: 20px;
}

.pdtext {
  letter-spacing: 1px;
}

#datagridbottomText {
  position: absolute;
  bottom: 12px;
  font-size: 13px;
  margin: 10px 0px 0px 0px;
}

/* 소재검색 버튼 */
.dx-fa-button.dx-fa-button-main .dx-overlay-content {
  background-color: #008ecc;
}

#searchBtnClose {
  color: #008ecc;
  padding: 0;
  width: 50px;
  height: 40px;
  position: absolute;
  top: 10px;
  right: 10px;
  font-weight: 550;
  font-size: 27px;
}

/* 소재 토글창 */
.schdiv {
  position: fixed;
  border-top: thick solid #008ecc;
  bottom: 0px;
  z-index: 6;
  background-color: white;
  width: 1715px;
  height: 375px;
  /* transition-duration: 1s; */
}

#schtitleText {
  margin: 0px 0px 0px 30px;
  font-size: 25.2px;
  color: #3a3a3a;
  font-weight: 500;
  line-height: 27.84px;
}

/* 토글 On / Off CSS */
.schdivon {
  /* transition-duration: 1s;
  transform: translate(1955px, 0px); */
}

/* 소재검색 상단 목록 */
.subjectList {
  display: inline-block;
}

.subjectDiv {
  border-right: 1px solid #d7d7d7;
}

.dx-menu-item-wrapper {
  width: 130px;
  line-height: 23.5px;
}

.schlist {
  display: grid;
  border: solid 0.5px rgb(182, 182, 182);
  grid-template-columns: 1fr 5fr;
  border-radius: 2px;
  position: absolute;
  width: 470px;
  height: 340px;
  padding: 10px 10px 10px 10px;
  margin: 0px 0px 0px 20px;
}

#schbtn2 {
  width: 100px;
  font-weight: 300;
  font-size: 13px;
}

/* 소재검색 테이블 */
.dx-cell-focus-disabled {
  text-align: center;
}

.schtable {
  right: 0;
  border-radius: 1.5px;
  position: absolute;
  width: 1200px;
  margin: 10px 0px 0px 20px;
}

.schtext {
  position: absolute;
  top: 325px;
  right: 30px;
}

/* 도구아이콘 */
.iconDiv {
  position: absolute;
  top: 40px;
  right: 0px;
}

/* 아이콘 */
.b-icon {
  margin: 0px 8px -1px 0px;
}

/* .rowbtn > i {
  color: #d8d8d8;
} */

.dx-icon-globe {
  padding: 0px 5px 5px 0px;
  color: #2a4878 !important;
}

.toptext > .dx-icon-globe {
  position: fixed;
  font: 40px/1 DXIcons !important;
  margin: 15px 5px 4px 18px !important;
}

/* 버튼 */
.btn {
  margin-left: 5px;
}

/* 메인타이틀옆 부가정보 타이틀 */
.subTitle {
  margin-right: 5px;
  color: #3a3a3a;
  font-size: 12.8px;
  font-weight: 100;
}

.badge {
  width: 40px;
  height: 25px;
  font-size: 12.5px;
}

#subtitleDiv {
  margin: 5px 0px 20px 0px;
}

.test {
  width: 2px;
  height: 15px;
  background-color: #008ecc;
  border-radius: 1.5px;
  margin: 0px 4px 0px 5px;
}

.separator {
  border-bottom: 1px solid #d7d7d7;
}
.back {
  font-weight: 200;
  margin: 0px 0px 0px 5px;
}
.front {
  font-size: 12.8px;
  font-weight: 200;
  margin: 0px 20px 0px 0px;
}

/* 좌측 프로그램 상세 정보 */
.detailText {
  bottom: 0;
  height: 50px;
  padding: 15px 0px 0px 0px;
}

#autosavebtn {
  float: right;
}

.autosavesapn {
  position: absolute;
  right: 10px;
  bottom: 15px;
}

/* Main */
.overView {
  margin-top: -20px;
  margin-right: -35px;
  margin-bottom: -20px;
}

/* 좌측도구 전체DIV */
.toolDiv {
  position: relative;
  width: 95%;
}

.dx-widget {
  font-family: "MBC 새로움 M";
}

#cuesheet_view {
  position: static;
  display: grid;
  grid-template-columns: 1fr 1fr;
}

/* 좌측하단 버튼 */
.buttonDiv {
  position: absolute;
  right: 0px;
  top: 40px;
}

#listbtn {
  background-color: #fff;
  border-bottom-color: rgb(187, 187, 187);
  color: #3a3a3a;
  border: 1px solid #bbb;
  padding: 0.5rem 1rem;
  font-size: 0.8rem;
  line-height: 1.8;
  font-weight: 250;
}

/* 소재상세보기 */
#musicTool {
  border: solid 0.5px rgb(182, 182, 182);
  background-color: white;
  border-radius: 4px;
  text-align: center;
  width: 100%;
  height: 350px;
  display: grid;
  grid-template-rows: 1fr 6fr;
}

.musicseparator {
  height: 80px;
  border-bottom: 1px solid #d7d7d7;
}

.toptextin {
  letter-spacing: 0.8px;
  position: fixed;
  font-size: 20.5px;
  margin: 18px 5px 0px 70px;
}

.toptext {
  position: absolute;
  margin: 5px 0px 0px 0px;
}

.centertext {
  color: #6c757d;
  letter-spacing: 0.6px;
  position: absolute;
  left: 280px;
  margin: 10px 0px 0px 0px;
  text-align: left;
}

.sidebtn {
  position: absolute;
  right: 10px;
  margin: 35px 0px 0px 0px;
}

/* 태그 */
.fileuploader-container,
.tagDiv {
  border: 1px solid #d3d3d3;
  border-radius: 4px;
  margin-top: 20px;
}
.tagDiv {
  width: 100%;
  overflow: auto;
}

/* 파일업로드 */
.dx-fileuploader-files-container {
  height: 240px;
  overflow: auto;
}
.dx-fileuploader-files-container {
  padding: 0px 0px 0px 0px;
}
.dx-fileuploader-show-file-list .dx-fileuploader-files-container {
  padding-top: 0px;
}
.dx-fileuploader-file-container {
  background-color: white;
}
</style>