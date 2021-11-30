﻿using MAMBrowser.BLL;
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
    public class CueUserInfoController : Controller
    {
        private readonly CueUserInfoBll _bll;

        public CueUserInfoController(CueUserInfoBll bll)
        {
            _bll = bll;
        }

        //유저별 프로그램 리스트 가져오기
        [HttpGet("GetProgramList")]
        public IEnumerable<PgmListDTO> GetUserProgramList([FromQuery] string personid, char media)
        {
            try
            {
                return _bll.GetUserPgmList(personid, media);
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

    }
}
