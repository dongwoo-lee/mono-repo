<template>
  <div class="overView2">
    <div id="cuesheet_view2">
      <div class="toolDiv2">
        <div class="schdiv2">
          <div id="schtitleText2" class="mt-4">
            즐겨찾기
            <span id="subtitleDiv2" class="ml-3">
              <span class="subTitle2">|</span>
              <span class="subTitle2"
                >드래그하여 소재를 추가할 수 있습니다.</span
              >
            </span>
          </div>
          <div class="schlist2 mt-3">
            <div class="subjectDiv2">
              <DxMenu
                class="subjectMenu2"
                :data-source="products"
                orientation="vertical"
                :selectedItem="products[0]"
                display-expr="name"
                :selectByClick="true"
                selectionMode="single"
                :onItemClick="itemclick"
              />
            </div>
            <div class="schsch2" v-if="menuitem_id != '4_1'">
              <h3 class="ml-5 mb-5 mt-3">음반기록실</h3>
              <div class="ml-5">
                <div class="subjectListDiv2 mt-3 mb-3">
                  <span class="subjectList2 mr-2">대분류 :</span>
                  <DxSelectBox
                    class="subjectList2 mr-3"
                    :items="subjectList_b"
                    :value="subjectList_b[0]"
                    width="120px"
                    height="35px"
                  />
                  <span class="subjectList2 mr-2">소분류 :</span>
                  <DxSelectBox
                    class="subjectList2 mr-2"
                    :items="subjectList_s"
                    :value="subjectList_s[0]"
                    width="150px"
                    height="35px"
                  />
                  <span class="subjectList2 mr-2">검색옵션 :</span>
                  <DxSelectBox
                    class="subjectList2 mr-3"
                    :items="subjectList_o"
                    :value="subjectList_o[0]"
                    width="120px"
                    height="35px"
                  />
                </div>
                <div class="subjectListDiv2 mt-3 mb-3">
                  <span class="subjectList2 mr-2">검색어 :</span>
                  <DxTextBox
                    placeholder=""
                    class="subjectList2 mr-2"
                    width="250px"
                  />
                  <button
                    type="button"
                    id="schbtn2"
                    class="btn btn-outline-primary btn-sm"
                  >
                    검색
                  </button>
                </div>
              </div>
            </div>
            <div class="schsch2" v-else>
              <h3 class="ml-5 mb-5 mt-3">부조SB</h3>
              <div class="ml-5">
                <div>
                  <span class="subjectList2 mr-2">방송일 :</span>
                  <DxDateBox
                    :value="now"
                    type="date"
                    class="subjectList mr-3"
                    width="130px"
                    height="35px"
                    display-format="yyyy-MM-dd"
                  />
                  <span class="subjectList2 mr-2">매체 :</span>
                  <DxSelectBox
                    class="subjectList2 mr-2"
                    :items="subjectList_sb"
                    :value="subjectList_sb[0]"
                    width="120px"
                    height="35px"
                  />
                </div>
                <div class="subjectListDiv2 mt-3 mb-3">
                  <span class="subjectList2 mr-2">사용처 :</span>
                  <DxTextBox
                    placeholder=""
                    class="subjectList2 mr-2"
                    width="250px"
                  />
                  <button
                    type="button"
                    id="schbtn2"
                    class="btn btn-outline-primary btn-sm"
                  >
                    검색
                  </button>
                </div>
              </div>
            </div>
          </div>
          <div class="schseparator2 mb-5"></div>
          <div class="schtext2">전체 : 152 건</div>
          <div class="schtable2">
            <DxDataGrid
              :data-source="dataSource"
              :ref="dataGridRef"
              :row-alternation-enabled="true"
              height="407"
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
                <div class="rowbtn2" v-if="data === data">CM</div>
              </template>
            </DxDataGrid>
          </div>
          <button
            type="button"
            id="saveBtn2"
            class="btn btn-outline-primary btn-lg"
          >
            <b-icon-check-circle></b-icon-check-circle> 즐겨찾기 저장
          </button>
        </div>
      </div>
    </div>
    <div>
      <div id="musicTool2">
        <div class="musicseparator2">
          <div class="toptext2">
            <i class="dx-icon-globe"></i>
            <span class="toptextin2">꿈의팝송72-b</span>
          </div>
          <div class="centertext2">
            <div>카테고리 : 꿈의팝송</div>
            <div>편집자 : 김현경</div>
            <div>편집일 : 2020-03-29 16:11:44</div>
          </div>
          <div class="sidebtn2">
            <button
              type="button"
              class="btn btn-outline-dark btn-sm"
              @click="c_som"
            >
              SOM
            </button>
            <button
              type="button"
              class="btn btn-outline-dark btn-sm"
              @click="c_eom"
            >
              EOM
            </button>
          </div>
        </div>
        <div class="playerDiv2">
          <div class="playerout2">
            <Player
              ref="play"
              :requestType="requestType"
              :fileKey="fileKey"
              :tempDownloadUrl="tempDownloadUrl"
              :waveformUrl="waveformUrl"
              :streamingUrl="streamingUrl"
              :direct="direct"
              :som="som"
              :eom="eom"
            />
          </div>
        </div>
      </div>
      <div class="TabDiv2">
        <div class="cuesheet-cart-table2">
          <div class="column2">
            <SortableWidget
              :widgetIndex="widgetIndex"
              :fileData="channelC_my"
              :channelC_1_Class="(channelC_Class = 0)"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import Player from "../../../components/Common/Player";
