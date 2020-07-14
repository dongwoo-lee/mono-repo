using Dapper;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
     

        /// <summary>
        /// 방송일으로 부터 프로그램 소재목록 가져오기.(시간순정렬되며, 임의로 정렬불가)
        /// </summary>
        /// <param name="media"></param>
        /// <param name="brd_dt"></param>
        /// <returns></returns>
        [HttpGet("pgm")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_PGM_INFO>> FindPGM([FromQuery] string media, [FromQuery] string brd_dt)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_PGM_INFO>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_PGM_INFO>>();
            try
            {
                string connectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=dev.adsoft.kr)(PORT=1523)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));User Id=MIROS;Password=MIROS;";
                using (OracleConnection con = new OracleConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT MEDIANAME, EVENTNAME, ONAIRDATE, ONAIRTIME, STATENAME, MILLISEC, EDITORNAME, EDITTIME, REQTIME, MASTERFILE" +
                        " FROM" +
                        " MEM_PROGRAM_VIEW" +
                        " WHERE" +
                        " MEDIA = :MEDIA" +
                        " AND ONAIRDATE = :ONAIRDATE";

                    var queryData = con.Query(query, new { MEDIA = media, ONAIRDATE = brd_dt }).
                        Select((row,b)=> new DTO_PGM_INFO {
                            MediaName = row.MEDIANAME,
                            Name = row.EVENTNAME,
                            BrdDT = row.ONAIRDATE,
                            BrdTime = row.ONAIRTIME,
                            Status = row.STATENAME,
                            Duration = row.MILLISEC,
                            UserName = row.EDITORNAME,
                            EditDtm = row.EDITTIME,
                            CompleteDtm = row.REQTIME,
                            FilePath = row.MASTERFILE
                        });

                    var datalist = queryData.ToList();
                    var firstObj = datalist.FirstOrDefault();
                    if (firstObj != null)
                    {
                        result.ResultObject.TotalRowCount = firstObj.Count;
                        result.ResultObject.RowPerPage = 200;
                        result.ResultObject.SelectPage = 1;
                    }
                    result.ResultObject.DataList = datalist;
                }
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpGet("spot/scr")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_SCR_SPOT>> FindSCRSpot([FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string pd, [FromQuery] string name, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_SCR_SPOT>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_SCR_SPOT>>();
            try
            {
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpGet("report")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_REPORT>> FindReport([FromQuery] string cate, [FromQuery] string brd_dt, [FromQuery] string pgm, [FromQuery] string pd, [FromQuery] string reporter, [FromQuery] string name)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_REPORT>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_REPORT>>();
            try
            {
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpGet("old_pro")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_PRO>> FindOldPro([FromQuery] string media, [FromQuery] string type, [FromQuery] string pd, [FromQuery] string name)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_PRO>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_PRO>>();
            try
            {
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpGet("music")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_SONG>> FindMusic([FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_SONG>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_SONG>>();
            try
            {
                var startNO = 1;
                startNO += (rowPerPage * selectPage) - rowPerPage;
                var lastNO = startNO + rowPerPage;

                //var resultObj = "";
                //var obj = resultObj.FirstOrDefault();
                //if (obj != null)
                //{
                //    result.ResultObject.TotalRowCount = obj.Count;
                //    result.ResultObject.RowPerPage = rowPerPage;
                //    result.ResultObject.SelectPage = selectPage;
                //    //result.ResultObject
                //}

                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpGet("effect")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_EFFECT>> FindEffect([FromQuery] string searchWord, [FromQuery] int rowPerPage, [FromQuery] int selectPage, [FromQuery] string sortKey, [FromQuery] string sortValue)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_EFFECT>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_EFFECT>>();
            try
            {
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpGet("sb/mcr")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_SB>> FindMcrSB([FromQuery] string media, [FromQuery] string brd_dt, [FromQuery] string pgm)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_SB>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_SB>>();
            try
            {
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpGet("sb/scr")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_SB>> FindScrSB([FromQuery] string media, [FromQuery] string brd_dt, [FromQuery] string pgm)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_SB>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_SB>>();
            try
            {
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }

        [HttpGet("cm")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_CM>> FindCM([FromQuery] string media, [FromQuery] string brd_dt, [FromQuery] string adcd, [FromQuery] string pgm)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_CM>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_CM>>();
            try
            {
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpGet("spot/mcr")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_MCR_SPOT>> FindMcrSpot([FromQuery] string media, [FromQuery] string cate, [FromQuery] string end_dt, [FromQuery] string status, [FromQuery] string pd)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_MCR_SPOT>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_MCR_SPOT>>();
            try
            {
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpGet("filler/pr")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> FindProFiller([FromQuery] string media, [FromQuery] string cate, [FromQuery] string pd, [FromQuery] string name)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>>();
            try
            {
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpGet("filler/general")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> FindFeneralFiller([FromQuery] string media, [FromQuery] string cate, [FromQuery] string pd, [FromQuery] string name)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>>();
            try
            {
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpGet("filler/time")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> FindTimetoneFiller([FromQuery] string media, [FromQuery] string start_dt, [FromQuery] string end_dt, [FromQuery] string status, [FromQuery] string cate, [FromQuery] string pd, [FromQuery] string name)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>>();
            try
            {
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpGet("filler/etc")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> FindETCFiller([FromQuery] string media, [FromQuery] string cate, [FromQuery] string pd, [FromQuery] string name)
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_PUBLIC_FILE>>();
            try
            {
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        [HttpGet("dl30")]
        public DTO_RESULT<DTO_RESULT_LIST<DTO_DL30>> FindNewDL([FromQuery] string media, [FromQuery] string cate, [FromQuery] string reg_dt )
        {
            DTO_RESULT<DTO_RESULT_LIST<DTO_DL30>> result = new DTO_RESULT<DTO_RESULT_LIST<DTO_DL30>>();
            try
            {
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
    }
}
