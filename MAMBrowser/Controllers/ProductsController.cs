using Dapper;
using MAMBrowser.BLL;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly ProductsBLL _bll;
        public ProductsController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSesstings, ProductsBLL bll)
        {
            _hostingEnvironment = hostingEnvironment;
            _appSesstings = appSesstings;
            _bll = bll;
        }

        /// <summary>
        /// 프로그램 소재 조회 (페이징 x)
        /// </summary>
        /// <param name="media">매체 : ex)A,C,F,D</param>
        /// <param name="brd_dt">방송일(20200101)</param>
        /// <param name="pgm">프로그램 ID</param>
        /// <param name="editor">제작자 ID</param>
        /// <returns></returns>
        [HttpGet("pgm/{media}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PGM_INFO>> FindPGM(string media, [FromQuery] string brd_dt, [FromQuery] string pgm, [FromQuery] string editor)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PGM_INFO>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PGM_INFO>>();
            try
            {

                result.ResultObject = _bll.FindPGM(media, brd_dt, pgm, editor);
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

                result.ResultObject = _bll.FindSCRSpot(media, start_dt, end_dt, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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
        /// <param name="pgm">사용처 : ex) PM1200NA, PM1900NA, PM1900SA</param>
        /// <param name="editor">제작자 : ex)010502</param>
        /// <param name="reporterName"> 취재인 이름 : ex) 한수연</param>
        /// <param name="name"> 여성, 뉴스</param>
        /// <param name="rowPerPage">16</param>
        /// <param name="selectPage">1</param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("report")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_REPORT>> FindReport([FromQuery] string cate, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string pgm, [FromQuery] string editor, [FromQuery] string reporterName, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_REPORT>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_REPORT>>();
            try
            {

                result.ResultObject = _bll.FindReport(cate, start_dt, end_dt, pgm, editor, reporterName, name, rowPerPage, selectPage, sortKey, sortValue);
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

                result.ResultObject = _bll.FindOldPro(media, cate, type, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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
        /// 음반 기록실 조회
        /// </summary>
        /// <param name="rowPerPage"></param>
        /// <param name="selectPage"></param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("music")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>> FindMusic([FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>>();
            try
            {
                var startNO = 1;
                startNO += (rowPerPage * selectPage) - rowPerPage;
                var lastNO = startNO + rowPerPage;
                string projectRootPath = _hostingEnvironment.ContentRootPath;
                string root = _hostingEnvironment.WebRootPath;
                PhysicalFileProvider pr = new PhysicalFileProvider(projectRootPath);
                var songXml = pr.GetFileInfo("song.xml");
                XmlSerializer xs = new XmlSerializer(typeof(EDTO_MB_RETURN<EDTO_SONG>));
                using (Stream fs = songXml.CreateReadStream())
                {
                    var data = (EDTO_MB_RETURN<EDTO_SONG>)xs.Deserialize(fs);
                    var data2 = data.Section.Data.Select(edto => new DTO_SONG(edto));
                    result.ResultObject = new DTO_RESULT_PAGE_LIST<DTO_SONG>();
                    result.ResultObject.Data = data2.ToList();
                    result.ResultObject.RowPerPage = rowPerPage;
                    result.ResultObject.SelectPage = selectPage;
                    result.ResultObject.TotalRowCount = data.Section.TOTAL_COUNT;
                }
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
        /// 효과음 조회
        /// </summary>
        /// <param name="searchWord"></param>
        /// <param name="rowPerPage"></param>
        /// <param name="selectPage"></param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("effect")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_EFFECT>> FindEffect([FromQuery] string searchWord, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_EFFECT>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_EFFECT>>();
            try
            {
                var startNO = 1;
                startNO += (rowPerPage * selectPage) - rowPerPage;
                var lastNO = startNO + rowPerPage;
                string projectRootPath = _hostingEnvironment.ContentRootPath;
                string root = _hostingEnvironment.WebRootPath;
                PhysicalFileProvider pr = new PhysicalFileProvider(projectRootPath);
                var songXml = pr.GetFileInfo("effect.xml");
                XmlSerializer xs = new XmlSerializer(typeof(EDTO_MB_RETURN<EDTO_EFFECT>));
                using (Stream fs = songXml.CreateReadStream())
                {
                    var data = (EDTO_MB_RETURN<EDTO_EFFECT>)xs.Deserialize(fs);
                    var data2 = data.Section.Data.Select(edto => new DTO_EFFECT(edto));
                    result.ResultObject = new DTO_RESULT_PAGE_LIST<DTO_EFFECT>();
                    result.ResultObject.Data = data2.ToList();
                    result.ResultObject.RowPerPage = rowPerPage;
                    result.ResultObject.SelectPage = selectPage;
                    result.ResultObject.TotalRowCount = data.Section.TOTAL_COUNT;
                }
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

                result.ResultObject = _bll.FindSB("MAIN", media, brd_dt, pgm);
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

                result.ResultObject = _bll.FindSB("SUB", media, brd_dt, pgm);
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

                result.ResultObject = _bll.FindSBContents(brd_dt, sbID);
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

                result.ResultObject = _bll.FindCM(media, brd_dt, cate, pgmName);
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

                result.ResultObject = _bll.FindCMContents(brd_dt, cmgrpid);
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

                result.ResultObject = _bll.FindMcrSpot(media, start_dt, end_dt, spotId, editor, rowPerPage, selectPage, sortKey, sortValue);
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

                result.ResultObject = _bll.FindFiller("MEM_FILLER_PR_VIEW", brd_dt, cate, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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

                result.ResultObject = _bll.FindFiller("MEM_FILLER_MATERIAL_VIEW", brd_dt, cate, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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

                result.ResultObject = _bll.FindFillerTime(media, start_dt, end_dt, cate, status, editor, name, rowPerPage, selectPage, sortKey, sortValue);
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
                //result.ResultObject = _bll.FindFiller("MEM_FILLER_MATERIAL_VIEW", brd_dt, cate, editor, editorName, name, rowPerPage, selectPage, sortKey, sortValue);
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

                result.ResultObject = _bll.FindNewDL(media, schDate, pgmName, sortKey, sortValue);
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
        /// 음악/효과음 소재를 제외한 미리듣기,다운로드
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpGet("files")]
        public FileResult GetFile([FromQuery] string filePath)
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
        [HttpPost("external/files")]
        public FileResult GetExternalFile([FromQuery] string filePath)
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
    }
}
