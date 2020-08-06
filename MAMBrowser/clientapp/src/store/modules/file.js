import FileUploadModel from '../../lib/file/FileUploadModel';

export default {
    namespaced: true,
    getters:{
        getFileData: state => state.fileData,
    },
    state: {
        /**
         * fileData: [ { jsonMetaData: '', file: file }, ....]
         */
        fileData: [],
        component: null,
    },
    mutations: {
        SET_FILES: (state, { files, meta }) => {
            files.forEach(file => {
                state.fileData.push(
                    { jsonMetaData: JSON.parse(JSON.stringify(meta)), file: file }
                );
            })
        },
        REMOVE_FILES: (state, id) => {
            const findIndex = state.fileData.findIndex(file => file.file.id === id);
            state.fileData.splice(findIndex, 1);
        },
        REMOVE_FILES_ALL: (state) => {
            state.fileData = [];
        }
    },
    actions: {
        // 파일 추가
        add_files: ({ state, commit }, { files, meta }) => {
            if (!files) return;
            
            // if (files && state.fileData.length > 0) {
            //     FileUploadModel.uploadPopup.hide();
            // }

            commit('SET_FILES', { files, meta });
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
            if (files.length <= 1) {
                FileUploadModel.singleMetaPopup.show(files);
                return;
            }
            
            if (files.length > 1) {
                FileUploadModel.multiMetaPopup.show(files);
            }
        },
        // 메타 데이터 입력 팝업 닫기
        close_meta_data_popup: () => {
            FileUploadModel.singleMetaPopup.close();
            FileUploadModel.multiMetaPopup.close();
        }
    }
}