class FileUploadModel {
    constructor() {
        this.self = null;
        this.uploadToast = null;
        this.uploadPopup = null;
        this.singleMetaPopup = null;
        this.multiMetaPopup = null;
    }

    init(self) {
        this.self = self;
        this.uploadToast = this.getUploadToast();
        this.uploadPopup = this.getUploadPopup();
        this.singleMetaPopup = this.getSingleFileMetaDataPopup();
        this.multiMetaPopup = this.getMultiFileMetaDataPopup();
    }

    refsFind(refsName, self) {
        if (typeof self.$refs[refsName] !== 'undefined') {
            return self.$refs[refsName];
        }
        return this.refsFind(refsName, self.$parent);
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
}

export default new FileUploadModel;
