class LoginPopupRefElement {
    constructor() {
        this.self = null;
    }

    init(self) {
        this.self = self;
        this.loginPopup = this.getLoginPopup();
    }

    refsFind(refsName, self) {
        if (typeof self.$refs[refsName] !== 'undefined') {
            return self.$refs[refsName];
        }
        return this.refsFind(refsName, self.$parent);
    }

    getLoginPopup() {
        return this.refsFind('refLoginPopup', this.self);
    }
}

export default new LoginPopupRefElement;