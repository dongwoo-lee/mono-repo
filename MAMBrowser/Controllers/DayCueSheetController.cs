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
using Oracle.ManagedDataAccess.Client;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DayCueSheetController : ControllerBase
    {
        private readonly DayCueSheetBll _bll;

        public DayCueSheetController(DayCueSheetBll bll)
        {
            _bll = bll;
        }
        
        public class DayPram
        {
            public List<string> products { get; set; }
            public int row_per_page { get; set; }
            public int select_page { get; set; }
            public string start_dt { get; set; }
            public string end_dt { get; set; }
            public string media { get; set; }

        }
        
        //시작일, 종료일 날짜
        [HttpGet("setDateList")] //Swagger 오류 수정
        public List<string> setDateList(string start_dt, string end_dt)
        {
            List<string> dateList = new List<string>();
            DateTime startDate = DateTime.ParseExact(start_dt, "yyyyMMdd", null);
            DateTime endDate = DateTime.ParseExact(end_dt, "yyyyMMdd", null);
            int dateSum = (endDate - startDate).Days;
            for (int i = 0; i <= dateSum; i++)
            {
                string result = startDate.AddDays(i).ToString("yyyyMMdd");
                dateList.Add(result);
            }
            return dateList;
        }

        //일일큐시트 목록 가져오기
        [HttpPost("GetDayCueList")]
        public DayCueList_Result GetDayCueList([FromBody] DayPram pram)
        {
            try
            {
                DayCueList_Result result = new DayCueList_Result();
                result.ResultObject = new DayCueList_Page();
                List<string> dates = setDateList(pram.start_dt, pram.end_dt);            
                //media 추가
                result.ResultObject = _bll.GetDayCueSheetList(pram.products, dates, pram.row_per_page, pram.select_page,pram.media);
                result.ResultCode = RESUlT_CODES.SUCCESS;
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //일일큐시트 상세내용 가져오기
        [HttpGet("GetDayCue")]
        public CueSheetCollectionDTO GetDayCue([FromQuery] string productid, string pgmcode, string brd_dt)
        {
            try
            {
                return _bll.GetDayCueSheet(productid, pgmcode, brd_dt);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        
        [HttpGet("GetAddSponsor")]
        public List<CueSheetConDTO> GetAddSponsor(string pgmcode, string brd_dt)
        {
            var toDate = DateTime.Today;
            if (brd_dt == null)
            {
                brd_dt = toDate.ToString("yyyyMMdd");
            }
            try
            {
                return _bll.GetAddSponsorList(pgmcode, brd_dt);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        //일일큐시트 생성 & 업데이트
        [HttpPost("SaveDayCue")]
        public int SaveDayCue([FromBody] CueSheetCollectionDTO pram)
        {
            try
            {
                var result = _bll.SaveDayCue(pram);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

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
            catch(OracleException oe)
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
