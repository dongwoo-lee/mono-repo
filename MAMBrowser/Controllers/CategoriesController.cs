using MAMBrowser.BLL;
using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        /// <summary>
        /// 매체 목록
        /// </summary>
        /// <returns>매체 목록 반환</returns>
        [HttpPost("media")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetMedia()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                CategoriesBLL bll = new CategoriesBLL();
                result.ResultObject = bll.GetMedia();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpPost("report")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetReport()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                CategoriesBLL bll = new CategoriesBLL();
                result.ResultObject = bll.GetReport();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpPost("pro")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetPro()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                CategoriesBLL bll = new CategoriesBLL();
                result.ResultObject = bll.GetPro();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpPost("cm")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetCM()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                CategoriesBLL bll = new CategoriesBLL();
                result.ResultObject = bll.GetCM();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpPost("mcr/spot")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetMcrSpot(string media)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                CategoriesBLL bll = new CategoriesBLL();
                result.ResultObject = bll.GetMcrSpot(media);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpPost("filler/pro")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetFillerPr(string media)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                CategoriesBLL bll = new CategoriesBLL();
                result.ResultObject = bll.GetFillerPr(media);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpPost("filler/general")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetFillerGeneral()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                CategoriesBLL bll = new CategoriesBLL();
                result.ResultObject = bll.GetFillerGeneral();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpPost("filler/timetone")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetFillerTimetone()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                CategoriesBLL bll = new CategoriesBLL();
                result.ResultObject = bll.GetFillerTimetone();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpPost("filler/etc")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetFillerETC()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                CategoriesBLL bll = new CategoriesBLL();
                result.ResultObject = bll.GetFillerETC();
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
