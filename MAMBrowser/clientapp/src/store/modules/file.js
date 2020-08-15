import $http from '../../http';
import FileUploadModel from '../../lib/file/FileUploadModel';
import $fn from '../../utils/CommonFunctions';

let fileUloadCancel;

export default {
    namespaced: true,
    getters:{
        getFileData: state => state.fileData,
        getUploadState: state => state.uploadState,
    },
    state: {
        /**
         * fileData: [ { jsonMetaData: '', file: file, isUpload: false }, ....]
         */
        fileData: [],
        uploadState: false,
        component: null,
    },
    mutations: {
        SET_FILES: (state, { files, meta }) => {
            files.forEach(file => {
                state.fileData.push(
                    { metaData: JSON.stringify(meta), file: file, isUpload: false }
                );
            })
        },
        REMOVE_FILES: (state, id) => {
            const findIndex = state.fileData.findIndex(file => file.file.id === id);
            state.fileData.splice(findIndex, 1);
        },
        REMOVE_FILES_ALL: (state) => {
            state.fileData = [];
        },
        CHANGE_FILE_UPLOAD_STATE: (state) => {
            state.fileData.some(file => {
                if (!file.file.success && file.isUpload) {
                    file.isUpload = false;
                    return true;
                }
                return false;
            })
        },
    },
    actions: {
        // 파일 추가
        add_files: ({ state, commit }, { files, meta }) => {
            if (!files) return;
            commit('SET_FILES', { files, meta });
        },
        async upload({ state }) {
            FileUploadModel.fileUpload.active = true;
            state.uploadState = true;
            try {
                for (const file of state.fileData) {
                    if (!file.isUpload) {
                        const formData = new FormData();
                        formData.append('file', file.file.file);
                        formData.append('metaData', file.metaData);

                        const config = {
                            onUploadProgress: progressEvent => 
                            {
                                file.file.active = true;
                                file.isUpload = true;
                                file.file.progress = Math.round((progressEvent.loaded / file.file.size) * 100);

                                if (file.file.progress === 100) {
                                    file.file.success = true;
                                }
                            },
                            cancelToken: new $http.CancelToken((c) => fileUloadCancel = c),
                            'Content-Type': 'multipart/form-data',
                        }

                        const res = await $http.post('/api/products/workspace/private/files/159', formData, config);
                        if (res && res.status === 200 && !res.data.errorMsg) {
                            $fn.notify('success', { message: '파일 업로드 완료' })
                        } else {
                            $fn.notify('error', { message: '파일 업로드 실패: ' + res.data.errorMsg })
                        }
                    }
                }

                FileUploadModel.fileUpload.active = false;
                state.uploadState = false;
            } catch (ex) {
                console.log(ex);
            }
        },
        cancel_upload: ({ state, commit }, value) => {
            fileUloadCancel();
            state.uploadState = false;
            commit('CHANGE_FILE_UPLOAD_STATE');
            
        },
        // 파일 제거
        remove_files: ({ commit }, id) => {
            commit('REMOVE_FILES', id);
        },
        // 파일 업로드딩 하단 창 호출
        open_toast: ({ dispatch }, data) => {
            FileUploadModel.uploadPopup.hide(true);
            FileUploadModel.uploadToast.show();

            if (data && data.files) {
                dispatch('add_files', data);
            }
        },
        // 파일 업로드 팝업 호출
        open_popup: () => {
            FileUploadModel.uploadToast.close();
            FileUploadModel.uploadPopup.show();
        },
        // 메타 데이터 입력 팝업 호출
        open_meta_data_popup: ({}, files) => {
            FileUploadModel.fileMetaPopup.show(files);
        },
        // 메타 데이터 입력 팝업 닫기
        close_meta_data_popup: () => {
            FileUploadModel.fileMetaPopup.close();
        }
    }
}