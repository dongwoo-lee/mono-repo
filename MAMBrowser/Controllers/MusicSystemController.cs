using MAMBrowser.BLL;
using M30.AudioFile.Common;
using MAMBrowser.Entiies;
using MAMBrowser.Foundation;
using MAMBrowser.Helper;
using MAMBrowser.Helpers;
using MAMBrowser.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using M30.AudioFile.Common.DTO;
using M30.AudioFile.Common.Models;
using M30.AudioFile.DAL.WebService;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusicSystemController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AppSettings _appSesstings;
        private readonly MusicWebService _fileService;
        private readonly WebServerFileHelper _fileHelper;
        private readonly ILogger<MusicSystemController> _logger;
        public MusicSystemController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSesstings, MusicWebService fileService, WebServerFileHelper fileHelper, ILogger<MusicSystemController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _appSesstings = appSesstings.Value;
            _fileService = fileService;
            _fileHelper = fileHelper;
            _logger = logger;
        }


        /// <summary>
        /// 음반 기록실 조회
        /// </summary>
        /// <param name="searchType1"></param>
        /// <param name="searchType2"></param>
        /// <param name="gradeType"></param>
        /// <param name="searchText"></param>
        /// <param name="rowPerPage"></param>
        /// <param name="selectPage"></param>
        /// <param name="sortKey"></param>
        /// <param name="sortValue"></param>
        /// <returns></returns>
        [HttpGet("music")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>> FindMusic([FromQuery] int searchType1, [FromQuery] string searchType2, [FromQuery] int gradeType, [FromQuery] string searchText, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>>();
            try
            {
                result.ResultObject = new DTO_RESULT_PAGE_LIST<DTO_SONG>();
                long totalCount = 0;
                if (string.IsNullOrEmpty(searchText))
                    result.ResultObject.Data = new List<DTO_SONG>();
                else
                    result.ResultObject.Data = _fileService.SearchSong((MusicSearchTypes1)searchType1, searchType2, (GradeTypes)gradeType, searchText, rowPerPage, selectPage, out totalCount);

                result.ResultObject.RowPerPage = rowPerPage;
                result.ResultObject.SelectPage = selectPage;
                result.ResultObject.TotalRowCount = totalCount;
                result.ResultCode = RESUlT_CODES.SUCCESS;
                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_EFFECT>> FindEffect([FromQuery] string searchText, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_EFFECT>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_EFFECT>>();
            try
            {
                result.ResultObject = new DTO_RESULT_PAGE_LIST<DTO_EFFECT>();
                long totalCount = 0;
                if (string.IsNullOrEmpty(searchText))
                    result.ResultObject = new DTO_RESULT_PAGE_LIST<DTO_EFFECT>();
                else
                    result.ResultObject.Data = _fileService.SearchEffect(searchText, rowPerPage, selectPage, out totalCount);

                result.ResultObject.RowPerPage = rowPerPage;
                result.ResultObject.SelectPage = selectPage;
                result.ResultObject.TotalRowCount = totalCount;
                result.ResultCode = RESUlT_CODES.SUCCESS;
                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }



        /// <summary>
        /// 음악/효과음 소재 - 다운로드
        /// </summary>
        /// <param name="token">파일 SEQ</param>
        /// <returns></returns>
        [HttpGet("files")]
        public IActionResult MusicDownload([FromQuery] string token, [FromQuery] string downloadName, [FromQuery] string inline = "N")
        {
            var jsonMusicInfo = CommonUtility.ParseToJsonRequestContent(token);
            var musicInfo = CommonUtility.ParseToRequestContent(token);
            var requestInfo = _fileService.GetRequestInfo(musicInfo);
            string contentType = "audio/wav";
            long fileSize;
            var stream = _fileService.GetFileStream(requestInfo[0] as string, Convert.ToInt32(requestInfo[1]), jsonMusicInfo, out fileSize);
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                //FileName = WebUtility.UrlEncode(requestInfo[2] as string),
                FileName = Uri.EscapeDataString(downloadName+".wav"),
                Inline = inline == "Y" ? true : false
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(stream, contentType);
        }
        /// <summary>
        /// 음악/효과음 소재  - 스트리밍
        /// </summary>
        /// <param name="token"></param>
        /// <param name="direct">Y, N</param>
        /// <returns></returns>
        [HttpGet("streaming")]
        public IActionResult MusicStreaming([FromQuery] string token, [FromQuery] string userId, [FromQuery] string direct = "N")
        {
            var decodedFilePath = CommonUtility.GetFilePathFromMusicToken(token);
            string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
            try
            {
                return _fileHelper.StreamingFromPath(decodedFilePath, userId, remoteIp);
            }
            catch (HttpStatusErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }
        /// <summary>
        /// 음악 소재 - 파형 요청
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("waveform")]
        public ActionResult<List<float>> MusicGetWaveform([FromQuery] string token, [FromQuery] string userId)
        {
            var decodedFilePath = CommonUtility.GetFilePathFromMusicToken(token);
            string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
            try
            {
                return _fileHelper.GetWaveformFromPath(decodedFilePath, userId, remoteIp);
            }
            catch (HttpStatusErrorException ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }
        /// <summary>
        /// 음악/효과음 소재 - 임시 다운로드
        /// </summary>
        /// <param name="token">파일 SEQ</param>
        /// <returns></returns>
        [HttpGet("temp-download")]
        public IActionResult MusicTempDownload([FromQuery] string token)
        {
            string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
            string userId = HttpContext.Items[Define.USER_ID] as string;
            _fileService.TempDownloadWavAndEgy(userId, remoteIp, token);
            return Ok();
        }
        [HttpPost("music-to-myspace")]
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> MusicToMyspace([FromQuery] string token, [FromBody] M30_MAM_PRIVATE_SPACE metaData, [FromServices] PrivateFileBll privateBll)
        {
            DTO_RESULT<DTO_RESULT_OBJECT<string>> result = new DTO_RESULT<DTO_RESULT_OBJECT<string>>();
            try
            {
                var jsonMusicInfo = CommonUtility.ParseToJsonRequestContent(token);
                var musicInfo = CommonUtility.ParseToRequestContent(token);
                var requestInfo = _fileService.GetRequestInfo(musicInfo);
                var fileName = requestInfo[2] as string;

                string userId = HttpContext.Items[Define.USER_ID] as string;
                long fileSize;
                using (var stream = _fileService.GetFileStream(requestInfo[0] as string, Convert.ToInt32(requestInfo[1]), jsonMusicInfo, out fileSize))
                {
                    metaData.FILE_SIZE = fileSize;
                    result = privateBll.UploadFile(userId, stream, fileName, metaData);
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
        /// 음악/효과음 소재 - 앨범 이미지 임시 다운로드
        /// </summary>
        /// <param name="token">파일 SEQ</param>
        /// <returns></returns>
        [HttpGet("images/temp-download")]
        public ActionResult<List<string>> AlbumImageTempDownload([FromQuery] string token, [FromQuery] string albumToken)
        {
            string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
            string userId = HttpContext.Items[Define.USER_ID] as string;
            return _fileService.TempImageDownload(userId, remoteIp, token, albumToken);
        }

        /// <summary>
        /// 음악/효과음 소재 - 앨범 이미지 스트리밍
        /// </summary>
        /// <param name="token">파일 SEQ</param>
        /// <returns></returns>
        [HttpGet("images/streaming")]
        public IActionResult ImageStreaming([FromQuery] string fileName, [FromQuery] string userId)
        {
            string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
            try
            {
                //string userId = HttpContext.Items[MAMUtility.USER_ID] as string;  확인필요..
                return _fileHelper.StreamingFromFileName(fileName, userId, remoteIp);
            }
            catch (HttpStatusErrorException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        /// <summary>
        /// 음악 - 가사요청
        /// </summary>
        /// <param name="lyricsSeq"></param>
        /// <returns></returns>
        [HttpGet("lyrics/{lyricsSeq}")]
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> SearchLyrics(string lyricsSeq)
        {
            DTO_RESULT<DTO_RESULT_OBJECT<string>> result = new DTO_RESULT<DTO_RESULT_OBJECT<string>>();
            try
            {
                result.ResultObject = new DTO_RESULT_OBJECT<string>();
                result.ResultObject.Data = _fileService.SearchLyrics(lyricsSeq);
                result.ResultCode = RESUlT_CODES.SUCCESS;
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
