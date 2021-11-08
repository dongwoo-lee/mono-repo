<template>
  <div class="overView">
    <div id="cuesheet_view">
      <div class="toolDiv">
        <div v-if="tttt">
          <div id="subtitleDiv">
            <span class="subtitle">큐시트</span>
            <span class="subtitle">|</span>
            <span class="subtitle">이전 큐시트 조회</span>
          </div>
          <div>
            <h1>김이나의 별이 빛나는 밤에</h1>
          </div>
          <div class="separator mb-3 mt-0"></div>
          <div class="iconDiv">
            <DxButton icon="arrowleft" v-b-modal.modal-new />
            <DxButton icon="columnchooser" v-b-modal.modal-template />
            <DxDropDownButton
              :items="downloaditem"
              :drop-down-options="{ width: 70 }"
              icon="download"
              :showArrowIcon="false"
              @item-click="onDownloadItemClick"
            />
          </div>
          <div class="detailText">
            <span class="front">
              <span class="test">.</span>
              방송일 :
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
              최종수정일 :
              <span class="back">2021.04.21 16:09</span>
            </span>
          </div>
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
          id="modal-save"
          size="lg"
          centered
          title="큐시트 저장"
          ok-title="확인"
          cancel-title="취소"
        >
          <div id="modelDiv" class="d-block text-center">
            <div class="mb-3 mt-3" style="font-size: 20px">
              <div class="mb-3">큐시트를 저장합니다.</div>
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
                현재 DAP (A,B) 탭에 추가된 소재를 하나의 WAV 파일로 병합합니다.
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
                <div class="dx-field-label" style="font-size: 15px">진행 :</div>
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
        <div v-if="!tttt">
          <div class="iconDivClose">
            <div class="pt-1">
              <DxButton icon="save" type="default" v-b-modal.modal-save />
            </div>
            <div class="pt-1">
              <DxDropDownButton
                :items="downloaditem"
                :drop-down-options="{ width: 70 }"
                width="36px"
                icon="download"
                :showArrowIcon="false"
                @item-click="onDownloadItemClick"
              />
            </div>
            <div class="pt-1">
              <DxDropDownButton
                :items="activefolderitem"
                width="36px"
                :drop-down-options="{ width: 160 }"
                icon="activefolder"
                :showArrowIcon="false"
                @item-click="onActivefolderItemClick"
              />
            </div>
            <div class="pt-1">
              <DxButton icon="columnchooser" v-b-modal.modal-template />
            </div>
            <div class="pt-1">
              <DxButton icon="doc" v-b-modal.modal-new />
            </div>
            <div class="pt-1">
              <DxButton icon="arrowleft" v-b-modal.modal-new />
            </div>
          </div>
        </div>
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
                        :ref="dataGridRef"
                        :height="view_channel_height"
                        :focusedRowEnabled="false"
                        :showColumnLines="true"
                        :show-borders="true"
                        :showRowLines="true"
                        @selection-changed="onSelectionChanged_view"
                        @toolbar-preparing="viewtableOnToolbarPreparing($event)"
                        @focused-row-changed="onFocusedRowChanged2"
                        keyExpr="rowNO"
                      >
                        <DxEditing
                          :allow-adding="true"
                          mode="cell"
                          :allow-updating="true"
                          start-edit-action="dblClick"
                        />
                        <DxSelection
                          mode="multiple"
                          showCheckBoxesMode="none"
                        />
                        <DxRowDragging
                          dropFeedbackMode="indicate"
                          :allow-reordering="true"
                          :on-reorder="onReorder2"
                          :on-add="onAddViewChannel"
                          :on-drag-start="onDragStart"
                          :on-drag-end="onDragEnd"
                          :show-drag-icons="false"
                          group="tasksGroup"
                        />
                        <DxColumn
                          width="13%"
                          edit-cell-template="cellTemplate8"
                          alignment="center"
                          data-field="code"
                          caption="코드"
                        />
                        <DxColumn
                          caption="내용"
                          data-field="name"
                          alignment="center"
                          cell-template="cellTemplate12"
                        />
                        <DxColumn
                          css-class="durationTime"
                          data-field="startDuration"
                          caption="사용시간"
                          alignment="center"
                          :width="100"
                        />
                        <DxColumn
                          css-class="durationTime"
                          caption="시작시간"
                          :calculate-cell-value="calculateSalesAmount"
                          alignment="center"
                          :width="100"
                        />
                        <DxColumn
                          caption="비고"
                          :width="150"
                          alignment="center"
                          data-field="remarks"
                        />
                        <DxScrolling mode="virtual" />
                        <template #cellTemplate8="{ data: cellInfo }">
                          <div>
                            <DxSelectBox
                              :items="code_list"
                              placeholder=""
                              :on-value-changed="
                                (value) => onValueChanged(value, cellInfo)
                              "
                              :value="cellInfo.data.code"
                            />
                          </div>
                        </template>
                        <template #cellTemplate12="{ data: cellInfo }">
                          <div>
                            {{ cellInfo.data.name }}
                          </div>
                        </template>
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
                        <template #totalGroupCount3>
                          <div>
                            <DxButton
                              id="gridDeleteSelected"
                              :height="34"
                              :disabled="!selectedItemKeys_view.length"
                              icon="trash"
                              @click="selectionDel_view"
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
                        <template #totalGroupCount_setting>
                          <div>
                            <DxButton
                              id="gridDeleteSelected"
                              :height="34"
                              icon="preferences"
                              v-b-modal.modal-setting
                            />
                          </div>
                        </template>
                      </DxDataGrid>
                    </div>
                  </div>
                </template>
              </DxItem>
              <template #title="data">
                <div>
                  <span v-if="data.index == 0"
                    >출력용
                    <span v-if="activetab == '출력용'" class="ml-1">
                      <DxButton icon="trash" id="c1tabbtn" /> </span
                  ></span>
                  <span v-if="data.index == 1"
                    >C1
                    <span v-if="activetab == 'C1'" class="ml-1">
                      <DxButton icon="trash" id="c1tabbtn" />
                    </span>
                  </span>
                  <span v-if="data.index == 2"
                    >C2
                    <span v-if="activetab == 'C2'" class="ml-1">
                      <DxButton icon="trash" id="c1tabbtn" /> </span
                  ></span>
                  <span v-if="data.index == 3"
                    >C3
                    <span v-if="activetab == 'C3'" class="ml-1">
                      <DxButton icon="trash" id="c1tabbtn" /> </span
                  ></span>
                  <span v-if="data.index == 4"
                    >C4
                    <span v-if="activetab == 'C4'" class="ml-1">
                      <DxButton icon="trash" id="c1tabbtn" /> </span
                  ></span>
                  <span v-if="data.index == 5"
                    >즐겨찾기
                    <span v-if="activetab == '즐겨찾기'" class="ml-1">
                      <DxButton icon="trash" id="c1tabbtn" />
                    </span>
                  </span>
                  <span v-if="data.index == 6"
                    >부가정보
                    <span v-if="activetab == '부가정보'" class="ml-1">
                      <DxButton icon="trash" id="c1tabbtn" /> </span
                  ></span>
                </div>
              </template>
              <DxItem title="C1">
                <template #default>
                  <div class="cuesheet-cart-table">
                    <div class="column">
                      <SortableWidgetC
                        :widgetIndex="widgetIndex"
                        :fileData="channelC_1"
                        :channelC_1_Class="(channelC_Class = 1)"
                        :tttt="tttt"
                      />
                    </div>
                  </div>
                </template>
              </DxItem>
              <DxItem title="C2">
                <template #default>
                  <div class="cuesheet-cart-table">
                    <div class="column">
                      <SortableWidgetC
                        :widgetIndex="widgetIndex"
                        :fileData="channelC_2"
                        :channelC_1_Class="(channelC_Class = 2)"
                        :tttt="tttt"
                      />
                    </div>
                  </div>
                </template>
              </DxItem>
              <DxItem title="C3">
                <template #default>
                  <div class="cuesheet-cart-table">
                    <div class="column">
                      <SortableWidgetC
                        :widgetIndex="widgetIndex"
                        :fileData="channelC_3"
                        :channelC_1_Class="(channelC_Class = 3)"
                        :tttt="tttt"
                      />
                    </div>
                  </div>
                </template>
              </DxItem>
              <DxItem title="C4">
                <template #default>
                  <div class="cuesheet-cart-table">
                    <div class="column">
                      <SortableWidgetC
                        :widgetIndex="widgetIndex"
                        :fileData="channelC_4"
                        :channelC_1_Class="(channelC_Class = 4)"
                        :tttt="tttt"
                      />
                    </div>
                  </div>
                </template>
              </DxItem>
              <DxItem title="즐겨찾기">
                <template #default>
                  <div class="cuesheet-cart-table">
                    <div class="column">
                      <SortableWidgetC
                        :widgetIndex="widgetIndex"
                        :fileData="channelC_my"
                        :channelC_1_Class="(channelC_Class = 0)"
                        :tttt="tttt"
                      />
                    </div>
                  </div>
                </template>
              </DxItem>
              <DxItem title="부가정보">
                <template #default>
                  <div style="padding: 5px">
                    <div>
                      <div id="fileuploader" class="fileuploader-container">
                        <DxFileUploader
                          select-button-text="파일 첨부"
                          accept="image/*"
                          :height="310"
                          :multiple="true"
                          label-text="큐시트에 필요한 대본 등을 첨부합니다. (※ 파일을 드래그하여 추가가 가능합니다.)"
                          upload-mode="instantly"
                        />
                      </div>
                      <div id="modelDiv" class="d-block text-center">
                        <div class="mb-3 mt-3" style="font-size: 20px">
                          <DxTextArea
                            :height="232"
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
      </div>
      <div>
        <div class="abchannel">
          <div class="tabletoptextgrid">
            DAP (A,B) <DxButton icon="trash" id="c1tabbtn" />
          </div>
          <DxDataGrid
            id="channelAB_id2"
            :focused-row-enabled="true"
            :data-source="channelAB"
            :height="ab_channel_height"
            :focusedRowEnabled="false"
            :showColumnLines="false"
            :show-borders="true"
            :showRowLines="true"
            keyExpr="abchannelrowNO"
            @selection-changed="onSelectionChanged_ab"
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
              start-edit-action="dblClick"
            />
            <DxSelection mode="multiple" showCheckBoxesMode="none" />
            <DxColumn
              cell-template="rowIndexTemplate"
              caption="순서"
              :width="70"
              alignment="center"
            />
            <DxColumn caption="" :width="80" cell-template="cellTemplate2" />
            <DxColumn
              caption="소재명"
              cell-template="cellTemplate3"
              data-field="name"
            />
            <DxColumn
              caption="시간"
              alignment="center"
              :width="120"
              cell-template="cellTemplate4"
            />
            <DxColumn
              caption="편집정보"
              alignment="center"
              data-field="transtype"
              :width="120"
              cell-template="cellTemplate1"
            />
            <DxScrolling mode="virtual" />
            <template #rowIndexTemplate="{ data }">
              <div>
                <div>{{ data.rowIndex + 1 }}</div>
              </div>
            </template>
            <template #cellTemplate1="{ data }">
              <div class="rowbtn" v-if="data.data[0].categoryName">
                <b-icon icon="align-start"></b-icon>
                <b-icon icon="align-end"></b-icon>
                <b-icon icon="play-circle-fill" style="color: #008ecc"></b-icon>
              </div>
            </template>
            <template #cellTemplate2="{ data }">
              <div class="rowbtn" v-if="data.data[0].categoryName">
                <b-icon icon="disc"></b-icon>
                <b-icon
                  icon="suit-diamond-fill"
                  style="color: #ffc107"
                  @click="iconselect(data)"
                  v-if="data.data.transtype === 's'"
                ></b-icon>
                <b-icon
                  icon="arrow-repeat"
                  style="color: #f44336"
                  @click="iconselect(data)"
                  v-if="data.data.transtype === 'l'"
                ></b-icon>
                <b-icon
                  icon="arrow-down"
                  style="color: #03a9f4"
                  @click="iconselect(data)"
                  v-if="data.data.transtype === 'c'"
                ></b-icon>
              </div>
            </template>
            <template #cellTemplate3="{ data }">
              <div>
                <div>
                  {{ data.data[0].name }}
                </div>
                <div class="categoryname" v-if="data.data[0].categoryName">
                  {{ data.data[0].categoryName }}
                </div>
              </div>
            </template>
            <template #cellTemplate4="{ data }">
              <div>
                <div v-if="data.data[0].categoryName">
                  {{ data.data[0].duration }}
                </div>
              </div>
            </template>
            <template #totalGroupCount>
              <div>
                <DxButton
                  id="gridDeleteSelected"
                  :height="34"
                  :disabled="!selectedItemKeys_ab.length"
                  icon="trash"
                  @click="selectionDel_ab"
                />
              </div>
            </template>
          </DxDataGrid>
        </div>
      </div>
    </div>
    <div class="schdiv">
      <div class="schlist mt-3">
        <div>
          <DxMenu
            id="subjectMenu"
            :data-source="products"
            orientation="vertical"
            :selectedItem="products[0]"
            display-expr="name"
            :selectByClick="true"
            selectionMode="single"
            :onItemClick="itemclick"
          />
        </div>
        <div>
          <div class="schsch" v-if="menuitem_id != '4_1'">
            <h3 class="ml-4 mb-5 mt-3">음반기록실</h3>
            <div class="ml-5">
              <div class="mt-3 mb-3">
                <div>
                  <span class="mr-3">대분류 :</span>
                  <DxSelectBox
                    class="subjectList mr-3"
                    :items="subjectList_b"
                    :value="subjectList_b[0]"
                    width="150px"
                    height="35px"
                  />
                </div>
                <div class="mt-2">
                  <span class="mr-3">소분류 :</span>
                  <DxSelectBox
                    class="subjectList mr-2"
                    :items="subjectList_s"
                    :value="subjectList_s[0]"
                    width="150px"
                    height="35px"
                  />
                </div>
                <div class="mt-2">
                  <span class="mr-2">검색옵션 :</span>
                  <DxSelectBox
                    class="subjectList mr-3"
                    :items="subjectList_o"
                    :value="subjectList_o[0]"
                    width="150px"
                    height="35px"
                  />
                </div>
              </div>
              <div class="mt-4 mb-3">
                <div>
                  <span class="mr-3">검색어 :</span>
                  <DxTextBox
                    placeholder=""
                    class="subjectList mr-3"
                    width="200px"
                  />
                </div>
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
          <div class="schsch" v-else>
            <h3 class="ml-5 mb-5 mt-3">부조SB</h3>
            <div class="ml-5">
              <div>
                <span class="subjectList mr-2">방송일 :</span>
                <DxDateBox
                  :value="now"
                  type="date"
                  class="subjectList mr-3"
                  width="130px"
                  height="35px"
                  display-format="yyyy-MM-dd"
                />
                <span class="subjectList mr-2">매체 :</span>
                <DxSelectBox
                  class="subjectList mr-2"
                  :items="subjectList_sb"
                  :value="subjectList_sb[0]"
                  width="120px"
                  height="35px"
                />
              </div>
              <div class="subjectListDiv mt-3 mb-3">
                <span class="subjectList mr-2">사용처 :</span>
                <DxTextBox
                  placeholder=""
                  class="subjectList mr-2"
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
      </div>
      <div class="schtable mt-3">
        <DxDataGrid
          :data-source="dataSource"
          :ref="dataGridRef"
          height="400"
          :focusedRowEnabled="false"
          :showColumnLines="true"
          :show-borders="true"
          :row-alternation-enabled="true"
          :showRowLines="true"
          keyExpr="rowNO"
          :element-attr="dataGridAttri"
        >
          <DxRowDragging
            :show-drag-icons="false"
            group="tasksGroup"
            :on-drag-start="onDragStart"
            :on-drag-end="onDragEnd"
          />
          <DxSelection mode="multiple" showCheckBoxesMode="none" />
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
        </DxDataGrid>
      </div>
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
// import SortableWidget from "./SortableWidget";
// import SortableWidgetC from "./SortableWidget_c.vue";
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
    DxTextBox,
    // SortableWidget,
    // SortableWidgetC,
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
      testkeyevent: false,
      code_list: [
        "오프닝",
        "CM",
        "SB",
        "SM",
        "BGM",
        "LOGO",
        "M",
        "Filler",
        "CODE",
        "(빈칸)",
      ],
      view_channel_height: "650px",
      ab_channel_height: "820px",
      tttt: true,
      viewtitletext: "김이나의 별이 빛나는 밤에",
      abchannelrowNO: 0,
      viewtestNO: 9,
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
      activetab: "출력용",
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
      viewtable: [
        { code: "", name: "1부", startDuration: "", remarks: "", rowNO: 0 },
        {
          code: "SM",
          name: "오프닝",
          startDuration: "",
          remarks: "",
          rowNO: 1,
        },
        {
          code: "CM",
          name: "1부 CM",
          startDuration: "",
          remarks: "",
          rowNO: 2,
        },
        { code: "", name: "2부", startDuration: "", remarks: "", rowNO: 3 },
        {
          code: "CM",
          name: "2부 CM",
          startDuration: "",
          remarks: "",
          rowNO: 4,
        },
        { code: "", name: "3부", startDuration: "", remarks: "", rowNO: 5 },
        {
          code: "CM",
          name: "3부 CM",
          startDuration: "",
          remarks: "",
          rowNO: 6,
        },
        { code: "", name: "4부", startDuration: "", remarks: "", rowNO: 7 },
        {
          code: "CM",
          name: "4부 CM",
          startDuration: "",
          remarks: "",
          rowNO: 8,
        },
      ],
      channelAB: [],
      channelC_1: [],
      channelC_2: [],
      channelC_3: [],
      channelC_4: [],
      channelC_my: [],
      selectedItemKeys_view: [],
      selectedItemKeys_ab: [],
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
    iconselect(data) {
      if (data.data.transtype == "s") {
        data.component.cellValue(data.rowIndex, "transtype", "l");
      } else {
        if (data.data.transtype == "l") {
          data.component.cellValue(data.rowIndex, "transtype", "c");
        } else {
          data.component.cellValue(data.rowIndex, "transtype", "s");
        }
      }
      data.component.saveEditData();
    },
    downSpace() {
      this.testkeyevent = false;
    },
    clickSpace() {
      this.testkeyevent = true;
    },
    getSelectedRowElements(keys) {
      let arr = [],
        idx = null,
        el;
      keys.forEach((key) => {
        idx = this.dataGrid.getRowIndexByKey(key);
        el = this.dataGrid.getRowElement(idx)[0];
        arr.push(el);
      });
      return arr;
    },
    onDragStart(e) {
      var selectedRowKeys = e.component.getSelectedRowKeys();
      var selectedRowElements = this.getSelectedRowElements(selectedRowKeys);
      var numSelected = selectedRowKeys.length;
      e.component._selectedRowElements = selectedRowElements;
      var testIndex = [];

      selectedRowKeys.forEach((selectindex) => {
        var index = e.component.getRowIndexByKey(selectindex);
        testIndex.push(index);
      });
      if (!testIndex.includes(e.fromIndex)) {
        numSelected = 0;
        if (document.styleSheets[2].rules.length > 3)
          document.styleSheets[2].removeRule(3);
      }

      if (numSelected >= 2) {
        document.styleSheets[2].addRule(
          ".dx-sortable-clone.dx-sortable-dragging:before",
          'content: "' +
            numSelected +
            '"; background-color: green; color: white; padding: 2px 5px 2px 5px;'
        );
      }
    },
    onDragEnd() {
      if (document.styleSheets[2].rules.length > 3)
        document.styleSheets[2].removeRule(3);
    },
    //시작시간이 string 형태라서 변환하고 계산해야함
    calculateSalesAmount(rowData) {
      if (rowData.startDuration) {
        return rowData.startDuration;
      }
    },
    onFocusedRowChanged(e) {
      this.focusRowIndex = e.rowIndex;
    },
    onFocusedRowChanged2(e) {
      this.focusRowIndex2 = e.rowIndex;
      this.dataGrid.focus(this.dataGrid.getCellElement(3, e.columnIndex));
      //this.focusedRowKey = e.component.option("focusedRowKey");
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
    SelectionChanged(e) {
      this.activetab = e.addedItems[0].title;
    },
    c_eom() {
      this.eom = !this.eom;
    },
    c_som() {
      this.som = !this.som;
    },
    onValueChanged(value, cellInfo) {
      if (value.value == "(빈칸)") {
        cellInfo.data.code = "";
      } else {
        cellInfo.data.code = value.value;
      }
    },
    itemclick(e) {
      this.menuitem_id = e.itemData.id;
    },
    viewtableOnToolbarPreparing(e) {
      let toolbarItems = e.toolbarOptions.items;

      toolbarItems.push({
        location: "after",
        template: "totalGroupCount2",
      });
    },
    selectionDel_ab() {
      let a = this.channelAB;
      let b = this.selectedItemKeys_ab;
      for (let i = 0; i < b.length; i++) {
        for (let j = 0; j < a.length; j++) {
          if (b[i].abchannelrowNO == a[j].abchannelrowNO) {
            a.splice(j, 1);
            break;
          }
        }
        this.channelAB = a;
      }
    },
    selectionDel_view() {
      let a = this.viewtable;
      let b = this.selectedItemKeys_view;
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
    onSelectionChanged_ab({ selectedRowsData }) {
      this.selectedItemKeys_ab = selectedRowsData;
    },
    onSelectionChanged_view({ selectedRowsData }) {
      this.selectedItemKeys_view = selectedRowsData;
    },
    onAddChannelAB(e) {
      this.channelAB = [...this.channelAB];
      console.log(e.itemData);
      var selectedRowsData = e.fromComponent.getSelectedRowsData();
      if (selectedRowsData.length >= 2) {
        selectedRowsData.forEach((data, index) => {
          var row = [];
          row.push(data);
          row.transtype = "s";
          row.abchannelrowNO = this.abchannelrowNO;
          this.channelAB.splice(e.toIndex + index, 0, row);
          this.abchannelrowNO = this.abchannelrowNO + 1;
        });
      } else {
        var row = [];
        row.push(e.itemData);
        row.transtype = "s";
        row.abchannelrowNO = this.abchannelrowNO;
        if (e.fromData !== undefined) {
          this.channelAB.splice(e.toIndex, 0, fromData);
        } else {
          this.channelAB.splice(e.toIndex, 0, row);
          this.abchannelrowNO = this.abchannelrowNO + 1;
        }
      }
      e.fromComponent.clearSelection();
    },
    onAddViewChannel(e) {
      this.viewtable = [...this.viewtable];
      var selectedRowsData = e.fromComponent.getSelectedRowsData();
      if (selectedRowsData.length >= 2) {
        selectedRowsData.forEach((data, index) => {
          var row = [];
          row.code = data.categoryID;
          row.name = data.name;
          row.startDuration = data.duration;
          row.rowNO = this.viewtestNO;
          this.viewtable.splice(e.toIndex + index, 0, row);
          this.viewtestNO = this.viewtestNO + 1;
        });
      } else {
        var row = [];
        if (e.fromData !== undefined) {
          this.viewtable.splice(e.toIndex, 0, e.fromData);
        } else {
          //소재유형 알아내서 categoryID 바꾸기
          row.code = e.itemData.categoryID;
          row.name = e.itemData.name;
          row.startDuration = e.itemData.duration;
          row.rowNO = this.viewtestNO;
          this.viewtable.splice(e.toIndex, 0, row);
          this.viewtestNO = this.viewtestNO + 1;
        }
      }
      e.fromComponent.clearSelection();
    },
    onReorder(e) {
      this.channelAB = [...this.channelAB];
      this.channelAB.splice(e.fromIndex, 1);
      this.channelAB.splice(e.toIndex, 0, e.itemData);
    },
    onReorder2(e) {
      this.viewtable = [...this.viewtable];
      var selectedRowsData = e.component.getSelectedRowsData();
      var selectedRowsKey = this.dataGrid.getSelectedRowKeys();
      // selectedRowsKey.forEach((rowkey) => {
      //   console.log(
      //     "바뀐값 [" + rowkey + "] : " + e.component.getRowIndexByKey(rowkey)
      //   );
      // });
      var testIndex = [];
      selectedRowsKey.forEach((selectindex) => {
        var index = e.component.getRowIndexByKey(selectindex);
        testIndex.push(index);
      });
      if (!testIndex.includes(e.fromIndex)) {
        selectedRowsData = [];
      }
      if (selectedRowsData.length > 1) {
        var startindex = e.fromIndex;
        var newindex = e.toIndex;
        this.selectionDel_view();

        if (startindex > e.toIndex) {
          selectedRowsKey.forEach((selectindex) => {
            var index = e.component.getRowIndexByKey(selectindex);
            if (index < e.toIndex) {
              newindex = newindex - 1;
            }
          });
          selectedRowsData.forEach((obj, index) => {
            this.viewtable.splice(newindex + index, 0, obj);
          });
        } else {
          selectedRowsKey.forEach((selectindex) => {
            var index = e.component.getRowIndexByKey(selectindex);
            if (index < e.toIndex) {
              newindex = newindex - 1;
            }
          });
          newindex = newindex + 1;
          selectedRowsData.forEach((obj, index) => {
            this.viewtable.splice(newindex + index, 0, obj);
          });
        }
      } else {
        this.viewtable.splice(e.fromIndex, 1);
        this.viewtable.splice(e.toIndex, 0, e.itemData);
      }
      e.component.clearSelection();
    },
    itemfindOff() {
      document.querySelector(".schdiv").classList.remove("schdivon");
      this.schdivVal = true;
    },
    itemfindOn() {
      if (this.tttt) {
        document.querySelector(".schdiv").classList.add("schdivon");
        this.schdivVal = false;
        this.view_channel_height = "364px";
        this.ab_channel_height = "395px";
        this.tttt = !this.tttt;
      } else {
        document.querySelector(".schdiv").classList.remove("schdivon");
        this.schdivVal = true;
        this.view_channel_height = "650px";
        this.ab_channel_height = "820px";
        this.tttt = !this.tttt;
      }
    },
    onCellPrepared_view(e) {
      if (
        e.row.data.code === "" &&
        (e.row.data.name === "1부" ||
          e.row.data.name === "2부" ||
          e.row.data.name === "3부" ||
          e.row.data.name === "4부")
      ) {
        e.cellElement.style.backgroundColor = "#E0E0E0";
        //e.cellElement.style.color = "#C4C4C4";
      }
      if (e.row.data.code === "SB") {
        e.cellElement.style.backgroundColor = "#DCEDC8";
      }
      if (e.row.data.code === "CM") {
        e.cellElement.style.backgroundColor = "#B3E5FC";
      }
    },
    onCellPrepared_ab(e) {
      if (!e.row.data[0].categoryName)
        e.cellElement.style.backgroundColor = "#E0E0E0";
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
.dx-field-label {
  padding-left: 20px;
  width: 20%;
}
.dx-field-value-static,
.dx-field-value:not(.dx-switch):not(.dx-checkbox):not(.dx-button) {
  width: 80%;
}
.dx-field {
  font-family: "MBC 새로움 M";
}
.dx-datagrid-rowsview
  .dx-row-focused.dx-data-row
  .dx-command-edit:not(.dx-focused)
  .dx-link,
.dx-datagrid-rowsview .dx-row-focused.dx-data-row > td:not(.dx-focused),
.dx-datagrid-rowsview .dx-row-focused.dx-data-row > tr > td:not(.dx-focused) {
  background-color: #5c95c5 !important;
}
#c1tabbtn {
  background-color: #e0e0e0;
  width: 20px;
  height: 20px;
  border-radius: 50%;
  margin-bottom: 1px;
}
#c1tabbtn i {
  font-size: 12px;
  color: #6c757d;
}
#c1tabbtn .dx-button-content {
  padding: 0px;
}
.opentexttitle {
  position: absolute;
  top: 250px;
  left: 25px;
  font-size: 12px;
  letter-spacing: 2px;
  writing-mode: vertical-rl;
  text-orientation: mixed;
  width: 10px;
  height: 250px;
}
.opentext {
  position: absolute;
  top: 250px;
  font-size: 12px;
  left: 7px;
  letter-spacing: 2px;
  writing-mode: vertical-rl;
  text-orientation: mixed;
  width: 10px;
  height: 200px;
}
.iconDivClose {
  position: fixed;
  left: 133px;
  z-index: 4;
  display: grid;
  grid-template-columns: 1fr;
}
#subjectMenu .dx-item-content {
  line-height: 3;
  height: 50px;
}
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
  width: 130px;
}
.wavemodalDiv {
  margin: auto;
  width: 500px;
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
#channelAB_id {
  padding: 5px;
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
  padding: 5px;
}

