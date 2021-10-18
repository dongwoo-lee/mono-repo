import MixinCommon from "./MixinCommon";

let mixinBasicPage = {
  mixins: [MixinCommon],
  data() {
    return {
      selectedIds: null
    };
  },
  //NOTE: 음반 기록실, 효과음 onLoad 검색 제외
  // created() {
  //     this.getData();
  // },
  methods: {
    onRefresh() {
      console.debug("onRefresh");
      this.getData();
    },
    onSelectedIds(ids) {
      this.selectedIds = ids;
    }
  }
};

export default mixinBasicPage;
