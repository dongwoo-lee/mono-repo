using MAMBrowser.BLL;
using MAMBrowser.Common;
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
    public class DefCueSheetController : Controller
    {
        private readonly DefCueSheetBll _bll;

        public DefCueSheetController(DefCueSheetBll bll)
        {
            _bll = bll;
        }

        // 기본큐시트 목록 가져오기
        [HttpGet("GetDefList")]
        public DefCueList_Result GetDefList([FromQuery] List<string> productids, int row_per_page, int select_page)
        {
            try
            {
                DefCueList_Result result = new DefCueList_Result();
                result.ResultObject = new DefCueList_Page();
                result.ResultObject = _bll.GetDefCueList(productids, row_per_page, select_page);
                result.ResultCode = RESUlT_CODES.SUCCESS;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        // 기본큐시트 상세내용 가져오기
        [HttpGet("GetDefCue")]
        public CueSheetCollectionDTO GetDefCue([FromQuery] string productid, [FromQuery] List<string> week)
        {
            try
            {
                var result = _bll.GetDefCue(productid, week);
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
