using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using SmartSql;
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
        //[HttpGet("pgm?media={media}&brd_dt={brd_dt}")]
        [HttpGet("pgm")]
        public DTO_RESULT<DTO_RESULT_LIST> FindPGM([FromQuery] string media, [FromQuery] string brd_dt, [FromQuery] int rowPerPage, [FromQuery] int selectPage)
        {
            DTO_RESULT<DTO_RESULT_LIST> result = new DTO_RESULT<DTO_RESULT_LIST>();
            try
            {
                var startNO = rowPerPage * selectPage;
                //HybridDictionary param = new HybridDictionary();
                //param.Add("MEDIA", media);
                //param.Add("BRD_DT", brd_dt);
                //param.Add("MAX_ROW", startNO + rowPerPage);
                //param.Add("MIN_ROW", startNO);

                using (var dbsession = SqlMap.GetSession())
                {
                    var resultObj = dbsession.Query<DTO_PGM_INFO>(new RequestContext
                    {
                        Scope = "Products",
                        SqlId = "SELECT_PGM_01",
                        Request = new { MAX_ROW= startNO + rowPerPage, MIN_ROW=startNO }
                    });
                    JsonSerializerOptions jso = new JsonSerializerOptions();
                    jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                    var strReturnObj = JsonSerializer.Serialize(resultObj, jso);
                    DTO_RESULT_LIST list = new DTO_RESULT_LIST();
                    list.StrDataList = strReturnObj;
                    result.RESULT_OBJECT = list;
                }
                result.RESULT_CODE = RESUlT_CODES.SUCCESS;
            }
            catch (Exception ex)
            {
                result.ERROR_MSG = ex.Message;
                MyLogger.Error(LOG_CATEGORIES.UNKNOWN_EXCEPTION.ToString(), ex.Message);
            }
            return result;
        }
        //[HttpGet("spot/scr?media={media}&start_dt={start_dt}&end_dt={end_dt}&pd={pd}&name={name}")]
        public DTO_RESULT FindSCRSpot(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        //[HttpGet("report?cate={cate}&brd_dt={brd_dt}&pgm={pgm}&pd={pd}&name={name}")]
        public DTO_RESULT FindReport(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        //[HttpGet("old_pro?media={media}&type={type}&pd={pd}&name={name}")]
        public DTO_RESULT FindOldPro(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        //[HttpGet("music")]
        public DTO_RESULT FindMusic(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        //[HttpGet("effect?keyword={keyword}")]
        public DTO_RESULT FindEffect(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        //[HttpGet("sb/mcr?media={media}&brd_dt={brd_dt}&pgm={pgm}")]
        public DTO_RESULT FindMcrSB(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        //[HttpGet("sb/scr?media={media}&brd_dt={brd_dt}&pgm={pgm}")]
        public DTO_RESULT FindScrSB(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

        //[HttpGet("cm?media={media}&brd_dt={brd_dt}&adcd={adcd}&pgm={pgm}")]
        public DTO_RESULT FindCM(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        //[HttpGet("spot/mcr?media={media}&cate={cate}&start_dt={start_dt}&end_dt={end_dt}&status={status}&pd={pd}")]
        public DTO_RESULT FindMcrSpot(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        //[HttpGet("filler/pr?media={media}&cate={cate}&pd={pd}&name={name}")]
        public DTO_RESULT FindProFiller(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        //[HttpGet("filler/general")]
        public DTO_RESULT FindFeneralFiller(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        //[HttpGet("filler/time")]
        public DTO_RESULT FindTimetoneFiller(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        //[HttpGet("filler/etc")]
        public DTO_RESULT FindETCFiller(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        //[HttpGet("digitallibrary")]
        public DTO_RESULT FindNewDL(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
    }
}
