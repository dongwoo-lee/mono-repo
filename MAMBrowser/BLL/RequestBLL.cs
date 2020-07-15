using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    public class RequestBLL
    {
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
        public DTO_RESULT<CacheFIleStatus> GetCacheFilePath(string sourcePath)
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