import DxTabPanel, { DxItem } from "devextreme-vue/tab-panel";
import DxDateBox from "devextreme-vue/date-box";
import { mapGetters } from "vuex";
import DxSelectBox from "devextreme-vue/select-box";
import DxMenu from "devextreme-vue/menu";
import SortableWidget from "./SortableWidget_c";
import {
  DxDataGrid,
  DxColumn,
  DxSelection,
  DxScrolling,
  DxRowDragging,
} from "devextreme-vue/data-grid";
import DxButton from "devextreme-vue/button";
import DxTextBox from "devextreme-vue/text-box";
import axios from "axios";

const dataGridRef = "dataGrid";
const widgetIndex = 16;
export default {
  components: {
    DxDataGrid,
    DxButton,
    DxColumn,
    DxScrolling,
    DxSelection,
    DxTextBox,
    SortableWidget,
    DxRowDragging,
    DxMenu,
    DxSelectBox,
    DxTabPanel,
    DxItem,
    DxDateBox,
    Player,
  },
  data() {
    return {
      som: false,
      eom: false,
      requestType: "key",
      fileKey: 5000902,
      tempDownloadUrl: "/api/products/workspace/private/temp-download",
      waveformUrl: "/api/products/workspace/private/waveform",
      streamingUrl: "/api/products/workspace/private/streaming",
      direct: "Y",
      now: new Date(),
      menuitem_id: null,
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
      channelC_my: [],
      widgetIndex,
      dataSource2: ["My 공간", "제작", "음원", "광고", "편성MD"],
      dataSource3: [
        "프로그램",
        "부조 SPOT",
        "공유소재",
        "취재물",
        "(구)프로소재",
      ],
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
    };
  },
  created() {
    axios(`/api/products/old_pro`, {
      params: this.searchItems,
    }).then((res) => {
      this.dataSource = res.data.resultObject.data;
    });
  },
  computed: {
    ...mapGetters("cuesheet", ["cuesheetSchedule"]),

    dataGrid: function () {
      return this.$refs[dataGridRef].instance;
    },
  },
  methods: {
    c_eom() {
      this.eom = !this.eom;
    },
    c_som() {
      this.som = !this.som;
      console.log(this.som);
    },
    onAddChannelAB(e) {
      this.channelAB = [...this.channelAB];
      if (e.fromData !== undefined) {
        this.channelAB.splice(e.toIndex, 0, e.fromData);
      } else {
        this.channelAB.splice(e.toIndex, 0, e.itemData);
      }
    },
  },
};
</script>

<style scopedSlots>
/* 드래그 안고장나게해주는 CSS */
#app-container {
  position: fixed;
}
#saveBtn2 {
  position: absolute;
  left: 660px;
  bottom: 10px;
}
.schtext2 {
  text-align: right;
  padding-right: 15px;
}
.schtable2 {
  border-radius: 1.5px;
  top: 340px;
  position: static;
  width: 835px;
  margin: 10px 0px 0px 20px;
}
.subjectList2 {
  display: inline-block;
}
.schdiv2 {
  position: relative;
  left: -20px;
  top: 0px;
  background-color: white;
  border-right: thick solid #008ecc;
  width: 890px;
  height: 840px;
  padding: 10px;
}
.schlist2 {
  display: grid;
  border: solid 0.5px rgb(182, 182, 182);
  grid-template-columns: 1fr 5fr;
  border-radius: 2px;
  position: static;
  top: 60px;
  width: 835px;
  height: 220px;
  padding: 10px 10px 10px 10px;
  margin: 10px 0px 0px 20px;
}
.subTitle2 {
  margin-right: 5px;
  color: #3a3a3a;
  font-size: 12.8px;
  font-weight: 100;
}
#schtitleText2 {
  margin: 0px 0px 0px 30px;
  font-size: 25.2px;
  color: #3a3a3a;
  font-weight: 500;
  line-height: 27.84px;
}
.TabDiv2 {
  margin-top: 11px;
  border: solid 1px #008ecc;
  border-radius: 4px;
  width: 100%;
  height: 480px;
}
/* C채널 즐겨찾기 */
.column2 {
  margin: 5px;
  background-color: white;
}
.cuesheet-cart-table2 {
  height: 100%;
  width: 100%;
  display: grid;
}

/* 파형 크기설정 */
.playerout2 {
  width: 99%;
  height: 500px;
  padding: 5px 10px 10px 10px;
}
.playerDiv2 {
  width: 100%;
  height: 100%;
}

.overView2 {
  display: grid;
  grid-template-columns: 1fr 1fr;
  margin-top: -20px;
  margin-right: -35px;
  margin-bottom: -20px;
}

#musicTool2 {
  border: solid 0.5px rgb(182, 182, 182);
  background-color: white;
  border-radius: 4px;
  text-align: center;
  width: 100%;
  height: 350px;
  display: grid;
  grid-template-rows: 1fr 6fr;
  position: relative;
}

.musicseparator2 {
  height: 80px;
  border-bottom: 1px solid #d7d7d7;
}

.toptext2 > .dx-icon-globe {
  position: fixed;
  font: 40px/1 DXIcons !important;
  margin: 15px 5px 4px 18px !important;
}

.toptextin2 {
  letter-spacing: 0.8px;
  position: fixed;
  font-size: 20.5px;
  margin: 18px 5px 0px 70px;
}

.toptext2 {
  position: absolute;
  margin: 5px 0px 0px 0px;
}

.centertext2 {
  color: #6c757d;
  letter-spacing: 0.6px;
  position: absolute;
  left: 280px;
  margin: 10px 0px 0px 0px;
  text-align: left;
}

.sidebtn2 {
  position: absolute;
  right: 10px;
  margin: 35px 0px 0px 0px;
}
</style>