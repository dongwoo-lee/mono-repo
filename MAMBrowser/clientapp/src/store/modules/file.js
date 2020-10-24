import $http from '../../http';
import FileUploadRefElement from '../../lib/file/FileUploadRefElement';
import $fn from '../../utils/CommonFunctions';
let uploadCancelToken;

export default {
    namespaced: true,
    getters:{
        getFileData: state => state.fileData,
        getUploadTransmitState: state => state.uploadTransmitState,
        getBeingUploaded: state => state.fileData.length > 0 && state.fileData.some(file => file.uploadState !== 'success'),
    },
    state: {
        fileData: [],
        uploadTransmitState: false,                    // 업로드 전송 상태
        uploadViewType: '',                            // 업로드 화면 타입: 'private, 'public
        isDragDropState: false,
    },
    mutations: {
        SET_FILES: (state, { files, meta }) => {
            if (!files) return;
            const existFiles = state.fileData.filter(data => {
                return files.some(file => data.file.name === file.name);
            });

            if (existFiles.length > 0) {
                const existFileNames = [];
                existFiles.forEach(file => {
                    existFileNames.push(file.file.name);
                })

                $fn.notify('warning', { message:  `${existFileNames.join(',')}인 파일이 존재합니다.` })
                return;
            }
            files.forEach(file => {
                state.fileData.push(
                    { 
                        metaData: JSON.stringify(meta), // 메타데이터
                        file: file,                     // 파일
                       uploadState: 'wait'             // 업로드 상태(wait:대기중, stop:정지, start:전송중, success:전송완료, save:저장중)
                    }
                );
            })
        },
        REMOVE_FILES: (state, id) => {
            const findIndex = state.fileData.findIndex(data => data.file.id === id);
            state.fileData.splice(findIndex, 1);
        },
        REMOVE_FILES_ALL: (state) => {
            state.fileData = [];
        },
        CHANGE_FILE_UPLOAD_STATE: (state) => {
            state.fileData.some(data => {
                if (!data.file.success) {
                    data.uploadState = 'stop';
                    return true;
                }
                return false;
            })
        },
        SET_UPLOAD_STATE: (state, value) => {
            state.uploadTransmitState = value;
        },
        SET_UPLOAD_VIEW_TYPE: (state, value) => {
            state.uploadViewType = value;
        },
        SET_DRAG_DROP_STATE: (state, value) => {
            state.isDragDropState = value;
        }
    },
    actions: {
        async upload({ state, commit }) {
            // 취소 토큰 생성
            uploadCancelToken = new $http.CancelToken.source();
            // 업로드 상태 변경
            commit('SET_UPLOAD_STATE', true);
            // 파일 업로드
            for (const data of state.fileData) {
                if (data.uploadState !== 'success') {
                    const formData = new FormData();
                    formData.append('file', data.file.file);
                    formData.append('metaData', data.metaData);
                    
                    // 요청 설정
                    const config = {
                        onUploadProgress: progressEvent => 
                        {
                            data.file.active = true;
                            data.uploadState = 'start';
                            data.file.progress = Math.round((progressEvent.loaded / data.file.size) * 100);

                            if (data.file.progress === 100) {
                                data.file.success = true;
                                data.uploadState = 'save';
                            }
                        },
                        cancelToken: uploadCancelToken.token,
                        'Content-Type': 'multipart/form-data',
                    }

                    let userExtId = state.uploadViewType === 'public' ? '' : `/${sessionStorage.getItem('user_ext_id')}`;
                    try {
                        const res = await $http.post(`/api/products/workspace/${state.uploadViewType}/files${userExtId}`, formData, config);

                        if (res && res.status === 200 && !res.data.errorMsg) {
                            data.uploadState = 'success';
                            $fn.notify('success', { message: '파일 업로드 완료' })
                            // 테이블 새로고침
                            const refScrollPaging = FileUploadRefElement.getScrollPaging();
                            refScrollPaging.tableRefresh();
                        } else {
                            $fn.notify('error', { message: '파일 업로드 실패: ' + res.data.errorMsg })
                        }
                    } catch (e) {
                        console.log(e);
                    }
                }
                console.info('test-2');
                commit('SET_UPLOAD_STATE', false);
            }
        },
        download({}, {ids, type}) {
            ids.forEach(id => {
                $http.get(`/api/products/workspace/${type}/files/${id}`)
                    .then(res => {
                        $fn.fileDownload(res);
                        $fn.notify('success', { message: '파일 다운로드 완료' })
                    })
                    .catch(error => {
                        $fn.notify('error', { message: '파일 다운로드 실패: ' + error })
                    })

            })
        },
        downloadSound({}, item) {
            $http.get(`/api/Products/external/files?filePath=${item.filePath}`)
                .then(res => {
                    $fn.fileDownload(res);
                    $fn.notify('success', { message: '파일 다운로드 완료' })
                })
                .catch(error => {
                    $fn.notify('error', { message: '파일 다운로드 실패: ' + error })
                })
        },
        downloadEtc({}, item) {
            $http.get(`/api/Products/files?filePath=${item.filePath}`)
                .then(res => {
                    $fn.fileDownload(res);
                    $fn.notify('success', { message: '파일 다운로드 완료' })
                })
                .catch(error => {
                    $fn.notify('error', { message: '파일 다운로드 실패: ' + error })
                })
        },
        cancel_upload: ({ commit }) => {
            // 취소 토큰 실행
            uploadCancelToken.cancel();
            commit('SET_UPLOAD_STATE', false);
            commit('CHANGE_FILE_UPLOAD_STATE');
        },
        // 파일 제거
        remove_files: ({ commit }, id) => {
            commit('REMOVE_FILES', id);
        },
        // 파일 업로드 하단 창 호출
        open_toast: ({}) => {
            FileUploadRefElement.uploadPopup.hide(true);
            FileUploadRefElement.uploadToast.show();
        },
        // 파일 업로드 팝업 호출
        open_popup: () => {
            FileUploadRefElement.uploadToast.close();
            FileUploadRefElement.uploadPopup.show();
        },
        // 메타 데이터 입력 팝업 호출
        open_meta_data_popup: ({}, files) => {
            FileUploadRefElement.fileMetaPopup.show(files);
        },
        // 메타 데이터 입력 팝업 닫기
        close_meta_data_popup: () => {
            FileUploadRefElement.fileMetaPopup.close();
        }
    }
}