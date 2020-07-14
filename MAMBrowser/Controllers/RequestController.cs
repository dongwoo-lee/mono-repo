using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/products/[controller]")]
    public class RequestController : ControllerBase
    {
        /// <summary>
        /// 파일 캐시 요청
        /// </summary>
        /// <param name="sourcePath">소스 파일 경로</param>
        /// <returns></returns>
        [HttpPost("file")]
        public DTO_RESULT RequestCacheFile([FromBody] string sourcePath)
        {
            DTO_RESULT result = new DTO_RESULT();
            try
            {
                result.ResultObject = Guid.NewGuid().ToString();
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
        /// 캐시된 파일 경로 요청
        /// </summary>
        /// <param name="sourcePath">소스 파일 경로</param>
        /// <returns>타겟 파일 경로</returns>
        [HttpGet("file")]
        public DTO_RESULT<CacheFIleStatus> GetCacheFilePath([FromQuery] string sourcePath)
        {
            DTO_RESULT<CacheFIleStatus> result = new DTO_RESULT<CacheFIleStatus>();
            try
            {
                result.ResultObject = new CacheFIleStatus();
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
