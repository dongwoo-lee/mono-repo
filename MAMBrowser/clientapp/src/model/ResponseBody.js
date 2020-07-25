export default class ResponseBody {
    constructor(response, oldData) {
        this.response = response;
        this.responseData = null;
        this.oldData = oldData;
        this.init();
    }

    init() {
        this.responseData = this.response.data.resultObject;
        if (oldData) {
            
        }
    }

    getResponseData() {
        const { data, rowPerPage, selectPage, totalRowCount } = this.responseData;

        if (this.selectPage > 1)  {
            data.forEach(row => {
                data.push(row);
            })
        }
        const result = {
            data: data,
            rowPerPage: rowPerPage,
            selectPage: selectPage,
            totalRowCount: totalRowCount,
        }
        console.info (result);
        return result;
    }
}

class OldData {
    static getOldData(data) {
        
    }
}