<template>
  <div>
    <BasicTable
      :data-source="dataSource"
      :fields="fields"
      :is-loading="isLoading"
      :config-actions="['delete']"
      :filter-fileds="searchField"
      :placeholder-text="placeholderText"
      @deleteOk="onDeleteOk"
    />
  </div>
</template>
<script>
import BasicTable from "../widget/table_basic.vue";
import "moment/locale/ko";
const moment = require("moment");
export default {
  data() {
    return {
      selectParm: {
        startDate: new Date(2000, 5, 4),
        endDate: new Date(2023, 5, 4),
        name: "난타음악",
      },
      dataSource: [],
      isLoading: false,
      placeholderText: "이름, ID, 역할(인증 그룹)을 입렵하세요.",
      filter: "",
      searchField: [],
      fields: [
        { key: "selected", label: "__select", thStyle: { width: "20px" } },
        {
          key: "no",
          label: "순서",
          sortable: false,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "personid",
          label: "ID",
          sortable: true,
          sortDirection: "desc",
          thClass: "text-center",
          tdClass: "text-center",
        },
        // {
        //   key: "passwd",
        //   label: "비밀번호",
        //   sortable: true,
        //   thClass: "text-center",
        //   tdClass: "text-center",
        // },
        {
          key: "personname",
          label: "이름",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "devision",
          label: "소속국실",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "departmant",
          label: "소속부서",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "indate",
          label: "등록일자",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
          thStyle: { width: "250px" },
          formatter: (value, key, item) => {
            return moment(value).format("YYYY-MM-DD");
          },
        },
        {
          key: "tel1",
          label: "사내 전화번호",
          sortable: true,
          thClass: "text-center",
          tdClass: "text-center",
        },
        // {
        //   key: "tel2",
        //   label: "개인 전화번호",
        //   sortable: true,
        //   thClass: "text-center",
        //   tdClass: "text-center",
        // },
        // {
        //   key: "emailid",
        //   label: "Email",
        //   thClass: "text-center",
        //   tdClass: "text-center",
        // },
        // {
        //   key: "rank",
        //   label: "직위",
        //   thClass: "text-center",
        //   tdClass: "text-center",
        // },
        {
          key: "role",
          label: "역할(인증 그룹)",

          thClass: "text-center",
          tdClass: "text-center",
        },
        {
          key: "actions",
          label: "작업",
          thClass: "text-center",
          tdClass: "text-center",
          thStyle: { width: "200px" },
        },
      ],
      get_data_url: "/api/managementsystem/getDelaudiolist",
      delete_url: "/api/managementsystem/deleteUser?personid=",
    };
  },
  components: { BasicTable },
  created() {
    this.getData();
  },
  methods: {
    getData() {
      this.isLoading = true;
      this.$http.post(this.get_data_url, this.selectParm).then((res) => {
        console.log(res);
        if (res.status === 200) {
          this.dataSource = res.data.resultObject;
        } else {
          this.$fn.notify("server-error", { message: "조회 에러" });
        }
        this.isLoading = false;
      });
    },
    inDateSet(date, format) {
      return moment(date).format(format);
    },
    onDeleteOk(rowData) {
      if (rowData.personid) {
        this.deleteData(rowData.personid);
      }
    },
    deleteData(role) {
      if (role) {
        this.$http.delete(this.delete_url + role).then((res) => {
          if (res.status === 200 && res.data.resultObject) {
            this.getData();
          } else {
            this.$fn.notify("server-error", { message: "추가 에러" });
          }
        });
      }
    },
  },
};
</script>
