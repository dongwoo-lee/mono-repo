using MAMBrowser.BLL;
using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using M30.AudioFile.Common;
using Oracle.ManagedDataAccess.Client;
using M30.AudioFile.Common.DTO;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DayCueSheetController : ControllerBase
    {
        private readonly DayCueSheetBll _bll;
        private readonly CueAttachmentsBll _attachmentsBll;

        public DayCueSheetController(DayCueSheetBll bll, CueAttachmentsBll attachmentsBll)
        {
            _bll = bll;
            _attachmentsBll = attachmentsBll;
        }

        public class DayPram
        {
            public List<string> products { get; set; }
            public int row_per_page { get; set; }
            public int select_page { get; set; }
            public string brd_dt { get; set; }
            public string media { get; set; }
        }

        //일일큐시트 목록 가져오기
        [HttpPost("GetDayCueList")]
        public DTO_RESULT<DayCueList_Page> GetDayCueList([FromBody] DayPram pram)
        {
            DTO_RESULT<DayCueList_Page> result = new DTO_RESULT<DayCueList_Page>();
            try
            {
                List<string> dates = new List<string>();
                dates.Add(pram.brd_dt);
                result.ResultObject = _bll.GetDayCueSheetList(pram.products, dates, pram.row_per_page, pram.select_page, pram.media);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //일일큐시트 상세내용 가져오기
        [HttpGet("GetDayCue")]
        public DTO_RESULT<CueSheetCollectionDTO> GetDayCue([FromQuery] string productid, string pgmcode, string brd_dt)
        {
            DTO_RESULT<CueSheetCollectionDTO> result = new DTO_RESULT<CueSheetCollectionDTO>();
            try
            {
                result.ResultObject = _bll.GetDayCueSheet(productid, pgmcode, brd_dt);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //광고 가져오기
        [HttpGet("GetAddSponsor")]
        public DTO_RESULT<List<CueSheetConDTO>> GetAddSponsor(string pgmcode, string brd_dt)
        {
            DTO_RESULT<List<CueSheetConDTO>> result = new DTO_RESULT<List<CueSheetConDTO>>();
            var toDate = DateTime.Today;
            try
            {
                if (brd_dt == null)
                {
                    brd_dt = toDate.ToString("yyyyMMdd");
                }
                result.ResultObject = _bll.GetAddSponsorList(pgmcode, brd_dt);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //일일큐시트 생성 & 업데이트
        [HttpPost("SaveDayCue")]
        public DTO_RESULT<int> SaveDayCue([FromBody] CueSheetCollectionDTO pram)
        {
            var result = new DTO_RESULT<int>();
            var files = new List<AttachmentDTO>();
            try
            {
                foreach (var item in pram.Attachments)
                {
                    if (item.FILEID != 0)
                    {
                        //기존 데이터 삭제
                        if (item.DELSTATE)
                        {
                            _attachmentsBll.DeleteAttachmentsFile(item);
                        }
                        else
                        {
                            files.Add(item);
                        }
                    }
                    else
                    {
                        item.FILEID = _attachmentsBll.GetAttachmentsFileId();
                        _attachmentsBll.MoveToStorage(item,false);
                        files.Add(item);
                    }
                }
                pram.Attachments = files;
                result.ResultObject = _bll.SaveDayCue(pram);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //구 DAP 저장
        [HttpPost("SaveOldCue")]
        public int SaveOldCue([FromBody] CueSheetCollectionDTO pram)
        {
            try
            {
                var saveResult = _bll.SaveOldCueSheet(pram);
                if (saveResult)
                    return 1;
                else
                    return -1;
            }
            catch (OracleException oe)
            {

                switch (oe.Number)
                {
                    case 20011:
                        return 0;
                    default:
                        throw;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
