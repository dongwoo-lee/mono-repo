using DAP3.CueSheetCommon.DTO.Param;
using DAP3.CueSheetCommon.DTO.Result;
using Dapper;
using MAMBrowser.BLL;
using MAMBrowser.Common;
using MAMBrowser.Common.Foundation;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Entiies;
using MAMBrowser.Foundation;
using MAMBrowser.Helper;
using MAMBrowser.Helpers;
using MAMBrowser.Models;
using MAMBrowser.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DayCueSheetController : ControllerBase
    {
        private readonly DayCueSheetBll _bll;

        public DayCueSheetController(DayCueSheetBll bll)
        {
            Debug.WriteLine("DayCueSheetController");
            _bll = bll;
        }

        //시작일, 종료일 날짜
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
        // 일일큐시트 목록 가져오기 (날짜별)
        [HttpGet("GetDayCueList")]
        public IEnumerable<DayCueSheetListDTO> GetDayCueList([FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string[] products)
        {
            try
            {
                Debug.WriteLine("GetDayCueList");
                List<string> cal = setDateList(start_dt, end_dt);
                string[] dates = cal.ToArray();
                var result =  _bll.GetDayCueList(products, dates);
                return result;
            }
            catch
            {
                throw;
            }
        //일일큐시트 상세내용 가져오기
        }
        [HttpGet("GetDayCue")]
        public CueSheetCollectionDTO GetDayCue([FromQuery] string productid, [FromQuery] int cueid)
        {
            try
            {
                Debug.WriteLine("GetDayCue");
                var result = _bll.GetDayCue(productid, cueid);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //일일큐시트 생성 & 업데이트
        [HttpPost("SaveDayCue")]
        public SaveResultDTO SaveDayCue([FromBody] CueData pram)
        {
            try
            {
                Debug.WriteLine("SaveDayCue");
                var result = _bll.SaveDayCue(pram.cueParam, pram.dayParam, pram.conParams, pram.tagParams, pram.printParams, pram.attParams);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        //일일큐시트 삭제
        //[HttpDelete("DelDayCue")]
        //public bool DelDayCue([FromQuery] int cueid)
        //{
        //    try
        //    {
        //        return _bll.DelDayCue(cueid);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
    }
}