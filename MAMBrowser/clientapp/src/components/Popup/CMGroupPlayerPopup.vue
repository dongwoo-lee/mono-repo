<template>
  <!-- 미리듣기 팝업 -->
  <b-modal id="modal-player" size="xl" v-model="show" no-close-on-backdrop>
    <template slot="modal-title">
      <h5>{{ title }}</h5>
    </template>
    <template slot="default">
      <CMGroupPlayer
        ref="play"
        :title="this.title"
        :grpType="this.grpType"
        :brd_Dt="this.brd_Dt"
        :grpId="this.grpId"
      />
    </template>
    <template v-slot:modal-footer>
      <b-button
        variant="outline-danger default cutom-label-cancel"
        size="sm"
        class="float-right"
        @click="show = false"
      >
        닫기</b-button
      >
      <!-- 여기에다가 편집 저장 버튼 추가해야함 그리고 거기에 Click이벤트로 SOM, EOM 찍히는지 확인하기 -->
    </template>
  </b-modal>
</template>
<script>
export default {
  props: ["showPlayerPopup", "title", "grpId", "grpType", "brd_Dt"],
  data() {
    return {};
  },
  computed: {
    show: {
      get() {
        return this.showPlayerPopup;
      },
      set(v) {
        if (!v) {
          this.closePlayer();
        }
      },
    },
  },
  methods: {
    closePlayer() {
      this.$refs.play.close();
      this.$emit("closePlayer");
    },
  },
};
</script>