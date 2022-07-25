using MAMBrowser.BLL;
using M30.AudioFile.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAMBrowser.DTO;
using M30.AudioFile.Common;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteController : Controller
    {
        private readonly FavoriteBll _bll;

        public FavoriteController(FavoriteBll bll)
        {
            _bll = bll;
        }

        //즐겨찾기 가져오기
        [HttpGet("GetFavorites")]
        public DTO_RESULT<IEnumerable<CueSheetConDTO>> GetFavorites([FromQuery] string personid, string pgmcode, string brd_dt)
        {
            var result = new DTO_RESULT<IEnumerable<CueSheetConDTO>>();
            try
            {
                result.ResultObject =  _bll.GetUserFavorites(personid, pgmcode, brd_dt);
                result.ResultCode = RESUlT_CODES.SUCCESS;
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.ResultCode = RESUlT_CODES.SERVICE_ERROR;
            }
            return result;
        }

        //즐겨찾기 저장
        [HttpPost("SetFavorites")]
        public DTO_RESULT<int> SetFavorites([FromQuery] string personid, [FromBody] List<CueSheetConDTO> pram)
        {
            var result = new DTO_RESULT<int>();
            try
            {
                result.ResultObject = _bll.SaveUserFavorites(personid, pram);
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
