using Dapper;
using MAMBrowser.BLL;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Entiies;
using MAMBrowser.Foundation;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using MAMBrowser.Processor;
using MAMBrowser.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[CustomAuthorize]
    public class ProductsController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AppSettings _appSesstings;
        private readonly ProductsDAL _dal;
        private readonly IFileService _fileService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSesstings, ProductsDAL dal, ServiceResolver sr, ILogger<ProductsController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _appSesstings = appSesstings.Value;
            _dal = dal;
            _fileService = sr("MirosConnection");
            _logger = logger;
        }

        /// <summary>
        /// 프로그램 소재 조회 
        /// </summary>
        /// <param name="media">매체 : ex)A,C,F,D</param>
        /// <param name="brd_dt">방송일(20200101)</param>
        /// <param name="pgm">프로그램 ID</param>
        /// <param name="editor">제작자 ID</param>
        /// <param name="rowPerPage">16</param>
        /// <param name="selectPage">1</param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("pgm/{media}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PGM_INFO>> FindPGM(string media, [FromQuery] string brd_dt, [FromQuery] string pgm, [FromQuery] string editor, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PGM_INFO>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PGM_INFO>>();
            try
            {

                result.ResultObject = _dal.FindPGM(media, brd_dt, pgm, editor, rowPerPage, selectPage, sortKey, sortValue);
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
        /// 부조 SPOT 소재 조회
        /// </summary>
        /// <param name="media">매체 : A, F, D</param>
        /// <param name="start_dt">20200101</param>
        /// <param name="end_dt">20200701</param>
        /// <param name="editor">사용자 ID: ex) 180988 (최지민)</param>
        /// <param name="name">근로</param>
        /// <param name="rowPerPage">16</param>
        /// <param name="selectPage">1</param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("spot/scr/{media}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SCR_SPOT>> FindSCRSpot(string media, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string pgmName, [FromQuery] string editor, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            //사용자 ID ex) 180988
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SCR_SPOT>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SCR_SPOT>>();
            try
            {

                result.ResultObject = _dal.FindSCRSpot(media, start_dt, end_dt, pgmName, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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
        /// 취재물 소재 조회. (사용처ID와 사용처이름이 동시에 기입될 경우 ID만 검색함)
        /// </summary>
        /// <param name="cate">분류 : ex)NPS-M</param>
        /// <param name="start_dt">시작일 : 20200101</param>
        ///  <param name="end_dt">종료일 : 20200620</param>
        ///  <param name="isMastering">마스터링 여부 : Y / N</param>
        /// <param name="pgmName">사용처 : ex) PM1200NA, PM1900NA, PM1900SA</param>
        /// <param name="editor">제작자 : ex)010502</param>
        /// <param name="reporterName"> 취재인 이름 : ex) 한수연</param>
        /// <param name="name"> 여성, 뉴스</param>
        /// <param name="rowPerPage">16</param>
        /// <param name="selectPage">1</param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("report")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_REPORT>> FindReport([FromQuery] string cate, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string isMastering, [FromQuery] string pgmName, [FromQuery] string editor, [FromQuery] string reporterName, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_REPORT>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_REPORT>>();
            try
            {

                result.ResultObject = _dal.FindReport(cate, start_dt, end_dt, isMastering, pgmName, editor, reporterName, name, rowPerPage, selectPage, sortKey, sortValue);
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
        /// (구)프로소재 조회
        /// </summary>
        /// <param name="media">매체 : ex)A,C,F,D</param>
        /// <param name="cate">분류 : ex) AC00279990, AC00279444,AC00192685 </param>
        /// <param name="start_dt">시작일 : 20200101</param>
        ///  <param name="end_dt">종료일 : 20200620</param>
        /// <param name="type">구분 : ex) Y = 방송중, N=폐지 </param>
        /// <param name="editor"></param>
        /// <param name="name"></param>
        /// <param name="rowPerPage">16</param>
        /// <param name="selectPage">1</param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("old_pro")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRO>> FindOldPro([FromQuery] string media, [FromQuery] string cate, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string type, [FromQuery] string editor, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRO>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRO>>();
            try
            {

                result.ResultObject = _dal.FindOldPro(media, cate, start_dt, end_dt, type, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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
        /// 주조SB 목록 조회 (페이징x)
        /// </summary>
        /// <param name="media">매체 : ex)A,C,F,D</param>
        /// <param name="brd_dt">방송일 : ex) 20200620</param>
        /// <param name="pgm"></param>
        /// <returns></returns>
        [HttpGet("sb/mcr/{media}/{brd_dt}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SB>> FindMcrSB(string media, string brd_dt, [FromQuery] string pgm)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SB>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SB>>();
            try
            {

                result.ResultObject = _dal.FindSB("MAIN", media, brd_dt, pgm);
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
        /// 부조SB 목록 조회 (페이징x)
        /// </summary>
        /// <param name="media">매체 : ex)A,C,F,D</param>
        /// <param name="brd_dt">방송일 : ex) 20200620</param>
        /// <param name="pgm"></param>
        /// <returns></returns>
        [HttpGet("sb/scr/{media}/{brd_dt}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SB>> FindScrSB(string media, string brd_dt, [FromQuery] string pgm)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SB>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SB>>();
            try
            {

                result.ResultObject = _dal.FindSB("SUB", media, brd_dt, pgm);
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
        /// 주조/부조 SB그룹 내 소재 조회 (페이징x)
        /// </summary>
        /// <param name="brd_dt">방송일 : ex) 20200620</param>
        /// <param name="sbID">SB ID : ex) ASG201029</param>
        /// <returns></returns>
        [HttpGet("sb/contents/{brd_dt}/{sbID}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SB_CONTENT>> FindSBContents(string brd_dt, string sbID)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SB_CONTENT>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SB_CONTENT>>();
            try
            {

                result.ResultObject = _dal.FindSBContents(brd_dt, sbID);
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
        /// 광고그룹 목록조회 (페이징x)
        /// </summary>
        /// <param name="media">매체 : ex)A,C,F,D</param>
        /// <param name="brd_dt">방송일 : ex) 20200620</param>
        /// <param name="cate">분류코드</param>
        /// <param name="pgmName">사용처명</param>
        /// <returns></returns>
        [HttpGet("cm/{media}/{brd_dt}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_CM>> FindCM(string media, string brd_dt, [FromQuery] string cate, [FromQuery] string pgmName)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_CM>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_CM>>();
            try
            {

                result.ResultObject = _dal.FindCM(media, brd_dt, cate, pgmName);
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
        /// 광고그룹 내 소재 조회 (페이징x)
        /// </summary>
        /// <param name="brd_dt">방송일 : ex) 20200620</param>
        /// <param name="cmgrpid">광고그룹 ID : </param>
        /// <returns></returns>
        [HttpGet("cm/contents/{brd_dt}/{cmgrpid}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_CM_CONTENT>> FindCMContents(string brd_dt, string cmgrpid)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_CM_CONTENT>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_CM_CONTENT>>();
            try
            {

                result.ResultObject = _dal.FindCMContents(brd_dt, cmgrpid);
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
        /// 주조SPOT 소재 조회
        /// </summary>
        /// <param name="media">매체 : ex)A,C,F,D</param>
        /// <param name="start_dt"></param>
        /// <param name="end_dt"></param>
        /// <param name="spotId">SPOT소재 ID</param>
        /// <param name="editor"></param>
        /// <returns></returns>
        [HttpGet("spot/mcr/{media}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_MCR_SPOT>> FindMcrSpot(string media, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string spotId, [FromQuery] string editor, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_MCR_SPOT>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_MCR_SPOT>>();
            try
            {

                result.ResultObject = _dal.FindMcrSpot(media, start_dt, end_dt, spotId, editor, rowPerPage, selectPage, sortKey, sortValue);
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
        /// 필러(pr) 소재 조회
        /// </summary>
        /// <param name="brd_dt">방송일 : ex) 20200620</param>
        /// <param name="cate"></param>
        /// <param name="editor"></param>
        /// <param name="name"></param>
        /// <param name="rowPerPage"></param>
        /// <param name="selectPage"></param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("filler/pr/{brd_dt}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER>> FindProFiller(string brd_dt, [FromQuery] string cate, [FromQuery] string editor, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER>>();
            try
            {

                result.ResultObject = _dal.FindFiller("M30_VW_FILLER_PR", brd_dt, cate, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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
        /// 필러(일반) 소재 조회
        /// </summary>
        /// <param name="brd_dt">방송일 : ex) 20200620</param>
        /// <param name="cate"></param>
        /// <param name="editor"></param>
        /// <param name="name"></param>
        /// <param name="isAvailable"> 방송 유효일이 오늘기준으로 유효한거만 보기 : Y </param>
        /// <param name="rowPerPage"></param>
        /// <param name="selectPage"></param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("filler/general/{brd_dt}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER>> FindFeneralFiller(string brd_dt, [FromQuery] string cate, [FromQuery] string editor, [FromQuery] string name, [FromQuery] string isAvailable, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER>>();
            try
            {

                result.ResultObject = _dal.FindFiller("M30_VW_FILLER_MATERIAL", brd_dt, cate, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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
        /// 필러(시간) 소재 조회
        /// </summary>
        /// <param name="media">매체 : ex)A,C,F,D</param>
        /// <param name="start_dt"></param>
        /// <param name="end_dt"></param>
        /// <param name="status"></param>
        /// <param name="cate"></param>
        /// <param name="editor"></param>
        /// <param name="name"></param>
        /// <param name="isAvailable"> 방송 종료일이 오늘기준으로 유효한거만 보기 : Y </param>
        /// <param name="rowPerPage"></param>
        /// <param name="selectPage"></param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("filler/time/{media}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER_TIME>> FindTimetoneFiller(string media, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string status, [FromQuery] string cate, [FromQuery] string editor, [FromQuery] string name, [FromQuery] string isAvailable, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER_TIME>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER_TIME>>();
            try
            {

                result.ResultObject = _dal.FindFillerTime(media, start_dt, end_dt, cate, status, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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
        /// 필러(기타) 소재 조회
        /// </summary>
        /// <param name="brd_dt">방송일 : ex) 20200620</param>
        /// <param name="cate"></param>
        /// <param name="editor"></param>
        /// <param name="name"></param>
        /// <param name="rowPerPage"></param>
        /// <param name="selectPage"></param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("filler/etc/{brd_dt}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER>> FindETCFiller(string brd_dt, [FromQuery] string cate, [FromQuery] string editor, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER>>();
            try
            {
                //
                result.ResultObject = _dal.FindFiller("M30_VW_FILLER_ETC", brd_dt, cate, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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
        /// DL3.0 소재 조회 (페이징x)
        /// </summary>
        /// <param name="media">매체코드</param>
        /// <param name="schDate">송출표의 편성일자</param>
        /// <param name="name">녹음명</param>
        /// <returns></returns>
        [HttpGet("dl30/{media}/{schDate}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_DL30>> FindDLArchive(string media, string schDate, [FromQuery] string pgmName, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_DL30>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_DL30>>();
            try
            {

                result.ResultObject = _dal.FineDLArchive(media, schDate, pgmName, sortKey, sortValue);
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
        /// 일반 소재 - 다운로드
        /// </summary>
        /// <param name="seq">파일 SEQ</param>
        /// <returns></returns>
        [HttpGet("files")]
        public IActionResult Download([FromQuery] string token, [FromQuery] string inline = "N")
        {
            try
            {
                return MAMUtility.Download(token, Response, _fileService, inline);
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
        /// 일반소재 - 병합 다운로드 요청
        /// </summary>
        /// <returns></returns>
        [HttpGet("request-concatenate-files")]
        public ActionResult<DTO_RESULT<DTO_RESULT_OBJECT<string>>> ConcatenateDownload([FromQuery] string grpType, [FromQuery] string brd_Dt, [FromQuery] string grpId, [FromQuery] string downloadName, [FromQuery] string inline = "N")
        {
            //grpType : sb or cm
            try
            {
                DTO_RESULT<DTO_RESULT_OBJECT<string>> result = new DTO_RESULT<DTO_RESULT_OBJECT<string>>();
                result.ResultObject = new DTO_RESULT_OBJECT<string>();


                if (string.IsNullOrEmpty(grpType))
                    return BadRequest("grpType is empty");

                List<string> filePathList = new List<string>();
                string decodedFilePath = "";
                if (grpType.ToUpper() == "SB")
                {
                    var sbfiles = FindSBContents(brd_Dt, grpId);
                    foreach (var sbFile in sbfiles.ResultObject.Data)
                    {
                        if (MAMUtility.ValidateMAMToken(sbFile.FileToken, ref decodedFilePath))
                        {
                            filePathList.Add(decodedFilePath);
                            decodedFilePath = "";
                        }
                        else
                            return StatusCode(StatusCodes.Status403Forbidden, "invalid token");
                    }

                }
                else if (grpType.ToUpper() == "CM")
                {
                    var sbfiles = FindCMContents(brd_Dt, grpId);
                    foreach (var sbFile in sbfiles.ResultObject.Data)
                    {
                        if (MAMUtility.ValidateMAMToken(sbFile.FileToken, ref decodedFilePath))
                        {
                            filePathList.Add(decodedFilePath);
                            decodedFilePath = "";
                        }
                        else
                            return StatusCode(StatusCodes.Status403Forbidden, "invalid token");
                    }
                }
                else
                    return BadRequest($"invalid value(grpType : {grpType})");


                if (filePathList.Count <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "소재가 없습니다.");

                
                
                string mergeType = MAMUtility.WAV;
                var firstFileExt = Path.GetExtension(filePathList.First()).ToUpper();
                mergeType = filePathList.All(filePath => Path.GetExtension(filePath).ToUpper() == firstFileExt) ? firstFileExt : mergeType;
                _logger.LogDebug($"mergeType : {mergeType}");
                string downloadFileName = downloadName + mergeType;

                var fileExtProvider = new FileExtensionContentTypeProvider();
                string contentType;
                if (!fileExtProvider.TryGetContentType(firstFileExt, out contentType))
                {
                    contentType = "application/octet-stream";
                }
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = WebUtility.UrlEncode(downloadFileName),
                    Inline = inline == "Y" ? true : false
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());


          
                // 일단 메모리 스트림. 
                // 
                byte[] buffer = new byte[10240];
                string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
                string userId = HttpContext.Items[MAMUtility.USER_ID] as string;
                MAMUtility.InitTempFoler(userId, remoteIp);
                var tempFilePath = MAMUtility.GetTempFilePath(userId, remoteIp, downloadFileName);
                

                using (FileStream outFileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    WaveFileWriter waveFileWriter = null;
                    foreach (var filePath in filePathList)
                    {
                        var ext = Path.GetExtension(filePath).ToUpper();
                        using (var inStream = _fileService.GetFileStream(filePath, 0))
                        {
                            if (mergeType == MAMUtility.WAV)    //wav 또는 mp2포함된 혼합 일경우 wav로 출력
                            {
                                if (waveFileWriter == null) //향후확인 출력파일 한번만...생성자..통과하면 헤더가 자동으로 쓰여짐.
                                {
                                    waveFileWriter = new WaveFileWriter(outFileStream, new WaveFormat(44100, 16, 2));   
                                }

                                using (MemoryStream tempMemoryStream = new MemoryStream())
                                {
                                    //in이 FTP면 library(naudio)로 읽을수 없어서 메모리 스트림에 담음.(향후확인)
                                    inStream.CopyTo(tempMemoryStream);
                                    tempMemoryStream.Position = 0;

                                    if (ext == MAMUtility.WAV)//일단 wav포맷은 신경안써도될듯. 
                                    {

                                        using (WaveFileReader reader = new WaveFileReader(tempMemoryStream))
                                        {
                                            //if (!reader.WaveFormat.Equals(waveFileWriter.WaveFormat))
                                            //{
                                            //    throw new InvalidOperationException("Can't concatenate WAV Files that don't share the same format");
                                            //}
                                            int read;
                                            while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                                            {
                                                waveFileWriter.Write(buffer, 0, read);
                                            }
                                            waveFileWriter.Flush();
                                        }
                                    }
                                    else if (ext == MAMUtility.MP2)
                                    {
                                        
                                        AudioEngine.ConvertMp2ToWav(tempMemoryStream, waveFileWriter);
                                    }
                                }
                            }
                            else if (mergeType == MAMUtility.MP2)   //모두  mp2인경우
                            {
                                inStream.CopyTo(outFileStream);
                            }
                        }
                    }
                }
                result.ResultObject.Data = downloadFileName;
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 일반소재 - 병합 다운로드
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="downloadName"> </param>
        /// /// <param name="inline"> </param>
        /// <returns></returns>
        [HttpGet("concatenate-files")]
        public IActionResult ConcatenateDownload([FromQuery] string userId, [FromQuery] string downloadName, [FromQuery] string inline = "N")
        {
            try
            {
                string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
                var tempFilePath = MAMUtility.GetTempFilePath(userId, remoteIp, downloadName);
                var fileExtProvider = new FileExtensionContentTypeProvider();
                string contentType;
                if (!fileExtProvider.TryGetContentType(downloadName, out contentType))
                {
                    contentType = "application/octet-stream";
                }
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = WebUtility.UrlEncode(downloadName),
                    Inline = inline == "Y" ? true : false
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());

                return PhysicalFile(tempFilePath, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 일반 소재  - 스트리밍
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="direct">Y, N</param>
        /// <returns></returns>
        [HttpGet("streaming")]
        public IActionResult Streaming([FromQuery] string token, [FromQuery] string userId, [FromQuery] string direct = "N")
        {
            string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
            try
            {
                return MAMUtility.Streaming(token, userId, remoteIp);
            }
            catch (HttpStatusErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }
        /// <summary>
        /// 일반 소재 - 파형 요청
        /// </summary>
        /// <param name="token"></param>
        /// /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("waveform")]
        public ActionResult<List<float>> GetWaveform([FromQuery] string token, [FromQuery] string userId)
        {
            string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
            try
            {
                return MAMUtility.GetWaveform(token, userId, remoteIp);
            }
            catch (HttpStatusErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        /// <summary>
        /// 일반 소재 - 임시 다운로드
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("temp-download")]
        public IActionResult TempDownload([FromQuery] string token)
        {
            string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
            string userId = HttpContext.Items[MAMUtility.USER_ID] as string;
            try
            {
                MAMUtility.TempDownload(token, userId, remoteIp, _fileService);
                return Ok();
            }
            catch (HttpStatusErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }


        /// <summary>
        /// 일반 소재 - My공간으로 복사
        /// </summary>
        /// <param name="seq">파일 SEQ</param>
        /// <returns></returns>
        [HttpGet("product-to-myspace")]
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> FileToMySpace([FromQuery] string token, [FromServices] PrivateFileBLL privateBll)
        {
            DTO_RESULT<DTO_RESULT_OBJECT<string>> result = new DTO_RESULT<DTO_RESULT_OBJECT<string>>();
            try
            {
                string filePath = "";
                if (MAMUtility.ValidateMAMToken(token, ref filePath))
                {
                    var fileName = Path.GetFileName(filePath);
                    string userId = HttpContext.Items[MAMUtility.USER_ID] as string;
                    using (var stream = _fileService.GetFileStream(filePath, 0))
                    {
                        PrivateFileModel model = new PrivateFileModel();
                        model.TITLE = Path.GetFileNameWithoutExtension(fileName);
                        model.MEMO = model.TITLE;
                        model.FILE_SIZE = stream.Length;

                        privateBll.UploadFile(userId, stream, fileName, model);
                        result.ResultCode = RESUlT_CODES.SUCCESS;
                    }
                }
                else
                    throw new HttpStatusErrorException(HttpStatusCode.Forbidden, "invalid token");
            }
            catch (Exception ex)
            {
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                result.ErrorMsg = ex.Message;
            }

            return result;
        }



        /// <summary>
        /// DL30 소재 - 다운로드
        /// </summary>
        /// <param name="seq">파일 SEQ</param>
        /// <param name="inline">inline 여부</param>
        /// <returns></returns>
        [HttpGet("dl30-files/{seq}")]
        public IActionResult Dl30Download([FromServices] ServiceResolver sr, long seq, [FromQuery] string fileType = "WAV", [FromQuery] string inline = "N")
        {
            var fileService = sr("DLArchiveConnection");
            var fileData = _dal.GetDLArchive(seq);
            try
            {
                return MAMUtility.DownloadFromPath(fileData.FilePath, Response, fileService, inline);
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
        /// DL30 소재  - 스트리밍
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="direct">Y, N</param>
        /// <returns></returns>
        [HttpGet("dl30-streaming/{seq}")]
        public IActionResult Dl30Streaming([FromServices] ServiceResolver sr, long seq, string userId, [FromQuery] string fileType = "WAV", [FromQuery] string direct = "N")
        {
            var fileService = sr("DLArchiveConnection");
            var fileData = _dal.GetDLArchive(seq);
            try
            {
                string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
                return MAMUtility.StreamingFromPath(fileData.FilePath, userId, remoteIp);
            }
            catch (HttpStatusErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }
        /// <summary>
        /// DL30 소재 - 파형 요청
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        [HttpGet("dl30-waveform/{seq}")]
        public ActionResult<List<float>> GetDl30Waveform([FromServices] ServiceResolver sr, long seq, [FromQuery] string userId)
        {
            var fileData = _dal.GetDLArchive(seq);
            string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
            return MAMUtility.GetWaveformFromPath(fileData.FilePath,  userId, remoteIp);
        }
        /// <summary>
        /// DL30 소재 - 임시 다운로드
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("dl30-temp-download/{seq}")]
        public IActionResult TempDl30Download([FromServices] ServiceResolver sr, long seq)
        {
            var fileService = sr("DLArchiveConnection");
            var fileData = _dal.GetDLArchive(seq);

            string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
            string userId = HttpContext.Items[MAMUtility.USER_ID] as string;
            try
            {
                MAMUtility.TempDownloadFromPath(fileData.FilePath, userId, remoteIp, fileService);
                return Ok();
            }
            catch (HttpStatusErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }
        /// <summary>
        /// DL30 소재 - My공간으로 복사
        /// </summary>
        /// <param name="seq">파일 SEQ</param>
        /// <returns></returns>
        [HttpGet("dl30-to-myspace/{seq}")]
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> Dl30FileToMySpace([FromServices] ServiceResolver sr, long seq, [FromServices] PrivateFileBLL privateBll)
        {
            DTO_RESULT<DTO_RESULT_OBJECT<string>> result = new DTO_RESULT<DTO_RESULT_OBJECT<string>>();
            try
            {
                var fileService = sr("DLArchiveConnection");
                var fileData = _dal.GetDLArchive(seq);
                var fileName = Path.GetFileName(fileData.FilePath);
                string userId = HttpContext.Items[MAMUtility.USER_ID] as string;
                using (var stream = fileService.GetFileStream(fileData.FilePath, 0))
                {
                    PrivateFileModel model = new PrivateFileModel();
                    model.TITLE = fileData.RecName;
                    model.MEMO = fileData.RecName;
                    model.FILE_SIZE = fileData.FileSize;

                    privateBll.UploadFile(userId, stream, fileName, model);
                    result.ResultCode = RESUlT_CODES.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                result.ErrorMsg = ex.Message;
            }

            return result;
        }

    }
}
