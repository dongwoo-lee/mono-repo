using M30_CueSheetDAO;
using M30_CueSheetDAO.DAO;
using M30_CueSheetDAO.Entity;
using M30_CueSheetDAO.Interfaces;
using M30_CueSheetDAO.ParamEntity;
using MAMBrowser.DTO;
using MAMBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.BLL
{
    public class FavoriteBll
    {
        private readonly IFavoritesDAO _dao;

        public FavoriteBll(IFavoritesDAO dao)
        {
            _dao = dao;
        }
        // 즐겨찾기 가져오기 
        public IEnumerable<CueSheetConDTO> GetUserFavorites(string personid)
        {
            return _dao.GetUserFavorites(personid)?.Converting();
        }

        // 즐겨찾기 저장
        public int SaveUserFavorites(string personid, List<CueSheetConDTO> pram)
        {
            var paramData = pram.FavToEntity();
            List<UserFavCreateParam> param = new List<UserFavCreateParam>();
            foreach (var item in paramData)
            {
                var paramItem = new UserFavCreateParam();
                paramItem = item;
                paramItem.p_personid = personid;
                param.Add(paramItem);
            }

            return _dao.CreateFavorites(param);
        }
    }
}
