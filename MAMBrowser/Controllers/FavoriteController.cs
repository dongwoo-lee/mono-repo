using MAMBrowser.BLL;
using M30.AudioFile.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAMBrowser.DTO;

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
        public IEnumerable<CueSheetConDTO> GetFavorites([FromQuery] string personid)
        {
            try
            {
                return _bll.GetUserFavorites(personid);
            }
            catch
            {
                throw;
            }
        }
        //즐겨찾기 저장
        [HttpPost("SetFavorites")]
        public int SetFavorites([FromQuery] string personid, [FromBody] List<CueSheetConDTO> pram)
        {
            try
            {
                var result = _bll.SaveUserFavorites(personid, pram);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
