using Dapper;
using MAMBrowser.Common;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Foundation;
using MAMBrowser.Models;
using System;
using System.IO;

namespace MAMBrowser.BLL
{
    public class PublicFileBll
    {
        private readonly PublicFileDao _dao;
        private readonly IFileProtocol _fileProtocol;
        public PublicFileBll(PublicFileDao dao, ServiceResolver sr)
        {
            _dao = dao;
            _fileProtocol = sr("PublicWorkConnection");
        }
    
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> UploadFile(string userId, Stream stream, string fileName, M30_MAM_PUBLIC_SPACE metaData)
        { //유효성검사
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
                var relativeSourceFolder = $"{_fileProtocol.TmpUploadFolder}";
                var relativeTargetFolder = @$"{_fileProtocol.UploadFolder}\{userId}\{userId}-{date}";      //공유소재도 유저확장ID 사용?, 분류코드별로...필요해보임.
                var relativeSourcePath = @$"{relativeSourceFolder}\{newFileName}";
                var relativeTargetPath = @$"{relativeTargetFolder}\{newFileName}";

                var headerStream = AudioEngine.GetHeaderStream(stream);
                headerStream.Position = 0;
                var audioFormat = AudioEngine.GetAudioFormat(headerStream, relativeTargetPath);
                headerStream.Position = 0;

                _fileProtocol.MakeDirectory(relativeSourceFolder);
                _fileProtocol.Upload(headerStream, stream, relativeSourcePath);
                _fileProtocol.MakeDirectory(relativeTargetFolder);
                _fileProtocol.Move(relativeSourcePath, relativeTargetPath);
                stream.Dispose();
                headerStream.Dispose();

                metaData.SEQ = ID;
                metaData.USER_ID = userId;
                metaData.AUDIO_FORMAT = audioFormat;
                metaData.FILE_PATH = @$"\\{_fileProtocol.UploadHost}\{relativeTargetPath}";
                _dao.Insert(metaData);

                return validateResult;
            }
        }
        public bool Delete(long seq)
        {
            var fileData = Get(seq);
            _fileProtocol.Delete(fileData.FilePath);

            return _dao.Delete(seq);
        }
        public int UpdateData(long seq, M30_MAM_PUBLIC_SPACE metaData)
        {
            return _dao.UpdateData(seq, metaData);
        }
        public DTO_PUBLIC_FILE Get(long id)
        {
            return _dao.Get(id);
        }
        public DTO_RESULT_PAGE_LIST<DTO_PUBLIC_FILE> FineData(string mediaCd, string cateCd, string start_dt, string end_dt, string userId, string title, string memo, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            return _dao.FineData(mediaCd, cateCd, start_dt, end_dt, userId, title, memo, rowPerPage, selectPage, sortKey, sortValue);
        }
     
        public bool IsExistTitle(string title)
        {
            return _dao.Get(title)!=null ? true : false;
        }
        public long CountPublicCategory(string cateCd)
        {
            return _dao.CountPublicCategory(cateCd);
        }
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> VerifyModel(string userId,  M30_MAM_PUBLIC_SPACE metaData, string fileName)
        {
            DTO_RESULT<DTO_RESULT_OBJECT<string>> result = new DTO_RESULT<DTO_RESULT_OBJECT<string>>();

            if (CountPublicCategory(metaData.CATE_CD) >= 200)
            {
                result.ResultCode = RESUlT_CODES.INVALID_DATA;
                result.ErrorMsg = "공유소재 최대 등록한도가 초과되었습니다.(200개 초과)";
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
    }
}
