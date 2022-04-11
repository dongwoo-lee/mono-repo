using MAMBrowser.BLL;
using M30.AudioFile.Common;
using M30.AudioFile.DAL;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M30.AudioFile.Common.DTO;

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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 주조 SPOT 매체 목록
        /// </summary>
        /// <returns>주조 SPOT 매체 목록 반환</returns>
        [HttpGet("media/mcrspot")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetMcrSpotMedia()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                result.ResultObject = _bll.GetMcrSpotMedia();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 프로목록 조회
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 부조 SPOT 분류 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("scr/spot")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetScrSpot()
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                result.ResultObject = _bll.GetScrSpot();
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 사용처 목록 조회
        /// </summary>
        /// <param name="brd_dt">방송 종료일</param>
        /// <param name="media">매체코드</param>
        /// <returns></returns>
        [HttpGet("pgmcodes")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetPgmCodes([FromQuery] string brd_dt, [FromQuery] string media)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                if (string.IsNullOrEmpty(brd_dt))
                    brd_dt = "22000101";

                result.ResultObject = _bll.GetPgmCodes(brd_dt, media);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 프로소재, 공유소재 매체목록 조회
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
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
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }






        /// <summary>
        /// 프로그램 목록 반환(PGM)
        /// </summary>
        /// <param name="media">매체코드 - ex) A, F, M</param>
        /// <param name="date">시작일자 - ex) 20210101</param>
        /// <returns></returns>
        [HttpGet("pgm-sch")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_BrdSch>> GetPgmSch(string media, string date)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_BrdSch>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_BrdSch>>();
            try
            {
                result.ResultObject = new DTO_RESULT_LIST<DTO_BrdSch>();
                result.ResultObject.Data = _bll.GetPgmSch(media, date);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 프로그램 목록 반환(주조SPOT, 변동소재, 고정소재)
        /// </summary>
        /// <param name="media">매체코드 - ex) A, F, M</param>
        /// <param name="date">시작일자 - ex) 20210101</param>
        /// <param name="spotType">MS : 주조SPOT , TS : 변동소재, TT : 고정소재</param>
        /// <returns></returns>
        [HttpGet("spot-sch")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_BrdSpot>> GetSpotSch(string media, string date, string spotType)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_BrdSpot>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_BrdSpot>>();
            try
            {
                result.ResultObject = new DTO_RESULT_LIST<DTO_BrdSpot>();
                result.ResultObject.Data = _bll.GetSpotSch(media, date, spotType);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 특정 사용자가 담당하는 프로그램 목록 반환
        /// </summary>
        /// <param name="media"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user-pgmcodes")]
        public DTO_RESULT<DTO_RESULT_LIST<string>> GetPgmCodeByUser([FromQuery] string media, [FromQuery] string userId)
        {
            DTO_RESULT<DTO_RESULT_LIST<string>> result = new DTO_RESULT<DTO_RESULT_LIST<string>>();
            try
            {
                result.ResultObject = new DTO_RESULT_LIST<string>();
                result.ResultObject.Data = _bll.GetPgmCodeByUser(media, userId);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 특정 사용자가 담당하는 오디오코드 목록 반환(프로소재 목록)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user-audiocodes")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> GetAudioCodeByUser([FromQuery] string userId)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CATEGORY>>();
            try
            {
                result.ResultObject = new DTO_RESULT_LIST<DTO_CATEGORY>();
                result.ResultObject.Data = _bll.GetAudioCodeByUser(userId);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 부조SPOT 소재파일 목록 조회
        /// </summary>
        /// <param name="spotName"></param>
        /// <param name="codeId"></param>
        /// <param name="cmOwner"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("scr-spot")]
        public DTO_RESULT<DTO_RESULT_LIST<Dto_ScrSpot>> GetScrSpotList([FromQuery] string spotName, [FromQuery] string codeId, [FromQuery] string cmOwner, [FromQuery] string startDate, [FromQuery]  string endDate)
        {
            DTO_RESULT<DTO_RESULT_LIST<Dto_ScrSpot>> result = new DTO_RESULT<DTO_RESULT_LIST<Dto_ScrSpot>>();
            try
            {
                result.ResultObject = new DTO_RESULT_LIST<Dto_ScrSpot>();
                result.ResultObject.Data = _bll.GetScrSpotList(spotName, codeId, cmOwner, startDate, endDate);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                FileLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
    }
}
