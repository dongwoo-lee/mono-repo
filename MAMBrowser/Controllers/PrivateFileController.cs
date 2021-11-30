using MAMBrowser.BLL;
using M30.AudioFile.Common;
using MAMBrowser.Foundation;
using MAMBrowser.Helper;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using M30.AudioFile.Common.Models;
using M30.AudioFile.Common.DTO;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/products/workspace/private")]
    public class PrivateFileController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly PrivateFileBll _bll;
        private readonly WebServerFileHelper _fileHelper;
        private readonly IFileProtocol _fileSystem;
        private readonly HttpContextDBLogger _logBll;

        public PrivateFileController(IHostingEnvironment hostingEnvironment, PrivateFileBll bll, HttpContextDBLogger logger, ServiceResolver sr, WebServerFileHelper fileHelper)
        {
            _bll = bll;
            _logBll = logger;
            
            _hostingEnvironment = hostingEnvironment;
            _fileHelper = fileHelper;
            _fileSystem = sr(MAMDefine.PrivateWorkConnection).FileSystem;
        }

        /// <summary>
        /// My 공간- 파일+메타데이터 등록
        /// </summary>
        /// <param name="userId">유저확장ID</param>
        /// <param name="file">파일</param>
        /// <param name="metaData">메타데이터</param>
        /// <returns></returns>
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        [HttpPost("files/{userId}")]
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> UploadFile(string userId, [FromForm] IFormFile file, [ModelBinder(BinderType = typeof(JsonModelBinder))] M30_MAM_PRIVATE_SPACE metaData)
        {
            
            DTO_RESULT<DTO_RESULT_OBJECT<string>> result = new DTO_RESULT<DTO_RESULT_OBJECT<string>>();
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    metaData.FILE_SIZE = file.Length;
                    result =  _bll.UploadFile(userId, stream, file.FileName, metaData);
                    if(result.ResultCode == RESUlT_CODES.SUCCESS)
                    {
                        _logBll.InfoAsync(HttpContext, userId, $"MY공간 소재 - 파일 업로드", UTF8JsonSerializer.Serialize(metaData));
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.ToString();
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        ///  My 공간 - 메타데이터 편집
        /// </summary>
        /// <param name="userId">유저확장ID</param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        [HttpPut("meta/{userId}")]
        public DTO_RESULT UpdateData(string userId, [FromBody] M30_MAM_PRIVATE_SPACE metaData)
        {
            DTO_RESULT result = new DTO_RESULT();
            try
            {
                if (_bll.UpdateData(metaData) > 0)
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                    
                    _logBll.InfoAsync(HttpContext, userId, "MY공간 소재 - 메타데이터 편집", UTF8JsonSerializer.Serialize(metaData));
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.APPLIED_NONE_WARN;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        /// <summary>
        /// VerifyModel
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        [HttpPost("verify/{userId}")]
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> VerifyModel(string userId, [FromBody] M30_MAM_PRIVATE_SPACE metaData)
        {
            return _bll.VerifyModel(userId, metaData, metaData.FILE_PATH);
        }


        /// <summary>
        /// My 공간 - 검색
        /// </summary>
        /// <param name="userId">사용자 확장ID</param>
        /// <param name="title">제목</param>
        /// <param name="memo">메모</param>
        /// <param name="rowPerPage">페이지당 행 개수</param>
        /// <param name="selectPage">선택된 페이지</param>
        /// <param name="sortKey">정렬 키(필드명)</param>
        /// <param name="sortValue">정렬 값(ASC/DESC)</param>
        /// <returns></returns>
        [HttpGet("meta/{userId}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> FindData(string userId, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string title, [FromQuery] string memo, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
                result.ResultObject = _bll.FindData("Y", start_dt, end_dt, userId, title, memo, rowPerPage, selectPage, sortKey, sortValue);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 휴지통 데이터 목록
        /// </summary>
        /// <param name="userId">사용자 확장ID</param>
        /// <param name="title">제목</param>
        /// <param name="memo">메모</param>
        /// <param name="rowPerPage">페이지당 행 개수</param>
        /// <param name="selectPage">선택된 페이지</param>
        /// <param name="sortKey">정렬 키(필드명)</param>
        /// <param name="sortValue">정렬 값(ASC/DESC)</param>
        /// <returns></returns>
        [HttpGet("recyclebin/{userId}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> FindRecycleBin(string userId, [FromQuery] string title, [FromQuery] string memo, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
                result.ResultObject = _bll.FindData("N", null, null, userId, title, memo, rowPerPage, selectPage, sortKey, sortValue);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }



        /// <summary>
        /// My공간 - 다운로드
        /// </summary>
        /// <param name="key">파일 SEQ</param>
        /// <returns></returns>
        [HttpGet("files/{key}")]
        public IActionResult Download(long key, [FromQuery] string inline = "N")
        {
            var fileData = _bll.Get(key);
            try
            {
                string fileName = Path.GetFileName(fileData.FilePath);
                var startIdx = fileName.IndexOf('_');
                var downloadName = fileName.Substring(startIdx+1, fileName.Length - startIdx -1);
                return _fileHelper.DownloadFromPath(downloadName, fileData.FilePath, Response, _fileSystem, inline);
            }
            catch (HttpStatusErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// My공간 - 스트리밍
        /// </summary>
        /// <param name="key"></param>
        /// <param name="direct">Y, N</param>
        /// <returns></returns>
        [HttpGet("streaming/{key}")]
       
        public IActionResult Streaming(long key, string userId, [FromQuery] string direct = "N")
        {
            //range 있을떄는 206 반환하도록
            var fileData = _bll.Get(key);
            try
            {
                string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
                return _fileHelper.StreamingFromPath(fileData.FilePath, userId, remoteIp);
            }
            catch (HttpStatusErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// My공간 - 파형 요청
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("waveform/{key}")]
        public ActionResult<List<float>> GetWaveform(long key, string userId)
        {
            var fileData = _bll.Get(key);
            try
            {
                string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
                return _fileHelper.GetWaveformFromPath(fileData.FilePath, userId, remoteIp);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// my공간- 임시 다운로드
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        [HttpGet("temp-download/{seq}")]
        public IActionResult TempDownload([FromServices] ServiceResolver sr, long seq)
        {
            var fileData = _bll.Get(seq);

            string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
            string userId = HttpContext.Items[Define.USER_ID] as string;
            try
            {
                _fileHelper.TempDownloadFromPath(fileData.FilePath, userId, remoteIp, _fileSystem);
                return Ok();
            }
            catch (HttpStatusErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        /// <summary>
        /// My공간 - 휴지통으로 보내기(삭제)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="seqlist"></param>
        /// <returns></returns>

        [HttpDelete("meta/{userId}/{seqlist}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> DeleteDB(string userId, LongList seqlist)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
                var datalist = _bll.Get(seqlist);
                _bll.MoveRecycleBin(userId, seqlist.ToList<long>());
                result.ResultCode = RESUlT_CODES.SUCCESS;

                List<string> titles = new List<string>();
                foreach(var data in datalist)
                {
                    titles.Add(data.Title);
                }
                _logBll.InfoAsync(HttpContext, userId, $"MY공간 소재 - 휴지통으로 보내기 : {UTF8JsonSerializer.Serialize(titles)}", null);
            }
            catch (Exception ex)
            {
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 휴지통 - 삭제 (물리적인 파일 포함 영구삭제)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="seqlist"></param>
        /// <returns></returns>
        [HttpDelete("recyclebin/{userId}/{seqlist}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> DeletePhysical(string userId, LongList seqlist)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
                var datalist = _bll.Get(seqlist);
                _bll.DeleteRecycleBin(userId, seqlist.ToList<long>());
                result.ResultCode = RESUlT_CODES.SUCCESS;
                foreach (var data in datalist)
                {
                    data.FileToken = null;
                }
                _logBll.InfoAsync(HttpContext, userId, $"MY공간 소재 - 영구삭제", UTF8JsonSerializer.Serialize(datalist));
            }
            catch (Exception ex)
            {
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }


        /// <summary>
        /// 휴지통 - 휴지통 비우기(물리적인파일 포함 전체 영구삭제)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("recyclebin/{userId}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> DeleteAllPhysical(string userId)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
                var dataList = _bll.GetAllByUsedN(userId);
                _bll.DeleteAllRecycleBin(userId);
                result.ResultCode = RESUlT_CODES.SUCCESS;
                foreach (var data in dataList)
                {
                    data.FileToken = null;
                }
                _logBll.InfoAsync(HttpContext, userId, $"MY공간 소재 - 휴지통 비우기", UTF8JsonSerializer.Serialize(dataList));
            }
            catch (Exception ex)
            {
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 휴지통 - 선택 목록 복원
        /// </summary>
        /// <param name="userId">My공간 사용자확장ID</param>
        /// <param name="seqlist">My공간 파일 고유키 목록</param>
        /// <returns></returns>
        [HttpPut("recyclebin/{userId}/recycle/{seqlist}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> RecycleAll(string userId, LongList seqlist)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
                var datalist = _bll.Get(seqlist);
                _bll.RecycleAll(userId, seqlist);
                result.ResultCode = RESUlT_CODES.SUCCESS;
                
                List<string> titles = new List<string>();
                foreach (var data in datalist)
                {
                    titles.Add(data.Title);
                }
                _logBll.InfoAsync(HttpContext, userId, $"MY공간 소재 복원 : {UTF8JsonSerializer.Serialize(titles)}", null);
            }
            catch (Exception ex)
            {
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
    }
}
 