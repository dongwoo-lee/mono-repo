using MAMBrowser.BLL;
using M30.AudioFile.Common;
using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M30.AudioFile.Common.DTO;

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
        public DTO_RESULT<TempCueList_Page> GetTempList([FromQuery] string personid, string title, int row_per_page, int select_page)
        {
            var result = new DTO_RESULT<TempCueList_Page>();
            try
            {
                result.ResultObject = _bll.GetPersonIDWithTitleTemplateList(personid, title, row_per_page, select_page);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
                return result;

        }

        //템플릿 상세내용 가져오기
        [HttpGet("GetTempCue")]
        public CueSheetCollectionDTO GetTempCue([FromQuery] int cueid, string pgmcode, string brd_dt)
        {
            try
            {
                return _bll.GetTemplate(cueid, pgmcode, brd_dt);
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
