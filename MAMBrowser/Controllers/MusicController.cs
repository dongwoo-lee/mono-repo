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
    public class MusicController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AppSettings _appSesstings;
        private readonly MusicService _fileService;
        public MusicController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSesstings, ServiceResolver sr)
        {
            _hostingEnvironment = hostingEnvironment;
            _appSesstings = appSesstings.Value;
            _fileService = (MusicService)sr("MusicConnection");
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
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>> FindMusic([FromBody] DTO_MUSIC_REQUEST requestInfo, [FromQuery] int searchType, [FromQuery] int gradeType, [FromQuery] string searchText, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_SONG>>();
            try
            {
                long totalCount;
                result.ResultObject.Data = _fileService.SearchSong(requestInfo, (SearchTypes)searchType, (GradeTypes)gradeType, searchText, rowPerPage, selectPage, out totalCount);
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
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_EFFECT>> FindEffect([FromBody] DTO_MUSIC_REQUEST requestInfo, [FromQuery] string searchText, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_EFFECT>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_EFFECT>>();
            try
            {
                long totalCount;
                result.ResultObject.Data = _fileService.SearchLyrics(requestInfo, searchText, rowPerPage, selectPage, out totalCount);
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
        /// <param name="seq">파일 SEQ</param>
        /// <returns></returns>
        [HttpPost("files")]
        public IActionResult MusicDownload([FromBody] string filePath, [FromQuery] string inline = "N")
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
        /// <summary>
        /// 음악/효과음 소재  - 스트리밍
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="direct">Y, N</param>
        /// <returns></returns>
        [HttpPost("streaming")]
        public IActionResult MusicStreaming([FromBody] string filePath, [FromQuery] string direct = "N")
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
                var downloadPath = MAMUtility.DownloadFile(_fileService, filePath, fileName, out contentType);
                return PhysicalFile(downloadPath, contentType, true);
            }
        }
        /// <summary>
        /// 일반 소재 - 파형 요청
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        [HttpPost("waveform")]
        public List<float> MusicGetWaveform([FromBody] string filePath)
        {
            //var fileData = _dal.Get(seq);
            return MAMUtility.GetWaveform(_fileService, filePath);
        }


        /// <summary>
        /// 앨범 이미지 반환
        /// </summary>
        /// <param name="filaPath"></param>
        /// <returns></returns>
        [HttpPost("albums/images/files")]
        public IActionResult GetAlbumImage([FromBody] DTO_MUSIC_REQUEST filePath, [FromQuery] string inline = "N")
        {
            //1. 앨범 이미지 목록 받아오기.
            //2. 앨범 이미지 스트리밍 서비스 받기.
            //3. 

            //string fileName = Path.GetFileName(filePath.FilePath);
            var fileExtProvider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!fileExtProvider.TryGetContentType(filePath.FilePath, out contentType))
            {
                contentType = "application/octet-stream";
            }
            //var stream = _fileService.GetFileStream(filePath.FilePath, 0);
            //System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            //{
            //    FileName = fileName,
            //    Inline = inline == "Y" ? true : false
            //};
            //Response.Headers.Add("Content-Disposition", cd.ToString());
            return PhysicalFile(filePath.FilePath, contentType);
        }
        /// <summary>
        /// 앨범 이미지 목록 반환
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpPost("albums/images-list")]
        public IList<string> GetAlbumImageList([FromBody] string filePath)
        {
            //1. 앨범이미지 fullPath에서 상대경로를 일단 저장해놈
            //2. 정보컨텐츠부서로부터 이미지파일 이름 목록을 서비스 받음.(파라매터는 파일 풀 경로)
            //3. 파일이름을 반환받으면, 상대경로를 전부 붙여서 프론트로 전달.(암호화해서;)

            return _fileService.GetImages();
        }
    }
}
