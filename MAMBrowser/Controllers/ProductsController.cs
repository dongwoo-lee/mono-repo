using MAMBrowser.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet("pgm?media={media}&brd_dt={brd_dt}")]
        public DTO_RESULT FindPGM(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("spot/scr?media={media}&start_dt={start_dt}&end_dt={end_dt}&pd={pd}&name={name}")]
        public DTO_RESULT FindSCRSpot(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("report?cate={cate}&brd_dt={brd_dt}&pgm={pgm}&pd={pd}&name={name}")]
        public DTO_RESULT FindReport(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("old_pro?media={media}&type={type}&pd={pd}&name={name}")]
        public DTO_RESULT FindOldPro(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("music")]
        public DTO_RESULT FindMusic(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("effect?keyword={keyword}")]
        public DTO_RESULT FindEffect(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("sb/mcr?media={media}&brd_dt={brd_dt}&pgm={pgm}")]
        public DTO_RESULT FindMcrSB(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("sb/scr?media={media}&brd_dt={brd_dt}&pgm={pgm}")]
        public DTO_RESULT FindScrSB(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }

        [HttpGet("cm?media={media}&brd_dt={brd_dt}&adcd={adcd}&pgm={pgm}")]
        public DTO_RESULT FindCM(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("spot/mcr?media={media}&cate={cate}&start_dt={start_dt}&end_dt={end_dt}&status={status}&pd={pd}")]
        public DTO_RESULT FindMcrSpot(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("filler/pr?media={media}&cate={cate}&pd={pd}&name={name}")]
        public DTO_RESULT FindProFiller(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("filler/general")]
        public DTO_RESULT FindFeneralFiller(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("filler/time")]
        public DTO_RESULT FindTimetoneFiller(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("filler/etc")]
        public DTO_RESULT FindETCFiller(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
        [HttpGet("digitallibrary")]
        public DTO_RESULT FindNewDL(string filename, string title, string memo)
        {
            DTO_RESULT result = new DTO_RESULT();
            return result;
        }
    }
}
