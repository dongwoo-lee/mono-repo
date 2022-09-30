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

        public DTO_RESULT<DTO_RESULT_OBJECT<string>> UploadFile(string userId, Stream stream, string fileName, M30_MAM_PRIVATE_SPACE metaData)
        {
            // 파일 크기로 유효성검사 필요. 이함수 들어오기전 metaData의 FileSize필드 값이 꼭 채워져야함.
            // 업로드시 IFROMFILE 객체의 사이즈
            // PRODUCT MY공간 복사 : 넷드라이브 소스 스트림의 사이즈.  DB에 파일 사이즈 없음
            // PUBLIC MY공간 복사 : DB의 파일 사이즈 사용
            // DL30 MY공간 복사 : DB의 파일 사이즈 이용
            // MUSIC MY공간 복사 : HTTP로 content의 content-length

            //유효성검사
            var validateResult = VerifyModel(userId, metaData, fileName);

            if (validateResult.ResultCode != RESUlT_CODES.SUCCESS)
            {
                return validateResult;
            }
            else
            {
                long ID = _dao.GetID();
                string date = DateTime.Now.ToString(Define.DTM8);
                string newFileName = $"{ID.ToString() }_{fileName}";
                string fileExt = Path.GetExtension(newFileName);
                var relativeSourceFolder = $"{_fileProtocol.TmpUploadFolder}";
                var relativeTargetFolder = @$"{_fileProtocol.UploadFolder}\{userId}\{userId}-{date}";
                var relativeSourcePath = @$"{relativeSourceFolder}\{newFileName}";
                var relativeTargetPath = @$"{relativeTargetFolder}\{newFileName}";

                var audioHeaderStream = GetHeaderStream(stream);
                //audioHeaderStream.Position = 0;
                var soundInfo = AudioEngine.GetAudioInfo(audioHeaderStream, fileExt, metaData.FILE_SIZE);
                audioHeaderStream.Position = 0;
                _fileProtocol.MakeDirectory(relativeSourceFolder);
                _fileProtocol.Upload(audioHeaderStream, stream, relativeSourcePath);
                _fileProtocol.MakeDirectory(relativeTargetFolder);
                _fileProtocol.Move(relativeSourcePath, relativeTargetPath);
                stream.Dispose();
                audioHeaderStream.Dispose();
                
                metaData.SEQ = ID;
                metaData.USER_ID = userId;
                metaData.AUDIO_FORMAT = soundInfo.AudioFormat;
                metaData.FILE_PATH = @$"\\{_fileProtocol.UploadHost}\{relativeTargetPath}";
                _dao.Insert(metaData, (int)soundInfo.TotalTime.TotalMilliseconds);
               
                return validateResult;
            }
        }

        public DTO_RESULT<DTO_RESULT_OBJECT<string>> VerifyModel(string userId, M30_MAM_PRIVATE_SPACE metaData, string fileName)
        {
            DTO_RESULT<DTO_RESULT_OBJECT<string>> result = new DTO_RESULT<DTO_RESULT_OBJECT<string>>();
            var user = _apiDao.GetUserSummary(userId);
            if (user.DiskAvailable < metaData.FILE_SIZE)
            {
                result.ResultCode = RESUlT_CODES.INVALID_DATA;
                result.ErrorMsg = "디스크 여유 공간이 부족합니다.";
                return result;
            }
            if (int.MaxValue < metaData.FILE_SIZE)
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
            if (IsExistTitle(metaData.TITLE))
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

        public void ResistryMyDisk(string title, string memo, string userId, string uncSourceFilePath)
        {
            M30_MAM_PRIVATE_SPACE myDiskData = new M30_MAM_PRIVATE_SPACE();
            long fileSize = 0;
            int intDuration = 0;
            string fileName = Path.GetFileName(uncSourceFilePath);
            string fileExt = Path.GetExtension(uncSourceFilePath);
            string audioFormat = "";


            //1. 파일의 헤더를 읽어서 wav형식, 음원길이, 파일크기 가져오기.
            using (FileStream sourceStream = new FileStream(uncSourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileSize = sourceStream.Length;
                using (var audioHeaderStream = GetHeaderStream(sourceStream))
                {
                    var soundInfo = AudioEngine.GetAudioInfo(audioHeaderStream, fileExt, fileSize);
                    audioFormat = soundInfo.AudioFormat;
                    intDuration = (int)soundInfo.TotalTime.TotalMilliseconds;
                }
            }

            //2. MY-DISK 저장 공간 옵션 값 가져오기
            var options =_apiDao.GetOptions("S01G06C001").ToList();
            var myDiskDirectory =options.Find(op => op.Name == "MYDISK_PATH");

            //3. MY-DISK ID 채번 (파일명
            var myDiskID = _dao.GetID();

            //4. MY-DISK 저장 공간 디렉토리 만들기
            string date = DateTime.Now.ToString(Define.DTM8);
            string newFileName = $"{myDiskID}_{fileName}";
            var targetDirectory = @$"{myDiskDirectory}\{userId}\{userId}-{date}";
            var targetFilePath = @$"{targetDirectory}\{newFileName}";
            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            //5. 파일을 목적지로 옮기기.
            _fileProtocol.Move(uncSourceFilePath, targetFilePath);
              
            //6. DB에 등록하기.
            myDiskData.SEQ = myDiskID;
            myDiskData.TITLE = title;
            myDiskData.MEMO = memo;
            myDiskData.USER_ID = userId;
            myDiskData.AUDIO_FORMAT = audioFormat;
            myDiskData.FILE_SIZE = fileSize;
            myDiskData.FILE_PATH = targetFilePath;
            _dao.Insert(myDiskData, intDuration);

        }

    }
}
