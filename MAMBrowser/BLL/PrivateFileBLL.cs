using Dapper;
using MAMBrowser.BLL;
using M30.AudioFile.DAL;
using MAMBrowser.Foundation;
using MAMBrowser.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using M30.AudioFile.DAL.Dao;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common.Models;
using M30.AudioFile.Common;
using M30.AudioEngine;

namespace MAMBrowser.BLL
{
    public class PrivateFileBll
    {

        private readonly PrivateFileDao _dao;
        private readonly APIDao _apiDao;
        private readonly IFileProtocol _fileProtocol;
        
        public PrivateFileBll(PrivateFileDao dao, APIDao apiDao, ServiceResolver sr)
        {
            _dao = dao;
            _apiDao = apiDao;
            _fileProtocol = sr(MAMDefine.PrivateWorkConnection).FileSystem;
        }
        private MemoryStream GetHeaderStream(Stream stream)
        {
            int stepLength = 100000;
            int totalHeadLength = stepLength * 5;
            int totalRead = 0;

            byte[] buffer = new byte[stepLength];
            MemoryStream ms = new MemoryStream();
            while (true)
            {
                if (totalRead >= totalHeadLength)
                    break;

                var read = stream.Read(buffer, 0, buffer.Length);
                ms.Write(buffer, 0, read);
                totalRead += read;
            }
            ms.Flush();
            ms.Position = 0;
            return ms;
        }
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> VerifyModel(string title, string userId, long fileSize, string fileName)
        {
            DTO_RESULT<DTO_RESULT_OBJECT<string>> result = new DTO_RESULT<DTO_RESULT_OBJECT<string>>();
            var user = _apiDao.GetUserSummary(userId);
            if (user.DiskAvailable < fileSize)
            {
                result.ResultCode = RESUlT_CODES.INVALID_DATA;
                result.ErrorMsg = "디스크 여유 공간이 부족합니다.";
                return result;
            }
            if (int.MaxValue < fileSize)
            {
                result.ResultCode = RESUlT_CODES.INVALID_DATA;
                result.ErrorMsg = "파일 용량이 2GB를 초과하였습니다.";
                return result;
            }
            if (Path.GetExtension(fileName).ToUpper() != ".WAV" && Path.GetExtension(fileName).ToUpper() != ".MP3")
            {
                result.ResultCode = RESUlT_CODES.INVALID_DATA;
                result.ErrorMsg = "WAV, MP3 파일만 업로드 할 수 있습니다.";
                return result;
            }
            if (IsExistTitle(title))
            {
                result.ResultCode = RESUlT_CODES.INVALID_DATA;
                result.ErrorMsg = "동일한 제목이 이미 있습니다. 제목을 수정해주세요.";
                return result;
            }

            result.ResultCode = RESUlT_CODES.SUCCESS;
            return result;
        }
       
        public void MoveRecycleBin(string userId, List<long> seqList)
        {
            _dao.UpdateUsedN(userId, seqList);
        }
        public void DeleteRecycleBin(string userId, List<long> seqList)
        {
            if (seqList.Count <= 0)
                return;

            //물리파일 삭제
            foreach (var seq in seqList)
            {
                var fileData = Get(seq);

                if (fileData == null)
                    continue;
               
                if (File.Exists(fileData.FilePath))
                {
                    _fileProtocol.Delete(fileData.FilePath);
                }
            }

            _dao.Delete(userId, seqList);
        }

        public void DeleteAllRecycleBin(string userId)    
        {
            var seqList = _dao.GetSeqAllByUsedN(userId);
            DeleteRecycleBin(userId, seqList);
        }
        public IList<DTO_PRIVATE_FILE> GetAllByUsedN(string userId)
        {
            return _dao.GetAllByUsedN(userId);
        }

        public void RecycleAll(string userId, List<long> seqList)    //복원
        {
            _dao.UpdateUsedY(userId, seqList);
        }

        public int UpdateData(M30_MAM_PRIVATE_SPACE metaData)
        {
            return _dao.UpdateData(metaData);
        }

