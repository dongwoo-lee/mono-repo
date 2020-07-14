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
    public class MirosController : ControllerBase
    {
        /// <summary>
        /// 프로그램 목록 조회
        /// </summary>
        /// <param name="start_dt"></param>
        /// <param name="end_dt"></param>
        /// <returns></returns>
        [HttpGet("programlist")]
        public DTO_RESULT GetProgramList([FromQuery] string start_dt, [FromQuery] string end_dt)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        /// <summary>
        /// 매체 목록 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("medialist")]
        public DTO_RESULT GetMediaList()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        /// <summary>
        /// 프로그램 분류 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories/pro")]
        public DTO_RESULT GetProCatetory()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        /// <summary>
        /// 필러(pr) 분류 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories/filler/pro")]
        public DTO_RESULT GetFillerProCatetory()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        /// <summary>
        /// 필러(일반) 분류 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories/filler/general")]
        public DTO_RESULT GetGeneralCatetory()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        /// <summary>
        /// 필러(시간) 분류 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories/filler/timetone")]
        public DTO_RESULT GetTimetoneCatetory()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        /// <summary>
        /// 필러(기타) 분류 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories/filler/etc")]
        public DTO_RESULT GetEtcCatetory()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        /// <summary>
        /// 광고 분류 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories/cm")]
        public DTO_RESULT GetCMCatetory()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        /// <summary>
        /// 취재물 분류 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories/report")]
        public DTO_RESULT GetReportCatetory()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
    }
}
