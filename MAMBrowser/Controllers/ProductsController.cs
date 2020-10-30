using Dapper;
using MAMBrowser.BLL;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using MAMBrowser.Processor;
using MAMBrowser.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Xml.Serialization;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<AppSettings> _appSesstings;
        private readonly ProductsDAL _dal;
        private readonly IFileService _fileService;
        
        public ProductsController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSesstings, ProductsDAL dal, ServiceResolver sr)
        {
            _hostingEnvironment = hostingEnvironment;
            _appSesstings = appSesstings;
            _dal = dal;
            _fileService = sr("MirosConnection");
        }

        /// <summary>
        /// 프로그램 소재 조회 (페이징 x)
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
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_DL30>> FindNewDL(string media, string schDate, [FromQuery] string pgmName, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_DL30>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_DL30>>();
            try
            {

                result.ResultObject = _dal.FindNewDL(media, schDate, pgmName, sortKey, sortValue);
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
        /// 소재 다운로드
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        [HttpPost("files")]
        public IActionResult Download([FromBody] string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            var fileExtProvider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!fileExtProvider.TryGetContentType(filePath, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var stream = _fileService.GetFileStream(filePath, 0);
            return File(stream, contentType, fileName);
        }
        /// <summary>
        /// 소재 스트리밍2
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        [HttpPost("streaming/indirect")]
        public IActionResult IndirectStreaming([FromBody] string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            var fileExtProvider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!fileExtProvider.TryGetContentType(filePath, out contentType))
            {
                contentType = "application/octet-stream";
            }
           
            string newFilePath = @$"c:\tmpdownload\{Path.GetRandomFileName()}";
            _fileService.DownloadFile(filePath, newFilePath);
            return File(newFilePath, contentType, fileName, true);
        }
        /// <summary>
        /// 소재 스트리밍
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        [HttpPost("streaming/direct")]
        public IActionResult Streaming([FromBody] string filePath)
        {
            return null;
        }
        /// <summary>
        /// 소재 파형
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        [HttpPost("waveform")]
        public List<float> GetWaveform([FromBody] string filePath)
        {
            string waveFileName = Path.GetFileName(filePath);
            string waveformFileName = $"{Path.GetFileNameWithoutExtension(filePath)}.egy";
            string waveformFilePath = filePath.Replace(waveFileName, waveformFileName);

            if (_fileService.ExistFile(waveformFilePath))
            {
                using (var downloadStream = _fileService.GetFileStream(waveformFilePath, 0))
                {
                    return AudioEngine.GetVolumeFromEgy(downloadStream);
                }
            }
            else
            {
                var inputStream = _fileService.GetFileStream(filePath, 0);
                WaveFileReader reader = new WaveFileReader(inputStream);
                var data = AudioEngine.GetVolumeFromWav(reader, 2);
                inputStream.Close();
                return data;
            }
        }





        /// <summary>
        /// 음반 기록실 조회
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="gradeType"></param>
        /// <param name="searchText"></param>
        /// <param name="rowPerPage"></param>
        /// <param name="selectPage"></param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("music")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>> FindMusic([FromServices] ServiceResolver sr, [FromQuery] int searchType, [FromQuery] int gradeType, [FromQuery] string searchText, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>>();
            try
            {
                var musicService = sr("MusicConnection") as MusicService;
                long totalCount;
                result.ResultObject.Data = musicService.SearchSong((SearchTypes)searchType, (GradeTypes)gradeType, searchText, rowPerPage, selectPage, out totalCount);
                result.ResultObject.RowPerPage = rowPerPage;
                result.ResultObject.SelectPage = selectPage;
                result.ResultObject.TotalRowCount = totalCount;
                result.ResultCode = RESUlT_CODES.SUCCESS;
                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 효과음 조회
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="rowPerPage"></param>
        /// <param name="selectPage"></param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("effect")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_EFFECT>> FindEffect([FromServices] MusicService musicService, [FromQuery] string searchText, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_EFFECT>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_EFFECT>>();
            try
            {
                long totalCount;
                result.ResultObject.Data = musicService.SearchLyrics(searchText, rowPerPage, selectPage, out totalCount);
                result.ResultObject.RowPerPage = rowPerPage;
                result.ResultObject.SelectPage = selectPage;
                result.ResultObject.TotalRowCount = totalCount;
                result.ResultCode = RESUlT_CODES.SUCCESS;
                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }



        /// <summary>
        /// 음악/효과음 소재 wav, egy 파일 다운로드
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpGet("music/files")]
        public FileResult GetMusicFileDownload([FromQuery] string filePath)
        {
            //string directoryPath = @"c:\임시파일";
            //string filePath = Path.Combine(directoryPath, fileID);
            //IFileProvider provider = new PhysicalFileProvider(directoryPath);
            //IFileInfo fileInfo = provider.GetFileInfo(fileID);
            //var readStream = fileInfo.CreateReadStream();

            //var fileExtProvider = new FileExtensionContentTypeProvider();
            //string contentType;
            //if (!fileExtProvider.TryGetContentType(filePath, out contentType))
            //{
            //    contentType = "application/octet-stream";
            //}

            //return File(readStream, contentType, fileID);
            return null;
        }
        /// <summary>
        /// 음악/효과음 소재 미리듣기/다운로드 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpGet("music/streaming/indirect")]
        public FileResult MusicFileStreaming([FromQuery] string filePath)
        {
            //string directoryPath = @"c:\임시파일";
            //string filePath = Path.Combine(directoryPath, fileID);
            //IFileProvider provider = new PhysicalFileProvider(directoryPath);
            //IFileInfo fileInfo = provider.GetFileInfo(fileID);
            //var readStream = fileInfo.CreateReadStream();

            //var fileExtProvider = new FileExtensionContentTypeProvider();
            //string contentType;
            //if (!fileExtProvider.TryGetContentType(filePath, out contentType))
            //{
            //    contentType = "application/octet-stream";
            //}

            //return File(readStream, contentType, fileID);
            return null;
        }
        /// <summary>
        /// 앨범 이미지 목록 반환
        /// </summary>
        /// <param name="filaPath"></param>
        /// <returns></returns>
        [HttpGet("music/albums/images")]
        public IActionResult GetAlbumImage([FromQuery] string filaPath)
        {
            //1. 앨범 이미지 목록 받아오기.
            //2. 앨범 이미지 스트리밍 서비스 받기.
            //3. 
            return null;
        }
        /// <summary>
        /// 앨범 이미지 목록 반환
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpGet("music/albums/images-list")]
        public IActionResult GetAlbumImageList([FromQuery] string filePath)
        {
            //1. 앨범이미지 fullPath에서 상대경로를 일단 저장해놈
            //2. 정보컨텐츠부서로부터 이미지파일 이름 목록을 서비스 받음.(파라매터는 파일 풀 경로)
            //3. 파일이름을 반환받으면, 상대경로를 전부 붙여서 프론트로 전달.(암호화해서;)

            return null;
        }
    }
}
