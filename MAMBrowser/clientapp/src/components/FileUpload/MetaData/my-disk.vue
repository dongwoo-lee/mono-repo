<template>
  <div v-show="this.MetaData.typeSelected == 'my-disk'">
    <div style="height: 50px; margin-top: 10px">
      <b-form-input
        class="editTask"
        v-model="MetaData.title"
        :state="titleState"
        :maxLength="200"
        placeholder="제목"
        trim
      />

      <button
        v-show="titleState"
        style="
          position: relative;
          left: 315px;
          top: -27px;
          z-index: 99;
          width: 3px;
          heigth: 3px;
          background-color: #ffffff;
          border: 0;
          outline: 0;
        "
      >
        <b-icon
          icon="x-circle"
          font-scale="1"
          style="position: relative; top: 0px; right: 0px; z-index: 999"
          variant="secondary"
          @click="resetTitle"
        ></b-icon>
      </button>
      <p
        v-show="titleState"
        style="
          position: relative;
          left: 310px;
          top: -20px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.title.length }}/200
      </p>
    </div>

    <div style="height: 50px">
      <b-form-input
        class="editTask"
        v-model="MetaData.memo"
        :state="memoState"
        :maxLength="200"
        placeholder="메모"
        trim
      />

      <button
        v-show="memoState"
        style="
          position: relative;
          left: 315px;
          top: -27px;
          z-index: 99;
          width: 3px;
          heigth: 3px;
          background-color: #ffffff;
          border: 0;
          outline: 0;
        "
      >
        <b-icon
          icon="x-circle"
          font-scale="1"
          style="position: relative; top: 0px; right: 0px; z-index: 999"
          variant="secondary"
          @click="resetMemo"
        ></b-icon>
      </button>
      <p
        v-show="memoState"
        style="
          position: relative;
          left: 310px;
          top: -20px;
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
