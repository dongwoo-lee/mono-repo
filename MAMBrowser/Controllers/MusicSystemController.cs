using MAMBrowser.BLL;
using MAMBrowser.Common;
using MAMBrowser.DTO;
using MAMBrowser.Entiies;
using MAMBrowser.Foundation;
using MAMBrowser.Helper;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using MAMBrowser.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
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

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusicSystemController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AppSettings _appSesstings;
        private readonly MusicService _fileProtocol;
        private readonly WebServerFileHelper _fileHelper;
        public MusicSystemController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSesstings, MusicService fileService, WebServerFileHelper fileHelper)
        {
            _hostingEnvironment = hostingEnvironment;
            _appSesstings = appSesstings.Value;
            _fileProtocol = fileService;
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
                    result.ResultObject.Data = _fileProtocol.SearchSong((MusicSearchTypes1)searchType1, searchType2, (GradeTypes)gradeType, searchText, rowPerPage, selectPage, out totalCount);

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
                    result.ResultObject.Data = _fileProtocol.SearchEffect(searchText, rowPerPage, selectPage, out totalCount);

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
        /// 음악/효과음 소재 - 다운로드
        /// </summary>
        /// <param name="token">파일 SEQ</param>
        /// <returns></returns>
        [HttpGet("files")]
        public IActionResult MusicDownload([FromQuery] string token, [FromQuery] string inline = "N")
        {
            var jsonMusicInfo = MAMUtility.ParseToJsonRequestContent(token);
            var musicInfo = MAMUtility.ParseToRequestContent(token);
            var requestInfo = _fileProtocol.GetRequestInfo(musicInfo);
            string contentType = "audio/wav";
            long fileSize;
            var stream = _fileProtocol.GetFileStream(requestInfo[0] as string, Convert.ToInt32(requestInfo[1]), jsonMusicInfo, out fileSize);
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                //FileName = WebUtility.UrlEncode(requestInfo[2] as string),
                FileName = Uri.EscapeDataString(requestInfo[2] as string),
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
            var decodedFilePath = MAMUtility.GetFilePathFromMusicToken(token);
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
            var decodedFilePath = MAMUtility.GetFilePathFromMusicToken(token);
            string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
            try
            {
                return _fileHelper.GetWaveformFromPath(decodedFilePath, userId, remoteIp);
            }
            catch (HttpStatusErrorException ex)
            {
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
            _fileProtocol.TempDownloadWavAndEgy(userId, remoteIp, token);
            return Ok();
        }
        [HttpPost("music-to-myspace")]
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> MusicToMyspace([FromQuery] string token, [FromBody] M30_MAM_PRIVATE_SPACE metaData, [FromServices] PrivateFileBll privateBll)
        {
            DTO_RESULT<DTO_RESULT_OBJECT<string>> result = new DTO_RESULT<DTO_RESULT_OBJECT<string>>();
            try
            {
                var jsonMusicInfo = MAMUtility.ParseToJsonRequestContent(token);
                var musicInfo = MAMUtility.ParseToRequestContent(token);
                var requestInfo = _fileProtocol.GetRequestInfo(musicInfo);
                var fileName = requestInfo[2] as string;

                string userId = HttpContext.Items[Define.USER_ID] as string;
                long fileSize;
                using (var stream = _fileProtocol.GetFileStream(requestInfo[0] as string, Convert.ToInt32(requestInfo[1]), jsonMusicInfo, out fileSize))
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
            return _fileProtocol.TempImageDownload(userId, remoteIp, token, albumToken);
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
                result.ResultObject.Data = _fileProtocol.SearchLyrics(lyricsSeq);
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
