using MAMBrowser.BLL;
using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using M30.AudioFile.Common;
using M30.AudioFile.Common.DTO;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArchiveCueSheetController : ControllerBase
    {
        private readonly ArchiveCueSheetBll _bll;

        public ArchiveCueSheetController(ArchiveCueSheetBll bll)
        {
            _bll = bll;
        }
        public class ArchPram
        {
            public string media { get; set; }
            public string title { get; set; }
            public int row_per_page { get; set; }
            public int select_page { get; set; }
            public string start_dt { get; set; }
            public string end_dt { get; set; }
            public string tag { get; set; }
        }

        //이전큐시트 목록 가져오기
        [HttpPost("GetArchiveCueList")]
        public DTO_RESULT<ArchiveCueList_Page> GetArchiveCueList([FromBody] ArchPram pram)
        {
            var result = new DTO_RESULT<ArchiveCueList_Page>();
            try
            {
                var tags = new List<string>();
                if (pram.tag != ""&&pram.tag!=null)
                    tags.Add(pram.tag);
                result.ResultObject = _bll.GetArchiveCueSheetList(pram.media,pram.title, pram.start_dt, pram.end_dt, pram.row_per_page, pram.select_page, tags);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
                return result;
        }

        //이전큐시트 상세내용 가져오기
        [HttpGet("GetArchiveCue")]
        public DTO_RESULT<CueSheetCollectionDTO> GetArchiveCue([FromQuery] int cueid)
        {
            var result = new DTO_RESULT<CueSheetCollectionDTO>();
            try
            {
                result.ResultObject = _bll.GetArchiveCueSheet(cueid);
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
