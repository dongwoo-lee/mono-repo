using MAMBrowser.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    public class PrivateFileBLL
    {
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
        public DTO_RESULT<DTO_RESULT_LIST<DTO_PRIVATE_FILE>> FineData(string filename, string title, string memo, string editor, int rowPerPage, int selectPage, string sortKey, string sortValue)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_PRIVATE_FILE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_PRIVATE_FILE>>();
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
