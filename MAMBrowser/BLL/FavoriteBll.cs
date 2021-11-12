using DAP3.CueSheetCommon.DTO.Param;
using DAP3.CueSheetCommon.DTO.Result;
using DAP3.CueSheetDAL.Factories.Web;
using Dapper;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MAMBrowser.BLL
{
    public class FavoriteBll
    {
        private readonly WebCueSheetFactory _factory;
        public FavoriteBll(WebCueSheetFactory factory)
        {
            _factory = factory;
        }

        // 즐겨찾기 가져오기 
        public List<UserFavDTO> GetUserFavorites(string personid)
        {
            return _factory.UserRepository.GetUserFavorites(personid).ToList();
        }

        // 즐겨찾기 저장
        public SaveResultDTO SaveUserFavorites(string personid, IEnumerable<FavConParamDTO> favConParam)
        {
            var result = _factory.UserRepository.SaveUserFavorites(personid, favConParam);
            return result;
        }

        // 즐겨찾기 삭제
        public bool DeleteUserFavorites(string personid)
        {
            return _factory.UserRepository.DeleteUserFavorites(personid);
        }
    }
}
