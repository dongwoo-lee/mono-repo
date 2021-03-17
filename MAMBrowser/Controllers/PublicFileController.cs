using MAMBrowser.BLL;
using MAMBrowser.Common;
using MAMBrowser.DTO;
using MAMBrowser.Foundation;
using MAMBrowser.Helper;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/products/workspace/public")]
    public class PublicFileController : ControllerBase
    {
        private readonly IFileProtocol _fileService;
        private readonly IOptions<AppSettings> _appSesstings;
        private readonly PublicFileBll _bll;
        private readonly WebServerFileHelper _fileHelper;
        private readonly HttpContextDBLogger _logBll;

        public PublicFileController(IOptions<AppSettings> appSesstings, PublicFileBll bll, HttpContextDBLogger logger, ServiceResolver sr, WebServerFileHelper fileHelper)
        {
            _appSesstings = appSesstings;
            _bll = bll;
            _logBll = logger;
            _fileService = sr("PublicWorkConnection");
            _fileHelper = fileHelper;
        }
        /// <summary>
        /// 공유소재 -  파일+메타데이터 등록
        /// </summary>
        /// <param name="userId">유저확장ID</param>
        /// <param name="file">파일</param>
        /// <param name="metaData">메타데이터</param>
        /// <returns></returns>
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        [HttpPost("files/{userId}")]
        public DTO_RESULT UploadFile(string userId, [FromForm] IFormFile file, [ModelBinder(BinderType = typeof(JsonModelBinder))] M30_MAM_PUBLIC_SPACE metaData)
        {
            DTO_RESULT result = new DTO_RESULT();
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    metaData.FILE_SIZE = file.Length;
                    var dto = _bll.UploadFile(userId, stream, file.FileName, metaData);
                    result.ResultCode = dto != null ? RESUlT_CODES.SUCCESS : RESUlT_CODES.SERVICE_ERROR;
                    if (result.ResultCode == RESUlT_CODES.SUCCESS)
                    {
                        _logBll.InfoAsync(HttpContext, userId, $"공유 소재 - 파일 업로드", UTF8JsonSerializer.Serialize(metaData));
                    }
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
        ///  공유소재 -  메타데이터 편집
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        [HttpPut("meta/{userId}")]
        public DTO_RESULT UpdateData(string userId, [FromBody] M30_MAM_PUBLIC_SPACE metaData)
        {
            DTO_RESULT result = new DTO_RESULT();
            try
            {
                if (_bll.UpdateData(metaData.SEQ, metaData) > 0)
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                    _logBll.InfoAsync(HttpContext, userId, "공유 소재 - 메타데이터 편집", UTF8JsonSerializer.Serialize(metaData));
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
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> VerifyModel(string userId, [FromBody] M30_MAM_PUBLIC_SPACE metaData)
        {
            return _bll.VerifyModel(userId, metaData, metaData.FILE_PATH);
        }


        /// <summary>
        /// 공유소재 -  검색
        /// </summary>
        /// <param name="userId">사용자 확장ID</param>
        /// <param name="mediaCd">공유소재 매체</param>
        /// <param name="cateCd">공유소재 소분류</param>
        /// <param name="start_dt">검색시작일</param>
        /// <param name="end_dt">검색종료일</param>
        /// <param name="title">제목</param>
        /// <param name="memo">메모</param>
        /// <param name="rowPerPage">페이지당 행 개수</param>
        /// <param name="selectPage">선택된 페이지</param>
        /// <param name="sortKey">정렬 키(필드명)</param>
        /// <param name="sortValue">정렬 값(ASC/DESC)</param>
        /// <returns></returns>
        [HttpGet("meta/{mediaCd}/{cateCd}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PUBLIC_FILE>> FindData(string mediaCd, string cateCd, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string userId, [FromQuery] string title, [FromQuery] string memo, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PUBLIC_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PUBLIC_FILE>>();
            try
            {
                result.ResultObject = _bll.FineData(mediaCd, cateCd, start_dt, end_dt, userId, title, memo, rowPerPage, selectPage, sortKey, sortValue);
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
        /// 공유소재 - 다운로드
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
                var downloadName = fileName.Substring(startIdx + 1, fileName.Length - startIdx - 1);
                return _fileHelper.DownloadFromPath(downloadName, fileData.FilePath, Response, _fileService, inline);
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
        /// 공유소재 - 스트리밍
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
        /// 공유소재 - 파형 요청
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
        /// 공유소재- 임시 다운로드
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
                _fileHelper.TempDownloadFromPath(fileData.FilePath, userId, remoteIp, _fileService);
                return Ok();
            }
            catch (HttpStatusErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }
        [HttpPost("public-to-myspace/{key}")]
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> PublicFileToMyspace(long key, [FromBody] M30_MAM_PRIVATE_SPACE metaData, [FromServices]PrivateFileBll privateBll)
        {
            DTO_RESULT<DTO_RESULT_OBJECT<string>> result = new DTO_RESULT<DTO_RESULT_OBJECT<string>>();
            try
            {
                var fileData = _bll.Get(key);
                var fileName = Path.GetFileName(fileData.FilePath);
                string userId = HttpContext.Items[Define.USER_ID] as string;
                //using (var stream = _fileService.GetFileStream(fileData.FilePath, 0))
                //{
                //    metaData.FILE_SIZE = fileData.FileSize;
                //    result = privateBll.UploadFile(userId, stream, fileName, metaData);
                //}

                using (MemoryStream ms = new MemoryStream())
                {
                    using (var stream = _fileService.GetFileStream(fileData.FilePath, 0))
                    {
                        stream.CopyTo(ms);
                    }
                    ms.Position = 0;
                    metaData.FILE_SIZE = fileData.FileSize;
                    result = privateBll.UploadFile(userId, ms, fileName, metaData);
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                result.ErrorMsg = ex.Message;
            }

            return result;
        }



        /// <summary>
        /// 공유소재 - 삭제
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        [HttpDelete("meta/{seq}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> DeleteDB(long seq)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
                var data = _bll.Get(seq);
                data.FileToken = null;
                if (_bll.Delete(seq))
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                    string userId = HttpContext.Items[Define.USER_ID] as string;
                    _logBll.InfoAsync(HttpContext, userId, $"공유 소재 - 삭제", UTF8JsonSerializer.Serialize(data));
                }
                else
                    result.ResultCode = RESUlT_CODES.APPLIED_NONE_WARN;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
    }
}
