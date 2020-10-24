import MixinCommon from './MixinCommon';

let mixinFillerPage = {
    mixins: [ MixinCommon ],
    data() {
        return {
            spotOptions: [],                         // 주조 spot 분류 목록
            categoryOptions: [],                     // 필러 화면별 카테고리 분류
            timetoneOptions: [],                     // 필러(시간) 분류 목록
            reqStatusOptions: [],                    // 방송의뢰 상태 목록
        }
    },
    methods: {
        // 주조 spot 분류 목록 조회
        getSpotOptions(media) {
          this.requestCall('/api/Categories/mcr/spot?media=' + media, 'spotOptions');
        },
        // 방송의뢰 상태 목록 조회
        getReqStatusOptions() {
            this.requestCall('/api/Categories/req-status', 'reqStatusOptions');
        },
        // 필러(pr) 분류 목록 조회
        getProOptions() {
            this.requestCall('/api/Categories/filler/pro', 'categoryOptions');
        },
        // 필러(일반) 분류 목록 조회
        getGeneralOptions() {
            this.requestCall('/api/Categories/filler/general', 'categoryOptions');
        },
        // 필러(시간) 분류 목록 조회
        getTimetoneOptions() {
            this.requestCall('/api/Categories/filler/timetone', 'timetoneOptions');
        },
        // 필러(기타) 분류 목록 조회
        getEtcOptions() {
            this.requestCall('/api/Categories/filler/etc', 'categoryOptions');
        },
        onSpotSelected(data) {
            const { id, name } = data;
            this.searchItems.spotId = id;
            this.searchItems.spotName = name;
        }
    }
}

export default mixinFillerPage;