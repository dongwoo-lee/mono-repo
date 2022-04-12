<template>
  <div v-show="this.MetaData.typeSelected == 'my-disk'">
    <div style="height: 50px; margin-top: 27px">
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
          left: 380px;
          top: -15px;
          z-index: 9999;
          width: 30px;
        "
      >
        {{ MetaData.title.length }}/200
      </p>
    </div>

    <div style="height: 50px; margin-top: 17px">
      <b-form-group
        label="메모"
        class="has-float-label"
        style="font-size: 15px"
      >
        <b-form-textarea
          class="editTask"
          v-model="MetaData.memo"
          :state="memoState"
          :maxLength="200"
          :rows="5"
          :max-rows="5"
          placeholder="메모"
          no-resize
          trim
        />
      </b-form-group>
      <p
        v-show="memoState"
        style="
          position: relative;
          left: 380px;
          top: -15px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ MetaData.memo.length }}/200
      </p>
    </div>
    <div style="height: 50px; margin-top: 85px">
      <b-form-group
        label="제작자"
        class="has-float-label"
        style="font-size: 15px"
      >
        <b-form-input
          title="제작자"
          style="width: 430px; font-size: 14px"
          class="editTask"
          :value="userID"
          disabled
          aria-describedby="input-live-help input-live-feedback"
          placeholder="제작자"
          trim
        />
      </b-form-group>
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
    this.setTitle(this.sliceExt(200));
  },
  methods: {},
};
</script>

<style></style>