        public DTO_PRIVATE_FILE Get(long id)
        {
            return _dao.Get(id);
        }
        public IList<DTO_PRIVATE_FILE> Get(List<long> ids)
        {
            return _dao.Get(ids);
        }
       
        public DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE> FindData(string used, string start_dt, string end_dt, string userId, string title, string memo, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            return _dao.FindData(used, start_dt, end_dt, userId, title, memo, rowPerPage, selectPage, sortKey, sortValue);
        }
        public long GetID()
        {
            return _dao.GetID();
        }
        public bool IsExistTitle (string title)
        {
            var dto = _dao.Get(title);
            return dto != null ? true : false;
        }



        /// <summary>
        /// MY디스크 등록전 유효성 검사
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fileSize"></param>
        /// <param name="title"></param>
        /// <param name="fileName">확장자 포함 파일 이름</param>
        /// <returns></returns>
        public WorkResult VerifyModel(string title, string userId, long fileSize)
        {
            WorkResult result = new WorkResult();
            var user = _apiDao.GetUserSummary(userId);

            if (user.DiskAvailable < fileSize)
            {
                result.Success = false;
                result.ErrorMessage = "디스크 여유 공간이 부족합니다.";
                return result;
            }
            if (int.MaxValue < fileSize)
            {
                result.Success = false;
                result.ErrorMessage = "파일 용량이 2GB를 초과하였습니다.";
                return result;
            }
            if (IsExistTitle(title))
            {
                result.Success = false;
                result.ErrorMessage = "동일한 제목이 이미 있습니다. 제목을 수정해주세요.";
                return result;
            }

            result.Success = true;
            return result;
        }

