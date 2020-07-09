using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/products/[controller]")]
    public class RequestController : ControllerBase
    {
        [HttpPost("file")]
        public DTO_RESULT RequestDownloadFile()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("job_status?jobid={jobid}")]
        public DTO_RESULT GetJobStatus(string jobid)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
    }
}
