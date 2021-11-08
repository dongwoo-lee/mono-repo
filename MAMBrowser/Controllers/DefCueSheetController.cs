using DAP3.CueSheetCommon.DTO.Result;
using MAMBrowser.BLL;
using MAMBrowser.Entiies;
using MAMBrowser.Foundation;
using MAMBrowser.Helper;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DefCueSheetController : ControllerBase
    {
        private readonly DefCueSheetBll _bll;

        public DefCueSheetController(DefCueSheetBll bll)
        {
            _bll = bll;
        }
        // 기본큐시트 목록 가져오기
        [HttpGet("GetDefList")]
        public List<DefaultCueSheetListDTO> GetDefList([FromQuery] string[] productids)
        {

            try
            {
                var result = _bll.GetDefCueList(productids);
                return result;
            }
            catch
            {
                throw;
            }
        }
        // 기본큐시트 상세내용 가져오기
        [HttpGet("GetDefCue")]
        public CueSheetCollectionDTO GetDefCue([FromQuery]string productid, [FromQuery] int[] cueid)
        {
            try
            {
                var result = _bll.GetDefCue(productid, cueid);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // 기본큐시트 저장 (테스트필요)
        [HttpPost("SaveDefCue")]
        public bool SaveDefCue([FromBody]CueData pram)
        {
            try
            {
                //pram.defParams.Skip(2);
                //var def = pram.defParams.Skip(2);
                return _bll.SaveDefaultCueSheet(pram.cueParam, pram.defParams, pram.conParams, pram.tagParams, pram.printParams, pram.attParams);
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
