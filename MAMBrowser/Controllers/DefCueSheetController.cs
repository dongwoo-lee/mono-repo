using MAMBrowser.BLL;
using M30.AudioFile.Common;
using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using M30.AudioFile.Common.DTO;

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
        public DTO_RESULT<DefCueList_Page> GetDefList([FromBody] Pram pram)
        {
            var result = new DTO_RESULT<DefCueList_Page>();
            try
            {
                result.ResultObject = _bll.GetDefCueList(pram.productids, pram.row_per_page, pram.select_page);
                result.ResultCode = RESUlT_CODES.SUCCESS;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        // 기본큐시트 상세내용 가져오기
        [HttpGet("GetDefCue")]
        public DTO_RESULT<CueSheetCollectionDTO> GetDefCue([FromQuery] string productid, [FromQuery] List<string> week, string pgmcode, string brd_dt)
        {
            var result = new DTO_RESULT<CueSheetCollectionDTO>();
            try
            {
                result.ResultObject = _bll.GetDefCue(productid, week, pgmcode, brd_dt);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //기본큐시트 생성 & 업데이트
        [HttpPost("SaveDefCue")]
        public DTO_RESULT<int> SaveDefCue([FromBody] CueSheetCollectionDTO pram)
        {
            var result = new DTO_RESULT<int>();
            try
            {
                result.ResultObject = _bll.SaveDefaultCueSheet(pram);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
                return result;
        }

        //기본큐시트 삭제
        [HttpDelete("DelDefCue")]
        public DTO_RESULT<bool> DelDefCue([FromQuery] int[] delParams)
        {
            var result = new DTO_RESULT<bool>();
            try
            {
                result.ResultObject = _bll.DeleteDefaultCueSheet(delParams);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
                return result;
        }

    }
}
