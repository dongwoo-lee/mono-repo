﻿using MAMBrowser.BLL;
using MAMBrowser.Common;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
    [CustomAuthorize]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesBll _bll;
        private readonly IOptions<AppSettings> _appSesstings;

        public CategoriesController(IOptions<AppSettings> appSesstings, CategoriesBll bll)
        {
            _appSesstings = appSesstings;
            _bll = bll;
        }
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
                result.ResultObject = _bll.GetMedia();
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
                result.ResultObject = _bll.GetUserList();
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
        /// 프로그램 제작자 목록 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/pd")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_USER>> GetPDUserList()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_USER>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_USER>>();
            try
            {
                result.ResultObject = _bll.GetPDUserList();
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
        /// 취재물 제작자 목록 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/reporter")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_USER>> GetReportUserList()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_USER>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_USER>>();
            try
            {
                result.ResultObject = _bll.GetReportUserList();
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
        /// MD 제작자 목록 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/md")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_USER>> GetMDUserList()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_USER>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_USER>>();
            try
            {
                result.ResultObject = _bll.GetMDUserList();
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
                result.ResultObject = _bll.GetReport();
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
                result.ResultObject = _bll.GetPro();
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
                result.ResultObject = _bll.GetCM();
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
        /// 주조 SPOT 분류 조회
        /// </summary>
        /// <param name="media">매체 : ex)A,C,F,D</param>
        /// <returns></returns>
        [HttpGet("mcr/spot")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetMcrSpot(string media)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                result.ResultObject = _bll.GetMcrSpot(media);
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
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetFillerPr()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                result.ResultObject = _bll.GetFillerPr();
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
                result.ResultObject = _bll.GetFillerGeneral();
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
                result.ResultObject = _bll.GetFillerTimetone();
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
        [HttpGet("filler/etc")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetFillerETC()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                result.ResultObject = _bll.GetFillerETC();
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
        /// 사용처 목록 조회
        /// </summary>
        /// <param name="brd_dt">방송 종료일</param>
        /// <returns></returns>
        [HttpGet("pgmcodes")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetPgmCodes([FromQuery] string brd_dt)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                if (string.IsNullOrEmpty(brd_dt))
                    brd_dt = "22000101";

                result.ResultObject = _bll.GetPgmCodes(brd_dt);
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
        /// (구)프로소재, 공유소재 매체목록 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("public-codes/primary")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetPublicPrimary()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try

            {
                result.ResultObject = _bll.GetPublicPrimary();
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
        /// 공유소재 분류 목록
        /// </summary>
        /// <param name="primaryCode">공유소재 매체코드</param>
        /// <returns></returns>
        [HttpGet("public-codes/primary/{primaryCode}")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetPublicSecond(string primaryCode, [FromQuery] string userId)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                result.ResultObject = _bll.GetPublicSecond(primaryCode, userId);
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
        /// DL Device 목록
        /// </summary>
        /// <returns></returns>
        [HttpGet("dldevice-list")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetDLDeviceList()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                result.ResultObject = _bll.GetDLDeviceList();
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
        /// 방송의뢰 상태 목록 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("req-status")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetReqStatus()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                result.ResultObject = _bll.GetReqStatus();
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
