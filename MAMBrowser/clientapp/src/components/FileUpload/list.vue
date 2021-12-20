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
    <vuetable
      v-show="this.role == 'ADMIN'"
      :table-height="listTableHeight"
      ref="vuetable-scrollable"
      :api-mode="false"
      :fields="adminListFields"
      :data="masteringListData"
      no-data-template="데이터가 없습니다."
    >
      <template slot="title" scope="props">
        <div style="font-size: 14px">
          {{ props.rowData.title }}
        </div>
      </template>
      <template slot="type" scope="props">
        <div>
          {{ props.rowData.type }}
        </div>
      </template>
      <template slot="user_id" scope="props">
        <div>
          {{ props.rowData.user_id }}
        </div>
      </template>
      <template slot="date" scope="props">
        <div>
          {{ props.rowData.date }}
        </div>
      </template>
      <template slot="step" scope="props">
        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 0"
            style="margin-top: 0px; height: 20px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 0px;
                "
              >
                {{ props.rowData.step + 1 }}
              </div>
              <span
                style="
                  color: #27ae60;
                  position: relative;
                  top: -16px;
                  left: -10px;
                "
              >
                대기중
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 1"
            style="margin-top: 0px; height: 20px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 0px;
                "
              >
                {{ props.rowData.step + 1 }}
              </div>
              <span
                style="
                  color: #27ae60;
                  position: relative;
                  top: -16px;
                  left: -10px;
                "
              >
                디코딩
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 2"
            style="margin-top: 0px; height: 20px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 0px;
                "
              >
                {{ props.rowData.step + 1 }}
              </div>
              <span
                style="
                  color: #27ae60;
                  position: relative;
                  top: -16px;
                  left: -5px;
                "
              >
                리샘플링
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 3"
            style="margin-top: 0px; height: 20px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 0px;
                "
              >
                {{ props.rowData.step + 1 }}
              </div>
              <span
                style="
                  color: #27ae60;
                  position: relative;
                  top: -16px;
                  left: 3px;
                "
              >
                노말라이즈
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 4"
            style="margin-top: 0px; height: 20px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 0px;
                "
              >
                {{ props.rowData.step + 1 }}
              </div>
              <span
                style="
                  color: #27ae60;
                  position: relative;
                  top: -16px;
                  left: 12px;
                "
              >
                스토리지 저장
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </div>
        </transition>
      </template>
    </vuetable>
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
    <vuetable
      v-show="this.role != 'ADMIN'"
      :table-height="listTableHeight"
      ref="vuetable-scrollable"
      :api-mode="false"
      :fields="userListFields"
      :data="masteringListData"
      no-data-template="데이터가 없습니다."
    >
      <template slot="title" scope="props">
        <div style="font-size: 14px">
          {{ props.rowData.title }}
        </div>
      </template>
      <template slot="type" scope="props">
        <div>
          {{ props.rowData.type }}
        </div>
      </template>
      <template slot="date" scope="props">
        <div>
          {{ props.rowData.date }}
        </div>
      </template>
      <template slot="step" scope="props">
        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 0"
            style="margin-top: 0px; height: 20px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 0px;
                "
              >
                {{ props.rowData.step + 1 }}
              </div>
              <span
                style="
                  color: #27ae60;
                  position: relative;
                  top: -16px;
                  left: -10px;
                "
              >
                대기중
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 1"
            style="margin-top: 0px; height: 20px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 0px;
                "
              >
                {{ props.rowData.step + 1 }}
              </div>
              <span
                style="
                  color: #27ae60;
                  position: relative;
                  top: -16px;
                  left: -10px;
                "
              >
                디코딩
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 2"
            style="margin-top: 0px; height: 20px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 0px;
                "
              >
                {{ props.rowData.step + 1 }}
              </div>
              <span
                style="
                  color: #27ae60;
                  position: relative;
                  top: -16px;
                  left: -5px;
                "
              >
                리샘플링
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 3"
            style="margin-top: 0px; height: 20px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 0px;
                "
              >
                {{ props.rowData.step + 1 }}
              </div>
              <span
                style="
                  color: #27ae60;
                  position: relative;
                  top: -16px;
                  left: 3px;
                "
              >
                노말라이즈
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 4"
            style="margin-top: 0px; height: 20px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 20px;
                  padding-top: 2px;
                  border-radius: 2px;
                  background-color: #27ae60;
                  color: white;
                  margin-right: 0px;
                "
              >
                {{ props.rowData.step + 1 }}
              </div>
              <span
                style="
                  color: #27ae60;
                  position: relative;
                  top: -16px;
                  left: 12px;
                "
              >
                스토리지 저장
                <b-spinner small type="grow"></b-spinner>
              </span>
            </span>
          </div>
        </transition>
      </template>
    </vuetable>
  </div>
</template>

<script>
import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
import Vuetable from "vuetable-2/src/components/Vuetable";
import VueStepProgressIndicator from "vue-step-progress-indicator";
export default {
  components: {
    Vuetable,
    VueStepProgressIndicator,
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
          return "My 디스크";
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
