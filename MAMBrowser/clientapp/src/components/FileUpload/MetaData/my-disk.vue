<template>
  <div>
    <div style="height: 50px; margin-top: 27px">
      <b-form-group
        label="제목"
        class="has-float-label"
        style="font-size: 15px"
      >
        <b-form-input
          class="editTask"
          v-model="myDiskMetaData.title"
          :state="myDiskTitleState"
          :maxLength="200"
          placeholder="제목"
          trim
        />
      </b-form-group>
      <p
        v-show="myDiskTitleState"
        style="
          position: relative;
          left: 380px;
          top: -15px;
          z-index: 9999;
          width: 30px;
        "
      >
        {{ myDiskMetaData.title.length }}/200
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
          v-model="myDiskMetaData.memo"
          :state="myDiskMemoState"
          :maxLength="200"
          rows="5"
          :max-rows="5"
          placeholder="메모"
          no-resize
          trim
        />
      </b-form-group>
      <p
        v-show="myDiskMemoState"
        style="
          position: relative;
          left: 380px;
          top: -15px;
          z-index: 9999;
          width: 30px;
          margin-right: 0px;
        "
      >
        {{ myDiskMetaData.memo.length }}/200
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
          :value="this.userName"
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
import { mapState, mapGetters, mapMutations } from "vuex";
export default {
  data() {
    return {
      userName: sessionStorage.getItem("user_name"),
    };
  },
  created() {
    this.RESET_MYDISK();
    this.SET_MYDISK_TITLE(this.sliceExt(200));
  },
  computed: {
    ...mapState("FileIndexStore", {
      myDiskMetaData: (state) => state.myDiskMetaData,
      MetaModalTitle: (state) => state.MetaModalTitle,
    }),
    ...mapGetters("FileIndexStore", ["myDiskTitleState", "myDiskMemoState"]),
  },
  methods: {
    ...mapMutations("FileIndexStore", ["SET_MYDISK_TITLE", "RESET_MYDISK"]),
    sliceExt(maxLength) {
      var result = this.MetaModalTitle.replace(/(.wav|.mp3)$/, "");
      return result.substring(0, maxLength);
    },
  },
};
</script>

<style></style>
