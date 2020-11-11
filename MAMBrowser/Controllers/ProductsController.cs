using Dapper;
using MAMBrowser.BLL;
using MAMBrowser.DTO;
using MAMBrowser.Entiies;
using MAMBrowser.Helpers;
using MAMBrowser.Processor;
using MAMBrowser.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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

        public ProductsController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSesstings, ProductsDAL dal, ServiceResolver sr)
        {
            _hostingEnvironment = hostingEnvironment;
            _appSesstings = appSesstings.Value;
            _dal = dal;
            _fileService = sr("MirosConnection");
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
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SCR_SPOT>> FindSCRSpot(string media, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string editor, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            //사용자 ID ex) 180988
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SCR_SPOT>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SCR_SPOT>>();
            try
            {

                result.ResultObject = _dal.FindSCRSpot(media, start_dt, end_dt, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_REPORT>> FindReport([FromQuery] string cate, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string pgmName, [FromQuery] string editor, [FromQuery] string reporterName, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_REPORT>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_REPORT>>();
            try
            {

                result.ResultObject = _dal.FindReport(cate, start_dt, end_dt, pgmName, editor, reporterName, name, rowPerPage, selectPage, sortKey, sortValue);
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
        /// <param name="type">구분 : ex) Y = 방송중, N=폐지 </param>
        /// <param name="editor"></param>
        /// <param name="name"></param>
        /// <param name="rowPerPage">16</param>
        /// <param name="selectPage">1</param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("old_pro")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRO>> FindOldPro([FromQuery] string media, [FromQuery] string cate, [FromQuery] string type, [FromQuery] string editor, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRO>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRO>>();
            try
            {

                result.ResultObject = _dal.FindOldPro(media, cate, type, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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

                result.ResultObject = _dal.FindFiller("MEM_FILLER_PR_VIEW", brd_dt, cate, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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
        /// <param name="rowPerPage"></param>
        /// <param name="selectPage"></param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("filler/general/{brd_dt}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER>> FindFeneralFiller(string brd_dt, [FromQuery] string cate, [FromQuery] string editor, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER>>();
            try
            {

                result.ResultObject = _dal.FindFiller("MEM_FILLER_MATERIAL_VIEW", brd_dt, cate, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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
        /// <param name="rowPerPage"></param>
        /// <param name="selectPage"></param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("filler/time/{media}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_FILLER_TIME>> FindTimetoneFiller(string media, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string status, [FromQuery] string cate, [FromQuery] string editor, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
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
                //result.ResultObject = _dal.FindFiller("MEM_FILLER_MATERIAL_VIEW", brd_dt, cate, editor, editorName, name, rowPerPage, selectPage, sortKey, sortValue);
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
                string filePath = "";
                if (MAMUtility.ValidateMAMToken(token, ref filePath))
                {
                    string fileName = Path.GetFileName(filePath);
                    var fileExtProvider = new FileExtensionContentTypeProvider();
                    string contentType;
                    if (!fileExtProvider.TryGetContentType(filePath, out contentType))
                    {
                        contentType = "application/octet-stream";
                    }
                    var stream = _fileService.GetFileStream(filePath, 0);
                    System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = fileName,
                        Inline = inline == "Y" ? true : false
                    };
                    Response.Headers.Add("Content-Disposition", cd.ToString());
                    return File(stream, contentType);
                }
                else
                    return StatusCode((int)HttpStatusCode.Forbidden, "invalid token");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }
        /// <summary>
        /// 일반소재 - 병합 다운로드
        /// </summary>
        /// <param name="seq">파일 SEQ</param>
        /// <returns></returns>
        [HttpGet("concatenate-files")]
        public IActionResult ConcatenateDownload([FromQuery] StringList tokenList, [FromQuery] string inline = "N")
        {
            try
            {
                List<string> filePathList = new List<string>();
                foreach (var token in tokenList)
                {
                    string filePath = "";
                    if (MAMUtility.ValidateMAMToken(token, ref filePath))
                    {
                        filePathList.Add(filePath);
                    }
                    else
                        return StatusCode((int)HttpStatusCode.Forbidden, "invalid token");
                }
                var firstFilePath = filePathList.First();
                string fileName = Path.GetFileName(firstFilePath);
                var fileExtProvider = new FileExtensionContentTypeProvider();
                string contentType;
                if (!fileExtProvider.TryGetContentType(firstFilePath, out contentType))
                {
                    contentType = "application/octet-stream";
                }


                byte[] buffer = new byte[1024];
                MemoryStream outStream = new MemoryStream();
                WaveFileWriter waveFileWriter = new WaveFileWriter(outStream, new WaveFormat(44100, 16, 2));

                foreach (var filePath in filePathList)
                {
                    var stream = _fileService.GetFileStream(filePath, 0);
                    System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = fileName,
                        Inline = inline == "Y" ? true : false
                    };
                    Response.Headers.Add("Content-Disposition", cd.ToString());


                    MemoryStream inStream = new MemoryStream();
                    stream.CopyTo(inStream);
                    using (WaveFileReader reader = new WaveFileReader(inStream))
                    {
                        if (!reader.WaveFormat.Equals(waveFileWriter.WaveFormat))
                        {
                            waveFileWriter.Dispose();
                            throw new InvalidOperationException("Can't concatenate WAV Files that don't share the same format");
                        }

                        int read;
                        while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            waveFileWriter.Write(buffer, 0, read);
                        }
                    }

                }
                return File(outStream, contentType);
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
        public IActionResult Streaming([FromQuery] string token, [FromQuery] string direct = "N")
        {
            string filePath = "";
            if (MAMUtility.ValidateMAMToken(token, ref filePath))
            {
                string fileName = Path.GetFileName(filePath);
                if (direct.ToUpper() == "Y")
                {
                    int fileSize = 0;
                    return new PushStreamResult(filePath, fileName, fileSize, _fileService);
                }
                else
                {
                    string contentType;
                    var downloadPath = MAMUtility.TempDownloadToLocal(HttpContext.Items["UserId"] as string, _fileService, filePath, out contentType);
                    return PhysicalFile(downloadPath, contentType, true);
                }
            }
            else
                return StatusCode((int)HttpStatusCode.Forbidden, "invalid token");
        }
        /// <summary>
        /// 일반 소재 - 파형 요청
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        [HttpGet("waveform")]
        public ActionResult<List<float>> GetWaveform([FromQuery] string token)
        {
            string filePath = "";
            if (MAMUtility.ValidateMAMToken(token, ref filePath))
            {
                string fileName = Path.GetFileName(filePath);
                //var fileData = _dal.Get(seq);
                return MAMUtility.GetWaveform(HttpContext.Items["UserId"] as string, _fileService, filePath);
            }
            else
                return StatusCode((int)HttpStatusCode.Forbidden, "invalid token");
        }


        /// <summary>
        /// 일반 소재 - 다운로드
        /// </summary>
        /// <param name="seq">파일 SEQ</param>
        /// <param name="inline">inline 여부</param>
        /// <returns></returns>
        [HttpGet("dl30/files/{seq}")]
        public IActionResult Dl30Download([FromServices] ServiceResolver sr, long seq, [FromQuery] string fileType = "WAV", [FromQuery] string inline = "N")
        {
            var fileService = sr("DLArchiveConnection");
            var fileData = _dal.GetDLArchive(seq);
            string relativePath = MAMUtility.GetRelativePath(fileData.FilePath);
            string fileName = $"{fileData.SourceID}.{fileType}";
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
            var stream = fileService.GetFileStream(relativePath, 0);
            return File(stream, contentType);
        }
        /// <summary>
        /// 일반 소재  - 스트리밍
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="direct">Y, N</param>
        /// <returns></returns>
        [HttpGet("dl30/streaming/{seq}")]
        public IActionResult Dl30Streaming([FromServices] ServiceResolver sr, long seq, [FromQuery] string fileType = "WAV", [FromQuery] string direct = "N")
        {
            var fileService = sr("DLArchiveConnection");
            //range 있을떄는 206 반환하도록
            var fileData = _dal.GetDLArchive(seq);
            string relativePath = MAMUtility.GetRelativePath(fileData.FilePath);
            string fileName = $"{fileData.SourceID}.{fileType}";

            if (direct.ToUpper() == "Y")
            {
                return new PushStreamResult(relativePath, fileName, fileData.FileSize, fileService);
            }
            else
            {
                string contentType;
                var downloadPath = MAMUtility.TempDownloadToLocal(HttpContext.Items["UserId"] as string, fileService, relativePath, out contentType);
                return PhysicalFile(downloadPath, contentType, true);
            }
        }
        /// <summary>
        /// 일반 소재 - 파형 요청
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        [HttpGet("dl30/waveform/{seq}")]
        public List<float> GetDl30Waveform([FromServices] ServiceResolver sr, long seq)
        {
            var fileService = sr("DLArchiveConnection");
            var fileData = _dal.GetDLArchive(seq);
            string relativePath = MAMUtility.GetRelativePath(fileData.FilePath);
            return MAMUtility.GetWaveform(HttpContext.Items["UserId"] as string, fileService, relativePath);
        }
    }
}
