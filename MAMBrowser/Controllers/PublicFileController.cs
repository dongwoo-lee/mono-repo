using MAMBrowser.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/products/workspace/[controller]")]
    public class PublicFileController : ControllerBase
    {
        /// <summary>
        /// 공유소재 - 파일+메타데이터 등록
        /// </summary>
        /// <param name="file">파일</param>
        /// <param name="jsonMetaData">메타데이터</param>
        /// <returns></returns>
        [RequestSizeLimit(int.MaxValue)]
        [HttpPost("upload")]
        public DTO_RESULT UploadFile(IFormFile file, [FromForm] string jsonMetaData)
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
        /// 공유소재 - 메타데이터 편집
        /// </summary>
        /// <param name="id">ID 값</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public DTO_RESULT UpdateData(string id)
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
        /// 공유소재 - 검색
        /// </summary>
        /// <param name="media">매체코드(A,D,C,F)</param>
        /// <param name="cate">분류코드</param>
        /// <param name="filename">파일명</param>
        /// <param name="title">제목</param>
        /// <param name="memo">메모</param>
        /// <param name="user">제작(등록)자</param>
        /// <param name="rowPerPage">페이지당 행 개수</param>
        /// <param name="selectPage">선택된 페이지</param>
        /// <param name="sortKey">정렬 키(필드명)</param>
        /// <param name="sortValue">정렬 값(ASC/DESC)</param>
        /// <returns></returns>
        [HttpGet]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> FineData(string media, string cate, string filename, string title, string memo, string editor, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>>();
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
    }
}
