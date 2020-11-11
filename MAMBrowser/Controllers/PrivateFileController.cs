using log4net.Appender;
using log4net.Util;
using MAMBrowser.BLL;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using MAMBrowser.Processor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/products/workspace/private")]
    public class PrivateFileController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AppSettings _appSesstings;
        private readonly PrivateFileDAL _dal;
        private readonly IFileService _fileService;
        public PrivateFileController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSesstings, PrivateFileDAL dal, ServiceResolver sr)
        {
            _dal = dal;
            _appSesstings = appSesstings.Value;
            _fileService = sr("PrivateWorkConnection");
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// My 공간- 파일+메타데이터 등록
        /// </summary>
        /// <param name="userextid">유저확장ID</param>
        /// <param name="file">파일</param>
        /// <param name="metaData">메타데이터</param>
        /// <returns></returns>
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        [HttpPost("files/{userextid}")]
        public DTO_RESULT UploadFile(long userextid, [FromForm] IFormFile file, [ModelBinder(BinderType = typeof(JsonModelBinder))] PrivateFileModel metaData)
        {
            DTO_RESULT result = new DTO_RESULT();
            try
            {
                var success = _dal.Insert(userextid, file, metaData, _fileService.Host);
                result.ResultCode = success != null ? RESUlT_CODES.SUCCESS : RESUlT_CODES.SERVICE_ERROR;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        ///  My 공간 - 메타데이터 편집
        /// </summary>
        /// <param name="userextid">유저확장ID</param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        [HttpPut("meta/{userextid}")]
        public DTO_RESULT UpdateData(long userextid, [FromBody] PrivateFileModel metaData)
        {
            DTO_RESULT result = new DTO_RESULT();
            try
            {
                if (_dal.UpdateData(metaData) > 0)
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }
                else
                {
                    result.ResultCode = RESUlT_CODES.APPLIED_NONE_WARN;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        /// <summary>
        /// VerifyModel
        /// </summary>
        /// <param name="userextid"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        [HttpPost("meta/verify/{userextid}")]
        public DTO_RESULT VerifyModel(long userextid, [FromBody] PrivateFileModel metaData)
        {
            return new DTO_RESULT();
            //return true;
            //if (!_userService.VerifyEmail(email))
            //{
            //    return Json($"Email {email} is already in use.");
            //}

            //return Json(true);
        }


        /// <summary>
        /// My 공간 - 검색
        /// </summary>
        /// <param name="userextid">사용자 확장ID</param>
        /// <param name="title">제목</param>
        /// <param name="memo">메모</param>
        /// <param name="rowPerPage">페이지당 행 개수</param>
        /// <param name="selectPage">선택된 페이지</param>
        /// <param name="sortKey">정렬 키(필드명)</param>
        /// <param name="sortValue">정렬 값(ASC/DESC)</param>
        /// <returns></returns>
        [HttpGet("meta/{userextid}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> FindData(long userextid, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string title, [FromQuery] string memo, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
                result.ResultObject = _dal.FindData("Y", start_dt, end_dt, userextid, title, memo, rowPerPage, selectPage, sortKey, sortValue);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 휴지통 데이터 목록
        /// </summary>
        /// <param name="userextid">사용자 확장ID</param>
        /// <param name="title">제목</param>
        /// <param name="memo">메모</param>
        /// <param name="rowPerPage">페이지당 행 개수</param>
        /// <param name="selectPage">선택된 페이지</param>
        /// <param name="sortKey">정렬 키(필드명)</param>
        /// <param name="sortValue">정렬 값(ASC/DESC)</param>
        /// <returns></returns>
        [HttpGet("recyclebin/{userextid}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> FindRecycleBin(long userextid, [FromQuery] string title, [FromQuery] string memo, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
                result.ResultObject = _dal.FindData("N", null, null, userextid, title, memo, rowPerPage, selectPage, sortKey, sortValue);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
            var fileData = _dal.Get(key);
            string fileName = $"{fileData.Title}{fileData.FileExt}";
            string relativePath = MAMUtility.GetRelativePath(fileData.FilePath);
            var fileExtProvider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!fileExtProvider.TryGetContentType(relativePath, out contentType))
            {
                contentType = "application/octet-stream";
            }
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = inline == "Y" ? true : false
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            var stream = _fileService.GetFileStream(relativePath, 0);
            return File(stream, contentType);
        }
        /// <summary>
        /// My공간 - 스트리밍
        /// </summary>
        /// <param name="key"></param>
        /// <param name="direct">Y, N</param>
        /// <returns></returns>
        [HttpGet("streaming/{key}")]
       
        public IActionResult Streaming(long key, [FromQuery] string direct = "N")
        {
            //range 있을떄는 206 반환하도록
            var fileData = _dal.Get(key);
            string fileName = $"{fileData.Title}{fileData.FileExt}";
            string relativePath = MAMUtility.GetRelativePath(fileData.FilePath);
            if (direct.ToUpper() == "Y")
            {
                return new PushStreamResult(relativePath, fileName, fileData.FileSize, _fileService);
            }
            else
            {
                string contentType;
                var downloadPath = MAMUtility.TempDownloadToLocal(HttpContext.Items["UserId"] as string, _fileService, relativePath, out contentType);
                return PhysicalFile(downloadPath, contentType, true);
            }
        }
        /// <summary>
        /// My공간 - 파형 요청
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("waveform/{key}")]
        public ActionResult<List<float>> GetWaveform(long key)
        {
            try
            {
                var fileData = _dal.Get(key);
                string relativePath = MAMUtility.GetRelativePath(fileData.FilePath);
                return MAMUtility.GetWaveform(HttpContext.Items["UserId"] as string, _fileService, relativePath);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// My공간 - 휴지통으로 보내기(삭제)
        /// </summary>
        /// <param name="userextid"></param>
        /// <param name="seqlist"></param>
        /// <returns></returns>

        [HttpDelete("meta/{userextid}/{seqlist}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> DeleteDB(long userextid, LongList seqlist)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
                if (_dal.DeleteDB(userextid, seqlist))
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                else
                    result.ResultCode = RESUlT_CODES.APPLIED_NONE_WARN;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 휴지통 - 삭제 (물리적인 파일 포함 영구삭제)
        /// </summary>
        /// <param name="userextid"></param>
        /// <param name="seqlist"></param>
        /// <returns></returns>
        [HttpDelete("recyclebin/{userextid}/{seqlist}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> DeletePhysical(long userextid, LongList seqlist)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
                if (_dal.DeletePhysical(userextid, seqlist))
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                else
                    result.ResultCode = RESUlT_CODES.APPLIED_NONE_WARN;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }


        /// <summary>
        /// 휴지통 - 휴지통 비우기(물리적인파일 포함 전체 영구삭제)
        /// </summary>
        /// <param name="userextid"></param>
        /// <returns></returns>
        [HttpDelete("recyclebin/{userextid}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> DeleteAllPhysical(long userextid)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
                if (_dal.DeleteAllPhysical(userextid))
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                else
                    result.ResultCode = RESUlT_CODES.APPLIED_NONE_WARN;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }


        /// <summary>
        /// 휴지통 - 복원
        /// </summary>
        /// <param name="userextid"></param>
        /// <param name="seq"></param>
        /// <returns></returns>
        //[HttpPut("recyclebin/{userextid}/single-recycle/{seq}")]
        //public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> Recycle(long userextid, long seq)
        //{
        //    DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
        //    try
        //    {
        //        PrivateFileBLL bll = new PrivateFileBLL();
        //        if (bll.Recycle(userextid, seq))
        //            result.ResultCode = RESUlT_CODES.SUCCESS;
        //        else
        //            result.ResultCode = RESUlT_CODES.APPLIED_NONE_WARN;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.ErrorMsg = ex.Message;
        //        MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
        //    }
        //    return result;
        //}


        /// <summary>
        /// 휴지통 - 선택 목록 복원
        /// </summary>
        /// <param name="userextid">My공간 사용자확장ID</param>
        /// <param name="seqlist">My공간 파일 고유키 목록</param>
        /// <returns></returns>
        [HttpPut("recyclebin/{userextid}/recycle/{seqlist}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> RecycleAll(long userextid, LongList seqlist)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
                if (_dal.RecycleAll(userextid, seqlist))
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                else
                    result.ResultCode = RESUlT_CODES.APPLIED_NONE_WARN;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
    }
}
