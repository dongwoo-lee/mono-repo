using MAMBrowser.BLL;
using MAMBrowser.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    /// <summary>
    /// 카테고리 조회 서비스
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        /// <summary>
        /// 매체 목록
        /// </summary>
        /// <returns>매체 목록 반환</returns>
        [HttpGet("media")]
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
        /// <summary>
        /// 사용자 목록 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_USER>> GetUserList()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_USER>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_USER>>();
            try
            {
                APIBLL bll = new APIBLL();
                result.ResultObject = bll.GetUserList();
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
        /// 취재물 분류 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("report")]
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
        /// <summary>
        /// (구)프로목록 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("pro")]
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
        /// <summary>
        /// 광고 분류 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("cm")]
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
        /// <summary>
        /// 주조 SPOT 분류 주회
        /// </summary>
        /// <param name="media">매체 : ex)A,C,F,D</param>
        /// <returns></returns>
        [HttpGet("mcr/spot")]
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
        /// <summary>
        /// 필러(pr) 분류 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("filler/pro")]
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
        /// <summary>
        /// 필러(일반) 분류 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("filler/general")]
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
        /// <summary>
        /// 필러(시간) 분류 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("filler/timetone")]
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

        /// <summary>
        /// 필러(기타) 분류 조회
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("filler/etc")]
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
