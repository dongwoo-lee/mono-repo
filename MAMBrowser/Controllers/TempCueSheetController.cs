using MAMBrowser.BLL;
using M30.AudioFile.Common;
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
    public class TempCueSheetController : Controller
    {
        private readonly TemplateBll _bll;

        public TempCueSheetController(TemplateBll bll)
        {
            _bll = bll;
        }

        // 템플릿 목록 가져오기
        [HttpGet("GetTempList")]
        public TempCueList_Result GetTempList([FromQuery] string personid, string title, int row_per_page, int select_page)
        {
            try
            {
                var result = new TempCueList_Result();
                result.ResultObject = new TempCueList_Page();
                result.ResultObject = _bll.GetPersonIDWithTitleTemplateList(personid, title, row_per_page, select_page);
                result.ResultCode = RESUlT_CODES.SUCCESS;
                return result;

            }
            catch
            {
                throw;
            }

        }

        //템플릿 상세내용 가져오기
        [HttpGet("GetTempCue")]
        public CueSheetCollectionDTO GetTempCue([FromQuery] int cueid)
        {
            try
            {
                return _bll.GetTemplate(cueid);
            }
            catch
            {
                throw;
            }
        }

        //템플릿 생성 & 업데이트 (테스트필요)
        [HttpPost("SaveTempCue")]
        public int SaveTempCue([FromBody] CueSheetCollectionDTO pram)
        {
            try
            {
                return _bll.SaveTemplate(pram);
            }
            catch
            {
                throw;
            }
        }

        //템플릿 삭제
        [HttpDelete("DelTempCue")]
        public bool DelTempCue([FromQuery] int[] tempids)
        {
            try
            {
                return _bll.DeleteTemplate(tempids);
            }
            catch
            {
                throw;
            }
        }

    }
}
