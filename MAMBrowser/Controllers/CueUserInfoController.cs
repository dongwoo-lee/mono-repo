using MAMBrowser.BLL;
using M30.AudioFile.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAMBrowser.DTO;
using M30.AudioFile.Common;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CueUserInfoController : Controller
    {
        private readonly CueUserInfoBll _bll;

        public CueUserInfoController(CueUserInfoBll bll)
        {
            _bll = bll;
        }

        // 방송일 프로그램 리스트 
        [HttpGet("GetPgmListByBrdDate")]
        public DTO_RESULT<IEnumerable<PgmListDTO>> GetPgmListByBrdDate (string person, char media,string brd_dt)
        {
            var result = new DTO_RESULT<IEnumerable<PgmListDTO>>();
            try
            {
                result.ResultObject = _bll.GetPgmListByBrdDate(person, media,brd_dt);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        // 송출일 프로그램 리스트 
        [HttpGet("GetSCHPgmList")]
        public DTO_RESULT<IEnumerable<PgmListDTO>> GetSCHPgmList(string person, char media, string brd_dt)
        {
            var result = new DTO_RESULT<IEnumerable<PgmListDTO>>();
            try
            {
                result.ResultObject = _bll.GetSCHPgmList(person, media, brd_dt);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }


        //프로그램 전체 담당자 가져오기
        [HttpGet("GetDirectorList")]
        public DTO_RESULT<string> GetDirectorList([FromQuery] string productid)
        {
            var result = new DTO_RESULT<string>();
            try
            {
                result.ResultObject =  _bll.GetDirectorList(productid);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //프로그램 키워드 가져오기
        [HttpGet("GetKeyword")]
        public DTO_RESULT<string> GetKeyword([FromQuery] string pgmcode)
        {
            var result = new DTO_RESULT<string>(); 
            try
            {
                result.ResultObject = _bll.GetPgmcodeKeyword(pgmcode);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;

            }
            return result;
        }

    }
}