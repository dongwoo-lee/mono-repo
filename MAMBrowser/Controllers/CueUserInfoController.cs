using DAP3.CueSheetCommon.DTO.Result;
using MAMBrowser.BLL;
using MAMBrowser.Helper;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CueUserInfoController : ControllerBase
    {
        private readonly CueUserInfoBll _bll;

        public CueUserInfoController(CueUserInfoBll bll)
        {
            Debug.WriteLine("CueUserInfoController");
            _bll = bll;
        }
        //유저별 프로그램 리스트 가져오기
        [HttpGet("GetProgramList")]
        public List<PgmListDTO> GetUserProgramList([FromQuery] string personid, string media)
        {
            try
            {
                Debug.WriteLine("GetUserProgramList");
                return _bll.GetUserPgmList(personid, media);
            }
            catch
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
                Debug.WriteLine("GetDirectorList");
                var result = _bll.GetDirectorList(productid);
                return result;
            }
            catch(Exception exp)
            {
                Debug.WriteLine(exp.Message);
                throw;
            }
        }

    }
}