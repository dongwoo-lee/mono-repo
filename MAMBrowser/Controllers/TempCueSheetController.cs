using DAP3.CueSheetCommon.DTO.Result;
using MAMBrowser.BLL;
using MAMBrowser.Entiies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TempCueSheetController : ControllerBase
    {
        private readonly TemplateBll _bll;

        public TempCueSheetController(TemplateBll bll)
        {
            _bll = bll;
        }

        // 템플릿 목록 가져오기
        [HttpGet("GetTempList")]
        public List<TemplateListDTO> GetTempList([FromQuery] string personid, string title)
        {
            try
            {
                return _bll.GetPersonIDWithTitleTemplateList(personid, title);
            }
            catch
            {
                throw;
            }
            
        }

        //템플릿 상세내용 가져오기
        [HttpGet("GetTempCue")]
        public TemplateCollectionDTO GetTempCue([FromQuery] int cueid)
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
        public bool SaveTempCue([FromBody] CueData pram)
        {
            try
            {
                return _bll.SaveTemplate(pram.temParam, pram.conParams, pram.tagParams, pram.printParams, pram.attParams);
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
