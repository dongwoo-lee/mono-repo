using DAP3.CueSheetCommon.DTO.Param;
using DAP3.CueSheetCommon.DTO.Result;
using MAMBrowser.BLL;
using MAMBrowser.Entiies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MAMBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteController : ControllerBase
    {
        private readonly FavoriteBll _bll;

        public FavoriteController(FavoriteBll bll)
        {
            _bll = bll;
        }
        //즐겨찾기 가져오기
        [HttpGet("GetFavorites")]
        public List<UserFavDTO> GetFavorites([FromQuery] string personid)
        {
            try
            {
                return _bll.GetUserFavorites(personid).ToList();
            }
            catch
            {
                throw;
            }
        }
        //즐겨찾기 저장 (테스트 필요)
        [HttpPost("SetFavorites")]
        public SaveResultDTO SetFavorites([FromQuery] string personid, [FromBody] CueData pram)
        {
            try
            {
               var result = _bll.SaveUserFavorites(personid, pram.favConParam);
                return result;
            }
            catch
            {
                throw;
            }
        }
        //즐겨찾기 삭제
        [HttpDelete("DelFavorites")]
        public bool DelFavorites([FromBody] string personid)
        {
            try
            {
                return _bll.DeleteUserFavorites(personid);
            }
            catch
            {
                throw;
            }
        }
    }
}
