<template>
  <div>
    <b-col lg="4" class="my-1">
      <b-form-group>
        <b-button variant="outline-primary default" @click=""
          >자동 삭제 규칙</b-button
        >
      </b-form-group>
    </b-col>
    <BasicTable />
  </div>
</template>
<script>
import BasicTable from "../widget/table_basic.vue";
export default {
  data() {
    return {
      get_data_url: "",
      fields: [], // field 정보 리스트
      filter: "",
      filterFileds: [], // 검색 원하는 값
    };
  },
  components: { BasicTable },
  created() {},
  methods: {},
};
</script>
