using System;
using MAMBrowser.Common.Expand.Menus;
using MAMBrowser.Common.Expand.Result;
using MAMBrowser.Common.Expand.SearchOptions;

namespace MAMBrowser.DAL.Expand.Pages
{
    internal class CMPage : Page
    {
        public override MenuDTO GetMenu(MenuParamDTO param)
        {
            CMMenuDTO menuDTO = new CMMenuDTO();
            menuDTO.Medias = DalFactory.CategoriesDao.GetMedia();
            menuDTO.CM = DalFactory.CategoriesDao.GetCM();

            return menuDTO;
        }

        public override MenuDTO GetSubMenu(MenuParamDTO param = null)
        {
            throw new NotImplementedException();
        }

        public override T Search<T>(SearchOptionDTO param)
        {
            CMSearchOptionDTO option = param as CMSearchOptionDTO;
            CMResultDTO result = new CMResultDTO();
            result.Result = DalFactory.ProductsDao.FindCM(option.Media, option.BrdDate, option.Cate, option.PgmName);
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}
