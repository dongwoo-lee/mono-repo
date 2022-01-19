using MAMBrowser.BLL;
using M30.AudioFile.Common;
using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DefCueSheetController : Controller
    {
        private readonly DefCueSheetBll _bll;
        private readonly ILogger<ProductsController> _logger;

        public class Pram
        {
            public List<string> productids {get;set; }
            public int row_per_page { get; set; }
            public int select_page { get; set; }

        }

        public DefCueSheetController(DefCueSheetBll bll, ILogger<ProductsController> logger)
        {
            _logger = logger;
            _bll = bll;
            
        }

        //기본 큐시트 목록 가져오기
        [HttpPost("GetDefList")]
        public DefCueList_Result GetDefList([FromBody] Pram pram)
        {
            DefCueList_Result result = new DefCueList_Result();
            result.ResultObject = new DefCueList_Page();
            result.ResultObject = _bll.GetDefCueList(pram.productids, pram.row_per_page, pram.select_page);
            result.ResultCode = RESUlT_CODES.SUCCESS;
            return result;
        }

        //프로그램별 기본큐시트 적용요일 리스트 가져오기
        [HttpGet("GetSelectedWeek")]
        public string[] GetSelectedWeekList([FromQuery] string productid)
        {
            return _bll.GetDefWeek(productid);

        }

        // 기본큐시트 상세내용 가져오기
        [HttpGet("GetDefCue")]
        public CueSheetCollectionDTO GetDefCue([FromQuery] string productid, [FromQuery] List<string> week, string pgmcode, string brd_dt)
        {
            try
            {
                var result = _bll.GetDefCue(productid, week, pgmcode, brd_dt);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //기본큐시트 생성 & 업데이트
        [HttpPost("SaveDefCue")]
        public int SaveDefCue([FromBody] CueSheetCollectionDTO pram)
        {
            try
            {
                var result = _bll.SaveDefaultCueSheet(pram);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //기본큐시트 삭제
        [HttpDelete("DelDefCue")]
        public bool DelDefCue([FromQuery] int[] delParams)
        {
            try
            {
                var result = _bll.DeleteDefaultCueSheet(delParams);
                return result;
            }
            catch
            {
                throw;
            }
        }

    }
}
