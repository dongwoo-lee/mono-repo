using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/products/private/workspace")]
    public class PrivateWorkspaceController : ControllerBase
    {
        [HttpPost("upload")]
        public DTO_RESULT UploadAndInsertData()
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpPut("{id}")]
        public DTO_RESULT UpdateData(string id)
        {
            //메타데이터만 수정. 파일은 삭제해야함.
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        //[HttpGet("filename={filename}&title={title}&memo={memo}")]
        [HttpGet("files")]
        public DTO_RESULT GetPrivateDataList([FromQuery] string filename, [FromQuery] string title, [FromQuery] string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
    }
}
