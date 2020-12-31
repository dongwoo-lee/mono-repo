using MAMBrowser.Controllers;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using MAMBrowser.Processor;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.BLL
{
    public class PrivateFileBLL
    {
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;
        private readonly PrivateFileDAL _dal;
        private readonly ILogger<ProductsController> _logger;
        public PrivateFileBLL(IOptions<AppSettings> appSesstings, ServiceResolver sr, PrivateFileDAL dal, ILogger<ProductsController> logger)
        {
            _appSettings = appSesstings.Value;
            _fileService = sr("PrivateWorkConnection");
            _dal = dal;
            _logger = logger;
        }

        public DTO_RESULT<DTO_RESULT_OBJECT<string>> UploadFile(string userId, Stream stream, string fileName, PrivateFileModel metaData)
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
                _logger.LogDebug($"VerifyModel - fail");
                return validateResult;
            }
            else
            {
                long ID = _dal.GetID();
                string date = DateTime.Now.ToString(MAMUtility.DTM8);
                string newFileName = $"{ ID.ToString() }_{fileName}";
                var relativeSourceFolder = $"{_fileService.TmpUploadFolder}";
                var relativeTargetFolder = @$"{_fileService.UploadFolder}\{userId}\{userId}-{date}";
                var relativeSourcePath = @$"{relativeSourceFolder}\{newFileName}";
                var relativeTargetPath = @$"{relativeTargetFolder}\{newFileName}";

                var headerStream = AudioEngine.GetHeaderStream(stream);
                headerStream.Position = 0;
                var audioFormat = AudioEngine.GetAudioFormat(headerStream, relativeTargetPath);
                headerStream.Position = 0;
                _fileService.MakeDirectory(relativeSourceFolder);
                _fileService.Upload(headerStream, stream, relativeSourcePath);
                _fileService.MakeDirectory(relativeTargetFolder);
                _fileService.Move(relativeSourcePath, relativeTargetPath);
                stream.Dispose();
                headerStream.Dispose();

                metaData.SEQ = ID;
                metaData.USER_ID = userId;
                metaData.AUDIO_FORMAT = audioFormat;
                metaData.FILE_PATH = @$"\\{_fileService.UploadHost}\{relativeTargetPath}";
                _dal.Insert(metaData);

                return validateResult;
            }
        }
        
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> VerifyModel(string userId,  PrivateFileModel metaData, string fileName)
        {
            _logger.LogDebug($"VerifyModel - fileName{fileName}");
            DTO_RESULT<DTO_RESULT_OBJECT<string>> result = new DTO_RESULT<DTO_RESULT_OBJECT<string>>();
            APIDAL apiDal = new APIDAL();
            var user = apiDal.GetUserSummary(userId);
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
            if (_dal.IsExistTitle(metaData.TITLE))
            {
                result.ResultCode = RESUlT_CODES.INVALID_DATA;
                result.ErrorMsg = "동일한 제목이 이미 있습니다. 제목을 수정해주세요.";
                return result;
            }

            result.ResultCode = RESUlT_CODES.SUCCESS;
            return result;
        }

    }
}
