using log4net.Appender;
using log4net.Util;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/products/workspace/private")]
    public class PrivateFileController : ControllerBase
    {
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
                PrivateFileBLL bll = new PrivateFileBLL();
                var success = bll.Upload(userextid,file, metaData);
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
        public DTO_RESULT UpdateData(long userextid, [ModelBinder(BinderType = typeof(JsonModelBinder))] PrivateFileModel metaData)
        {
            DTO_RESULT result = new DTO_RESULT();
            try
            {
                PrivateFileBLL bll = new PrivateFileBLL();
                if (bll.UpdateData(metaData) > 0)
                {
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }
                else {
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
                PrivateFileBLL bll = new PrivateFileBLL();
                result.ResultObject = bll.FineData("Y", start_dt, end_dt, userextid, title, memo, rowPerPage, selectPage, sortKey, sortValue);
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
                PrivateFileBLL bll = new PrivateFileBLL();
                result.ResultObject = bll.FineData("N", null, null, userextid, title, memo, rowPerPage, selectPage, sortKey, sortValue);
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
        /// My공간 - 파일 요청(미리듣기, 다운로드 시 사용)
        /// </summary>
        /// <param name="seq">파일 SEQ</param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet("files/{seq}")]
        public IActionResult GetFile(long seq)
        {
            PrivateFileBLL bll = new PrivateFileBLL();
            var fileData = bll.Get(seq);
            var fileExtProvider = new FileExtensionContentTypeProvider();
            string contentType;
            string fileName = fileData.Title;
          
            contentType = "application/octet-stream";
       
            var downloadStream = MyFtp.Download(fileData.FilePath, 0);
            string tmpPath = @"d:\임시폴더\";
            var filePath = Path.Combine(tmpPath, Path.GetRandomFileName());
            BufferedStream bst = new BufferedStream(downloadStream);
            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            downloadStream.CopyTo(fs);
            
            return PhysicalFile(filePath, contentType);
        }
        [HttpGet("files2/{seq}")]
        public FileResult GetFile2(long seq)
        {
            PrivateFileBLL bll = new PrivateFileBLL();
            var fileData = bll.Get(seq);
            var fileExtProvider = new FileExtensionContentTypeProvider();
            string contentType;
            string fileName = fileData.Title; ;
            if (!fileExtProvider.TryGetContentType(fileData.FilePath, out contentType))
            {
                contentType = "application/octet-stream";
            }

            var downloadStream = MyFtp.Download(fileData.FilePath, 0);
            return File(downloadStream, contentType, true);
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
                PrivateFileBLL bll = new PrivateFileBLL();
                if (bll.DeleteDB(userextid, seqlist))
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
                PrivateFileBLL bll = new PrivateFileBLL();
                if (bll.DeletePhysical(userextid, seqlist))
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
                PrivateFileBLL bll = new PrivateFileBLL();
                if (bll.DeleteAllPhysical(userextid))
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
                PrivateFileBLL bll = new PrivateFileBLL();
                if (bll.RecycleAll(userextid, seqlist))
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
