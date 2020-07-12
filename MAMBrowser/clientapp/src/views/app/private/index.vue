<template>
  <div>
    <b-row>
      <b-colxx xxs="12">
        <piaf-breadcrumb :heading="$t('menu.private')"/>
        <div class="separator mb-5"></div>
      </b-colxx>
    </b-row>
    <b-row>
      <b-colxx xxs="12">
        <!-- 검색 -->
        <b-card class="mb-4">
          <b-form @submit.prevent="onGridFormSubmit">
            <b-row>
              <b-colxx sm="2">
                <b-form-group label="채널">
                  <c-input-text type="text" v-model="gridForm.email"></c-input-text>
                </b-form-group>
              </b-colxx>
              <b-colxx sm="2">
                <b-form-group label="소재명">
                  <b-form-select v-model="gridForm.state" :options="stateOptions" plain />
                </b-form-group>
              </b-colxx>
              <b-colxx sm="4">
                <b-form-group label="날짜" class="inline-block">
                  <div class="d-flex">
                    <b-input-group class="w-50 mr-2">
                      <b-form-input
                        v-model="startDate"
                        type="text"
                        placeholder="YYYY-MM-DD"
                        pattern="\d\d\d\d-(0[1-9]|1[1-2])-(0[1-9]|[1-2][0-9]|3[0-1])"
                      ></b-form-input>
                      <b-input-group-append>
                        <b-form-datepicker
                          v-model="startDate"
                          button-only
                          left
                          aria-controls="example-input"
                          button-variant="secondary default"
                          label-next-month="다음달"
                          today-variant
                          :hide-header="true"
                          size="sm"
                        ></b-form-datepicker>
                      </b-input-group-append>
                    </b-input-group>
                    <b-input-group class="w-50 mr-2">
                      <b-form-input
                        v-model="endDate"
                        type="text"
                        placeholder="YYYY-MM-DD"
                        autocomplete="off"
                      ></b-form-input>
                      <b-input-group-append>
                        <b-form-datepicker
                          v-model="endDate"
                          button-only
                          left
                          aria-controls="example-input"
                          button-variant="secondary default"
                          label-next-month="다음달"
                          today-variant
                          :hide-header="true"
                          size="sm"
                        ></b-form-datepicker>
                      </b-input-group-append>
                    </b-input-group>
                  </div>
                </b-form-group>
              </b-colxx>
              <b-colxx sm="2">
                <b-form-group label="상태">
                  <b-form-select v-model="gridForm.state" :options="stateOptions" plain />
                </b-form-group>
              </b-colxx>
              <b-colxx sm="2">
                <b-form-group label="제작자">
                  <b-form-input type="text" v-model="gridForm.email" />
                </b-form-group>
              </b-colxx>
              <b-colxx sm="3">
                <b-form-group label="추가 검색">
                  <b-form-checkbox-group v-model="selected">
                    <b-form-checkbox value="orange">히트곡</b-form-checkbox>
                    <b-form-checkbox value="apple">금지곡</b-form-checkbox>
                    <b-form-checkbox value="pineapple">주의</b-form-checkbox>
                    <b-form-checkbox value="grape">청소년유해</b-form-checkbox>
                    <b-form-checkbox value="banana">뮤직비디오</b-form-checkbox>
                  </b-form-checkbox-group>
                </b-form-group>
              </b-colxx>
            </b-row>
          </b-form>
        </b-card>
        <!-- Scroll 데이터 테이블 -->
        <b-card class="mb-4">
          <b-form class="mb-3" inline>
            <b-input-group class="mr-2">
              <b-button class="mb-1" variant="primary default" size="sm">파일 업로드</b-button>
            </b-input-group>
            <b-input-group class="mr-2">
              <b-button class="mb-1" variant="danger default" size="sm">휴지통비우기</b-button>
            </b-input-group>
            <b-input-group class="mr-2">
              <b-button class="mb-1" variant="secondary default" size="sm">복원</b-button>
            </b-input-group>
          </b-form>
          <vuetable
            table-height="420px"
            ref="vuetable"
            :api-url="apiBase"
            class="order-with-arrow"
            :query-params="makeQueryParams"
            :per-page="perPage"
            :reactive-api-url="true"
            :fields="fields"
            pagination-path
            :row-class="onRowClass"
            @vuetable:row-clicked="rowClicked"
            @vuetable:cell-rightclicked="rightClicked"
          >
            <!-- <template slot="checkbox" slot-scope="props">
              <b-form-checkbox
                :checked="selectedItems.includes(props.rowData.id)"
                class="itemCheck mb-0"
              ></b-form-checkbox>
            </template> -->
          </vuetable>
        </b-card>
      </b-colxx>
    </b-row>

    <!-- 컨텍스트 메뉴 -->
    <v-contextmenu ref="contextmenu">
      <v-contextmenu-item @click="onContextMenuAction('copy')">
        <i class="simple-icon-docs" />
        <span>Copy</span>
      </v-contextmenu-item>
      <v-contextmenu-item @click="onContextMenuAction('move-to-archive')">
        <i class="simple-icon-drawer" />
        <span>Move to archive</span>
      </v-contextmenu-item>
      <v-contextmenu-item @click="onContextMenuAction('delete')">
        <i class="simple-icon-trash" />
        <span>Delete</span>
      </v-contextmenu-item>
    </v-contextmenu>

  </div>