/* 드래그 안고장나게해주는 CSS */
/* #app-container {
  position: fixed;
} */
:root {
  --ab-channel-height: 395px;
  --c-channel-height: 364px;
  --tab-height: 375px;
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
  position: absolute;
  bottom: -1155px;
  z-index: 6;
  background-color: white;
  right: 25px;
  left: 180px;
  height: 90%;
  transition-duration: 1s;
  box-shadow: 0 3px 30px rgb(0 0 0 / 10%), 0 3px 20px rgb(0 0 0 / 10%);
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
  transition-duration: 1s;
  transform: translate(0px, -700px);
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
  border: solid 0.5px #ddd;
  grid-template-columns: 1fr 7fr;
  border-radius: 2px;
  position: absolute;
  width: 500px;
  height: 400px;
  padding: 10px 10px 10px 10px;
  margin: 0px 0px 0px 15px;
}

#schbtn2 {
  position: absolute;
  right: 25px;
  width: 100px;
  bottom: 25px;
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
  width: 1180px;
  margin: 10px 10px 0px 20px;
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
.subtitle {
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
  height: 35px;
  padding: 0px 0px 0px 0px;
}

#autosavebtn {
  float: right;
}

.autosavesapn {
  position: absolute;
  right: 10px;
  top: 100px;
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
  margin-right: 10px;
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