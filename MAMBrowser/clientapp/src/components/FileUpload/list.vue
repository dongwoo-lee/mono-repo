<template>
  <div
    style="
      width: 1300px;
      margin-left: auto;
      margin-right: auto;
      font-size: 14px;
    "
  >
    <div
      v-show="this.role == 'ADMIN'"
      style="width: 1300px; margin-top: -20px; height: 30px; font-size: 12px"
    >
      <vue-step-progress-indicator
        :steps="['대기중', '디코딩', '리샘플링', '노말라이즈', '스토리지 저장']"
        :active-step="4"
        :is-reactive="false"
        :styles="styleData"
        :colors="colorData"
        style="margin-left: 850px; width: 670px"
      />
    </div>
    <DxDataGrid
      v-show="this.role == 'ADMIN'"
      style="
        height: 450px;
        border: 1px solid silver;
        font-family: 'MBC 새로움 M';
      "
      :data-source="masteringListData"
      :show-borders="false"
      :hover-state-enabled="true"
      key-expr="title"
      :allow-column-resizing="true"
      :column-auto-width="true"
      no-data-text="No Data"
      data-row-template="dataRowTemplate"
    >
      <DxColumn data-field="title" caption="제목" />
      <DxColumn data-field="type" caption="소재유형" />
      <DxColumn data-field="user_id" caption="등록자" />
      <DxColumn data-field="date" caption="등록일시" />
      <DxColumn data-field="step" caption="상태" alignment="left" />
      <template #dataRowTemplate="{ data: rowInfo }">
        <tr>
          <td style="border-right: 1px solid #dddddd">
            {{ rowInfo.data.title }}
          </td>
          <td style="border-right: 1px solid #dddddd">
            {{ rowInfo.data.type }}
          </td>
          <td style="border-right: 1px solid #dddddd">
            {{ rowInfo.data.user_id }}
          </td>
          <td style="border-right: 1px solid #dddddd">
            {{ rowInfo.data.date }}
          </td>
          <td v-if="rowInfo.data.step == 0">
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  padding-left: 4px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 5px;
                  float: left;
                "
              >
                {{ rowInfo.data.step + 1 }}
              </div>
              <span style="color: #27ae60"> 대기중 </span>
            </span>
          </td>
          <td v-if="rowInfo.data.step == 1">
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  padding-left: 4px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 5px;
                  float: left;
                "
              >
                {{ rowInfo.data.step + 1 }}
              </div>
              <span style="color: #27ae60">
                디코딩
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </td>
          <td v-if="rowInfo.data.step == 2">
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  padding-left: 4px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 5px;
                  float: left;
                "
              >
                {{ rowInfo.data.step + 1 }}
              </div>
              <span style="color: #27ae60">
                리샘플링
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </td>
          <td v-if="rowInfo.data.step == 3">
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  padding-left: 4px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 5px;
                  float: left;
                "
              >
                {{ rowInfo.data.step + 1 }}
              </div>
              <span style="color: #27ae60">
                노말라이즈
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </td>
          <td v-if="rowInfo.data.step == 4">
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  padding-left: 4px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 5px;
                  float: left;
                "
              >
                {{ rowInfo.data.step + 1 }}
              </div>
              <span style="color: #27ae60">
                스토리지 저장
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </td>
        </tr>
      </template>
      <DxPager :visible="false" />
      <DxScrolling mode="standard" />
    </DxDataGrid>
    <div
      v-show="this.role != 'ADMIN'"
      style="width: 1300px; margin-top: -20px; height: 30px; font-size: 12px"
    >
      <vue-step-progress-indicator
        :steps="['대기중', '디코딩', '리샘플링', '노말라이즈', '스토리지 저장']"
        :active-step="4"
        :is-reactive="false"
        :styles="styleData"
        :colors="colorData"
        style="margin-left: 850px; width: 670px"
      />
    </div>
    <DxDataGrid
      v-show="this.role != 'ADMIN'"
      style="
        height: 450px;
        border: 1px solid silver;
        font-family: 'MBC 새로움 M';
      "
      :data-source="masteringListData"
      :show-borders="false"
      :hover-state-enabled="true"
      key-expr="title"
      :allow-column-resizing="true"
      :column-auto-width="true"
      no-data-text="No Data"
      data-row-template="dataRowTemplate"
    >
      <DxColumn data-field="title" caption="제목" />
      <DxColumn data-field="type" caption="소재유형" />
      <DxColumn data-field="date" caption="등록일시" />
      <DxColumn data-field="step" caption="상태" alignment="left" />
      <template #dataRowTemplate="{ data: rowInfo }">
        <tr>
          <td style="border-right: 1px solid #dddddd">
            {{ rowInfo.data.title }}
          </td>
          <td style="border-right: 1px solid #dddddd">
            {{ rowInfo.data.type }}
          </td>
          <td style="border-right: 1px solid #dddddd">
            {{ rowInfo.data.date }}
          </td>
          <td v-if="rowInfo.data.step == 0">
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  padding-left: 4px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 5px;
                  float: left;
                "
              >
                {{ rowInfo.data.step + 1 }}
              </div>
              <span style="color: #27ae60"> 대기중 </span>
            </span>
          </td>
          <td v-if="rowInfo.data.step == 1">
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  padding-left: 4px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 5px;
                  float: left;
                "
              >
                {{ rowInfo.data.step + 1 }}
              </div>
              <span style="color: #27ae60">
                디코딩
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </td>
          <td v-if="rowInfo.data.step == 2">
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  padding-left: 4px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 5px;
                  float: left;
                "
              >
                {{ rowInfo.data.step + 1 }}
              </div>
              <span style="color: #27ae60">
                리샘플링
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </td>
          <td v-if="rowInfo.data.step == 3">
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  padding-left: 4px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 5px;
                  float: left;
                "
              >
                {{ rowInfo.data.step + 1 }}
              </div>
              <span style="color: #27ae60">
                노말라이즈
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </td>
          <td v-if="rowInfo.data.step == 4">
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  padding-left: 4px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 5px;
                  float: left;
                "
              >
                {{ rowInfo.data.step + 1 }}
              </div>
              <span style="color: #27ae60">
                스토리지 저장
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </td>
        </tr>
      </template>
      <DxPager :visible="false" />
      <DxScrolling mode="standard" />
    </DxDataGrid>
  </div>
