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
        [HttpGet("programlist")]
        public DTO_RESULT GetProgramList([FromQuery] string start_dt, [FromQuery] string end_dt)
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
        [HttpGet("categories/filler/general")]
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
