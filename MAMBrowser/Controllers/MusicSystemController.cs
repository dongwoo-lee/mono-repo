using MAMBrowser.DTO;
using MAMBrowser.Entiies;
using MAMBrowser.Helpers;
using MAMBrowser.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusicSystemController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AppSettings _appSesstings;
        private readonly MusicService _fileService;
        public MusicSystemController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSesstings, MusicService fileService)
        {
            _hostingEnvironment = hostingEnvironment;
            _appSesstings = appSesstings.Value;
            _fileService = fileService;
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
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>> FindMusic([FromQuery] int searchType1, [FromQuery] int searchType2, [FromQuery] int gradeType, [FromQuery] string searchText, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>>();
            try
            {
                long totalCount;
                //result.ResultObject.Data = _fileService.SearchSong((SearchTypes)searchType1, searchType2, (GradeTypes)gradeType, searchText, rowPerPage, selectPage, out totalCount);
                result.ResultObject = new DTO_RESULT_PAGE_LIST<DTO_SONG>();
                result.ResultObject.Data = new List<DTO_SONG>();
                result.ResultObject.Data.Add(new DTO_SONG());
                totalCount = 1;

                //if (string.IsNullOrEmpty(searchText))
                //    result.ResultObject.Data = null;
                //else
                    //result.ResultObject.Data = _fileService.SearchSong((SearchTypes)searchType1, searchType2, (GradeTypes)gradeType, searchText, rowPerPage, selectPage, out totalCount);

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
                long totalCount = 0;
                if (string.IsNullOrEmpty(searchText))
                    result.ResultObject.Data = null;
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
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }



        /// <summary>
        /// 음악/효과음 소재 - 다운로드
        /// </summary>
        /// <param name="token">파일 SEQ</param>
        /// <returns></returns>
        [HttpPost("files")]
        public IActionResult MusicDownload([FromQuery] string token, [FromQuery] string inline = "N")
        {
            string fileName = Path.GetFileName(token);
            var fileExtProvider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!fileExtProvider.TryGetContentType(token, out contentType))
            {
                contentType = "application/octet-stream";
            }
            var stream = _fileService.GetFileStream(token, 0);
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
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
        [HttpPost("streaming")]
        public IActionResult MusicStreaming([FromQuery] string token, [FromQuery] string direct = "N")
        {
            string fileName = Path.GetFileName(token);

            if (direct.ToUpper() == "Y")
            {
                int fileSize = 0;
                return new PushStreamResult(token, fileName, fileSize, _fileService);
            }
            else
            {
                string contentType;
                var downloadPath = MAMUtility.TempDownloadToLocal(HttpContext.Items["UserId"] as string, _fileService, token, out contentType);
                return PhysicalFile(downloadPath, contentType, true);
            }
        }
        /// <summary>
        /// 음악 소재 - 파형 요청
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("waveform")]
        public List<float> MusicGetWaveform([FromQuery] string token)
        {
            //var fileData = _dal.Get(seq);
            return MAMUtility.GetWaveform(HttpContext.Items["UserId"] as string, _fileService, token);
        }

        /// <summary>
        /// 일반 소재 - 파형 요청
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        [HttpPost("lyrics")]
        public DTO_RESULT<DTO_RESULT_OBJECT<string>> SearchLyrics([FromQuery] long seq)
        {
            DTO_RESULT<DTO_RESULT_OBJECT<string>> result = new DTO_RESULT<DTO_RESULT_OBJECT<string>>();
            try
            {
                result.ResultObject.Data = _fileService.SearchLyrics(seq);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 앨범 이미지 목록 반환
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("albums/images-path")]
        public DTO_RESULT<DTO_RESULT_LIST<string>> GetAlbumImagePathList([FromQuery] string token, [FromQuery] string albumToken)
        {
            DTO_RESULT<DTO_RESULT_LIST<string>> result = new DTO_RESULT<DTO_RESULT_LIST<string>>();
            try
            {
                //result.ResultObject.Data = _fileService.GetImageTokenList(musicToken, albumToken);
                string path1 = @"\\192.168.1.201\detail-small-2";
                string path2 = @"\\192.168.1.201\detail-small-3";
                string path3 = @"\\192.168.1.201\profile-pic-6";
                string path4 = @"\\192.168.1.201\profile-pic-l-9";
                List<string> pathList = new List<string>();
                pathList.Add(MAMUtility.GenerateMusicToken(path1));
                pathList.Add(MAMUtility.GenerateMusicToken(path2));
                pathList.Add(MAMUtility.GenerateMusicToken(path3));
                pathList.Add(MAMUtility.GenerateMusicToken(path4));

                result.ResultObject.Data = pathList;
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
                result.ErrorMsg = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 앨범 이미지 반환
        /// </summary>
        /// <param name="mamToken"></param>
        /// <returns></returns>
        [HttpPost("albums/images/files")]
        public IActionResult GetAlbumImage([FromQuery] string albumToken, [FromQuery] string inline = "Y")
        {
            string contentType;
            var result = _fileService.GetAlbumImage(albumToken, out contentType);
            return File(result, contentType);
        }
       
    }
}
