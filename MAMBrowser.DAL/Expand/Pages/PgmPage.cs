using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;
using System;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class PgmPage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param)
        {
            PgmMenuDTO menuDTO = new PgmMenuDTO();
            menuDTO.Medias = DalFactory.CategoriesDao.GetMedia();
            menuDTO.PgmCodes = DalFactory.CategoriesDao.GetPgmCodes(
                (param == null) ? DateTime.Today.ToString("yyyyMMdd") : 
                (string.IsNullOrEmpty(param.BrdDate)) ? DateTime.Today.ToString("yyyyMMdd") : param.BrdDate,
                (param == null) ? "A" :
                (string.IsNullOrEmpty(param.Media)) ? "A" : param.Media);
            menuDTO.PDUserList = DalFactory.CategoriesDao.GetPDUserList();

            return menuDTO;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            throw new NotImplementedException();
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            PgmSearchOptionDTO option = param as PgmSearchOptionDTO;
            PgmResultDTO result = new PgmResultDTO();
            result.Result = DalFactory.ProductsDao.FindPGM(option.Media, option.BrdDate, option.PgmName, option.EditorName,
                option.RowPerPage, option.SelectPage, option.SortKey, option.SortValue);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}
