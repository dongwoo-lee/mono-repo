<template>
  <div
    style="
      width: 1300px;
      margin-left: auto;
      margin-right: auto;
      font-size: 14px;
    "
  >
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
            style="margin-top: 7px; height: 25px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 18px;
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
                <b-spinner
                  small
                  type="grow"
                  v-if="props.rowIndex == 0"
                ></b-spinner>
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 1"
            style="margin-top: 7px; height: 25px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 18px;
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
                <b-spinner
                  small
                  type="grow"
                  v-if="props.rowIndex == 0"
                ></b-spinner>
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 2"
            style="margin-top: 7px; height: 25px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 18px;
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
                <b-spinner
                  small
                  type="grow"
                  v-if="props.rowIndex == 0"
                ></b-spinner>
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 3"
            style="margin-top: 7px; height: 25px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 18px;
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
                <b-spinner
                  small
                  type="grow"
                  v-if="props.rowIndex == 0"
                ></b-spinner>
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 4"
            style="margin-top: 7px; height: 25px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 18px;
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
                <b-spinner
                  small
                  type="grow"
                  v-if="props.rowIndex == 0"
                ></b-spinner>
              </span>
            </span>
          </div>
        </transition>
      </template>
    </vuetable>
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
            style="margin-top: 7px; height: 25px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 18px;
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
                <b-spinner
                  small
                  type="grow"
                  v-if="props.rowIndex == 0"
                ></b-spinner>
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 1"
            style="margin-top: 7px; height: 25px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 18px;
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
                <b-spinner
                  small
                  type="grow"
                  v-if="props.rowIndex == 0"
                ></b-spinner>
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 2"
            style="margin-top: 7px; height: 25px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 18px;
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
                <b-spinner
                  small
                  type="grow"
                  v-if="props.rowIndex == 0"
                ></b-spinner>
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 3"
            style="margin-top: 7px; height: 25px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 18px;
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
                <b-spinner
                  small
                  type="grow"
                  v-if="props.rowIndex == 0"
                ></b-spinner>
              </span>
            </span>
          </div>
        </transition>

        <transition name="list-fade">
          <div
            v-if="props.rowData.step == 4"
            style="margin-top: 7px; height: 25px"
          >
            <span>
              <div
                style="
                  width: 18px;
                  height: 18px;
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
                <b-spinner
                  small
                  type="grow"
                  v-if="props.rowIndex == 0"
                ></b-spinner>
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
export default {
  components: {
    Vuetable,
  },
  data() {
    return {
      role: "",
      listTableHeight: "530px",
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