        /// <summary>
        /// Copy storage to storage
        /// </summary>
        /// <param name="title"></param>
        /// <param name="memo"></param>
        /// <param name="userId"></param>
        /// <param name="uncSourceFilePath"></param>
        /// <returns></returns>
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> RegistryMyDiskFromStorage(string title, string memo, string userId, string uncSourceFilePath)
        {
            M30_MAM_PRIVATE_SPACE myDiskData = new M30_MAM_PRIVATE_SPACE();
            long fileSize = 0;
            int intDuration = 0;
            string fileName = Path.GetFileName(uncSourceFilePath);
            string fileExt = Path.GetExtension(uncSourceFilePath);
            string audioFormat = "";

            
            //1. 파일의 헤더를 읽어서 wav형식, 음원길이, 파일크기 가져오기.
            using (Stream sourceStream = _fileProtocol.GetFileStream(uncSourceFilePath, 0))
            {
                fileSize = sourceStream.Length;
                using (var audioHeaderStream = GetHeaderStream(sourceStream))
                {
                    var soundInfo = AudioEngine.GetAudioInfo(audioHeaderStream, fileExt, fileSize);
                    audioFormat = soundInfo.AudioFormat;
                    intDuration = (int)soundInfo.TotalTime.TotalMilliseconds;
                }
            }

            // 1.1 유효성 검사
            var validateResult = VerifyModel(title, userId, fileSize, fileName);
            if (validateResult.ResultCode != RESUlT_CODES.SUCCESS)
                return validateResult;

            //2. MY-DISK 저장 공간 옵션 값 가져오기
            var options =_apiDao.GetOptions("S01G06C001").ToList();
            var uncTempUploadPath = options.Find(op => op.Name == "MAM_UPLOAD_PATH").Value;
            var uncMyDiskDirectory = options.Find(op => op.Name == "MYDISK_PATH").Value;


            //3. MY-DISK ID 채번 (파일명
            var myDiskID = _dao.GetID();

            //4. MY-DISK 저장 공간 디렉토리 만들기
            string date = DateTime.Now.ToString(Define.DTM8);
            string newFileName = $"{myDiskID}_{fileName}";
            var tempFilePath = Path.Combine(uncTempUploadPath, newFileName);
            var uncTargetDirectory = @$"{uncMyDiskDirectory}\{userId}\{userId}-{date}";
            var uncTargetFilePath = @$"{uncTargetDirectory}\{newFileName}";

            if (!_fileProtocol.ExistDirectory(uncTempUploadPath))
                _fileProtocol.MakeDirectory(uncTempUploadPath);

            if (!_fileProtocol.ExistDirectory(uncTargetDirectory))
                _fileProtocol.MakeDirectory(uncTargetDirectory);

            //5. 파일을 임시 목적지로 복사.
            _fileProtocol.Copy(uncSourceFilePath, tempFilePath);
              
            //6. DB에 등록하기.
            myDiskData.SEQ = myDiskID;
            myDiskData.TITLE = title;
            myDiskData.MEMO = memo;
            myDiskData.USER_ID = userId;
            myDiskData.AUDIO_FORMAT = audioFormat;
            myDiskData.FILE_SIZE = fileSize;
            myDiskData.FILE_PATH = uncTargetFilePath;
            _dao.Insert(myDiskData, intDuration);

            //7. 파일 최종위치로 옮김.
            _fileProtocol.Move(tempFilePath, uncTargetFilePath);

            return validateResult;
        }
        /// <summary>
        /// upload local to storage
        /// </summary>
        /// <param name="title"></param>
        /// <param name="memo"></param>
        /// <param name="userId"></param>
        /// <param name="localSourceFilePath"></param>
        /// <returns></returns>
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> RegistryMyDiskFromLocalFile(string title, string memo, string userId, string localSourceFilePath)
        {
            M30_MAM_PRIVATE_SPACE myDiskData = new M30_MAM_PRIVATE_SPACE();
            long fileSize = 0;
            int intDuration = 0;
            string fileName = Path.GetFileName(localSourceFilePath);
            string fileExt = Path.GetExtension(localSourceFilePath);
            string audioFormat = "";


            //1. 파일의 헤더를 읽어서 wav형식, 음원길이, 파일크기 가져오기.
            using (Stream sourceStream = new FileStream(localSourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileSize = sourceStream.Length;
                using (var audioHeaderStream = GetHeaderStream(sourceStream))
                {
                    var soundInfo = AudioEngine.GetAudioInfo(audioHeaderStream, fileExt, fileSize);
                    audioFormat = soundInfo.AudioFormat;
                    intDuration = (int)soundInfo.TotalTime.TotalMilliseconds;
                }
            }

            // 1.1 유효성 검사
            var validateResult = VerifyModel(title, userId, fileSize, fileName);
            if (validateResult.ResultCode != RESUlT_CODES.SUCCESS)
                return validateResult;

            //2. MY-DISK 저장 공간 옵션 값 가져오기
            var options = _apiDao.GetOptions("S01G06C001").ToList();
            var uncTempUploadPath = options.Find(op => op.Name == "MAM_UPLOAD_PATH").Value;
            var uncMyDiskDirectory = options.Find(op => op.Name == "MYDISK_PATH").Value;


            //3. MY-DISK ID 채번 (파일명
            var myDiskID = _dao.GetID();

            //4. MY-DISK 저장 공간 디렉토리 만들기
            string date = DateTime.Now.ToString(Define.DTM8);
            string newFileName = $"{myDiskID}_{fileName}";
            var tempFilePath = Path.Combine(uncTempUploadPath, newFileName);
            var uncTargetDirectory = @$"{uncMyDiskDirectory}\{userId}\{userId}-{date}";
            var uncTargetFilePath = @$"{uncTargetDirectory}\{newFileName}";

            if (!_fileProtocol.ExistDirectory(uncTempUploadPath))
                _fileProtocol.MakeDirectory(uncTempUploadPath);

            if (!_fileProtocol.ExistDirectory(uncTargetDirectory))
                _fileProtocol.MakeDirectory(uncTargetDirectory);

            //5. 파일을 임시 목적지로 복사.
            _fileProtocol.Upload(localSourceFilePath, tempFilePath);

            //6. DB에 등록하기.
            myDiskData.SEQ = myDiskID;
            myDiskData.TITLE = title;
            myDiskData.MEMO = memo;
            myDiskData.USER_ID = userId;
            myDiskData.AUDIO_FORMAT = audioFormat;
            myDiskData.FILE_SIZE = fileSize;
            myDiskData.FILE_PATH = uncTargetFilePath;
            _dao.Insert(myDiskData, intDuration);

            //7. 파일 최종위치로 옮김.
            _fileProtocol.Move(tempFilePath, uncTargetFilePath);

            return validateResult;
        }
        /// <summary>
        /// 메모리 스트림으로 복사 후 처리.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="memo"></param>
        /// <param name="userId"></param>
        /// <param name="fileName"></param>
        /// <param name="sourceStream"></param>
        /// <returns></returns>
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> RegistryMyDiskFromMusicStream(string title, string memo, string userId, string fileName, Stream sourceStream)
        {
            M30_MAM_PRIVATE_SPACE myDiskData = new M30_MAM_PRIVATE_SPACE();
            long fileSize = 0;
            int intDuration = 0;
            //string fileName = Path.GetFileName(fileName);
            string fileExt = Path.GetExtension(fileName);
            string audioFormat = "";

            //1. 파일의 헤더를 읽어서 wav형식, 음원길이, 파일크기 가져오기.
            using (MemoryStream ms = new MemoryStream())
            {
                sourceStream.CopyTo(ms);
                fileSize = ms.Length;
                ms.Position = 0;

                using (var audioHeaderStream = GetHeaderStream(ms))
                {
                    var soundInfo = AudioEngine.GetAudioInfo(audioHeaderStream, fileExt, fileSize);
                    audioFormat = soundInfo.AudioFormat;
                    intDuration = (int)soundInfo.TotalTime.TotalMilliseconds;
                }
                ms.Position = 0;

                // 1.1 유효성 검사
                var validateResult = VerifyModel(title, userId, fileSize, fileName);
                if (validateResult.ResultCode != RESUlT_CODES.SUCCESS)
                    return validateResult;

                //2. MY-DISK 저장 공간 옵션 값 가져오기
                var options = _apiDao.GetOptions("S01G06C001").ToList();
                var uncTempUploadPath = options.Find(op => op.Name == "MAM_UPLOAD_PATH").Value;
                var uncMyDiskDirectory = options.Find(op => op.Name == "MYDISK_PATH");

                //3. MY-DISK ID 채번 (파일명
                var myDiskID = _dao.GetID();

                //4. MY-DISK 저장 공간 디렉토리 만들기
                string date = DateTime.Now.ToString(Define.DTM8);
                string newFileName = $"{myDiskID}_{fileName}";
                var tempFilePath = Path.Combine(uncTempUploadPath, newFileName);
                var uncTargetDirectory = @$"{uncMyDiskDirectory}\{userId}\{userId}-{date}";
                var uncTargetFilePath = @$"{uncTargetDirectory}\{newFileName}";

                if (_fileProtocol.ExistDirectory(uncTempUploadPath))
                    _fileProtocol.MakeDirectory(uncTempUploadPath);

                if (_fileProtocol.ExistDirectory(uncTargetDirectory))
                    _fileProtocol.MakeDirectory(uncTargetDirectory);

                //5. 파일을 임시 목적지로 복사.
                _fileProtocol.Upload(ms, tempFilePath);

                //6. DB에 등록하기.
                myDiskData.SEQ = myDiskID;
                myDiskData.TITLE = title;
                myDiskData.MEMO = memo;
                myDiskData.USER_ID = userId;
                myDiskData.AUDIO_FORMAT = audioFormat;
                myDiskData.FILE_SIZE = fileSize;
                myDiskData.FILE_PATH = uncTargetFilePath;
                _dao.Insert(myDiskData, intDuration);

                //7. 파일 최종위치로 옮김.
                _fileProtocol.Move(tempFilePath, uncTargetFilePath);

                return validateResult;
            }
        }
    }
}
