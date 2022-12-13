<template>
  <!-- 미리듣기 팝업 -->
  <b-modal id="music-player" size="lg" v-model="show" no-close-on-backdrop>
    <template
      slot="modal-title"
      class="musicPlayer-overflow"
      :title="this.music.name"
    >
      <h5>{{ this.music.name }}</h5>
    </template>
    <template slot="default">
      <b-card v-if="lyrics && imagePathList.length <= 0">
        <b-row>
          <b-col cols="5">
            <b-form-textarea
              v-if="imagePathList.length <= 0"
              rows="15"
              v-model="NotExistImage"
              no-resize
              readonly
            />

            <b-carousel
              v-else
              id="carousel1"
              style="text-shadow: 1px 1px 2px #333; height: 300px; width: 300px"
              controls
              indicators
              background="#ababab"
              :interval="5000"
              v-model="slide"
              @sliding-start="onSlideStart"
              @sliding-end="onSlideEnd"
            >
              <b-carousel-slide
                v-for="imagePath in imagePathList"
                :key="imagePath"
              >
                <template v-slot:img>
                  <img
                    class="d-block class-name"
                    width="300"
                    height="300"
                    :src="imagePath"
                    alt="image slot"
                  />
                </template>
              </b-carousel-slide>
            </b-carousel>
          </b-col>
          <b-col cols="7">
            <b-form-textarea v-model="lyrics" rows="15" no-resize readonly>
            </b-form-textarea>
          </b-col>
        </b-row>
      </b-card>
      <br />
      <b-card>
        <Player
          ref="play"
          :requestType="requestType"
          :fileKey="music.fileToken"
          :streamingUrl="streamingUrl"
          :waveformUrl="waveformUrl"
          :tempDownloadUrl="tempDownloadUrl"
          :direct="direct"
        />
      </b-card>
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
    </template>
  </b-modal>
</template>
<script>
import { USER_ID } from "@/constants/config";
export default {
  data() {
    return {
      tempImageDownloadUrl: "/api/musicsystem/song-images/temp-download",
      tempImageStreamingUrl: "/api/musicsystem/images/streaming",
      lyricsUrl: "/api/musicsystem/lyrics",
      imagePathList: [],
      lyrics: "",
      NotExistImage: "등록된 이미지가 없습니다.",
      slide: 0,
      sliding: null,
      USER_ID,
    };
  },
  props: {
    requestType: {
      type: String,
      default: () => {},
    },
    music: {
      type: Object,
      default: () => {},
    },
    tempDownloadUrl: {
      type: String,
      default: () => {},
    },
    streamingUrl: {
      type: String,
      default: () => {},
    },
    waveformUrl: {
      type: String,
      default: () => {},
    },
    showPlayerPopup: {
      type: Boolean,
      default: () => false,
    },
    direct: {
      type: String,
      default: () => {},
    },
  },
  computed: {
    show: {
      get() {
        return this.showPlayerPopup;
      },
      set(v) {
        if (!v) {
          this.closePlayer();
        } else {
          this.getMusic();
        }
      },
    },
  },
  watch: {
    showPlayerPopup: {
      handler: function (val, oldVal) {
        if (val) {
          this.getMusic();
        }
      },
    },
  },
  methods: {
    getMusic() {
      this.tempDownloadedImageUrl();
      if (this.music.lyricsSeq) {
        this.getLyrics();
      } else {
        this.lyrics = "등록된 가사 정보가 없습니다.";
      }
    },
    tempDownloadedImageUrl() {
      let userId = sessionStorage.getItem(USER_ID);
      var tempList = [];
      this.imagePathList = [];
      if (this.music.albumToken) {
        this.$http
          .get(`${this.tempImageDownloadUrl}?songID=${this.music.id}`, null)
          .then((res) => {
            tempList = res.data;
            tempList.forEach((fileName) => {
              let newPath = `${this.tempImageStreamingUrl}?filename=${fileName}&userId=${userId}`;
              this.imagePathList.push(newPath);
              console.info("newPath", newPath);
            });
          });
      }
    },
    getLyrics() {
      this.$http
        .get(`${this.lyricsUrl}/${this.music.lyricsSeq}`, null)
        .then((res) => {
          this.lyrics = res.data.resultObject.data;
        });
    },
    closePlayer() {
      this.lyrics = "";
      this.$refs.play.close();
      this.$emit("closePlayer");
    },
    onSlideStart(slide) {
      this.sliding = true;
    },
    onSlideEnd(slide) {
      this.sliding = false;
    },
  },
};
</script>
<style scoped>
.musicPlayer-overflow {
  width: 700px;
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
}
</style>
