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
        private readonly CueAttachmentsBll _attachmentsBll;

        public TempCueSheetController(TemplateBll bll, CueAttachmentsBll attachmentsBll)
        {
            _bll = bll;
            _attachmentsBll = attachmentsBll;
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
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;

        }

        //템플릿 상세내용 가져오기
        [HttpGet("GetTempCue")]
        public DTO_RESULT<CueSheetCollectionDTO> GetTempCue([FromQuery] int cueid, string pgmcode, string brd_dt)
        {
            var result = new DTO_RESULT<CueSheetCollectionDTO>();
            try
            {
                result.ResultObject = _bll.GetTemplate(cueid, pgmcode, brd_dt);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //템플릿 생성 & 업데이트 
        [HttpPost("SaveTempCue")]
        public DTO_RESULT<int> SaveTempCue([FromBody] CueSheetCollectionDTO pram)
        {
            var result = new DTO_RESULT<int>();
            try
            {
                result.ResultObject = _bll.SaveTemplate(pram);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //템플릿으로 저장
        [HttpPost("SaveByTemp")]
        public DTO_RESULT<int> SaveByTemplate([FromBody] CueSheetCollectionDTO pram)
        {
            var result = new DTO_RESULT<int>();
            try
            {
                result.ResultObject = _bll.SaveTemplate(pram);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //템플릿 삭제
        [HttpDelete("DelTempCue")]
        public DTO_RESULT<bool> DelTempCue([FromQuery] int[] tempids)
        {
            var result = new DTO_RESULT<bool>();
            try
            {
                foreach (var i in tempids)
                {
                    var files = _attachmentsBll.GetAttachmentDTOs(i);

                    foreach (var item in files)
                    {
                        _attachmentsBll.DeleteAttachmentsFile(item);
                    }
                }
                result.ResultObject = _bll.DeleteTemplate(tempids);
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
