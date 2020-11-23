﻿using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using MAMBrowser.Processor;
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
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/products/workspace/public")]
    public class PublicFileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IOptions<AppSettings> _appSesstings;
        private readonly PublicFileDAL _dal;
        public PublicFileController(IOptions<AppSettings> appSesstings, PublicFileDAL dal, ServiceResolver sr)
        {
            _appSesstings = appSesstings;
            _dal = dal;
            _fileService = sr("PublicWorkConnection");
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
        public DTO_RESULT UploadFile(string userId, [FromForm] IFormFile file, [ModelBinder(BinderType = typeof(JsonModelBinder))] PublicFileModel metaData)
        {
            DTO_RESULT result = new DTO_RESULT();
            try
            {
                var success = _dal.Insert(userId, file, metaData, _fileService.Host);
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
        ///  공유소재 -  메타데이터 편집
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        [HttpPut("meta/{seq}")]
        public DTO_RESULT UpdateData(long seq, [FromBody] PublicFileModel metaData)
        {
            DTO_RESULT result = new DTO_RESULT();
            try
            {
                if (_dal.UpdateData(seq, metaData) > 0)
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
                result.ResultObject = _dal.FineData(mediaCd, cateCd, start_dt, end_dt, userId, title, memo, rowPerPage, selectPage, sortKey, sortValue);
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
        /// 공유소재 - 다운로드
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
        /// 공유소재 - 스트리밍
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
        /// 공유소재 - 파형 요청
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("waveform/{key}")]
        public List<float> GetWaveform(long key)
        {
            var fileData = _dal.Get(key);
            string relativePath = MAMUtility.GetRelativePath(fileData.FilePath);
            return MAMUtility.GetWaveform(HttpContext.Items["UserId"] as string, _fileService, relativePath);
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
                if (_dal.DeletePhysical(seq))
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