</template>

<script>
import Vuetable from "vuetable-2/src/components/Vuetable";
import { apiUrl } from "../../../constants/config";
import CInputText from '../../../components/Input/CInputText';
import DataTableScrollPaging from '../../../components/DataTable/DataTableScrollPaging';

export default {
  components: { Vuetable, CInputText, DataTableScrollPaging },
  data() {
    return {
      apiBase: apiUrl + "/cakes/fordatatable",
      page: 1,
      perPage: 12,
      startDate: '',
      endDate: '',
      selected: [],
      stateOptions: [
        { value: null, text: '선택해주세요.' },
        { value: 'option1', text: 'option1' },
        { value: 'option2', text: 'option2' },
        { value: 'option3', text: 'option3' },
      ],
      gridForm: {
        email: "",
        password: "",
        address1: "",
        address2: "",
        city: "",
        state: "",
        zip: ""
      },
      fields: [
        {
          name: "__checkbox",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "5%"
        },
        {
          name: "title",
          title: "파일명",
          titleClass: "",
          dataClass: "list-item-heading",
          width: "10%"
        },
        {
          name: "sales",
          title: "메모",
          titleClass: "",
          dataClass: "text-muted",
        },
        {
          name: "stock",
          title: "파일형식",
          titleClass: "",
          dataClass: "text-muted",
          width: "10%"
        },
        {
          name: "category",
          title: "상세정보",
          titleClass: "",
          dataClass: "text-muted",
          width: "25%"
        },
        {
          name: "writeDate",
          title: "등록일시",
          titleClass: "",
          dataClass: "text-muted",
          width: "5%"
        },
      ]
    }
  },
  methods: {
    onGridFormSubmit() {
      console.log(JSON.stringify(this.gridForm));
    },
    makeQueryParams(sortOrder, currentPage, perPage) {
      this.selectedItems = [];
      return sortOrder[0]
        ? {
            sort: sortOrder[0]
              ? sortOrder[0].field + "|" + sortOrder[0].direction
              : "",
            page: currentPage,
            per_page: this.perPage,
            search: this.search
          }
        : {
            page: currentPage,
            per_page: this.perPage,
            search: this.search
          };
    },
    onRowClass(dataItem, index) {
      if (this.selectedItems.includes(dataItem.id)) {
        return "selected";
      }
      return "";
    },
    rowClicked(dataItem, event) {
      const itemId = dataItem.id;
      if (event.shiftKey && this.selectedItems.length > 0) {
        let itemsForToggle = this.items;
        var start = this.getIndex(itemId, itemsForToggle, "id");
        var end = this.getIndex(
          this.selectedItems[this.selectedItems.length - 1],
          itemsForToggle,
          "id"
        );
        itemsForToggle = itemsForToggle.slice(
          Math.min(start, end),
          Math.max(start, end) + 1
        );
        this.selectedItems.push(
          ...itemsForToggle.map(item => {
            return item.id;
          })
        );
        this.selectedItems = [...new Set(this.selectedItems)];
      } else {
        if (this.selectedItems.includes(itemId)) {
          this.selectedItems = this.selectedItems.filter(x => x !== itemId);
        } else this.selectedItems.push(itemId);
      }
    },
    rightClicked(dataItem, field, event) {
      event.preventDefault();
      if (!this.selectedItems.includes(dataItem.id)) {
        this.selectedItems = [dataItem.id];
      }
      this.$refs.contextmenu.show({ top: event.pageY, left: event.pageX });
    },
  }
}
</script>