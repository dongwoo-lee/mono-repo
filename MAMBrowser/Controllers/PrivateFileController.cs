using log4net.Util;
using MAMBrowser.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// <param name="file">파일</param>
        /// <param name="jsonMetaData">메타데이터</param>
        /// <returns></returns>
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        [HttpPost("files")]
        public DTO_RESULT UploadFile(IFormFile file, [FromForm] string jsonMetaData)
        {
            DTO_RESULT result = new DTO_RESULT();
            try
            {
                string directoryPath = @"c:\임시파일";
                if (!System.IO.Directory.Exists(directoryPath))
                    System.IO.Directory.CreateDirectory(directoryPath);

                string filePath = Path.Combine(directoryPath, file.FileName);

                using (FileStream fs = new FileStream(filePath, FileMode.Create,FileAccess.ReadWrite))
                {
                    file.CopyTo(fs);
                    result.ResultCode = RESUlT_CODES.SUCCESS;
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
        /// My 공간 - 메타데이터 편집
        /// </summary>
        /// <param name="id">ID 값</param>
        /// <returns></returns>
        [HttpPut("meta")]
        public DTO_RESULT UpdateData([FromBody] DTO_PRIVATE_FILE dto)
        {
            DTO_RESULT result = new DTO_RESULT();
            try
            {
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
        /// My 공간 - 검색
        /// </summary>
        /// <param name="editor">제작자</param>
        /// <param name="cate">분류</param>
        /// <param name="filename">파일명</param>
        /// <param name="title">제목</param>
        /// <param name="memo">메모</param>
        /// <param name="rowPerPage">페이지당 행 개수</param>
        /// <param name="selectPage">선택된 페이지</param>
        /// <param name="sortKey">정렬 키(필드명)</param>
        /// <param name="sortValue">정렬 값(ASC/DESC)</param>
        /// <returns></returns>
        [HttpGet("meta/{editor}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> FindData(string editor, [FromQuery] string cate, [FromQuery] string filename, [FromQuery] string title, [FromQuery] string memo, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
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
        /// <param name="editor">제작자</param>
        /// <param name="cate">분류</param>
        /// <param name="filename">파일명</param>
        /// <param name="title">제목</param>
        /// <param name="memo">메모</param>
        /// <param name="rowPerPage">페이지당 행 개수</param>
        /// <param name="selectPage">선택된 페이지</param>
        /// <param name="sortKey">정렬 키(필드명)</param>
        /// <param name="sortValue">정렬 값(ASC/DESC)</param>
        /// <returns></returns>
        [HttpGet("recyclebin/{editor}")]
        public DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> FindRecycleBin(string editor, [FromQuery] string cate, [FromQuery] string filename, [FromQuery] string title, [FromQuery] string memo, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_PAGE_LIST<DTO_PRIVATE_FILE>>();
            try
            {
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
        /// My공간 - 파일 다운로드
        /// </summary>
        //[Authorize]
        [HttpGet("files/{fileid}")]
        public FileResult GetFile(string fileID)
        {
            string directoryPath = @"c:\임시파일";
            string filePath = Path.Combine(directoryPath, fileID);
            IFileProvider provider = new PhysicalFileProvider(directoryPath);
            IFileInfo fileInfo = provider.GetFileInfo(fileID);
            var readStream = fileInfo.CreateReadStream();

            var fileExtProvider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!fileExtProvider.TryGetContentType(filePath, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return File(readStream, contentType, fileID);
        }

        
    }
}
