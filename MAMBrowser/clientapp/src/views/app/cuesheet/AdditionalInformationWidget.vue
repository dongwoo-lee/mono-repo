<template>
  <div id="mainDiv">
    <div>
      <Attachments />
    </div>
    <div>태그</div>
    <div>메모</div>
  </div>
</template>

<script>
import Attachments from "../../../../src/components/FileUpload/Attachments/Upload.vue";
export default {
  data() {
    return {};
  },
  mounted() {},
  created() {},
  components: {
    Attachments,
  },
  computed: {},
  methods: {},
};
</script>

<style>
#mainDiv {
  height: 580px;
}
</style>