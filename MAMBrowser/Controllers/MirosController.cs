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
        [HttpGet("miros/programlist?start_dt={start_dt}&end_dt={end_dt}")]
        public DTO_RESULT GetProgramList()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("medialist")]
        public DTO_RESULT GetMediaList()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("categories/pro")]
        public DTO_RESULT GetProCatetory()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("categories/filler/pro")]
        public DTO_RESULT GetFillerProCatetory()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("categories/filler?general")]
        public DTO_RESULT GetGeneralCatetory()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("categories/filler/timetone")]
        public DTO_RESULT GetTimetoneCatetory()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("categories/filler/etc")]
        public DTO_RESULT GetEtcCatetory()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("categories/cm")]
        public DTO_RESULT GetCMCatetory()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("categories/report")]
        public DTO_RESULT GetReportCatetory()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
    }
}