</template>

<script>
import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
import Vuetable from "vuetable-2/src/components/Vuetable";
import VueStepProgressIndicator from "vue-step-progress-indicator";
import {
  DxScrolling,
  DxLoadPanel,
  DxPager,
  DxDataGrid,
  DxColumn,
} from "devextreme-vue/data-grid";
import {} from "devextreme-vue/data-grid";
export default {
  components: {
    Vuetable,
    VueStepProgressIndicator,
    DxScrolling,
    DxLoadPanel,
    DxPager,
    DxDataGrid,
    DxColumn,
  },
  data() {
    return {
      styleData: {
        progress__block: {
          display: "flex",
          alignItems: "center",
        },
        progress__bridge: {
          backgroundColor: "grey",
          height: "2px",
          flexGrow: "1",
          width: "20px",
        },
        progress__bubble: {
          margin: "0",
          padding: "0",
          lineHeight: "10px",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          height: "18px",
          width: "18px",
          borderRadius: "10%",
          backgroundColor: "transparent",
          border: "2px grey solid",
          textAlign: "center",
        },
        progress__label: {
          margin: "0 0.8rem",
          font: "14px;",
        },
      },
      colorData: {
        progress__bubble: {
          active: {
            color: "#fff",
            backgroundColor: "#27ae60",
            borderColor: "#27ae60",
          },
          inactive: {
            color: "#fff",
            backgroundColor: "#EF5350",
            borderColor: "#EF5350",
          },
          completed: {
            color: "#fff",
            borderColor: "#27ae60",
            backgroundColor: "#27ae60",
          },
        },
        progress__label: {
          active: {
            color: "#27ae60",
          },
          inactive: {
            color: "#EF5350",
          },
          completed: {
            color: "#27ae60",
          },
        },
      },
      role: "",
      listTableHeight: "400px",
      userListFields: [
        {
          name: "__slot:title",
          title: "제목",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%",
        },
        {
          name: "__slot:type",
          title: "소재유형",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "4%",
        },
        {
          name: "__slot:date",
          title: "등록일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
        },
        {
          name: "__slot:step",
          title: "상태",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "4%",
        },
      ],
      adminListFields: [
        {
          name: "__slot:title",
          title: "제목",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "12%",
        },
        {
          name: "__slot:type",
          title: "소재유형",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "4%",
        },
        {
          name: "__slot:user_id",
          title: "등록자",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "4%",
        },
        {
          name: "__slot:date",
          title: "등록일시",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "8%",
        },
        {
          name: "__slot:step",
          title: "상태",
          titleClass: "center aligned text-center",
          dataClass: "center aligned text-center",
          width: "4%",
        },
      ],
    };
  },
  computed: {
    ...mapState("FileIndexStore", {
      masteringListData: (state) => state.masteringListData,
    }),
  },
  created() {
    this.role = sessionStorage.getItem("authority");
    var user_id = sessionStorage.getItem("user_id");
    this.startDBConnection(user_id);
  },
  beforeDestroy() {
    this.stopDBConnection();
  },
  methods: {
    ...mapMutations("FileIndexStore", [
      "startDBConnection",
      "stopDBConnection",
    ]),

    getCategory(v) {
      switch (v) {
        case "MY":
          return "MY 디스크";
        case "AC":
          return "프로소재";
        case "PM":
          return "프로그램";
        case "MS":
          return "주조SPOT";
        case "ST":
          return "부조SPOT";
        case "FC":
          return "FILLER";
        case "RC":
          return "취재물";
        case "TT":
          return "고정소재";
        case "TS":
          return "변동소재";
        default:
          return "";
      }
    },
  },
};
</script>

<style>
.list-fade-enter-active {
  transition: all 0.3s ease;
}
.list-fade-leave-active {
  transition: all 0.3s cubic-bezier(1, 0.5, 0.8, 1);
}
.list-fade-enter {
  transform: translateX(10px);
  opacity: 0;
}
.list-fade-leave-to {
  transform: translateX(-10px);
  opacity: 40%;
}
</style>
