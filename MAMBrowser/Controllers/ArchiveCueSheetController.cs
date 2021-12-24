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

        // 이전큐시트 목록 가져오기 (날짜별)
        [HttpGet("GetArchiveCueList")]
        public ArchiveCueList_Result GetArchiveCueList([FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] List<string> products,string media, int row_per_page, int select_page)
        {
            try
            {
                ArchiveCueList_Result result = new ArchiveCueList_Result();
                result.ResultObject = new ArchiveCueList_Page();
                result.ResultObject = _bll.GetArchiveCueSheetList(products, start_dt, end_dt, row_per_page, select_page);
                result.ResultCode = RESUlT_CODES.SUCCESS;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //이전큐시트 상세내용 가져오기
        [HttpGet("GetArchiveCue")]
        public CueSheetCollectionDTO GetArchiveCue([FromQuery] int cueid)
        {
            try
            {
                return _bll.GetArchiveCueSheet(cueid);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
