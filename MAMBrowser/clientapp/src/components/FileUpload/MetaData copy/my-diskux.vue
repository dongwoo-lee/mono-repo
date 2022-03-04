<template>
  <div v-show="this.MetaData.typeSelected == 'my-disk'">
    <div style="height: 50px; margin-top: 25px">
      <b-form-group
        label="제목"
        class="has-float-label"
        style="font-size: 15px"
      >
        <b-form-input
          class="editTask"
          v-model="MetaData.title"
          :state="titleState"
          :maxLength="200"
          placeholder="제목"
          trim
        />
      </b-form-group>
      <p
        v-show="titleState"
        style="
          position: relative;
          left: 310px;
          top: -15px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.title.length }}/200
      </p>
    </div>

    <div style="margin-left: 390px; height: 50px; margin-top: 20px">
      <b-form-group
        label="메모"
        class="has-float-label"
        style="font-size: 15px"
      >
        <b-form-input
          class="editTask"
          v-model="MetaData.memo"
          :state="memoState"
          :maxLength="200"
          placeholder="메모"
          trim
        />
      </b-form-group>
      <p
        v-show="memoState"
        style="
          position: relative;
          left: 310px;
          top: -15px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.memo.length }}/200
      </p>
    </div>
  </div>
</template>

<script>
import CommonFileFunction from "../CommonFileFunction";
export default {
  components: {},
  mixins: [CommonFileFunction],
  data() {
    return {};
  },
  created() {
    this.reset();
  },
  methods: {},
};
</script>

<style></style>
