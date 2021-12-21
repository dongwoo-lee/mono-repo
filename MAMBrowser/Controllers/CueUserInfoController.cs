using MAMBrowser.BLL;
using M30.AudioFile.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAMBrowser.DTO;

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

        //유저별 프로그램 리스트 가져오기
        [HttpGet("GetProgramList")]
        public IEnumerable<PgmListDTO> GetUserProgramList(string person, char media)
        {
            try
            {

                return _bll.GetUserPgmList(person, media);
            }
            catch(Exception)
            {
                throw;
            }
        }
        //프로그램 전체 담당자 가져오기
        [HttpGet("GetDirectorList")]
        public string GetDirectorList([FromQuery] string productid)
        {
            try
            {
                return _bll.GetDirectorList(productid);
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        //광고 목록 가져오기
        [HttpGet("GetSponsorList")]
        public IEnumerable<CueSheetConDTO> GetSponsorList([FromQuery] string pgmcode, [FromQuery] string brd_dt)
        {
            try
            {
                return _bll.GetSponsorList(pgmcode, brd_dt);
            }
            catch (Exception exp)
            {
                throw;
            }
        }

    }
}