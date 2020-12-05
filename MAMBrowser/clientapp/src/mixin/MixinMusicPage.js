import MixinCommon from './MixinCommon';

let mixinBasicPage = {
    mixins: [ MixinCommon ],
    data() {
        return {
            selectedIds: null,
        }
    },
    created() {
        this.getData();
    },
    methods: {
        onRefresh() {
            console.debug('onRefresh');
            this.getData();
        },
        onSelectedIds(ids) {
            this.selectedIds = ids;
        },
    }
}

export default mixinBasicPage;