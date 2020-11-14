import $http from '../http';

export default {
    /**
     * API
     */
    /**
     * Categories
     */
    /**
     * PrivateFile
     */
    postPrivateFileUpload(userExtId, formData, config) {
        return $http.post(`/api/products/workspace/private/files${userExtId}`, formData, config);
    },
    getPrivateFileDownload(seq) {
        return $http.get(`/api/products/workspace/private/files/${seq}`)
    },
    /**
     * Product
     */
    postFileDownload() {
        return $http.post(`/api/products/files`);
    },
    postExternalFileDownload() {
        return $http.post(`/api/products/external/files`);
    },
    /**
     * PublicFile
     */
    postPublicFileUpload(userExtId, formData, config) {
        return $http.post(`/api/products/workspace/public/files`, formData, config);
    },
    getPublicFileDownload(seq) {
        return $http.get(`/api/products/workspace/public/files/${seq}`)
    },
}