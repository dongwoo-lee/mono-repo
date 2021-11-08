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
                var result = _bll.GetDayCueList(products, dates);
                return result;
            }
            catch
            {
                throw;
            }
        }
        //일일큐시트 상세내용 가져오기
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
        //일일큐시트 상세내용 가져오기 - DTO
        [HttpGet("GetDayCueDTO")]
        public ViewCueSheetCollection GetDayCueDTO([FromQuery] string productid, [FromQuery] int cueid)
        {
            var result = new ViewCueSheetCollection();
            var abConResult = new List<ViewCueSheetConDTO>();
            var cCon = new List<ViewCueSheetConDTO>();
            int rowNum_ab = 1;
            try
            {
                Debug.WriteLine("GetDayCue");
                var conData = _bll.GetDayCue(productid, cueid);

                foreach (var item in conData.CueSheetCons)
                {
                    var con = new ViewCueSheetConDTO();
                    con.CartCode = item.CARTCODE ?? "";
                    con.ChannelType = item.CHANNELTYPE ?? "";
                    con.CartId = item.CARTID ?? "";
                    con.OnairDate = item.ONAIRDATE ?? "";
                    con.Duration = item.ENDPOSITION;
                    con.StartPosition = item.STARTPOSITION;
                    con.EndPosition = item.ENDPOSITION;
                    con.FadeInTime = item.FADEINTIME;
                    con.FadeOutTime = item.FADEOUTTIME;
                    con.MainTitle = item.MAINTITLE ?? "";
                    con.SubTitle = item.SUBTITLE ?? "";
                    con.Memo = item.MEMO ?? "";
                    con.TransType = item.TRANSTYPE;
                    con.UseFlag = item.USEFLAG ?? "Y";
                    con.Editting = true;

                    if (item.CONS.Count != 0)
                    {
                        List<string> filepath = new List<string>();
                        foreach (var con_i in item.CONS)
                        {
                            filepath.Add(con_i.P_MASTERFILE);
                        }
                        con.FilePath_test = filepath;
                        con.FilePath = item.CONS[0].P_MASTERFILE;
                        List<string> filetoken = new List<string>();
                        foreach (var path in con.FilePath_test)
                        {
                            var token = TokenGenerator.GenerateMusicToken(path);
                            filetoken.Add(token);
                        }
                        con.FileToken = filetoken;
                    }
                    if (item.CHANNELTYPE == "N")
                    {
                        con.RowNum = rowNum_ab++;
                        abConResult.Add(con);
                    }
                    if (item.CHANNELTYPE == "I")
                    {
                        con.RowNum = item.SEQNUM;
                        cCon.Add(con);
                    }

                }
                 result.abCartCon = abConResult;
                var cConResult = new List<List<ViewCueSheetConDTO>>();
                 for (var channelNum=0; 4 > channelNum; channelNum++)
                {
                    var cartData = new List<ViewCueSheetConDTO>();
                    for(var i=0; 16 > i; i++)
                    {
                        var row = new ViewCueSheetConDTO();
                        for (var itemIndex = 0; cCon.Count > itemIndex; itemIndex++)
                        {
                            if(cCon[itemIndex].RowNum == i + 16 * channelNum + 1)
                            {
                                row = cCon[itemIndex];
                                break;
                            }
                        }
                        cartData.Add(row);
                    }
                  cConResult.Add(cartData);
                }
                result.cCartCon = cConResult;
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
            catch (Exception ex)
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