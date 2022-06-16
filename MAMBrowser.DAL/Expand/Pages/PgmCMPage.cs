using System;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class PgmCMPage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param)
        {
            PgmCMMenuDTO menuDTO = new PgmCMMenuDTO();
            menuDTO.Medias = DalFactory.CategoriesDao.GetMedia();

            return menuDTO;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            throw new NotImplementedException();
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            PgmCMSearchOptionDTO option = param as PgmCMSearchOptionDTO;
            PgmCMResultDTO result = new PgmCMResultDTO();
            result.Result = DalFactory.ProductsDao.FindCM(option.Media, option.BrdDate, "P", option.PgmName);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}
