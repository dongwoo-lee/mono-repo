class FileUploadModel {
    constructor() {
        this.self = null;
        this.uploadToast = null;
        this.uploadPopup = null;
        this.singleMetaPopup = null;
        this.multiMetaPopup = null;
        this.fileUpload = null;
    }

    init(self) {
        this.self = self;
        this.uploadToast = this.getUploadToast();
        this.uploadPopup = this.getUploadPopup();
        this.singleMetaPopup = this.getSingleFileMetaDataPopup();
        this.multiMetaPopup = this.getMultiFileMetaDataPopup();
        this.fileUpload = this.getFileUpload();
    }

    refsFind(refsName, self) {
        if (typeof self.$refs[refsName] !== 'undefined') {
            return self.$refs[refsName];
        }
        return this.refsFind(refsName, self.$parent);
    }

    refsFindChildren(refsName, self) {
        let findComponent = null;
        // 강제로 하위 컴포넌트 찾기
        const childrenComponent = self['$children'][0]['$children'];
        childrenComponent.forEach(vueComponent => {
            if (typeof vueComponent.$refs[refsName] !== 'undefined') {
                findComponent = vueComponent.$refs[refsName];
            }
        })

        return findComponent;
    }

    getUploadToast() {
        return this.refsFind('refFileUploadingToast', this.self);
    }

    getUploadPopup() {
        return this.refsFind('refFileUploadPopup', this.self);
    }

    getSingleFileMetaDataPopup() {
        return this.refsFind('refSingleFileMetaDataPopup', this.self);
    }

    getMultiFileMetaDataPopup() {
        return this.refsFind('refMultiFileMetaDataPopup', this.self);
    }

    getFileUpload() {
        return this.refsFindChildren('refFileUpload', this.self);
    }
}

export default new FileUploadModel;
