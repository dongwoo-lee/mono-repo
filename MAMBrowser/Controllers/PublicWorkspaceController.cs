using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/products/public/workspace")]
    public class publicWorkspaceController : ControllerBase
    {
        [HttpPost("upload")]
        public DTO_RESULT DataUpload()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpPut("{id}")]
        public DTO_RESULT DataUpdate(string id)
        {
            //메타데이터만 수정. 파일은 삭제해야함.
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        //[HttpGet("?media={media}&adcd={adcd}&pd={pd}&name={name}")]
        [HttpGet("files")]
        public DTO_RESULT GetPrivateDataList([FromQuery] string filename, [FromQuery] string title, [FromQuery] string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
    }
}
