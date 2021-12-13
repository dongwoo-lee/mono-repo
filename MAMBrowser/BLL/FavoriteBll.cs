using M30_CueSheetDAO;
using M30_CueSheetDAO.DAO;
using M30_CueSheetDAO.Entity;
using M30_CueSheetDAO.Interfaces;
using M30_CueSheetDAO.ParamEntity;
using M30.AudioFile.Common.DTO;
using MAMBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAMBrowser.DTO;

namespace MAMBrowser.BLL
{
    public class FavoriteBll
    {
        private readonly IFavoritesDAO _dao;
        private readonly ICommonDAO _common_dao;

        public FavoriteBll(IFavoritesDAO dao, ICommonDAO common_dao)
        {
            _dao = dao;
            _common_dao = common_dao;
        }
        // 즐겨찾기 가져오기 
        public IEnumerable<CueSheetConDTO> GetUserFavorites(string personid, string pgmcode, string brd_dt)
        {
            var result =  _dao.GetUserFavorites(personid);
            var toDate = DateTime.Today;
            if (brd_dt == null|| brd_dt == "undefined")
            {
                brd_dt = toDate.ToString("yyyyMMdd");
            }
            if (pgmcode != null && brd_dt != null)
            {
                SponsorParam spon_param = new SponsorParam();
                spon_param.BrdDate = brd_dt;
                spon_param.PgmCode = pgmcode;
                result = _common_dao.GetSponsor(spon_param).SetSponsorToEntity(result);
            }

            return result?.Converting();
        }

        // 즐겨찾기 저장
        public int SaveUserFavorites(string personid, List<CueSheetConDTO> pram)
        {
            var paramData = pram.FavToEntity();
            List<UserFavCreateParam> param = new List<UserFavCreateParam>();
            if (paramData.Any())
            {
                foreach (var item in paramData)
                {
                    var paramItem = new UserFavCreateParam();
                    paramItem = item;
                    paramItem.p_personid = personid;
                    param.Add(paramItem);
                }

            }
            return _dao.CreateFavorites(personid, param);
            //0개면 에러나는데 이거 한번 봐야함
        }
    }
}
